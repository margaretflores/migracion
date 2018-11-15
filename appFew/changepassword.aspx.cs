using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using appConstantes;
using appFew.appServicio;

namespace appFew
{
    public partial class changepassword : System.Web.UI.Page
    {
        ParametrosFe parametrosIni;

        protected void Page_Load(object sender, EventArgs e)
        {
            parametrosIni = (ParametrosFe)Session["ParametrosFe"];
            if (parametrosIni == null && Session["Data"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetNoStore();

                if (!Page.IsPostBack)
                {
                    if (parametrosIni != null)
                    {
                        _USUARIO.Text = parametrosIni.Usuario;
                    }
                    else 
                    {
                        _USUARIO.Text = (string)Session["Data"];
                        //Session["Data"] = null;
                    }
                    _USUARIO.Enabled = false;
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                CambiarContrasenia();
            }
            catch (Exception ex)
            {
                lblErrorLogin.Text = ex.Message;
            }
        }
        private void CambiarContrasenia()
        {
            if (ContrasenaNuevaTextBox.Text != RepetirContraseniaText.Text)
            {
                lblErrorLogin.Text="Las contraseñas ingresadas no coinciden";
                return;
            }
            parametrosIni = new ParametrosFe() { Usuario = _USUARIO.Text.ToUpper(), UserHostAddress = HttpContext.Current.Request.UserHostAddress };
            parametrosIni.IP4Address = FuncionesUtil.GetIP4Address(parametrosIni.UserHostAddress);

            IappServiceClient clt = parametrosIni.IniciaNuevoCliente();
            RESOPE resultado;
            //codigo de operacion
            PAROPE argumentos = new PAROPE();
            argumentos.CODOPE = CodigoOperacion.CAMBIAR_PASSWORD;
            //asigna parametros entrada en orden
            List<string> parEnt = new List<string>();
            parEnt.Add(parametrosIni.Usuario);  //0 usuario
            parEnt.Add(txtPassword.Text);       //1 clave, fase cifrar
            parEnt.Add(ContrasenaNuevaTextBox.Text);
            argumentos.VALENT = parEnt.ToArray();
            resultado = clt.EjecutaOperacion(argumentos);
            if (resultado.ESTOPE)
            {
                string HomePageUrl = "~/login.aspx";
                Response.Redirect(HomePageUrl);
            }
            else
            {
                lblErrorLogin.Text = resultado.MENERR;                
            }
            parametrosIni.FinalizaCliente(clt);
        }
    }
}