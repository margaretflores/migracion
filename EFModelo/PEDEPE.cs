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
    
    public partial class PEDEPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PEDEPE()
        {
            this.PEDPDG = new HashSet<PEDPDG>();
            this.PEHIMO = new HashSet<PEHIMO>();
            this.PEBODP = new HashSet<PEBODP>();
        }
    
        public decimal DEPEIDDP { get; set; }
        public decimal DEPEIDCP { get; set; }
        public string DEPECOAR { get; set; }
        public string DEPEPART { get; set; }
        public string DEPECONT { get; set; }
        public decimal DEPEALMA { get; set; }
        public decimal DEPECASO { get; set; }
        public decimal DEPEPESO { get; set; }
        public decimal DEPECAAT { get; set; }
        public decimal DEPEPEAT { get; set; }
        public decimal DEPEPERE { get; set; }
        public decimal DEPETADE { get; set; }
        public decimal DEPEPEBR { get; set; }
        public Nullable<decimal> DEPESTOC { get; set; }
        public decimal DEPEESTA { get; set; }
        public string DEPEUSCR { get; set; }
        public System.DateTime DEPEFECR { get; set; }
        public string DEPEUSMO { get; set; }
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        public decimal DEPEDISP { get; set; }
        public string DEPEDSAR { get; set; }
        public decimal DEPENUBU { get; set; }
        public int DEPESERS { get; set; }
        public decimal DEPESECR { get; set; }
        public decimal DEPESECU { get; set; }
    
        public virtual I1DD20A I1DD20A { get; set; }
        public virtual PECAPE PECAPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDPDG> PEDPDG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEHIMO> PEHIMO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEBODP> PEBODP { get; set; }
    }
}
