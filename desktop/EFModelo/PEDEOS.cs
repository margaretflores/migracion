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
    
    public partial class PEDEOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PEDEOS()
        {
            this.PEHIMO = new HashSet<PEHIMO>();
            this.PEBODP = new HashSet<PEBODP>();
        }
    
        public decimal DEOSIDDO { get; set; }
        public decimal DEOSIDCO { get; set; }
        public decimal DEOSSECU { get; set; }
        public string DEOSFOLI { get; set; }
        public decimal DEOSPESO { get; set; }
        public decimal DEOSPEAT { get; set; }
        public decimal DEOSCAAT { get; set; }
        public decimal DEOSPERE { get; set; }
        public Nullable<decimal> DEOSSTOC { get; set; }
        public decimal DEOSESTA { get; set; }
        public string DEOSUSCR { get; set; }
        public System.DateTime DEOSFECR { get; set; }
        public string DEOSUSMO { get; set; }
        public Nullable<System.DateTime> DEOSFEMO { get; set; }
        public decimal DEOSTADE { get; set; }
        public decimal DEOSPEBR { get; set; }
        public string DEOSPART { get; set; }
        public string DEOSCOAR { get; set; }
        public decimal DEOSALMA { get; set; }
        public decimal DEOSPEOR { get; set; }
        public decimal DEOSCAOR { get; set; }
        public decimal DEOSESPA { get; set; }
        public decimal DEOSSECR { get; set; }
    
        public virtual PECAOS PECAOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEHIMO> PEHIMO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEBODP> PEBODP { get; set; }
    }
}