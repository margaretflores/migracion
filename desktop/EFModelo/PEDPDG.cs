//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFModelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class PEDPDG
    {
        public decimal DPDGIDDD { get; set; }
        public decimal DPDGIDDP { get; set; }
        public decimal DPDGIDDG { get; set; }
        public string DPDGUSCR { get; set; }
        public System.DateTime DPDGFECR { get; set; }
        public string DPDGUSMO { get; set; }
        public Nullable<System.DateTime> DEPDGFEMO { get; set; }
    
        public virtual PEDEPE PEDEPE { get; set; }
    }
}