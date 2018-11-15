using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Microsoft.Reporting.WebForms;

using appFew.appServicio;
using appWcfService;
using appConstantes;
using appFew;
using System.IO;
using System.Text;


namespace appFew.plan
{
    public partial class bdcarga : System.Web.UI.Page 
    {
        private ParametrosFe _ParametrosIni;
        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;
        private string url = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            _ParametrosIni = (ParametrosFe)Session["ParametrosFe"];
            if (Session["ParametrosFe"] == null)
            {
                Response.Redirect("../login.aspx?ReturnURL=" + Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    if (!Page.IsPostBack)
                    {
                        ObtieneUltimaFechaGeneracion();
                    }
                }
                catch (Exception ex)
                {
                    Error_1 = "Ha ocurrido un error en la pagina.";
                    Error_2 = ex.Message;
                    url = "ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + Error_2;
                    Response.Redirect(url);
                }
            }
        }

        private void ObtieneUltimaFechaGeneracion()
        {
            IappServiceClient clt = null;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_VALOR_PARAMETRO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Parametros.ULTIMA_FECHA_GENERACION_CARGA);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        ultimaLabel.Text = FuncionesUtil.ParseDate(resultado.VALSAL[1].Trim(), Constantes.FORMATO_FECHA_PARAMETRO).ToString(Constantes.FORMATO_FECHA_HORA);
                    }
                    else
                    {
                        MostrarMensaje(Mensajes.MENSAJE_PARAMETRO_NO_ENCONTRADO, true);
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR, true);
                }
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        private void GeneraReporte()
        {
            IappServiceClient clt = null;
            Session[Constantes.NOMBRE_SESION_DATOS_PDB] = null;
            try
            {
                //if (Convert.ToInt32(periodoDropDownList.SelectedValue) > Convert.ToInt32(periodoFinDropDownList.SelectedValue))
                //{
                //    ErrorLabel.Text = Mensajes.MENSAJE_PERIODO_GENERAR_MAYOR;
                //    return;
                //}
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GENERA_BD_CARGA;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add("");

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    //if (resultado.VALSAL[0].Equals("1")) //encontrado
                    //{
                    //    List<DETCON> datos = Funciones.Deserialize<List<DETCON>>(resultado.VALSAL[1]);

                    //    ImpresionDatos objetoImpr = new ImpresionDatos() { Destino = ImpresionDatos.ADestino.AReporte, Datos = datos, NombreDataSet = "DataSetDetalleContrato", Path = "~/reportes/detcontrato.rdlc", NombreArchivo = "DetalleContrato" };
                    //    Session[Constantes.NOMBRE_SESION_DATOS_PDB] = objetoImpr; // datos;
                    //    //Response.Redirect("formatodoc2.aspx", true);
                    //    //ExportToExcel(System.Guid.NewGuid().ToString(), objetoImpr.Datos, objetoImpr.Path, objetoImpr.NombreDataSet);
                    //    string tempabc = "javascript:window.open('../reportes/formatodoc2.aspx')";
                    //    ScriptManager.RegisterStartupScript(up, up.GetType(), "Formato", tempabc, true);
                    //    return;
                    //}
                    //else
                    //{
                    //    ErrorLabel.Font.Bold = true;
                    //    ErrorLabel.Text = Mensajes.MENSAJE_DATOS_NO_ENCOTRADOS;
                    //}
                    MostrarMensaje(Mensajes.MENSAJE_BD_GENERADA);
                    ObtieneUltimaFechaGeneracion();
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        private void MostrarMensaje(string mensaje, bool noalert = false, bool eserror = false)
        {
            if (!noalert)
            {
                ScriptManager.RegisterStartupScript(up, up.GetType(), "myAlert", "alert('" + mensaje.Replace("<br>", " - ") + "');", true);
            }
            //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(mensaje );
            if (eserror)
            {
                ErrorLabel.Font.Bold = true;
            }
            ErrorLabel.Text = mensaje;
        }

        public void ExportToExcel(string fileName, object datos, string path, string nombreDataSet)
        {

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.ServerReport.ReportServerUrl = new Uri("http://localhost/w004cac");
            //viewer.ServerReport.ReportPath = "/" + Server.MapPath("~/reportes/FormatoLiquidacion.rdlc"); ;
            //viewer.ServerReport.SetParameters(reportParams);
            viewer.LocalReport.ReportPath = Server.MapPath(path); //"~/reportes/FormatoLiquidacion.rdlc");
            ReportDataSource rds = new ReportDataSource(nombreDataSet, datos); //"DataSetLiquidacion", datos);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rds);
            //viewer.LocalReport.Refresh();


            byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
            //Response.End(); 
        }


        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }


        protected void generarButton_Click(object sender, EventArgs e)
        {
            try
            {
                GeneraReporte();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

    }
}