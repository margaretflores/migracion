using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;

using appConstantes;

using appFew.appServicio;

namespace appFew
{
    public partial class _Login : System.Web.UI.Page
    {
        ParametrosFe parametrosIni;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                if (Session["ParametrosFe"] != null)
                {
                    //Session["ParametrosFe"] = null; 
                    //Session.Abandon();
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["ParametrosFe"] != null)
            {
                //Session["ParametrosFe"] = null;
                //Session.Abandon();
                Response.Redirect("default.aspx"); //20151203
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                InicioSesion();
            }
            catch (Exception ex)
            {
                lblErrorLogin.Text = parametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private void InicioSesion()
        {
            parametrosIni = new ParametrosFe() { Usuario = _USUARIO.Text.ToUpper(), UserHostAddress = HttpContext.Current.Request.UserHostAddress };
            parametrosIni.IP4Address = FuncionesUtil.GetIP4Address(parametrosIni.UserHostAddress);

            IappServiceClient clt = parametrosIni.IniciaNuevoCliente();
            RESOPE resultado;
            //codigo de operacion
            PAROPE argumentos = new PAROPE();
            argumentos.CODOPE = CodigoOperacion.VALIDA_USUARIO;
            //asigna parametros entrada en orden
            List<string> parEnt = new List<string>();
            parEnt.Add(parametrosIni.Usuario);  //0 usuario
            parEnt.Add(txtPassword.Text);       //1 clave, fase cifrar
            argumentos.VALENT = parEnt.ToArray();

            Session["Data"] = null;
            resultado = clt.EjecutaOperacion(argumentos);
            if (resultado.ESTOPE)
            {
                parametrosIni.NombreUsuario = resultado.VALSAL[1];
                parametrosIni.Menugeneral = ObtieneMenu(resultado.VALSAL);
                //apPaternoTextBox.Text = resultado.VALSAL[2];
                //apMaternoTextBox.Text = resultado.VALSAL[3];

                Session["ParametrosFe"] = parametrosIni;

                var returnUrl = Request.QueryString["ReturnURL"];
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "default.aspx"; // "~/";
                }
                Response.Redirect(returnUrl);
                //Response.Redirect("default.aspx");

            }
            else
            {
                lblErrorLogin.Text = resultado.MENERR;

                if (resultado.MENERR.Substring(0, 9) == "CWBSY0003")
                {
                    Session["Data"] = _USUARIO.Text;
                    string HomePageUrl = "changepassword.aspx";
                    //Page.Header.Controls.Add(new LiteralControl(string.Format("<META http-equiv=\"REFRESH\" content=\"2;url={0}\" > ", HomePageUrl)));
                    Response.Redirect(HomePageUrl);
                }

            }
            parametrosIni.FinalizaCliente(clt); 

        }

        private DataTable ObtieneMenu(string[] valRespuesta)
        {
            int cantidad;
            DataTable menuDataTable = new DataTable("GAMEGA");
            menuDataTable.Columns.Add(new DataColumn("MEAPMEPA", Type.GetType("System.String")));
            menuDataTable.Columns.Add(new DataColumn("MEAPCOME", Type.GetType("System.String")));
            menuDataTable.Columns.Add(new DataColumn("MEAPDEME", Type.GetType("System.String")));
            menuDataTable.Columns.Add(new DataColumn("MEAPMEOB", Type.GetType("System.String")));
            menuDataTable.Columns.Add(new DataColumn("MEAPMEIC", Type.GetType("System.String")));
            cantidad = int.Parse(valRespuesta[2]);
            for (int i = 3; i < cantidad + 3; i++)
            {
                string[] campos = valRespuesta[i].Split(new char[] { ',' });
                DataRow menuitem = menuDataTable.NewRow();
                menuitem["MEAPMEPA"] = campos[0];
                menuitem["MEAPCOME"] = campos[1];
                menuitem["MEAPDEME"] = campos[2];
                menuitem["MEAPMEOB"] = campos[3];
                menuitem["MEAPMEIC"] = campos[4];
                menuDataTable.Rows.Add(menuitem);
            }
            return menuDataTable;
        }
    }
}
