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
    
    public partial class PEBOLS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PEBOLS()
        {
            this.PEHIBO = new HashSet<PEHIBO>();
            this.PEHIMO = new HashSet<PEHIMO>();
            this.PEKABO = new HashSet<PEKABO>();
            this.PEBODP = new HashSet<PEBODP>();
        }
    
        public decimal BOLSIDBO { get; set; }
        public string BOLSCOAR { get; set; }
        public decimal BOLSIDTC { get; set; }
        public string BOLSCOEM { get; set; }
        public string BOLSCOCA { get; set; }
        public decimal BOLSALMA { get; set; }
        public decimal BOLSESTA { get; set; }
        public string BOLSUSCR { get; set; }
        public System.DateTime BOLSFECR { get; set; }
        public string BOLSUSMO { get; set; }
        public Nullable<System.DateTime> BOLSFEMO { get; set; }
        public string BOLSUSUB { get; set; }
        public Nullable<System.DateTime> BOLSFEUB { get; set; }
    
        public virtual PECASI PECASI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEHIBO> PEHIBO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEHIMO> PEHIMO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEKABO> PEKABO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEBODP> PEBODP { get; set; }
    }
}