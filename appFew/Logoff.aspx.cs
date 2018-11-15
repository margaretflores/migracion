using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace appFew
{
    public partial class Logoff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Session["ParametrosFe"] = null;
                Session.Abandon();
            }
            catch { }
            //Response.Redirect("Login.aspx");
            //Server.Transfer("Login.aspx", true);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", true);
        }

        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            //Response.Redirect("Login.aspx", true);
        }
    }
}