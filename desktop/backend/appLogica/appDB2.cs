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
using System.Text.RegularExpressions;

namespace appLogica
{
    public class appDB2
    {
        internal BaseDatos DB2;

        private bool OcultaErrorReal;
        private string Aplicacion;
        private string DataSourceDB2;

        #region constantes

        private const decimal TIPO_MOV_SALIDA_PREP_PED = 1; //-
        private const decimal TIPO_MOV_CANCELA_SALIDA_PREP_PED = 2; //+
        private const decimal TIPO_MOV_MODIFICA_SALIDA_PREP_PED = 3;  //+

        private string ServidorSMTP, CuentaDe, CuentaDescripcion, ClaveCuenta, DominioCuenta, PuertoSMTP;

        #endregion

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
        public RESOPE obtieneDatosPartida(PAROPE paramOperacion)//string articulo, string partida, decimal idalmacen)
        {
            DataTable otrosDataTable;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            try
            {
                string codigoColor;
                StringBuilder comandoSql;
                ///////

                string articulo, partida;
                decimal idalmacen;

                articulo = paramOperacion.VALENT[0];
                partida = paramOperacion.VALENT[1];
                idalmacen = Decimal.Parse(paramOperacion.VALENT[2]);

                ///////

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
                    vpar.VALSAL.Add(stock.ToString(Constantes.FORMATO_DECIMAL_0 + "0").Trim());
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

        public RESOPE obtieneSeriesUsuario(PAROPE paramOperacion)
        {
            DataTable otrosDataTable;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            try
            {
                string codigousuario;
                StringBuilder comandoSql;

                codigousuario = paramOperacion.VALENT[0];
                DB2.Conectar();

                //stock
                vpar.VALSAL = new List<string>();

                comandoSql = new StringBuilder();
                comandoSql.Append("FAELDAT.USP_OBTIENE_SERIES_POR_USUARIO"); //FAELDAT
                DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
                DB2.AsignarParamProcAlmac("@PUSERCUID", iDB2DbType.iDB2Char, codigousuario);
                DB2.AsignarParamProcAlmac("@PSRIEIDTD", iDB2DbType.iDB2Numeric, Constantes.ID_TIPO_DOC_PEDNAC);
                otrosDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
                if (!Util.TablaVacia(otrosDataTable))
                {
                    foreach (DataRow cr in otrosDataTable.Rows)
                    {
                        vpar.VALSAL.Add(Convert.ToString(cr["SRIECUID"]));                     
                    }
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

        ///descomentar

        //public RESOPE GeneraPreguia(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> cabdetpedido, string serie, string usuario, string codprovtrans, string estabpart, decimal estadest)
        //{
        //    DataTable otrosDataTable;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    try
        //    {
        //        StringBuilder comandoSql;
        //        string ptopartida = "";
        //        decimal idpreguia;
        //        DB2.Conectar();

        //        //stock
        //        vpar.VALSAL = new List<string>();

        //        comandoSql = new StringBuilder();
        //        comandoSql.Append("COSTDAT.USP_OBTIENE_ESTABLECIMIENTO"); //COSTDAT
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //        DB2.AsignarParamProcAlmac("@PESTAESTA", iDB2DbType.iDB2Char, estabpart); //032
        //        otrosDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
        //        if (!Util.TablaVacia(otrosDataTable))
        //        {
        //            DataRow row = otrosDataTable.Rows[0];
        //            ptopartida = Convert.ToString(row["ESTADIR1"]).Trim() + Convert.ToString(row["ESTADIR2"]).Trim() + " - " + Convert.ToString(row["ESTADIST"]).Trim() + " - " + Convert.ToString(row["ESTAPROV"]).Trim() + " - " + Convert.ToString(row["ESTADEPA"]).Trim();
        //        }
        //        //por tipo usar el tipodoc y motivo en guia

        //        comandoSql = new StringBuilder();
        //        comandoSql.Append("FAELDAT.USP_PED_INSERTA_GUIA_CAB"); //FAELDAT
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //        //20181123
        //        if (cabdetpedido[0].CAPETIPO == Constantes.VENTA || cabdetpedido[0].CAPETIPO == Constantes.CONSIGNACION)
        //        {
        //            //20180418
        //            Decimal idtipdoc = cabdetpedido[0].CAPEIDTD;
        //            if (idtipdoc != Constantes.ID_TIPO_DOC_GUIA && idtipdoc != Constantes.ID_TIPO_DOC_NE)
        //            {
        //                idtipdoc = Constantes.ID_TIPO_DOC_GUIA;
        //            }
        //            if (idtipdoc == Constantes.ID_TIPO_DOC_NE)
        //            {
        //                codprovtrans = "";
        //                DB2.AsignarParamProcAlmac("@PDOCOIDES", iDB2DbType.iDB2Numeric, Constantes.ID_ESTADO_CREADO_NE); //7 creado
        //            }
        //            else
        //            {
        //                DB2.AsignarParamProcAlmac("@PDOCOIDES", iDB2DbType.iDB2Numeric, Constantes.ID_ESTADO_CREADO_PREGUIA); //7 creado
        //            }

        //            DB2.AsignarParamProcAlmac("@PDOCOIDTD", iDB2DbType.iDB2Numeric, idtipdoc); //3 TIPO GUIA o 5 NOTA ENTR
        //        }
        //        else if (cabdetpedido[0].CAPETIPO == Constantes.TRANSF_ALMACENES)
        //        {
        //            DB2.AsignarParamProcAlmac("@PDOCOIDES", iDB2DbType.iDB2Numeric, Constantes.ID_ESTADO_CREADO_PREGUIA); //7 creado
        //            DB2.AsignarParamProcAlmac("@PDOCOIDTD", iDB2DbType.iDB2Numeric, Constantes.ID_TIPO_DOC_GUIA); //3 TIPO GUIA
        //        }
        //        else
        //        {
        //            DB2.AsignarParamProcAlmac("@PDOCOIDES", iDB2DbType.iDB2Numeric, Constantes.ID_ESTADO_CREADO_TI); //7 creado
        //            DB2.AsignarParamProcAlmac("@PDOCOIDTD", iDB2DbType.iDB2Numeric, Constantes.ID_TIPO_DOC_TI); //ERA 14 NO 11 TIPO TI
        //        }
        //        decimal totalbruto = cabdetpedido.Sum(X => X.DEPEPEBR) + cabdetpedido[0].CAPETADE;
        //        DB2.AsignarParamProcAlmac("@PDOCOSRIE", iDB2DbType.iDB2Char, serie); //R007 o T001
        //        DB2.AsignarParamProcAlmac("@PDOCOEMIS", iDB2DbType.iDB2Date, DateTime.Today); // new DateTime(2016, 12, 1)); // DateTime.Today);
        //        DB2.AsignarParamProcAlmac("@PDOCOPBRU", iDB2DbType.iDB2Numeric, totalbruto);
        //        DB2.AsignarParamProcAlmac("@PDOCONBTS", iDB2DbType.iDB2Numeric, cabdetpedido[0].CAPENUBU);
        //        DB2.AsignarParamProcAlmac("@PDOCOOBSE", iDB2DbType.iDB2VarChar, cabdetpedido[0].CAPENOTG);
        //        DB2.AsignarParamProcAlmac("@PDOCODIRC", iDB2DbType.iDB2Char, cabdetpedido[0].CAPEDIRE);
        //        DB2.AsignarParamProcAlmac("@PDOCOUSCR", iDB2DbType.iDB2VarChar, usuario);
        //        DB2.AsignarParamProcAlmac("@PENTICUID", iDB2DbType.iDB2Char, cabdetpedido[0].CAPEIDCL);
        //        DB2.AsignarParamProcAlmac("@PDCENTRAN", iDB2DbType.iDB2Char, codprovtrans);
        //        DB2.AsignarParamProcAlmac("@PDCENPUPATR", iDB2DbType.iDB2VarChar, ptopartida);
        //        DB2.AsignarParamProcAlmac("@PFECHTRAS", iDB2DbType.iDB2TimeStamp, DateTime.Now); //new DateTime(2016, 12, 1));  //DateTime.Today); //---TEMPORAL PARA PRUEBAS
        //        DB2.AsignarParamProcAlmac("@PTIPOPEDI", iDB2DbType.iDB2Numeric, cabdetpedido[0].CAPETIPO); //20180221 MOTIVO DEL PEDIDO
        //        DB2.AsignarParamProcAlmac("@PESTADEST", iDB2DbType.iDB2Numeric, estadest); //20180221 MOTIVO DEL PEDIDO

        //        DB2.AsignarParamSalidaProcAlmac("@PDOCOCUID", iDB2DbType.iDB2Numeric, 19);
        //        DB2.EjecutarProcedimientoAlmacenado();
        //        idpreguia = Convert.ToDecimal(DB2.ObtieneParametro("@PDOCOCUID"));

        //        int numitemdet = 0;
                    
        //        foreach (var item in cabdetpedido)
        //        {
        //            numitemdet++;
        //            comandoSql = new StringBuilder();
        //            comandoSql.Append("FAELDAT.USP_PED_INSERTA_GUIA_DET"); //FAELDAT
        //            DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //            DB2.AsignarParamProcAlmac("@PDOCOCUID", iDB2DbType.iDB2Numeric, idpreguia);
        //            DB2.AsignarParamProcAlmac("@PDOCOUSCR", iDB2DbType.iDB2VarChar, usuario);
        //            DB2.AsignarParamProcAlmac("@PDDCOESAF", iDB2DbType.iDB2Numeric, 2); //2
        //            DB2.AsignarParamProcAlmac("@PDDCODESC", iDB2DbType.iDB2VarChar, item.DEPEDSAR); //INVOCAR CL ANTES O DESPUES
        //            if (numitemdet == 1)
        //            {
        //                DB2.AsignarParamProcAlmac("@PDDCOBLTS", iDB2DbType.iDB2Numeric, cabdetpedido[0].CAPENUBU); //ES BULTOS NO CONOS DE DONDE SALE
        //                DB2.AsignarParamProcAlmac("@PDDCOPACN", iDB2DbType.iDB2Numeric, item.DEPEPEAT); //--EL MISMO neto, NO SE HA ESPECIFICADO acond
        //                DB2.AsignarParamProcAlmac("@PDDCOPBRU", iDB2DbType.iDB2Numeric, item.DEPEPEBR + cabdetpedido[0].CAPETADE);
        //            }
        //            else
        //            {
        //                DB2.AsignarParamProcAlmac("@PDDCOBLTS", iDB2DbType.iDB2Numeric, 0); //ES BULTOS NO CONOS DE DONDE SALE
        //                DB2.AsignarParamProcAlmac("@PDDCOPACN", iDB2DbType.iDB2Numeric, item.DEPEPEAT); //--EL MISMO neto, NO SE HA ESPECIFICADO acond
        //                DB2.AsignarParamProcAlmac("@PDDCOPBRU", iDB2DbType.iDB2Numeric, item.DEPEPEBR);
        //            }
        //            DB2.AsignarParamProcAlmac("@PDDCOPNET", iDB2DbType.iDB2Numeric, item.DEPEPEAT);
        //            DB2.AsignarParamProcAlmac("@PDDCOFACN", iDB2DbType.iDB2Numeric, 0);
        //            DB2.AsignarParamProcAlmac("@PPDDCIDPI", iDB2DbType.iDB2Char, item.DEPECOAR);
        //            DB2.AsignarParamProcAlmac("@PPDDCCANT", iDB2DbType.iDB2Numeric, item.DEPECAAT);
        //            DB2.AsignarParamProcAlmac("@PPDDCPEDI", iDB2DbType.iDB2Char, item.DEPECONT);
        //            DB2.AsignarParamProcAlmac("@PPRALIDAL", iDB2DbType.iDB2Numeric, item.DEPEALMA);
        //            DB2.AsignarParamProcAlmac("@PPRALIDPA", iDB2DbType.iDB2Char, item.DEPEPART);

        //            DB2.AsignarParamProcAlmac("@PCVTDSECU", iDB2DbType.iDB2Numeric, item.DEPESECU);

        //            DB2.AsignarParamSalidaProcAlmac("@PDDCOCUID", iDB2DbType.iDB2Numeric, 19);
        //            DB2.EjecutarProcedimientoAlmacenado();

        //        }
        //        vpar.VALSAL.Add(Convert.ToString(idpreguia));
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //    return vpar;
        //}

        ///descomentar

        //public RESOPE GeneraPreguia(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> cabdetpedido, string serie, string usuario, string codprovtrans, string estabpart)
        //{
        //    DataTable otrosDataTable;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    try
        //    {
        //        StringBuilder comandoSql;
        //        string ptopartida = "";
        //        decimal idpreguia;
        //        DB2.Conectar();

        //        //stock
        //        vpar.VALSAL = new List<string>();

        //        comandoSql = new StringBuilder();
        //        comandoSql.Append("COSTDAT.USP_OBTIENE_ESTABLECIMIENTO"); //COSTDAT
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //        DB2.AsignarParamProcAlmac("@PESTAESTA", iDB2DbType.iDB2Char, estabpart); //032
        //        otrosDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
        //        if (!Util.TablaVacia(otrosDataTable))
        //        {
        //            DataRow row = otrosDataTable.Rows[0];
        //            ptopartida = Convert.ToString(row["ESTADIR1"]).Trim() + Convert.ToString(row["ESTADIR2"]).Trim() + " - " + Convert.ToString(row["ESTADIST"]).Trim() + " - " + Convert.ToString(row["ESTAPROV"]).Trim() + " - " + Convert.ToString(row["ESTADEPA"]).Trim();
        //        }

        //        comandoSql = new StringBuilder();
        //        comandoSql.Append("FAELDAT.USP_PED_INSERTA_GUIA_CAB"); //FAELDAT
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //        DB2.AsignarParamProcAlmac("@PDOCOIDES", iDB2DbType.iDB2Numeric, Constantes.ID_ESTADO_CREADO_PREGUIA); //7 creado
        //        DB2.AsignarParamProcAlmac("@PDOCOIDTD", iDB2DbType.iDB2Numeric, Constantes.ID_TIPO_DOC_GUIA); //3 TIPO GUIA
        //        DB2.AsignarParamProcAlmac("@PDOCOSRIE", iDB2DbType.iDB2Char, serie); //R007 
        //        DB2.AsignarParamProcAlmac("@PDOCOEMIS", iDB2DbType.iDB2Date, new DateTime(2016, 12, 1)); // DateTime.Today);
        //        DB2.AsignarParamProcAlmac("@PDOCOPBRU", iDB2DbType.iDB2Numeric, cabdetpedido.Sum(X => X.DEPEPEBR));
        //        DB2.AsignarParamProcAlmac("@PDOCONBTS", iDB2DbType.iDB2Numeric, cabdetpedido[0].CAPENUBU);
        //        DB2.AsignarParamProcAlmac("@PDOCOOBSE", iDB2DbType.iDB2VarChar, cabdetpedido[0].CAPENOTG);
        //        DB2.AsignarParamProcAlmac("@PDOCODIRC", iDB2DbType.iDB2VarChar, cabdetpedido[0].CAPEDIRE);
        //        DB2.AsignarParamProcAlmac("@PDOCOUSCR", iDB2DbType.iDB2VarChar, usuario);
        //        DB2.AsignarParamProcAlmac("@PENTICUID", iDB2DbType.iDB2Char, cabdetpedido[0].CAPEIDCL);
        //        DB2.AsignarParamProcAlmac("@PDCENTRAN", iDB2DbType.iDB2Char, codprovtrans);
        //        DB2.AsignarParamProcAlmac("@PDCENPUPATR", iDB2DbType.iDB2VarChar, ptopartida);
        //        DB2.AsignarParamProcAlmac("@PFECHTRAS", iDB2DbType.iDB2TimeStamp, new DateTime(2016, 12, 1));  //DateTime.Today); //---TEMPORAL PARA PRUEBAS

        //        DB2.AsignarParamSalidaProcAlmac("@PDOCOCUID", iDB2DbType.iDB2Numeric, 19);
        //        DB2.EjecutarProcedimientoAlmacenado();
        //        idpreguia = Convert.ToDecimal(DB2.ObtieneParametro("@PDOCOCUID"));

        //        foreach (var item in cabdetpedido)
        //        {
        //            comandoSql = new StringBuilder();
        //            comandoSql.Append("FAELDAT.USP_PED_INSERTA_GUIA_DET"); //FAELDAT
        //            DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);
        //            DB2.AsignarParamProcAlmac("@PDOCOCUID", iDB2DbType.iDB2Numeric, idpreguia);
        //            DB2.AsignarParamProcAlmac("@PDOCOUSCR", iDB2DbType.iDB2VarChar, usuario);
        //            DB2.AsignarParamProcAlmac("@PDDCOESAF", iDB2DbType.iDB2Numeric, 2); //2
        //            DB2.AsignarParamProcAlmac("@PDDCODESC", iDB2DbType.iDB2VarChar, item.DEPEDSAR); //INVOCAR CL ANTES O DESPUES
        //            DB2.AsignarParamProcAlmac("@PDDCOBLTS", iDB2DbType.iDB2Numeric, item.DEPECAAT); //ES BULTOS NO CONOS DE DONDE SALE
        //            DB2.AsignarParamProcAlmac("@PDDCOPACN", iDB2DbType.iDB2Numeric, item.DEPEPEBR); //--EL MISMO BRUTO, NO SE HA ESPECIFICADO acond
        //            DB2.AsignarParamProcAlmac("@PDDCOPBRU", iDB2DbType.iDB2Numeric, item.DEPEPEBR);
        //            DB2.AsignarParamProcAlmac("@PDDCOPNET", iDB2DbType.iDB2Numeric, item.DEPEPEAT);
        //            DB2.AsignarParamProcAlmac("@PDDCOFACN", iDB2DbType.iDB2Numeric, 0);
        //            DB2.AsignarParamProcAlmac("@PPDDCIDPI", iDB2DbType.iDB2Char, item.DEPECOAR);
        //            DB2.AsignarParamProcAlmac("@PPDDCCANT", iDB2DbType.iDB2Numeric, item.DEPECAAT);
        //            DB2.AsignarParamProcAlmac("@PPDDCPEDI", iDB2DbType.iDB2Char, item.DEPECONT);
        //            DB2.AsignarParamProcAlmac("@PPRALIDAL", iDB2DbType.iDB2Numeric, item.DEPEALMA);
        //            DB2.AsignarParamProcAlmac("@PPRALIDPA", iDB2DbType.iDB2Char, item.DEPEPART);

        //            DB2.AsignarParamSalidaProcAlmac("@PDDCOCUID", iDB2DbType.iDB2Numeric, 19);
        //            DB2.EjecutarProcedimientoAlmacenado();

        //        }

        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //    return vpar;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TipRsrva">Tipo de reserva, N, considerar solo 99999 o contratos S 
        /// I = Reserva de Insumos
        /// N = Reserva Normal
        /// S = Reserva para Atención de Stock</param>
        /// <param name="NoFolio">Número de Folio usar 0</param>
        /// <param name="Secuencia">Secuencia</param>
        /// <param name="Articulo">Artículo que se reserva</param>
        /// <param name="Partida">Partida que se reserva</param>
        /// <param name="Almacen">Almacén donde está la partida que se reserva</param>
        /// <param name="Destino">Destino de la reserva(pedido, partida o referencia de un usuario, usar usuario 6 caracteres</param>
        /// <param name="TpDest">Tipo de destino, usar temporalmente Z, se debe definir 
        /// P = Partida
        /// E = Pedido de exportación
        /// N = Pedido Nacional
        /// T = Pedido de Servicio a Terceros
        /// I = Pedido Interno
        /// S = Pedido de Stock
        /// Z = Reserva preventiva sin pedido</param>
        /// <param name="SecDest">Secuencia del ítem del pedido, en partidas es 01, en preventivas en 00, usar 0</param>
        /// <param name="ScPrdDst">Secuencia de producto intermedio donde se utilizará (nodo superior), usar 0</param>
        /// <param name="ScPrdAtn">Secuencia de producto intermedio origen, usar 0</param>
        /// <param name="Cantidad">Cantidad que se reserva, cantidad con 2 ultimos digitos parte decimal</param>
        /// <param name="TipAcci">Tipo de acción, usar siempre A insert o L anular o liiberar reserva
        /// A = Agregar Reserva
        /// L = Liberar Reserva
        /// M = Modificar Reserva
        /// E = Entregar o Atender Reserva
        /// R = Reconfirmar Reserva</param>
        /// 


        ///descomentar
        //public void generaReserva(bool conectado, string TipRsrva, string NoFolio, string Secuencia, string Articulo, string Partida, string Almacen, string Destino, string TpDest, string SecDest, string ScPrdDst, string ScPrdAtn, string Cantidad, string TipAcci)
        //{
        //    try
        //    {

        //        if (!conectado)
        //        {
        //            DB2.Conectar();
        //        }
        //        StringBuilder comandoSql = new StringBuilder();
        //        StringBuilder actreservSql; // = new StringBuilder();

        //        TipRsrva = TipRsrva.PadRight(1, ' ');
        //        NoFolio = NoFolio.PadLeft(5, '0').Substring(0,5);
        //        Secuencia = Secuencia.PadLeft(2, '0');
        //        Articulo = Articulo.PadRight(15, ' ');
        //        Partida = Partida.PadRight(6, ' ');
        //        Almacen = Almacen.PadLeft(3, '0');
        //        Destino = Destino.PadRight(6, ' ').Substring(0,6);
        //        TpDest = TpDest.PadRight(1, ' ');

        //        SecDest = SecDest.PadLeft(2, '0');
        //        ScPrdDst = ScPrdDst.PadLeft(3, '0');
        //        ScPrdAtn = ScPrdAtn.PadLeft(3, '0');
        //        Cantidad = Cantidad.PadLeft(9, '0');
        //        TipAcci = TipAcci.PadRight(1, ' ');

        //        actreservSql = new StringBuilder();
        //        actreservSql.Append("CALL INCAOBJ.ACTRESPP(");  //GMA003PP 20151130 PRUEBAS USUARIO
        //        actreservSql.Append("'").Append(TipRsrva).Append("', ");
        //        actreservSql.Append("'").Append(NoFolio).Append("', ");
        //        actreservSql.Append("'").Append(Secuencia).Append("', ");
        //        actreservSql.Append("'").Append(Articulo).Append("', ");
        //        actreservSql.Append("'").Append(Partida).Append("', ");
        //        actreservSql.Append("'").Append(Almacen).Append("', ");
        //        actreservSql.Append("'").Append(Destino).Append("', ");
        //        actreservSql.Append("'").Append(TpDest).Append("', ");
        //        actreservSql.Append("'").Append(SecDest).Append("', ");
        //        actreservSql.Append("'").Append(ScPrdDst).Append("', ");
        //        actreservSql.Append("'").Append(ScPrdAtn).Append("', ");
        //        actreservSql.Append("'").Append(Cantidad).Append("', ");
        //        actreservSql.Append("'").Append(TipAcci).Append("') ");

        //        //xselec = "call GEMAPRG.GMA003PP('"+cia +"','"+ allt(tcomp) +"','"+ allt(comprob) +"','"+ allt(fecha) +"','"+ m->articulo +"','"+ allt(m->almacen) +"','"+ allt(partida) +"','"+ allt(cantidad) +"','"+ total +"','"+ m->signo +"','"+ orides +"','"+ destino +"','"+ unidad +"','"+ item +"','" + estado +"')"

        //        DB2.CrearComando(actreservSql.ToString(), CommandType.Text);
        //        //PRUEBAS 
        //        DB2.EjecutarComando();
        //    }
        //    finally
        //    {
        //        if (!conectado)
        //        {
        //            DB2.Desconectar();
        //        }
        //    }
        //}


        ///descomentar

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conectado"></param>
        /// <param name="articulo"></param>
        /// <param name="descripcion"></param>
        /// <param name="pedido"></param>
        /// <param name="tipoPedido"></param>
        /// <param name="secuencia"></param>
        /// <param name="cantidad"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public string descripcionItem(bool conectado, string articulo, string descripcion, string pedido, string tipoPedido, string secuencia, string cantidad, string accion)
        {
            try
            {
                StringBuilder comandoSql;

                if (!conectado)
                {
                    DB2.Conectar();
                }

                if (pedido != "999999")
                {
                    DB2.CrearComando("FAELDAT.USP_PED_OBTIENE_SEC_CONTRATO", CommandType.StoredProcedure);
                    DB2.AsignarParamProcAlmac("@PCONTRATO", iDB2DbType.iDB2Char, pedido); //cod
                    DB2.AsignarParamProcAlmac("@PCVTDARTI", iDB2DbType.iDB2Char, articulo); //si
                    DB2.AsignarParamSalidaProcAlmac("@PCVTDSECU", iDB2DbType.iDB2Decimal, 2); //00
                    DB2.EjecutarProcedimientoAlmacenado();
                    if (DB2.ObtieneParametro("@PCVTDSECU") != DBNull.Value)
                    {
                        secuencia = Convert.ToString(DB2.ObtieneParametro("@PCVTDSECU")).Trim();
                    }
                }

                comandoSql = new StringBuilder();
                comandoSql.Append("FAELDAT.PR_FAEL_OBTENERDESCRIPCION"); //FAELDAT
                DB2.CrearComando(comandoSql.ToString(), CommandType.StoredProcedure);

                DB2.AsignarParamEntSalProcAlmac("@ART", iDB2DbType.iDB2VarChar, articulo.PadRight(15, ' ')); //cod
                DB2.AsignarParamEntSalProcAlmac("@DES", iDB2DbType.iDB2VarChar, descripcion.PadLeft(120, ' ')); //
                DB2.AsignarParamEntSalProcAlmac("@PED", iDB2DbType.iDB2VarChar, pedido.PadRight(6, '0')); //si
                DB2.AsignarParamEntSalProcAlmac("@TPD", iDB2DbType.iDB2VarChar, tipoPedido.PadLeft(1, '0')); //N
                DB2.AsignarParamEntSalProcAlmac("@SEC", iDB2DbType.iDB2VarChar, secuencia.PadLeft(2, '0')); //00
                DB2.AsignarParamEntSalProcAlmac("@CAN", iDB2DbType.iDB2VarChar, cantidad.PadLeft(5, '0')); //int
                DB2.AsignarParamEntSalProcAlmac("@ACC", iDB2DbType.iDB2VarChar, accion.PadLeft(1, ' ')); //3 o 1 si no hay pedido

                DB2.EjecutarProcedimientoAlmacenado();
                descripcion = Convert.ToString(DB2.ObtieneParametro("@DES")).Trim();

                return descripcion;

            }
            finally
            {
                if (!conectado)
                {
                    DB2.Desconectar();
                }
            }
        }

        ////descomentar


        //public void actualizaPROSAS(string folio, decimal secuencia, decimal pesoentregado, string estado)
        //{
        //    try
        //    {
        //        DB2.Conectar();
        //        DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_OSA", CommandType.StoredProcedure);
        //        DB2.AsignarParamProcAlmac("@POSASFOLI", iDB2DbType.iDB2Char, folio);
        //        DB2.AsignarParamProcAlmac("@POSASSECU", iDB2DbType.iDB2Numeric, secuencia);
        //        DB2.AsignarParamProcAlmac("@POSASCAEN", iDB2DbType.iDB2Decimal, pesoentregado); //-1 para no actualizar el campo y si actualizar estado
        //        DB2.AsignarParamProcAlmac("@POSASSTOS", iDB2DbType.iDB2Char, estado); //vacio cuando se envía pesoentregado
        //        DB2.EjecutarComando();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //}

        //public void actualizaGMDEEM(string empaque, decimal secuencia, decimal cantidadrestante, decimal pesorestante, decimal stockcerobolsa, decimal estadobolsa)
        //{
        //    try
        //    {
        //        DB2.Conectar();

        //        DB2.CrearComando("PRODDAT.USP_PED_ACTUALIZA_GMDEEM", CommandType.StoredProcedure);
        //        DB2.AsignarParamProcAlmac("@PDEEMCOEM", iDB2DbType.iDB2Char, empaque);
        //        DB2.AsignarParamProcAlmac("@PDEEMSECU", iDB2DbType.iDB2Numeric, secuencia);
        //        DB2.AsignarParamProcAlmac("@PDEEMCAST", iDB2DbType.iDB2Decimal, cantidadrestante);
        //        DB2.AsignarParamProcAlmac("@PDEEMPEST", iDB2DbType.iDB2Decimal, pesorestante);
        //        DB2.AsignarParamProcAlmac("@PDEEMSTCE", iDB2DbType.iDB2Numeric, stockcerobolsa); //-1 si no se actualizará EL CAMPO 
        //        DB2.AsignarParamProcAlmac("@PDEEMESBO", iDB2DbType.iDB2Numeric, estadobolsa);
        //        DB2.EjecutarComando();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //}

        ///descomentar

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

        #region METODOS MIGRADOS
        //Clientes
        public RESOPE BuscaCliente(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.TCLIE> lista = null;
            try
            {
                string valbus;
                valbus = paramOperacion.VALENT[0].Trim();

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.BuscaCliente(valbus);
                }
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE MuestraPedidos(PAROPE paramOperacion)
        {
            //Muestra pedidos con estado 1
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            List<appWcfService.PECAPE> lista = null;
            try
            {
                int valbus;
                valbus = int.Parse(paramOperacion.VALENT[0]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (valbus == 0)
                    {
                        lista = context.MuestraPedidos();
                        lista = lista.OrderByDescending(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                    }
                    else
                    {
                        lista = context.MuestraPedidos();
                        lista = lista.Where(ped => ped.CAPEIDES == valbus).ToList<appWcfService.PECAPE>();
                    }
                }

                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE BuscaPedidosFechas(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PECAPE> lista = null;
            try
            {   //Variables         
                string valbus, fechainicio, fechafin, serie;
                decimal estado, nume = 0;

                valbus = paramOperacion.VALENT[0];
                estado = decimal.Parse(paramOperacion.VALENT[1]);
                fechainicio = paramOperacion.VALENT[2];
                fechafin = paramOperacion.VALENT[3];
                serie = paramOperacion.VALENT[4];

                Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
                Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.


                bool num = regnum.IsMatch(valbus); //que tenga numeros
                bool letra = reglet.IsMatch(valbus); //que tenga letras


                if (num && !letra)//se verifica que tenga numeros pero ninguna letra
                {
                    nume = decimal.Parse(paramOperacion.VALENT[0]);
                }

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (fechainicio == "" && fechafin == "") //Busquedas sin fechas
                    {
                        if (estado == 0 && serie.Equals("")) //Si el estado es 0 Mostrara todos los pedidos
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado == 0 & serie != "") //Todos los estados, sin parametro de busqueda pero con serie
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                        //filtro segun estado indicado
                        else if (valbus != "" && serie.Equals("")) //Mostrara todos con el estado indicado y sin serie
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                       && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                        else if (valbus.Equals("") && serie != "") // busca con serie indicada y sin valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                        else if (valbus != "" && serie != "") //busca con serie inidicada y con valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                       && ped.CAPEIDES == estado && serie.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                        else //Solo busca por el estado
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                        }
                    }
                    //Busqueda por fechas
                    else if (fechainicio != "" && fechafin != "")
                    {
                        DateTime feini, fefin;
                        feini = DateTime.Parse(fechainicio);
                        fefin = DateTime.Parse(fechafin);
                        //Busqueda sin estado
                        if (estado == 0 && serie.Equals("") && valbus.Equals("")) //toma en cuenta solo el rango de fechas
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado == 0 && serie.Equals("") && valbus != "") // toma en cuenta el rango y valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                            && ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta el rango de fechas mas la serie
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPESERI.Contains(serie)).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado == 0 && serie != "" && valbus != "") //toma en cuenta el rango de fechas, serie y valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                          && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin)) && ped.CAPESERI.Contains(serie)).ToList<appWcfService.PECAPE>();
                        }

                        // Busqueda con estado
                        else if (estado != 0 && serie.Equals("") && valbus.Equals("")) //toma en cuenta el rango de fechas y el estado
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado != 0 && serie != "" && valbus.Equals("")) //toma en cuenta el rango de fechas, la serie y el estado
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado) && ped.CAPESERI.Contains(serie)).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado != 0 && serie.Equals("") && valbus != "") //toma en cuenta el rango de fechas, el estado, y el valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                            && ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado)).ToList<appWcfService.PECAPE>();
                        }
                        else if (estado != 0 && serie != "" && valbus != "") // toma en cuenta el rango de fechas, estado, serie y valor de busqueda
                        {
                            lista = context.MuestraPedidos();
                            lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
                            && ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado) && ped.CAPESERI.Contains(serie)).ToList<appWcfService.PECAPE>();
                        }
                    }
                }

                //lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE MuestraDetallePedidos(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            //List<object> listaeo2 = new List<object>();

            List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> lista = null;
            List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> lista2 = new List<USP_OBTIENE_DETALLE_PEDIDOS_Result>();
            List<appWcfService.PECAPE> lisIdPedido = null;
            try
            {
                lisIdPedido = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
                foreach (var item in lisIdPedido)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        lista = context.MuestraDetallePedidos(item.CAPEIDCP);
                    }
                    lista2.AddRange(lista);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>(listaeo2);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista2));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        //pedidos area almacen
        public RESOPE MuestraPedidosAlmacen(PAROPE paramOperacion)
        {
            //DMA 07/05/2018 Se ha modificado para que tome en cuenta la prioridad al momento de ordenar
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PECAPE> lista = null;
            try
            {
                DateTime feini, fefin;

                feini = DateTime.Parse(paramOperacion.VALENT[0]);
                fefin = DateTime.Parse(paramOperacion.VALENT[1]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraPedidos();
                    lista = lista.Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                        .OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<appWcfService.PECAPE>();
                }
                //lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE BuscaPedidosAlmacen(PAROPE paramOperacion)
        {
            //DMA 07/05/2018 Se ha modificado para que tome en cuenta la prioridad al momento de ordenar
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };


            //List<object> listaeo = null;
            List<appWcfService.PECAPE> lista = null;
            try
            {
                string valbus, serie, fechainicio, fechafin;
                decimal estado, nume = 0;
                valbus = paramOperacion.VALENT[0];
                estado = decimal.Parse(paramOperacion.VALENT[1]);
                serie = paramOperacion.VALENT[2];
                fechainicio = paramOperacion.VALENT[3];
                fechafin = paramOperacion.VALENT[4];


                Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
                Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.
                bool num = regnum.IsMatch(valbus); //que tenga numeros
                bool letra = reglet.IsMatch(valbus); //que tenga letras

                if (num && !letra)//se verifica que tenga numeros pero ninguna letra
                {
                    nume = decimal.Parse(paramOperacion.VALENT[0]);
                }
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    //BUSQUEDA SIN RANGO DE FECHAS
                    if (fechainicio.Equals("") && fechafin.Equals(""))
                    {
                        if (estado == 0) //Busqueda sin estado primero
                        {
                            if (estado == 0 && serie.Equals("") && valbus.Equals("")) //Muestra todos los pedidos 2 y 3
                            {
                                lista = context.MuestraPedidos(); 
                                lista = lista.Where(ped => ped.CAPEIDES == 2 || ped.CAPEIDES == 3)
                                    .OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie.Equals("") && valbus != "") //Toma en cuenta el valor de busqueda
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta solo la serie
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && ped.CAPESERI.Contains(serie)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie != "" && valbus != "") // toma en cuenta serie y valor de busqueda
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && ped.CAPESERI.Contains(serie)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                        }
                        else if (estado != 0) //Busqueda por estado
                        {
                            if (serie.Equals("") && valbus.Equals("")) // busca solo por el estado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie != "" && valbus.Equals("")) //busca solo por la serie con el estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie.Equals("") && valbus != "") // busca solo por el valor de busqueda con el estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie != "" && valbus != "") // busca por serie, valor de busqueda y estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPESERI.Contains(serie)) && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                        }
                    }
                    else
                    {
                        DateTime feini, fefin;
                        feini = DateTime.Parse(fechainicio);
                        fefin = DateTime.Parse(fechafin);

                        //Busqueda sin estado
                        if (estado == 0) //Busqueda sin estado primero
                        {
                            if (estado == 0 && serie.Equals("") && valbus.Equals("")) //Muestra todos los pedidos 2 y 3 
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                    .OrderByDescending(ped => ped.CAPEIDES).OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie.Equals("") && valbus != "") //Toma en cuenta el valor de busqueda
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                    .OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta solo la serie
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ((ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && ped.CAPESERI.Contains(serie)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                    .OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (estado == 0 && serie != "" && valbus != "") // toma en cuenta serie y valor de busqueda
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && ped.CAPESERI.Contains(serie))
                                && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                        }
                        else if (estado != 0) //Busqueda por estado
                        {
                            if (serie.Equals("") && valbus.Equals("")) // busca solo por el estado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                        .OrderBy(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie != "" && valbus.Equals("")) //busca solo por la serie con el estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => (ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                        .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie.Equals("") && valbus != "") // busca solo por el valor de busqueda con el estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                        .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                            else if (serie != "" && valbus != "") // busca por serie, valor de busqueda y estado indicado
                            {
                                lista = context.MuestraPedidos();
                                lista = lista.Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPESERI.Contains(serie)) && ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
                                        .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<appWcfService.PECAPE>();
                            }
                        }
                    }
                    //lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
                    vpar.VALSAL = new List<string>();
                    if (lista.Count > 0)
                    {
                        vpar.VALSAL.Add("1");
                        vpar.VALSAL.Add(Util.Serialize(lista));
                    }
                    else
                    {
                        vpar.VALSAL.Add("0");

                    }
                    vpar.ESTOPE = true;

                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);

            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE Buscasolicitud(PAROPE paramOperacion)
        {
            //filtro de las solicitudes de aprobacion
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PECAPE> lista = null;
            try
            {
                string valbus;
                decimal nume = 0;

                valbus = paramOperacion.VALENT[0];
                // estado = decimal.Parse(paramOperacion.VALENT[1]);
                Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
                Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.


                bool num = regnum.IsMatch(valbus); //que tenga numeros
                bool letra = reglet.IsMatch(valbus); //que tenga letras


                if (num && !letra)//se verifica que tenga numeros pero ninguna letra
                {
                    nume = decimal.Parse(paramOperacion.VALENT[0]);
                }

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (valbus == "") //Supone mostrar todos
                    {
                        lista = context.MuestraPedidos();
                        lista = lista.Where(ped => ped.CAPEIDES == 4).ToList<appWcfService.PECAPE>();
                    }
                    else
                    {
                        //Orden para filtrar??
                        lista = context.MuestraPedidos();
                        lista = lista.Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSMO.Contains(valbus) || ped.CAPENUME == nume || ped.CAPESERI.Contains(valbus) || ped.CAPEUSFP.Contains(valbus)) && ped.CAPEIDES == 4).ToList<appWcfService.PECAPE>();
                    }
                }
                //lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE MuestraUbicacionesArticulo(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_UBICACIONES_Result> lista = null;
            try
            {
                string articulo, partida;
                decimal almacen;
                articulo = paramOperacion.VALENT[0];
                partida = paramOperacion.VALENT[1];
                almacen = decimal.Parse(paramOperacion.VALENT[2]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraUbicacionesArticulo(articulo, partida, almacen);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_UBICACIONES_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE ObtieneBolsa(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_BOLSA_Result> lista = null;
            try
            {
                decimal iddetalle = decimal.Parse(paramOperacion.VALENT[0]);
                string empaque = paramOperacion.VALENT[1];

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtieneBolsa(iddetalle, empaque);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_BOLSA_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {

            }
            return vpar;
        }

        public RESOPE MuestraPasillos(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PEPASI> lista = null;
            try
            {
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraPasillos();
                }
                //lista = Util.ParseEntityObject<appWcfService.PEPASI>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE MuestraNiveles(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PENIVE> lista = null;
            try
            {
                decimal idpasillo;
                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraNiveles(idpasillo);
                }

                //lista = Util.ParseEntityObject<appWcfService.PENIVE>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE MuestraColumnas(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PECOLU> lista = null;
            try
            {
                decimal idpasillo;
                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraColumnas(idpasillo);
                }

                //lista = Util.ParseEntityObject<appWcfService.PECOLU>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE MuestraCasilleros(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PECASI> lista = null;
            try
            {
                decimal idpasillo, idcolumna;
                string idnivel;

                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
                idcolumna = decimal.Parse(paramOperacion.VALENT[1]);
                idnivel = paramOperacion.VALENT[2];

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.MuestraCasilleros(idpasillo, idcolumna, idnivel);
                }

                //lista = Util.ParseEntityObject<appWcfService.PECASI>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        //Reportes
        public RESOPE ObtienePedidoConsulta(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
            try
            {
                decimal idpedido;
                idpedido = decimal.Parse(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtienePedidoConsulta(idpedido);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE ReportePartidaAlmacen(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result> lista = null;
            try
            {
                string partida;
                decimal almacen;
                partida = paramOperacion.VALENT[0];
                almacen = decimal.Parse(paramOperacion.VALENT[1]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ReportePartidaAlmacen(partida, almacen);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE ReporteMovimientosPartida(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

           // List<object> listaeo = null;
            List<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result> lista = null;
            try
            {
                string partida;
                decimal almacen;
                partida = paramOperacion.VALENT[0];
                almacen = decimal.Parse(paramOperacion.VALENT[1]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ReporteMovimientosPartida(partida, almacen);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }

        public RESOPE ReporteMovimientosFechas(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result> lista = null;
            DateTime feini;
            DateTime fefin;
            try
            {
                feini = DateTime.Parse(paramOperacion.VALENT[0].ToString());
                fefin = DateTime.Parse(paramOperacion.VALENT[1].ToString());
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ReporteMovimientosFechas(feini, fefin);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");
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
            }
            return vpar;
        }

        public RESOPE DevuelveCasilleros(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //List<object> listaeo = null;
            List<appWcfService.PECASI> lista = null;
            decimal idpasillo, idcolumna;
            string idnivel;
            try
            {
                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
                idcolumna = decimal.Parse(paramOperacion.VALENT[1]);
                idnivel = paramOperacion.VALENT[2];

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (idpasillo != 0 && idcolumna == 0 && idnivel.Equals(""))
                    {
                        //solo pasillos
                        lista = context.DevuelveCasilleros();
                        lista = lista.Where(cas => cas.CASIIDPA == idpasillo).ToList<appWcfService.PECASI>();
                    }
                    else
                    {
                        if (idcolumna != 0 && idpasillo != 0 && idnivel.Equals(""))
                        {
                            //columnas y pasillo
                            lista = context.DevuelveCasilleros();
                            lista = lista.Where(cas => cas.CASIIDCO == idcolumna && cas.CASIIDPA == idpasillo).ToList<appWcfService.PECASI>();
                        }
                        else
                        {
                            //nivel y pasillo
                            lista = context.DevuelveCasilleros();
                            lista = lista.Where(cas => cas.CASIIDNI.Equals(idnivel) && cas.CASIIDPA == idpasillo).ToList<appWcfService.PECASI>();
                        }
                    }
                }
                //lista = Util.ParseEntityObject<appWcfService.PECASI>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE DevuelveNiveyColu(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //List<object> listaeo = null;
            decimal idpasillo = 0;
            string informacion = "";
            try
            {
                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
                informacion = paramOperacion.VALENT[1];

                List<appWcfService.PENIVE> lista = null;
                List<appWcfService.PECOLU> lista1 = null;

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (informacion.Equals("nivel"))
                    {
                        //solo pasillos
                        lista = context.DevuelveNiveles();
                        lista = lista.Where(nive => nive.NIVEIDPA == idpasillo).ToList<appWcfService.PENIVE>();
                    }
                    else
                    {
                        lista1 = context.DevuelveColumna();
                        lista1 = lista1.Where(colu => colu.COLUIDPA == idpasillo).ToList<appWcfService.PECOLU>();
                    }
                }
                vpar.VALSAL = new List<string>();
                if (informacion.Equals("nivel"))
                {
                    //lista = Util.ParseEntityObject<appWcfService.PENIVE>(listaeo);
                    if (lista.Count > 0)
                    {
                        vpar.VALSAL.Add("1"); //existe //0
                        vpar.VALSAL.Add(Util.Serialize(lista));
                        //vpar.VALSAL.Add()
                    }
                    else
                    {
                        vpar.VALSAL.Add("0"); //no existe
                    }
                }
                else
                {
                    //lista1 = Util.ParseEntityObject<appWcfService.PECOLU>(listaeo);
                    if (lista1.Count > 0)
                    {
                        vpar.VALSAL.Add("1"); //existe //0
                        vpar.VALSAL.Add(Util.Serialize(lista1));
                        //vpar.VALSAL.Add()
                    }
                    else
                    {
                        vpar.VALSAL.Add("0"); //no existe
                    }
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE ObtienePedidoTracking(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
            try
            {
                int iniciobus;
                string trackingno, num2der, ser2der, diaemi;
                decimal idpedido;
                trackingno = paramOperacion.VALENT[0];
                //N-2-2-2
                //ID-NUM 2 DERECHA-SERIE 2 DERECHA-DIA EMISION
                iniciobus = trackingno.Length - 6;
                idpedido = decimal.Parse(trackingno.Substring(0, iniciobus));
                num2der = trackingno.Substring(iniciobus, 2);
                ser2der = trackingno.Substring(iniciobus + 2, 2);
                diaemi = trackingno.Substring(iniciobus + 4, 2);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtienePedidoTracking(idpedido);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    string numero = lista[0].CAPENUME.ToString().PadLeft(8, '0').Substring(6);
                    //valdia el trackingnumber
                    if (lista[0].CAPEFECH.Day.ToString().PadLeft(2, '0') == diaemi && lista[0].CAPESERI.Substring(2) == ser2der && numero == num2der && lista[0].CAPEIDES != 1 && lista[0].CAPEIDES != 9)
                    {
                        vpar.VALSAL.Add("1");
                        vpar.VALSAL.Add(Util.Serialize(lista));
                    }
                    else
                    {
                        vpar.VALSAL.Add("0");
                    }
                }
                else
                {
                    vpar.VALSAL.Add("0");
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
            }
            return vpar;
        }

        public bool EnviaCorreoNotificacionPedido(decimal idpedido, out string mensaje)
        {
            //string destinatario, cc, bcc, asunto, body;

            bool vpar;
            vpar = false;
            mensaje = "comentado 2019-02-14";//2019-02-14
            //2019-02-14
            ////List<object> listaeo = null;
            //List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
            //appLogica.appDB2 _appDB2 = null;
            //appLogica.MKT _appcs = null;

            //try
            //{
            //    destinatario = bcc = "";

            //    using (var context = new PEDIDOSEntitiesDB2())
            //    {
            //        lista = context.EnviaCorreoNotificacionPedido(idpedido);
            //    }
            //    //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);

            //    if (lista.Count > 0)
            //    {
            //        if (lista[0].CAPEIDES == 1)
            //        {
            //            mensaje = "Pedido no emitido";
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrWhiteSpace(lista[0].CAPEEMAI))
            //            {
            //                destinatario = lista[0].CAPEEMAI.Trim();
            //            }
            //            //pruebas
            //            destinatario = "ddk_sk@hotmail.com";
            //            _appDB2 = new appLogica.appDB2();
            //            RFEUSER usuario = _appDB2.ObtieneUsuarioDeFacturacion(lista[0].CAPEUSEM);
            //            if (usuario != null)
            //            {
            //                bcc = usuario.USERMAIL.Trim();
            //            }
            //            //pruebas
            //            bcc = "mlopez@incatops.com";
            //            if ( _appcs.PreparaCorreoNotificacionPedido(lista, out cc, out asunto, out body, out mensaje))
            //            {
            //                if ( _appcs.EnvioCorreo(destinatario, cc, bcc, asunto, body))
            //                {
            //                    vpar = true;
            //                }
            //                else
            //                {
            //                    mensaje = Mensajes.MENSAJE_CORREO_ERROR_ENVIO;
            //                }
            //            }
            //            else
            //            {
            //                //vpar.MENERR = mensaje;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        mensaje = Mensajes.MENSAJE_PEDIDO_NO_ENCONTRADO;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Util.EscribeLog(ex.Message);
            //    mensaje = ErrorGenerico(ex.Message);
            //    //throw ex;
            //}
            //finally
            //{
            //    if (_appDB2 != null)
            //    {
            //        _appDB2.Finaliza();
            //        _appDB2 = null;
            //    }
            //}
            return vpar;
        }//YA ESTA

        public RESOPE ObtieneParametros(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PEPARM> lista = null;
            try
            {
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtieneParametros();
                }
                //lista = Util.ParseEntityObject<appWcfService.PEPARM>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                    //vpar.VALSAL.Add()
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE ObtieneDetallePreparacionse(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> lista = null;
            try
            {
                decimal iddetallepedido = decimal.Parse(paramOperacion.VALENT[0]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtieneDetallePreparacionse(iddetallepedido);
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1"); //existe //0
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0"); //no existe
                }
                vpar.ESTOPE = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {

            }
            return vpar;
        }

        public RESOPE ValidaitemExcel(PAROPE paramOperacion)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            appWcfService.PEDEPE item = new appWcfService.PEDEPE();
            try
            {
                item = Util.Deserialize<appWcfService.PEDEPE>(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var obj = context.ValidaitemExcel(item.DEPECOAR, item.DEPEPART, item.DEPEALMA);//FirstOrDefault(x => x.LOTITEM.Equals(item.DEPECOAR) && x.LOTPARTI.Equals(item.DEPEPART) && x.LOTALM.Equals(item.DEPEALMA));
                    if (obj != null)
                    {
                        vpar.ESTOPE = true;
                    }
                }
                vpar.VALSAL = new List<string>();
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }//EF

        public RESOPE ValidaPreparacion(PAROPE paramOperacion) //uno por uno
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            appWcfService.PEDEPE item = new appWcfService.PEDEPE();
            try
            {
                item = Util.Deserialize<appWcfService.PEDEPE>(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var obj = context.ValidaPreparacion(item.DEPEIDDP);//PEDEPE.FirstOrDefault(x => x.DEPEIDDP == item.DEPEIDDP);
                    if (obj != null)
                    {
                        if (obj[0].DEPECAAT == 0 && obj[0].DEPEPEAT == 0)
                        {
                            vpar.ESTOPE = true;
                        }
                    }
                }
                vpar.VALSAL = new List<string>();
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE ValidaPreparacionList(PAROPE paramOperacion) //multiple
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.PEDEPE> Listadetalles = new List<appWcfService.PEDEPE>();
            bool validalista = true; //Flag para determinar si toda la lista es valida
            try
            {
                Listadetalles = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    foreach (var item in Listadetalles)
                    {
                        var det = context.ValidaPreparacion(item.DEPEIDDP);//.PEDEPE.FirstOrDefault(x => x.DEPEIDDP == item.DEPEIDDP);
                        if (det != null)
                        {
                            if (det[0].DEPECAAT > 0 || det[0].DEPEPEAT > 0)
                            {
                                validalista = false;
                                break;
                            }
                        }
                    }
                }
                vpar.VALSAL = new List<string>();
                if (validalista)
                {
                    vpar.ESTOPE = true;
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE CambiaEstaList(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.PECAPE> lista = null;
            List<appWcfService.PEPARM> listapar = new List<appWcfService.PEPARM>();
            appWcfService.PEPARM par = new appWcfService.PEPARM();
           // appLogica.appDB2 _appDB2 = null;
            try
            {
                decimal estado = 0;
                decimal bultos = 0;
                decimal tade = 0;
                lista = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
                estado = decimal.Parse(paramOperacion.VALENT[1]);
                string usuario = paramOperacion.VALENT[2];
                if (estado == 4)
                {
                    bultos = Convert.ToDecimal(paramOperacion.VALENT[3]);
                    tade = Convert.ToDecimal(paramOperacion.VALENT[4]);
                }

                string mensajecorreo;
                foreach (var item in lista)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        var ped = context.PECAPE_Find(item.CAPEIDCP);//.PECAPE.Find(item.CAPEIDCP);
                        ped.CAPEIDES = estado;
                        switch (estado.ToString())
                        {
                            case "2": //Emitido
                                ped.CAPEUSEM = usuario;
                                ped.CAPEFHEM = DateTime.Now;
                                context.PECAPE_UPDATE_ESTADO_EMITIDO(ped.CAPEIDCP, pCAPEUSEM: ped.CAPEUSEM, pCAPEFHEM: ped.CAPEFHEM, pCAPEIDES: ped.CAPEIDES);
                                break;
                            case "3": //En preparacion
                                ped.CAPEUSIP = usuario;
                                ped.CAPEFHIP = DateTime.Now;
                                context.PECAPE_UPDATE_ESTADO_ENPREPARACION(ped.CAPEIDCP, pCAPEUSIP: ped.CAPEUSIP, pCAPEFHIP: ped.CAPEFHIP, pCAPEIDES: ped.CAPEIDES);
                                break;
                            case "4": //En espera de aprobacion
                                if (ped.CAPEUSAP != null) //Si viene de reabrir completado almacen
                                {
                                    ped.CAPEUSMO = usuario;
                                    ped.CAPEFEMO = DateTime.Now;
                                    ped.CAPEUSAP = null;
                                    ped.CAPEFEAP = null;
                                    context.PECAPE_UPDATE_ESTADO_ESP_APROB_REABIERTO(ped.CAPEIDCP, ped.CAPEUSMO, ped.CAPEFEMO, ped.CAPEUSAP,  ped.CAPEFEAP,  ped.CAPEIDES);
                                }
                                else
                                {
                                    ped.CAPEUSFP = usuario;
                                    ped.CAPEFHFP = DateTime.Now;
                                    ped.CAPENUBU = bultos;
                                    ped.CAPETADE = tade;
                                    context.PECAPE_UPDATE_ESTADO_ESP_APROBACION(ped.CAPEIDCP, pCAPEUSFP: ped.CAPEUSFP, pCAPEFHFP: ped.CAPEFHFP, pCAPENUBU: ped.CAPENUBU, pCAPETADE: ped.CAPETADE, pCAPEIDES: ped.CAPEIDES);
                                }
                                break;

                            case "5":// APROBADO 
                                ped.CAPEUSAP = usuario;
                                ped.CAPEFEAP = DateTime.Now;
                                context.PECAPE_UPDATE_ESTADO_APROBADO(ped.CAPEIDCP, pCAPEUSAP: ped.CAPEUSAP, pCAPEFEAP: ped.CAPEFEAP, pCAPEIDES: ped.CAPEIDES);
                                break;
                            case "9"://ANULADOS
                                ped.CAPEUSMO = usuario;
                                ped.CAPEFEMO = DateTime.Now;
                                anularReservasPedidoAnulado(context, item);
                                context.PECAPE_UPDATE_ESTADO_ANULADO(ped.CAPEIDCP, pCAPEUSMO: ped.CAPEUSMO, pCAPEFEMO: ped.CAPEFEMO,  pCAPEIDES: ped.CAPEIDES);
                                break;
                        }
                        //context.SaveChanges();
                        listapar = context.ObtieneParametros();//.PEPARM.ToList();
                    }
                }
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista)); //revisar
                }
                else
                {
                    vpar.VALSAL.Add("0");

                }
                vpar.ESTOPE = true;
                //Envio de correo de acuerdo a la tabla de parametros
                if (estado != 1 && estado != 9)
                {
                    switch (estado.ToString())
                    {
                        case "2": //Emitidos"
                            par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_EMITIR_PEDIDO);
                            break;
                        case "3": //En preparacion
                            par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_INICIAR_PREPARACION_PEDIDO);
                            break;
                        case "4": //Fin Preparacion
                            par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_FINALIZAR_PREPARACION_PEDIDO);
                            break;
                        case "5": //Despachado
                            par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_DESPACHAR_PEDIDO);
                            break;
                    }

                    if (par != null && par.PARMVAPA.Equals("1"))
                    {
                        foreach (var item in lista)
                        {
                            if (!EnviaCorreoNotificacionPedido(item.CAPEIDCP, out mensajecorreo)) //if (!_appDB2.EnviaCorreoNotificacionPedido(item.CAPEIDCP, out mensajecorreo))
                            {
                                Util.EscribeLog(mensajecorreo);
                            }
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {

            }
            return vpar;
        }

        private void anularReservasPedidoAnulado(PEDIDOSEntitiesDB2 context, appWcfService.PECAPE pedido)
        {
            //appLogica.appDB2 _appDB2 = null;
            //_appDB2 = new appLogica.appDB2();

            var listdet = context.DetallexCabe(pedido.CAPEIDCP);//.PEDEPE.Where(x => x.DEPEIDCP == pedido.CAPEIDCP).ToList();
            foreach (var item in listdet)
            {
                //  DMA, SE SOLICITO REALIZAR LA RESERVA CON EL ID DEL DETALLE DEL PEDIDO PARA EVITAR RESERVAS DUPLICADAS, (det.DEPEIDDP)

                ///descomentar
                //_appDB2.generaReserva(false, "X", Convert.ToInt32(item.DEPEIDDP).ToString(), Convert.ToString(item.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");
                ///descomentar

                //_appDB2.generaReserva(false, "X", Convert.ToInt32(pedido.CAPEIDCP).ToString(), Convert.ToString(item.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");
            }
        }

        public RESOPE GeneraPreguia(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            List<appWcfService.PEPARM> listapar = null; //Revisar si es APPWCFSERVICE O EFMODELO
            appWcfService.PEPARM par; //Revisar si es APPWCFSERVICE O EFMODELO
            List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
            //appLogica.appDB2 _appDB2 = null;
            string codprovtrans, estabpart, serieguiadefault;
            string mensajecorreo;
            decimal tped = 0; //TIPO DE PEDIDO PARA LA GUIA
            decimal nubu = 0;// numero de bultos;
            decimal tade = 0;
            decimal estadest = 0;
            try
            {
                decimal idpedido, idtipdoc;
                int secuemcia;
                string usuario, usuariopedido;
                idpedido = decimal.Parse(paramOperacion.VALENT[0]);
                usuario = paramOperacion.VALENT[1];
                tped = decimal.Parse(paramOperacion.VALENT[2]);
                nubu = Convert.ToDecimal(paramOperacion.VALENT[3]);
                tade = Convert.ToDecimal(paramOperacion.VALENT[4]);
                idtipdoc = 0;
                if (paramOperacion.VALENT.Count > 5)
                {
                    idtipdoc = Convert.ToDecimal(paramOperacion.VALENT[5]); //20180418
                }

                usuariopedido = "";
                //_appDB2 = new appLogica.appDB2();

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    lista = context.ObtienePedidoConsulta(idpedido);//.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
                    secuemcia = 0;
                    foreach (appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result item in lista)
                    {
                        String descripcionitem;
                        var detPed = context.PEDEPE_Find(item.DEPEIDDP);//.PEDEPE.Find(item.DEPEIDDP);
                        secuemcia++;
                        descripcionitem = descripcionItem(false, item.DEPECOAR, "", item.DEPECONT, "N", Convert.ToString(secuemcia), Convert.ToInt32(item.DEPECAAT).ToString(), "3");
                        //_appDB2.descripcionItem(false, item.DEPECOAR, "", item.DEPECONT, "N", Convert.ToString(secuemcia), Convert.ToInt32(item.DEPECAAT).ToString(), "3");
                        item.DEPEDSAR = descripcionitem;
                        detPed.DEPEDSAR = descripcionitem;

                        //2018/15/12
                        context.PEDEPE_UPDATE(item.DEPEIDDP,pDEPEDSAR: descripcionitem);//pCAPEUSIP: ped.CAPEUSIP

                        //20180220
                        item.CAPENUBU = nubu;
                        item.CAPETIPO = tped;
                        item.CAPETADE = tade;
                        if (idtipdoc != 0)
                        {
                            item.CAPEIDTD = idtipdoc; //20180418

                            //2018/15/12
                            context.PECAPE_UPDATE_GENERA_PRE_GUIA_1(item.CAPEIDCP, pCAPENUBU: item.CAPENUBU, pCAPETIPO: item.CAPETIPO, pCAPETADE: item.CAPETADE, pCAPEIDTD: item.CAPEIDTD);
                        }
                        else
                        {
                            idtipdoc = item.CAPEIDTD;
                        }
                        usuariopedido = item.CAPEUSCR.Trim();
                    }

                    listapar = context.ObtieneParametros();//.PEPARM.ToList();
                    //context.SaveChanges();
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
                vpar.VALSAL = new List<string>();

                //codigo incalpaca
                par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_INCALPACA);
                if (lista[0].CAPEIDCL == par.PARMVAPA.Trim()) //es incalpaca
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_INCALPACA);
                    codprovtrans = par.PARMVAPA.Trim();
                }
                else
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_OTROS);
                    codprovtrans = par.PARMVAPA.Trim();
                }
                //establecimiento partida
                par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_ESTAB_PARTIDA);
                estabpart = par.PARMVAPA.Trim();

                //VENTA = 1; //GR id tipo 3, motivo 35
                //TRANSF_ALMACENES = 2; //GR id tipo 3, motivo 36, almacen destino Lima 33
                //TRANSF_INTERNA = 3; //T/I id tipo 11, motivo 36, almacen destino tienda 35
                //REMATES = 4; //T/I id tipo 11, motivo 36, almacen destino remates 75
                //Serie guia default
                if (tped == Constantes.VENTA || tped == Constantes.CONSIGNACION) //20180418
                {
                    //20181123
                    if (idtipdoc != Constantes.ID_TIPO_DOC_GUIA && idtipdoc != Constantes.ID_TIPO_DOC_NE)
                    {
                        idtipdoc = Constantes.ID_TIPO_DOC_GUIA;
                    }
                    if (idtipdoc == Constantes.ID_TIPO_DOC_GUIA)
                    {
                        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_GUIA_DEFAULT);
                    }
                    else
                    {
                        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_NOTENTR_DEFAULT);
                    }
                }
                else if (tped == Constantes.TRANSF_ALMACENES)
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_GUIA_DEFAULT);
                }
                else
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_TI_DEFAULT);
                }
                serieguiadefault = par.PARMVAPA.Trim();

                //establ destino buscar parametro por tipo pedido
                if (tped == Constantes.TRANSF_ALMACENES)
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_ESTABLEC);
                    estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
                }
                else if (tped == Constantes.TRANSF_INTERNA)
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_INTERNO);
                    estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
                }
                else if (tped == Constantes.REMATES)
                {
                    par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_REMATE);
                    estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
                }
                //20180411
                if (string.IsNullOrWhiteSpace(usuariopedido))
                {
                    usuariopedido = usuario;
                }
                ///descomentar
                //vpar = _appDB2.GeneraPreguia(lista, serieguiadefault, usuariopedido.Trim(), codprovtrans, estabpart, estadest);
                ///descomentar

                if (vpar.ESTOPE == true)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        var ped = context.PECAPE_Find(idpedido);//.PECAPE.Find(idpedido);
                        ped.CAPEIDES = 5;
                        ped.CAPEUSAP = usuario;
                        ped.CAPEFEAP = DateTime.Now;
                        ped.CAPENUBU = nubu;
                        ped.CAPETADE = tade;
                        ped.CAPETIPO = tped;
                        ped.CAPEIDTD = idtipdoc; //20180418
                        //20180220
                        ped.CAPEDOCO = Convert.ToDecimal(vpar.VALSAL[0]);

                        //2018-15-12
                        context.PECAPE_UPDATE_GENERA_PRE_GUIA_2(idpedido, pCAPEIDES: ped.CAPEIDES, pCAPEUSAP: ped.CAPEUSAP, pCAPEFEAP: ped.CAPEFEAP, pCAPENUBU: ped.CAPENUBU, 
                            pCAPETADE: ped.CAPETADE, pCAPETIPO: ped.CAPETIPO, pCAPEIDTD: ped.CAPEIDTD, pCAPEDOCO: ped.CAPEDOCO);
                        //context.SaveChanges();
                        if (tped == Constantes.VENTA)
                        {
                            par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_DESPACHAR_PEDIDO);
                            if (par != null && par.PARMVAPA.Equals("1"))
                            {
                                if (!EnviaCorreoNotificacionPedido(idpedido, out mensajecorreo))//if (!_appDB2.EnviaCorreoNotificacionPedido(idpedido, out mensajecorreo))
                                {
                                    Util.EscribeLog(mensajecorreo);
                                }
                            }
                        }
                    }
                }

                //if (lista.Count > 0)
                //{
                //    vpar.VALSAL.Add("1");
                //    //vpar.VALSAL.Add(Util.Serialize(lista));
                //}
                //else
                //{
                //    vpar.VALSAL.Add("0");

                //}
                //vpar.ESTOPE = true;

            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {
                //if (_appDB2 != null)
                //{
                    Finaliza();// _appDB2.Finaliza();
                                       //   _appDB2 = null;
                                       // }
            }
            return vpar;
        }

        public RESOPE GuardaPrioridad(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.PECAPE> lista = null;
            try
            {
                lista = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
                string usuario = paramOperacion.VALENT[1];

                foreach (var item in lista)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        var ped = context.PECAPE_Find(item.CAPEIDCP);//.PECAPE.Find(item.CAPEIDCP);

                        ped.CAPEPRIO = item.CAPEPRIO;
                        context.PECAPE_UPDATE_NUM_PRIO(item.CAPEIDCP, pCAPEPRIO: ped.CAPEPRIO);
                        if (item.CAPEEPRI == 1 && (item.CAPEUSPR == null || usuario == item.CAPEUSEM))
                        {
                            ped.CAPEEPRI = item.CAPEEPRI;
                            ped.CAPEFEPR = DateTime.Now;
                            ped.CAPEUSPR = usuario;
                            context.PECAPE_UPDATE_ES_PRIO(item.CAPEIDCP, pCAPEEPRI: ped.CAPEEPRI, pCAPEFEPR: ped.CAPEFEPR, pCAPEUSPR: ped.CAPEUSPR);
                        }
                        //context.SaveChanges();
                    }
                }
                //vpar.VALSAL = new List<string>();
                //if (lista.Count > 0)
                //{
                //    vpar.VALSAL.Add("1");
                //    vpar.VALSAL.Add(Util.Serialize(lista));
                //}
                //else
                //{
                //    vpar.VALSAL.Add("0");

                //}
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {

            }
            return vpar;
        }//------------------------------------------------------------

        public RESOPE ModificaCasillero(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            appWcfService.PECASI Casillero;

            //List<object> listaeo = null;
            try
            {
                Casillero = Util.Deserialize<appWcfService.PECASI>(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var cas = context.PECASI_Find(Casillero.CASICOCA);//.PECASI.Find(Casillero.CASICOCA);
                    if (cas != null)
                    {
                        cas.CASICAPA = Casillero.CASICAPA;
                        cas.CASIALTU = Casillero.CASIALTU;
                        cas.CASILARG = Casillero.CASILARG;
                        cas.CASIANCH = Casillero.CASIANCH;
                        cas.CASIUSMO = Casillero.CASIUSMO;
                        cas.CASIFEMO = DateTime.Now;
                        context.PECASI_UPDATE(cas.CASICOCA, pCASIESTA: cas.CASIESTA, pCASIIDPA: cas.CASIIDPA ,  pCASIIDNI: cas.CASIIDNI, pCASIIDCO: cas.CASIIDCO,
                                pCASICAPA: cas.CASICAPA, pCASIALTU: cas.CASIALTU, pCASIANCH: cas.CASIANCH , pCASIUSMO: cas.CASIUSMO, pCASIFEMO: cas.CASIFEMO
                                , pCASILARG: cas.CASILARG);
                        //context.SaveChanges();
                        vpar.ESTOPE = true;
                    }
                }
                vpar.VALSAL = new List<string>();
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE DeshabilitaCasillero(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.PECASI> listcasilleros = null;
            string usuario;
            try
            {
                listcasilleros = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[0]);
                usuario = paramOperacion.VALENT[1];
                foreach (var item in listcasilleros)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        var cas = context.PECASI_Find(item.CASICOCA);//.PECASI.Find(item.CASICOCA);
                        if (cas != null)
                        {
                            cas.CASIESTA = 0;
                            cas.CASIUSMO = usuario;
                            cas.CASIFEMO = DateTime.Now;
                            context.PECASI_UPDATE(cas.CASICOCA, pCASIIDPA: cas.CASIIDPA, pCASIESTA: cas.CASIESTA, pCASIIDNI: cas.CASIIDNI, pCASIIDCO: cas.CASIIDCO,
                                pCASICAPA: cas.CASICAPA, pCASIALTU: cas.CASIALTU, pCASIANCH: cas.CASIANCH
                                , pCASILARG: cas.CASILARG, pCASIUSMO: cas.CASIUSMO, pCASIFEMO: cas.CASIFEMO);//pCASIUSCR: cas.CASIUSCR, pCASIFECR: cas.CASIFECR, 
                            //context.SaveChanges();
                            vpar.ESTOPE = true;
                        }
                        else
                        {
                            vpar.ESTOPE = false;
                        }
                    }
                }
                vpar.VALSAL = new List<string>();
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE DeshabilitaColumna(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            decimal columna = 0;
            string usuario;
            decimal pasillo;
            List<appWcfService.PECASI> lista = null;

            try
            {
                columna = Decimal.Parse(paramOperacion.VALENT[0]);
                pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
                usuario = paramOperacion.VALENT[2];
                bool activa = Boolean.Parse(paramOperacion.VALENT[3]);
                lista = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[4]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            var cas = context.PECASI_Find(item.CASICOCA);//.PECASI.Find(item.CASICOCA);
                            if (cas != null)
                            {

                                if (activa)
                                {
                                    cas.CASIESTA = 1;
                                }
                                else cas.CASIESTA = 0;
                                cas.CASIUSMO = usuario;
                                cas.CASIFEMO = DateTime.Now;
                                context.PECASI_UPDATE(cas.CASICOCA, pCASIESTA: cas.CASIESTA, pCASIIDPA: cas.CASIIDPA, pCASIIDNI: cas.CASIIDNI, pCASIIDCO: cas.CASIIDCO,
                                pCASICAPA: cas.CASICAPA, pCASIALTU: cas.CASIALTU, pCASIANCH: cas.CASIANCH
                                , pCASILARG: cas.CASILARG);// pCASIUSCR: cas.CASIUSCR, pCASIFECR: cas.CASIFECR,
                            }
                        }
                    }
                    var colu = context.PECOLU_Find(columna, pasillo);//.PECOLU.Find(columna, pasillo);
                    if (colu != null)
                    {
                        if (activa)
                        {
                            colu.COLUESTA = 1;
                        }
                        else colu.COLUESTA = 0;
                        colu.COLUUSMO = usuario;
                        colu.COLUFEMO = DateTime.Now;
                        vpar.ESTOPE = true;
                        context.PECOLU_UPDATE(pCOLUIDCO: colu.COLUIDCO, pCOLUIDPA: colu.COLUIDPA, pCOLUESTA: colu.COLUESTA, pCOLUUSMO: colu.COLUUSMO, pCOLUFEMO: colu.COLUFEMO);//, pCOLUUSCR: colu.COLUUSCR, pCOLUFECR: colu.COLUFECR
                    }
                    else
                    {
                        vpar.ESTOPE = false;
                    }
                    //context.SaveChanges();
                }
                vpar.VALSAL = new List<string>();
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE DeshabilitaNivel(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            string nivel = "";
            string usuario;
            decimal pasillo;
            List<appWcfService.PECASI> lista = null;

            try
            {
                nivel = paramOperacion.VALENT[0];
                pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
                usuario = paramOperacion.VALENT[2];
                bool activa = Boolean.Parse(paramOperacion.VALENT[3]);
                lista = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[4]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            var cas = context.PECASI_Find(item.CASICOCA);//.PECASI.Find(item.CASICOCA);
                            if (cas != null)
                            {

                                if (activa)
                                {
                                    cas.CASIESTA = 1;
                                }
                                else cas.CASIESTA = 0;
                                cas.CASIUSMO = usuario;
                                cas.CASIFEMO = DateTime.Now;
                                context.PECASI_UPDATE(cas.CASICOCA, pCASIESTA: cas.CASIESTA, pCASIIDPA: cas.CASIIDPA, pCASIIDNI: cas.CASIIDNI, pCASIIDCO: cas.CASIIDCO,
                                pCASICAPA: cas.CASICAPA, pCASIALTU: cas.CASIALTU, pCASIANCH: cas.CASIANCH
                                , pCASILARG: cas.CASILARG, pCASIUSMO: cas.CASIUSMO, pCASIFEMO: cas.CASIFEMO);//, pCASIUSCR: cas.CASIUSCR, pCASIFECR: cas.CASIFECR
                            }
                        }
                    }
                    var nive = context.PENIVE_Find(nivel, pasillo);//.PENIVE.Find(nivel, pasillo);
                    if (nive != null)
                    {
                        if (activa)
                        {
                            nive.NIVEESTA = 1;
                        }
                        else nive.NIVEESTA = 0;
                        nive.NIVEUSMO = usuario;
                        nive.NIVEFEMO = DateTime.Now;
                        context.PENIVE_UPDATE(pNIVEIDNI: nive.NIVEIDNI, pNIVEIDPA: nive.NIVEIDPA, pNIVEESTA: nive.NIVEESTA, pNIVEUSMO: nive.NIVEUSMO, pNIVEFEMO: nive.NIVEFEMO);//, pNIVEUSCR: nive.NIVEUSCR, pNIVEFECR: nive.NIVEFECR
                        vpar.ESTOPE = true;
                    }
                    else
                    {
                        vpar.ESTOPE = false;
                    }
                    //context.SaveChanges();
                }
                vpar.VALSAL = new List<string>();
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE DeshabilitaGeneral(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            string usuario;
            decimal pasillo;
            List<appWcfService.PECASI> listaCasillero = null;
            List<appWcfService.PENIVE> listaNivel = null;
            List<appWcfService.PECOLU> listColumna = null;
            try
            {
                pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
                usuario = paramOperacion.VALENT[1];
                listaCasillero = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[2]);
                listaNivel = Util.Deserialize<List<appWcfService.PENIVE>>(paramOperacion.VALENT[3]);
                listColumna = Util.Deserialize<List<appWcfService.PECOLU>>(paramOperacion.VALENT[4]);
                bool activa = Boolean.Parse(paramOperacion.VALENT[5]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    if (listaCasillero != null)
                    {
                        foreach (var item in listaCasillero)
                        {
                            var cas = context.PECASI_Find(item.CASICOCA);//.PECASI.Find(item.CASICOCA);
                            if (cas != null)
                            {
                                if (activa)
                                {
                                    cas.CASIESTA = 1;
                                }
                                else cas.CASIESTA = 0;
                                cas.CASIUSMO = usuario;
                                cas.CASIFEMO = DateTime.Now;
                                context.PECASI_UPDATE(cas.CASICOCA, pCASIESTA: cas.CASIESTA, pCASIIDPA: cas.CASIIDPA, pCASIIDNI: cas.CASIIDNI, pCASIIDCO: cas.CASIIDCO,
                                pCASICAPA: cas.CASICAPA, pCASIALTU: cas.CASIALTU, pCASIANCH: cas.CASIANCH
                                , pCASILARG: cas.CASILARG, pCASIUSMO: cas.CASIUSMO, pCASIFEMO: cas.CASIFEMO);//, pCASIUSCR: cas.CASIUSCR, pCASIFECR: cas.CASIFECR
                            }
                        }
                    }
                    if (listaNivel != null)
                    {
                        foreach (var item in listaNivel)
                        {
                            var nive = context.PENIVE_Find(item.NIVEIDNI, pasillo);//PENIVE.Find(item.NIVEIDNI, pasillo);
                            if (nive != null)
                            {
                                if (activa)
                                {
                                    nive.NIVEESTA = 1;
                                }
                                else nive.NIVEESTA = 0;
                                nive.NIVEUSMO = usuario;
                                nive.NIVEFEMO = DateTime.Now;
                                context.PENIVE_UPDATE(pNIVEIDNI: nive.NIVEIDNI,pNIVEIDPA: nive.NIVEIDPA, pNIVEESTA: nive.NIVEESTA, pNIVEUSMO: nive.NIVEUSMO, pNIVEFEMO: nive.NIVEFEMO);//, pNIVEUSCR: nive.NIVEUSCR, pNIVEFECR: nive.NIVEFECR
                            }
                        }
                    }
                    if (listColumna != null)
                    {
                        foreach (var item in listColumna)
                        {
                            var colu = context.PECOLU_Find(item.COLUIDCO, pasillo);//.PECOLU.Find(item.COLUIDCO, pasillo);
                            if (colu != null)
                            {
                                if (activa)
                                {
                                    colu.COLUESTA = 1;
                                }
                                else colu.COLUESTA = 0;
                                colu.COLUUSMO = usuario;
                                colu.COLUFEMO = DateTime.Now;
                                context.PECOLU_UPDATE(pCOLUIDCO: colu.COLUIDCO, pCOLUIDPA: colu.COLUIDPA, pCOLUESTA: colu.COLUESTA, pCOLUUSMO: colu.COLUUSMO, pCOLUFEMO: colu.COLUFEMO);//, pCOLUUSCR: colu.COLUUSCR, pCOLUFECR: colu.COLUFECR
                            }
                        }
                    }
                    var pasi = context.PEPASI_Find(pasillo);//.PEPASI.Find(pasillo);
                    if (pasi != null)
                    {
                        if (activa)
                        {
                            pasi.PASIESTA = 1;
                        }
                        else pasi.PASIESTA = 0;
                        pasi.PASIUSMO = usuario;
                        pasi.PASIFEMO = DateTime.Now;
                        context.PEPASI_UPDATE(pasi.PASIIDPA, pPASIESTA: pasi.PASIESTA, pPASIUSMO: pasi.PASIUSMO, pPASIFEMO: pasi.PASIFEMO);//, pPASIUSCR: pasi.PASIUSCR, pPASIFECR: pasi.PASIFECR);
                    }
                    vpar.ESTOPE = true;
                    //context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE EliminaNivel(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            string nivel = "";
            decimal pasillo = 0;
            try
            {
                pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
                nivel = paramOperacion.VALENT[1];
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var nive = context.PENIVE_Find(nivel, pasillo);//.PENIVE.Find(nivel, pasillo);
                    if (nive != null)
                    {
                        context.PENIVE_DELETE(nive.NIVEIDNI, nive.NIVEIDPA);//.PENIVE.Remove(nive);
                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE EliminaColumna(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            decimal columna = 0;
            decimal pasillo = 0;
            try
            {
                pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
                columna = Decimal.Parse(paramOperacion.VALENT[1]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var colu = context.PECOLU_Find(columna, pasillo);//.PECOLU.Find(columna, pasillo);
                    if (colu != null)
                    {
                        context.PECOLU_DELETE(colu.COLUIDCO, colu.COLUIDPA);//.PECOLU.Remove(colu);
                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE EliminaPasillo(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            decimal pasillo = 0;
            try
            {
                pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var pasi = context.PEPASI_Find(pasillo);//.PEPASI.Find(pasillo);
                    if (pasi != null)
                    {
                        context.PEPASI_DELETE(pasi.PASIIDPA);//.PEPASI.Remove(pasi);
                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE EliminaDetalle(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.PEDEPE> lista = null;
            try
            {
                lista = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[0]);
                foreach (var item in lista)
                {
                    using (var context = new PEDIDOSEntitiesDB2())
                    {
                        var det = context.PEDEPE_Find(item.DEPEIDDP);//.PEDEPE.Find(item.DEPEIDDP);
                        context.PEDEPE_DELETE(det.DEPEIDDP, det.DEPEIDCP);//.PEDEPE.Remove(det);
                        //context.SaveChanges();
                    }
                }
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");
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

            }
            return vpar;
        }

        public RESOPE AgregaPasi(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //appWcfService.PEPASI column = null;
            try
            {
                //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
                string usuario = paramOperacion.VALENT[0].Trim();
                decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var det = context.PEPASI_Find(pasillo + 1);//.PEPASI.Find(pasillo + 1);
                    if (det == null)
                    {
                        //det = new EFModelo.PEPASI();
                        det = new appWcfService.PEPASI();
                        det.PASIIDPA = pasillo + 1;
                        det.PASIFECR = DateTime.Now;
                        det.PASIUSCR = usuario;
                        det.PASIESTA = 1;
                        //context.PEPASI.Add(det);
                        context.PEPASI_INSERT(det.PASIIDPA, pPASIESTA: det.PASIESTA, pPASIFECR: det.PASIFECR, pPASIUSCR: det.PASIUSCR);

                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE AgregaNivel(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //appWcfService.PEPASI column = null;
            try
            {
                //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
                string usuario = paramOperacion.VALENT[0].Trim();
                decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
                string nivel = paramOperacion.VALENT[2];

                if (nivel.Equals(""))
                {
                    nivel = "@";
                }

                int num;
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    //var nivel = context.PENIVE.Find();
                    //var ped = context.PENIVE.Max(x => x.NIVEIDNI);
                    num = Encoding.ASCII.GetBytes(nivel)[0];
                    num++;
                    byte[] bytes2 = BitConverter.GetBytes(num);
                    string a = Encoding.ASCII.GetString(bytes2);
                    var det = context.PENIVE_Find((a[0]).ToString(), pasillo);//.PENIVE.Find((num).ToString(), pasillo);
                    
                    if (det == null)
                    {
                        //det = new EFModelo.PENIVE();
                        det = new appWcfService.PENIVE();
                        det.NIVEIDNI = (a[0]).ToString();
                        det.NIVEIDPA = pasillo;
                        det.NIVEESTA = 1;
                        det.NIVEUSCR = usuario;
                        det.NIVEFECR = DateTime.Now;
                        //context.PENIVE.Add(det);
                        context.PENIVE_INSERT(det.NIVEIDNI, det.NIVEIDPA, pNIVEESTA: det.NIVEESTA, pNIVEUSCR: det.NIVEUSCR, pNIVEFECR: det.NIVEFECR);
                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE AgregaColumna(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //appWcfService.PEPASI column = null;
            try
            {
                //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
                string usuario = paramOperacion.VALENT[0].Trim();
                decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
                decimal columna = Decimal.Parse(paramOperacion.VALENT[2]);

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var det = context.PECOLU_Find(columna + 1, pasillo);//.PECOLU.Find(columna + 1, pasillo);

                    if (det == null)
                    {
                        //det = new EFModelo.PECOLU();
                        det = new appWcfService.PECOLU();
                        det.COLUIDCO = columna + 1;
                        det.COLUIDPA = pasillo;
                        det.COLUESTA = 1;
                        det.COLUUSCR = usuario;
                        det.COLUFECR = DateTime.Now;
                        //context.PECOLU.Add(det);
                        context.PECOLU_INSERT(det.COLUIDCO, det.COLUIDPA, pCOLUESTA: det.COLUESTA, pCOLUUSCR: det.COLUUSCR, pCOLUFECR: det.COLUFECR);
                    }
                    //context.SaveChanges();
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

            }
            return vpar;
        }

        public RESOPE AgregaCasillero(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            decimal idpasillo, idcolumna;
            string idnivel, usuario;

            //List<object> listaeo = null;
            try
            {
                idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
                idnivel = paramOperacion.VALENT[1];
                idcolumna = decimal.Parse(paramOperacion.VALENT[2]);
                usuario = paramOperacion.VALENT[3].Trim();
                string codcas = ("P" + idpasillo.ToString().PadLeft(2, '0') + idnivel + idcolumna.ToString().PadLeft(2, '0')).Trim();

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var casillero = context.PECASI_Find(codcas);//.PECASI.Find(codcas);
                    if (casillero != null)
                    {
                        if (casillero.CASIESTA == 0)
                        {
                            casillero.CASIESTA = 1;
                            casillero.CASIUSMO = usuario;
                            casillero.CASIFEMO = DateTime.Now;
                            casillero.CASICAPA = 0;
                            casillero.CASIALTU = 0;
                            casillero.CASIANCH = 0;
                            casillero.CASILARG = 0;
                            context.PECASI_UPDATE(casillero.CASICOCA, pCASIESTA: casillero.CASIESTA, pCASIIDPA: casillero.CASIIDPA, pCASIIDNI: casillero.CASIIDNI, pCASIIDCO: casillero.CASIIDCO,
                                pCASICAPA: casillero.CASICAPA, pCASIALTU: casillero.CASIALTU, pCASIANCH: casillero.CASIANCH
                                , pCASILARG: casillero.CASILARG,pCASIFEMO:casillero.CASIFEMO,pCASIUSMO:casillero.CASIUSMO);//, pCASIUSCR: casillero.CASIUSCR, pCASIFECR: casillero.CASIFECR,
                        }
                        else
                        {
                            vpar.ESTOPE = false;
                            //goto Guardacambios;
                        }
                    }
                    else
                    {
                        //casillero = new EFModelo.PECASI();
                        casillero = new appWcfService.PECASI();
                        casillero.CASICOCA = codcas;
                        casillero.CASIIDPA = idpasillo;
                        casillero.CASIIDNI = idnivel;
                        casillero.CASIIDCO = idcolumna;
                        casillero.CASIUSCR = usuario;
                        casillero.CASIFECR = DateTime.Now;
                        casillero.CASIESTA = 1;
                        casillero.CASICAPA = 0;
                        casillero.CASIALTU = 0;
                        casillero.CASIANCH = 0;
                        casillero.CASILARG = 0;
                        //context.PECASI.Add(casillero);
                        context.PECASI_INSERT(casillero.CASICOCA, pCASIIDPA: casillero.CASIIDPA, pCASIIDNI: casillero.CASIIDNI, pCASIIDCO: casillero.CASIIDCO,
                            pCASIUSCR: casillero.CASIUSCR, pCASIFECR: casillero.CASIFECR, pCASIESTA: casillero.CASIESTA, pCASICAPA: casillero.CASICAPA,
                            pCASIALTU: casillero.CASIALTU, pCASIANCH: casillero.CASIANCH, pCASILARG: casillero.CASILARG);
                    }
                    vpar.ESTOPE = true;
               // Guardacambios:
                    //context.SaveChanges();

                }
                vpar.VALSAL = new List<string>();
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
            }
            return vpar;
        }

        public RESOPE GuardaPedido(PAROPE paramOperacion)
        {
            RESOPE vpar;
            decimal idpedido, idestado, idcabpedido;
            string numeropedido;

            ///2018-12-29 (1- se agrega, 2 - se actualiza) MF
            int tipo = 0;
            ///

            vpar = new RESOPE() { ESTOPE = false };
            idpedido = 0;
            idestado = 0;
            idcabpedido = 0;

            appWcfService.PECAPE pedido;
            List<appWcfService.PEDEPE> lista = null;
            List<appWcfService.PEDEPE> listaborrados = null;

            //appLogica.appDB2 _appDB2 = null;
            try
            {
                int secuencia;
               // _appDB2 = new appLogica.appDB2();
                pedido = Util.Deserialize<appWcfService.PECAPE>(paramOperacion.VALENT[0]);
                lista = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[1]);

                //articulo = String.IsNullOrWhiteSpace(partida) ? null : articulo;
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var ped = context.PECAPE_Find(pedido.CAPEIDCP);//.PECAPE.Find(pedido.CAPEIDCP);
                    if (ped == null)
                    {
                        //ped = new EFModelo.PECAPE();
                        ped = new appWcfService.PECAPE();
                        ped.CAPEFECR = DateTime.Now;
                        ped.CAPEUSCR = pedido.CAPEUSCR;
                        ped.CAPESERI = pedido.CAPESERI;
                        ped.CAPEIDES = Constantes.ID_ESTADO_CREADO;
                        //context.PECAPE.Add(ped);
                        //tipo 1 se añade una nueva cabecera
                        tipo = 1;
                        //2018-12-29
                        ped.CAPEIDCP = pedido.CAPEIDCP;
                    }
                    else
                    {
                        ped.CAPEFEMO = DateTime.Now;
                        ped.CAPEUSMO = pedido.CAPEUSCR; //el usuario peude enviarse aqui siempre, no es necesario en el mod
                        if (ped.CAPEIDES == 1 && pedido.CAPEIDES == 1)
                        {
                            ped.CAPEIDES = Constantes.ID_ESTADO_CREADO;
                        }
                        //tipo 2 se modifica una cabecera
                        tipo = 2;
                    }
                    ped.CAPEIDCL = pedido.CAPEIDCL;
                    ped.CAPENOTG = pedido.CAPENOTG;
                    ped.CAPENOTI = pedido.CAPENOTI;
                    ped.CAPEFECH = pedido.CAPEFECH;
                    ped.CAPEDIRE = pedido.CAPEDIRE;
                    ped.CAPEEMAI = pedido.CAPEEMAI;
                    ped.CAPETIPO = pedido.CAPETIPO;
                    ped.CAPEIDTD = pedido.CAPEIDTD;
                    ped.CAPEDEST = pedido.CAPEDEST;
                    //////////elimna borrados de la grilla
                    listaborrados = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[2]);
                    foreach (var item in listaborrados)
                    {
                        var det = context.PEDEPE_Find(item.DEPEIDDP);//.PEDEPE.Find(item.DEPEIDDP);
                        if (det != null)
                        {
                            context.PEDEPE_DELETE(det.DEPEIDDP, det.DEPEIDCP);//.PEDEPE.Remove(det);
                        }
                    }
                    //////
                    //context.SaveChanges();
                    ///2018-12-29 añadiendo o actualizando
                    if (tipo == 1)
                    {
                        idpedido = context.PECAPE_INSERT( pCAPESERI: ped.CAPESERI, pCAPEUSCR: ped.CAPEUSCR, pCAPEFECR: ped.CAPEFECR,
                            pCAPENUME: ped.CAPENUME, pCAPEIDCL: ped.CAPEIDCL, pCAPEFECH: ped.CAPEFECH, pCAPEDIRE: ped.CAPEDIRE, pCAPEIDES: ped.CAPEIDES,
                            pCAPEEMAI: ped.CAPEEMAI, pCAPENOTI: ped.CAPENOTI, pCAPENOTG: ped.CAPENOTG, pCAPETIPO: ped.CAPETIPO, pCAPEIDTD: ped.CAPEIDTD, pCAPEDEST: ped.CAPEDEST);
                    }
                    else
                    {
                        context.PECAPE_UPDATE_GUARDA_PED(ped.CAPEIDCP, pCAPEIDCL: ped.CAPEIDCL, pCAPEFECH: ped.CAPEFECH,
                            pCAPEDIRE: ped.CAPEDIRE, pCAPEIDES: ped.CAPEIDES, pCAPEEMAI: ped.CAPEEMAI, pCAPENOTI: ped.CAPENOTI, pCAPENOTG: ped.CAPENOTG,
                            pCAPETIPO: ped.CAPETIPO, pCAPEUSMO: ped.CAPEUSMO, pCAPEFEMO: ped.CAPEFEMO,
                            pCAPEIDTD: ped.CAPEIDTD, pCAPEDEST: ped.CAPEDEST);
                    }


                    ped.CAPEIDCP = idpedido;//REV
                    //2019-02-28
                    tipo = 0; //reinicializando para usarlo en detalles 1 - anadir 2 - actulizar
                    //
                    secuencia = 0;
                    foreach (var item in lista)
                    {
                        secuencia++;

                        bool generarres = false;
                        var det = context.PEDEPE_Find(item.DEPEIDDP);//.PEDEPE.Find(item.DEPEIDDP);
                        if (det == null)
                        {
                            //det = new EFModelo.PEDEPE();
                            det = new appWcfService.PEDEPE();
                            det.DEPEFECR = DateTime.Now;
                            det.DEPEUSCR = pedido.CAPEUSCR;
                            det.DEPEESTA = Constantes.ID_ESTADO_CREADO;
                            //context.PEDEPE.Add(det);

                            //tipo 1 se añade una nuevo detalle
                            tipo = 1;
                            //2018-12-29
                            det.DEPEIDDP = item.DEPEIDDP;

                            generarres = true;
                        }
                        else
                        {
                            if (det.DEPEIDCP != ped.CAPEIDCP)
                            {
                                throw new Exception("Inconsistencia de datos, ID de pedido no coincide con Id de detalle");
                            }
                            //Eliminar el existente y crear de nuevo la reserva
                            det.DEPEFEMO = DateTime.Now;
                            det.DEPEUSMO = pedido.CAPEUSCR; //EL MISMO USUARIO
                            if (det.DEPEPESO != item.DEPEPESO || det.DEPECOAR.Trim() != item.DEPECOAR.Trim() || det.DEPEPART != item.DEPEPART)
                            {
                                //  DMA, SE SOLICITO REALIZAR LA RESERVA CON EL ID DEL DETALLE DEL PEDIDO PARA EVITAR RESERVAS DUPLICADAS, (det.DEPEIDDP)
                                ///descomentar
                                //_appDB2.generaReserva(false, "X", Convert.ToInt32(det.DEPEIDDP).ToString(), Convert.ToString(det.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");
                                ///descomentar
                                //_appDB2.generaReserva(false, "X", Convert.ToInt32(ped.CAPEIDCP).ToString(), Convert.ToString(det.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");

                                generarres = true;
                            }
                            //tipo 2 se modifica un detalle
                            tipo = 2;
                        }
                        det.DEPEIDCP = ped.CAPEIDCP;
                        det.DEPEALMA = item.DEPEALMA;
                        det.DEPECASO = item.DEPECASO;
                        det.DEPEPESO = item.DEPEPESO;
                        det.DEPECOAR = item.DEPECOAR;
                        det.DEPEPART = item.DEPEPART;
                        det.DEPECONT = item.DEPECONT;
                        det.DEPEDISP = item.DEPEDISP;
                        det.DEPEDSAR = item.DEPEDSAR;
                        det.DEPESERS = secuencia;
                        det.DEPESECU = item.DEPESECU;
                        //context.SaveChanges();

                        ///2018-12-29 añadiendo o actualizando
                        if (tipo == 1)
                        {
                            det.DEPEIDDP = context.PEDEPE_INSERT( pDEPEIDCP: det.DEPEIDCP, pDEPEFECR: det.DEPEFECR, pDEPEUSCR: det.DEPEUSCR
                                , pDEPEESTA: det.DEPEESTA, pDEPEALMA: det.DEPEALMA, pDEPECASO: det.DEPECASO, pDEPEPESO: det.DEPEPESO,
                                pDEPECOAR: det.DEPECOAR, pDEPEPART: det.DEPEPART, pDEPECONT: det.DEPECONT, pDEPEDISP: det.DEPEDISP,
                                pDEPEDSAR: det.DEPEDSAR, pDEPESERS: det.DEPESERS, pDEPESECU: det.DEPESECU);//.PEDEPE.Add(det);
                        }
                        else
                        {
                            context.PEDEPE_UPDATE_GUARDA_PED(det.DEPEIDDP, pDEPEIDCP: det.DEPEIDCP, pDEPEALMA: det.DEPEALMA, 
                                pDEPECASO: det.DEPECASO, pDEPEPESO: det.DEPEPESO,
                               pDEPECOAR: det.DEPECOAR, pDEPEPART: det.DEPEPART, pDEPECONT: det.DEPECONT, pDEPEDISP: det.DEPEDISP,
                               pDEPEDSAR: det.DEPEDSAR, pDEPESERS: det.DEPESERS, pDEPESECU: det.DEPESECU, pDEPEFEMO: det.DEPEFEMO, pDEPEUSMO: det.DEPEUSMO);
                        }
                        if (generarres)
                        {
                            //  DMA, SE SOLICITO REALIZAR LA RESERVA CON EL ID DEL DETALLE DEL PEDIDO PARA EVITAR RESERVAS DUPLICADAS, (det.DEPEIDDP)
                            ///descomentar
                            //_appDB2.generaReserva(false, "X", Convert.ToInt32(det.DEPEIDDP).ToString(), Convert.ToString(secuencia), det.DEPECOAR, det.DEPEPART, Convert.ToString(det.DEPEALMA), det.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(det.DEPEPESO * 100)), "A");
                            ///descomentar
                            //_appDB2.generaReserva(false, "X", Convert.ToInt32(ped.CAPEIDCP).ToString(), Convert.ToString(secuencia), det.DEPECOAR, det.DEPEPART, Convert.ToString(det.DEPEALMA), det.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(det.DEPEPESO * 100)), "A");
                        }

                    }

                    //ACTUALIZA EL NRO
                    var nume = context.PECAPE_NUME(ped.CAPESERI);//.PECAPE.Where(x => x.CAPESERI == ped.CAPESERI).Max(x => x.CAPENUME);
                    nume++;
                    if (ped.CAPENUME == 0)
                    {
                        ped.CAPENUME = nume;
                    }
                    numeropedido = ped.CAPENUME.ToString().PadLeft(7, '0');
                    idestado = ped.CAPEIDES;
                    idcabpedido = ped.CAPEIDCP;
                    //context.SaveChanges();
                    context.PECAPE_UPDATE_NUME(ped.CAPEIDCP, ped.CAPENUME);

                }
                vpar.VALSAL = new List<string>();
                vpar.VALSAL.Add(idpedido.ToString());
                vpar.VALSAL.Add(numeropedido);
                vpar.VALSAL.Add(idestado.ToString());
                vpar.VALSAL.Add(idcabpedido.ToString());

                vpar.ESTOPE = true;

            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation("Property: {0} Error: {1}",
            //                                    validationError.PropertyName,
            //                                    validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }

            finally
            {
            }
            return vpar;
        }

        private void insertaMovimientoKardex(PEDIDOSEntitiesDB2 context, decimal? idbolsa, decimal idtipomovimiento, decimal almacen, string partida, string articulo, decimal cantidad, decimal peso, decimal pesobr, string usuario, Nullable<decimal> iddetpedido, Nullable<decimal> iddetosa)
        {
            decimal valorsigno = 1;
            if (idtipomovimiento == TIPO_MOV_SALIDA_PREP_PED)
            {
                valorsigno = -1;
            }
            var entk = new appWcfService.PEKABO();//new EFModelo.PEKABO();
            entk.KABOIDBO = idbolsa.Value;
            entk.KABOIDTM = idtipomovimiento;
            entk.KABOALMA = almacen;
            entk.KABOPART = partida;
            entk.KABOITEM = articulo;
            entk.KABOCANT = cantidad * valorsigno;
            entk.KABOPESO = peso * valorsigno;
            entk.KABOPEBR = pesobr * valorsigno;
            entk.KABOTARA = entk.KABOPEBR - entk.KABOPESO;
            entk.KABOFECH = DateTime.Today;
            entk.KABOUSCR = usuario;
            entk.KABOFECR = DateTime.Now;
            entk.KABOIDDP = iddetpedido;
            entk.KABOIDDO = iddetosa;
            //context.PEKABO.Add(entk);
            context.PEKABO_INSERT(entk.KABOIDBO, pKABOIDTM: entk.KABOIDTM, pKABOFECH: entk.KABOFECH, pKABOALMA: entk.KABOALMA,
                pKABOCANT: entk.KABOCANT, pKABOPESO: entk.KABOPESO, pKABOIDDP: entk.KABOIDDP, pKABOIDDO: entk.KABOIDDO
                , pKABOPART: entk.KABOPART, pKABOITEM: entk.KABOITEM, pKABOTARA: entk.KABOTARA, pKABOPEBR: entk.KABOPEBR,
                pKABOUSCR: entk.KABOUSCR, pKABOFECR: entk.KABOFECR);

        }

        public RESOPE remueveBolsaPedidose(PAROPE paramOperacion)//decimal idbolsapedido, string usuario)
        {
            Nullable<decimal> iddetpedido;
            Nullable<decimal> iddetpedidoint;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            try
            {
                decimal idbolsapedido = decimal.Parse(paramOperacion.VALENT[0]);
                string usuario = paramOperacion.VALENT[1];

                partida = articulo = "";
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    appWcfService.PEDEPE detpednac = null;//EFModelo.PEDEPE detpednac = null;
                    appWcfService.PEDEOS detpedint = null;//EFModelo.PEDEOS detpedint = null;

                    //bool inserta = false;
                    //foreach (var item in collection)
                    //{

                    //}
                    var ent = context.PEBODP_Find(idbolsapedido);//.PEBODP.Find(idbolsapedido);
                    if (ent != null) //detallebolsa.BODPIDDE != 0)
                    {
                        bool sinempaque = !ent.BODPIDBO.HasValue;
                        appWcfService.PEBOLS bol = null;//EFModelo.PEBOLS bol = null;
                        if (!sinempaque)
                        {
                            bol = context.PEBOLS_Find_IDBO(ent.BODPIDBO);//.PEBOLS.Find(ent.BODPIDBO);
                        }

                        if (ent.BODPIDDP.HasValue)
                        {
                            detpednac = context.PEDEPE_Find(ent.BODPIDDP);//.PEDEPE.Find(ent.BODPIDDP);
                            partida = detpednac.DEPEPART;
                            articulo = detpednac.DEPECOAR;
                        }
                        else if (ent.BODPIDDO.HasValue)
                        {
                            detpedint = context.PEDEOS_Find(ent.BODPIDDO);//.PEDEOS.Find(ent.BODPIDDO);
                            partida = detpedint.DEOSPART;
                            articulo = detpedint.DEOSCOAR;
                        }
                        iddetpedido = ent.BODPIDDP;
                        iddetpedidoint = ent.BODPIDDO;
                        //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                        //{
                        //bol.BOLSESTA = 1; //ya no se usa
                        //}
                        if (!sinempaque)
                        {
                            insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
                        }
                        context.PEBODP_DELETE( ent.BODPIDDE);//.PEBODP.Remove(ent);
                        //context.SaveChanges();
                        decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

                        if (iddetpedido.HasValue)
                        {
                            var listbolsas = context.PEBODP_Find_IDDP(iddetpedido);//.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //detpednac = context.PEDEPE.Find(iddetpedido);
                            detpednac.DEPECAAT = cantatendida;
                            detpednac.DEPEPEAT = pesoatendido;
                            detpednac.DEPEPERE = pesoreal;
                            detpednac.DEPETADE = tade;
                            detpednac.DEPEPEBR = pebr;

                            //inserta tipo 2 kardex reingreso
                            //context.bodp
                            //context.SaveChanges();
                            context.PEDEPE_UPDATE_GUARDA_ELIMINA_BOLSA(pDEPEIDDP: detpednac.DEPEIDDP, pDEPEIDCP: detpednac.DEPEIDCP, pDEPECAAT: detpednac.DEPECAAT, pDEPEPEAT: detpednac.DEPEPEAT, pDEPEPERE: detpednac.DEPEPERE,
                                pDEPETADE: detpednac.DEPETADE, pDEPEPEBR: detpednac.DEPEPEBR,pDEPEUSMO: usuario, pDEPEFEMO:DateTime.Now);
                        }
                        else if (iddetpedidoint.HasValue)
                        {
                            var listbolsas = context.PEBODP_Find_IDDO(iddetpedidoint);//.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            detpedint.DEOSCAAT = cantatendida;
                            detpedint.DEOSPEAT = pesoatendido;
                            detpedint.DEOSPERE = pesoreal;
                            detpedint.DEOSTADE = tade;
                            detpedint.DEOSPEBR = pebr;
                            //actualiza peso en osa ---PENDIENTE EN AS
                            var osa = context.PROSAS_Find( 1, detpedint.DEOSFOLI, detpedint.DEOSSECU);//.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                            if (osa != null)
                            {
                                osa.OSASCAEN = pesoatendido;
                                ///descomentar
                                //actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar AS
                                ///descomentar
                            }
                            //context.SaveChanges();
                            context.PEDEOS_UPDATE(pDEOSIDDO: detpedint.DEOSIDDO, pDEOSIDCO: detpedint.DEOSIDCO, pDEOSCAAT: detpedint.DEOSCAAT,
                                pDEOSPEAT: detpedint.DEOSPEAT, pDEOSPERE: detpedint.DEOSPERE, pDEOSTADE: detpedint.DEOSTADE, pDEOSPEBR: detpedint.DEOSPEBR);
                        }
                        if (!sinempaque)
                        {
                            //actualiza el stock de la bolsa
                            var todasbolsasprep = context.PEBODP_Find_IDBO_ALMA_PART_COAR(bol.BOLSIDBO, bol.BOLSALMA , partida , articulo);
                            //.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in todasbolsasprep)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            var detemp = context.GMDEEM_Find_DEEMARTI(1 , bol.BOLSCOEM , partida , articulo , "N");
                                //.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");//PRPEDAT.USP_EP_GMDEEM_FIND_DEEMARTI
                            if (detemp != null)
                            {
                                detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                //si la bolsa preparada estaba marcada como stock cero, lo desmarco
                                if (ent.BODPSTCE == 1)
                                {
                                    detemp.DEEMSTCE = 0;
                                }
                                if (detemp.DEEMSTCE == 0)
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                else
                                if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                //context.SaveChanges();
                                ///descomentar
                                //actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); //Validar AS
                                ///descomentar
                                                                                                                                                       //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                detemp = context.GMDEEM_Find_DEEMESBO( 1 , bol.BOLSCOEM , "N" , 9);
                                    //.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);//PRPEDAT.USP_EP_GMDEEM_FIND_DEEMESBO
                                if (detemp == null)
                                {
                                    bol.BOLSESTA = 9; //ya no se usa la bolsa
                                    context.PEBOLS_UPDATE(bol.BOLSIDBO, PBOLSESTA: bol.BOLSESTA, PBOLSUSMO: usuario, PBOLSFEMO: DateTime.Now);
                                }
                                else
                                {
                                    bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                    context.PEBOLS_UPDATE(bol.BOLSIDBO, PBOLSESTA: bol.BOLSESTA, PBOLSUSMO: usuario, PBOLSFEMO: DateTime.Now);
                                }
                                //context.SaveChanges();
                            }
                        }
                    }
                    vpar.ESTOPE = true;
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }

        public RESOPE guardaPreparacionBolsase(PAROPE paramOperacion)//appWcfService.PEBODP detallebolsa)
        {
            ////
            List<appWcfService.PEBODP> listBolsas;
            ////

            Nullable<decimal> iddetpedidostoc = null;

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            decimal almacen;
            try
            {
                ///
                listBolsas = Util.Deserialize<List<appWcfService.PEBODP>>(paramOperacion.VALENT[0]);
                ///

                partida = articulo = "";
                almacen = 0;
                //if (detallebolsa == null)
                //{
                //    Util.EscribeLog("detallebolsa es null");
                //}
                //else
                //{
                //    Util.EscribeLog("detallebolsa " + detallebolsa.BODPIDDP.ToString() + " " + detallebolsa.BODPIDDP.ToString());
                //}

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    foreach (var detallebolsa in listBolsas)
                    {
                        appWcfService.PEDEPE detpednac = null;
                        appWcfService.PEDEOS detpedint = null;
                        appWcfService.GMCAEM emp = null;
                        //inserta PBOLS si no existe
                        //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
                        bool sinempaque = string.IsNullOrWhiteSpace(detallebolsa.PEBOLS.BOLSCOEM);
                        if (!sinempaque)
                        {
                            emp = context.GMCAEM_Find_first(1, detallebolsa.PEBOLS.BOLSCOEM);
                            //.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
                        }

                        if (emp != null || sinempaque)
                        {
                            appWcfService.PEBOLS bol = null;
                            //appWcfService.PEBOLS bolcreada = null;//  objeto para obtener el idbo creado
                            if (!sinempaque)
                            {
                                bol = context.PEBOLS_Find_first(detallebolsa.PEBOLS.BOLSCOEM);
                                //.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);

                                if (bol == null) //no existe pbols, insertar
                                {
                                    bol = new appWcfService.PEBOLS();
                                    //inserta = true;
                                    bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
                                    bol.BOLSUSCR = detallebolsa.BODPUSCR;
                                    bol.BOLSFECR = DateTime.Now;
                                    bol.BOLSCOCA = null;
                                    bol.BOLSESTA = 1;
                                    bol.BOLSALMA = emp.CAEMALMA;
                                    bol.BOLSCOAR = "";
                                    Util.EscribeLog("3");

                                    //context.PEBOLS.Add(bol);
                                    detallebolsa.BODPIDBO = context.PEBOLS_INSERT(PBOLSCOAR: bol.BOLSCOAR, PBOLSCOEM: bol.BOLSCOEM, PBOLSCOCA: bol.BOLSCOCA, PBOLSALMA: bol.BOLSALMA,
                                        PBOLSESTA: bol.BOLSESTA, PBOLSUSCR: bol.BOLSUSCR, PBOLSFECR: bol.BOLSFECR);
                                    Util.EscribeLog("4");

                                    //context.SaveChanges();
                                    Util.EscribeLog("5");
                                    //2019-02-15
                                    //bolcreada = context.PEBOLS_Find_first(detallebolsa.PEBOLS.BOLSCOEM);
                                    //detallebolsa.BODPIDBO = bolcreada.BOLSIDBO;
                                }
                                else
                                {
                                    if (bol.BOLSALMA == 0)
                                    {
                                        bol.BOLSALMA = emp.CAEMALMA;
                                    }
                                }
                            }

                            if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                            {
                                //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
                            }
                            //bool inserta = false;
                            //
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                detpednac = context.PEDEPE_Find(detallebolsa.BODPIDDP);//.PEDEPE.Find(detallebolsa.BODPIDDP);
                                partida = detpednac.DEPEPART;
                                articulo = detpednac.DEPECOAR;
                                almacen = detpednac.DEPEALMA;
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                detpedint = context.PEDEOS_Find(detallebolsa.BODPIDDO);//.PEDEOS.Find(detallebolsa.BODPIDDO);
                                partida = detpedint.DEOSPART;
                                articulo = detpedint.DEOSCOAR;
                                almacen = detpedint.DEOSALMA;
                            }

                            //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
                            var ent = context.PEBODP_Find(detallebolsa.BODPIDDE);
                            //.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

                            bool insert = false;//2019/02/11

                            if (ent == null) //detallebolsa.BODPIDDE != 0)
                            {
                                ent = new appWcfService.PEBODP();
                                //inserta = true;
                                ent.BODPUSCR = detallebolsa.BODPUSCR;
                                ent.BODPFECR = DateTime.Now;
                                //context.PEBODP.Add(ent);
                                insert = true;//2019-02-11
                                if (!sinempaque)
                                {
                                    //inserta tipo 1 SALIDA
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                }
                            }
                            else
                            {
                                insert = false;
                                ent.BODPUSMO = detallebolsa.BODPUSCR;
                                ent.BODPFEMO = DateTime.Now;
                                ent.BODPESTA = 3;
                                if (!sinempaque)
                                {
                                    //SOLO si las cantidades o pesos son diferentes
                                    //inserta tipo 3 reingreso
                                    //inserta tipo 1 salida
                                    if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
                                    {
                                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                    }
                                }
                            }
                            //automatizar el parse sin incluir la PK
                            ent.BODPIDBO = detallebolsa.BODPIDBO;
                            ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
                            ent.BODPALMA = almacen; //bol.BOLSALMA;
                            ent.BODPPART = partida;
                            ent.BODPCOAR = articulo;
                            ent.BODPCANT = detallebolsa.BODPCANT;
                            ent.BODPPESO = detallebolsa.BODPPESO;
                            ent.BODPPERE = detallebolsa.BODPPERE;
                            ent.BODPDIFE = detallebolsa.BODPDIFE;
                            ent.BODPSTCE = detallebolsa.BODPSTCE;
                            ent.BODPINBO = detallebolsa.BODPINBO;
                            ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
                                                                  //2018-04-11
                            ent.BODPTAUN = detallebolsa.BODPTAUN;
                            //iddetpedido = ent.BODPIDDP;
                            //iddetpedidoint = ent.BODPIDDO;
                            if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
                            {
                                iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
                            }

                            ent.BODPTADE = detallebolsa.BODPTADE;
                            ent.BODPPEBR = detallebolsa.BODPPEBR;
                            ent.BODPESTA = detallebolsa.BODPESTA;

                            //context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
                            if (insert)
                            {
                                if (sinempaque)
                                {
                                    ent.BODPIDBO = null;
                                }

                                context.PEBODP_INSERT(PBODPIDBO: ent.BODPIDBO, PBODPIDDP: ent.BODPIDDP, PBODPALMA: ent.BODPALMA,
                                    PBODPPART: ent.BODPPART, PBODPCOAR: ent.BODPCOAR, PBODPCANT: ent.BODPCANT, PBODPPESO: ent.BODPPESO,
                                    PBODPPERE: ent.BODPPERE, PBODPDIFE: ent.BODPDIFE, PBODPIDDO: ent.BODPIDDO, PBODPSTCE: ent.BODPSTCE,
                                    PBODPINBO: ent.BODPINBO, PBODPTADE: ent.BODPTADE, PBODPPEBR: ent.BODPPEBR, PBODPESTA: ent.BODPESTA,
                                    PBODPUSCR: ent.BODPUSCR, PBODPFECR: ent.BODPFECR, PBODPSECR: ent.BODPSECR, PBODPTAUN: ent.BODPTAUN,
                                    PBODPAPOR: "D");
                            }
                            else
                            {
                                context.PEBODP_UPDATE(ent.BODPIDDE, PBODPIDBO: ent.BODPIDBO, PBODPIDDP: ent.BODPIDDP, PBODPALMA: ent.BODPALMA,
                                    PBODPPART: ent.BODPPART, PBODPCOAR: ent.BODPCOAR, PBODPCANT: ent.BODPCANT, PBODPPESO: ent.BODPPESO,
                                    PBODPPERE: ent.BODPPERE, PBODPDIFE: ent.BODPDIFE, PBODPIDDO: ent.BODPIDDO, PBODPSTCE: ent.BODPSTCE,
                                    PBODPINBO: ent.BODPINBO, PBODPTADE: ent.BODPTADE, PBODPPEBR: ent.BODPPEBR, PBODPESTA: ent.BODPESTA,
                                    PBODPUSMO: ent.BODPUSMO, PBODPFEMO: ent.BODPFEMO, PBODPSECR: ent.BODPSECR, PBODPTAUN: ent.BODPTAUN,
                                    PBODPAPOR: "D");
                            }//BODPSECR

                            decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                var listbolsas = context.PEBODP_Find_IDDP(detallebolsa.BODPIDDP);
                                    //.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
                                // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpednac = context.PEDEPE.Find(iddetpedido);
                                detpednac.DEPECAAT = cantatendida;
                                detpednac.DEPEPEAT = pesoatendido;
                                detpednac.DEPEPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpednac.DEPESTOC = iddetpedidostoc;
                                }
                                detpednac.DEPETADE = tade;
                                detpednac.DEPEPEBR = pebr;

                                //context.SaveChanges();

                                context.PEDEPE_UPDATE_GUARDA_ELIMINA_BOLSA(detpednac.DEPEIDDP, pDEPECAAT: detpednac.DEPECAAT, pDEPEPEAT: detpednac.DEPEPEAT,
                                    pDEPEPERE: detpednac.DEPEPERE, pDEPESTOC: detpednac.DEPESTOC, pDEPETADE: detpednac.DEPETADE, pDEPEPEBR: detpednac.DEPEPEBR,pDEPEUSMO: detallebolsa.BODPUSCR,pDEPEFEMO:DateTime.Now);
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                var listbolsas = context.PEBODP_Find_IDDO(detallebolsa.BODPIDDO);
                                    //.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                                detpedint.DEOSCAAT = cantatendida;
                                detpedint.DEOSPEAT = pesoatendido;
                                detpedint.DEOSPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpedint.DEOSSTOC = iddetpedidostoc;
                                }
                                detpedint.DEOSTADE = tade;
                                detpedint.DEOSPEBR = pebr;
                                //actualiza peso en osa ---PENDIENTE EN AS
                                var osa = context.PROSAS_Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                                    //.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                                if (osa != null)
                                {
                                    osa.OSASCAEN = pesoatendido;
                                    ///descomentar
                                    //actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
                                    ///descomentar
                                }
                                //context.SaveChanges();
                                context.PEDEOS_UPDATE(detpedint.DEOSIDDO, pDEOSCAAT: detpedint.DEOSCAAT, pDEOSPEAT: detpedint.DEOSPEAT, pDEOSPERE: detpedint.DEOSPERE,
                                    pDEOSSTOC: detpedint.DEOSSTOC, pDEOSTADE: detpedint.DEOSTADE, pDEOSPEBR: detpedint.DEOSPEBR);
                            }
                            if (!sinempaque)
                            {
                                //actualiza el stock de la bolsa
                                var todasbolsasprep = context.PEBODP_Find_IDBO_ALMA_PART_COAR(bol.BOLSIDBO, bol.BOLSALMA, partida, articulo);
                                    //.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in todasbolsasprep)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                var detemp = context.GMDEEM_Find_DEEMARTI(1, bol.BOLSCOEM, partida, articulo, "N");
                                    //.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                                if (detemp != null)
                                {
                                    //---PENDIENTE EN AS
                                    detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                    if (emp.CAEMMSPA == "+")
                                    {
                                        detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                    }
                                    else
                                    {
                                        detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
                                    }
                                    if (iddetpedidostoc.HasValue)
                                    {
                                        detemp.DEEMSTCE = iddetpedidostoc.Value;
                                    }
                                    if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
                                    {
                                        detemp.DEEMESBO = 9;
                                    }
                                    else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                    {
                                        detemp.DEEMESBO = 9;
                                    }
                                    else
                                    {
                                        detemp.DEEMESBO = 1;
                                    }
                                    ///descomentar
                                    //actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
                                    ///descomentar
                                    //context.SaveChanges();
                                    //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                    detemp = context.GMDEEM_Find_DEEMESBO(1, bol.BOLSCOEM, "N", 9);
                                        //.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                    if (detemp == null)
                                    {
                                        bol.BOLSESTA = 9; //ya no se usa la bolsa
                                        context.PEBOLS_UPDATE(bol.BOLSIDBO, PBOLSESTA: bol.BOLSESTA, PBOLSUSMO: detallebolsa.BODPUSCR, PBOLSFEMO: DateTime.Now);
                                    }
                                    else
                                    {
                                        bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                        context.PEBOLS_UPDATE(bol.BOLSIDBO, PBOLSESTA: bol.BOLSESTA, PBOLSUSMO: detallebolsa.BODPUSCR, PBOLSFEMO: DateTime.Now);
                                    }
                                    //context.SaveChanges();
                                }
                            }
                            vpar.VALSAL = new List<string>();
                            ////vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                            ////vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                            resultado = ""; // ent.BODPIDDE.ToString();
                            vpar.ESTOPE = true;
                        }
                        else
                        {
                            resultado = "Código de empaque incorrecto";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }

        public bool PreparaCorreoNotificacionPedido(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> pedidodet, out string cc, out string asunto, out string body, out string mensaje)
        {
            bool resultado = false;
            string detallehtm;
            string idseguimiento, estado, accionestado;
            appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result pedcab;

            cc = asunto = body = mensaje = null;
            //cc = ConfigurationManager.AppSettings["ControlCalidadCopia"];
            pedcab = pedidodet[0];

            idseguimiento = pedcab.CAPEIDCP.ToString() + pedcab.CAPENUME.ToString().PadLeft(8, '0').Substring(6) + pedcab.CAPESERI.Substring(2) + pedcab.CAPEFECH.Day.ToString().PadLeft(2, '0');
            //2   Emitido
            //3   En preparación
            //4   En aprobación
            //5   Completado
            estado = accionestado = "";
            if (pedcab.CAPEIDES == 2)
            {
                accionestado = "ha sido emitido";
                estado = "Emitido";
            }
            else if (pedcab.CAPEIDES == 3)
            {
                accionestado = "esta siendo preparado";
                estado = "En preparación";
            }
            else if (pedcab.CAPEIDES == 4)
            {
                accionestado = "ha sido preparado";
                estado = "Preparado";
            }
            else if (pedcab.CAPEIDES == 5)
            {
                accionestado = "ha sido despachado";
                estado = "Despachado";
            }
            else if (pedcab.CAPEIDES == 9)
            {
                accionestado = "ha sido anulado";
                estado = "Anulado";
            }
            else
            {
                mensaje = "El estado para la notificacion del pedido no es válido";
                return false;
            }

            detallehtm = getHtmlDetallePed(pedidodet);

            asunto = string.Format(Mensajes.TEXTO_ASUNTO_NOTIFICACION_PEDIDO, idseguimiento, accionestado);
            body = "<p>" + "Estimados Señores:" + "<b>" + "</b>" + "</p>" + "<p>" + "A continuación se detalla la información relacionada a su pedido </p>";
            body += "<p>" + "<b>" + "Identificador Pedido: " + "</b>" + idseguimiento + "</p>";
            body += "<b>" + "Fecha Emisión: " + "</b>" + pedcab.CAPEFHEM.Value.ToString(Constantes.FORMATO_FECHA) + "";

            body += "<p>" + "<b>" + "Inicio Preparación: " + "</b>" + (pedcab.CAPEFHIP.HasValue ? pedcab.CAPEFHIP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Preparación Completada: " + "</b>" + (pedcab.CAPEFHFP.HasValue ? pedcab.CAPEFHFP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Fecha de Despacho: " + "</b>" + (pedcab.CAPEFEAP.HasValue ? pedcab.CAPEFEAP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Estado: " + "</b>" + estado + "</p>";

            body += "<br>";
            body += "<p>" + "Puede visualizar el estado de su pedido ingresando a la siguiente dirección y usando el identificador de su pedido" + "</p>";
            body += "<p>" + "http://localhost:52998/default.aspx" + "</p>";
            body += "<br>";

            body += detallehtm + "<br>";
            body += "<p>" + "<b>" + "Observación pedido: " + "</b>" + pedcab.CAPENOTG + "</p>";
            body += "<br>Atentamente<br><p>" + "</p>" + "<br>" + "<p>" + "<b>" + Mensajes.TEXTO_NOTIFICACION_PIE + "</b>" + "</p>";
            body = body.Replace("\\n", "<br>");
            body = "<style>   body {font-family:Verdana; font-size: x-small } p {font-family:Verdana; font-size: x-small } </style>" + body;
            resultado = true;

            return resultado;
        }

        public string getHtmlDetallePed(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> pedidodet)
        {
            int fil;
            string encabezado;
            string[] aux;
            string messageBody = ""; //"<font>The following are the records: </font><br><br>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center; \" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6B696B; color:White; font-weight:bold;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"background-color:#E3E3E3; color:Black;\">";
            string htmlTrEnd = "</tr>";
            string htmlTrStart2 = "<tr style =\"background-color:#F3F3F3; color:Black;\">";
            string htmlTrEnd2 = "</tr>";

            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:none; border-width:thin; padding: 5px; text-align:left; \">";
            string htmlTdEnd = "</td>";

            string htmlTdAlgRStart = "<td style=\" border-color:#5c87b2; border-style:none; border-width:thin; padding: 5px; text-align:right; \">";
            string htmlTdAlgREnd = "</td>";

            string htmlFooterRowStart = "<tr style =\"background-color:#a33; color:White; font-weight:bold;\">";
            string htmlFooterRowEnd = "</tr>";


            //decimal SumaBlanco = 0;

            int pos = 0;
            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;

            encabezado = "#,Código,Lote,Kg Solicitados,Kg Atendidos";

            aux = encabezado.Split(new char[] { ',' });
            foreach (string _encabezado in aux)
            {
                if (pos >= 3 && pos <= 4)
                {
                    messageBody += htmlTdAlgRStart + _encabezado + htmlTdEnd;
                }
                else
                {
                    messageBody += htmlTdStart + _encabezado + htmlTdEnd;
                }
                pos++;
            }

            messageBody += htmlHeaderRowEnd;

            fil = 0;
            foreach (var Row in pedidodet)
            {
                if ((fil % 2 != 0))
                {
                    messageBody = messageBody + htmlTrStart;
                }
                else
                {
                    messageBody = messageBody + htmlTrStart2;
                }
                //#,Código,Lote,Kg Solicitados,Kg Atendidos
                messageBody = messageBody + htmlTdStart + Convert.ToString(fil + 1) + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + Convert.ToString(Row.DEPECOAR.Trim()) + htmlTdAlgREnd;
                messageBody = messageBody + htmlTdStart + Convert.ToString(Row.DEPEPART.Trim()) + htmlTdAlgREnd;

                messageBody = messageBody + htmlTdAlgRStart + Convert.ToDecimal(Row.DEPEPESO).ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;
                messageBody = messageBody + htmlTdAlgRStart + Convert.ToDecimal(Row.DEPEPEAT).ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;

                if ((fil % 2 != 0))
                {
                    messageBody = messageBody + htmlTrEnd;
                }
                else
                {
                    messageBody = messageBody + htmlTrEnd2;
                }
                fil++;
            }

            messageBody += htmlFooterRowStart;
            //messageBody = messageBody + htmlTdStart + "TOTAL" + htmlTdEnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdEnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;

            //messageBody = messageBody + htmlTdAlgRStart + SumaBlanco.ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;

            messageBody += htmlFooterRowEnd;

            messageBody = messageBody + htmlTableEnd;
            messageBody += "<br>";

            return messageBody;
        }

        public bool EnvioCorreo(string destinatario, string cc, string bcc, string asunto, string body)
        {
            bool resultado = false;
            try
            {
                System.Net.Mail.MailMessage mailMessage = default(System.Net.Mail.MailMessage);
                mailMessage = new System.Net.Mail.MailMessage();
                //mailMessage.From = new System.Net.Mail.MailAddress(CuentaDe); // , CuentaDescripcion);
                mailMessage.From = new System.Net.Mail.MailAddress(CuentaDe, CuentaDescripcion); //prueba 20160201
                mailMessage.To.Add(destinatario);
                if (!string.IsNullOrEmpty(cc))
                {
                    mailMessage.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    mailMessage.Bcc.Add(bcc);
                }
                //asunto = AsuntoCorreo;
                mailMessage.Subject = asunto;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;

                System.Net.Mail.SmtpClient o = new System.Net.Mail.SmtpClient(ServidorSMTP);
                o.UseDefaultCredentials = false;
                o.Credentials = new System.Net.NetworkCredential(CuentaDe, ClaveCuenta, DominioCuenta);
                //o.EnableSsl = True 'gmail
                o.Port = Convert.ToInt32(PuertoSMTP);
                o.Send(mailMessage);
                resultado = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            return resultado;
        }

        public RESOPE ActualizaNotificaciones(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string emitir, iprepa, fprepa, despa;
            try
            {
                emitir = paramOperacion.VALENT[0];
                iprepa = paramOperacion.VALENT[1];
                fprepa = paramOperacion.VALENT[2];
                despa = paramOperacion.VALENT[3];

                using (var context = new PEDIDOSEntitiesDB2())
                {
                    var aleemi = context.Parametros_find(21);//.PEPARM.Find(21);
                    if (aleemi != null)
                    {
                        aleemi.PARMVAPA = emitir;
                    }
                    var aleipr = context.Parametros_find(22);// PEPARM.Find(22);
                    if (aleipr != null)
                    {
                        aleipr.PARMVAPA = iprepa;
                    }
                    var alefpr = context.Parametros_find(23);//.PEPARM.Find(23);
                    if (alefpr != null)
                    {
                        alefpr.PARMVAPA = fprepa;
                    }
                    var aledes = context.Parametros_find(24);// .PEPARM.Find(24);
                    if (aledes != null)
                    {
                        aledes.PARMVAPA = despa;
                    }
                    context.PEPARM_UPDATE(aleemi.PARMIDPA,aleemi.PARMVAPA);// .SaveChanges();
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
            }
            return vpar;
        }

        public RESOPE BuscaArticulo(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            //List<object> listaeo = null;
            //List<appWcfService.USP_OBTIENE_ARTICULOS_Result> lista = null;
            List<appWcfService.USP_OBTIENE_ARTICULOS_Result> lista = null;
            try
            {
                string contrato, articulo, partida, selec;
                contrato = paramOperacion.VALENT[0];
                articulo = paramOperacion.VALENT[1];
                partida = paramOperacion.VALENT[2];
                selec = paramOperacion.VALENT[3];

                partida = String.IsNullOrWhiteSpace(partida) ? null : partida;
                contrato = string.IsNullOrWhiteSpace(contrato) ? null : contrato;
                //articulo = String.IsNullOrWhiteSpace(partida) ? null : articulo;
                using (var context = new PEDIDOSEntitiesDB2())
                {
                    //if (contrato.Length == 0)
                    //{
                    lista = context.ObtieneArticulos(contrato, partida, articulo, selec);//.USP_OBTIENE_ARTICULOS(contrato, partida, articulo, selec).ToList<object>();
                    //}
                    //else
                    //{
                    //    //completar
                    //}
                }
                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_ARTICULOS_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

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
            }
            return vpar;
        }//COMPLICADO AL FINAL NO ESTA MIGRADO AUN

        #endregion


    }
}
