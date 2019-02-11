using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data;
using IBM.Data.DB2.iSeries;

using appWcfService;
//using AccesoDatos;
using appConstantes;

using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using EFModelo;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace appLogica
{
    public class MKT
    {
        //internal BaseDatos DB2;

        #region variables
        //correo
        private string ServidorSMTP, CuentaDe, CuentaDescripcion, ClaveCuenta, DominioCuenta, PuertoSMTP;

        private bool OcultaErrorReal;
        private string Aplicacion;
        private string DataSourceDB2;

        public static string NombreCLDescargaInventario = "GMA003PP"; //GMA003P 20151130 PRUEBAS USUARIO //GMA003PP produccion

        #endregion

        #region constantes

        private const decimal TIPO_MOV_SALIDA_PREP_PED = 1; //-
        private const decimal TIPO_MOV_CANCELA_SALIDA_PREP_PED = 2; //+
        private const decimal TIPO_MOV_MODIFICA_SALIDA_PREP_PED = 3;  //+

        #endregion

        public MKT()
        {
            string stringConnection, userId, userPassword;
            DataSourceDB2 = ConfigurationManager.AppSettings["DataSource"];
            Aplicacion = ConfigurationManager.AppSettings["Aplicacion"];

            //userId = "PCS400";
            //userPassword = "pcs400";
            userId = "PCVTAS";
            userPassword = "PCVTAS";

            stringConnection = "DataSource=" + DataSourceDB2 + ";User Id=" + userId + ";Password=" + userPassword + ";Naming=System;LibraryList=QS36F,INCAOBJ,PRODDAT,MANTDAT;CheckConnectionOnOpen=true;";
            //DB2 = new BaseDatos(stringConnection);

            ServidorSMTP = ConfigurationManager.AppSettings["Smtp"];
            CuentaDe = ConfigurationManager.AppSettings["DeSmtp"];
            CuentaDescripcion = ConfigurationManager.AppSettings["NombreSmtp"];
            ClaveCuenta = ConfigurationManager.AppSettings["ClaveSmtp"];
            DominioCuenta = ConfigurationManager.AppSettings["DominioSmtp"];
            PuertoSMTP = ConfigurationManager.AppSettings["PuertoSmtp"];


            if (ConfigurationManager.AppSettings["OcultaErrorReal"].Equals("1"))
            {
                OcultaErrorReal = true;
            }
            else
            {
                OcultaErrorReal = false;
            }
        }

        //Clientes
        //public RESOPE BuscaCliente(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.TCLIE> lista = null;
        //    try
        //    {
        //        string valbus;
        //        valbus = paramOperacion.VALENT[0].Trim();

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            //listaeo = context.TCLIE.Where(cli => cli.CLICVE.Contains(valbus) || cli.CLINOM.Contains(valbus) || cli.CLIRUC.Contains(valbus)).ToList<object>();
        //            listaeo = context.USP_BUSCA_CLIENTE(valbus).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.TCLIE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);

        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}
        //Articulos
        public RESOPE BuscaArticulo(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_ARTICULOS_Result> lista = null;
            try
            {
                string contrato, articulo, partida, selec;
                contrato = paramOperacion.VALENT[0];
                articulo = paramOperacion.VALENT[1];
                partida = paramOperacion.VALENT[2];
                selec = paramOperacion.VALENT[3];

                partida = String.IsNullOrWhiteSpace(partida) ? null : partida;
                contrato = string.IsNullOrWhiteSpace(contrato) ? null : contrato;
                //articulo = String.IsNullOrWhiteSpace(partida) ? null : articulo;
                using (var context = new PEDIDOSEntities())
                {
                    //if (contrato.Length == 0)
                    //{
                    listaeo = context.USP_OBTIENE_ARTICULOS(contrato, partida, articulo, selec).ToList<object>();
                    //}
                    //else
                    //{
                    //    //completar
                    //}
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_ARTICULOS_Result>(listaeo);
                vpar.VALSAL = new List<string>();
                if (lista.Count > 0)
                {
                    vpar.VALSAL.Add("1");
                    vpar.VALSAL.Add(Util.Serialize(lista));
                }
                else
                {
                    vpar.VALSAL.Add("0");

                }
                vpar.ESTOPE = true;

            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {
            }
            return vpar;
        }//COMPLICADO AL FINAL

        //Pedidos area comercial
        //public RESOPE GuardaPedido(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    decimal idpedido, idestado, idcabpedido;
        //    string numeropedido;

        //    vpar = new RESOPE() { ESTOPE = false };
        //    idpedido = 0;
        //    idestado = 0;
        //    idcabpedido = 0;

        //    appWcfService.PECAPE pedido;
        //    List<appWcfService.PEDEPE> lista = null;
        //    List<appWcfService.PEDEPE> listaborrados = null;

        //    appLogica.appDB2 _appDB2 = null;
        //    try
        //    {
        //        int secuencia;
        //        _appDB2 = new appLogica.appDB2();
        //        pedido = Util.Deserialize<appWcfService.PECAPE>(paramOperacion.VALENT[0]);
        //        lista = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[1]);

        //        //articulo = String.IsNullOrWhiteSpace(partida) ? null : articulo;
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var ped = context.PECAPE.Find(pedido.CAPEIDCP);
        //            if (ped == null)
        //            {
        //                ped = new EFModelo.PECAPE();
        //                ped.CAPEFECR = DateTime.Now;
        //                ped.CAPEUSCR = pedido.CAPEUSCR;
        //                ped.CAPESERI = pedido.CAPESERI;
        //                ped.CAPEIDES = Constantes.ID_ESTADO_CREADO;
        //                context.PECAPE.Add(ped);
        //            }
        //            else
        //            {
        //                ped.CAPEFEMO = DateTime.Now;
        //                ped.CAPEUSMO = pedido.CAPEUSCR; //el usuario peude enviarse aqui siempre, no es necesario en el mod
        //                if (ped.CAPEIDES == 1 && pedido.CAPEIDES == 1)
        //                {
        //                    ped.CAPEIDES = Constantes.ID_ESTADO_CREADO;
        //                }
        //            }
        //            ped.CAPEIDCL = pedido.CAPEIDCL;
        //            ped.CAPENOTG = pedido.CAPENOTG;
        //            ped.CAPENOTI = pedido.CAPENOTI;
        //            ped.CAPEFECH = pedido.CAPEFECH;
        //            ped.CAPEDIRE = pedido.CAPEDIRE;
        //            ped.CAPEEMAI = pedido.CAPEEMAI;
        //            ped.CAPETIPO = pedido.CAPETIPO;
        //            ped.CAPEIDTD = pedido.CAPEIDTD;
        //            ped.CAPEDEST = pedido.CAPEDEST;
        //            //////////elimna borrados de la grilla
        //            listaborrados = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[2]);
        //            foreach (var item in listaborrados)
        //            {
        //                var det = context.PEDEPE.Find(item.DEPEIDDP);
        //                if (det != null)
        //                {
        //                    context.PEDEPE.Remove(det);
        //                }
        //            }
        //            //////
        //            context.SaveChanges();
        //            idpedido = ped.CAPEIDCP;

        //            secuencia = 0;
        //            foreach (var item in lista)
        //            {
        //                secuencia++;

        //                bool generarres = false;
        //                var det = context.PEDEPE.Find(item.DEPEIDDP);
        //                if (det == null)
        //                {
        //                    det = new EFModelo.PEDEPE();
        //                    det.DEPEFECR = DateTime.Now;
        //                    det.DEPEUSCR = pedido.CAPEUSCR;
        //                    det.DEPEESTA = Constantes.ID_ESTADO_CREADO;
        //                    context.PEDEPE.Add(det);
        //                    generarres = true;
        //                }
        //                else
        //                {
        //                    if (det.DEPEIDCP != ped.CAPEIDCP)
        //                    {
        //                        throw new Exception("Inconsistencia de datos, ID de pedido no coincide con Id de detalle");
        //                    }
        //                    //Eliminar el existente y crear de nuevo la reserva
        //                    det.DEPEFEMO = DateTime.Now;
        //                    det.DEPEUSMO = pedido.CAPEUSCR; //EL MISMO USUARIO
        //                    if (det.DEPEPESO != item.DEPEPESO || det.DEPECOAR.Trim() != item.DEPECOAR.Trim() || det.DEPEPART != item.DEPEPART)
        //                    {
        //                        _appDB2.generaReserva(false, "X", Convert.ToInt32(ped.CAPEIDCP).ToString(), Convert.ToString(det.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");

        //                        generarres = true;
        //                    }
        //                }
        //                det.DEPEIDCP = ped.CAPEIDCP;
        //                det.DEPEALMA = item.DEPEALMA;
        //                det.DEPECASO = item.DEPECASO;
        //                det.DEPEPESO = item.DEPEPESO;
        //                det.DEPECOAR = item.DEPECOAR;
        //                det.DEPEPART = item.DEPEPART;
        //                det.DEPECONT = item.DEPECONT;
        //                det.DEPEDISP = item.DEPEDISP;
        //                det.DEPEDSAR = item.DEPEDSAR;
        //                det.DEPESERS = secuencia;
        //                det.DEPESECU = item.DEPESECU;
        //                context.SaveChanges();

        //                if (generarres)
        //                {
        //                    _appDB2.generaReserva(false, "X", Convert.ToInt32(ped.CAPEIDCP).ToString(), Convert.ToString(secuencia), det.DEPECOAR, det.DEPEPART, Convert.ToString(det.DEPEALMA), det.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(det.DEPEPESO * 100)), "A");
        //                }

        //            }

        //            //ACTUALIZA EL NRO
        //            var nume = context.PECAPE.Where(x => x.CAPESERI == ped.CAPESERI).Max(x => x.CAPENUME);
        //            nume++;
        //            if (ped.CAPENUME == 0)
        //            {
        //                ped.CAPENUME = nume;
        //            }
        //            numeropedido = ped.CAPENUME.ToString().PadLeft(7, '0');
        //            idestado = ped.CAPEIDES;
        //            idcabpedido = ped.CAPEIDCP;
        //            context.SaveChanges();

        //        }
        //        vpar.VALSAL = new List<string>();
        //        vpar.VALSAL.Add(idpedido.ToString());
        //        vpar.VALSAL.Add(numeropedido);
        //        vpar.VALSAL.Add(idestado.ToString());
        //        vpar.VALSAL.Add(idcabpedido.ToString());

        //        vpar.ESTOPE = true;

        //    }
        //    //catch (DbEntityValidationException dbEx)
        //    //{
        //    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
        //    //    {
        //    //        foreach (var validationError in validationErrors.ValidationErrors)
        //    //        {
        //    //            Trace.TraceInformation("Property: {0} Error: {1}",
        //    //                                    validationError.PropertyName,
        //    //                                    validationError.ErrorMessage);
        //    //        }
        //    //    }
        //    //}
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }

        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //private void anularReservasPedidoAnulado(PEDIDOSEntities context, appWcfService.PECAPE pedido)
        //{
        //    appLogica.appDB2 _appDB2 = null;
        //    _appDB2 = new appLogica.appDB2();

        //    var listdet = context.PEDEPE.Where(x => x.DEPEIDCP == pedido.CAPEIDCP).ToList();
        //    foreach (var item in listdet)
        //    {
        //        _appDB2.generaReserva(false, "X", Convert.ToInt32(pedido.CAPEIDCP).ToString(), Convert.ToString(item.DEPESERS), item.DEPECOAR, item.DEPEPART, Convert.ToInt32(item.DEPEALMA).ToString(), item.DEPECONT, "Z", "0", "0", "0", Convert.ToString(Convert.ToInt32(item.DEPEPESO * 100)), "L");
        //    }

        //}

        //public RESOPE MuestraPedidos(PAROPE paramOperacion)
        //{
        //    //Muestra pedidos con estado 1
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        int valbus;
        //        valbus = int.Parse(paramOperacion.VALENT[0]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (valbus == 0)
        //            {
        //                listaeo = context.PECAPE.Include("TCLIE").OrderByDescending(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEFECH).ToList<object>();
        //            }
        //            else
        //            {
        //                listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == valbus).ToList<object>();
        //            }
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//listo

        //public RESOPE MuestraPedidosPendientes(PAROPE paramOperacion)
        //{
        //    //Pedidos pendientes de aprobacion

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        //string valbus;
        //        //valbus = paramOperacion.VALENT[0];

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == 4).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//NO SE USA

        //public RESOPE BuscaPedidos(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        string Finicio, Ffin, filtro, estado, busqueda, serie;
        //        Finicio = paramOperacion.VALENT[0];
        //        Ffin = paramOperacion.VALENT[1];
        //        filtro = paramOperacion.VALENT[2];
        //        estado = paramOperacion.VALENT[3];
        //        busqueda = paramOperacion.VALENT[4];
        //        serie = paramOperacion.VALENT[5];

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            //listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.TCLIE.CLINOM.Contains(valbus) && ped.CAPEIDES==estado || ped.CAPEUSCR.Contains(valbus) && ped.CAPEIDES == estado).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);

        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//NO SE USA

        //public RESOPE BuscaPedidosFechas(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {   //Variables         
        //        string valbus, fechainicio, fechafin, serie;
        //        decimal estado, nume = 0;

        //        valbus = paramOperacion.VALENT[0];
        //        estado = decimal.Parse(paramOperacion.VALENT[1]);
        //        fechainicio = paramOperacion.VALENT[2];
        //        fechafin = paramOperacion.VALENT[3];
        //        serie = paramOperacion.VALENT[4];

        //        Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
        //        Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.


        //        bool num = regnum.IsMatch(valbus); //que tenga numeros
        //        bool letra = reglet.IsMatch(valbus); //que tenga letras


        //        if (num && !letra)//se verifica que tenga numeros pero ninguna letra
        //        {
        //            nume = decimal.Parse(paramOperacion.VALENT[0]);
        //        }

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (fechainicio == "" && fechafin == "") //Busquedas sin fechas
        //            {
        //                if (estado == 0 && serie.Equals("")) //Si el estado es 0 Mostrara todos los pedidos
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //                else if (estado == 0 & serie != "") //Todos los estados, sin parametro de busqueda pero con serie
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //                //filtro segun estado indicado
        //                else if (valbus != "" && serie.Equals("")) //Mostrara todos con el estado indicado y sin serie
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //               && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //                else if (valbus.Equals("") && serie != "") // busca con serie indicada y sin valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //                else if (valbus != "" && serie != "") //busca con serie inidicada y con valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //               && ped.CAPEIDES == estado && serie.Contains(serie)).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //                else //Solo busca por el estado
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO == 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                }
        //            }
        //            //Busqueda por fechas
        //            else if (fechainicio != "" && fechafin != "")
        //            {
        //                DateTime feini, fefin;
        //                feini = DateTime.Parse(fechainicio);
        //                fefin = DateTime.Parse(fechafin);
        //                //Busqueda sin estado
        //                if (estado == 0 && serie.Equals("") && valbus.Equals("")) //toma en cuenta solo el rango de fechas
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin).ToList<object>();
        //                }
        //                else if (estado == 0 && serie.Equals("") && valbus != "") // toma en cuenta el rango y valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //                    && ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin).ToList<object>();
        //                }
        //                else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta el rango de fechas mas la serie
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPESERI.Contains(serie)).ToList<object>();
        //                }
        //                else if (estado == 0 && serie != "" && valbus != "") //toma en cuenta el rango de fechas, serie y valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //                  && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin)) && ped.CAPESERI.Contains(serie)).ToList<object>();
        //                }

        //                // Busqueda con estado
        //                else if (estado != 0 && serie.Equals("") && valbus.Equals("")) //toma en cuenta el rango de fechas y el estado
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado).ToList<object>();
        //                }
        //                else if (estado != 0 && serie != "" && valbus.Equals("")) //toma en cuenta el rango de fechas, la serie y el estado
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado) && ped.CAPESERI.Contains(serie)).ToList<object>();
        //                }
        //                else if (estado != 0 && serie.Equals("") && valbus != "") //toma en cuenta el rango de fechas, el estado, y el valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //                    && ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado)).ToList<object>();
        //                }
        //                else if (estado != 0 && serie != "" && valbus != "") // toma en cuenta el rango de fechas, estado, serie y valor de busqueda
        //                {
        //                    listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume)
        //                    && ((ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin) && ped.CAPEIDES == estado) && ped.CAPESERI.Contains(serie)).ToList<object>();
        //                }
        //            }
        //        }

        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE MuestraDetallePedidos(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<object> listaeo2 = new List<object>();

        //    List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> lista = null;
        //    List<appWcfService.PECAPE> lisIdPedido = null;
        //    try
        //    {
        //        lisIdPedido = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
        //        foreach (var item in lisIdPedido)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                listaeo = context.USP_OBTIENE_DETALLE_PEDIDOS(item.CAPEIDCP).ToList<object>();
        //            }
        //            listaeo2.AddRange(listaeo);
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>(listaeo2);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}// ya esta PRPEDAT.USP_MOSTRAR_DETALLE_PEDIDOS 

        //public RESOPE AnulaPedidos(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        lista = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
        //        foreach (var item in lista)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var ped = context.PECAPE.Find(item.CAPEIDCP);
        //                ped.CAPEIDES = 9;
        //                context.SaveChanges();
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE CambiaEstadoPedido(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    string est;
        //    //List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        decimal idpedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        int estado = int.Parse(paramOperacion.VALENT[1]);
        //        string usuario = paramOperacion.VALENT[2];
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var ped = context.PECAPE.Find(idpedido);
        //            ped.CAPEIDES = estado;
        //            ped.CAPEUSMO = ped.CAPEUSCR;
        //            ped.CAPEFEMO = DateTime.Now;
        //            switch (estado)
        //            {
        //                case 2:
        //                    ped.CAPEUSEM = usuario;
        //                    ped.CAPEFHEM = DateTime.Now;
        //                    break;

        //                default:
        //                    break;
        //            }
        //            context.SaveChanges();
        //            est = ped.CAPEIDES.ToString(); //Almacenamos el estado del pedido para devolverlo
        //        }

        //        vpar.VALSAL = new List<string>();
        //        vpar.VALSAL.Add("1");
        //        vpar.VALSAL.Add(est);
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE EliminaDetalle(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PEDEPE> lista = null;
        //    try
        //    {
        //        lista = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[0]);
        //        foreach (var item in lista)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var det = context.PEDEPE.Find(item.DEPEIDDP);
        //                context.PEDEPE.Remove(det);
        //                context.SaveChanges();
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //pedidos area almacen
        //public RESOPE MuestraPedidosAlmacen(PAROPE paramOperacion)
        //{
        //    //DMA 07/05/2018 Se ha modificado para que tome en cuenta la prioridad al momento de ordenar
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        DateTime feini, fefin;

        //        feini = DateTime.Parse(paramOperacion.VALENT[0]);
        //        fefin = DateTime.Parse(paramOperacion.VALENT[1]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                .OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//ya esta

        //public RESOPE BuscaPedidosAlmacen(PAROPE paramOperacion)
        //{
        //    //DMA 07/05/2018 Se ha modificado para que tome en cuenta la prioridad al momento de ordenar
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };


        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        string valbus, serie, fechainicio, fechafin;
        //        decimal estado, nume = 0;
        //        valbus = paramOperacion.VALENT[0];
        //        estado = decimal.Parse(paramOperacion.VALENT[1]);
        //        serie = paramOperacion.VALENT[2];
        //        fechainicio = paramOperacion.VALENT[3];
        //        fechafin = paramOperacion.VALENT[4];


        //        Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
        //        Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.
        //        bool num = regnum.IsMatch(valbus); //que tenga numeros
        //        bool letra = reglet.IsMatch(valbus); //que tenga letras

        //        if (num && !letra)//se verifica que tenga numeros pero ninguna letra
        //        {
        //            nume = decimal.Parse(paramOperacion.VALENT[0]);
        //        }
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            //BUSQUEDA SIN RANGO DE FECHAS
        //            if (fechainicio.Equals("") && fechafin.Equals(""))
        //            {
        //                if (estado == 0) //Busqueda sin estado primero
        //                {
        //                    if (estado == 0 && serie.Equals("") && valbus.Equals("")) //Muestra todos los pedidos 2 y 3
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == 2 || ped.CAPEIDES == 3)
        //                            .OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie.Equals("") && valbus != "") //Toma en cuenta el valor de busqueda
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta solo la serie
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && ped.CAPESERI.Contains(serie)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie != "" && valbus != "") // toma en cuenta serie y valor de busqueda
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && ped.CAPESERI.Contains(serie)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                }
        //                else if (estado != 0) //Busqueda por estado
        //                {
        //                    if (serie.Equals("") && valbus.Equals("")) // busca solo por el estado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie != "" && valbus.Equals("")) //busca solo por la serie con el estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie.Equals("") && valbus != "") // busca solo por el valor de busqueda con el estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie != "" && valbus != "") // busca por serie, valor de busqueda y estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPESERI.Contains(serie)) && ped.CAPEIDES == estado).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                DateTime feini, fefin;
        //                feini = DateTime.Parse(fechainicio);
        //                fefin = DateTime.Parse(fechafin);

        //                //Busqueda sin estado
        //                if (estado == 0) //Busqueda sin estado primero
        //                {
        //                    if (estado == 0 && serie.Equals("") && valbus.Equals("")) //Muestra todos los pedidos 2 y 3 
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                            .OrderByDescending(ped => ped.CAPEIDES).OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie.Equals("") && valbus != "") //Toma en cuenta el valor de busqueda
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                            .OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie != "" && valbus.Equals("")) //toma en cuenta solo la serie
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.CAPEIDES == 2 || ped.CAPEIDES == 3) && ped.CAPESERI.Contains(serie)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                            .OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (estado == 0 && serie != "" && valbus != "") // toma en cuenta serie y valor de busqueda
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)) && ped.CAPESERI.Contains(serie))
        //                        && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin)).OrderByDescending(ped => ped.CAPEIDES).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                }
        //                else if (estado != 0) //Busqueda por estado
        //                {
        //                    if (serie.Equals("") && valbus.Equals("")) // busca solo por el estado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                                .OrderBy(ped => ped.CAPEIDES).OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie != "" && valbus.Equals("")) //busca solo por la serie con el estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == estado && ped.CAPESERI.Contains(serie)) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                                .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie.Equals("") && valbus != "") // busca solo por el valor de busqueda con el estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                                .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                    else if (serie != "" && valbus != "") // busca por serie, valor de busqueda y estado indicado
        //                    {
        //                        listaeo = context.PECAPE.Include("TCLIE").Where(ped => ((ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSCR.Contains(valbus) || ped.CAPENUME == nume) && (ped.CAPESERI.Contains(serie)) && ped.CAPEIDES == estado) && (ped.CAPEFECH >= feini && ped.CAPEFECH <= fefin))
        //                                .OrderBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFECH).ToList<object>();
        //                    }
        //                }
        //            }
        //            lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //            vpar.VALSAL = new List<string>();
        //            if (lista.Count > 0)
        //            {
        //                vpar.VALSAL.Add("1");
        //                vpar.VALSAL.Add(Util.Serialize(lista));
        //            }
        //            else
        //            {
        //                vpar.VALSAL.Add("0");

        //            }
        //            vpar.ESTOPE = true;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);

        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//ya esta

        //public RESOPE Buscasolicitud(PAROPE paramOperacion)
        //{
        //    //filtro de las solicitudes de aprobacion
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        string valbus;
        //        decimal nume = 0;

        //        valbus = paramOperacion.VALENT[0];
        //        // estado = decimal.Parse(paramOperacion.VALENT[1]);
        //        Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
        //        Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.


        //        bool num = regnum.IsMatch(valbus); //que tenga numeros
        //        bool letra = reglet.IsMatch(valbus); //que tenga letras


        //        if (num && !letra)//se verifica que tenga numeros pero ninguna letra
        //        {
        //            nume = decimal.Parse(paramOperacion.VALENT[0]);
        //        }

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (valbus == "") //Supone mostrar todos
        //            {
        //                listaeo = context.PECAPE.Include("TCLIE").Where(ped => ped.CAPEIDES == 4).ToList<object>();
        //            }
        //            else
        //            {
        //                //Orden para filtrar??
        //                listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.TCLIE.CLINOM.Contains(valbus) || ped.CAPEUSMO.Contains(valbus) || ped.CAPENUME == nume || ped.CAPESERI.Contains(valbus) || ped.CAPEUSFP.Contains(valbus)) && ped.CAPEIDES == 4).ToList<object>();
        //            }
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);

        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//ya esta

        //public RESOPE CambiaEstaList(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PECAPE> lista = null;
        //    List<EFModelo.PEPARM> listapar = new List<EFModelo.PEPARM>();
        //    EFModelo.PEPARM par = new EFModelo.PEPARM();
        //    appLogica.appDB2 _appDB2 = null;
        //    try
        //    {
        //        decimal estado = 0;
        //        decimal bultos = 0;
        //        decimal tade = 0;
        //        lista = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
        //        estado = decimal.Parse(paramOperacion.VALENT[1]);
        //        string usuario = paramOperacion.VALENT[2];
        //        if (estado == 4)
        //        {
        //            bultos = Convert.ToDecimal(paramOperacion.VALENT[3]);
        //            tade = Convert.ToDecimal(paramOperacion.VALENT[4]);
        //        }

        //        string mensajecorreo;
        //        foreach (var item in lista)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var ped = context.PECAPE.Find(item.CAPEIDCP);
        //                ped.CAPEIDES = estado;
        //                switch (estado.ToString())
        //                {
        //                    case "2": //Emitido
        //                        ped.CAPEUSEM = usuario;
        //                        ped.CAPEFHEM = DateTime.Now;
        //                        break;
        //                    case "3": //En preparacion
        //                        ped.CAPEUSIP = usuario;
        //                        ped.CAPEFHIP = DateTime.Now;
        //                        break;
        //                    case "4": //En espera de aprobacion
        //                        if (ped.CAPEUSAP != null) //Si viene de reabrir completado almacen
        //                        {
        //                            ped.CAPEUSMO = usuario;
        //                            ped.CAPEFEMO = DateTime.Now;
        //                            ped.CAPEUSAP = null;
        //                            ped.CAPEFEAP = null;
        //                        }
        //                        else
        //                        {
        //                            ped.CAPEUSFP = usuario;
        //                            ped.CAPEFHFP = DateTime.Now;
        //                            ped.CAPENUBU = bultos;
        //                            ped.CAPETADE = tade;
        //                        }
        //                        break;

        //                    case "5":
        //                        ped.CAPEUSAP = usuario;
        //                        ped.CAPEFEAP = DateTime.Now;
        //                        break;
        //                    case "9":
        //                        ped.CAPEUSMO = usuario;
        //                        ped.CAPEFEMO = DateTime.Now;
        //                        anularReservasPedidoAnulado(context, item);
        //                        break;
        //                }
        //                context.SaveChanges();
        //                listapar = context.PEPARM.ToList();
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista)); //revisar
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;
        //        //Envio de correo de acuerdo a la tabla de parametros
        //        if (estado != 1 && estado != 9)
        //        {
        //            switch (estado.ToString())
        //            {
        //                case "2": //Emitidos"
        //                    par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_EMITIR_PEDIDO);
        //                    break;
        //                case "3": //En preparacion
        //                    par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_INICIAR_PREPARACION_PEDIDO);
        //                    break;
        //                case "4": //Fin Preparacion
        //                    par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_FINALIZAR_PREPARACION_PEDIDO);
        //                    break;
        //                case "5": //Despachado
        //                    par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_DESPACHAR_PEDIDO);
        //                    break;
        //            }

        //            if (par != null && par.PARMVAPA.Equals("1"))
        //            {
        //                foreach (var item in lista)
        //                {
        //                    if (!_appDB2.EnviaCorreoNotificacionPedido(item.CAPEIDCP, out mensajecorreo))
        //                    {
        //                        Util.EscribeLog(mensajecorreo);
        //                    }
        //                }
        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE GuardaDetalle(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;

        //    vpar = new RESOPE() { ESTOPE = false };
        //    appWcfService.PECAPE pedido;
        //    List<appWcfService.PEDEPE> lista = null;
        //    try
        //    {
        //        pedido = Util.Deserialize<appWcfService.PECAPE>(paramOperacion.VALENT[0]);
        //        lista = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[1]);
        //        //articulo = String.IsNullOrWhiteSpace(partida) ? null : articulo;
        //        using (var context = new PEDIDOSEntities())
        //        {

        //            foreach (var item in lista)
        //            {
        //                var det = context.PEDEPE.Find(item.DEPEIDDP);

        //                det.DEPEFEMO = DateTime.Now;
        //                det.DEPEUSMO = pedido.CAPEUSCR; //EL MISMO USUARIO
        //                det.DEPEIDCP = pedido.CAPEIDCP;
        //                det.DEPEALMA = item.DEPEALMA;
        //                det.DEPECAAT = item.DEPECAAT;
        //                det.DEPEPEAT = item.DEPEPEAT;
        //                det.DEPECOAR = item.DEPECOAR;
        //                det.DEPEPART = item.DEPEPART;
        //                det.DEPESTOC = item.DEPESTOC;
        //                context.SaveChanges();
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }

        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE MuestraUbicacionesArticulo(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_UBICACIONES_Result> lista = null;
        //    try
        //    {
        //        string articulo, partida;
        //        decimal almacen;
        //        articulo = paramOperacion.VALENT[0];
        //        partida = paramOperacion.VALENT[1];
        //        almacen = decimal.Parse(paramOperacion.VALENT[2]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_UBICACIONES(articulo, partida, almacen).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_UBICACIONES_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}// YA ESTA USP_OBTIENE_UBICACIONES

        //public RESOPE ObtieneBolsa(PAROPE paramOperacion)//YA ESTA USP_OBTIENE_BOLSA
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_BOLSA_Result> lista = null;
        //    try
        //    {
        //        decimal iddetalle = decimal.Parse(paramOperacion.VALENT[0]);
        //        string empaque = paramOperacion.VALENT[1];

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_BOLSA(iddetalle, empaque).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_BOLSA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE ObtieneDetallePreparacion(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> lista = null;
        //    try
        //    {
        //        decimal iddetallepedido = decimal.Parse(paramOperacion.VALENT[0]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE(iddetallepedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE guardaPreparacionBolsa(PAROPE paramOperacion)//appWcfService.PEBODP detallebolsa)
        //{
        //    ////
        //    appWcfService.PEBODP detallebolsa;
        //    ////

        //    Nullable<decimal> iddetpedidostoc = null;

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    string resultado = "";
        //    string partida, articulo;
        //    try
        //    {
        //        ///
        //        detallebolsa = Util.Deserialize<appWcfService.PEBODP>(paramOperacion.VALENT[0]);
        //        ///

        //        partida = articulo = "";
        //        //if (detallebolsa == null)
        //        //{
        //        //    Util.EscribeLog("detallebolsa es null");
        //        //}
        //        //else
        //        //{
        //        //    Util.EscribeLog("detallebolsa " + detallebolsa.BODPIDDP.ToString() + " " + detallebolsa.BODPIDDP.ToString());
        //        //}

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            EFModelo.PEDEPE detpednac = null;
        //            EFModelo.PEDEOS detpedint = null;
        //            //inserta PBOLS si no existe
        //            //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
        //            var emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
        //            if (emp != null)
        //            {
        //                var bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);
        //                if (bol == null) //no existe pbols, insertar
        //                {
        //                    bol = new EFModelo.PEBOLS();
        //                    //inserta = true;
        //                    bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
        //                    bol.BOLSUSCR = detallebolsa.BODPUSCR;
        //                    bol.BOLSFECR = DateTime.Now;
        //                    bol.BOLSCOCA = null;
        //                    bol.BOLSESTA = 1;
        //                    bol.BOLSALMA = emp.CAEMALMA;
        //                    bol.BOLSCOAR = "";

        //                    context.PEBOLS.Add(bol);
        //                    context.SaveChanges();
        //                    detallebolsa.BODPIDBO = bol.BOLSIDBO;
        //                }
        //                else
        //                {
        //                    if (bol.BOLSALMA == 0)
        //                    {
        //                        bol.BOLSALMA = emp.CAEMALMA;
        //                    }
        //                }
        //                if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
        //                {
        //                    //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
        //                }
        //                //bool inserta = false;
        //                //
        //                if (detallebolsa.BODPIDDP.HasValue)
        //                {
        //                    detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
        //                    partida = detpednac.DEPEPART;
        //                    articulo = detpednac.DEPECOAR;
        //                }
        //                else if (detallebolsa.BODPIDDO.HasValue)
        //                {
        //                    detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
        //                    partida = detpedint.DEOSPART;
        //                    articulo = detpedint.DEOSCOAR;
        //                }

        //                var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
        //                if (ent == null) //detallebolsa.BODPIDDE != 0)
        //                {
        //                    ent = new EFModelo.PEBODP();
        //                    //inserta = true;
        //                    ent.BODPUSCR = detallebolsa.BODPUSCR;
        //                    ent.BODPFECR = DateTime.Now;
        //                    context.PEBODP.Add(ent);
        //                    //inserta tipo 1 SALIDA
        //                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                }
        //                else
        //                {
        //                    ent.BODPUSMO = detallebolsa.BODPUSCR;
        //                    ent.BODPFEMO = DateTime.Now;
        //                    //SOLO si las cantidades o pesos son diferentes
        //                    //inserta tipo 3 reingreso
        //                    //inserta tipo 1 salida
        //                    if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
        //                    {
        //                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                    }
        //                }
        //                //automatizar el parse sin incluir la PK
        //                ent.BODPIDBO = detallebolsa.BODPIDBO;
        //                ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
        //                ent.BODPALMA = bol.BOLSALMA;
        //                ent.BODPPART = partida;
        //                ent.BODPCOAR = articulo;
        //                ent.BODPCANT = detallebolsa.BODPCANT;
        //                ent.BODPPESO = detallebolsa.BODPPESO;
        //                ent.BODPPERE = detallebolsa.BODPPERE;
        //                ent.BODPDIFE = detallebolsa.BODPDIFE;
        //                ent.BODPSTCE = detallebolsa.BODPSTCE;
        //                ent.BODPINBO = detallebolsa.BODPINBO;
        //                ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
        //                                                      //iddetpedido = ent.BODPIDDP;
        //                                                      //iddetpedidoint = ent.BODPIDDO;
        //                ent.BODPTAUN = detallebolsa.BODPTAUN;

        //                if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
        //                {
        //                    iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
        //                }

        //                ent.BODPTADE = detallebolsa.BODPTADE;
        //                ent.BODPPEBR = detallebolsa.BODPPEBR;

        //                context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
        //                decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                if (detallebolsa.BODPIDDP.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
        //                    // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    //var detpednac = context.PEDEPE.Find(iddetpedido);
        //                    detpednac.DEPECAAT = cantatendida;
        //                    detpednac.DEPEPEAT = pesoatendido;
        //                    detpednac.DEPEPERE = pesoreal;
        //                    if (iddetpedidostoc.HasValue)
        //                    {
        //                        detpednac.DEPESTOC = iddetpedidostoc;
        //                    }
        //                    detpednac.DEPETADE = tade;
        //                    detpednac.DEPEPEBR = pebr;

        //                    context.SaveChanges();
        //                }
        //                else if (detallebolsa.BODPIDDO.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
        //                    detpedint.DEOSCAAT = cantatendida;
        //                    detpedint.DEOSPEAT = pesoatendido;
        //                    detpedint.DEOSPERE = pesoreal;
        //                    if (iddetpedidostoc.HasValue)
        //                    {
        //                        detpedint.DEOSSTOC = iddetpedidostoc;
        //                    }
        //                    detpedint.DEOSTADE = tade;
        //                    detpedint.DEOSPEBR = pebr;
        //                    //actualiza peso en osa ---PENDIENTE EN AS
        //                    var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
        //                    if (osa != null)
        //                    {
        //                        osa.OSASCAEN = pesoatendido;
        //                        actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //ASPROSAS validar si es correcto
        //                    }
        //                    context.SaveChanges();
        //                }
        //                //actualiza el stock de la bolsa
        //                var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
        //                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                foreach (var bolsaprep in todasbolsasprep)
        //                {
        //                    cantatendida += bolsaprep.BODPCANT;
        //                    pesoatendido += bolsaprep.BODPPESO;
        //                    pesoreal += bolsaprep.BODPPERE;
        //                    tade += bolsaprep.BODPTADE;
        //                    pebr += bolsaprep.BODPPEBR;
        //                }
        //                var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
        //                if (detemp != null)
        //                {
        //                    detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
        //                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
        //                    if (iddetpedidostoc.HasValue)
        //                    {
        //                        detemp.DEEMSTCE = iddetpedidostoc.Value;
        //                    }
        //                    if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
        //                    {
        //                        detemp.DEEMESBO = 9;
        //                    }
        //                    else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
        //                    {
        //                        detemp.DEEMESBO = 9;
        //                    }
        //                    else
        //                    {
        //                        detemp.DEEMESBO = 1;
        //                    }
        //                    actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); // Validar AS 
        //                    context.SaveChanges();
        //                    //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
        //                    detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
        //                    if (detemp == null)
        //                    {
        //                        bol.BOLSESTA = 9; //ya no se usa la bolsa
        //                    }
        //                    else
        //                    {
        //                        bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
        //                    }
        //                    context.SaveChanges();
        //                }
        //                vpar.VALSAL = new List<string>();
        //                vpar.VALSAL.Add(ent.BODPIDDE.ToString());
        //                vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
        //                resultado = ""; // ent.BODPIDDE.ToString();
        //                vpar.ESTOPE = true;
        //            }
        //            else
        //            {
        //                resultado = "Código de empaque incorrecto";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        resultado = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    vpar.MENERR = resultado;
        //    return vpar;
        //}

        //private void insertaMovimientoKardex(PEDIDOSEntities context, decimal? idbolsa, decimal idtipomovimiento, decimal almacen, string partida, string articulo, decimal cantidad, decimal peso, decimal pesobr, string usuario, Nullable<decimal> iddetpedido, Nullable<decimal> iddetosa)
        //{
        //    decimal valorsigno = 1;
        //    if (idtipomovimiento == TIPO_MOV_SALIDA_PREP_PED)
        //    {
        //        valorsigno = -1;
        //    }
        //    var entk = new EFModelo.PEKABO();
        //    entk.KABOIDBO = idbolsa.Value;
        //    entk.KABOIDTM = idtipomovimiento;
        //    entk.KABOALMA = almacen;
        //    entk.KABOPART = partida;
        //    entk.KABOITEM = articulo;
        //    entk.KABOCANT = cantidad * valorsigno;
        //    entk.KABOPESO = peso * valorsigno;
        //    entk.KABOPEBR = pesobr * valorsigno;
        //    entk.KABOTARA = entk.KABOPEBR - entk.KABOPESO;
        //    entk.KABOFECH = DateTime.Today;
        //    entk.KABOUSCR = usuario;
        //    entk.KABOFECR = DateTime.Now;
        //    entk.KABOIDDP = iddetpedido;
        //    entk.KABOIDDO = iddetosa;
        //    context.PEKABO.Add(entk);
        //}

        //public RESOPE remueveBolsaPedido(PAROPE paramOperacion)//decimal idbolsapedido, string usuario)
        //{
        //    Nullable<decimal> iddetpedido;
        //    Nullable<decimal> iddetpedidoint;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    string resultado = "";
        //    string partida, articulo;
        //    try
        //    {
        //        decimal idbolsapedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[1];

        //        partida = articulo = "";
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            EFModelo.PEDEPE detpednac = null;
        //            EFModelo.PEDEOS detpedint = null;

        //            //bool inserta = false;
        //            //foreach (var item in collection)
        //            //{

        //            //}
        //            var ent = context.PEBODP.Find(idbolsapedido);
        //            if (ent != null) //detallebolsa.BODPIDDE != 0)
        //            {
        //                var bol = context.PEBOLS.Find(ent.BODPIDBO);
        //                if (ent.BODPIDDP.HasValue)
        //                {
        //                    detpednac = context.PEDEPE.Find(ent.BODPIDDP);
        //                    partida = detpednac.DEPEPART;
        //                    articulo = detpednac.DEPECOAR;
        //                }
        //                else if (ent.BODPIDDO.HasValue)
        //                {
        //                    detpedint = context.PEDEOS.Find(ent.BODPIDDO);
        //                    partida = detpedint.DEOSPART;
        //                    articulo = detpedint.DEOSCOAR;
        //                }
        //                iddetpedido = ent.BODPIDDP;
        //                iddetpedidoint = ent.BODPIDDO;
        //                //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
        //                //{
        //                //bol.BOLSESTA = 1; //ya no se usa
        //                //}
        //                insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
        //                context.PEBODP.Remove(ent);
        //                context.SaveChanges();
        //                decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

        //                if (iddetpedido.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    //detpednac = context.PEDEPE.Find(iddetpedido);
        //                    detpednac.DEPECAAT = cantatendida;
        //                    detpednac.DEPEPEAT = pesoatendido;
        //                    detpednac.DEPEPERE = pesoreal;
        //                    detpednac.DEPETADE = tade;
        //                    detpednac.DEPEPEBR = pebr;

        //                    //inserta tipo 2 kardex reingreso
        //                    context.SaveChanges();
        //                }
        //                else if (iddetpedidoint.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    detpedint.DEOSCAAT = cantatendida;
        //                    detpedint.DEOSPEAT = pesoatendido;
        //                    detpedint.DEOSPERE = pesoreal;
        //                    detpedint.DEOSTADE = tade;
        //                    detpedint.DEOSPEBR = pebr;
        //                    //actualiza peso en osa ---PENDIENTE EN AS
        //                    var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
        //                    if (osa != null)
        //                    {
        //                        osa.OSASCAEN = pesoatendido;
        //                        actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //validar si es correcto
        //                    }
        //                    context.SaveChanges();

        //                }
        //                //actualiza el stock de la bolsa
        //                var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
        //                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                foreach (var bolsaprep in todasbolsasprep)
        //                {
        //                    cantatendida += bolsaprep.BODPCANT;
        //                    pesoatendido += bolsaprep.BODPPESO;
        //                    pesoreal += bolsaprep.BODPPERE;
        //                    tade += bolsaprep.BODPTADE;
        //                    pebr += bolsaprep.BODPPEBR;
        //                }
        //                var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
        //                if (detemp != null)
        //                {
        //                    detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
        //                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
        //                    //si la bolsa preparada estaba marcada como stock cero, lo desmarco
        //                    if (ent.BODPSTCE == 1)
        //                    {
        //                        detemp.DEEMSTCE = 0;
        //                    }
        //                    if (detemp.DEEMSTCE == 0)
        //                    {
        //                        detemp.DEEMESBO = 1;
        //                    }
        //                    else
        //                    if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
        //                    {
        //                        detemp.DEEMESBO = 9;
        //                    }
        //                    else
        //                    {
        //                        detemp.DEEMESBO = 1;
        //                    }
        //                    actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); // Validar AS 
        //                    context.SaveChanges();
        //                    //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
        //                    detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
        //                    if (detemp == null)
        //                    {
        //                        bol.BOLSESTA = 9; //ya no se usa la bolsa
        //                    }
        //                    else
        //                    {
        //                        bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
        //                    }
        //                    context.SaveChanges();
        //                }
        //            }
        //            vpar.ESTOPE = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        resultado = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    vpar.MENERR = resultado;
        //    return vpar;
        //}


        //public RESOPE MuestraPasillos(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PEPASI> lista = null;
        //    try
        //    {
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PEPASI.ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PEPASI>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE MuestraNiveles(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PENIVE> lista = null;
        //    try
        //    {
        //        decimal idpasillo;
        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PENIVE.Where(pas => pas.NIVEIDPA == idpasillo).ToList<object>();
        //        }

        //        lista = Util.ParseEntityObject<appWcfService.PENIVE>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE MuestraColumnas(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECOLU> lista = null;
        //    try
        //    {
        //        decimal idpasillo;
        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PECOLU.Where(colu => colu.COLUIDPA == idpasillo).ToList<object>();
        //        }

        //        lista = Util.ParseEntityObject<appWcfService.PECOLU>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE MuestraCasilleros(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PECASI> lista = null;
        //    try
        //    {
        //        decimal idpasillo, idcolumna;
        //        string idnivel;

        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
        //        idcolumna = decimal.Parse(paramOperacion.VALENT[1]);
        //        idnivel = paramOperacion.VALENT[2];

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PECASI.Where(casi => casi.CASIIDPA == idpasillo && casi.CASIIDCO == idcolumna && casi.CASIIDNI.Equals(idnivel) && casi.CASIESTA == 1).ToList<object>();
        //        }

        //        lista = Util.ParseEntityObject<appWcfService.PECASI>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE GeneraPreguia(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<EFModelo.PEPARM> listapar = null; //Revisar si es APPWCFSERVICE O EFMODELO
        //    EFModelo.PEPARM par; //Revisar si es APPWCFSERVICE O EFMODELO
        //    List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
        //    appLogica.appDB2 _appDB2 = null;
        //    string codprovtrans, estabpart, serieguiadefault;
        //    string mensajecorreo;
        //    decimal tped = 0; //TIPO DE PEDIDO PARA LA GUIA
        //    decimal nubu = 0;// numero de bultos;
        //    try
        //    {
        //        decimal idpedido;
        //        int secuemcia;
        //        string usuario;
        //        idpedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        usuario = paramOperacion.VALENT[1];
        //        tped = decimal.Parse(paramOperacion.VALENT[2]);
        //        nubu = decimal.Parse(paramOperacion.VALENT[3]);


        //        _appDB2 = new appLogica.appDB2();

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
        //            secuemcia = 0;
        //            foreach (EFModelo.USP_OBTIENE_PEDIDO_CONSULTA_Result item in listaeo)
        //            {
        //                String descripcionitem;
        //                var detPed = context.PEDEPE.Find(item.DEPEIDDP);
        //                secuemcia++;
        //                descripcionitem = _appDB2.descripcionItem(false, item.DEPECOAR, "", item.DEPECONT, "N", Convert.ToString(secuemcia), Convert.ToInt32(item.DEPECAAT).ToString(), "3");
        //                item.DEPEDSAR = descripcionitem;
        //                detPed.DEPEDSAR = descripcionitem;
        //            }

        //            listapar = context.PEPARM.ToList();
        //            context.SaveChanges();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();

        //        //codigo incalpaca
        //        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_INCALPACA);
        //        if (lista[0].CAPEIDCL == par.PARMVAPA.Trim()) //es incalpaca
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_INCALPACA);
        //            codprovtrans = par.PARMVAPA.Trim();
        //        }
        //        else
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_OTROS);
        //            codprovtrans = par.PARMVAPA.Trim();
        //        }
        //        //establecimiento partida
        //        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_ESTAB_PARTIDA);
        //        estabpart = par.PARMVAPA.Trim();

        //        //Serie guia default
        //        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_GUIA_DEFAULT);
        //        serieguiadefault = par.PARMVAPA.Trim();

        //        vpar = _appDB2.GeneraPreguia(lista, serieguiadefault, usuario, codprovtrans, estabpart);

        //        if (vpar.ESTOPE == true)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var ped = context.PECAPE.Find(idpedido);
        //                ped.CAPEIDES = 5;
        //                ped.CAPEUSAP = usuario;
        //                ped.CAPEFEAP = DateTime.Now;
        //                ped.CAPENUBU = nubu;
        //                ped.CAPETIPO = tped;
        //                context.SaveChanges();
        //                par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_DESPACHAR_PEDIDO);
        //                if (par != null && par.PARMVAPA.Equals("1"))
        //                {
        //                    if (!EnviaCorreoNotificacionPedido(idpedido, out mensajecorreo))
        //                    {
        //                        Util.EscribeLog(mensajecorreo);
        //                    }
        //                }

        //            }
        //        }

        //        //if (lista.Count > 0)
        //        //{
        //        //    vpar.VALSAL.Add("1");
        //        //    //vpar.VALSAL.Add(Util.Serialize(lista));
        //        //}
        //        //else
        //        //{
        //        //    vpar.VALSAL.Add("0");

        //        //}
        //        //vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //        if (_appDB2 != null)
        //        {
        //            _appDB2.Finaliza();
        //            _appDB2 = null;
        //        }
        //    }
        //    return vpar;
        //}
        //public RESOPE GeneraPreguia(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<EFModelo.PEPARM> listapar = null; //Revisar si es APPWCFSERVICE O EFMODELO
        //    EFModelo.PEPARM par; //Revisar si es APPWCFSERVICE O EFMODELO
        //    List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
        //    appLogica.appDB2 _appDB2 = null;
        //    string codprovtrans, estabpart, serieguiadefault;
        //    string mensajecorreo;
        //    decimal tped = 0; //TIPO DE PEDIDO PARA LA GUIA
        //    decimal nubu = 0;// numero de bultos;
        //    decimal tade = 0;
        //    decimal estadest = 0;
        //    try
        //    {
        //        decimal idpedido, idtipdoc;
        //        int secuemcia;
        //        string usuario, usuariopedido;
        //        idpedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        usuario = paramOperacion.VALENT[1];
        //        tped = decimal.Parse(paramOperacion.VALENT[2]);
        //        nubu = Convert.ToDecimal(paramOperacion.VALENT[3]);
        //        tade = Convert.ToDecimal(paramOperacion.VALENT[4]);
        //        idtipdoc = 0;
        //        if (paramOperacion.VALENT.Count > 5)
        //        {
        //            idtipdoc = Convert.ToDecimal(paramOperacion.VALENT[5]); //20180418
        //        }

        //        usuariopedido = "";
        //        _appDB2 = new appLogica.appDB2();

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
        //            secuemcia = 0;
        //            foreach (EFModelo.USP_OBTIENE_PEDIDO_CONSULTA_Result item in listaeo)
        //            {
        //                String descripcionitem;
        //                var detPed = context.PEDEPE.Find(item.DEPEIDDP);
        //                secuemcia++;
        //                descripcionitem = _appDB2.descripcionItem(false, item.DEPECOAR, "", item.DEPECONT, "N", Convert.ToString(secuemcia), Convert.ToInt32(item.DEPECAAT).ToString(), "3");
        //                item.DEPEDSAR = descripcionitem;
        //                detPed.DEPEDSAR = descripcionitem;
        //                //20180220
        //                item.CAPENUBU = nubu;
        //                item.CAPETIPO = tped;
        //                item.CAPETADE = tade;
        //                if (idtipdoc != 0)
        //                {
        //                    item.CAPEIDTD = idtipdoc; //20180418
        //                }
        //                else
        //                {
        //                    idtipdoc = item.CAPEIDTD;
        //                }
        //                usuariopedido = item.CAPEUSCR.Trim();
        //            }

        //            listapar = context.PEPARM.ToList();
        //            context.SaveChanges();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();

        //        //codigo incalpaca
        //        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_INCALPACA);
        //        if (lista[0].CAPEIDCL == par.PARMVAPA.Trim()) //es incalpaca
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_INCALPACA);
        //            codprovtrans = par.PARMVAPA.Trim();
        //        }
        //        else
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_TRANS_OTROS);
        //            codprovtrans = par.PARMVAPA.Trim();
        //        }
        //        //establecimiento partida
        //        par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_COD_ESTAB_PARTIDA);
        //        estabpart = par.PARMVAPA.Trim();

        //        //VENTA = 1; //GR id tipo 3, motivo 35
        //        //TRANSF_ALMACENES = 2; //GR id tipo 3, motivo 36, almacen destino Lima 33
        //        //TRANSF_INTERNA = 3; //T/I id tipo 11, motivo 36, almacen destino tienda 35
        //        //REMATES = 4; //T/I id tipo 11, motivo 36, almacen destino remates 75
        //        //Serie guia default
        //        if (tped == Constantes.VENTA || tped== Constantes.CONSIGNACION) //20180418
        //        {
        //            //20181123
        //            if (idtipdoc != Constantes.ID_TIPO_DOC_GUIA && idtipdoc != Constantes.ID_TIPO_DOC_NE)
        //            {
        //                idtipdoc = Constantes.ID_TIPO_DOC_GUIA;
        //            }
        //            if (idtipdoc == Constantes.ID_TIPO_DOC_GUIA)
        //            {
        //                par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_GUIA_DEFAULT);
        //            }
        //            else
        //            {
        //                par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_NOTENTR_DEFAULT);
        //            }
        //        }
        //        else if (tped == Constantes.TRANSF_ALMACENES)
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_GUIA_DEFAULT);
        //        }
        //        else
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_SERIE_TI_DEFAULT);
        //        }
        //        serieguiadefault = par.PARMVAPA.Trim();

        //        //establ destino buscar parametro por tipo pedido
        //        if (tped == Constantes.TRANSF_ALMACENES)
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_ESTABLEC);
        //            estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
        //        }
        //        else if (tped == Constantes.TRANSF_INTERNA)
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_INTERNO);
        //            estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
        //        }
        //        else if (tped == Constantes.REMATES)
        //        {
        //            par = listapar.Find(x => x.PARMIDPA == Constantes.ID_PAR_ESTADEST_TRASLADO_REMATE);
        //            estadest = Convert.ToDecimal(par.PARMVAPA.Trim());
        //        }
        //        //20180411
        //        if (string.IsNullOrWhiteSpace(usuariopedido))
        //        {
        //            usuariopedido = usuario;
        //        }
        //        vpar = _appDB2.GeneraPreguia(lista, serieguiadefault, usuariopedido.Trim(), codprovtrans, estabpart, estadest);

        //        if (vpar.ESTOPE == true)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var ped = context.PECAPE.Find(idpedido);
        //                ped.CAPEIDES = 5;
        //                ped.CAPEUSAP = usuario;
        //                ped.CAPEFEAP = DateTime.Now;
        //                ped.CAPENUBU = nubu;
        //                ped.CAPETADE = tade;
        //                ped.CAPETIPO = tped;
        //                ped.CAPEIDTD = idtipdoc; //20180418
        //                //20180220
        //                ped.CAPEDOCO = Convert.ToDecimal(vpar.VALSAL[0]);
        //                context.SaveChanges();
        //                if (tped == Constantes.VENTA)
        //                {
        //                    par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_DESPACHAR_PEDIDO);
        //                    if (par != null && par.PARMVAPA.Equals("1"))
        //                    {
        //                        if (!_appDB2.EnviaCorreoNotificacionPedido(idpedido, out mensajecorreo))
        //                        {
        //                            Util.EscribeLog(mensajecorreo);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //if (lista.Count > 0)
        //        //{
        //        //    vpar.VALSAL.Add("1");
        //        //    //vpar.VALSAL.Add(Util.Serialize(lista));
        //        //}
        //        //else
        //        //{
        //        //    vpar.VALSAL.Add("0");

        //        //}
        //        //vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //        if (_appDB2 != null)
        //        {
        //            _appDB2.Finaliza();
        //            _appDB2 = null;
        //        }
        //    }
        //    return vpar;
        //}
        //public RESOPE GuardaPrioridad(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        lista = Util.Deserialize<List<appWcfService.PECAPE>>(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[1];

        //        foreach (var item in lista)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var ped = context.PECAPE.Find(item.CAPEIDCP);

        //                ped.CAPEPRIO = item.CAPEPRIO;
        //                if (item.CAPEEPRI == 1 && (item.CAPEUSPR == null || usuario == item.CAPEUSEM))
        //                {
        //                    ped.CAPEEPRI = item.CAPEEPRI;
        //                    ped.CAPEFEPR = DateTime.Now;
        //                    ped.CAPEUSPR = usuario;
        //                }


        //                context.SaveChanges();
        //            }
        //        }
        //        //vpar.VALSAL = new List<string>();
        //        //if (lista.Count > 0)
        //        //{
        //        //    vpar.VALSAL.Add("1");
        //        //    vpar.VALSAL.Add(Util.Serialize(lista));
        //        //}
        //        //else
        //        //{
        //        //    vpar.VALSAL.Add("0");

        //        //}
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}


        //public RESOPE AgregaPasi(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    //appWcfService.PEPASI column = null;
        //    try
        //    {
        //        //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[0].Trim();
        //        decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var det = context.PEPASI.Find(pasillo + 1);
        //            if (det == null)
        //            {
        //                det = new EFModelo.PEPASI();
        //                det.PASIIDPA = pasillo + 1;
        //                det.PASIFECR = DateTime.Now;
        //                det.PASIUSCR = usuario;
        //                det.PASIESTA = 1;
        //                context.PEPASI.Add(det);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE AgregaNivel(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    //appWcfService.PEPASI column = null;
        //    try
        //    {
        //        //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[0].Trim();
        //        decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
        //        string nivel = paramOperacion.VALENT[2];

        //        if (nivel.Equals(""))
        //        {
        //            nivel = "@";
        //        }

        //        int num;
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            //var nivel = context.PENIVE.Find();
        //            //var ped = context.PENIVE.Max(x => x.NIVEIDNI);
        //            num = Encoding.ASCII.GetBytes(nivel)[0];
        //            num++;
        //            var det = context.PENIVE.Find((num).ToString(), pasillo);
        //            byte[] bytes2 = BitConverter.GetBytes(num);
        //            string a = Encoding.ASCII.GetString(bytes2);
        //            if (det == null)
        //            {
        //                det = new EFModelo.PENIVE();
        //                det.NIVEIDNI = (a[0]).ToString();
        //                det.NIVEIDPA = pasillo;
        //                det.NIVEESTA = 1;
        //                det.NIVEUSCR = usuario;
        //                det.NIVEFECR = DateTime.Now;
        //                context.PENIVE.Add(det);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE AgregaColumna(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    //appWcfService.PEPASI column = null;
        //    try
        //    {
        //        //column = Util.Deserialize<appWcfService.PEPASI>(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[0].Trim();
        //        decimal pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
        //        decimal columna = Decimal.Parse(paramOperacion.VALENT[2]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var det = context.PECOLU.Find(columna + 1, pasillo);

        //            if (det == null)
        //            {
        //                det = new EFModelo.PECOLU();
        //                det.COLUIDCO = columna + 1;
        //                det.COLUIDPA = pasillo;
        //                det.COLUESTA = 1;
        //                det.COLUUSCR = usuario;
        //                det.COLUFECR = DateTime.Now;
        //                context.PECOLU.Add(det);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //Reportes
        //public RESOPE ObtienePedidoConsulta(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
        //    try
        //    {
        //        decimal idpedido;
        //        idpedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//listo USP_OBTIENE_PEDIDO_CONSULTA

        //public RESOPE ReportePartidaAlmacen(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result> lista = null;
        //    try
        //    {
        //        string partida;
        //        decimal almacen;
        //        partida = paramOperacion.VALENT[0];
        //        almacen = decimal.Parse(paramOperacion.VALENT[1]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_REPORTE_EMPAQUES_PARTIDA(partida, almacen).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}// LISTO USP_REPORTE_EMPAQUES_PARTIDA

        //public RESOPE ReporteMovimientosPartida(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result> lista = null;
        //    try
        //    {
        //        string partida;
        //        decimal almacen;
        //        partida = paramOperacion.VALENT[0];
        //        almacen = decimal.Parse(paramOperacion.VALENT[1]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_REPORTE_MOVIMIENTOS_PARTIDA(partida, almacen).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");

        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//YA ESTA
        //public RESOPE ReporteMovimientosFechas(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result> lista = null;
        //    DateTime feini;
        //    DateTime fefin;
        //    try
        //    {
        //        feini = DateTime.Parse(paramOperacion.VALENT[0].ToString());
        //        fefin = DateTime.Parse(paramOperacion.VALENT[1].ToString());
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_REPORTE_MOVIMIENTOS_FECHAS(feini, fefin).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_REPORTE_MOVIMIENTOS_FECHAS_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1");
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");
        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//YA ESTA
        //public RESOPE AgregaCasillero(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    decimal idpasillo, idcolumna;
        //    string idnivel, usuario;

        //    //List<object> listaeo = null;
        //    try
        //    {
        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
        //        idnivel = paramOperacion.VALENT[1];
        //        idcolumna = decimal.Parse(paramOperacion.VALENT[2]);
        //        usuario = paramOperacion.VALENT[3].Trim();
        //        string codcas = ("P" + idpasillo.ToString().PadLeft(2, '0') + idnivel + idcolumna.ToString().PadLeft(2, '0')).Trim();

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var casillero = context.PECASI.Find(codcas);
        //            if (casillero != null)
        //            {
        //                if (casillero.CASIESTA == 0)
        //                {
        //                    casillero.CASIESTA = 1;
        //                    casillero.CASIUSCR = usuario;
        //                    casillero.CASIFECR = DateTime.Now;
        //                    casillero.CASICAPA = 0;
        //                    casillero.CASIALTU = 0;
        //                    casillero.CASIANCH = 0;
        //                    casillero.CASILARG = 0;
        //                }
        //                else
        //                {
        //                    vpar.ESTOPE = false;
        //                    goto Guardacambios;
        //                }
        //            }
        //            else
        //            {
        //                casillero = new EFModelo.PECASI();
        //                casillero.CASICOCA = codcas;
        //                casillero.CASIIDPA = idpasillo;
        //                casillero.CASIIDNI = idnivel;
        //                casillero.CASIIDCO = idcolumna;
        //                casillero.CASIUSCR = usuario;
        //                casillero.CASIFECR = DateTime.Now;
        //                casillero.CASIESTA = 1;
        //                casillero.CASICAPA = 0;
        //                casillero.CASIALTU = 0;
        //                casillero.CASIANCH = 0;
        //                casillero.CASILARG = 0;
        //                context.PECASI.Add(casillero);
        //            }
        //            vpar.ESTOPE = true;
        //            Guardacambios:
        //            context.SaveChanges();

        //        }
        //        vpar.VALSAL = new List<string>();
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE ModificaCasillero(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    appWcfService.PECASI Casillero;

        //    //List<object> listaeo = null;
        //    try
        //    {
        //        Casillero = Util.Deserialize<appWcfService.PECASI>(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var cas = context.PECASI.Find(Casillero.CASICOCA);
        //            if (cas != null)
        //            {
        //                cas.CASICAPA = Casillero.CASICAPA;
        //                cas.CASIALTU = Casillero.CASIALTU;
        //                cas.CASILARG = Casillero.CASILARG;
        //                cas.CASIANCH = Casillero.CASIANCH;
        //                cas.CASIUSMO = Casillero.CASIUSMO;
        //                cas.CASIFEMO = DateTime.Now;
        //                context.SaveChanges();
        //                vpar.ESTOPE = true;
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE DeshabilitaCasillero(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PECASI> listcasilleros = null;
        //    string usuario;
        //    try
        //    {
        //        listcasilleros = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[0]);
        //        usuario = paramOperacion.VALENT[1];
        //        foreach (var item in listcasilleros)
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                var cas = context.PECASI.Find(item.CASICOCA);
        //                if (cas != null)
        //                {
        //                    cas.CASIESTA = 0;
        //                    cas.CASIUSMO = usuario;
        //                    cas.CASIFEMO = DateTime.Now;
        //                    context.SaveChanges();
        //                    vpar.ESTOPE = true;
        //                }
        //                else
        //                {
        //                    vpar.ESTOPE = false;
        //                }
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE DevuelveCasilleros(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<object> listaeo = null;
        //    List<appWcfService.PECASI> lista = null;
        //    decimal idpasillo, idcolumna;
        //    string idnivel;
        //    try
        //    {
        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
        //        idcolumna = decimal.Parse(paramOperacion.VALENT[1]);
        //        idnivel = paramOperacion.VALENT[2];

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (idpasillo != 0 && idcolumna == 0 && idnivel.Equals(""))
        //            {
        //                //solo pasillos
        //                listaeo = context.PECASI.Where(cas => cas.CASIIDPA == idpasillo).ToList<object>();
        //            }
        //            else
        //            {
        //                if (idcolumna != 0 && idpasillo != 0 && idnivel.Equals(""))
        //                {
        //                    //columnas y pasillo
        //                    listaeo = context.PECASI.Where(cas => cas.CASIIDCO == idcolumna && cas.CASIIDPA == idpasillo).ToList<object>();
        //                }
        //                else
        //                {
        //                    //nivel y pasillo
        //                    listaeo = context.PECASI.Where(cas => cas.CASIIDNI.Equals(idnivel) && cas.CASIIDPA == idpasillo).ToList<object>();
        //                }
        //            }
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECASI>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE EliminaNivel(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    string nivel = "";
        //    decimal pasillo = 0;
        //    try
        //    {
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
        //        nivel = paramOperacion.VALENT[1];
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var nive = context.PENIVE.Find(nivel, pasillo);
        //            if (nive != null)
        //            {
        //                context.PENIVE.Remove(nive);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}
        //public RESOPE EliminaColumna(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    decimal columna = 0;
        //    decimal pasillo = 0;
        //    try
        //    {
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
        //        columna = Decimal.Parse(paramOperacion.VALENT[1]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var colu = context.PECOLU.Find(columna, pasillo);
        //            if (colu != null)
        //            {
        //                context.PECOLU.Remove(colu);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE DevuelveNiveyColu(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<object> listaeo = null;
        //    decimal idpasillo = 0;
        //    string informacion = "";
        //    try
        //    {
        //        idpasillo = decimal.Parse(paramOperacion.VALENT[0]);
        //        informacion = paramOperacion.VALENT[1];

        //        List<appWcfService.PENIVE> lista = null;
        //        List<appWcfService.PECOLU> lista1 = null;

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (informacion.Equals("nivel"))
        //            {
        //                //solo pasillos
        //                listaeo = context.PENIVE.Where(nive => nive.NIVEIDPA == idpasillo).ToList<object>();
        //            }
        //            else
        //            {
        //                listaeo = context.PECOLU.Where(colu => colu.COLUIDPA == idpasillo).ToList<object>();
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (informacion.Equals("nivel"))
        //        {
        //            lista = Util.ParseEntityObject<appWcfService.PENIVE>(listaeo);
        //            if (lista.Count > 0)
        //            {
        //                vpar.VALSAL.Add("1"); //existe //0
        //                vpar.VALSAL.Add(Util.Serialize(lista));
        //                //vpar.VALSAL.Add()
        //            }
        //            else
        //            {
        //                vpar.VALSAL.Add("0"); //no existe
        //            }
        //        }
        //        else
        //        {
        //            lista1 = Util.ParseEntityObject<appWcfService.PECOLU>(listaeo);
        //            if (lista1.Count > 0)
        //            {
        //                vpar.VALSAL.Add("1"); //existe //0
        //                vpar.VALSAL.Add(Util.Serialize(lista1));
        //                //vpar.VALSAL.Add()
        //            }
        //            else
        //            {
        //                vpar.VALSAL.Add("0"); //no existe
        //            }
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        //public RESOPE EliminaPasillo(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    decimal pasillo = 0;
        //    try
        //    {
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var pasi = context.PEPASI.Find(pasillo);
        //            if (pasi != null)
        //            {
        //                context.PEPASI.Remove(pasi);
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}

        //public RESOPE DeshabilitaColumna(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    decimal columna = 0;
        //    string usuario;
        //    decimal pasillo;
        //    List<appWcfService.PECASI> lista = null;

        //    try
        //    {
        //        columna = Decimal.Parse(paramOperacion.VALENT[0]);
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
        //        usuario = paramOperacion.VALENT[2];
        //        bool activa = Boolean.Parse(paramOperacion.VALENT[3]);
        //        lista = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[4]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (lista != null)
        //            {
        //                foreach (var item in lista)
        //                {
        //                    var cas = context.PECASI.Find(item.CASICOCA);
        //                    if (cas != null)
        //                    {

        //                        if (activa)
        //                        {
        //                            cas.CASIESTA = 1;
        //                        }
        //                        else cas.CASIESTA = 0;
        //                        cas.CASIUSMO = usuario;
        //                        cas.CASIFEMO = DateTime.Now;
        //                    }
        //                }
        //            }
        //            var colu = context.PECOLU.Find(columna, pasillo);
        //            if (colu != null)
        //            {
        //                if (activa)
        //                {
        //                    colu.COLUESTA = 1;
        //                }
        //                else colu.COLUESTA = 0;
        //                colu.COLUUSMO = usuario;
        //                colu.COLUFEMO = DateTime.Now;
        //                vpar.ESTOPE = true;
        //            }
        //            else
        //            {
        //                vpar.ESTOPE = false;
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE DeshabilitaNivel(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    string nivel = "";
        //    string usuario;
        //    decimal pasillo;
        //    List<appWcfService.PECASI> lista = null;

        //    try
        //    {
        //        nivel = paramOperacion.VALENT[0];
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[1]);
        //        usuario = paramOperacion.VALENT[2];
        //        bool activa = Boolean.Parse(paramOperacion.VALENT[3]);
        //        lista = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[4]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (lista != null)
        //            {
        //                foreach (var item in lista)
        //                {
        //                    var cas = context.PECASI.Find(item.CASICOCA);
        //                    if (cas != null)
        //                    {

        //                        if (activa)
        //                        {
        //                            cas.CASIESTA = 1;
        //                        }
        //                        else cas.CASIESTA = 0;
        //                        cas.CASIUSMO = usuario;
        //                        cas.CASIFEMO = DateTime.Now;
        //                    }
        //                }
        //            }
        //            var nive = context.PENIVE.Find(nivel, pasillo);
        //            if (nive != null)
        //            {
        //                if (activa)
        //                {
        //                    nive.NIVEESTA = 1;
        //                }
        //                else nive.NIVEESTA = 0;
        //                nive.NIVEUSMO = usuario;
        //                nive.NIVEFEMO = DateTime.Now;
        //                vpar.ESTOPE = true;
        //            }
        //            else
        //            {
        //                vpar.ESTOPE = false;
        //            }
        //            context.SaveChanges();
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE DeshabilitaGeneral(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    string usuario;
        //    decimal pasillo;
        //    List<appWcfService.PECASI> listaCasillero = null;
        //    List<appWcfService.PENIVE> listaNivel = null;
        //    List<appWcfService.PECOLU> listColumna = null;
        //    try
        //    {
        //        pasillo = Decimal.Parse(paramOperacion.VALENT[0]);
        //        usuario = paramOperacion.VALENT[1];
        //        listaCasillero = Util.Deserialize<List<appWcfService.PECASI>>(paramOperacion.VALENT[2]);
        //        listaNivel = Util.Deserialize<List<appWcfService.PENIVE>>(paramOperacion.VALENT[3]);
        //        listColumna = Util.Deserialize<List<appWcfService.PECOLU>>(paramOperacion.VALENT[4]);
        //        bool activa = Boolean.Parse(paramOperacion.VALENT[5]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            if (listaCasillero != null)
        //            {
        //                foreach (var item in listaCasillero)
        //                {
        //                    var cas = context.PECASI.Find(item.CASICOCA);
        //                    if (cas != null)
        //                    {
        //                        if (activa)
        //                        {
        //                            cas.CASIESTA = 1;
        //                        }
        //                        else cas.CASIESTA = 0;
        //                        cas.CASIUSMO = usuario;
        //                        cas.CASIFEMO = DateTime.Now;
        //                    }
        //                }
        //            }
        //            if (listaNivel != null)
        //            {
        //                foreach (var item in listaNivel)
        //                {
        //                    var nive = context.PENIVE.Find(item.NIVEIDNI, pasillo);
        //                    if (nive != null)
        //                    {
        //                        if (activa)
        //                        {
        //                            nive.NIVEESTA = 1;
        //                        }
        //                        else nive.NIVEESTA = 0;
        //                        nive.NIVEUSMO = usuario;
        //                        nive.NIVEFEMO = DateTime.Now;
        //                    }
        //                }
        //            }
        //            if (listColumna != null)
        //            {
        //                foreach (var item in listColumna)
        //                {
        //                    var colu = context.PECOLU.Find(item.COLUIDCO, pasillo);
        //                    if (colu != null)
        //                    {
        //                        if (activa)
        //                        {
        //                            colu.COLUESTA = 1;
        //                        }
        //                        else colu.COLUESTA = 0;
        //                        colu.COLUUSMO = usuario;
        //                        colu.COLUFEMO = DateTime.Now;
        //                    }
        //                }
        //            }
        //            var pasi = context.PEPASI.Find(pasillo);
        //            if (pasi != null)
        //            {
        //                if (activa)
        //                {
        //                    pasi.PASIESTA = 1;
        //                }
        //                else pasi.PASIESTA = 0;
        //                pasi.PASIUSMO = usuario;
        //                pasi.PASIFEMO = DateTime.Now;
        //            }
        //            vpar.ESTOPE = true;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}

        //public RESOPE ObtienePedidoTracking(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
        //    try
        //    {
        //        int iniciobus;
        //        string trackingno, num2der, ser2der, diaemi;
        //        decimal idpedido;
        //        trackingno = paramOperacion.VALENT[0];
        //        //N-2-2-2
        //        //ID-NUM 2 DERECHA-SERIE 2 DERECHA-DIA EMISION
        //        iniciobus = trackingno.Length - 6;
        //        idpedido = decimal.Parse(trackingno.Substring(0, iniciobus));
        //        num2der = trackingno.Substring(iniciobus, 2);
        //        ser2der = trackingno.Substring(iniciobus + 2, 2);
        //        diaemi = trackingno.Substring(iniciobus + 4, 2);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            string numero = lista[0].CAPENUME.ToString().PadLeft(8, '0').Substring(6);
        //            //valdia el trackingnumber
        //            if (lista[0].CAPEFECH.Day.ToString().PadLeft(2, '0') == diaemi && lista[0].CAPESERI.Substring(2) == ser2der && numero == num2der && lista[0].CAPEIDES != 1 && lista[0].CAPEIDES != 9)
        //            {
        //                vpar.VALSAL.Add("1");
        //                vpar.VALSAL.Add(Util.Serialize(lista));
        //            }
        //            else
        //            {
        //                vpar.VALSAL.Add("0");
        //            }
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0");
        //        }
        //        vpar.ESTOPE = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//YA ESTA

        #region notificaciones pedido
        public string getHtmlDetallePed(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> pedidodet)
        {
            int fil;
            string encabezado;
            string[] aux;
            string messageBody = ""; //"<font>The following are the records: </font><br><br>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center; \" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6B696B; color:White; font-weight:bold;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"background-color:#E3E3E3; color:Black;\">";
            string htmlTrEnd = "</tr>";
            string htmlTrStart2 = "<tr style =\"background-color:#F3F3F3; color:Black;\">";
            string htmlTrEnd2 = "</tr>";

            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:none; border-width:thin; padding: 5px; text-align:left; \">";
            string htmlTdEnd = "</td>";

            string htmlTdAlgRStart = "<td style=\" border-color:#5c87b2; border-style:none; border-width:thin; padding: 5px; text-align:right; \">";
            string htmlTdAlgREnd = "</td>";

            string htmlFooterRowStart = "<tr style =\"background-color:#a33; color:White; font-weight:bold;\">";
            string htmlFooterRowEnd = "</tr>";


            //decimal SumaBlanco = 0;

            int pos = 0;
            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;

            encabezado = "#,Código,Lote,Kg Solicitados,Kg Atendidos";

            aux = encabezado.Split(new char[] { ',' });
            foreach (string _encabezado in aux)
            {
                if (pos >= 3 && pos <= 4)
                {
                    messageBody += htmlTdAlgRStart + _encabezado + htmlTdEnd;
                }
                else
                {
                    messageBody += htmlTdStart + _encabezado + htmlTdEnd;
                }
                pos++;
            }

            messageBody += htmlHeaderRowEnd;

            fil = 0;
            foreach (var Row in pedidodet)
            {
                if ((fil % 2 != 0))
                {
                    messageBody = messageBody + htmlTrStart;
                }
                else
                {
                    messageBody = messageBody + htmlTrStart2;
                }
                //#,Código,Lote,Kg Solicitados,Kg Atendidos
                messageBody = messageBody + htmlTdStart + Convert.ToString(fil + 1) + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + Convert.ToString(Row.DEPECOAR.Trim()) + htmlTdAlgREnd;
                messageBody = messageBody + htmlTdStart + Convert.ToString(Row.DEPEPART.Trim()) + htmlTdAlgREnd;

                messageBody = messageBody + htmlTdAlgRStart + Convert.ToDecimal(Row.DEPEPESO).ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;
                messageBody = messageBody + htmlTdAlgRStart + Convert.ToDecimal(Row.DEPEPEAT).ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;

                if ((fil % 2 != 0))
                {
                    messageBody = messageBody + htmlTrEnd;
                }
                else
                {
                    messageBody = messageBody + htmlTrEnd2;
                }
                fil++;
            }

            messageBody += htmlFooterRowStart;
            //messageBody = messageBody + htmlTdStart + "TOTAL" + htmlTdEnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdEnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;

            //messageBody = messageBody + htmlTdAlgRStart + SumaBlanco.ToString(Constantes.FORMATO_IMPORTE) + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;
            messageBody = messageBody + htmlTdStart + "" + htmlTdAlgREnd;

            messageBody += htmlFooterRowEnd;

            messageBody = messageBody + htmlTableEnd;
            messageBody += "<br>";

            return messageBody;
        }

        public bool EnvioCorreo(string destinatario, string cc, string bcc, string asunto, string body)
        {
            bool resultado = false;
            try
            {
                System.Net.Mail.MailMessage mailMessage = default(System.Net.Mail.MailMessage);
                mailMessage = new System.Net.Mail.MailMessage();
                //mailMessage.From = new System.Net.Mail.MailAddress(CuentaDe); // , CuentaDescripcion);
                mailMessage.From = new System.Net.Mail.MailAddress(CuentaDe, CuentaDescripcion); //prueba 20160201
                mailMessage.To.Add(destinatario);
                if (!string.IsNullOrEmpty(cc))
                {
                    mailMessage.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    mailMessage.Bcc.Add(bcc);
                }
                //asunto = AsuntoCorreo;
                mailMessage.Subject = asunto;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;

                System.Net.Mail.SmtpClient o = new System.Net.Mail.SmtpClient(ServidorSMTP);
                o.UseDefaultCredentials = false;
                o.Credentials = new System.Net.NetworkCredential(CuentaDe, ClaveCuenta, DominioCuenta);
                //o.EnableSsl = True 'gmail
                o.Port = Convert.ToInt32(PuertoSMTP);
                o.Send(mailMessage);
                resultado = true;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            return resultado;
        }

        //public bool EnviaCorreoNotificacionPedido(decimal idpedido, out string mensaje)
        //{
        //    string destinatario, cc, bcc, asunto, body;

        //    bool vpar;
        //    vpar = false;
        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
        //    appLogica.appDB2 _appDB2 = null;

        //    try
        //    {
        //        destinatario = bcc = "";

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);

        //        if (lista.Count > 0)
        //        {
        //            if (lista[0].CAPEIDES == 1)
        //            {
        //                mensaje = "Pedido no emitido";
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrWhiteSpace(lista[0].CAPEEMAI))
        //                {
        //                    destinatario = lista[0].CAPEEMAI.Trim();
        //                }
        //                //pruebas
        //                destinatario = "ddk_sk@hotmail.com";
        //                _appDB2 = new appLogica.appDB2();
        //                RFEUSER usuario = _appDB2.ObtieneUsuarioDeFacturacion(lista[0].CAPEUSEM);
        //                if (usuario != null)
        //                {
        //                    bcc = usuario.USERMAIL.Trim();
        //                }
        //                //pruebas
        //                bcc = "mlopez@incatops.com";
        //                if (PreparaCorreoNotificacionPedido(lista, out cc, out asunto, out body, out mensaje))
        //                {
        //                    if (EnvioCorreo(destinatario, cc, bcc, asunto, body))
        //                    {
        //                        vpar = true;
        //                    }
        //                    else
        //                    {
        //                        mensaje = Mensajes.MENSAJE_CORREO_ERROR_ENVIO;
        //                    }
        //                }
        //                else
        //                {
        //                    //vpar.MENERR = mensaje;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            mensaje = Mensajes.MENSAJE_PEDIDO_NO_ENCONTRADO;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        mensaje = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //        if (_appDB2 != null)
        //        {
        //            _appDB2.Finaliza();
        //            _appDB2 = null;
        //        }
        //    }
        //    return vpar;
        //}//YA ESTA

        public bool PreparaCorreoNotificacionPedido(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> pedidodet, out string cc, out string asunto, out string body, out string mensaje)
        {
            bool resultado = false;
            string detallehtm;
            string idseguimiento, estado, accionestado;
            appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result pedcab;

            cc = asunto = body = mensaje = null;
            //cc = ConfigurationManager.AppSettings["ControlCalidadCopia"];
            pedcab = pedidodet[0];

            idseguimiento = pedcab.CAPEIDCP.ToString() + pedcab.CAPENUME.ToString().PadLeft(8, '0').Substring(6) + pedcab.CAPESERI.Substring(2) + pedcab.CAPEFECH.Day.ToString().PadLeft(2, '0');
            //2   Emitido
            //3   En preparación
            //4   En aprobación
            //5   Completado
            estado = accionestado = "";
            if (pedcab.CAPEIDES == 2)
            {
                accionestado = "ha sido emitido";
                estado = "Emitido";
            }
            else if (pedcab.CAPEIDES == 3)
            {
                accionestado = "esta siendo preparado";
                estado = "En preparación";
            }
            else if (pedcab.CAPEIDES == 4)
            {
                accionestado = "ha sido preparado";
                estado = "Preparado";
            }
            else if (pedcab.CAPEIDES == 5)
            {
                accionestado = "ha sido despachado";
                estado = "Despachado";
            }
            else if (pedcab.CAPEIDES == 9)
            {
                accionestado = "ha sido anulado";
                estado = "Anulado";
            }
            else
            {
                mensaje = "El estado para la notificacion del pedido no es válido";
                return false;
            }

            detallehtm = getHtmlDetallePed(pedidodet);

            asunto = string.Format(Mensajes.TEXTO_ASUNTO_NOTIFICACION_PEDIDO, idseguimiento, accionestado);
            body = "<p>" + "Estimados Señores:" + "<b>" + "</b>" + "</p>" + "<p>" + "A continuación se detalla la información relacionada a su pedido </p>";
            body += "<p>" + "<b>" + "Identificador Pedido: " + "</b>" + idseguimiento + "</p>";
            body += "<b>" + "Fecha Emisión: " + "</b>" + pedcab.CAPEFHEM.Value.ToString(Constantes.FORMATO_FECHA) + "";

            body += "<p>" + "<b>" + "Inicio Preparación: " + "</b>" + (pedcab.CAPEFHIP.HasValue ? pedcab.CAPEFHIP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Preparación Completada: " + "</b>" + (pedcab.CAPEFHFP.HasValue ? pedcab.CAPEFHFP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Fecha de Despacho: " + "</b>" + (pedcab.CAPEFEAP.HasValue ? pedcab.CAPEFEAP.Value.ToString(Constantes.FORMATO_FECHA) : "</p>") + "";
            body += "<p>" + "<b>" + "Estado: " + "</b>" + estado + "</p>";

            body += "<br>";
            body += "<p>" + "Puede visualizar el estado de su pedido ingresando a la siguiente dirección y usando el identificador de su pedido" + "</p>";
            body += "<p>" + "http://localhost:52998/default.aspx" + "</p>";
            body += "<br>";

            body += detallehtm + "<br>";
            body += "<p>" + "<b>" + "Observación pedido: " + "</b>" + pedcab.CAPENOTG + "</p>";
            body += "<br>Atentamente<br><p>" + "</p>" + "<br>" + "<p>" + "<b>" + Mensajes.TEXTO_NOTIFICACION_PIE + "</b>" + "</p>";
            body = body.Replace("\\n", "<br>");
            body = "<style>   body {font-family:Verdana; font-size: x-small } p {font-family:Verdana; font-size: x-small } </style>" + body;
            resultado = true;

            return resultado;
        }

        #endregion
        //public RESOPE ObtieneParametros(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.PEPARM> lista = null;
        //    try
        //    {
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PEPARM.ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PEPARM>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //            //vpar.VALSAL.Add()
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }

        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//LISTO

        public RESOPE ActualizaNotificaciones(PAROPE paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string emitir, iprepa, fprepa, despa;
            try
            {
                emitir = paramOperacion.VALENT[0];
                iprepa = paramOperacion.VALENT[1];
                fprepa = paramOperacion.VALENT[2];
                despa = paramOperacion.VALENT[3];

                using (var context = new PEDIDOSEntities())
                {
                    var aleemi = context.PEPARM.Find(21);
                    if (aleemi != null)
                    {
                        aleemi.PARMVAPA = emitir;
                    }
                    var aleipr = context.PEPARM.Find(22);
                    if (aleipr != null)
                    {
                        aleipr.PARMVAPA = iprepa;
                    }
                    var alefpr = context.PEPARM.Find(23);
                    if (alefpr != null)
                    {
                        alefpr.PARMVAPA = fprepa;
                    }
                    var aledes = context.PEPARM.Find(24);
                    if (aledes != null)
                    {
                        aledes.PARMVAPA = despa;
                    }
                    context.SaveChanges();
                }
                vpar.ESTOPE = true;
            }

            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {
            }
            return vpar;
        }

        //public RESOPE ValidaitemExcel(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    appWcfService.PEDEPE item = new appWcfService.PEDEPE();
        //    try
        //    {
        //        item = Util.Deserialize<appWcfService.PEDEPE>(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var obj = context.I1DD41A.FirstOrDefault(x => x.LOTITEM.Equals(item.DEPECOAR) && x.LOTPARTI.Equals(item.DEPEPART) && x.LOTALM.Equals(item.DEPEALMA));
        //            if (obj != null)
        //            {
        //                vpar.ESTOPE = true;
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//EF

        //public RESOPE ValidaPreparacion(PAROPE paramOperacion) //uno por uno
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    appWcfService.PEDEPE item = new appWcfService.PEDEPE();
        //    try
        //    {
        //        item = Util.Deserialize<appWcfService.PEDEPE>(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var obj = context.PEDEPE.FirstOrDefault(x => x.DEPEIDDP == item.DEPEIDDP);
        //            if (obj != null)
        //            {
        //                if (obj.DEPECAAT == 0 && obj.DEPEPEAT == 0)
        //                {
        //                    vpar.ESTOPE = true;
        //                }
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//EF FIRST OR DEFAULT

        //public RESOPE ValidaPreparacionList(PAROPE paramOperacion) //multiple
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<appWcfService.PEDEPE> Listadetalles = new List<appWcfService.PEDEPE>();
        //    bool validalista = true; //Flag para determinar si toda la lista es valida
        //    try
        //    {
        //        Listadetalles = Util.Deserialize<List<appWcfService.PEDEPE>>(paramOperacion.VALENT[0]);
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            foreach (var item in Listadetalles)
        //            {
        //                var det = context.PEDEPE.FirstOrDefault(x => x.DEPEIDDP == item.DEPEIDDP);
        //                if (det != null)
        //                {
        //                    if (det.DEPECAAT > 0 || det.DEPEPEAT > 0)
        //                    {
        //                        validalista = false;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        vpar.VALSAL = new List<string>();
        //        if (validalista)
        //        {
        //            vpar.ESTOPE = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return vpar;
        //}//EF FIRST OR DEFAULT
        /// <summary>
        /// Actualiza PROSAS en AS
        /// </summary>
        /// <param name="folio"></param>
        /// <param name="secuencia"></param>
        /// <param name="pesoentregado">Peso entregado en la OSA, -1 si no se actualizará</param>
        /// <param name="estado">estado de la OSA, solo se actualizará si pesoentregado es -1</param>
        private void actualizaPROSAS(string folio, decimal secuencia, decimal pesoentregado, string estado)
        {
            appLogica.appDB2 _appDB2 = null;
            try
            {
                _appDB2 = new appLogica.appDB2();
                _appDB2.actualizaPROSAS(folio, secuencia, pesoentregado, estado);
            }
            finally
            {
                if (_appDB2 != null)
                {
                    _appDB2.Finaliza();
                    _appDB2 = null;
                }
            }
        }

        /// <summary>
        /// Actualiza GMDEEM en AS
        /// </summary>
        /// <param name="empaque"></param>
        /// <param name="secuencia"></param>
        /// <param name="cantidadrestante"></param>
        /// <param name="pesorestante"></param>
        /// <param name="stockcerobolsa">Valor 1 o 0 para indicar que la bolsa se marcó como stock cero, -1 para no actualizar el valor</param>
        /// <param name="estadobolsa"></param>
        private void actualizaGMDEEM(string empaque, decimal secuencia, decimal cantidadrestante, decimal pesorestante, decimal stockcerobolsa, decimal estadobolsa)
        {
            appLogica.appDB2 _appDB2 = null;
            try
            {
                _appDB2 = new appLogica.appDB2();
                _appDB2.actualizaGMDEEM(empaque, secuencia, cantidadrestante, pesorestante, stockcerobolsa, estadobolsa);
            }
            finally
            {
                if (_appDB2 != null)
                {
                    _appDB2.Finaliza();
                    _appDB2 = null;
                }
            }
        }

        #region SIN EMPAQUE

        //public RESOPE ObtieneDetallePreparacionse(PAROPE paramOperacion)
        //{

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    List<object> listaeo = null;
        //    List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> lista = null;
        //    try
        //    {
        //        decimal iddetallepedido = decimal.Parse(paramOperacion.VALENT[0]);

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE(iddetallepedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>(listaeo);
        //        vpar.VALSAL = new List<string>();
        //        if (lista.Count > 0)
        //        {
        //            vpar.VALSAL.Add("1"); //existe //0
        //            vpar.VALSAL.Add(Util.Serialize(lista));
        //        }
        //        else
        //        {
        //            vpar.VALSAL.Add("0"); //no existe
        //        }
        //        vpar.ESTOPE = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        vpar.MENERR = ErrorGenerico(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    return vpar;
        //}//YA ESTA

        //public RESOPE guardaPreparacionBolsase(PAROPE paramOperacion)//appWcfService.PEBODP detallebolsa)
        //{
        //    ////
        //    List<appWcfService.PEBODP> listBolsas;
        //    ////

        //    Nullable<decimal> iddetpedidostoc = null;

        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    string resultado = "";
        //    string partida, articulo;
        //    decimal almacen;
        //    try
        //    {
        //        ///
        //        listBolsas = Util.Deserialize<List<appWcfService.PEBODP>>(paramOperacion.VALENT[0]);
        //        ///

        //        partida = articulo = "";
        //        almacen = 0;
        //        //if (detallebolsa == null)
        //        //{
        //        //    Util.EscribeLog("detallebolsa es null");
        //        //}
        //        //else
        //        //{
        //        //    Util.EscribeLog("detallebolsa " + detallebolsa.BODPIDDP.ToString() + " " + detallebolsa.BODPIDDP.ToString());
        //        //}

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            foreach (var detallebolsa in listBolsas)
        //            {
        //                EFModelo.PEDEPE detpednac = null;
        //                EFModelo.PEDEOS detpedint = null;
        //                EFModelo.GMCAEM emp = null;
        //                //inserta PBOLS si no existe
        //                //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
        //                bool sinempaque = string.IsNullOrWhiteSpace(detallebolsa.PEBOLS.BOLSCOEM);
        //                if (!sinempaque)
        //                {
        //                    emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
        //                }

        //                if (emp != null || sinempaque)
        //                {
        //                    EFModelo.PEBOLS bol = null;
        //                    if (!sinempaque)
        //                    {
        //                        bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);

        //                        if (bol == null) //no existe pbols, insertar
        //                        {
        //                            bol = new EFModelo.PEBOLS();
        //                            //inserta = true;
        //                            bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
        //                            bol.BOLSUSCR = detallebolsa.BODPUSCR;
        //                            bol.BOLSFECR = DateTime.Now;
        //                            bol.BOLSCOCA = null;
        //                            bol.BOLSESTA = 1;
        //                            bol.BOLSALMA = emp.CAEMALMA;
        //                            bol.BOLSCOAR = "";
        //                            Util.EscribeLog("3");

        //                            context.PEBOLS.Add(bol);
        //                            Util.EscribeLog("4");

        //                            context.SaveChanges();
        //                            Util.EscribeLog("5");

        //                            detallebolsa.BODPIDBO = bol.BOLSIDBO;
        //                        }
        //                        else
        //                        {
        //                            if (bol.BOLSALMA == 0)
        //                            {
        //                                bol.BOLSALMA = emp.CAEMALMA;
        //                            }
        //                        }
        //                    }

        //                    if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
        //                    {
        //                        //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
        //                    }
        //                    //bool inserta = false;
        //                    //
        //                    if (detallebolsa.BODPIDDP.HasValue)
        //                    {
        //                        detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
        //                        partida = detpednac.DEPEPART;
        //                        articulo = detpednac.DEPECOAR;
        //                        almacen = detpednac.DEPEALMA;
        //                    }
        //                    else if (detallebolsa.BODPIDDO.HasValue)
        //                    {
        //                        detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
        //                        partida = detpedint.DEOSPART;
        //                        articulo = detpedint.DEOSCOAR;
        //                        almacen = detpedint.DEOSALMA;
        //                    }

        //                    //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
        //                    var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

        //                    if (ent == null) //detallebolsa.BODPIDDE != 0)
        //                    {
        //                        ent = new EFModelo.PEBODP();
        //                        //inserta = true;
        //                        ent.BODPUSCR = detallebolsa.BODPUSCR;
        //                        ent.BODPFECR = DateTime.Now;
        //                        context.PEBODP.Add(ent);
        //                        if (!sinempaque)
        //                        {
        //                            //inserta tipo 1 SALIDA
        //                            insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ent.BODPUSMO = detallebolsa.BODPUSCR;
        //                        ent.BODPFEMO = DateTime.Now;
        //                        ent.BODPESTA = 3;
        //                        if (!sinempaque)
        //                        {
        //                            //SOLO si las cantidades o pesos son diferentes
        //                            //inserta tipo 3 reingreso
        //                            //inserta tipo 1 salida
        //                            if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
        //                            {
        //                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
        //                            }
        //                        }
        //                    }
        //                    //automatizar el parse sin incluir la PK
        //                    ent.BODPIDBO = detallebolsa.BODPIDBO;
        //                    ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
        //                    ent.BODPALMA = almacen; //bol.BOLSALMA;
        //                    ent.BODPPART = partida;
        //                    ent.BODPCOAR = articulo;
        //                    ent.BODPCANT = detallebolsa.BODPCANT;
        //                    ent.BODPPESO = detallebolsa.BODPPESO;
        //                    ent.BODPPERE = detallebolsa.BODPPERE;
        //                    ent.BODPDIFE = detallebolsa.BODPDIFE;
        //                    ent.BODPSTCE = detallebolsa.BODPSTCE;
        //                    ent.BODPINBO = detallebolsa.BODPINBO;
        //                    ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
        //                                                          //2018-04-11
        //                    ent.BODPTAUN = detallebolsa.BODPTAUN;
        //                    //iddetpedido = ent.BODPIDDP;
        //                    //iddetpedidoint = ent.BODPIDDO;
        //                    if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
        //                    {
        //                        iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
        //                    }

        //                    ent.BODPTADE = detallebolsa.BODPTADE;
        //                    ent.BODPPEBR = detallebolsa.BODPPEBR;
        //                    ent.BODPESTA = detallebolsa.BODPESTA;

        //                    context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
        //                    decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    if (detallebolsa.BODPIDDP.HasValue)
        //                    {
        //                        var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
        //                        // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                        cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                        foreach (var bolsaprep in listbolsas)
        //                        {
        //                            cantatendida += bolsaprep.BODPCANT;
        //                            pesoatendido += bolsaprep.BODPPESO;
        //                            pesoreal += bolsaprep.BODPPERE;
        //                            tade += bolsaprep.BODPTADE;
        //                            pebr += bolsaprep.BODPPEBR;
        //                        }
        //                        //var detpednac = context.PEDEPE.Find(iddetpedido);
        //                        detpednac.DEPECAAT = cantatendida;
        //                        detpednac.DEPEPEAT = pesoatendido;
        //                        detpednac.DEPEPERE = pesoreal;
        //                        if (iddetpedidostoc.HasValue)
        //                        {
        //                            detpednac.DEPESTOC = iddetpedidostoc;
        //                        }
        //                        detpednac.DEPETADE = tade;
        //                        detpednac.DEPEPEBR = pebr;

        //                        context.SaveChanges();
        //                    }
        //                    else if (detallebolsa.BODPIDDO.HasValue)
        //                    {
        //                        var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
        //                        //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                        cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                        foreach (var bolsaprep in listbolsas)
        //                        {
        //                            cantatendida += bolsaprep.BODPCANT;
        //                            pesoatendido += bolsaprep.BODPPESO;
        //                            pesoreal += bolsaprep.BODPPERE;
        //                            tade += bolsaprep.BODPTADE;
        //                            pebr += bolsaprep.BODPPEBR;
        //                        }
        //                        //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
        //                        detpedint.DEOSCAAT = cantatendida;
        //                        detpedint.DEOSPEAT = pesoatendido;
        //                        detpedint.DEOSPERE = pesoreal;
        //                        if (iddetpedidostoc.HasValue)
        //                        {
        //                            detpedint.DEOSSTOC = iddetpedidostoc;
        //                        }
        //                        detpedint.DEOSTADE = tade;
        //                        detpedint.DEOSPEBR = pebr;
        //                        //actualiza peso en osa ---PENDIENTE EN AS
        //                        var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
        //                        if (osa != null)
        //                        {
        //                            osa.OSASCAEN = pesoatendido;
        //                            actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
        //                        }
        //                        context.SaveChanges();
        //                    }
        //                    if (!sinempaque)
        //                    {
        //                        //actualiza el stock de la bolsa
        //                        var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
        //                        //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                        cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                        foreach (var bolsaprep in todasbolsasprep)
        //                        {
        //                            cantatendida += bolsaprep.BODPCANT;
        //                            pesoatendido += bolsaprep.BODPPESO;
        //                            pesoreal += bolsaprep.BODPPERE;
        //                            tade += bolsaprep.BODPTADE;
        //                            pebr += bolsaprep.BODPPEBR;
        //                        }
        //                        var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
        //                        if (detemp != null)
        //                        {
        //                            //---PENDIENTE EN AS
        //                            detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
        //                            if (emp.CAEMMSPA == "+")
        //                            {
        //                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
        //                            }
        //                            else
        //                            {
        //                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
        //                            }
        //                            if (iddetpedidostoc.HasValue)
        //                            {
        //                                detemp.DEEMSTCE = iddetpedidostoc.Value;
        //                            }
        //                            if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
        //                            {
        //                                detemp.DEEMESBO = 9;
        //                            }
        //                            else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
        //                            {
        //                                detemp.DEEMESBO = 9;
        //                            }
        //                            else
        //                            {
        //                                detemp.DEEMESBO = 1;
        //                            }
        //                            actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
        //                            context.SaveChanges();
        //                            //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
        //                            detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
        //                            if (detemp == null)
        //                            {
        //                                bol.BOLSESTA = 9; //ya no se usa la bolsa
        //                            }
        //                            else
        //                            {
        //                                bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
        //                            }
        //                            context.SaveChanges();
        //                        }
        //                    }
        //                    vpar.VALSAL = new List<string>();
        //                    ////vpar.VALSAL.Add(ent.BODPIDDE.ToString());
        //                    ////vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
        //                    resultado = ""; // ent.BODPIDDE.ToString();
        //                    vpar.ESTOPE = true;
        //                }
        //                else
        //                {
        //                    resultado = "Código de empaque incorrecto";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        resultado = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    vpar.MENERR = resultado;
        //    return vpar;
        //}
        //public RESOPE remueveBolsaPedidose(PAROPE paramOperacion)//decimal idbolsapedido, string usuario)
        //{
        //    Nullable<decimal> iddetpedido;
        //    Nullable<decimal> iddetpedidoint;
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };

        //    string resultado = "";
        //    string partida, articulo;
        //    try
        //    {
        //        decimal idbolsapedido = decimal.Parse(paramOperacion.VALENT[0]);
        //        string usuario = paramOperacion.VALENT[1];

        //        partida = articulo = "";
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            EFModelo.PEDEPE detpednac = null;
        //            EFModelo.PEDEOS detpedint = null;

        //            //bool inserta = false;
        //            //foreach (var item in collection)
        //            //{

        //            //}
        //            var ent = context.PEBODP.Find(idbolsapedido);//BODPIDDE
        //            if (ent != null) //detallebolsa.BODPIDDE != 0)
        //            {
        //                bool sinempaque = !ent.BODPIDBO.HasValue;
        //                EFModelo.PEBOLS bol = null;
        //                if (!sinempaque)
        //                {
        //                    bol = context.PEBOLS.Find(ent.BODPIDBO);
        //                }

        //                if (ent.BODPIDDP.HasValue)
        //                {
        //                    detpednac = context.PEDEPE.Find(ent.BODPIDDP);
        //                    partida = detpednac.DEPEPART;
        //                    articulo = detpednac.DEPECOAR;
        //                }
        //                else if (ent.BODPIDDO.HasValue)
        //                {
        //                    detpedint = context.PEDEOS.Find(ent.BODPIDDO);
        //                    partida = detpedint.DEOSPART;
        //                    articulo = detpedint.DEOSCOAR;
        //                }
        //                iddetpedido = ent.BODPIDDP;
        //                iddetpedidoint = ent.BODPIDDO;
        //                //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
        //                //{
        //                //bol.BOLSESTA = 1; //ya no se usa
        //                //}
        //                if (!sinempaque)
        //                {
        //                    insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
        //                }
        //                context.PEBODP.Remove(ent);
        //                context.SaveChanges();
        //                decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

        //                if (iddetpedido.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    //detpednac = context.PEDEPE.Find(iddetpedido);
        //                    detpednac.DEPECAAT = cantatendida;
        //                    detpednac.DEPEPEAT = pesoatendido;
        //                    detpednac.DEPEPERE = pesoreal;
        //                    detpednac.DEPETADE = tade;
        //                    detpednac.DEPEPEBR = pebr;

        //                    //inserta tipo 2 kardex reingreso
        //                    context.SaveChanges();
        //                }
        //                else if (iddetpedidoint.HasValue)
        //                {
        //                    var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in listbolsas)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    detpedint.DEOSCAAT = cantatendida;
        //                    detpedint.DEOSPEAT = pesoatendido;
        //                    detpedint.DEOSPERE = pesoreal;
        //                    detpedint.DEOSTADE = tade;
        //                    detpedint.DEOSPEBR = pebr;
        //                    //actualiza peso en osa ---PENDIENTE EN AS
        //                    var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
        //                    if (osa != null)
        //                    {
        //                        osa.OSASCAEN = pesoatendido;
        //                        actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar AS
        //                    }
        //                    context.SaveChanges();

        //                }
        //                if (!sinempaque)
        //                {
        //                    //actualiza el stock de la bolsa
        //                    var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
        //                    //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
        //                    cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
        //                    foreach (var bolsaprep in todasbolsasprep)
        //                    {
        //                        cantatendida += bolsaprep.BODPCANT;
        //                        pesoatendido += bolsaprep.BODPPESO;
        //                        pesoreal += bolsaprep.BODPPERE;
        //                        tade += bolsaprep.BODPTADE;
        //                        pebr += bolsaprep.BODPPEBR;
        //                    }
        //                    var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
        //                    if (detemp != null)
        //                    {
        //                        detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
        //                        detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
        //                        //si la bolsa preparada estaba marcada como stock cero, lo desmarco
        //                        if (ent.BODPSTCE == 1)
        //                        {
        //                            detemp.DEEMSTCE = 0;
        //                        }
        //                        if (detemp.DEEMSTCE == 0)
        //                        {
        //                            detemp.DEEMESBO = 1;
        //                        }
        //                        else
        //                        if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
        //                        {
        //                            detemp.DEEMESBO = 9;
        //                        }
        //                        else
        //                        {
        //                            detemp.DEEMESBO = 1;
        //                        }
        //                        context.SaveChanges();
        //                        actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); //Validar AS
        //                                                                                                                                               //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
        //                        detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
        //                        if (detemp == null)
        //                        {
        //                            bol.BOLSESTA = 9; //ya no se usa la bolsa
        //                        }
        //                        else
        //                        {
        //                            bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
        //                        }
        //                        context.SaveChanges();
        //                    }
        //                }
        //            }
        //            vpar.ESTOPE = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //        resultado = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    vpar.MENERR = resultado;
        //    return vpar;
        //}

        #endregion

        public void Finaliza()
        {
            //DB2 = null;
        }

        private string ErrorGenerico(string exception)
        {
            if (OcultaErrorReal)
            {
                return Mensajes.MENSAJE_ERROR_GENERICO;
            }
            else
            {
                return exception;
            }
        }
        //cambios



    }
}
