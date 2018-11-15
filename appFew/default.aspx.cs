using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace appFew
{
    public partial class _default : System.Web.UI.Page
    {
        private ParametrosFe _IBConexion;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            //Response.Cache.SetAllowResponseInBrowserHistory(false);
            //Response.Cache.SetNoStore();

            _IBConexion = (ParametrosFe)Session["ParametrosFe"];

            if (Session["ParametrosFe"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    //Response.Redirect(_IBConexion.PaginaInicial); // "~/posicion/posicioncLiente.aspx");
                }
            }

            if (!this.IsPostBack)
            {
                //MenuItemCollection listOpciones = new MenuItemCollection();
                ////foreach (DataRow row in ds.Tables[1].Rows)
                ////{
                //    listOpciones.Add(new MenuItem { Text = "Posición Cliente", Value = "P001", NavigateUrl = "~/content/" + "clientposition" + ".aspx" });
                //    MenuItem item2 = new MenuItem { Text = "Pagos", Value = "P002", Selectable = false };

                //    item2.ChildItems.Add(new MenuItem { Text = "Pago Crédito", Value = "P0021", NavigateUrl = "~/content/" + "PagoCreditos" + ".aspx" });
                //    item2.ChildItems.Add(new MenuItem { Text = "Pago Servicios", Value = "P0022", NavigateUrl = "~/content/" + "PagoServicios" + ".aspx" });
                //    item2.ChildItems.Add(new MenuItem { Text = "Pago Institucional", Value = "P0023", NavigateUrl = "~/content/" + "PagoInstitucional" + ".aspx" });
                //    listOpciones.Add(item2);
                //    //}

                //Session["OpcionesMenu"] = listOpciones;
                //colocar el uri en configuracion
            }
        }
    }
}
