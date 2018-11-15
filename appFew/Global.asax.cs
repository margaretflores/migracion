using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Configuration;

using appFew;

namespace appFew
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciarse la aplicación
            AuthConfig.RegisterOpenAuth();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta al cerrarse la aplicación

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se produce un error sin procesar

        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            string cs, jpg;
            if (ConfigurationManager.AppSettings["MaintenanceMode"] == "true")
            {
                //se pueden crear mas llaves para el nombre de la pagina, estilos e imagen
                cs ="Content/swagg.css".ToUpper();
                jpg = "Images/logo.jpg".ToUpper();

                //if (!Request.IsLocal && !allowedIPs.Contains(Request.UserHostAddress))
                //if (!Request.IsLocal)
                //{
                    if (!HttpContext.Current.Request.Url.LocalPath.ToUpper().Contains(cs) && !HttpContext.Current.Request.Url.LocalPath.ToUpper().Contains(jpg))
                    {
                        HttpContext.Current.RewritePath("~/maintenance.aspx");
                    }
                //}
            }
        }
    }
}
