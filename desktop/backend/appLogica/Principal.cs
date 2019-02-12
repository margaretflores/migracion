using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using IBM.Data.DB2.iSeries;

using appWcfService;
using appConstantes;

using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace appLogica
{
    public class Principal
    {

        //internal BaseDatos DB2;

        #region variables
        //correo
        private string ServidorSMTP, CuentaDe, CuentaDescripcion, ClaveCuenta, DominioCuenta, PuertoSMTP;

        private bool OcultaErrorReal;
        private string Aplicacion;
        private string DataSourceDB2;

        //CAMBIAR TABLAS DE PRUEBA Z1 A I1
        //PROGRAMA INVENTARIO
        public static string NombreCLDescargaInventario = "GMA003PP"; //GMA003P 20151130 PRUEBAS USUARIO //GMA003PP produccion

        #endregion

        public Principal()
        {
            string stringConnection, userId, userPassword;
            DataSourceDB2 = ConfigurationManager.AppSettings["DataSource"];
            Aplicacion = ConfigurationManager.AppSettings["Aplicacion"];

            userId = "PCVTAS";
            userPassword = "PCVTAS";
            stringConnection = "DataSource=" + DataSourceDB2 + ";User Id=" + userId + ";Password=" + userPassword + ";Naming=System;LibraryList=QS36F,INCAOBJ,ACOPDAT,costdat,speed400ik,speedobjik,SYSIBM,RRHHDAT,GEMAPRG;CheckConnectionOnOpen=true;";
            //DB2 = new BaseDatos(stringConnection);

            //correo
            ServidorSMTP = ConfigurationManager.AppSettings["Smtp"];
            //CuentaPara = ConfigurationManager.AppSettings["ParaOVT"];
            CuentaDe = ConfigurationManager.AppSettings["ComprasDeSmtp"];
            CuentaDescripcion = ConfigurationManager.AppSettings["ComprasNombreSmtp"];
            ClaveCuenta = ConfigurationManager.AppSettings["ComprasClaveSmtp"];
            DominioCuenta = ConfigurationManager.AppSettings["DominioSmtp"];
            PuertoSMTP = ConfigurationManager.AppSettings["PuertoSmtp"];

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
            //DB2 = null;
        }

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

        public RESOPE ValidaUsuario(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false, MENERR = Mensajes.MENSAJE_CREDENCIALES_NO_VALIDAS };
            sgus003Service.Isgus003ServiceClient cli = null;
            sgus003Service.PAROPE pope = new sgus003Service.PAROPE();
            try
            {
                cli = new sgus003Service.Isgus003ServiceClient();
                cli.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(ConfigurationManager.AppSettings["UriServicioUsuario"]), cli.Endpoint.Address.Identity, cli.Endpoint.Address.Headers);
                pope.CODOPE = paramOperacion.CODOPE;
                paramOperacion.VALENT.Add(ConfigurationManager.AppSettings["Aplicacion"]);
                pope.VALENT = paramOperacion.VALENT.ToArray();
                var resope = cli.EjecutaOperacion(pope);
                vpar.ESTOPE = resope.ESTOPE;
                vpar.MENERR = resope.MENERR;
                vpar.VALSAL = resope.VALSAL.ToList();
            }
            catch (Exception ex)
            {
                vpar.MENERR = ErrorGenerico(ex.Message);
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            finally
            {
                try
                {
                    if (cli != null)
                    {
                        cli.Close();
                    }
                }
                catch { }
                cli = null;
            }
            return vpar;
        }

        public RESOPE CambiaClave(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false, MENERR = Mensajes.MENSAJE_CREDENCIALES_NO_VALIDAS };
            sgus003Service.Isgus003ServiceClient cli = null;
            sgus003Service.PAROPE pope = new sgus003Service.PAROPE();
            try
            {
                cli = new sgus003Service.Isgus003ServiceClient();
                cli.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(ConfigurationManager.AppSettings["UriServicioUsuario"]), cli.Endpoint.Address.Identity, cli.Endpoint.Address.Headers);
                pope.CODOPE = paramOperacion.CODOPE;
                pope.VALENT = paramOperacion.VALENT.ToArray();
                var resope = cli.EjecutaOperacion(pope);
                vpar.ESTOPE = resope.ESTOPE;
                vpar.MENERR = resope.MENERR;
                vpar.VALSAL = resope.VALSAL.ToList();
            }
            catch (Exception ex)
            {
                vpar.MENERR = ErrorGenerico(ex.Message);
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            finally
            {
                try
                {
                    if (cli != null)
                    {
                        cli.Close();
                    }
                }
                catch { }
                cli = null;
            }
            return vpar;
        }

        //public RESOPE BuscaPersona(PAROPE paramOperacion)
        //{
        //    DataTable personaDataTable = null;
        //    DataRow re;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    try
        //    {
        //        string codigo;
        //        StringBuilder comandoSql = new StringBuilder();

        //        codigo = paramOperacion.VALENT[0];

        //        //temporal tabla de personas
        //        comandoSql.Append("SELECT");
        //        comandoSql.Append(" ACODIGO, ANOMBRE, AAPPAT, AAPMAT ");
        //        comandoSql.Append(" FROM A001MPER WHERE ACODIGO = '" + codigo + "' AND ACIA = 1");

        //        DB2.Conectar();
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.Text);
        //        personaDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
        //        if (!Util.TablaVacia(personaDataTable))
        //        {
        //            re = personaDataTable.Rows[0];
        //            vpar.VALSAL = new List<string>();
        //            vpar.VALSAL.Add(re["ACODIGO"].ToString().Trim());
        //            vpar.VALSAL.Add(re["ANOMBRE"].ToString().Trim());
        //            vpar.VALSAL.Add(re["AAPPAT"].ToString().Trim());
        //            vpar.VALSAL.Add(re["AAPMAT"].ToString().Trim());
        //            vpar.ESTOPE = true;

        //        }
        //        else
        //        {
        //            vpar.MENERR = "Código ingresado no existe";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //    return vpar;
        //}
              
        //public RESOPE ObtieneCorrelativo(PAROPE paramOperacion)
        //{
        //    DataTable detalleDataTable;
        //    DataRow re;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    try
        //    {
        //        string tipodoc, serie;
        //        int correlativo;
        //        StringBuilder comandoSql = new StringBuilder();

        //        tipodoc = paramOperacion.VALENT[0];
        //        serie = paramOperacion.VALENT[1];

        //        comandoSql.Append("SELECT CRDCCIA, CRDCTIDC, CRDCSERI, CRDCNUAC ");
        //        comandoSql.Append(" FROM CCCRDC where CRDCCIA = " + Convert.ToString(Constantes.CODIGO_CIA));
        //        comandoSql.Append(" AND CRDCTIDC = '" + tipodoc + "' AND CRDCSERI = '" + serie + "' ");
        //        DB2.Conectar();
        //        DB2.CrearComando(comandoSql.ToString(), CommandType.Text);
        //        detalleDataTable = DB2.EjecutarProcedimientoAlmacenado().Tables[0];
        //        vpar.VALSAL = new List<string>();
        //        correlativo = 0;
        //        if (!Util.TablaVacia(detalleDataTable))
        //        {
        //            re = detalleDataTable.Rows[0];
        //            if (int.TryParse(Convert.ToString(re["CRDCNUAC"]), out correlativo))
        //            {
        //                //correlativo++;
        //            }
        //        }
        //        //else
        //        //{
        //        //    vpar.VALSAL.Add("0");
        //        //}
        //        correlativo++;
        //        vpar.VALSAL.Add(correlativo.ToString());
        //        vpar.ESTOPE = true;
        //        //}
        //        //else
        //        //{
        //        //    vpar.MENERR = Mensajes.MENSAJE_DATOS_NO_ENCOTRADOS;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //        DB2.Desconectar();
        //    }
        //    return vpar;
        //}


    }
}
