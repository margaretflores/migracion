using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using IBM.Data.DB2.iSeries;
using System.Configuration;

using AccesoDatos;
using appWcfService;
using appConstantes;


namespace appLogica
{
    public class appDB2
    {
        internal BaseDatos DB2;

        private bool OcultaErrorReal;
        private string Aplicacion;
        private string DataSourceDB2;

        public appDB2()
        {
            string stringConnection, userId, userPassword;
            DataSourceDB2 = ConfigurationManager.AppSettings["DataSource"];
            Aplicacion = ConfigurationManager.AppSettings["Aplicacion"];

            userId = ConfigurationManager.AppSettings["UserId"]; //"PCVTAS"; // "PCS400";
            userPassword = ConfigurationManager.AppSettings["UserPassword"]; //"PCVTAS"; // "pcs400";
            //stringConnection = "DataSource=" + DataSourceDB2 + ";User Id=" + userId + ";Password=" + userPassword + ";Naming=System;LibraryList=QS36F,PRODDAT,PRODPRG,costdat,INCAOBJ;CheckConnectionOnOpen=true;";
            stringConnection = "DataSource=" + DataSourceDB2 + ";User Id=" + userId + ";Password=" + userPassword + ";Naming=SQL;CheckConnectionOnOpen=true;";
            DB2 = new BaseDatos(stringConnection);

            PEDIDOSEntitiesDB2.CadenaConexion = stringConnection;

            if (ConfigurationManager.AppSettings["OcultaErrorReal"].Equals("1"))
            {
                OcultaErrorReal = true;
            }
            else
            {
                OcultaErrorReal = false;
            }
        }
        public void Finaliza()
        {
            DB2 = null;
        }

