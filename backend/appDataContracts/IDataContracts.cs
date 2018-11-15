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

    [DataContract]
    public class PECAPE
    {
        [DataMember]
        public decimal CAPEIDCP { get; set; }
        [DataMember]
        public string CAPESERI { get; set; }
        [DataMember]
        public decimal CAPENUME { get; set; }
        [DataMember]
        public string CAPEIDCL { get; set; }
        [DataMember]
        public System.DateTime CAPEFECH { get; set; }
        [DataMember]
        public decimal CAPEIDES { get; set; }
        [DataMember]
        public Nullable<decimal> CAPEPRIO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CAPEFEPR { get; set; }
        [DataMember]
        public string CAPEUSPR { get; set; }
        [DataMember]
        public Nullable<decimal> CAPEPOCO { get; set; }
        [DataMember]
        public string CAPENOTI { get; set; }
        [DataMember]
        public string CAPENOTG { get; set; }
        [DataMember]
        public string CAPEUSCR { get; set; }
        [DataMember]
        public System.DateTime CAPEFECR { get; set; }
        [DataMember]
        public string CAPEUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CAPEFEMO { get; set; }
        [DataMember]
        public TCLIE TCLIE { get; set; }
        [DataMember]
        public decimal CAPETADE { get; set; }

    }

    [DataContract]
    public class TCLIE
    {
        [DataMember]
        public string CLICVE { get; set; }
        [DataMember]
        public string CLINOM { get; set; }
        [DataMember]
        public string CLIABR { get; set; }
        [DataMember]
        public string CLIDIR { get; set; }
        [DataMember]
        public string CLICPO { get; set; }
        [DataMember]
        public string CLIDIS { get; set; }
        [DataMember]
        public string CLIPRO { get; set; }
        [DataMember]
        public string CLIDPT { get; set; }
        [DataMember]
        public string CLIPAI { get; set; }
        [DataMember]
        public string CLITE1 { get; set; }
        [DataMember]
        public string CLITE2 { get; set; }
        [DataMember]
        public string CLITE3 { get; set; }
        [DataMember]
        public string CLITE4 { get; set; }
        [DataMember]
        public string CLIRUC { get; set; }
        [DataMember]
        public string CLIRF1 { get; set; }
        [DataMember]
        public string CLIRF2 { get; set; }
        [DataMember]
        public string CLIRF3 { get; set; }
        [DataMember]
        public string CLIVEN { get; set; }
        [DataMember]
        public string CLICOB { get; set; }
        [DataMember]
        public string CLIZON { get; set; }
        [DataMember]
        public string CLISIT { get; set; }
        [DataMember]
        public decimal CLILCR { get; set; }
        [DataMember]
        public decimal CLISAL { get; set; }
        [DataMember]
        public string CLISCR { get; set; }
        [DataMember]
        public string CPACVE { get; set; }
        [DataMember]
        public decimal CLDSCT { get; set; }
        [DataMember]
        public string CLPREC { get; set; }
        [DataMember]
        public string CLCNOM { get; set; }
        [DataMember]
        public string CLCPUE { get; set; }
        [DataMember]
        public string CLMAIL { get; set; }
        [DataMember]
        public string CLTIDE { get; set; }
        [DataMember]
        public string CLNIDE { get; set; }

    }

    [DataContract]
    public class PEDEPE
    {
        [DataMember]
        public decimal DEPEIDDP { get; set; }
        [DataMember]
        public decimal DEPEIDCP { get; set; }
        [DataMember]
        public string DEPECOAR { get; set; }
        [DataMember]
        public string DEPEPART { get; set; }
        [DataMember]
        public string DEPECONT { get; set; }
        [DataMember]
        public decimal DEPEALMA { get; set; }
        [DataMember]
        public decimal DEPECASO { get; set; }
        [DataMember]
        public decimal DEPEPESO { get; set; }
        [DataMember]
        public decimal DEPECAAT { get; set; }
        [DataMember]
        public decimal DEPEPEAT { get; set; }
        [DataMember]
        public decimal DEPEPERE { get; set; }
        [DataMember]
        public decimal DEPETADE { get; set; }
        [DataMember]
        public decimal DEPEPEBR { get; set; }
        [DataMember]
        public Nullable<decimal> DEPESTOC { get; set; }
        [DataMember]
        public decimal DEPEESTA { get; set; }
        [DataMember]
        public string DEPEUSCR { get; set; }
        [DataMember]
        public System.DateTime DEPEFECR { get; set; }
        [DataMember]
        public string DEPEUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        //[DataMember]
        //public PECAPE PECAPE { get; set; }
        [DataMember]
        public I1DD20A I1DD20A { get; set; }
    }

    [DataContract]
    public class I1DD20A
    {
        [DataMember]
        public string ARTX0 { get; set; }
        [DataMember]
        public decimal ARTCIA { get; set; }
        [DataMember]
        public string ARTCITEM { get; set; }
        [DataMember]
        public string ARTDES { get; set; }
        [DataMember]
        public string ARTMED { get; set; }
        [DataMember]
        public string ARTGRINV { get; set; }
        [DataMember]
        public string ARTGIRO { get; set; }
        [DataMember]
        public string ARTGRCOM { get; set; }
        [DataMember]
        public string ARTCCOMP { get; set; }
        [DataMember]
        public decimal ARTCCONS { get; set; }
        [DataMember]
        public decimal ARTCANA1 { get; set; }
        [DataMember]
        public decimal ARTCANA2 { get; set; }
        [DataMember]
        public string ARTCLINV { get; set; }
        [DataMember]
        public string ARTCIMP1 { get; set; }
        [DataMember]
        public string ARTCIMP2 { get; set; }
        [DataMember]
        public string ARTCIMP3 { get; set; }
        [DataMember]
        public string ARTCPLTR { get; set; }
        [DataMember]
        public string ARTUPLTR { get; set; }
        [DataMember]
        public decimal ARTSTSEG { get; set; }
        [DataMember]
        public decimal ARTPREOR { get; set; }
        [DataMember]
        public decimal ARTTLOTE { get; set; }
        [DataMember]
        public decimal ARTSTMAX { get; set; }
        [DataMember]
        public decimal ARTLDREV { get; set; }
        [DataMember]
        public decimal ARTFUREV { get; set; }
        [DataMember]
        public decimal ARTTREXT { get; set; }
        [DataMember]
        public string ARTTHIST { get; set; }
        [DataMember]
        public string ARTTPROM { get; set; }
        [DataMember]
        public decimal ARTPMVL1 { get; set; }
        [DataMember]
        public decimal ARTPMVL2 { get; set; }
        [DataMember]
        public decimal ARTLDCNT { get; set; }
        [DataMember]
        public decimal ARTFUCNT { get; set; }
        [DataMember]
        public decimal ARTSTOCK { get; set; }
        [DataMember]
        public decimal ARTSTANT { get; set; }
        [DataMember]
        public decimal ARTCANOR { get; set; }
        [DataMember]
        public decimal ARTCANRE { get; set; }
        [DataMember]
        public decimal ARTPSALM { get; set; }
        [DataMember]
        public decimal ARTAASAN { get; set; }
        [DataMember]
        public decimal ARTSTVA1 { get; set; }
        [DataMember]
        public decimal ARTCOSU1 { get; set; }
        [DataMember]
        public decimal ARTCOSU2 { get; set; }
        [DataMember]
        public string ARTTCOSR { get; set; }
        [DataMember]
        public decimal ARTCOUM1 { get; set; }
        [DataMember]
        public decimal ARTCOUM2 { get; set; }
        [DataMember]
        public string ARTTMAT { get; set; }
        [DataMember]
        public decimal ARTPREU1 { get; set; }
        [DataMember]
        public decimal ARTPREU2 { get; set; }
        [DataMember]
        public string ARTCDSRC { get; set; }
        [DataMember]
        public decimal ARTSTVA2 { get; set; }
        [DataMember]
        public string ARTX1 { get; set; }
        [DataMember]
        public decimal ARTFUSAL { get; set; }
        [DataMember]
        public decimal ARTFUENT { get; set; }
        [DataMember]
        public decimal ARTFUREQ { get; set; }
        [DataMember]
        public decimal ARTFUOC { get; set; }
        [DataMember]
        public decimal ARTFALTA { get; set; }
        [DataMember]
        public decimal ARTFBAJA { get; set; }
        [DataMember]
        public decimal ARTUPTRN { get; set; }
        [DataMember]
        public string ARTCACT { get; set; }
        [DataMember]
        public decimal ARTAASAE { get; set; }
        [DataMember]
        public decimal ARTUPROV { get; set; }
        [DataMember]
        public string ARTCMON { get; set; }
        [DataMember]
        public string ARTMEDA { get; set; }
        [DataMember]
        public decimal ARTEQMED { get; set; }
        [DataMember]
        public string ARTUBI { get; set; }
        [DataMember]
        public decimal ARTSTOCM { get; set; }
        [DataMember]
        public decimal ARTCANOM { get; set; }
        [DataMember]
        public decimal ARTCANRM { get; set; }
        [DataMember]
        public string ARTACTEM { get; set; }
        [DataMember]
        public string ARTACTOM { get; set; }
        [DataMember]
        public string ARTACTRM { get; set; }
        [DataMember]
        public decimal ARTCRSG { get; set; }
        [DataMember]
        public decimal ARTNRELM { get; set; }
        [DataMember]
        public decimal ARTNRELD { get; set; }
        [DataMember]
        public decimal ARTNRELH { get; set; }
        [DataMember]
        public string ARTFCTL { get; set; }
    }

    [DataContract]
    public class PEUBIC
    {
        [DataMember]
        public string UBICCOCA { get; set; }
        [DataMember]
        public decimal UBICCANT { get; set; }
    }

    [DataContract]
    public class PEESPE
    {
        [DataMember]
        public string ESPEIDPE { get; set; }
        [DataMember]
        public string ESPEIDES { get; set; }
        [DataMember]
        public string USUARIO { get; set; }
        [DataMember]
        public decimal CAPENUBU { get; set; }
        [DataMember]
        public decimal CAPETADE { get; set; }
    }

    [DataContract]
    public partial class USP_OBTIENE_DETALLE_PEDIDOS_Result
    {
        [DataMember]
        public decimal DEPEIDDP { get; set; }
        [DataMember]
        public decimal DEPEIDCP { get; set; }
        [DataMember]
        public string DEPECOAR { get; set; }
        [DataMember]
        public string DEPEPART { get; set; }
        [DataMember]
        public string DEPECONT { get; set; }
        [DataMember]
        public decimal DEPEALMA { get; set; }
        [DataMember]
        public decimal DEPECASO { get; set; }
        [DataMember]
        public decimal DEPEPESO { get; set; }
        [DataMember]
        public decimal DEPECAAT { get; set; }
        [DataMember]
        public decimal DEPEPEAT { get; set; }
        [DataMember]
        public decimal DEPEPERE { get; set; }
        [DataMember]
        public decimal DEPETADE { get; set; }
        [DataMember]
        public decimal DEPEPEBR { get; set; }
        [DataMember]
        public Nullable<decimal> DEPESTOC { get; set; }
        [DataMember]
        public decimal DEPEESTA { get; set; }
        [DataMember]
        public decimal DEPEDISP { get; set; }
        [DataMember]
        public string DEPEDSAR { get; set; }
        [DataMember]
        public decimal DEPENUBU { get; set; }
        [DataMember]
        public string DEPEUSCR { get; set; }
        [DataMember]
        public System.DateTime DEPEFECR { get; set; }
        [DataMember]
        public string DEPEUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        [DataMember]
        public int DEPESERS { get; set; }
        [DataMember]
        public string ARTCITEM { get; set; }
        [DataMember]
        public string PARTSTPR { get; set; }
    }

    [DataContract]
    public class USP_OBTIENE_BOLSA_Result
    {
        [DataMember]
        public decimal DEPEIDDP { get; set; }
        [DataMember]
        public decimal BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        //[DataMember]
        //public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }

    }

    [DataContract]
    public class USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result
    {
        [DataMember]
        public Nullable<decimal> DEPEIDDP { get; set; }
        [DataMember]
        public decimal BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal BODPIDDE { get; set; }
        [DataMember]
        public decimal BODPIDBO { get; set; }
        [DataMember]
        public decimal BODPTADE { get; set; }
        [DataMember]
        public decimal BODPPEBR { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDP { get; set; }
        [DataMember]
        public decimal BODPCANT { get; set; }
        [DataMember]
        public decimal BODPPESO { get; set; }
        [DataMember]
        public decimal TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }
        [DataMember]
        public decimal BODPPERE { get; set; }
        [DataMember]
        public decimal BODPSTCE { get; set; }
        [DataMember]
        public decimal BODPINBO { get; set; }
        [DataMember]
        public decimal BODPDIFE { get; set; }
        [DataMember]
        public decimal BODPTAUN { get; set; }

    }

    [DataContract]
    public class PEBOLS
    {
        [DataMember]
        public decimal BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOAR { get; set; }
        [DataMember]
        public decimal BOLSIDTC { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public decimal BOLSESTA { get; set; }
        [DataMember]
        public string BOLSUSCR { get; set; }
        [DataMember]
        public System.DateTime BOLSFECR { get; set; }
        [DataMember]
        public string BOLSUSMO { get; set; }
        [DataMember]
        public string BOLSUSUB { get; set; }
        [DataMember]
        public Nullable<System.DateTime> BOLSFEUB { get; set; }
        [DataMember]
        public Nullable<System.DateTime> BOLSFEMO { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }

    }

    [DataContract]
    public class PEBODP
    {
        [DataMember]
        public decimal BODPIDDE { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDBO { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDP { get; set; }
        [DataMember]
        public decimal BODPALMA { get; set; }
        [DataMember]
        public string BODPPART { get; set; }
        [DataMember]
        public string BODPCOAR { get; set; }
        [DataMember]
        public decimal BODPCANT { get; set; }
        [DataMember]
        public decimal BODPPESO { get; set; }
        [DataMember]
        public decimal BODPPERE { get; set; }
        [DataMember]
        public decimal BODPDIFE { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDO { get; set; }
        [DataMember]
        public decimal BODPSTCE { get; set; }
        [DataMember]
        public decimal BODPINBO { get; set; }
        [DataMember]
        public decimal BODPTADE { get; set; }
        [DataMember]
        public decimal BODPPEBR { get; set; }
        [DataMember]
        public string BODPUSCR { get; set; }
        [DataMember]
        public System.DateTime BODPFECR { get; set; }
        [DataMember]
        public string BODPUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> BODPFEMO { get; set; }
        [DataMember]
        public decimal BODPESTA { get; set; }
        [DataMember]
        public PEBOLS PEBOLS { get; set; }
        [DataMember]
        public PEDEPE PEDEPE { get; set; }
        [DataMember]
        public decimal BODPTAUN { get; set; }
    }

    public class USP_OBTIENE_PEDIDO_CONSULTA_Result
    {
        public decimal CAPEIDCP { get; set; }
        public string CAPESERI { get; set; }
        public decimal CAPENUME { get; set; }
        public string CAPEIDCL { get; set; }
        public System.DateTime CAPEFECH { get; set; }
        public string CAPEDIRE { get; set; }
        public decimal CAPEIDES { get; set; }
        public decimal CAPEPRIO { get; set; }
        public Nullable<System.DateTime> CAPEFEPR { get; set; }
        public string CAPEUSPR { get; set; }
        public string CAPENOTI { get; set; }
        public string CAPENOTG { get; set; }
        public string CAPEUSCR { get; set; }
        public System.DateTime CAPEFECR { get; set; }
        public string CAPEUSMO { get; set; }
        public Nullable<System.DateTime> CAPEFEMO { get; set; }
        public string CAPEUSEM { get; set; }
        public Nullable<System.DateTime> CAPEFHEM { get; set; }
        public string CAPEUSFP { get; set; }
        public Nullable<System.DateTime> CAPEFHFP { get; set; }
        public string CAPEUSIP { get; set; }
        public Nullable<System.DateTime> CAPEFHIP { get; set; }
        public string CAPEUSAP { get; set; }
        public Nullable<System.DateTime> CAPEFEAP { get; set; }
        public string CAPEEMAI { get; set; }
        public string CLINOM { get; set; }
        public string CLIRUC { get; set; }
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
        public decimal DEPEDISP { get; set; }
        public string DEPEUSCR { get; set; }
        public System.DateTime DEPEFECR { get; set; }
        public string DEPEUSMO { get; set; }
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        public string DEPEDSAR { get; set; }
        public string CALIDEAB { get; set; }
        public string CALICOMP { get; set; }
        public decimal CAPENUBU { get; set; }
        public decimal CAPETIPO { get; set; }

        public string CAPEESTA
        {
            get
            {
                return CAPEIDES.ToString().Replace("1", "Creado").Replace("2", "Emitido").Replace("3", "En preparación").Replace("4", "Despachado").Replace("5", "Completado").Replace("9", "Anulado");
            }
            //ejemplo
        }
    }

    public class RFEUSER
    {
        public string USERCUID { get; set; }
        public string USERIDEN { get; set; }
        public string USERCOAS { get; set; }
        public string USERMAIL { get; set; }
    }

    #region PEDIDOS INTERNOS

    [DataContract]
    public class USP_OBTIENE_BOLSA_OSA_Result
    {
        [DataMember]
        public decimal DEOSIDDO { get; set; }
        [DataMember]
        public decimal BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        //[DataMember]
        //public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }

    }

    [DataContract]
    public class PETIFO
    {
        [DataMember]
        public decimal TIFOIDTF { get; set; }
        [DataMember]
        public string TIFOCOFO { get; set; }
        [DataMember]
        public string TIFODESC { get; set; }
        [DataMember]
        public string TIFOUSCR { get; set; }
        [DataMember]
        public System.DateTime TIFOFECR { get; set; }
        [DataMember]
        public string TIFOUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> TIFOFEMO { get; set; }

        //public virtual ICollection<PEUSTF> PEUSTF { get; set; }
    }

    [DataContract]
    public class PEUSTF
    {
        [DataMember]
        public decimal USTFIDUF { get; set; }
        [DataMember]
        public string USTFUSUA { get; set; }
        [DataMember]
        public decimal USTFIDTF { get; set; }
        [DataMember]
        public bool USTFSUUS { get; set; }
        [DataMember]
        public string USTFUSCR { get; set; }
        [DataMember]
        public System.DateTime USTFFECR { get; set; }
        [DataMember]
        public string USTFUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> USTFFEMO { get; set; }

        public virtual PETIFO PETIFO { get; set; }
    }


    [DataContract]
    public class PROSAS
    {
        [DataMember]
        public decimal OSASCIA { get; set; }
        [DataMember]
        public string OSASFOLI { get; set; }
        [DataMember]
        public decimal OSASSECU { get; set; }
        [DataMember]
        public string OSASARTI { get; set; }
        [DataMember]
        public string OSASPAOR { get; set; }
        [DataMember]
        public decimal OSASALMA { get; set; }
        [DataMember]
        public decimal OSASCASO { get; set; }
        [DataMember]
        public string OSASPADE { get; set; }
        [DataMember]
        public System.DateTime OSASFEEM { get; set; }
        [DataMember]
        public string OSASRESP { get; set; }
        [DataMember]
        public decimal OSASCAEN { get; set; }
        [DataMember]
        public System.DateTime OSASFEAT { get; set; }
        [DataMember]
        public string OSASREEN { get; set; }
        [DataMember]
        public decimal OSASCCOS { get; set; }
        [DataMember]
        public string OSASDUEÑ { get; set; }
        [DataMember]
        public string OSASSTOS { get; set; }
        [DataMember]
        public decimal OSASUNID { get; set; }
        [DataMember]
        public string OSASFILL { get; set; }
        [DataMember]
        public string OSASFORZ { get; set; }
        [DataMember]
        public string OSASSTAT { get; set; }
    }

    [DataContract]
    public class USP_OBTIENE_OSAS_PENDIENTES_Result
    {
        [DataMember]
        public decimal OSASCIA { get; set; }
        [DataMember]
        public string OSASFOLI { get; set; }
        [DataMember]
        public Nullable<System.DateTime> PARTFEEF { get; set; }
        [DataMember]
        public System.DateTime OSASFEEM { get; set; }
        [DataMember]
        public string OSASRESP { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSIDCO { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSPRIO { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSIDES { get; set; }
        [DataMember]
        public string PARTSTPR { get; set; }

    }

    [DataContract]
    public partial class USP_OBTIENE_DETALLE_OSA_Result
    {
        [DataMember]
        public decimal OSASCIA { get; set; }
        [DataMember]
        public string OSASFOLI { get; set; }
        [DataMember]
        public decimal OSASSECU { get; set; }
        [DataMember]
        public string OSASARTI { get; set; }
        [DataMember]
        public string OSASPAOR { get; set; }
        [DataMember]
        public decimal OSASALMA { get; set; }
        [DataMember]
        public decimal OSASCASO { get; set; }
        [DataMember]
        public string OSASPADE { get; set; }
        [DataMember]
        public System.DateTime OSASFEEM { get; set; }
        [DataMember]
        public string OSASRESP { get; set; }
        [DataMember]
        public decimal OSASCAEN { get; set; }
        [DataMember]
        public System.DateTime OSASFEAT { get; set; }
        [DataMember]
        public string OSASREEN { get; set; }
        [DataMember]
        public decimal OSASCCOS { get; set; }
        [DataMember]
        public string OSASDUEÑ { get; set; }
        [DataMember]
        public string OSASSTOS { get; set; }
        [DataMember]
        public decimal OSASUNID { get; set; }
        [DataMember]
        public string OSASFILL { get; set; }
        [DataMember]
        public string OSASFORZ { get; set; }
        [DataMember]
        public string OSASSTAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSIDDO { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSIDCO { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSPEAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSCAAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSPERE { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSSTOC { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSESPA { get; set; }
        [DataMember]
        public decimal DEOSESTA { get; set; }
        [DataMember]
        public string PARTSTPR { get; set; }
        [DataMember]
        public decimal DEOSSECR { get; set; }

    }

    [DataContract]
    public class PECAOS
    {
        [DataMember]
        public decimal CAOSIDCO { get; set; }
        [DataMember]
        public string CAOSFOLI { get; set; }
        [DataMember]
        public decimal CAOSIDES { get; set; }
        [DataMember]
        public string CAOSUSCR { get; set; }
        [DataMember]
        public System.DateTime CAOSFECR { get; set; }
        [DataMember]
        public string CAOSUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CAOSFEMO { get; set; }
        [DataMember]
        public decimal CAOSPRIO { get; set; }
        [DataMember]
        public decimal CAOSEXTO { get; set; }
        [DataMember]
        public string CAOSNOTA { get; set; }

    }

    [DataContract]
    public class USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result
    {
        [DataMember]
        public Nullable<decimal> DEOSIDDO { get; set; }
        [DataMember]
        public decimal BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal BODPIDDE { get; set; }
        [DataMember]
        public decimal BODPIDBO { get; set; }
        [DataMember]
        public decimal BODPTADE { get; set; }
        [DataMember]
        public decimal BODPPEBR { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDO { get; set; }
        [DataMember]
        public decimal BODPCANT { get; set; }
        [DataMember]
        public decimal BODPPESO { get; set; }
        [DataMember]
        public decimal TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }
        [DataMember]
        public decimal BODPPERE { get; set; }
        [DataMember]
        public decimal BODPSTCE { get; set; }
        [DataMember]
        public decimal BODPINBO { get; set; }

        [DataMember]
        public decimal BODPDIFE { get; set; }
        [DataMember]
        public decimal BODPESTA { get; set; }
        [DataMember]
        public decimal BODPSECR { get; set; }
        [DataMember]
        public decimal BODPTAUN { get; set; }

    }

    [DataContract]
    public class PEDEOS
    {
        [DataMember]
        public decimal DEOSIDDO { get; set; }
        [DataMember]
        public decimal DEOSIDCO { get; set; }
        [DataMember]
        public decimal DEOSSECU { get; set; }
        [DataMember]
        public string DEOSFOLI { get; set; }
        [DataMember]
        public string DEOSPART { get; set; }
        [DataMember]
        public string DEOSCOAR { get; set; }
        [DataMember]
        public decimal DEOSALMA { get; set; }
        [DataMember]
        public decimal DEOSPESO { get; set; }
        [DataMember]
        public decimal DEOSPEAT { get; set; }
        [DataMember]
        public decimal DEOSCAAT { get; set; }
        [DataMember]
        public decimal DEOSPERE { get; set; }
        [DataMember]
        public decimal DEOSTADE { get; set; }
        [DataMember]
        public decimal DEOSPEBR { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSSTOC { get; set; }
        [DataMember]
        public decimal DEOSESTA { get; set; }
        [DataMember]
        public string DEOSUSCR { get; set; }
        [DataMember]
        public System.DateTime DEOSFECR { get; set; }
        [DataMember]
        public string DEOSUSMO { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DEOSFEMO { get; set; }
    }

    [DataContract]
    public class USP_FOLIO_USUARIO_Result
    {
        [DataMember]
        public decimal TIFOIDTF { get; set; }
        [DataMember]
        public string TIFOCOFO { get; set; }
        [DataMember]
        public string TIFODESC { get; set; }
    }


    #endregion

    #region ubicaciones
    [DataContract]
    public class USP_OBTIENE_BOLSA_UBICACION_Result
    {
        [DataMember]
        public string CAEMCOEM { get; set; }
        [DataMember]
        public string CAEMNPED { get; set; }
        [DataMember]
        public string MCSICL { get; set; }
        [DataMember]
        public string CAEMPART { get; set; }
        [DataMember]
        public System.DateTime CAEMFECH { get; set; }
        [DataMember]
        public decimal CAEMCAIT { get; set; }
        [DataMember]
        public decimal CAEMPNTO { get; set; }
        [DataMember]
        public decimal CAEMPBTO { get; set; }
    }

    [DataContract]
    public partial class USP_CONSULTA_EMPAQUES_PARTIDA_Result
    {
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public System.DateTime CAEMFECH { get; set; }
        [DataMember]
        public string CAEMRESP { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public decimal BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> PESONETO { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
    }

    [DataContract]
    public class USP_OBTIENE_DETALLE_BOLSA_Result
    {
        [DataMember]
        public string CAEMCOEM { get; set; }
        [DataMember]
        public decimal CAEMALMA { get; set; }
        [DataMember]
        public string DEEMPART { get; set; }
        [DataMember]
        public string DEEMARTI { get; set; }
        [DataMember]
        public decimal DEEMCANT { get; set; }
        [DataMember]
        public decimal DEEMPBRU { get; set; }
        [DataMember]
        public decimal DEEMDEST { get; set; }
        [DataMember]
        public Nullable<decimal> DEEMNETO { get; set; }
    }

    #endregion

    #region recepcion osas

    [DataContract]
    public partial class USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result
    {
        [DataMember]
        public Nullable<long> OSAROW { get; set; }
        [DataMember]
        public decimal OSASCIA { get; set; }
        [DataMember]
        public string OSASFOLI { get; set; }
        [DataMember]
        public Nullable<System.DateTime> PARTFEEF { get; set; }
        [DataMember]
        public System.DateTime OSASFEEM { get; set; }
        [DataMember]
        public string OSASRESP { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSIDCO { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSPRIO { get; set; }
        [DataMember]
        public Nullable<decimal> CAOSIDES { get; set; }
        [DataMember]
        public string CAOSUSPR { get; set; }
        [DataMember]
        public string OSASPADE { get; set; }
    }

    [DataContract]
    public partial class USP_OBTIENE_DETALLE_OSA_PLANTA_Result
    {
        [DataMember]
        public decimal OSASCIA { get; set; }
        [DataMember]
        public string OSASFOLI { get; set; }
        [DataMember]
        public decimal OSASSECU { get; set; }
        [DataMember]
        public string OSASARTI { get; set; }
        [DataMember]
        public string OSASPAOR { get; set; }
        [DataMember]
        public decimal OSASALMA { get; set; }
        [DataMember]
        public decimal OSASCASO { get; set; }
        [DataMember]
        public string OSASPADE { get; set; }
        [DataMember]
        public System.DateTime OSASFEEM { get; set; }
        [DataMember]
        public string OSASRESP { get; set; }
        [DataMember]
        public decimal OSASCAEN { get; set; }
        [DataMember]
        public System.DateTime OSASFEAT { get; set; }
        [DataMember]
        public string OSASREEN { get; set; }
        [DataMember]
        public decimal OSASCCOS { get; set; }
        [DataMember]
        public string OSASDUEÑ { get; set; }
        [DataMember]
        public string OSASSTOS { get; set; }
        [DataMember]
        public decimal OSASUNID { get; set; }
        [DataMember]
        public string OSASFILL { get; set; }
        [DataMember]
        public string OSASFORZ { get; set; }
        [DataMember]
        public string OSASSTAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSIDDO { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSIDCO { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSPEAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSCAAT { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSPERE { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSSTOC { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSESPA { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSPEOR { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSCAOR { get; set; }
        [DataMember]
        public Nullable<decimal> DEOSESTA { get; set; }
        [DataMember]
        public string DEOSUSMO { get; set; }
    }

    [DataContract]
    public class DET_USP_OBTIENE_DETALLE_OSA_PLANTA_Result
    {
        [DataMember]
        public List<USP_OBTIENE_DETALLE_OSA_PLANTA_Result> Items { get; set; }

    }

    public class DTO_USP_OBTIENE_DETALLE_OSA_Result
    {
        [DataMember]
        public List<USP_OBTIENE_DETALLE_OSA_Result> Items { get; set; }
        [DataMember]
        public string USUARIO { get; set; }
        [DataMember]
        public decimal ESTADO { get; set; }
    }

    public class DTO_PEBODP
    {
        [DataMember]
        public List<PEBODP> Items { get; set; }

        //Esta lista es para eliminar, recibe los id para eliminar
        [DataMember]
        public List<decimal> Items2 { get; set; }

    }

    #endregion

    #region SIN EMPAQUE
    [DataContract]
    public partial class USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result
    {
        [DataMember]
        public Nullable<decimal> DEPEIDDP { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal BODPIDDE { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDBO { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDP { get; set; }
        [DataMember]
        public decimal BODPCANT { get; set; }
        [DataMember]
        public decimal BODPPESO { get; set; }
        [DataMember]
        public Nullable<decimal> TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }
        [DataMember]
        public decimal BODPPERE { get; set; }
        [DataMember]
        public decimal BODPSTCE { get; set; }
        [DataMember]
        public decimal BODPINBO { get; set; }
        [DataMember]
        public decimal BODPTADE { get; set; }
        [DataMember]
        public decimal BODPPEBR { get; set; }
        [DataMember]
        public decimal BODPDIFE { get; set; }
        [DataMember]
        public decimal BODPTAUN { get; set; }
    }

    [DataContract]
    public partial class USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result
    {
        [DataMember]
        public Nullable<decimal> DEOSIDDO { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSIDBO { get; set; }
        [DataMember]
        public string BOLSCOEM { get; set; }
        [DataMember]
        public string BOLSCOCA { get; set; }
        [DataMember]
        public string BOLSARTI { get; set; }
        [DataMember]
        public string BOLSPART { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSALMA { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSCANT { get; set; }
        [DataMember]
        public Nullable<decimal> BOLSPESO { get; set; }
        [DataMember]
        public decimal BODPIDDE { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDBO { get; set; }
        [DataMember]
        public Nullable<decimal> BODPIDDO { get; set; }
        [DataMember]
        public decimal BODPCANT { get; set; }
        [DataMember]
        public decimal BODPPESO { get; set; }
        [DataMember]
        public Nullable<decimal> TIEMTARA { get; set; }
        [DataMember]
        public Nullable<decimal> UNIDTARA { get; set; }
        [DataMember]
        public decimal BODPPERE { get; set; }
        [DataMember]
        public decimal BODPSTCE { get; set; }
        [DataMember]
        public decimal BODPINBO { get; set; }
        [DataMember]
        public decimal BODPTADE { get; set; }
        [DataMember]
        public decimal BODPPEBR { get; set; }
        [DataMember]
        public decimal BODPDIFE { get; set; }
        [DataMember]
        public decimal BODPESTA { get; set; }
        [DataMember]
        public decimal BODPSECR { get; set; }
        [DataMember]
        public decimal BODPTAUN { get; set; }
    }

    #endregion

    //margaret 4/07
    [DataContract]
    public class ErrorMessage
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "message")]
        public String Message { get; set; }

        [DataMember(Name = "developermessage")]
        public String DeveloperMessage { get; set; }

        public ErrorMessage(GlobalApplicationException ex)
        {
            this.Status = ex.Status;
            this.Code = ex.Code;
            this.Message = ex.Message;
            this.DeveloperMessage = ex.DeveloperMessage;
        }

        public ErrorMessage()
        {
            // TODO: Complete member initialization
        }
    }
}
