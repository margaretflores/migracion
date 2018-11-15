using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appFew.dto
{
    public class CcLote
    {

        public int LoteCia { get; set; }

        public string LoteIdlo { get; set; }

        public string LoteEsta { get; set; }

        // propiedades para proposito de solo reporte
        public double TotalAcumulado { get; set; }

        public List<DetalleLote> Detalle { get; set; }
    }
}