using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;

using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using appFew.appServicio;
using appWcfService;
using appConstantes;
using appFew;

namespace appFew.plan
{
    public partial class peinado : System.Web.UI.Page
    {
        private ParametrosFe _ParametrosIni;
        private string Error_1 = string.Empty;
        private string url = string.Empty;
        //private string lote;

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
                        //lote = Request.QueryString["lote"];                        
                        ObtenerExistencias();
                    }
                    else
                    {
                        //GUARDA EN LAS VARIABLES DE SESION EL RANGO DE FECHAS
                        //txt_fechafin.Text = "";
                        //txt_fechaini.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    Error_1 = "Ha ocurrido un error en la pagina.";
                    url = "ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + ex.Message;
                    Response.Redirect(url);
                }

            }
        }

        private void ObtenerExistencias()
        {
            IappServiceClient clt = null;
            List<BDPLPR> _bdplpr = null;

            try
            {
                clt = _ParametrosIni.IniciaNuevoCliente();
                List<string> parametros = new List<string>();
                parametros = new List<string>();
                parametros.Add(Convert.ToString( Constantes.CODIGO_LINEA_PEINADO_ZAM));

                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GENERA_INFO_PLAN_PRODUCCION;
                argumentos.VALENT = parametros.ToArray();
                RESOPE resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    _bdplpr = appFew.FuncionesUtil.Deserialize<List<BDPLPR>>(resultado.VALSAL[0]);

                    //var viewer = new ReportViewer();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/rdlc/Peinado.rdlc");
                    ReportDataSource rds = new ReportDataSource("DataSetPlanProduccion", _bdplpr);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);

                    //string strRuta = (Request.Url.Scheme + (Uri.SchemeDelimiter + (Request.Url.Authority + (Request.ApplicationPath + "/compras/"))));
                    //strRuta = strRuta.Replace("//compras", "/compras");

                    //ReportParameter[] listaParam = new ReportParameter[1];
                    //ReportParameter param = new ReportParameter("Ruta", strRuta);
                    //listaParam[0] = param;
                    //ReportViewer1.LocalReport.SetParameters(listaParam);
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }

            }
            catch (Exception ex)
            {
                errorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
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
                //var clientScript = Page.ClientScript;
                //ClientScriptManager cs = Page.ClientScript;
                //StringBuilder msj = new StringBuilder();
                //cs.RegisterStartupScript(this.GetType(), "alert", "bootbox.alert('" + mensaje + "');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "bootbox.alert('" + mensaje + "');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + mensaje.Replace("<br>", " - ") + "');", true);

            }
            if (eserror)
            {
                errorLabel.Font.Bold = true;
            }
            errorLabel.Text = mensaje;
        }
    }
}