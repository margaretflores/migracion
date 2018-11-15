using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appWcfService
{

    public class MTMAQU
    {
        public decimal MAQUCIA { get; set; }
        public decimal MAQUIDMA { get; set; }
        public string MAQUCOAC { get; set; }
        public string MAQUCOMA { get; set; }
        public string MAQUDES1 { get; set; }
        public string MAQUDES2 { get; set; }
        public string MAQUCAPA { get; set; }
        public string MAQUMARC { get; set; }
        public string MAQUMODE { get; set; }
        public decimal MAQUPOTE { get; set; }
        public decimal MAQUANFA { get; set; }
        public DateTime MAQUFEIN { get; set; }
        public string MAQUIDSP { get; set; }
        public string MAQUPRIO { get; set; }
        public string MAQUPRID { get; set; }
        public string MAQUTTIM { get; set; }
        public string MAQUCTIM { get; set; }
        public string MAQUCECO { get; set; }
        public decimal MAQUIDLO { get; set; }
        public string LOINDESC { get; set; }
        public decimal MAQUIDPL { get; set; }
        public string PLANDESC { get; set; }
        public decimal MAQUPROM { get; set; }
        public string MAQUPRAU { get; set; }
        public decimal MAQULEAC { get; set; }
        public DateTime MAQUFELE { get; set; }
        public string MAQUESTA { get; set; }
        public string MAQUCOPR { get; set; }
        public string PRONOM { get; set; }
        public string PRORUC { get; set; }
        public string PRODIR { get; set; }
        public decimal MAQUNUM1 { get; set; }
        public decimal MAQUNUM2 { get; set; }
        public decimal MAQUNUM3 { get; set; }
        public string MAQUALF1 { get; set; }
        public string MAQUALF2 { get; set; }
        public string MAQUALF3 { get; set; }
        public string MAQUOBSE { get; set; }
        public string MAQUUSCR { get; set; }
        public DateTime MAQUFECR { get; set; }
        public DateTime MAQUHOCR { get; set; }
        public string MAQUUSMD { get; set; }
        public DateTime MAQUFEMD { get; set; }
        public DateTime MAQUHOMD { get; set; }
    }

    public class MTPLAN
    {
        public decimal PLANCIA { get; set; }
        public decimal PLANIDPL { get; set; }
        public string PLANDESC { get; set; }
        public string PLANREGI { get; set; }
        public string PLANREGD { get; set; }
        public string PLANTTUL { get; set; }
        public string PLANCTUL { get; set; }
        public string TIUNLE { get; set; }
        public string PLANOBSE { get; set; }
        public string PLANESTA { get; set; }
        public string PLANESTD { get; set; }
        public string PLANUSCR { get; set; }
        public DateTime PLANFECR { get; set; }
        public DateTime PLANHOCR { get; set; }
        public string PLANUSMD { get; set; }
        public DateTime PLANFEMD { get; set; }
        public DateTime PLANHOMD { get; set; }
    }

    public class MTTBTI
    {
        public decimal TBTICIA { get; set; }
        public string TBTITABL { get; set; }
        public string TBTICODI { get; set; }
        public string TBTIDESC { get; set; }
        public decimal TBTINUM1 { get; set; }
        public decimal TBTINUM2 { get; set; }
        public decimal TBTINUM3 { get; set; }
        public string TBTIALF1 { get; set; }
        public string TBTIALF2 { get; set; }
        public string TBTIALF3 { get; set; }
    }

    public class MTLOIN
    {
        public decimal LOINCIA { get; set; }
        public decimal LOINIDLO { get; set; }
        public decimal LOINIDPA { get; set; }
        public string LOINDESC { get; set; }
        public string LOINCECO { get; set; }
        public string LOINOBSE { get; set; }
        public string LOINUSCR { get; set; }
        public string LOINUSMD { get; set; }
    }


    public class MTPART
    {
        public decimal PARTCIA { get; set; }
        public decimal PARTIDPL { get; set; }
        public decimal PARTIDPA { get; set; }
        public decimal PARTIDPP { get; set; }
        public string PARTDESC { get; set; }
        public string PARTUSCR { get; set; }
        public string PARTUSMD { get; set; }
    }

    public class MTACTI
    {
        public decimal ACTICIA { get; set; }
        public decimal ACTIIDPL { get; set; }
        public decimal ACTIIDPA { get; set; }
        public decimal ACTIIDAC { get; set; }
        public string PARTDESC { get; set; }
        public string ACTIDESC { get; set; }
        public decimal ACTIFREC { get; set; }
        public string ACTIPRIO { get; set; }
        public DateTime ACTIDURA { get; set; }
        public string ACTIRQPA { get; set; }
        public decimal ACTIDIPA { get; set; }
        public string ACTIDSPR { get; set; }
        public string ACTIOBSE { get; set; }
        public string ACTILETL { get; set; }
        public decimal ACTIFRLE { get; set; }
        public string ACTIPRFR { get; set; }
        public string ACTIACTI { get; set; }

        public string ACTIUSCR { get; set; }
        public string ACTIUSMD { get; set; }
    }

    public class DETCON
    {
        public string NROCONTRATO { get; set; }
        public string CLIENTE { get; set; }
        public string ARTICULO { get; set; }
        public string PARTIDA { get; set; }
        public decimal CANTIDAD { get; set; }
        public DateTime FECHACOMPROMISO { get; set; }
        public DateTime FECHAEMBARQUE { get; set; }
        public string COLORCLIENTE { get; set; }
        public decimal PESO { get; set; }
    }

    public class FORPAG
    {
        public string TIPCOM { get; set; }
        public string TICPRB { get; set; } 
        public string SERCPR { get; set; }
        public string NUMCPR { get; set; }
        public string TIPPER { get; set; }
        public string TIPDOC { get; set; }
        public string NRODOC { get; set; }
        public string MEDPAG { get; set; }
        public string CODBAN { get; set; }
        public string NUMOPE { get; set; }
        public string FECOPE { get; set; }
        public decimal MONOPE { get; set; }
        public string FPLINEA { get; set; }

    }

    public class REGCOM
    {
        public string TIPCOM { get; set; }
        public string TCPROB { get; set; }
        public string FECEMI { get; set; }

        public string SERCPR { get; set; }
        public string NROCPR { get; set; }
        public string TIPPER { get; set; }
        public string TIPDOC { get; set; }
        public string NRODOC { get; set; }
        public string PRONOM { get; set; }
        public string APEPAT { get; set; }
        public string APEMAT { get; set; }
        public string PRINOM { get; set; }
        public string SEGNOM { get; set; }
        public string TIPMON { get; set; }
        public string CODDES { get; set; }
        public string NUMDES { get; set; }
        public decimal BASIMP { get; set; }
        public decimal ISC { get; set; }
        public decimal IGV { get; set; }
        public decimal OTROS { get; set; }
        public string INDDET { get; set; }
        public string CODTAS { get; set; }
        public string NUMCON { get; set; }
        public string INDRET { get; set; }
        public string TCPROBREF { get; set; }
        public string SERREF { get; set; }
        public string NROREF { get; set; }
        public string FECREF { get; set; }
        public decimal BASIMP2 { get; set; }
        public decimal IGV2 { get; set; }
        public string RCLINEA { get; set; }
    }

    public class REGVEN
    {
        public string TIPCOM { get; set; }
        public string TCPROB { get; set; }
        public string FEMPAG { get; set; }

        public string NSCPRB { get; set; }
        public string NCPROB { get; set; }
        public string TIPPER { get; set; }
        public string TIPDOC { get; set; }
        public string NRODOC { get; set; }
        public string NOMBRE { get; set; }
        public string APEPAT { get; set; }
        public string APEMAT { get; set; }
        public string NOMBR1 { get; set; }
        public string NOMBR2 { get; set; }
        public string TIPMON { get; set; }
        public string CODDES { get; set; }
        public string NRODES { get; set; }
        public decimal BASIMP { get; set; }
        public decimal MONISC { get; set; }
        public decimal MONIGV { get; set; }
        public decimal MONOTR { get; set; }
        public string INDETR { get; set; }
        public string CODDET { get; set; }

        public string NROCON { get; set; }
        public string INDRET { get; set; }
        public string TCPRBP { get; set; }
        public string SRCPRB { get; set; }
        public string NRCPRB { get; set; }
        public string FCEMIS { get; set; }
        public decimal BIMPON { get; set; }
        public decimal IGV { get; set; }
        public string RVLINEA { get; set; }
    }

    public class BDPLPR
    {
        public string ARBONUPE { get; set; }
        public string ARBOTIPE { get; set; }
        public decimal ARBOSEPE { get; set; }
        public decimal ARBOSEPI { get; set; }
        public string ARBOFLPE { get; set; }
        public string CVTCLIPR { get; set; }

        public decimal TUPRUPRD { get; set; }
        public string LIPRDESC { get; set; }
        public decimal ARBOUNPR { get; set; }
        public string TUPRDESC { get; set; }
        public string ARBOARTI { get; set; }
        public string PENDSTAT { get; set; }
        public string ASIGNUPA { get; set; }
        public string PARTARTI { get; set; }
        public string PARTSTPR { get; set; }

        public decimal PARTPESO { get; set; }
        public decimal PARTCAPR { get; set; }
        public decimal CVTDPESO { get; set; }
        public decimal PARTLONG { get; set; }
        public decimal CVTDLONG { get; set; }
        public decimal CANTPP { get; set; }
        public decimal CANTPRG { get; set; }
        public decimal CANTOT { get; set; }

        public decimal ARBOCANT { get; set; }
        public string FEIA { get; set; }
        public string FERP { get; set; }
        public string FECCTO { get; set; }

        public decimal CVTDFEMO { get; set; }
        public DateTime CVTAFPLA { get; set; }
        public DateTime CVTAFPLC { get; set; }
        public decimal SEMANA2 { get; set; }
        public string SEMANAPARTE { get; set; }
        public string COINTICO { get; set; }
        public string MCSICL { get; set; }
        public string CODETQ { get; set; }
        public string ARTDES { get; set; }
        public decimal CVTCPCON { get; set; }
        public string PENDTENI { get; set; }
        public decimal PENDNIVE { get; set; }

        public string MAQASI { get; set; }
        public string TIPFIB { get; set; }
        public string CLASIFHILATURA { get; set; }
        public string CLASIFHK { get; set; }
        public string CLASIFHK2 { get; set; }
        public string CLASIFHOMOG2 { get; set; }
        public string CLASIFHOMOG { get; set; }
        public string CLASIFAGREGHIL { get; set; }
        public string CLASIFPESO { get; set; }
        public string CLASIFTINTO { get; set; }
        public string CLASIFTINTOTI { get; set; }
        public string CLASIFHILATFC { get; set; }
        public string CLASIFPEINA { get; set; }
	
    }

    public class PRTUPR
    {
        public decimal TUPRCODI { get; set; }
        public string TUPRDESC { get; set; }
        public decimal TUPRUPRD { get; set; }
    }

    //public class PPHZPR
    //{
    //    public decimal HZPRCIA { get; set; }
    //    public decimal HZPRCOLP { get; set; }
    //    public string LIPRDESC { get; set; }
    //    public decimal HZPRDIHP { get; set; }
    //}

    public class PRTIRE
    {
        public decimal TIRECODI { get; set; }
        public string TIREDESC { get; set; }
    }

    public class PPMQFA
    {
        public decimal MQFAIDMF { get; set; }
        public decimal MQFACIA { get; set; }
        public string MQFAITEM { get; set; }
        public string ARTDES { get; set; }
        public decimal MQFAIDMA { get; set; }
        public string MAQUCOMA { get; set; }
        public string MAQUDES2 { get; set; }
        public string MAQUMARC { get; set; }
        public decimal MQFARNDE { get; set; }
        public decimal MQFARANA { get; set; }
        public string MQFAUSRC { get; set; }
        public DateTime MQFAFECR { get; set; }
        public DateTime MQFAHOCR { get; set; }
        public string MQFAUSMD { get; set; }
        public DateTime MQFAFEMD { get; set; }
        public DateTime MQFAHOMD { get; set; }
    }

    public class PPRUPR
    {
        public decimal ARBOCIA { get; set; }
        public decimal ARBOSEPE { get; set; }
        public decimal ARBOSEPI { get; set; }
        public string ARBONUPE { get; set; }
        public string ARBOTIPE { get; set; }
        public string ARBOARTI { get; set; }
        public DateTime PENDFERP { get; set; }
        public string MCSICL { get; set; }
        public decimal PENDCAPP { get; set; }
        public string PENDSTAT { get; set; }
        public string LIPRDESC { get; set; }
        public string TUPRDESC { get; set; }
        public decimal RUTASETP { get; set; }
        public string TTPRDESC { get; set; }
        public decimal RUPRSETP { get; set; }
        public string RUPRSETC { get; set; }
        public string RUPRUSCR { get; set; }
    }

    public class PPCTGP
    {
        public decimal CTGPCIA { get; set; }
        public decimal CTGPLICO { get; set; }
        public string CTGPCOCA { get; set; }
        public string CTGPCOCAORI { get; set; }
        public string CTGPTBGP { get; set; }
        public string CTGPCOGP { get; set; }
        public string TGPMDESC { get; set; }
    }

    public class PPTGPM
    {
        public decimal TGPMCIA { get; set; }
        public string TGPMTABL { get; set; }
        public string TGPMCODI { get; set; }
        public string TGPMDESC { get; set; }
    }

    //REMOVER LO ANTERIOR
    public class PRLIPR
    {
        public decimal LIPRCIA { get; set; }
        public decimal LIPRCODI { get; set; }
        public string LIPRDESC { get; set; }
        public string LIPRUNGE { get; set; }
        public string CLIPESTA { get; set; }
    }

    public class PRTTPR
    {
        public decimal TTPRCIA { get; set; }
        public decimal TTPRCODI { get; set; }
        public string TTPRDESC { get; set; }
        public decimal LIPRCODI { get; set; }
        public string CTTPESTA { get; set; }
    }

    public class PCMVAR
    {
        public decimal MVARIDVA { get; set; }
        public string MVARNOMB { get; set; }
        public string MVARDECO { get; set; }
        public string MVARTBUM { get; set; }
        public string MVARTCUM { get; set; }
        public string UNMEDESC { get; set; }
        public string MVARVACC { get; set; }
        public string MVARVACP { get; set; }
        public string MVARRQVA { get; set; }
        public string MVARTBTV { get; set; }
        public string MVARTCTV { get; set; }
        public string TPVADESC { get; set; }
        public string MVARTBMC { get; set; }
        public string MVARTCMC { get; set; }
        public string MTCADESC { get; set; }
        public string MVARESTA { get; set; }
        public DateTime MVARFHCR { get; set; }
        public string MVARUSCR { get; set; }
        public DateTime MVARFHMD { get; set; }
        public string MVARUSMD { get; set; }
    }

    public class PCTAUX
    {
        public string TAUXTABL { get; set; }
        public string TAUXCODI { get; set; }
        public string TAUXTABP { get; set; }
        public string TAUXCODP { get; set; }
        public string TAUXDESC { get; set; }
        public decimal TAUXNUM1 { get; set; }
        public decimal TAUXNUM2 { get; set; }
        public string TAUXALF1 { get; set; }
        public string TAUXALF2 { get; set; }
        public string TAUXESTA { get; set; }
    }

    public class PCMQPR
    {
        public decimal GRMQCIA { get; set; }
        public decimal TTPRCODI { get; set; }
        public string TTPRDESC { get; set; }
        public decimal GRMQCODI { get; set; }
        public string GRMQDESC { get; set; }
        public decimal MAQUIDMA { get; set; }
        public string MAQUCOMA { get; set; }
        public string MAQUDES2 { get; set; }
        public string MAQUMARC { get; set; }
        public string MAQUCOAC { get; set; }
    }

    public class PCVRPR
    {
        private string mVRPROBLI;
        public decimal VRPRCOLN { get; set; }
        public decimal VRPRCOPR { get; set; }
        public decimal VRPRIDMA { get; set; }
        public decimal VRPRIDVA { get; set; }
        public decimal MVARIDVA { get; set; }
        public string MVARNOMB { get; set; }
        public string VRPROBLI
        {
            get {
                return mVRPROBLI;
            }
            set {
                mVRPROBLI = value;
                VRPROBLI2 = value.Equals("S"); } 
        }
        public bool VRPROBLI2 { get; set; }
        public string VRPRFLA1 { get; set; }
        public string VRPRUSCR { get; set; }
        public DateTime VRPRFHCR { get; set; }
        public string VRPRUSMD { get; set; }
        public DateTime VRPRFHMD { get; set; }
        public string COLGRILLA { get; set; }
    }

    public class PCVRPR2
    {
        public decimal LIPRCODI { get; set; }
        public string LIPRDESC { get; set; }
        public decimal TTPRCODI { get; set; }
        public string TTPRDESC { get; set; }
        public decimal VRPRIDMA { get; set; }
        public string MAQUDES2 { get; set; }
        
        public decimal MVARIDVA { get; set; }
        public string MVARNOMB { get; set; }

    }

    public class GRROAP
    {
        public string GRUSCOGR { get; set; }
        public string GRUSDEGR { get; set; }
        public string ROAPCOAP { get; set; }
        public string ROAPESTA { get; set; }
    }

    public class GAUSUA
    {
        public string GRUSCOGR { get; set; }
        public string USUACOUS { get; set; }
        public string USUANOUS { get; set; }
        public string USUAMAIL { get; set; }
        public string USUAUSAD { get; set; }
    }

    public class PCNVAP
    {
        public decimal NVAPIDNI { get; set; }
        public decimal NVAPTICO { get; set; }
        public decimal NVAPIDNP { get; set; }
        public string NVAPDESC { get; set; }
        public string NVAPUSCR { get; set; }
        public DateTime NVAPFHCR { get; set; }
        public string NVAPUSMD { get; set; }
        public DateTime NVAPFHMD { get; set; }        

    }

    public class PCTEST
    {
        public decimal TESTIDTE { get; set; }
        public decimal TESTIDVA { get; set; }
        public string MVARNOMB { get; set; }
        public string MVARDECO { get; set; }
        public decimal TESTCOLP { get; set; }
        public string LIPRDESC { get; set; }
        public decimal TESTCOTP { get; set; }
        public string TTPRDESC { get; set; }
        public string TESTCALI { get; set; }
        public string DECADESC { get; set; }
        public string TESTTPFI { get; set; }
        public string TPFIDESC { get; set; }
        public string TESTTBTE { get; set; }
        public string TESTTCTE { get; set; }
        public string TCTEDESC { get; set; }
        public string TCTEDECO { get; set; }
        public decimal TESTVAL1 { get; set; }
        public decimal TESTVAL2 { get; set; }
        public string TESTTBLV { get; set; }
        public string TESTTCLV { get; set; }
        public DateTime TESTFHCR { get; set; }
        public string TESTUSCR { get; set; }
        public DateTime TESTFHMD { get; set; }
        public string TESTUSMD { get; set; }
    }
    
    public class PRCADE
    {
        public string CALICODI { get; set; }
        public string DECADESC { get; set; }
        public string GRCACODI { get; set; }
        public string GRCADESC { get; set; }
        public string GRCATPFI { get; set; }
    }

    //AUTORIZACION FUERA DE ESTANDAR
    public class PCATFE
    {
        public decimal ATFEIDED { get; set; }
        public DateTime ATFEFEEM { get; set; }
        public decimal ATFEIDNV { get; set; }
        public string ATFEESTA { get; set; }
        public string ATFEOBSE { get; set; }
        public string ATFEOBSA { get; set; }
        public string ATFEUSCR { get; set; }
        public DateTime ATFEFHCR { get; set; }
        public string ATFEUSAT { get; set; }

        public string ENSAPART { get; set; }
        public DateTime ENSAFEEN { get; set; }

        public decimal ENSACLPR { get; set; }
        public decimal ENSACTPR { get; set; }
        public string LIPRDESC { get; set; }
        public string TTPRDESC { get; set; }
        public string MAQUDES2 { get; set; }

        public string MVARNOMB { get; set; }
        public string MVARDECO { get; set; }
        public decimal ENVAIDVA { get; set; }
        public decimal ENVAIDTE { get; set; }
        public decimal ENVAVALO { get; set; }
        public decimal ENVAVAE1 { get; set; }
        public decimal ENVAVAE2 { get; set; }

        //public decimal TESTCOLP { get; set; }
        //public string TESTCALI { get; set; }
        //public string TESTTBLV { get; set; }
        //public string TESTTCLV { get; set; }
        //public DateTime TESTFHCR { get; set; }
        //public string TESTUSCR { get; set; }
        //public DateTime TESTFHMD { get; set; }
        //public string TESTUSMD { get; set; }
    }


    public class PCENSA
    {
        public decimal ENSAIDEN { get; set; }
        public decimal ENSANUEN { get; set; }
        public decimal ENSACLPR { get; set; }
        public decimal ENSACTPR { get; set; }
        public decimal ENSAIDMA { get; set; }
        public DateTime ENSAFEEN { get; set; }
        public string ENSAPART { get; set; }
        public decimal ENSATURN { get; set; }
        public string ENSAESTA { get; set; }
        public string ENSAESEX { get; set; }
        public string ENSAESPR { get; set; }
        public string ENSAESCT { get; set; }
        public decimal ENSACTNM { get; set; }
        public string ENSACTSM { get; set; }
        public string ENSACTDR { get; set; }
        public string ENSACTAR { get; set; }
        public string ENSACOPR { get; set; }
        public string ENSAOBSE { get; set; }
        public DateTime ENSAFHCR { get; set; }
        public string ENSAUSCR { get; set; }
        public DateTime ENSAFHMD { get; set; }
        public string ENSAUSMD { get; set; }

    }


    //para generacion de grilla jqwidget
    public class columns
    {
        public string text { get; set; }
        public string datafield { get; set; }
        public string width { get; set; }
        public bool editable { get; set; }
        public string columntype { get; set; }
    }

    public class fieldsgrilla
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class PCPRNC
    {
        public decimal PRNCIDNC { get; set; }
        public decimal PRNCLIPR { get; set; }
        public decimal PRNCNUSO { get; set; }
        public decimal PRNCIDED { get; set; }
        public DateTime PRNCFEEM { get; set; }
        public string PRNCPART { get; set; }
        public string PRNCESTA { get; set; }
        public string PRNCOBSS { get; set; }
        public DateTime PRNCFEAU { get; set; }
        public string PRNCESAC { get; set; }

        public string PRNCESRP { get; set; }
        public DateTime PRNCFELI { get; set; }
        public decimal PRNCALMA { get; set; }
        public string PRNCUSCR { get; set; }
        public DateTime PRNCFHCR { get; set; }
        public string PRNCUSMD { get; set; }
        public DateTime PRNCFHMD { get; set; }

        public string LIPRDESC { get; set; }
        
    }

    public class PCPNCA
    {
        public decimal PNCAIDAN { get; set; }
        public decimal PNCAIDNC { get; set; }
        public decimal PNCAIDNI { get; set; }
        public DateTime PNCAFEAU { get; set; }
        public string PNCAESAC { get; set; }

        public string PNCAESRP { get; set; }
        public decimal PNCAALMA { get; set; }
        public string PNCAOBSA { get; set; }
        public string PNCAUSCR { get; set; }
        public DateTime PNCAFHCR { get; set; }
        public string PNCAUSMD { get; set; }
        public DateTime PNCAFHMD { get; set; }
    }

    public class PCNAPN
    {
        public decimal NAPNIDNI { get; set; }
        public decimal NAPNORDE { get; set; }
        public decimal NAPNIDNX { get; set; }
        public string NAPNCOGR { get; set; }
        public string NAPNUSCR { get; set; }
        public DateTime NAPNFHCR { get; set; }
        public string NAPNUSMD { get; set; }
        public DateTime NAPNFHMD { get; set; }
    }
}
