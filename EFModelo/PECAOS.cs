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
    
    public partial class PECAOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PECAOS()
        {
            this.PEDEOS = new HashSet<PEDEOS>();
        }
    
        public decimal CAOSIDCO { get; set; }
        public string CAOSFOLI { get; set; }
        public decimal CAOSIDES { get; set; }
        public decimal CAOSPRIO { get; set; }
        public string CAOSUSCR { get; set; }
        public System.DateTime CAOSFECR { get; set; }
        public string CAOSUSMO { get; set; }
        public Nullable<System.DateTime> CAOSFEMO { get; set; }
        public decimal CAOSEPRI { get; set; }
        public Nullable<System.DateTime> CAOSFEPR { get; set; }
        public string CAOSUSPR { get; set; }
        public decimal CAOSNUBU { get; set; }
        public string CAOSUSIP { get; set; }
        public Nullable<System.DateTime> CAOSFHIP { get; set; }
        public string CAOSUSFP { get; set; }
        public Nullable<System.DateTime> CAOSFHFP { get; set; }
        public string CAOSUSAP { get; set; }
        public Nullable<System.DateTime> CAOSFEAP { get; set; }
        public decimal CAOSEXTO { get; set; }
        public string CAOSNOTA { get; set; }
    
        public virtual PEESTA PEESTA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDEOS> PEDEOS { get; set; }
    }
}
