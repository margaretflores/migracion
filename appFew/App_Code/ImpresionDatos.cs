using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew
{
    public class ImpresionDatos
    {
        public enum ADestino
        {
            AExcel,
            APDF,
            AReporte
        }

        public ADestino Destino { get; set; }
        public object Datos { get; set; }
        public string Path { get; set; }
        public string NombreDataSet { get; set; }
        public string NombreArchivo { get; set; }

        public ImpresionDatos()
        {
            NombreArchivo = "";
        }
    }
}