        public RESOPE obtieneDatosPartida(string articulo, string partida, decimal idalmacen)
        {
            DataTable otrosDataTable;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            try
            {
                string codigoColor;
                StringBuilder comandoSql;

                articulo = articulo.PadRight(15);
                vpar.VALSAL = new List<string>();
                DB2.Conectar();

                //stock
                comandoSql = new StringBuilder();
                comandoSql.Append("SELECT ");
                comandoSql.Append(" LOTSTOCK ");
                comandoSql.Append(" FROM QS36F.\"I1.DD41A\" "); //PRODDAT
                comandoSql.Append(" WHERE LOTCIA = ").Append(Constantes.CODIGO_CIA).Append(" AND LOTITEM = '").Append(articulo.Trim()).Append("' ");
                comandoSql.Append(" AND LOTPARTI = '").Append(partida.Trim()).Append("' AND LOTALM = ").Append(idalmacen);
                DB2.CrearComando(comandoSql.ToString(), CommandType.Text);
                otrosDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
                if (!Util.TablaVacia(otrosDataTable))
                {
                    DataRow cr = otrosDataTable.Rows[0];
                    decimal stock = Convert.ToDecimal(cr["LOTSTOCK"]) * 100;
                    vpar.VALSAL.Add(stock.ToString("###0")); // Constantes.FORMATO_DECIMAL_0 + "0").Trim());
                }
                else
                {
                    vpar.VALSAL.Add("");
                }

                //COLOR
                codigoColor = articulo.Substring(9).Trim();
                if (string.IsNullOrEmpty(codigoColor))
                {
                    codigoColor = articulo.Substring(5, 4).Trim();
                }
                comandoSql = new StringBuilder();
                comandoSql.Append("SELECT ");
                comandoSql.Append(" DC.COINCOCO, DC.COINTICO, DC.COINGRCO, GC.TGRCDESC ");
                comandoSql.Append(" FROM PRODDAT.PRCOIN DC LEFT JOIN PRODDAT.PRTGRC GC ON DC.COINCIA = GC.TGRCCIA AND DC.COINGRCO = GC.TGRCCODI "); //QS36F PRODDAT  //QS36F.PRCOIN
                comandoSql.Append(" WHERE DC.COINCIA = ").Append(Constantes.CODIGO_CIA).Append(" AND DC.COINCOCO = '").Append(codigoColor).Append("' ");
                DB2.CrearComando(comandoSql.ToString(), CommandType.Text);
                otrosDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
                if (!Util.TablaVacia(otrosDataTable))
                {
                    DataRow cr = otrosDataTable.Rows[0];

                    vpar.VALSAL.Add(Convert.ToString(cr["COINTICO"]).Trim());
                    if (cr["COINTICO"] != DBNull.Value)
                    {
                        vpar.VALSAL.Add(Convert.ToString(cr["TGRCDESC"]).Trim());
                    }
                    else
                    {
                        vpar.VALSAL.Add("");
                    }
                }
                else
                {
                    vpar.VALSAL.Add("");
                    vpar.VALSAL.Add("");
                }

                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
            return vpar;
        }

        public RFEUSER ObtieneUsuarioDeFacturacion(string codusu)
        {
            List<RFEUSER> lista;
            RFEUSER usuario = null;
            try
            {
                DataTable cabeceraDataTable = null;
                DB2.Conectar();

                string comandoSql = "FAELDAT.USP_PED_OBTIENE_USUARIO";

                DB2.CrearComando(comandoSql, CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@PUSUARIO", iDB2DbType.iDB2Char, codusu);
                cabeceraDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
                lista = Util.ParseDataTable<RFEUSER>(cabeceraDataTable);
                if (lista.Count > 0)
                {
                    usuario = lista[0];
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
            return usuario;
        }

        public void descargaOSA(bool conectado, string NoFolio, int Secuencia, DateTime fechaOsa, string Articulo, string Partida, decimal Almacen, string Destino, string centrocosto, string signo, decimal Cantidad)
        {
            try
            {

                if (!conectado)
                {
                    DB2.Conectar();
                }
                StringBuilder descarinvenSql; // = new StringBuilder();
                string cia, pref, serie, numero, fecha, articulo, almacen, partida, cantidad, importe, origen, destino, unimed, secu, tipacc;
                cia = Constantes.CODIGO_CIA.ToString().PadLeft(2, '0');
                //5 TINTO T302
                //89 HILADOS H302
                //W MARKET K302 
                //Z LABORA H312
                pref = NoFolio.Substring(0, 1);
                if (pref == "5")
                {
                    serie = "T302";
                }
                else if (pref == "8" || pref == "9")
                {
                    serie = "H302";
                }
                else if (pref == "W")
                {
                    serie = "H400"; // "K302";
                }
                else if (pref == "Z")
                {
                    serie = "H312";
                }
                else
                {
                    throw new Exception("Folio no válido");
                }
                numero = NoFolio.PadLeft(6, '0');
                fecha = fechaOsa.ToString(Constantes.FORMATO_FECHA_INVENTARIO);
                articulo = Articulo.PadRight(15, ' ');
                almacen = Convert.ToInt32(Almacen).ToString().Trim().PadLeft(3, '0');
                partida = Partida.PadRight(6, ' ');
                cantidad = Util.DecCad(Cantidad, 2, 13);
                importe = Util.DecCad(0, 2, 13);
                //signo = "+"; // + es descarga normal, - es extorno
                origen = centrocosto.Trim().PadLeft(6, '0');
                //VERIFICAR SI CODIGO_DOCUMENTO_INGRESO_PROD_DIREC_CLASIFICADO EL DESTINO ESTA EN PADE, SE ACABA DE AGREGAR 20160526
                destino = Destino.PadRight(6, ' '); //partida destino de osa
                unimed = Constantes.UNIDAD_KG;
                secu = Secuencia.ToString().Trim().PadLeft(3, '0');
                tipacc = Constantes.TIPO_ACCION_OSA;
                if (pref == "W")
                {
                    tipacc = Constantes.TIPO_ACCION_OSA_TRANSFERENCIA_SAL;
                }
                else if (pref == "Z")
                {
                    tipacc = Constantes.TIPO_ACCION_OSA_CONSUMO_LAB;
                }

                descarinvenSql = new StringBuilder();
                descarinvenSql.Append("CALL GEMAPRG.GMA003PP(");  //GMA003PP 20151130 PRUEBAS USUARIO
                descarinvenSql.Append("'").Append(cia).Append("', ");
                descarinvenSql.Append("'").Append(serie).Append("', ");
                descarinvenSql.Append("'").Append(numero).Append("', ");
                descarinvenSql.Append("'").Append(fecha).Append("', ");
                descarinvenSql.Append("'").Append(articulo).Append("', ");
                descarinvenSql.Append("'").Append(almacen).Append("', ");
                descarinvenSql.Append("'").Append(partida).Append("', ");
                descarinvenSql.Append("'").Append(cantidad).Append("', ");
                descarinvenSql.Append("'").Append(importe).Append("', ");
                descarinvenSql.Append("'").Append(signo).Append("', ");
                descarinvenSql.Append("'").Append(origen).Append("', ");
                descarinvenSql.Append("'").Append(destino).Append("', ");
                descarinvenSql.Append("'").Append(unimed).Append("', ");
                descarinvenSql.Append("'").Append(secu).Append("', ");
                descarinvenSql.Append("'").Append(tipacc).Append("') ");

                DB2.CrearComando(descarinvenSql.ToString(), CommandType.Text);
                DB2.EjecutarComando();
                //POR SI LO SOLICITAN 20180427
                if (pref == "W")
                {
                    //hacer el ingreso
                    serie = "K400";
                    almacen = Convert.ToInt32(Constantes.ALMACEN_MARKETING).ToString().Trim().PadLeft(3, '0');
                    if (signo == "+")
                    {
                        signo = "-";
                    }
                    else
                    {
                        signo = "+";
                    }
                    destino = "".PadRight(6, ' '); //partida destino de osa
                    tipacc = Constantes.TIPO_ACCION_OSA_TRANSFERENCIA_ING;

                    descarinvenSql = new StringBuilder();
                    descarinvenSql.Append("CALL GEMAPRG.GMA003PP(");  //GMA003PP 20151130 PRUEBAS USUARIO
                    descarinvenSql.Append("'").Append(cia).Append("', ");
                    descarinvenSql.Append("'").Append(serie).Append("', ");
                    descarinvenSql.Append("'").Append(numero).Append("', ");
                    descarinvenSql.Append("'").Append(fecha).Append("', ");
                    descarinvenSql.Append("'").Append(articulo).Append("', ");
                    descarinvenSql.Append("'").Append(almacen).Append("', ");
                    descarinvenSql.Append("'").Append(partida).Append("', ");
                    descarinvenSql.Append("'").Append(cantidad).Append("', ");
                    descarinvenSql.Append("'").Append(importe).Append("', ");
                    descarinvenSql.Append("'").Append(signo).Append("', ");
                    descarinvenSql.Append("'").Append(origen).Append("', ");
                    descarinvenSql.Append("'").Append(destino).Append("', ");
                    descarinvenSql.Append("'").Append(unimed).Append("', ");
                    descarinvenSql.Append("'").Append(secu).Append("', ");
                    descarinvenSql.Append("'").Append(tipacc).Append("') ");

                    DB2.CrearComando(descarinvenSql.ToString(), CommandType.Text);
                    DB2.EjecutarComando();

                }
            }
            finally
            {
                if (!conectado)
                {
                    DB2.Desconectar();
                }
            }
        }

        public void actualizaPROSAS(string folio, decimal secuencia, decimal pesoentregado, string estado)
        {
            try
            {
                DB2.Conectar();
                DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_OSA", CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@POSASFOLI", iDB2DbType.iDB2Char, folio);
                DB2.AsignarParamProcAlmac("@POSASSECU", iDB2DbType.iDB2Numeric, secuencia);
                DB2.AsignarParamProcAlmac("@POSASCAEN", iDB2DbType.iDB2Decimal, pesoentregado); //-1 para no actualizar el campo y si actualizar estado
                DB2.AsignarParamProcAlmac("@POSASSTOS", iDB2DbType.iDB2Char, estado); //vacio cuando se envía pesoentregado
                DB2.EjecutarComando();
            }
            catch (Exception ex)
            {
                Util.EscribeLog("AS OSA " + ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
        }

        public void actualizaPROSAS(string folio, decimal secuencia, string estado, DateTime fechaatencion)
        {
            try
            {
                DB2.Conectar();
                DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_OSA_FEAT", CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@POSASFOLI", iDB2DbType.iDB2Char, folio);
                DB2.AsignarParamProcAlmac("@POSASSECU", iDB2DbType.iDB2Numeric, secuencia);
                DB2.AsignarParamProcAlmac("@POSASSTOS", iDB2DbType.iDB2Char, estado); //vacio cuando se envía pesoentregado
                DB2.AsignarParamProcAlmac("@POSASFEAT", iDB2DbType.iDB2Date, fechaatencion); //vacio cuando se envía pesoentregado
                DB2.EjecutarComando();
            }
            catch (Exception ex)
            {
                Util.EscribeLog("AS OSA " + ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
        }

        public void actualizaGMDEEM(string empaque, decimal secuencia, decimal cantidadrestante, decimal pesorestante, decimal stockcerobolsa, decimal estadobolsa)
        {
            try
            {
                DB2.Conectar();

                DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_GMDEEM", CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@PDEEMCOEM", iDB2DbType.iDB2Char, empaque);
                DB2.AsignarParamProcAlmac("@PDEEMSECU", iDB2DbType.iDB2Numeric, secuencia);
                DB2.AsignarParamProcAlmac("@PDEEMCAST", iDB2DbType.iDB2Decimal, cantidadrestante);
                DB2.AsignarParamProcAlmac("@PDEEMPEST", iDB2DbType.iDB2Decimal, pesorestante);
                DB2.AsignarParamProcAlmac("@PDEEMSTCE", iDB2DbType.iDB2Numeric, stockcerobolsa); //-1 si no se actualizará EL CAMPO 
                DB2.AsignarParamProcAlmac("@PDEEMESBO", iDB2DbType.iDB2Numeric, estadobolsa);
                DB2.EjecutarComando();
            }
            catch (Exception ex)
            {
                Util.EscribeLog("AS DEEM " + ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
        }

        public void actualizaGMCAEM(string empaque, string estado)
        {
            try
            {
                DB2.Conectar();

                DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_GMCAEM", CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@PCAEMCOEM", iDB2DbType.iDB2Char, empaque);
                DB2.AsignarParamProcAlmac("@PCAEMESRE", iDB2DbType.iDB2Char, estado);
                DB2.EjecutarComando();
            }
            catch (Exception ex)
            {
                Util.EscribeLog("actualizaGMCAEM " + ex.Message);
            }
            finally
            {
                DB2.Desconectar();
            }
        }

        #region METODOS MIGRADOS
        public List<appWcfService.PECAPE> MostrarPedidosApp2()
        {
            List<appWcfService.PECAPE> lista = null;
            try
            {
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MostrarPedidosApp2();
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }
        #endregion

        private string ErrorGenerico(string exception)
        {
            if (OcultaErrorReal)
            {
                return Mensajes.MENSAJE_ERROR_GENERICO;
            }
            else
            {
                return exception;
            }
        }
    }
}
