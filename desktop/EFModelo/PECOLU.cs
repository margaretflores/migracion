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
    
    public partial class PECOLU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PECOLU()
        {
            this.PECASI = new HashSet<PECASI>();
        }
    
        public decimal COLUIDCO { get; set; }
        public decimal COLUIDPA { get; set; }
        public decimal COLUESTA { get; set; }
        public string COLUUSCR { get; set; }
        public System.DateTime COLUFECR { get; set; }
        public string COLUUSMO { get; set; }
        public Nullable<System.DateTime> COLUFEMO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PECASI> PECASI { get; set; }
        public virtual PEPASI PEPASI { get; set; }
    }
}
