using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.IO;

namespace appWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class appService : IappService
    {
        appLanzador.Principal Lanzador;

        public RESOPE EjecutaOperacion(PAROPE paramOperacion)
        {
            RESOPE resultado;
            Lanzador = new appLanzador.Principal();
            resultado = Lanzador.EjecutaOperacion(paramOperacion);
            Lanzador = null;
            return resultado;
        }

    }
}
