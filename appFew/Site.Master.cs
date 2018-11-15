using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Threading;
using System.Globalization;

using appConstantes;

namespace appFew
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        private ParametrosFe _IbParametrosFe;

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //this.MainContent.Controls.Add(new LiteralControl(String.Format("<meta http-equiv='refresh' content='{0};url={1}'>", SessionTimeOutWithin * 60, PageToSentAfterSessionTimeOut)));
            //this.MainContent.Controls.Add(new LiteralControl(String.Format("<meta http-equiv='refresh' content='{0};url={1}'>", 10, PageToSentAfterSessionTimeOut)));
        }

        public string PageToSentAfterSessionTimeOut
        {
            get { return "Logoff.aspx"; }
        }

        public int SessionTimeOutWithin
        {
            get {
                int sessionTimeOut;
                sessionTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["SessionTimeOut"]);
                return sessionTimeOut;
                
            } // Session.Timeout; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["ParametrosFe"] != null)
            {
                //lblUser.Text = Session["userCode"].ToString();
                //testing info del Business
                //imgLogo.ImageUrl = "Images/" + Session["BusinessEntityLogo"].ToString();
                //lblBusinessName.Text = Session["BusinessEntityBusinessName"].ToString();
                _IbParametrosFe = (ParametrosFe)(Session["ParametrosFe"]);
                MuestraUsuario(true);
                CargaDatosIniciales();
                //llenarMenu();
                CargaMenu();
            }
            else
            {
                MuestraUsuario(false);
            }
            //MuestraUsuario(true);
            //CargaDatosIniciales();
            //llenarMenu();
        }

        private void CargaDatosIniciales()
        {
            lblUser.Text = _IbParametrosFe.NombreUsuario;
            //tipocambiovalorc.Text = ibParametrosFe.TipoCambioCompra.ToString("#0.000");
            //tipocambiovalorv.Text = ibParametrosFe.TipoCambioVenta.ToString("#0.000");
            // Establezca la propiedad CurrentCulture en Inglés (Estados Unidos).
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-PE");
            fechaLabel.Text = DateTime.Today.ToLongDateString();
        }

        //private void CargaMenu()
        //{
        //    MenuItem item;
        //    MenuItem childMenuItem;
            
        //    DataRow[] menu, submenu;
        //    bool tieneSubMenus;

        //    menu = _IbParametrosFe.Menugeneral.Select("MEAPMEPA = '00'", "MEAPMEPA, MEAPCOME");
        //    //convertir a recursivo
        //    foreach (DataRow opcion in menu)
        //    {
        //        string codmenu;
        //            tieneSubMenus = false;
        //            item = new MenuItem();
        //            codmenu = Convert.ToString(opcion["MEAPCOME"]).Trim();
        //            item.Text = Convert.ToString(opcion["MEAPDEME"]).Trim();
        //            if (Convert.ToString(opcion["MEAPMEIC"]).Trim().Equals(""))
        //            {
        //                item.ImageUrl = "Images/MenuIcon/" + "g_usuario2.png";
        //            }
        //            else
        //            {
        //                item.ImageUrl = "Images/MenuIcon/" + Convert.ToString(opcion["MEAPMEIC"]).Trim(); // "Images/MenuIcon/" + "g_cliente.png";
        //            }

        //            submenu = menu = _IbParametrosFe.Menugeneral.Select("MEAPMEPA = '" + codmenu + "'", "MEAPMEPA, MEAPCOME");

        //            foreach (DataRow subopcion in submenu)
        //            {
        //                //subopcion1
        //                childMenuItem = new MenuItem();
        //                childMenuItem.Text = Convert.ToString(subopcion["MEAPDEME"]).Trim();
        //                childMenuItem.NavigateUrl = Convert.ToString(subopcion["MEAPMEOB"]).Trim(); //"~/ibpaginas/" + "posicion.aspx";
        //                item.ChildItems.Add(childMenuItem);
        //                tieneSubMenus = true;
        //            }
        //            if (tieneSubMenus)
        //            {
        //                MenuPortal.Items.Add(item);
        //            }
        //            else
        //            {
        //                item = null;
        //            }
        //    }


        //    ////opcion 1
        //    //item = new MenuItem();
        //    //item.Text = "Cobranzas";

        //    //item.ImageUrl = "Images/MenuIcon/" + "g_cliente.png";

        //    ////subopcion1
        //    //childMenuItem = new MenuItem();
        //    //childMenuItem.Text = "Envio emails";
        //    //childMenuItem.NavigateUrl = "~/ibpaginas/" + "posicion.aspx";
        //    //item.ChildItems.Add(childMenuItem);

        //    ////subopcion2
        //    //childMenuItem = new MenuItem();
        //    //childMenuItem.Text = "Consulta de emails";
        //    //childMenuItem.NavigateUrl = "~/ibpaginas/" + "envios.aspx";
        //    //item.ChildItems.Add(childMenuItem);

        //    //MenuPortal.Items.Add(item);

        //    ////opcion 2
        //    //item = new MenuItem();
        //    //item.Text = "Trámites";

        //    //item.ImageUrl = "Images/MenuIcon/" + "g_cliente.png";

        //    ////subopcion1
        //    //childMenuItem = new MenuItem();
        //    //childMenuItem.Text = "Envio emails";
        //    //childMenuItem.NavigateUrl = "~/ibpaginas/" + "posicion.aspx";
        //    //item.ChildItems.Add(childMenuItem);

        //    ////subopcion2
        //    //childMenuItem = new MenuItem();
        //    //childMenuItem.Text = "Consulta de emails";
        //    //childMenuItem.NavigateUrl = "~/ibpaginas/" + "envios.aspx";
        //    //item.ChildItems.Add(childMenuItem);

        //    //MenuPortal.Items.Add(item);
        //}

        private void CargaSubMenu(MenuItem item, string codMenuPadre)
        {
            MenuItem childMenuItem;
            DataRow[] menu, submenu;
            //tieneSubMenus = false;
            submenu = menu = _IbParametrosFe.Menugeneral.Select("MEAPMEPA = '" + codMenuPadre + "'", "MEAPMEPA, MEAPCOME");

            foreach (DataRow subopcion in submenu)
            {
                string codmenu;
                codmenu = Convert.ToString(subopcion["MEAPCOME"]).Trim();
                //subopcion1
                childMenuItem = new MenuItem();
                childMenuItem.Text = Convert.ToString(subopcion["MEAPDEME"]).Trim();
                childMenuItem.NavigateUrl = Convert.ToString(subopcion["MEAPMEOB"]).Trim(); //"~/ibpaginas/" + "posicion.aspx";
                item.ChildItems.Add(childMenuItem);
                //tieneSubMenus = true;
                CargaSubMenu(childMenuItem, codmenu);
            }

        }

        private void CargaMenu()
        {
            MenuItem item;
            MenuItem childMenuItem;

            DataRow[] menu, submenu;
            bool tieneSubMenus;

            menu = _IbParametrosFe.Menugeneral.Select("MEAPMEPA = '00'", "MEAPMEPA, MEAPCOME");
            //convertir a recursivo
            foreach (DataRow opcion in menu)
            {
                string codmenu;
                tieneSubMenus = false;
                item = new MenuItem();
                codmenu = Convert.ToString(opcion["MEAPCOME"]).Trim();
                item.Text = Convert.ToString(opcion["MEAPDEME"]).Trim();
                if (Convert.ToString(opcion["MEAPMEIC"]).Trim().Equals(""))
                {
                    item.ImageUrl = "Images/MenuIcon/" + "default.png";
                }
                else
                {
                    item.ImageUrl = "Images/MenuIcon/" + Convert.ToString(opcion["MEAPMEIC"]).Trim(); // "Images/MenuIcon/" + "g_cliente.png";
                }

                CargaSubMenu(item, codmenu);

                //submenu = menu = _IbParametrosFe.Menugeneral.Select("MEAPMEPA = '" + codmenu + "'", "MEAPMEPA, MEAPCOME");

                //foreach (DataRow subopcion in submenu)
                //{
                //    //subopcion1
                //    childMenuItem = new MenuItem();
                //    childMenuItem.Text = Convert.ToString(subopcion["MEAPDEME"]).Trim();
                //    childMenuItem.NavigateUrl = Convert.ToString(subopcion["MEAPMEOB"]).Trim(); //"~/ibpaginas/" + "posicion.aspx";
                //    item.ChildItems.Add(childMenuItem);
                //    tieneSubMenus = true;
                //}
                //if (tieneSubMenus)
                //{
                if (item.ChildItems.Count > 0)
                {
                    MenuPortal.Items.Add(item);
                }
                else
                {
                    item = null;
                }
            }
            //if (Session["OCULTA_MENU"] != null && Convert.ToBoolean( Session["OCULTA_MENU"]) )
            //{
            //    MenuPortal.Visible = false;
            //    //navMenu.Style["display"] = "none";
            //    navMenu.Style["width"] = "10px";
                
            //}
            //else
            //{
            //    MenuPortal.Visible = true;
            //    //navMenu.Style["display"] = "inline-block";
            //    navMenu.Style["width"] = "150px";

            //}

        }

        internal string ObtenerValor(XmlElement elementopadre, string nombredato)
        {
            XmlNodeList datos;
            string valor = "";
            datos = elementopadre.GetElementsByTagName(nombredato);
            if (datos.Count > 0)
            {
                valor = datos[0].InnerText.Trim();
            }
            return valor;
        }

        private void llenarMenu()
        {
            MenuItem item;
            MenuItem childMenuItem;
            //opcion 1
            item = new MenuItem();
            item.Text = "Compras";
            
            item.ImageUrl = "Images/MenuIcon/" + "g_cliente.png";

            //subopcion1
            childMenuItem = new MenuItem();
            childMenuItem.Text = "Adelantos";
            childMenuItem.NavigateUrl = "~/ibpaginas/" + "posicion.aspx";
            item.ChildItems.Add(childMenuItem);

            //subopcion2
            childMenuItem = new MenuItem();
            childMenuItem.Text = "Compras";
            childMenuItem.NavigateUrl = "~/ibpaginas/" + "posicion.aspx";
            item.ChildItems.Add(childMenuItem);

            MenuPortal.Items.Add(item);

            //opcion 2
            item = new MenuItem();
            item.Text = "Envio Masivo";
            item.ImageUrl = "Images/MenuIcon/" + "g_kiosk.png";

            //subopcion1
            childMenuItem = new MenuItem();
            childMenuItem.Text = "Envios";
            childMenuItem.NavigateUrl = "~/ibpaginas/" + "pagocredito.aspx";
            item.ChildItems.Add(childMenuItem);

            MenuPortal.Items.Add(item);

            ////opcion 3
            //item = new MenuItem();
            //item.Text = "Pago de Servicios";
            //item.ImageUrl = "Images/MenuIcon/" + "g_usuario.png";

            ////subopcion2
            //childMenuItem = new MenuItem();
            //childMenuItem.Text = "Recargas";
            //childMenuItem.NavigateUrl = "~/ibpaginas/" + "recarga" + ".aspx";
            //item.ChildItems.Add(childMenuItem);

            //MenuPortal.Items.Add(item);

            //DataSet ds = CommonBUSINESS.ShowDataSet(StringConn.StringConnection.CadenaConexion, Texts.STPR_MENU_SELECT);

            //ds.Relations.Add("ChildRows", ds.Tables[0].Columns["MenuCode"], ds.Tables[1].Columns["MenuCode"]);

            //foreach (DataRow level1DataRow in ds.Tables[0].Rows)
            //{
            //    MenuItem item = new MenuItem();
            //    item.Text = level1DataRow["MenuDescription"].ToString();
            //    item.ImageUrl = "Images/MenuIcon/" + level1DataRow["MenuIcono"].ToString();

            //    DataRow[] level2DataRows = level1DataRow.GetChildRows("ChildRows");

            //    foreach (DataRow level2DataRow in level2DataRows)
            //    {
            //        MenuItem childMenuItem = new MenuItem();
            //        childMenuItem.Text = level2DataRow["MenuItemDescription"].ToString();
            //        childMenuItem.NavigateUrl = level2DataRow["MenuItemUrl"].ToString();

            //        item.ChildItems.Add(childMenuItem);
            //    }

            //    MenuPortal.Items.Add(item);
            //}
        }
        
        private void MuestraUsuario(bool show)
        {
            var div_nombre_usuario = this.FindControl("div_nombre_usuario") as System.Web.UI.HtmlControls.HtmlControl;
            var div_foto_usuario = this.FindControl("div_foto_usuario") as System.Web.UI.HtmlControls.HtmlControl;
            var div_ultima_conexion = this.FindControl("div_ultima_conexion") as System.Web.UI.HtmlControls.HtmlControl;
            if (div_nombre_usuario != null)
            {
                if (show)
                {
                    div_nombre_usuario.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    //div_foto_usuario.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    div_ultima_conexion.Style[HtmlTextWriterStyle.Visibility] = "visible";
                }
                else
                {
                    div_nombre_usuario.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    div_foto_usuario.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    div_ultima_conexion.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                }

            }

        }

    }
}