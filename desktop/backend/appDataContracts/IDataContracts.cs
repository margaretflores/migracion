using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace appWcfService
{

    [DataContract]
    public class RESOPE
    {
        [DataMember]
        public bool ESTOPE { get; set; }
        [DataMember]
        public string MENERR { get; set; }
        [DataMember]
        public List<string> VALSAL { get; set; }
    }

    [DataContract]
    public class PAROPE
    {
        [DataMember]
        public string CODOPE { get; set; }
        [DataMember]
        public List<string> VALENT { get; set; }
    }

    //[DataContract]
    //public class PECAPE
    //{
    //    [DataMember]
    //    public decimal CAPEIDCP { get; set; }
    //    [DataMember]
    //    public string CAPESERI { get; set; }
    //    [DataMember]
    //    public string CAPENUME { get; set; }
    //    [DataMember]
    //    public string CAPEIDCL { get; set; }
    //    [DataMember]
    //    public System.DateTime CAPEFECH { get; set; }
    //    [DataMember]
    //    public decimal CAPEIDES { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> CAPEPRIO { get; set; }
    //    [DataMember]
    //    public Nullable<System.DateTime> CAPEFEPR { get; set; }
    //    [DataMember]
    //    public string CAPEUSPR { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> CAPEPOCO { get; set; }
    //    [DataMember]
    //    public string CAPENOTI { get; set; }
    //    [DataMember]
    //    public string CAPENOTG { get; set; }
    //    [DataMember]
    //    public string CAPEUSCR { get; set; }
    //    [DataMember]
    //    public System.DateTime CAPEFECR { get; set; }
    //    [DataMember]
    //    public string CAPEUSMO { get; set; }
    //    [DataMember]
    //    public Nullable<System.DateTime> CAPEFEMO { get; set; }
    //    [DataMember]
    //    public TCLIE TCLIE { get; set; }
    //}

    //[DataContract]
    //public class TCLIE
    //{
    //    [DataMember]
    //    public string CLICVE { get; set; }
    //    [DataMember]
    //    public string CLINOM { get; set; }
    //    [DataMember]
    //    public string CLIABR { get; set; }
    //    [DataMember]
    //    public string CLIDIR { get; set; }
    //    [DataMember]
    //    public string CLICPO { get; set; }
    //    [DataMember]
    //    public string CLIDIS { get; set; }
    //    [DataMember]
    //    public string CLIPRO { get; set; }
    //    [DataMember]
    //    public string CLIDPT { get; set; }
    //    [DataMember]
    //    public string CLIPAI { get; set; }
    //    [DataMember]
    //    public string CLITE1 { get; set; }
    //    [DataMember]
    //    public string CLITE2 { get; set; }
    //    [DataMember]
    //    public string CLITE3 { get; set; }
    //    [DataMember]
    //    public string CLITE4 { get; set; }
    //    [DataMember]
    //    public string CLIRUC { get; set; }
    //    [DataMember]
    //    public string CLIRF1 { get; set; }
    //    [DataMember]
    //    public string CLIRF2 { get; set; }
    //    [DataMember]
    //    public string CLIRF3 { get; set; }
    //    [DataMember]
    //    public string CLIVEN { get; set; }
    //    [DataMember]
    //    public string CLICOB { get; set; }
    //    [DataMember]
    //    public string CLIZON { get; set; }
    //    [DataMember]
    //    public string CLISIT { get; set; }
    //    [DataMember]
    //    public decimal CLILCR { get; set; }
    //    [DataMember]
    //    public decimal CLISAL { get; set; }
    //    [DataMember]
    //    public string CLISCR { get; set; }
    //    [DataMember]
    //    public string CPACVE { get; set; }
    //    [DataMember]
    //    public decimal CLDSCT { get; set; }
    //    [DataMember]
    //    public string CLPREC { get; set; }
    //    [DataMember]
    //    public string CLCNOM { get; set; }
    //    [DataMember]
    //    public string CLCPUE { get; set; }
    //    [DataMember]
    //    public string CLMAIL { get; set; }
    //    [DataMember]
    //    public string CLTIDE { get; set; }
    //    [DataMember]
    //    public string CLNIDE { get; set; }

    //}

    //[DataContract]
    //public class PEDEPE
    //{
    //    [DataMember]
    //    public decimal DEPEIDDP { get; set; }
    //    [DataMember]
    //    public decimal DEPEIDCP { get; set; }
    //    [DataMember]
    //    public string DEPECOAR { get; set; }
    //    [DataMember]
    //    public string DEPEPART { get; set; }
    //    [DataMember]
    //    public string DEPECONT { get; set; }
    //    [DataMember]
    //    public decimal DEPEALMA { get; set; }
    //    [DataMember]
    //    public decimal DEPECASO { get; set; }
    //    [DataMember]
    //    public decimal DEPEPESO { get; set; }
    //    [DataMember]
    //    public decimal DEPECAAT { get; set; }
    //    [DataMember]
    //    public decimal DEPEPEAT { get; set; }
    //    [DataMember]
    //    public decimal DEPEPERE { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> DEPESTOC { get; set; }
    //    [DataMember]
    //    public decimal DEPEESTA { get; set; }
    //    [DataMember]
    //    public string DEPEUSCR { get; set; }
    //    [DataMember]
    //    public System.DateTime DEPEFECR { get; set; }
    //    [DataMember]
    //    public string DEPEUSMO { get; set; }
    //    [DataMember]
    //    public Nullable<System.DateTime> DEPEFEMO { get; set; }
    //    //[DataMember]
    //    //public PECAPE PECAPE { get; set; }
    //    [DataMember]
    //    public I1DD20A I1DD20A { get; set; }
    //}

    //[DataContract]
    //public class I1DD20A
    //{
    //    [DataMember]
    //    public string ARTX0 { get; set; }
    //    [DataMember]
    //    public decimal ARTCIA { get; set; }
    //    [DataMember]
    //    public string ARTCITEM { get; set; }
    //    [DataMember]
    //    public string ARTDES { get; set; }
    //    [DataMember]
    //    public string ARTMED { get; set; }
    //    [DataMember]
    //    public string ARTGRINV { get; set; }
    //    [DataMember]
    //    public string ARTGIRO { get; set; }
    //    [DataMember]
    //    public string ARTGRCOM { get; set; }
    //    [DataMember]
    //    public string ARTCCOMP { get; set; }
    //    [DataMember]
    //    public decimal ARTCCONS { get; set; }
    //    [DataMember]
    //    public decimal ARTCANA1 { get; set; }
    //    [DataMember]
    //    public decimal ARTCANA2 { get; set; }
    //    [DataMember]
    //    public string ARTCLINV { get; set; }
    //    [DataMember]
    //    public string ARTCIMP1 { get; set; }
    //    [DataMember]
    //    public string ARTCIMP2 { get; set; }
    //    [DataMember]
    //    public string ARTCIMP3 { get; set; }
    //    [DataMember]
    //    public string ARTCPLTR { get; set; }
    //    [DataMember]
    //    public string ARTUPLTR { get; set; }
    //    [DataMember]
    //    public decimal ARTSTSEG { get; set; }
    //    [DataMember]
    //    public decimal ARTPREOR { get; set; }
    //    [DataMember]
    //    public decimal ARTTLOTE { get; set; }
    //    [DataMember]
    //    public decimal ARTSTMAX { get; set; }
    //    [DataMember]
    //    public decimal ARTLDREV { get; set; }
    //    [DataMember]
    //    public decimal ARTFUREV { get; set; }
    //    [DataMember]
    //    public decimal ARTTREXT { get; set; }
    //    [DataMember]
    //    public string ARTTHIST { get; set; }
    //    [DataMember]
    //    public string ARTTPROM { get; set; }
    //    [DataMember]
    //    public decimal ARTPMVL1 { get; set; }
    //    [DataMember]
    //    public decimal ARTPMVL2 { get; set; }
    //    [DataMember]
    //    public decimal ARTLDCNT { get; set; }
    //    [DataMember]
    //    public decimal ARTFUCNT { get; set; }
    //    [DataMember]
    //    public decimal ARTSTOCK { get; set; }
    //    [DataMember]
    //    public decimal ARTSTANT { get; set; }
    //    [DataMember]
    //    public decimal ARTCANOR { get; set; }
    //    [DataMember]
    //    public decimal ARTCANRE { get; set; }
    //    [DataMember]
    //    public decimal ARTPSALM { get; set; }
    //    [DataMember]
    //    public decimal ARTAASAN { get; set; }
    //    [DataMember]
    //    public decimal ARTSTVA1 { get; set; }
    //    [DataMember]
    //    public decimal ARTCOSU1 { get; set; }
    //    [DataMember]
    //    public decimal ARTCOSU2 { get; set; }
    //    [DataMember]
    //    public string ARTTCOSR { get; set; }
    //    [DataMember]
    //    public decimal ARTCOUM1 { get; set; }
    //    [DataMember]
    //    public decimal ARTCOUM2 { get; set; }
    //    [DataMember]
    //    public string ARTTMAT { get; set; }
    //    [DataMember]
    //    public decimal ARTPREU1 { get; set; }
    //    [DataMember]
    //    public decimal ARTPREU2 { get; set; }
    //    [DataMember]
    //    public string ARTCDSRC { get; set; }
    //    [DataMember]
    //    public decimal ARTSTVA2 { get; set; }
    //    [DataMember]
    //    public string ARTX1 { get; set; }
    //    [DataMember]
    //    public decimal ARTFUSAL { get; set; }
    //    [DataMember]
    //    public decimal ARTFUENT { get; set; }
    //    [DataMember]
    //    public decimal ARTFUREQ { get; set; }
    //    [DataMember]
    //    public decimal ARTFUOC { get; set; }
    //    [DataMember]
    //    public decimal ARTFALTA { get; set; }
    //    [DataMember]
    //    public decimal ARTFBAJA { get; set; }
    //    [DataMember]
    //    public decimal ARTUPTRN { get; set; }
    //    [DataMember]
    //    public string ARTCACT { get; set; }
    //    [DataMember]
    //    public decimal ARTAASAE { get; set; }
    //    [DataMember]
    //    public decimal ARTUPROV { get; set; }
    //    [DataMember]
    //    public string ARTCMON { get; set; }
    //    [DataMember]
    //    public string ARTMEDA { get; set; }
    //    [DataMember]
    //    public decimal ARTEQMED { get; set; }
    //    [DataMember]
    //    public string ARTUBI { get; set; }
    //    [DataMember]
    //    public decimal ARTSTOCM { get; set; }
    //    [DataMember]
    //    public decimal ARTCANOM { get; set; }
    //    [DataMember]
    //    public decimal ARTCANRM { get; set; }
    //    [DataMember]
    //    public string ARTACTEM { get; set; }
    //    [DataMember]
    //    public string ARTACTOM { get; set; }
    //    [DataMember]
    //    public string ARTACTRM { get; set; }
    //    [DataMember]
    //    public decimal ARTCRSG { get; set; }
    //    [DataMember]
    //    public decimal ARTNRELM { get; set; }
    //    [DataMember]
    //    public decimal ARTNRELD { get; set; }
    //    [DataMember]
    //    public decimal ARTNRELH { get; set; }
    //    [DataMember]
    //    public string ARTFCTL { get; set; }
    //}

    //[DataContract]
    //public class PEUBIC
    //{
    //    [DataMember]
    //    public string UBICCOCA { get; set; }
    //    [DataMember]
    //    public decimal UBICCANT { get; set; }
    //}

    //[DataContract]
    //public class PEESPE
    //{
    //    [DataMember]
    //    public string ESPEIDPE { get; set; }
    //    [DataMember]
    //    public string ESPEIDES { get; set; }
    //}

    //[DataContract]
    //public partial class USP_OBTIENE_BOLSA_Result
    //{
    //    [DataMember]
    //    public decimal DEPEIDDP { get; set; }
    //    [DataMember]
    //    public decimal BOLSIDBO { get; set; }
    //    [DataMember]
    //    public string BOLSCOEM { get; set; }
    //    [DataMember]
    //    public string BOLSCOCA { get; set; }
    //    [DataMember]
    //    public string BOLSARTI { get; set; }
    //    [DataMember]
    //    public string BOLSPART { get; set; }
    //    [DataMember]
    //    public decimal BOLSALMA { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSCANT { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSPESO { get; set; }
    //    [DataMember]
    //    public decimal TIEMTARA { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> UNIDTARA { get; set; }

    //}

    //[DataContract]
    //public partial class USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result
    //{
    //    [DataMember]
    //    public Nullable<decimal> DEPEIDDP { get; set; }
    //    [DataMember]
    //    public decimal BOLSIDBO { get; set; }
    //    [DataMember]
    //    public string BOLSCOEM { get; set; }
    //    [DataMember]
    //    public string BOLSCOCA { get; set; }
    //    [DataMember]
    //    public string BOLSARTI { get; set; }
    //    [DataMember]
    //    public string BOLSPART { get; set; }
    //    [DataMember]
    //    public decimal BOLSALMA { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSCANT { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSPESO { get; set; }
    //    [DataMember]
    //    public decimal BODPIDDE { get; set; }
    //    [DataMember]
    //    public decimal BODPIDBO { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BODPIDDP { get; set; }
    //    [DataMember]
    //    public decimal BODPCANT { get; set; }
    //    [DataMember]
    //    public decimal BODPPESO { get; set; }
    //    [DataMember]
    //    public decimal TIEMTARA { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> UNIDTARA { get; set; }
    //    [DataMember]
    //    public decimal BODPPERE { get; set; }
    //    [DataMember]
    //    public decimal BODPSTCE { get; set; }
    //    [DataMember]
    //    public decimal BODPINBO { get; set; }
    //    [DataMember]
    //    public decimal BODPDIFE { get; set; }

    //}

    //[DataContract]
    //public partial class PEBOLS
    //{
    //    [DataMember]
    //    public decimal BOLSIDBO { get; set; }
    //    [DataMember]
    //    public string BOLSCOAR { get; set; }
    //    [DataMember]
    //    public decimal BOLSIDTC { get; set; }
    //    [DataMember]
    //    public string BOLSCOEM { get; set; }
    //    [DataMember]
    //    public string BOLSCOCA { get; set; }
    //    [DataMember]
    //    public decimal BOLSESTA { get; set; }
    //    [DataMember]
    //    public string BOLSUSCR { get; set; }
    //    [DataMember]
    //    public System.DateTime BOLSFECR { get; set; }
    //    [DataMember]
    //    public string BOLSUSMO { get; set; }
    //    [DataMember]
    //    public Nullable<System.DateTime> BOLSFEMO { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSCANT { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BOLSPESO { get; set; }

    //}

    //[DataContract]
    //public partial class PEBODP
    //{
    //    [DataMember]
    //    public decimal BODPIDDE { get; set; }
    //    [DataMember]
    //    public decimal BODPIDBO { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BODPIDDP { get; set; }
    //    [DataMember]
    //    public decimal BODPCANT { get; set; }
    //    [DataMember]
    //    public decimal BODPPESO { get; set; }
    //    [DataMember]
    //    public decimal BODPPERE { get; set; }
    //    [DataMember]
    //    public decimal BODPDIFE { get; set; }
    //    [DataMember]
    //    public Nullable<decimal> BODPIDDO { get; set; }
    //    [DataMember]
    //    public decimal BODPSTCE { get; set; }
    //    [DataMember]
    //    public decimal BODPINBO { get; set; }
    //    [DataMember]
    //    public string BODPUSCR { get; set; }
    //    [DataMember]
    //    public System.DateTime BODPFECR { get; set; }
    //    [DataMember]
    //    public string BODPUSMO { get; set; }
    //    [DataMember]
    //    public Nullable<System.DateTime> BODPFEMO { get; set; }
    //    [DataMember]
    //    public PEBOLS PEBOLS { get; set; }
    //    [DataMember]
    //    public PEDEPE PEDEPE { get; set; }

    //}

}
