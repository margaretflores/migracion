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
    
    public partial class PEUSTF
    {
        public decimal USTFIDUF { get; set; }
        public string USTFUSUA { get; set; }
        public decimal USTFIDTF { get; set; }
        public string USTFUSCR { get; set; }
        public System.DateTime USTFFECR { get; set; }
        public string USTFUSMO { get; set; }
        public Nullable<System.DateTime> USTFFEMO { get; set; }
        public bool USTFSUUS { get; set; }
    
        public virtual PETIFO PETIFO { get; set; }
    }
}
