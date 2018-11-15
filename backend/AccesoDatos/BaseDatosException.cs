using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccesoDatos
{
    public class BaseDatosException : ApplicationException
    {

        public BaseDatosException(String mensaje, Exception original)
            : base(mensaje, original)
        {

        }


        public BaseDatosException(String mensaje)
            : base(mensaje)
        {

        }

    }
}
