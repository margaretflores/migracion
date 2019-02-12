using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appWcfService
{
    public class PECAPE
    {
        public decimal CAPEIDCP { get; set; }
        public string CAPESERI { get; set; }
        public decimal CAPENUME { get; set; }
        public string CAPEIDCL { get; set; }
        public System.DateTime CAPEFECH { get; set; }
        public string CAPEDIRE { get; set; }
        public decimal CAPEIDES { get; set; }
        public decimal CAPEEPRI { get; set; }
        public decimal CAPEPRIO { get; set; }
        public Nullable<System.DateTime> CAPEFEPR { get; set; }
        public string CAPEUSPR { get; set; }
        public string CAPEEMAI { get; set; }
        public string CAPENOTI { get; set; }
        public string CAPENOTG { get; set; }
        public decimal CAPENUBU { get; set; }
        public decimal CAPETIPO { get; set; }
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
        public decimal CAPETADE { get; set; }
        public decimal CAPEIDTD { get; set; }
        public string CAPEDEST { get; set; }


        //public  PEESTA PEESTA { get; set; }
        public TCLIE TCLIE { get; set; }
        //public virtual ICollection<PEPEPG> PEPEPG { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PEDEPE> PEDEPE { get; set; }
        public Nullable<decimal> CAPEDOCO { get; set; }
        public bool CAPECHECK { get; set; } //Check para anular el pedido 

        public string CAPENUMC
        {
            get
            {
                return CAPENUME.ToString().Trim().PadLeft(7, '0');
            }
            //ejemplo
        }
        public string CAPEESTA
        {
            get
            {
                return CAPEIDES.ToString().Replace("1", "Creado").Replace("2", "Emitido").Replace("3", "En preparación").Replace("4", "Espera de aprobación").Replace("5", "Completado").Replace("9", "Anulado");
            }
            //ejemplo
        }
    }

    public class TCLIE
    {
        public string CLICVE { get; set; }
        public string CLINOM { get; set; }
        public string CLIABR { get; set; }
        public string CLIDIR { get; set; }
        public string CLICPO { get; set; }
        public string CLIDIS { get; set; }
        public string CLIPRO { get; set; }
        public string CLIDPT { get; set; }
        public string CLIPAI { get; set; }
        public string CLITE1 { get; set; }
        public string CLITE2 { get; set; }
        public string CLITE3 { get; set; }
        public string CLITE4 { get; set; }
        public string CLIRUC { get; set; }
        public string CLIRF1 { get; set; }
        public string CLIRF2 { get; set; }
        public string CLIRF3 { get; set; }
        public string CLIVEN { get; set; }
        public string CLICOB { get; set; }
        public string CLIZON { get; set; }
        public string CLISIT { get; set; }
        public decimal CLILCR { get; set; }
        public decimal CLISAL { get; set; }
        public string CLISCR { get; set; }
        public string CPACVE { get; set; }
        public decimal CLDSCT { get; set; }
        public string CLPREC { get; set; }
        public string CLCNOM { get; set; }
        public string CLCPUE { get; set; }
        public string CLMAIL { get; set; }
        public string CLTIDE { get; set; }
        public string CLNIDE { get; set; }
        public string CLDIRF { get; set; }

    }

    public class I1DD20A
    {
        public string ARTX0 { get; set; }
        public decimal ARTCIA { get; set; }
        public string ARTCITEM { get; set; }
        public string ARTDES { get; set; }
        public string ARTMED { get; set; }
        public string ARTGRINV { get; set; }
        public string ARTGIRO { get; set; }
        public string ARTGRCOM { get; set; }
        public string ARTCCOMP { get; set; }
        public decimal ARTCCONS { get; set; }
        public decimal ARTCANA1 { get; set; }
        public decimal ARTCANA2 { get; set; }
        public string ARTCLINV { get; set; }
        public string ARTCIMP1 { get; set; }
        public string ARTCIMP2 { get; set; }
        public string ARTCIMP3 { get; set; }
        public string ARTCPLTR { get; set; }
        public string ARTUPLTR { get; set; }
        public decimal ARTSTSEG { get; set; }
        public decimal ARTPREOR { get; set; }
        public decimal ARTTLOTE { get; set; }
        public decimal ARTSTMAX { get; set; }
        public decimal ARTLDREV { get; set; }
        public decimal ARTFUREV { get; set; }
        public decimal ARTTREXT { get; set; }
        public string ARTTHIST { get; set; }
        public string ARTTPROM { get; set; }
        public decimal ARTPMVL1 { get; set; }
        public decimal ARTPMVL2 { get; set; }
        public decimal ARTLDCNT { get; set; }
        public decimal ARTFUCNT { get; set; }
        public decimal ARTSTOCK { get; set; }
        public decimal ARTSTANT { get; set; }
        public decimal ARTCANOR { get; set; }
        public decimal ARTCANRE { get; set; }
        public decimal ARTPSALM { get; set; }
        public decimal ARTAASAN { get; set; }
        public decimal ARTSTVA1 { get; set; }
        public decimal ARTCOSU1 { get; set; }
        public decimal ARTCOSU2 { get; set; }
        public string ARTTCOSR { get; set; }
        public decimal ARTCOUM1 { get; set; }
        public decimal ARTCOUM2 { get; set; }
        public string ARTTMAT { get; set; }
        public decimal ARTPREU1 { get; set; }
        public decimal ARTPREU2 { get; set; }
        public string ARTCDSRC { get; set; }
        public decimal ARTSTVA2 { get; set; }
        public string ARTX1 { get; set; }
        public decimal ARTFUSAL { get; set; }
        public decimal ARTFUENT { get; set; }
        public decimal ARTFUREQ { get; set; }
        public decimal ARTFUOC { get; set; }
        public decimal ARTFALTA { get; set; }
        public decimal ARTFBAJA { get; set; }
        public decimal ARTUPTRN { get; set; }
        public string ARTCACT { get; set; }
        public decimal ARTAASAE { get; set; }
        public decimal ARTUPROV { get; set; }
        public string ARTCMON { get; set; }
        public string ARTMEDA { get; set; }
        public decimal ARTEQMED { get; set; }
        public string ARTUBI { get; set; }
        public decimal ARTSTOCM { get; set; }
        public decimal ARTCANOM { get; set; }
        public decimal ARTCANRM { get; set; }
        public string ARTACTEM { get; set; }
        public string ARTACTOM { get; set; }
        public string ARTACTRM { get; set; }
        public decimal ARTCRSG { get; set; }
        public decimal ARTNRELM { get; set; }
        public decimal ARTNRELD { get; set; }
        public decimal ARTNRELH { get; set; }
        public string ARTFCTL { get; set; }
    }

    public partial class PEBOLS
    {
        public decimal BOLSIDBO { get; set; }
        public string BOLSCOAR { get; set; }
        public decimal BOLSIDTC { get; set; }
        public string BOLSCOEM { get; set; }
        public string BOLSCOCA { get; set; }
        public decimal BOLSESTA { get; set; }
        public string BOLSUSCR { get; set; }
        public System.DateTime BOLSFECR { get; set; }
        public string BOLSUSMO { get; set; }
        public Nullable<System.DateTime> BOLSFEMO { get; set; }
        public Nullable<decimal> BOLSCANT { get; set; }
        public Nullable<decimal> BOLSPESO { get; set; }
        public decimal BOLSALMA { get; set; }

    }

    public class PEDEPE
    {
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
        public decimal DEPENUBU { get; set; }
        public string DEPEUSCR { get; set; }
        public System.DateTime DEPEFECR { get; set; }
        public string DEPEUSMO { get; set; }
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        public string DEPEDSAR { get; set; }
        public int DEPESERS { get; set; }
        public decimal DEPESECU { get; set; }


        public virtual I1DD20A I1DD20A { get; set; }
        //public virtual ICollection<PEBODP> PEBODP { get; set; }
        public virtual PECAPE PECAPE { get; set; }
        //public virtual ICollection<PEDPDG> PEDPDG { get; set; }
        //public virtual ICollection<PEHIMO> PEHIMO { get; set; }

        public bool CHECKSEL { get; set; }
        public bool CHECKDEL { get; set; }
        public bool ELIMINA { get; set; }
        public string PARTSTPR { get; set; }

        //18/05/2018
        public decimal LOTCANRE { get; set; } //Almacena la reserva
        public bool CHECKRESE { get; set; }
    }

    public partial class PEBODP
    {
        public decimal BODPIDDE { get; set; }
        public Nullable<decimal> BODPIDBO { get; set; }
        public Nullable<decimal> BODPIDDP { get; set; }
        public decimal BODPALMA { get; set; }
        public string BODPPART { get; set; }
        public string BODPCOAR { get; set; }
        public decimal BODPCANT { get; set; }
        public decimal BODPPESO { get; set; }
        public decimal BODPPERE { get; set; }
        public decimal BODPDIFE { get; set; }
        public Nullable<decimal> BODPIDDO { get; set; }
        public decimal BODPSTCE { get; set; }
        public decimal BODPINBO { get; set; }
        public decimal BODPTADE { get; set; }
        public decimal BODPPEBR { get; set; }
        public decimal BODPESTA { get; set; }
        public string BODPUSCR { get; set; }
        public System.DateTime BODPFECR { get; set; }
        public string BODPUSMO { get; set; }
        public Nullable<System.DateTime> BODPFEMO { get; set; }
        public decimal BODPSECR { get; set; }
        public decimal BODPTAUN { get; set; }

        public PEBOLS PEBOLS { get; set; }
        //public virtual PEDEOS PEDEOS { get; set; }
        public PEDEPE PEDEPE { get; set; }
    }

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

    public partial class PEHIMO
    {
        public decimal HIMOIDIM { get; set; }
        public string HIMOCOAR { get; set; }
        public Nullable<decimal> HIMOIDDO { get; set; }
        public Nullable<decimal> HIMOIDDP { get; set; }
        public decimal HIMOIDBO { get; set; }
        public System.DateTime HIMOFECH { get; set; }
        public Nullable<decimal> HIMOCANT { get; set; }
        public decimal HIMOPESO { get; set; }
        public string HIMOUSCR { get; set; }
        public System.DateTime HIMOFECR { get; set; }
        public string HIMOUSMO { get; set; }
        public Nullable<System.DateTime> HIMOFEMO { get; set; }

        public virtual PEBOLS PEBOLS { get; set; }
        //public virtual PEDEOS PEDEOS { get; set; }
        public virtual PEDEPE PEDEPE { get; set; }
    }

    public partial class PEESTA
    {
        public decimal ESTAIDES { get; set; }
        public string ESTADESC { get; set; }
        public string ESTAUSCR { get; set; }
        public System.DateTime ESTAFECR { get; set; }
        public string ESTAUSMO { get; set; }
        public Nullable<System.DateTime> ESTAFEMO { get; set; }

        //public virtual ICollection<PECAOS> PECAOS { get; set; }
        //public virtual ICollection<PECAPE> PECAPE { get; set; }
    }

    public class PEPASI
    {
        public decimal PASIIDPA { get; set; }
        public decimal PASIESTA { get; set; }
        public string PASIUSCR { get; set; }
        public System.DateTime PASIFECR { get; set; }
        public string PASIUSMO { get; set; }
        public Nullable<System.DateTime> PASIFEMO { get; set; }

        //public  PECOLU PECOLU { get; set; }
        //public  PENIVE PENIVE { get; set; }

    }

    public class PECASI
    {
        public string CASICOCA { get; set; }
        public decimal CASIIDPA { get; set; }
        public string CASIIDNI { get; set; }
        public decimal CASIIDCO { get; set; }
        public Nullable<decimal> CASICAPA { get; set; }
        public decimal CASIESTA { get; set; }
        public Nullable<decimal> CASIALTU { get; set; }
        public Nullable<decimal> CASILARG { get; set; }
        public Nullable<decimal> CASIANCH { get; set; }
        public string CASIUSCR { get; set; }
        public System.DateTime CASIFECR { get; set; }
        public string CASIUSMO { get; set; }
        public Nullable<System.DateTime> CASIFEMO { get; set; }

        //public virtual ICollection<PEBOLS> PEBOLS { get; set; }    
        //public virtual ICollection<PEHIBO> PEHIBO { get; set; }
        //public virtual PECOLU PECOLU { get; set; }
        //public virtual PENIVE PENIVE { get; set; }
    }

    public partial class PENIVE
    {
        public string NIVEIDNI { get; set; }
        public decimal NIVEIDPA { get; set; }
        public decimal NIVEESTA { get; set; }
        public string NIVEUSCR { get; set; }
        public System.DateTime NIVEFECR { get; set; }
        public string NIVEUSMO { get; set; }
        public Nullable<System.DateTime> NIVEFEMO { get; set; }

        //public virtual ICollection<PECASI> PECASI { get; set; }
        //public virtual PEPASI PEPASI { get; set; }
    }

    public class PECOLU
    {
        public decimal COLUIDCO { get; set; }
        public decimal COLUIDPA { get; set; }
        public decimal COLUESTA { get; set; }
        public string COLUUSCR { get; set; }
        public System.DateTime COLUFECR { get; set; }
        public string COLUUSMO { get; set; }
        public Nullable<System.DateTime> COLUFEMO { get; set; }

        //public virtual ICollection<PECASI> PECASI { get; set; }
        //public virtual PEPASI PEPASI { get; set; }
    }
    //STORED PROCEDURES
    public class USP_OBTIENE_ARTICULOS_Result
    {
        public string LOTITEM { get; set; }
        public string LOTPARTI { get; set; }
        public string ASIGNUPE { get; set; }
        public decimal LOTALM { get; set; }
        public decimal LOTSTOCK { get; set; }
        public decimal LOTCANRE { get; set; }
        public string CALIDEAB { get; set; }
        public string CALICOMP { get; set; }
        public decimal MCCOCL { get; set; }
        public string MCRUC { get; set; }
        public decimal CVTDSECU { get; set; }
        //adicionales
        public decimal PESOSOL { get; set; }
        public decimal CANTSOL { get; set; }
        public bool CHECKSEL { get; set; }
        public bool CHECKDEL { get; set; }


    }

    public class USP_OBTIENE_SERIES_POR_USUARIO
    {
        public string PUSERCUID { get; set; }
        public decimal PSRIEIDTD { get; set; }
    }

    public partial class USP_OBTIENE_DETALLE_PEDIDOS_Result
    {
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
        public string DEPEDSAR { get; set; }
        public decimal DEPENUBU { get; set; }
        public string DEPEUSCR { get; set; }
        public System.DateTime DEPEFECR { get; set; }
        public string DEPEUSMO { get; set; }
        public Nullable<System.DateTime> DEPEFEMO { get; set; }
        public int DEPESERS { get; set; }
        public string ARTCITEM { get; set; }
        public string PARTSTPR { get; set; }
        public decimal DEPESECU { get; set; }
    }

    public class USP_OBTIENE_PEDIDO_ALMACEN_Result
    {
        public decimal CAPEIDCP { get; set; }
        public string CAPESERI { get; set; }
        public decimal CAPENUME { get; set; }
        public string CAPEIDCL { get; set; }
        public System.DateTime CAPEFECH { get; set; }
        public string CAPEDIRE { get; set; }
        public decimal CAPEIDES { get; set; }
        public Nullable<decimal> CAPEPRIO { get; set; }
        public Nullable<System.DateTime> CAPEFEPR { get; set; }
        public string CAPEUSPR { get; set; }
        public string CAPENOTI { get; set; }
        public string CAPENOTG { get; set; }
        public string CAPEUSCR { get; set; }
        public System.DateTime CAPEFECR { get; set; }
        public string CAPEUSMO { get; set; }
        public Nullable<System.DateTime> CAPEFEMO { get; set; }
        public string CLINOM { get; set; }
        public string CLIABR { get; set; }
        public string CLIRUC { get; set; }
    }

    public partial class USP_OBTIENE_PEDIDO_FECHAS_Result
    {
        public decimal CAPEIDCP { get; set; }
        public string CAPESERI { get; set; }
        public decimal CAPENUME { get; set; }
        public string CAPEIDCL { get; set; }
        public System.DateTime CAPEFECH { get; set; }
        public string CAPEDIRE { get; set; }
        public decimal CAPEIDES { get; set; }
        public Nullable<decimal> CAPEPRIO { get; set; }
        public Nullable<System.DateTime> CAPEFEPR { get; set; }
        public string CAPEUSPR { get; set; }
        public string CAPENOTI { get; set; }
        public string CAPENOTG { get; set; }
        public string CAPEUSCR { get; set; }
        public System.DateTime CAPEFECR { get; set; }
        public string CAPEUSMO { get; set; }
        public Nullable<System.DateTime> CAPEFEMO { get; set; }
    }

    public class USP_OBTIENE_UBICACIONES_Result
    {
        public string UBICCOCA { get; set; }
        public Nullable<int> UBICCANT { get; set; }
    }

    public class USP_OBTIENE_BOLSA_Result
    {
        public Nullable<decimal> DEPEIDDP { get; set; }
        public decimal BOLSIDBO { get; set; }
        public string BOLSCOEM { get; set; }
        //public string BOLSCOCA { get; set; }
        public string BOLSARTI { get; set; }
        public string BOLSPART { get; set; }
        public decimal BOLSALMA { get; set; }
        public decimal BOLSCANT { get; set; }
        public decimal BOLSPESO { get; set; }
        public decimal TIEMTARA { get; set; }
        public decimal UNIDTARA { get; set; }
    }

    public class USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result
    {
        public Nullable<decimal> DEPEIDDP { get; set; }
        public decimal BOLSIDBO { get; set; }
        public string BOLSCOEM { get; set; }
        public string BOLSCOCA { get; set; }
        public string BOLSARTI { get; set; }
        public string BOLSPART { get; set; }
        public decimal BOLSALMA { get; set; }
        public Nullable<decimal> BOLSCANT { get; set; }
        public decimal BOLSPESO { get; set; }
        public decimal BODPIDDE { get; set; }
        public decimal BODPIDBO { get; set; }
        public Nullable<decimal> BODPIDDP { get; set; }
        public decimal BODPCANT { get; set; }
        public decimal BODPPESO { get; set; }
        public decimal TIEMTARA { get; set; }
        public decimal UNIDTARA { get; set; }
        public decimal BODPPERE { get; set; }
        public decimal BODPSTCE { get; set; }
        public decimal BODPINBO { get; set; }
        public decimal BODPTADE { get; set; }
        public decimal BODPPEBR { get; set; }
        public decimal BODPDIFE { get; set; }
        /// 
        public bool BODPCHECK { get; set; } //Check para anular el bolsa USP
                                            /// 
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
        public decimal CAPETADE { get; set; }
        public decimal CAPEIDTD { get; set; }
        public decimal DEPESECU { get; set; }
        public string CAPEDEST { get; set; }
        public string CAPEESTA
        {
            get
            {
                return CAPEIDES.ToString().Replace("1", "Creado").Replace("2", "Emitido").Replace("3", "En preparación").Replace("4", "Despachado").Replace("5", "Completado").Replace("9", "Anulado");
            }
            //ejemplo
        }
    }

    public class USP_REPORTE_EMPAQUES_PARTIDA_Result
    {
        public string BOLSCOEM { get; set; }
        public System.DateTime CAEMFECH { get; set; }
        public string CAEMRESP { get; set; }
        public string BOLSARTI { get; set; }
        public string BOLSPART { get; set; }
        public decimal BOLSALMA { get; set; }
        public Nullable<decimal> BOLSCANT { get; set; }
        public Nullable<decimal> PESONETO { get; set; }
        public Nullable<decimal> PESOBRUTO { get; set; }
        public string BOLSCOCA { get; set; }
    }
    public class USP_REPORTE_MOVIMIENTOS_PARTIDA_Result
    {
        public decimal KABOIDKB { get; set; }
        public Nullable<decimal> BOLSIDBO { get; set; }
        public System.DateTime FECH { get; set; }
        public string MOTIVO { get; set; }
        public string BOLSCOEM { get; set; }
        public decimal BOLSCANT { get; set; }
        public decimal PESONETO { get; set; }
        public string NRODOC { get; set; }
        public string RESP { get; set; }
        public string BOLSARTI { get; set; }
        public string BOLSPART { get; set; }
        public decimal BOLSALMA { get; set; }
    }

    public class GAUSUA
    {
        public string GRUSCOGR { get; set; }
        public string USUACOUS { get; set; }
        public string USUANOUS { get; set; }
        public string USUAMAIL { get; set; }
        public string USUAUSAD { get; set; }
    }

    public class RFEUSER
    {
        public string USERCUID { get; set; }
        public string USERIDEN { get; set; }
        public string USERCOAS { get; set; }
        public string USERMAIL { get; set; }
    }
    public class PEPARM
    {
        public decimal PARMIDPA { get; set; }
        public string PARMDSPA { get; set; }
        public string PARMVAPA { get; set; }
    }
    public class I1DD41A
    {
        public decimal LOTCIA { get; set; }
        public string LOTITEM { get; set; }
        public string LOTPARTI { get; set; }
        public decimal LOTALM { get; set; }
        public decimal LOTSTOCK { get; set; }
        public decimal LOTCANOR { get; set; }
        public decimal LOTCANRE { get; set; }
        public decimal LOTFDOCU { get; set; }
        public decimal LOTSALPA { get; set; }
        public string LOTX0 { get; set; }
        public string LOTSTPAR { get; set; }
        public string LOTUBIFI { get; set; }
        public decimal LOTCAUNI { get; set; }
    }

    public class USP_REPORTE_MOVIMIENTOS_FECHAS_Result
    {
        public string TIPDOC { get; set; }
        public string NRODOC { get; set; }
        public Nullable<System.DateTime> FECDOC { get; set; }
        public string KABOITEM { get; set; }
        public string KABOPART { get; set; }
        public decimal KABOALMA { get; set; }
        public Nullable<decimal> CANT { get; set; }
        public Nullable<decimal> PESO { get; set; }
        public string PEDIDO { get; set; }
        public string CLIENTE { get; set; }
    }
    public class CARGACOMBO
    {
        public string DESCRIPCION { get; set; }
        public decimal VALOR { get; set; }

    }
    public class USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result
    {
        public Nullable<decimal> DEPEIDDP { get; set; }
        public Nullable<decimal> BOLSIDBO { get; set; }
        public string BOLSCOEM { get; set; }
        public string BOLSCOCA { get; set; }
        public string BOLSARTI { get; set; }
        public string BOLSPART { get; set; }
        public Nullable<decimal> BOLSALMA { get; set; }
        public Nullable<decimal> BOLSCANT { get; set; }
        public Nullable<decimal> BOLSPESO { get; set; }
        public decimal BODPIDDE { get; set; }
        public Nullable<decimal> BODPIDBO { get; set; }
        public Nullable<decimal> BODPIDDP { get; set; }
        public decimal BODPCANT { get; set; }
        public decimal BODPPESO { get; set; }
        public Nullable<decimal> TIEMTARA { get; set; }
        public Nullable<decimal> UNIDTARA { get; set; }
        public decimal BODPPERE { get; set; }
        public decimal BODPSTCE { get; set; }
        public decimal BODPINBO { get; set; }
        public decimal BODPTADE { get; set; }
        public decimal BODPPEBR { get; set; }
        public decimal BODPDIFE { get; set; }
        public decimal BODPTAUN { get; set; }
    }

    public class PEDEOS

    {
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

        public PEBODP PEBODP { get; set; }

        //public PECAOS PECAOS { get; set; }

        public PEHIMO PEHIMO { get; set; }

        public decimal DEOSSECR { get; set; }

    }

    public class PEKABO
    {
        public decimal KABOIDKB { get; set; }
        public decimal KABOIDBO { get; set; }
        public decimal KABOIDTM { get; set; }
        public System.DateTime KABOFECH { get; set; }
        public decimal KABOCANT { get; set; }
        public decimal KABOPESO { get; set; }
        public Nullable<decimal> KABOIDDP { get; set; }
        public Nullable<decimal> KABOIDDO { get; set; }
        public string KABOUSCR { get; set; }
        public System.DateTime KABOFECR { get; set; }
        public string KABOUSMO { get; set; }
        public Nullable<System.DateTime> KABOFEMO { get; set; }
        public string KABOPART { get; set; }
        public string KABOITEM { get; set; }
        public decimal KABOTARA { get; set; }
        public decimal KABOPEBR { get; set; }
        public decimal KABOALMA { get; set; }

        public virtual PEBOLS PEBOLS { get; set; }
        //public virtual PETMKB PETMKB { get; set; }
    }

    public class PROSAS
    {
        public decimal OSASCIA { get; set; }
        public string OSASFOLI { get; set; }
        public decimal OSASSECU { get; set; }
        public string OSASARTI { get; set; }
        public string OSASPAOR { get; set; }
        public decimal OSASALMA { get; set; }
        public decimal OSASCASO { get; set; }
        public string OSASPADE { get; set; }
        public System.DateTime OSASFEEM { get; set; }
        public string OSASRESP { get; set; }
        public decimal OSASCAEN { get; set; }
        public System.DateTime OSASFEAT { get; set; }
        public string OSASREEN { get; set; }
        public decimal OSASCCOS { get; set; }
        public string OSASDUEÑ { get; set; }
        public string OSASSTOS { get; set; }
        public decimal OSASUNID { get; set; }
        public string OSASFILL { get; set; }
        public string OSASFORZ { get; set; }
        public string OSASSTAT { get; set; }
    }

    public class GMDEEM

    {

        public decimal DEEMCIA { get; set; }

        public string DEEMCOEM { get; set; }

        public decimal DEEMSECU { get; set; }

        public System.DateTime DEEMFECH { get; set; }

        public System.DateTime DEEMHORA { get; set; }

        public string DEEMPART { get; set; }

        public string DEEMARTI { get; set; }

        public decimal DEEMALMA { get; set; }

        public decimal DEEMCANT { get; set; }

        public string DEEMTIPE { get; set; }

        public decimal DEEMPNET { get; set; }

        public decimal DEEMDEST { get; set; }

        public decimal DEEMACON { get; set; }

        public string DEEMUSER { get; set; }

        public string DEEMESTA { get; set; }

        public decimal DEEMCAST { get; set; }

        public decimal DEEMPEST { get; set; }

        public decimal DEEMSTCE { get; set; }

        public decimal DEEMESBO { get; set; }

    }

    public class GMCAEM

    {
        public decimal CAEMCIA { get; set; }

        public string CAEMCOEM { get; set; }

        public System.DateTime CAEMFECH { get; set; }

        public decimal CAEMTURN { get; set; }

        public decimal CAEMPNTO { get; set; }

        public decimal CAEMDTTO { get; set; }

        public decimal CAEMDEEM { get; set; }

        public decimal CAEMACTO { get; set; }

        public decimal CAEMPBTO { get; set; }

        public decimal CAEMCAIT { get; set; }

        public string CAEMTIEM { get; set; }

        public string CAEMNPED { get; set; }

        public string CAEMDESP { get; set; }

        public string CAEMNNDC { get; set; }

        public decimal CAEMALMA { get; set; }

        public string CAEMPART { get; set; }

        public string CAEMMSPA { get; set; }

        public string CAEMESEM { get; set; }

        public string CAEMESRE { get; set; }

        public string CAEMUSER { get; set; }

        public string CAEMRESP { get; set; }

        public string CAEMUBIC { get; set; }



        //public FCTIEM FCTIEM { get; set; }

    }
}

