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
        private const decimal TIPO_MOV_DEVOLUCION_MATERIAL = 4; //+
        #endregion

        public MKT()
        {
            string stringConnection, userId, userPassword;
            DataSourceDB2 = ConfigurationManager.AppSettings["DataSource"];
            Aplicacion = ConfigurationManager.AppSettings["Aplicacion"];

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

        //public RESOPE MostrarPedidosApp(PAROPE paramOperacion)
        //{
        //    RESOPE vpar;
        //    vpar = new RESOPE() { ESTOPE = false };
        //    List<EFModelo.PECAPE> lista;
        //    try
        //    {
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            lista = context.PECAPE.Where(ped => ped.CAPEIDES == 1 || ped.CAPEIDES == 2).OrderByDescending(x => x.CAPEFHIP).ToList();
        //        }
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

        //public List<appWcfService.PECAPE> MostrarPedidosApp2()
        //{
        //    List<object> listaeo = null;
        //    List<appWcfService.PECAPE> lista = null;
        //    try
        //    {
        //        using (var sqlLogFile = new StreamWriter("D:\\scriptspedidos\\sqlLogFile.txt"))
        //        {
        //            using (var context = new PEDIDOSEntities())
        //            {
        //                context.Database.Log = sqlLogFile.Write; // Console.Write;

        //                listaeo = context.PECAPE.Include("TCLIE").Where(ped => (ped.CAPEIDES == 2 || ped.CAPEIDES == 3)).OrderByDescending(ped => ped.CAPEIDES).ThenByDescending(ped => ped.CAPEPRIO > 0).ThenBy(ped => ped.CAPEPRIO).ThenBy(ped => ped.CAPEFHEM).ToList<object>();

        //            }
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PECAPE>(listaeo);
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return lista;
        //}

        public List<appWcfService.PEDEPE> mostrarDetallePedidosApp(decimal idpedido)
        {
            List<object> listaeo = null;
            List<appWcfService.PEDEPE> lista = null;
            try
            {
                using (var sqlLogFile = new StreamWriter("D:\\scriptspedidos\\sqlLogFile.txt"))
                {
                    using (var context = new PEDIDOSEntities())
                    {
                        context.Database.Log = sqlLogFile.Write; // Console.Write;
                        listaeo = context.PEDEPE.Include("I1DD20A").Where(ped => ped.DEPEIDCP == idpedido).ToList<object>();
                    }
                }
                lista = Util.ParseEntityObject<appWcfService.PEDEPE>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//LISTO

        public List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> mostrarDetallePedidos(decimal idpedido)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> lista = null;
            try
            {
                    using (var context = new PEDIDOSEntities())
                    {
                        listaeo = context.USP_OBTIENE_DETALLE_PEDIDOS(idpedido).ToList<object>();
                    }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//LISTO

        public RESOPE cambiaEstadoPedido(PEESPE estadopedido)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            try
            {
                //Util.EscribeLog("SI ENTRA");
                //if (estadopedido == null)
                //{
                //    Util.EscribeLog("estadopedido es null");
                //}
                //else
                //{
                //    Util.EscribeLog("estadopedido " + estadopedido.ESPEIDPE.ToString() + " " + estadopedido.ESPEIDES.ToString());
                //}
                List<EFModelo.PEPARM> listapar;
                EFModelo.PEPARM par;
                string mensajecorreo;

                using (var context = new PEDIDOSEntities())
                {
                    var ent = context.PECAPE.Find(Convert.ToDecimal(estadopedido.ESPEIDPE));
                    if (ent == null)
                    {
                        vpar.MENERR = Mensajes.MENSAJE_PEDIDO_NO_ENCONTRADO;
                        return vpar;
                    }
                    if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3/* && ent.CAPEIDES != 2*/)  //3   En preparación                                    
                    {
                        ent.CAPEUSIP = estadopedido.USUARIO;
                        ent.CAPEFHIP = DateTime.Now;
                    }
                    else if (Convert.ToDecimal(estadopedido.ESPEIDES) == 4 /*&& ent.CAPEIDES != 3*/)  //4   preparación finalizada
                    {
                        ent.CAPEUSFP = estadopedido.USUARIO;
                        ent.CAPEFHFP = DateTime.Now;
                        //Faltaria agregarlo el tade
                    }
                    ent.CAPEIDES = Convert.ToDecimal(estadopedido.ESPEIDES);
                    context.SaveChanges();
                    listapar = context.PEPARM.ToList();
                }
                vpar.ESTOPE = true;

                //notificaciones cliente
                if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3 || Convert.ToDecimal(estadopedido.ESPEIDES) == 4)
                {
                    if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3) //En preparacion
                    {
                        par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_INICIAR_PREPARACION_PEDIDO);
                    }
                    else
                    {
                        //Fin Preparacion
                        par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_FINALIZAR_PREPARACION_PEDIDO);
                    }

                    if (par != null && par.PARMVAPA.Equals("1"))
                    {
                        if (!EnviaCorreoNotificacionPedido(Convert.ToDecimal(estadopedido.ESPEIDPE), out mensajecorreo))
                        {
                            Util.EscribeLog(mensajecorreo);
                        }
                    }

                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA
        public RESOPE cambiaEstadoPedido2(PEESPE estadopedido)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            try
            {
                //Util.EscribeLog("SI ENTRA");
                //if (estadopedido == null)
                //{
                //    Util.EscribeLog("estadopedido es null");
                //}
                //else
                //{
                //    Util.EscribeLog("estadopedido " + estadopedido.ESPEIDPE.ToString() + " " + estadopedido.ESPEIDES.ToString());
                //}
                List<EFModelo.PEPARM> listapar;
                EFModelo.PEPARM par;
                string mensajecorreo;
                    using (var context = new PEDIDOSEntities())
                    {
                        var ent = context.PECAPE.Find(Convert.ToDecimal(estadopedido.ESPEIDPE));
                        if (ent == null)
                        {
                            vpar.MENERR = Mensajes.MENSAJE_PEDIDO_NO_ENCONTRADO;
                            return vpar;
                        }
                        if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3/* && ent.CAPEIDES != 2*/)  //3   En preparación                                    
                        {
                            ent.CAPEUSIP = estadopedido.USUARIO;
                            ent.CAPEFHIP = DateTime.Now;
                        }
                        else if (Convert.ToDecimal(estadopedido.ESPEIDES) == 4 /*&& ent.CAPEIDES != 3*/)  //4   preparación finalizada
                        {
                            ent.CAPEUSFP = estadopedido.USUARIO;
                            ent.CAPEFHFP = DateTime.Now;
                            ent.CAPENUBU = estadopedido.CAPENUBU;
                            ent.CAPETADE = estadopedido.CAPETADE;
                        }
                        ent.CAPEIDES = Convert.ToDecimal(estadopedido.ESPEIDES);
                        context.SaveChanges();
                        listapar = context.PEPARM.ToList();
                    }
                vpar.ESTOPE = true;

                //notificaciones cliente
                if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3 || Convert.ToDecimal(estadopedido.ESPEIDES) == 4)
                {
                    if (Convert.ToDecimal(estadopedido.ESPEIDES) == 3) //En preparacion
                    {
                        par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_INICIAR_PREPARACION_PEDIDO);
                    }
                    else
                    {
                        //Fin Preparacion
                        par = listapar.Find(x => x.PARMIDPA == Constantes.NOTIFICACION_AL_FINALIZAR_PREPARACION_PEDIDO);
                    }

                    if (par != null && par.PARMVAPA.Equals("1"))
                    {
                        if (!EnviaCorreoNotificacionPedido(Convert.ToDecimal(estadopedido.ESPEIDPE), out mensajecorreo))
                        {
                            Util.EscribeLog(mensajecorreo);
                        }
                    }

                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA

        private void insertaMovimientoKardex(PEDIDOSEntities context, decimal? idbolsa, decimal idtipomovimiento, decimal almacen, string partida, string articulo, decimal cantidad, decimal peso, decimal pesobr, string usuario, Nullable<decimal> iddetpedido, Nullable<decimal> iddetosa)
        {
            using (var sqlLogFile = new StreamWriter("D:\\scriptspedidos\\sqlLogFile.txt"))
            {
                context.Database.Log = sqlLogFile.Write; // Console.Write;
                decimal valorsigno = 1;
                if (idtipomovimiento == TIPO_MOV_SALIDA_PREP_PED)
                {
                    valorsigno = -1;
                }
                var entk = new EFModelo.PEKABO();
                entk.KABOIDBO = idbolsa.Value;
                entk.KABOIDTM = idtipomovimiento;
                entk.KABOALMA = almacen;
                entk.KABOPART = partida;
                entk.KABOITEM = articulo;
                entk.KABOCANT = cantidad * valorsigno;
                entk.KABOPESO = peso * valorsigno;
                entk.KABOPEBR = pesobr * valorsigno;
                entk.KABOTARA = entk.KABOPEBR - entk.KABOPESO;
                entk.KABOFECH = DateTime.Today;
                entk.KABOUSCR = usuario;
                entk.KABOFECR = DateTime.Now;
                entk.KABOIDDP = iddetpedido;
                entk.KABOIDDO = iddetosa;
                context.PEKABO.Add(entk);
            }
           

        }//dd

        public RESOPE guardaPreparacionBolsa(appWcfService.PEBODP detallebolsa)
        {
            Nullable<decimal> iddetpedidostoc = null;

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            try
            {
                partida = articulo = "";
                if (detallebolsa == null)
                {
                    Util.EscribeLog("detallebolsa es null");
                }
                else
                {
                    Util.EscribeLog("detallebolsa " + detallebolsa.BODPIDDP.ToString() + " " + detallebolsa.PEBOLS.BOLSCOEM + " USCR " + detallebolsa.BODPUSCR);
                }

                using (var context = new PEDIDOSEntities())
                {
                    EFModelo.PEDEPE detpednac = null;
                    EFModelo.PEDEOS detpedint = null;
                    //inserta PBOLS si no existe
                    //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
                    var emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
                    Util.EscribeLog("1");

                    if (emp != null)
                    {
                        var bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);
                        Util.EscribeLog("2");

                        if (bol == null) //no existe pbols, insertar
                        {
                            bol = new EFModelo.PEBOLS();
                            //inserta = true;
                            bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
                            bol.BOLSUSCR = detallebolsa.BODPUSCR;
                            bol.BOLSFECR = DateTime.Now;
                            bol.BOLSCOCA = null;
                            bol.BOLSESTA = 1;
                            bol.BOLSALMA = emp.CAEMALMA;
                            bol.BOLSCOAR = "";
                            Util.EscribeLog("3");

                            context.PEBOLS.Add(bol);
                            Util.EscribeLog("4");

                            context.SaveChanges();
                            Util.EscribeLog("5");

                            detallebolsa.BODPIDBO = bol.BOLSIDBO;
                        }
                        else
                        {
                            if (bol.BOLSALMA == 0)
                            {
                                bol.BOLSALMA = emp.CAEMALMA;
                            }
                            detallebolsa.BODPIDBO = bol.BOLSIDBO;
                        }

                        if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                        {
                            //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
                        }
                        //bool inserta = false;
                        //
                        if (detallebolsa.BODPIDDP.HasValue)
                        {
                            detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
                            partida = detpednac.DEPEPART;
                            articulo = detpednac.DEPECOAR;
                        }
                        else if (detallebolsa.BODPIDDO.HasValue)
                        {
                            detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                            partida = detpedint.DEOSPART;
                            articulo = detpedint.DEOSCOAR;
                        }

                        //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
                        var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

                        if (ent == null) //detallebolsa.BODPIDDE != 0)
                        {
                            ent = new EFModelo.PEBODP();
                            //inserta = true;
                            ent.BODPUSCR = detallebolsa.BODPUSCR;
                            ent.BODPFECR = DateTime.Now;
                            context.PEBODP.Add(ent);
                            //inserta tipo 1 SALIDA
                            insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                        }
                        else
                        {
                            ent.BODPUSMO = detallebolsa.BODPUSCR;
                            ent.BODPFEMO = DateTime.Now;
                            ent.BODPESTA = 3;
                            //SOLO si las cantidades o pesos son diferentes
                            //inserta tipo 3 reingreso
                            //inserta tipo 1 salida
                            if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
                            {
                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                            }
                        }
                        //automatizar el parse sin incluir la PK
                        ent.BODPIDBO = detallebolsa.BODPIDBO;
                        ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
                        ent.BODPALMA = bol.BOLSALMA;
                        ent.BODPPART = partida;
                        ent.BODPCOAR = articulo;
                        ent.BODPCANT = detallebolsa.BODPCANT;
                        ent.BODPPESO = detallebolsa.BODPPESO;
                        ent.BODPPERE = detallebolsa.BODPPERE;
                        ent.BODPDIFE = detallebolsa.BODPDIFE;
                        ent.BODPSTCE = detallebolsa.BODPSTCE;
                        ent.BODPINBO = detallebolsa.BODPINBO;
                        ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
                        //2018-04-11
                        ent.BODPTAUN = detallebolsa.BODPTAUN;
                        //iddetpedido = ent.BODPIDDP;
                        //iddetpedidoint = ent.BODPIDDO;
                        if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
                        {
                            iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
                        }

                        ent.BODPTADE = detallebolsa.BODPTADE;
                        ent.BODPPEBR = detallebolsa.BODPPEBR;
                        ent.BODPESTA = detallebolsa.BODPESTA;

                        context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
                        decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                        if (detallebolsa.BODPIDDP.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
                            // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //var detpednac = context.PEDEPE.Find(iddetpedido);
                            detpednac.DEPECAAT = cantatendida;
                            detpednac.DEPEPEAT = pesoatendido;
                            detpednac.DEPEPERE = pesoreal;
                            if (iddetpedidostoc.HasValue)
                            {
                                detpednac.DEPESTOC = iddetpedidostoc;
                            }
                            detpednac.DEPETADE = tade;
                            detpednac.DEPEPEBR = pebr;

                            context.SaveChanges();
                        }
                        else if (detallebolsa.BODPIDDO.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                            detpedint.DEOSCAAT = cantatendida;
                            detpedint.DEOSPEAT = pesoatendido;
                            detpedint.DEOSPERE = pesoreal;
                            if (iddetpedidostoc.HasValue)
                            {
                                detpedint.DEOSSTOC = iddetpedidostoc;
                            }
                            detpedint.DEOSTADE = tade;
                            detpedint.DEOSPEBR = pebr;
                            //actualiza peso en osa ---PENDIENTE EN AS
                            var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                            if (osa != null)
                            {
                                osa.OSASCAEN = pesoatendido;
                                actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
                            }
                            context.SaveChanges();
                        }
                        //actualiza el stock de la bolsa
                        var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                        //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                        cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                        foreach (var bolsaprep in todasbolsasprep)
                        {
                            cantatendida += bolsaprep.BODPCANT;
                            pesoatendido += bolsaprep.BODPPESO;
                            pesoreal += bolsaprep.BODPPERE;
                            tade += bolsaprep.BODPTADE;
                            pebr += bolsaprep.BODPPEBR;
                        }
                        var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                        if (detemp != null)
                        {
                            //---PENDIENTE EN AS
                            detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                            if (emp.CAEMMSPA == "+")
                            {
                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                            }
                            else
                            {
                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
                            }
                            if (iddetpedidostoc.HasValue)
                            {
                                detemp.DEEMSTCE = iddetpedidostoc.Value;
                            }
                            if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
                            {
                                detemp.DEEMESBO = 9;
                            }
                            else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                            {
                                detemp.DEEMESBO = 9;
                            }
                            else
                            {
                                detemp.DEEMESBO = 1;
                            }
                            actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
                            context.SaveChanges();
                            //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                            detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                            if (detemp == null)
                            {
                                bol.BOLSESTA = 9; //ya no se usa la bolsa
                            }
                            else
                            {
                                bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                            }
                            context.SaveChanges();
                        }
                        vpar.VALSAL = new List<string>();
                        vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                        vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                        resultado = ""; // ent.BODPIDDE.ToString();
                        vpar.ESTOPE = true;
                    }
                    else
                    {
                        resultado = "Código de empaque incorrecto";
                    }
                }
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Util.EscribeLog(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //            //System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                if (ex.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.StackTrace);
                }
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA

        //cambio 26 -04-2018
        public RESOPE guardaPreparacionBolsa2(appWcfService.DTO_PEBODP paramOperacion)
        {
            //detallebolsa
            List<appWcfService.PEBODP> listBolsas = null;
            //

            Nullable<decimal> iddetpedidostoc = null;

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            try
            {
                //cambios
                listBolsas = paramOperacion.Items;
                //

                partida = articulo = "";
                //if (listBolsas == null)//detallebolsa == null)
                //{
                //    Util.EscribeLog("detallebolsa es null");
                //}
                //else
                //{
                //    Util.EscribeLog("detallebolsa " + listBolsas[0].BODPIDDP.ToString() /*detallebolsa.BODPIDDP.ToString()*/ 
                //        + " " + listBolsas[0].PEBOLS.BOLSCOEM/*detallebolsa.PEBOLS.BOLSCOEM*/ + " USCR " + listBolsas[0].BODPUSCR/*detallebolsa.BODPUSCR*/);
                //}

                using (var context = new PEDIDOSEntities())
                {
                    foreach (var detallebolsa in listBolsas)
                    {
                        EFModelo.PEDEPE detpednac = null;
                        EFModelo.PEDEOS detpedint = null;
                        //inserta PBOLS si no existe
                        //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
                        var emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
                        Util.EscribeLog("1");

                        if (emp != null)
                        {
                            var bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);
                            Util.EscribeLog("2");

                            if (bol == null) //no existe pbols, insertar
                            {
                                bol = new EFModelo.PEBOLS();
                                //inserta = true;
                                bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
                                bol.BOLSUSCR = detallebolsa.BODPUSCR;
                                bol.BOLSFECR = DateTime.Now;
                                bol.BOLSCOCA = null;
                                bol.BOLSESTA = 1;
                                bol.BOLSALMA = emp.CAEMALMA;
                                bol.BOLSCOAR = "";
                                Util.EscribeLog("3");

                                context.PEBOLS.Add(bol);
                                Util.EscribeLog("4");

                                context.SaveChanges();
                                Util.EscribeLog("5");

                                detallebolsa.BODPIDBO = bol.BOLSIDBO;
                            }
                            else
                            {
                                if (bol.BOLSALMA == 0)
                                {
                                    bol.BOLSALMA = emp.CAEMALMA;
                                }
                            }

                            if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                            {
                                //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
                            }
                            //bool inserta = false;
                            //
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
                                partida = detpednac.DEPEPART;
                                articulo = detpednac.DEPECOAR;
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                                partida = detpedint.DEOSPART;
                                articulo = detpedint.DEOSCOAR;
                            }

                            //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
                            var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

                            if (ent == null) //detallebolsa.BODPIDDE != 0)
                            {
                                ent = new EFModelo.PEBODP();
                                //inserta = true;
                                ent.BODPUSCR = detallebolsa.BODPUSCR;
                                ent.BODPFECR = DateTime.Now;
                                context.PEBODP.Add(ent);
                                //inserta tipo 1 SALIDA
                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                            }
                            else
                            {
                                ent.BODPUSMO = detallebolsa.BODPUSCR;
                                ent.BODPFEMO = DateTime.Now;
                                ent.BODPESTA = 3;
                                //SOLO si las cantidades o pesos son diferentes
                                //inserta tipo 3 reingreso
                                //inserta tipo 1 salida
                                if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
                                {
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                }
                            }
                            //automatizar el parse sin incluir la PK
                            ent.BODPIDBO = detallebolsa.BODPIDBO;
                            ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
                            ent.BODPALMA = bol.BOLSALMA;
                            ent.BODPPART = partida;
                            ent.BODPCOAR = articulo;
                            ent.BODPCANT = detallebolsa.BODPCANT;
                            ent.BODPPESO = detallebolsa.BODPPESO;
                            ent.BODPPERE = detallebolsa.BODPPERE;
                            ent.BODPDIFE = detallebolsa.BODPDIFE;
                            ent.BODPSTCE = detallebolsa.BODPSTCE;
                            ent.BODPINBO = detallebolsa.BODPINBO;
                            ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
                                                                  //2018-04-11
                            ent.BODPTAUN = detallebolsa.BODPTAUN;
                            //iddetpedido = ent.BODPIDDP;
                            //iddetpedidoint = ent.BODPIDDO;
                            if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
                            {
                                iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
                            }

                            ent.BODPTADE = detallebolsa.BODPTADE;
                            ent.BODPPEBR = detallebolsa.BODPPEBR;
                            ent.BODPESTA = detallebolsa.BODPESTA;

                            context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
                            decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
                                // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpednac = context.PEDEPE.Find(iddetpedido);
                                detpednac.DEPECAAT = cantatendida;
                                detpednac.DEPEPEAT = pesoatendido;
                                detpednac.DEPEPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpednac.DEPESTOC = iddetpedidostoc;
                                }
                                detpednac.DEPETADE = tade;
                                detpednac.DEPEPEBR = pebr;

                                context.SaveChanges();
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                                detpedint.DEOSCAAT = cantatendida;
                                detpedint.DEOSPEAT = pesoatendido;
                                detpedint.DEOSPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpedint.DEOSSTOC = iddetpedidostoc;
                                }
                                detpedint.DEOSTADE = tade;
                                detpedint.DEOSPEBR = pebr;
                                //actualiza peso en osa ---PENDIENTE EN AS
                                var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                                if (osa != null)
                                {
                                    osa.OSASCAEN = pesoatendido;
                                    actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
                                }
                                context.SaveChanges();
                            }
                            //actualiza el stock de la bolsa
                            var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in todasbolsasprep)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                            if (detemp != null)
                            {
                                //---PENDIENTE EN AS
                                detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                if (emp.CAEMMSPA == "+")
                                {
                                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                }
                                else
                                {
                                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
                                }
                                if (iddetpedidostoc.HasValue)
                                {
                                    detemp.DEEMSTCE = iddetpedidostoc.Value;
                                }
                                if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
                                context.SaveChanges();
                                //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                if (detemp == null)
                                {
                                    bol.BOLSESTA = 9; //ya no se usa la bolsa
                                }
                                else
                                {
                                    bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                }
                                context.SaveChanges();
                            }
                            //vpar.VALSAL = new List<string>();
                            ////vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                            ////vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                            //resultado = ""; // ent.BODPIDDE.ToString();
                            //vpar.ESTOPE = true;
                        }
                        else
                        {
                            resultado = "Código de empaque incorrecto";
                        }
                    }
                }
                vpar.VALSAL = new List<string>();
                //vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                //vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                resultado = ""; // ent.BODPIDDE.ToString();
                vpar.ESTOPE = true;
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Util.EscribeLog(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //            //System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                if (ex.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.StackTrace);
                }
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA
        //cambio 26 -04-2018




        public RESOPE remueveBolsaPedido(decimal idbolsapedido, string usuario)
        {
            Nullable<decimal> iddetpedido;
            Nullable<decimal> iddetpedidoint;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            try
            {
                partida = articulo = "";
                using (var context = new PEDIDOSEntities())
                {
                    EFModelo.PEDEPE detpednac = null;
                    EFModelo.PEDEOS detpedint = null;

                    //bool inserta = false;
                    var ent = context.PEBODP.Find(idbolsapedido);
                    if (ent != null) //detallebolsa.BODPIDDE != 0)
                    {
                        var bol = context.PEBOLS.Find(ent.BODPIDBO);
                        if (ent.BODPIDDP.HasValue)
                        {
                            detpednac = context.PEDEPE.Find(ent.BODPIDDP);
                            partida = detpednac.DEPEPART;
                            articulo = detpednac.DEPECOAR;
                        }
                        else if (ent.BODPIDDO.HasValue)
                        {
                            detpedint = context.PEDEOS.Find(ent.BODPIDDO);
                            partida = detpedint.DEOSPART;
                            articulo = detpedint.DEOSCOAR;
                        }
                        iddetpedido = ent.BODPIDDP;
                        iddetpedidoint = ent.BODPIDDO;
                        //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                        //{
                        //bol.BOLSESTA = 1; //ya no se usa
                        //}
                        insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
                        context.PEBODP.Remove(ent);
                        context.SaveChanges();
                        decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

                        if (iddetpedido.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //detpednac = context.PEDEPE.Find(iddetpedido);
                            detpednac.DEPECAAT = cantatendida;
                            detpednac.DEPEPEAT = pesoatendido;
                            detpednac.DEPEPERE = pesoreal;
                            detpednac.DEPETADE = tade;
                            detpednac.DEPEPEBR = pebr;

                            //inserta tipo 2 kardex reingreso
                            context.SaveChanges();
                        }
                        else if (iddetpedidoint.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            detpedint.DEOSCAAT = cantatendida;
                            detpedint.DEOSPEAT = pesoatendido;
                            detpedint.DEOSPERE = pesoreal;
                            detpedint.DEOSTADE = tade;
                            detpedint.DEOSPEBR = pebr;
                            //actualiza peso en osa ---PENDIENTE EN AS
                            var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                            if (osa != null)
                            {
                                osa.OSASCAEN = pesoatendido;
                                actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar AS
                            }
                            context.SaveChanges();

                        }
                        //actualiza el stock de la bolsa
                        var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                        //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                        cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                        foreach (var bolsaprep in todasbolsasprep)
                        {
                            cantatendida += bolsaprep.BODPCANT;
                            pesoatendido += bolsaprep.BODPPESO;
                            pesoreal += bolsaprep.BODPPERE;
                            tade += bolsaprep.BODPTADE;
                            pebr += bolsaprep.BODPPEBR;
                        }
                        var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                        if (detemp != null)
                        {
                            detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                            detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                            //si la bolsa preparada estaba marcada como stock cero, lo desmarco
                            if (ent.BODPSTCE == 1)
                            {
                                detemp.DEEMSTCE = 0;
                            }
                            if (detemp.DEEMSTCE == 0)
                            {
                                detemp.DEEMESBO = 1;
                            }
                            else
                            if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                            {
                                detemp.DEEMESBO = 9;
                            }
                            else
                            {
                                detemp.DEEMESBO = 1;
                            }
                            context.SaveChanges();
                            actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); //Validar AS
                            //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                            detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                            if (detemp == null)
                            {
                                bol.BOLSESTA = 9; //ya no se usa la bolsa
                            }
                            else
                            {
                                bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                            }
                            context.SaveChanges();
                        }
                    }
                    vpar.ESTOPE = true;
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA

        //public string cambiaEstadoPedidodic(Dictionary<string, string> estadopedido)
        //{
        //    string resultado = "";
        //    try
        //    {
        //        Util.EscribeLog("SI ENTRA DICCIONARY");
        //        //if (estadopedido == null) { 
        //        //    Util.EscribeLog("estadopedido es null");
        //        //}
        //        //else
        //        //{
        //        //    Util.EscribeLog("estadopedido " + estadopedido.ESPEIDPE.ToString() + " " + estadopedido.ESPEIDES.ToString());
        //        //}

        //        using (var context = new PEDIDOSEntities())
        //        {
        //            var ent = context.PECAPE.Find(Convert.ToDecimal(estadopedido["ESPEIDPE"]));
        //            ent.CAPEIDES = Convert.ToDecimal(estadopedido["ESPEIDES"]);
        //            context.SaveChanges();
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
        //    return resultado;
        //}

        public List<PEUBIC> mostrarUbicacionesArticulo(string articulo, string partida, decimal idalmacen)
        {
            List<object> listaeo = null;
            List<appWcfService.PEUBIC> lista = null;
            try
            {
                    using (var context = new PEDIDOSEntities())
                    {
                        listaeo = context.USP_OBTIENE_UBICACIONES(articulo, partida, idalmacen).ToList<object>();
                    }
                lista = Util.ParseEntityObject<appWcfService.PEUBIC>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//LISTO

        public List<appWcfService.USP_OBTIENE_BOLSA_Result> obtieneBolsa(decimal iddetalle, string empaque)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_BOLSA_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_BOLSA(iddetalle, empaque).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_BOLSA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        //public List<appWcfService.PEBODP> obtieneDetallePreparacionPedidos(decimal iddetallepedido)
        //{
        //    List<object> listaeo = null;
        //    List<appWcfService.PEBODP> lista = null;
        //    try
        //    {
        //        using (var context = new PEDIDOSEntities())
        //        {
        //            listaeo = context.PEBODP.Include("PEBOLS").Include("PEDEPE").Where(ped => ped.BODPIDDP == iddetallepedido).ToList<object>();
        //        }
        //        lista = Util.ParseEntityObject<appWcfService.PEBODP>(listaeo);
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.EscribeLog(ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return lista;
        //}

        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> obtieneDetallePreparacionPedidos(decimal iddetallepedido)//NO SE USA
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE(iddetallepedido).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }
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


        #region PEDIDOS INTERNOS
        public List<appWcfService.PETIFO> obtieneTiposFolio()
        {
            List<object> listaeo = null;
            List<appWcfService.PETIFO> lista = null;
            try
            {
                using (var sqlLogFile = new StreamWriter("D:\\scriptspedidos\\sqlLogFile.txt"))
                {
                    using (var context = new PEDIDOSEntities())
                    {
                        context.Database.Log = sqlLogFile.Write; // Console.Write;
                        listaeo = context.PETIFO.ToList<object>();
                    }
                }
                
                lista = Util.ParseEntityObject<appWcfService.PETIFO>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//LISTO

        public List<appWcfService.USP_FOLIO_USUARIO_Result> obtieneFoliosUsuario(string usuario)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_FOLIO_USUARIO_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    //listaeo = context.PEUSTF.Include("PETIFO").Where(a => a.USTFUSUA == usuario).ToList<object>();
                    listaeo = context.USP_FOLIO_USUARIO(usuario).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_FOLIO_USUARIO_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//LISTO

        public List<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_Result> mostrarPedidosInternos(string tipofolios, string partida = "")//cambiar proc
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_Result> lista = null;
            try
            {
                //string[] folios = tipofolios.Split();
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_OSAS_PENDIENTES(tipofolios, partida).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }

        public List<appWcfService.USP_OBTIENE_DETALLE_OSA_Result> mostrarDetallePedidosInternos(string folio)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETALLE_OSA_Result> lista = null;
            try
            {
                //string[] folios = tipofolios.Split();
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETALLE_OSA(folio).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_OSA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public RESOPE cambiaEstadoPedInt(appWcfService.PECAOS estadopedint)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            bool nuevoregistro = false;

            string resultado = "";
            try
            {
                    using (var context = new PEDIDOSEntities())
                    {
                        var ent = context.PECAOS.Find(Convert.ToDecimal(estadopedint.CAOSIDCO));
                        //var ent = context.PECAOS.FirstOrDefault(x=> x.CAOSIDCO == estadopedint.CAOSIDCO);

                        if (ent == null)
                        {
                            ent = new EFModelo.PECAOS();
                            ent.CAOSFOLI = estadopedint.CAOSFOLI;
                            ent.CAOSPRIO = 0;
                            ent.CAOSFECR = DateTime.Now;
                            ent.CAOSUSCR = estadopedint.CAOSUSCR;
                            context.PECAOS.Add(ent);
                            nuevoregistro = true;
                        }
                        else
                        {
                            if (!ent.CAOSFOLI.Trim().Equals(estadopedint.CAOSFOLI))
                            {
                                throw new Exception("Folio no válido");
                            }
                            ent.CAOSFEMO = DateTime.Now;
                            ent.CAOSUSMO = estadopedint.CAOSUSCR;
                        }
                        //ent.CAOSIDES = estadopedint.CAOSIDES;
                        ent.CAOSIDES = estadopedint.CAOSIDES; //probar para la transacción
                        if (estadopedint.CAOSIDES == 5 || estadopedint.CAOSIDES == 4) // se completo todas los detalles de ese folio
                        {
                            ent.CAOSFHFP = DateTime.Now;
                            ent.CAOSUSFP = estadopedint.CAOSUSCR;
                            ent.CAOSNOTA = estadopedint.CAOSNOTA;
                        }
                        if (ent.CAOSFHIP == null && ent.CAOSIDES == 3)
                        {
                            ent.CAOSFHIP = DateTime.Now;
                            ent.CAOSUSIP = estadopedint.CAOSUSCR;
                        }
                        context.SaveChanges();
                        //actualiza estado OSA en SQL --falta actualizar estado OSA en AS
                        var osas = context.PROSAS.Where(ped => ped.OSASCIA == 1 && ped.OSASFOLI == estadopedint.CAOSFOLI).ToList();
                        if (osas != null)
                        {
                            //eliiminar de pedeos todos los est de prosass E
                            foreach (var osa in osas)
                            {
                                var detped = context.PEDEOS.FirstOrDefault(det => det.DEOSFOLI == osa.OSASFOLI && det.DEOSSECU == osa.OSASSECU);
                                if (detped == null)
                                {
                                    //si llegaran a agregar lineas despues de emitida no funcionaría, peor si eliminan
                                    detped = new EFModelo.PEDEOS();
                                    detped.DEOSIDCO = ent.CAOSIDCO;
                                    detped.DEOSSECU = osa.OSASSECU;
                                    detped.DEOSFOLI = osa.OSASFOLI;
                                    detped.DEOSPEAT = 0;
                                    detped.DEOSCAAT = 0;
                                    detped.DEOSPERE = 0;
                                    detped.DEOSSTOC = 0;
                                    detped.DEOSESTA = 1; //CREADO //NO SE USA POR AHORA
                                    detped.DEOSFECR = DateTime.Now;
                                    detped.DEOSUSCR = estadopedint.CAOSUSCR;
                                    context.PEDEOS.Add(detped);
                                }
                                if (detped.DEOSCOAR != osa.OSASARTI || detped.DEOSPART != osa.OSASPAOR || detped.DEOSALMA != osa.OSASALMA || detped.DEOSPESO != osa.OSASCASO)
                                {
                                    detped.DEOSFEMO = DateTime.Now;
                                    detped.DEOSUSMO = estadopedint.CAOSUSCR;
                                }
                                detped.DEOSCOAR = osa.OSASARTI;
                                detped.DEOSPART = osa.OSASPAOR;
                                detped.DEOSALMA = osa.OSASALMA;
                                detped.DEOSPESO = osa.OSASCASO;

                                if (estadopedint.CAOSIDES == 3)  //3   En preparación                                    
                                {
                                    //actualizar al guardar bolsa preparada o al trabajar item?
                                    //if (osa.OSASSTOS.Equals("E"))
                                    //{
                                    //    osa.OSASSTOS = "S";
                                    //    //actualizar estado PENDIENTE AS
                                    //}
                                    //actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, -1, osa.OSASSTOS); //Pendiente Validar AS

                                }
                                else if (estadopedint.CAOSIDES == 4 || estadopedint.CAOSIDES == 5)  //4   preparación finalizada
                                {
                                    if (osa.OSASCASO > 0 && detped.DEOSESPA == 0 && !osa.OSASSTOS.Equals("T")) //osa.OSASSTOS.Equals("S") && .DEOSPEAT != 0
                                    {
                                        if (detped.DEOSPEAT == 0)
                                        {
                                            osa.OSASSTOS = "T";
                                        }
                                        else
                                        {
                                            osa.OSASSTOS = "C";
                                        }
                                        //actualizar estado PENDIENTE AS
                                        actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, -1, osa.OSASSTOS); //Pendiente verficar si es correcto.
                                    }
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                vpar.ESTOPE = true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.InnerException.Message);
                    resultado += ex.InnerException.InnerException.Message;
                }
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }///REVISAR LLAMA A UNA FUNCION

        public RESOPE actualizaPreparacionItemOSA(appWcfService.PEDEOS detpedint)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            try
            {

                using (var context = new PEDIDOSEntities())
                {
                    var osa = context.PROSAS.FirstOrDefault(ped => ped.OSASCIA == 1 && ped.OSASFOLI == detpedint.DEOSFOLI && ped.OSASSECU == detpedint.DEOSSECU);
                    if (osa != null)
                    {
                        //actualizar al guardar bolsa preparada o al trabajar item?
                        if (osa.OSASSTOS.Equals("E") || osa.OSASSTOS.Equals("P"))
                        {
                            osa.OSASSTOS = "S";
                            //actualizar estado PENDIENTE AS
                            actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, -1, osa.OSASSTOS);
                        }
                    }
                    context.SaveChanges();
                }
                vpar.ESTOPE = true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.InnerException.Message);
                    resultado += ex.InnerException.InnerException.Message;
                }
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//se usa no tocar

        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result> obtieneDetallePreparacionPedInt(decimal iddetallepedint)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA(iddetallepedint).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//NO SE USA

        public List<appWcfService.USP_OBTIENE_BOLSA_OSA_Result> obtieneBolsaOsa(decimal iddetalle, string empaque)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_BOLSA_OSA_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_BOLSA_OSA(iddetalle, empaque).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_BOLSA_OSA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc
        #endregion

        #region ubicaciones
        public List<appWcfService.USP_OBTIENE_BOLSA_UBICACION_Result> obtieneBolsaUbicacion(string empaque)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_BOLSA_UBICACION_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_BOLSA_UBICACION(empaque).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_BOLSA_UBICACION_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public List<appWcfService.USP_CONSULTA_EMPAQUES_PARTIDA_Result> consultaEmpaquesPartida(string partida, string articulo, string empaque)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_CONSULTA_EMPAQUES_PARTIDA_Result> lista = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(partida) || !string.IsNullOrWhiteSpace(articulo) || !string.IsNullOrWhiteSpace(empaque))
                {
                    if (string.IsNullOrWhiteSpace(partida))
                    {
                        partida = null;
                    }
                    if (string.IsNullOrWhiteSpace(articulo))
                    {
                        articulo = null;
                    }
                    if (string.IsNullOrWhiteSpace(empaque))
                    {
                        empaque = null;
                    }
                    using (var context = new PEDIDOSEntities())
                    {
                        listaeo = context.USP_CONSULTA_EMPAQUES_PARTIDA(partida, articulo, empaque).ToList<object>();
                    }
                    lista = Util.ParseEntityObject<appWcfService.USP_CONSULTA_EMPAQUES_PARTIDA_Result>(listaeo);
                }
                else
                {
                    lista = new List<appWcfService.USP_CONSULTA_EMPAQUES_PARTIDA_Result>();
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public List<appWcfService.USP_OBTIENE_DETALLE_BOLSA_Result> obtieneDetalleBolsa(string empaque)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETALLE_BOLSA_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETALLE_BOLSA(empaque).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_BOLSA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public RESOPE guardaBolsaUbicacion(appWcfService.PEBOLS bolsa)
        {

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            try
            {
                using (var sqlLogFile = new StreamWriter("D:\\scriptspedidos\\sqlLogFile.txt"))
                {
                    using (var context = new PEDIDOSEntities())
                    {
                        context.Database.Log = sqlLogFile.Write; // Console.Write;
                        var casi = context.PECASI.Find(bolsa.BOLSCOCA);
                        if (casi == null)
                        {
                            vpar.ESTOPE = false;
                            resultado = "Ubicación incorrecta.";
                        }
                        else
                        {
                            var emp = context.GMCAEM.First(b => b.CAEMCIA == 1 && b.CAEMCOEM == bolsa.BOLSCOEM);
                            if (emp != null)
                            {
                                //bool inserta = false;
                                var bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == bolsa.BOLSCOEM); //.Find(bolsa.BOLSIDBO);
                                if (bol == null) //no existe pbols, insertar
                                {
                                    bol = new EFModelo.PEBOLS();
                                    //inserta = true;
                                    bol.BOLSCOEM = bolsa.BOLSCOEM;
                                    bol.BOLSUSCR = bolsa.BOLSUSCR;
                                    bol.BOLSFECR = DateTime.Now;
                                    bol.BOLSESTA = 1;
                                    bol.BOLSALMA = emp.CAEMALMA;
                                    bol.BOLSCOAR = "";
                                    context.PEBOLS.Add(bol);
                                    //inserta tipo 1 SALIDA
                                    //insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                }
                                else
                                {
                                    bol.BOLSUSMO = bolsa.BOLSUSCR;
                                    bol.BOLSFEMO = DateTime.Now;
                                    //SOLO si las cantidades o pesos son diferentes
                                    //inserta tipo 3 reingreso
                                    //inserta tipo 1 salida
                                    if (bol.BOLSALMA == 0)
                                    {
                                        bol.BOLSALMA = emp.CAEMALMA;
                                    }

                                }
                                //if (!bol.BOLSCOEM.Equals(bolsa.BOLSCOEM))
                                //{
                                //    throw new Exception("Código Empaque no válido");
                                //}
                                //automatizar el parse sin incluir la PK
                                bol.BOLSCOCA = bolsa.BOLSCOCA;
                                bol.BOLSUSUB = bolsa.BOLSUSCR;
                                bol.BOLSFEUB = DateTime.Now;

                                //bol.BOLSESTA = 1;
                                context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD

                                vpar.VALSAL = new List<string>();
                                vpar.VALSAL.Add(bol.BOLSIDBO.ToString());

                                vpar.ESTOPE = true;
                            }
                            else
                            {
                                resultado = "Código de empaque incorrecto";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//dd

        #endregion

        #region recepcion osa
        public List<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result> mostrarPedidosIntPendRecepcion(string tipofolios, string numfolio)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result> lista = null;
            try
            {
                //string[] folios = tipofolios.Split();
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_OSAS_PENDIENTES_PLANTA(tipofolios, numfolio).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result> mostrarDetallePedidosIntPendRecepcion(string folio)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result> lista = null;
            try
            {
                //string[] folios = tipofolios.Split();
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETALLE_OSA_PLANTA(folio).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public RESOPE conformidadRecepcionOsa(appWcfService.DET_USP_OBTIENE_DETALLE_OSA_PLANTA_Result objetoLista)
        {
            //Recibe una lista
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            //List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result> Listadetalles = new List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result>();
            string usuario;
            appLogica.appDB2 _appDB2 = null;

            try
            {
                //Util.EscribeLog("ENTRO APRUEBAOSAPLANTA");
                List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result> Listadetalles = objetoLista.Items;

                if (Listadetalles == null || Listadetalles.Count == 0)
                {
                    Util.EscribeLog("ARGUMENTO NULO, ITEM NO RECIBIDOS");
                    vpar.MENERR = "ITEMS NO RECIBIDOS";
                    return vpar;
                }
                _appDB2 = new appLogica.appDB2();

                //Listadetalles = Util.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_OSA_PLANTA_Result>>(paramOperacion.VALENT[0]);
                usuario = Listadetalles[0].DEOSUSMO;  // paramOperacion.VALENT[1];
                using (var context = new PEDIDOSEntities())
                {
                    int secuencia = 0;
                    foreach (var item in Listadetalles)
                    {
                        decimal cantentreg = 0;
                        var osaplanta = context.PROSAS.SingleOrDefault(o => o.OSASCIA == item.OSASCIA && o.OSASFOLI == item.OSASFOLI && o.OSASSECU == item.OSASSECU && o.OSASSTOS == item.OSASSTOS);
                        if (osaplanta != null)
                        {
                            if (item.DEOSESPA == 1)
                            {
                                if (osaplanta.OSASSTOS != "E")
                                {
                                    osaplanta.OSASSTOS = "P";
                                }
                            }
                            else
                            {
                                osaplanta.OSASSTOS = "T";
                            }
                            //PENDIENTE AS
                            //20180424
                            //actualizaPROSAS(osaplanta.OSASFOLI, osaplanta.OSASSECU, -1, osaplanta.OSASSTOS); // VALIDAR AS
                            //osaplanta.OSASFEAT = DateTime.Today;
                            var pecaos = context.PECAOS.SingleOrDefault(p => p.CAOSFOLI == item.OSASFOLI && p.CAOSIDCO == item.DEOSIDCO);
                            osaplanta.OSASFEAT = pecaos.CAOSFHFP.Value.Date; // : DateTime.Today;
                            actualizaPROSAS(osaplanta.OSASFOLI, osaplanta.OSASSECU, osaplanta.OSASSTOS, osaplanta.OSASFEAT); // VALIDAR AS

                            //Obtenemos las bolsas
                            var bolsas = context.PEBODP.Where(dp => dp.BODPIDDO == item.DEOSIDDO).ToList();
                            if (bolsas != null)
                            {
                                foreach (var bol in bolsas)
                                {
                                    if (bol.BODPESTA == 4)
                                    {
                                        bol.BODPESTA = 5;
                                    }
                                }
                            }
                            secuencia++;
                            ////Descuento de inventario
                            //20180711
                            //cantentreg = osaplanta.OSASCAEN;
                            cantentreg = 0;
                            if (item.DEOSPEAT.HasValue)
                            {
                                cantentreg = item.DEOSPEAT.Value;
                            }

                            if (item.DEOSPEOR.HasValue)
                            {
                                cantentreg -= item.DEOSPEOR.Value;
                            }
                            if (cantentreg > 0)
                            {
                                //2018/02/05
                                _appDB2.descargaOSA(false, osaplanta.OSASFOLI, secuencia, osaplanta.OSASFEAT, osaplanta.OSASARTI, osaplanta.OSASPAOR, osaplanta.OSASALMA, osaplanta.OSASPADE, Convert.ToInt32(osaplanta.OSASCCOS).ToString(), "-", cantentreg); //consumo es negativo //extorno y devol es +
                            }

                            //20180424
                            //Actualiza el estado a la la cabecera en nuestra base de datos 
                            //var pecaos = context.PECAOS.SingleOrDefault(p => p.CAOSFOLI == item.OSASFOLI && p.CAOSIDCO == item.DEOSIDCO);
                            if (pecaos.CAOSIDES == 4)
                            {
                                //pecaos.CAOSIDES = 3; //cambiar a 3 para preparación
                                pecaos.CAOSUSAP = usuario;
                                pecaos.CAOSFEAP = DateTime.Now;

                            }
                            else if (pecaos.CAOSIDES == 5)
                            {
                                pecaos.CAOSIDES = 7; // Completado totalmente
                                pecaos.CAOSUSAP = usuario;
                                pecaos.CAOSFEAP = DateTime.Now;
                            }
                            pecaos.CAOSEXTO = 1; //lo habilita para poder extornarlo luego
                            var deos = context.PEDEOS.Find(item.DEOSIDDO);
                            if (deos != null)
                            {
                                if (deos.DEOSESTA == 6) //es parcial estado del detalle
                                {
                                    //deos.DEOSESTA = 3; //mantenemos en parcial
                                }
                                else // 5 significa completo 
                                {
                                    deos.DEOSESTA = 7;
                                }
                                deos.DEOSUSMO = usuario;
                                deos.DEOSFEMO = DateTime.Now;
                            }
                            context.SaveChanges();
                        }
                    }
                    vpar.ESTOPE = true;
                }

                vpar.VALSAL = new List<string>();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                vpar.MENERR = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                vpar.MENERR = ErrorGenerico(ex.Message);
            }
            finally
            {
                if (_appDB2 != null)
                {
                    _appDB2.Finaliza();
                    _appDB2 = null;
                }
            }
            return vpar;
        }//REVISAR TIENE LLAMADO A DB2

        public RESOPE cambiaestaDeosBodp(DTO_USP_OBTIENE_DETALLE_OSA_Result paramOperacion)
        {
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };
            List<appWcfService.USP_OBTIENE_DETALLE_OSA_Result> listdetosas = null;
            List<EFModelo.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result> listaeo = null;
            decimal estado;
            string usuario;
            try
            {
                listdetosas = paramOperacion.Items;  //Util.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_OSA_Result>>(paramOperacion.VALENT[0]);
                estado = paramOperacion.ESTADO;
                usuario = paramOperacion.USUARIO;

                foreach (var item in listdetosas)
                {
                        using (var context = new PEDIDOSEntities())
                        {
                            var deos = context.PEDEOS.Find(item.DEOSIDDO);
                            if (deos != null)
                            {
                                //deos.DEOSESTA = item.DEOSESTA; //20180420
                                var caos = context.PECAOS.Find(item.DEOSIDCO);

                                switch (Int32.Parse(item.DEOSESTA.ToString()))
                                {
                                    case 1:
                                        deos.DEOSESPA = item.DEOSESPA.Value;
                                        break;
                                    case 3:
                                        if (deos.DEOSESTA != 3 && deos.DEOSESTA != 7) //PODRIA SER 2 o 6 //7 para que no se generen errores de no descarga //20181004
                                        {
                                            //20180713 cambiar el peso origen, no usar el valor de peso atendido, en su lugar usar cant entregada de la OSA
                                            var osa = context.PROSAS.FirstOrDefault(o => o.OSASCIA == 1 && o.OSASFOLI == deos.DEOSFOLI && o.OSASSECU == deos.DEOSSECU);

                                            //20180420 SI SE IMPLEMENTA REABRIR DE EN PREPARACION A EMITIDO DEBE HACERSE EL PROCESO INVERSO
                                            deos.DEOSCAOR = item.DEOSCAAT.Value;
                                            if (osa == null)
                                            {
                                                //como estaba antes, no tendria porque no ocurrir
                                                deos.DEOSPEOR = item.DEOSPEAT.Value;
                                            }
                                            else
                                            {
                                                deos.DEOSPEOR = osa.OSASCAEN;
                                            }
                                        }
                                        deos.DEOSESTA = item.DEOSESTA;

                                        break;
                                    case 6: //FINALIZAR PREPARACION
                                        deos.DEOSESPA = 1;
                                        if (deos.DEOSPEAT >= deos.DEOSPESO) //20180425 peso atendido mayor igual a peso solicitado
                                        {
                                            deos.DEOSESPA = 0;
                                        }
                                        //20180420
                                        //deos.DEOSCAOR = item.DEOSCAAT.Value;
                                        //deos.DEOSPEOR = item.DEOSPEAT.Value;
                                        deos.DEOSESTA = item.DEOSESTA;

                                        if (estado == 4)
                                        {
                                            deos.DEOSSECR = deos.DEOSSECR + 1;
                                        }
                                        break;
                                    case 5: //FINALIZAR PREPARACION
                                        deos.DEOSESTA = item.DEOSESTA;
                                        deos.DEOSESPA = 0;
                                        if (estado == 4)
                                        {
                                            deos.DEOSSECR = deos.DEOSSECR + 1;
                                        }
                                        //caos.CAOSFHFP = DateTime.Now;
                                        //caos.CAOSUSFP = usuario;
                                        break;
                                    default:
                                        deos.DEOSESTA = item.DEOSESTA;
                                        break;
                                }
                                //////
                                listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA(item.DEOSIDDO).ToList();
                                //lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result>(listaeo);
                                ////
                                int estacambia = 0;
                                decimal secuencia = 0;
                                switch (Int32.Parse(estado.ToString())) //QUE ES ESTADO????
                                {
                                    case 4:
                                        estacambia = 3;
                                        secuencia = 1;
                                        break;
                                    case 3:
                                        estacambia = 4;
                                        break;
                                }
                                ////

                                foreach (var item1 in listaeo)
                                {
                                    if (estacambia != 0)
                                    {
                                        var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == item1.BODPIDDE);
                                        if (ent != null)
                                        {
                                            if (ent.BODPESTA == estacambia) // || ent.BODPESTA == estacreado
                                            {
                                                ent.BODPESTA = estado;
                                                ent.BODPFEMO = DateTime.Now;
                                                ent.BODPUSMO = usuario;
                                            }
                                            ent.BODPSECR = ent.BODPSECR + secuencia;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                                //////
                                context.SaveChanges();
                            }
                        }
                }
                vpar.VALSAL = new List<string>();
                vpar.ESTOPE = true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                vpar.MENERR = innerEx.Message;
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
        }///REVISAR LLAMA A UNA FUNCION

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

        private void actualizaPROSAS(string folio, decimal secuencia, string estado, DateTime fechaatencion)
        {
            appLogica.appDB2 _appDB2 = null;
            try
            {
                _appDB2 = new appLogica.appDB2();
                _appDB2.actualizaPROSAS(folio, secuencia, estado, fechaatencion);
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

        private void actualizaGMCAEM(string empaque, string estado)
        {
            appLogica.appDB2 _appDB2 = null;
            try
            {
                _appDB2 = new appLogica.appDB2();
                _appDB2.actualizaGMCAEM(empaque, estado);
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

        #endregion

        #region SIN EMPAQUE

        public RESOPE guardaPreparacionBolsase(appWcfService.PEBODP detallebolsa)// NO SE USA
        {
            Nullable<decimal> iddetpedidostoc = null;

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            decimal almacen;
            try
            {
                partida = articulo = "";
                almacen = 0;

                using (var context = new PEDIDOSEntities())
                {
                    EFModelo.PEDEPE detpednac = null;
                    EFModelo.PEDEOS detpedint = null;
                    EFModelo.GMCAEM emp = null;
                    bool sinempaque = string.IsNullOrWhiteSpace(detallebolsa.PEBOLS.BOLSCOEM);
                    if (!sinempaque)
                    {
                        emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
                    }

                    if (emp != null || sinempaque)
                    {
                        EFModelo.PEBOLS bol = null;
                        if (!sinempaque)
                        {
                            bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);

                            if (bol == null) //no existe pbols, insertar
                            {
                                bol = new EFModelo.PEBOLS();
                                //inserta = true;
                                bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
                                bol.BOLSUSCR = detallebolsa.BODPUSCR;
                                bol.BOLSFECR = DateTime.Now;
                                bol.BOLSCOCA = null;
                                bol.BOLSESTA = 1;
                                bol.BOLSALMA = emp.CAEMALMA;
                                bol.BOLSCOAR = "";
                                Util.EscribeLog("3");

                                context.PEBOLS.Add(bol);
                                Util.EscribeLog("4");

                                context.SaveChanges();
                                Util.EscribeLog("5");

                                detallebolsa.BODPIDBO = bol.BOLSIDBO;
                            }
                            else
                            {
                                if (bol.BOLSALMA == 0)
                                {
                                    bol.BOLSALMA = emp.CAEMALMA;
                                }
                                detallebolsa.BODPIDBO = bol.BOLSIDBO;
                            }
                        }

                        if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                        {
                            //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
                        }
                        //bool inserta = false;
                        //
                        if (detallebolsa.BODPIDDP.HasValue)
                        {
                            detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
                            partida = detpednac.DEPEPART;
                            articulo = detpednac.DEPECOAR;
                            almacen = detpednac.DEPEALMA;
                        }
                        else if (detallebolsa.BODPIDDO.HasValue)
                        {
                            detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                            partida = detpedint.DEOSPART;
                            articulo = detpedint.DEOSCOAR;
                            almacen = detpedint.DEOSALMA;
                        }

                        //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
                        var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

                        if (ent == null) //detallebolsa.BODPIDDE != 0)
                        {
                            ent = new EFModelo.PEBODP();
                            //inserta = true;
                            ent.BODPUSCR = detallebolsa.BODPUSCR;
                            ent.BODPFECR = DateTime.Now;
                            context.PEBODP.Add(ent);
                            if (!sinempaque)
                            {
                                //inserta tipo 1 SALIDA
                                insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                            }
                        }
                        else
                        {
                            ent.BODPUSMO = detallebolsa.BODPUSCR;
                            ent.BODPFEMO = DateTime.Now;
                            ent.BODPESTA = 3;
                            if (!sinempaque)
                            {
                                //SOLO si las cantidades o pesos son diferentes
                                //inserta tipo 3 reingreso
                                //inserta tipo 1 salida
                                if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
                                {
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                }
                            }
                        }
                        //automatizar el parse sin incluir la PK
                        ent.BODPIDBO = detallebolsa.BODPIDBO;
                        ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
                        ent.BODPALMA = almacen; // bol.BOLSALMA;
                        ent.BODPPART = partida;
                        ent.BODPCOAR = articulo;
                        ent.BODPCANT = detallebolsa.BODPCANT;
                        ent.BODPPESO = detallebolsa.BODPPESO;
                        ent.BODPPERE = detallebolsa.BODPPERE;
                        ent.BODPDIFE = detallebolsa.BODPDIFE;
                        ent.BODPSTCE = detallebolsa.BODPSTCE;
                        ent.BODPINBO = detallebolsa.BODPINBO;
                        ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
                        //2018-04-11
                        ent.BODPTAUN = detallebolsa.BODPTAUN;
                        //iddetpedido = ent.BODPIDDP;
                        //iddetpedidoint = ent.BODPIDDO;
                        if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
                        {
                            iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
                        }

                        ent.BODPTADE = detallebolsa.BODPTADE;
                        ent.BODPPEBR = detallebolsa.BODPPEBR;
                        ent.BODPESTA = detallebolsa.BODPESTA;

                        context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD
                        decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                        if (detallebolsa.BODPIDDP.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
                            // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //var detpednac = context.PEDEPE.Find(iddetpedido);
                            detpednac.DEPECAAT = cantatendida;
                            detpednac.DEPEPEAT = pesoatendido;
                            detpednac.DEPEPERE = pesoreal;
                            if (iddetpedidostoc.HasValue)
                            {
                                detpednac.DEPESTOC = iddetpedidostoc;
                            }
                            detpednac.DEPETADE = tade;
                            detpednac.DEPEPEBR = pebr;

                            context.SaveChanges();
                        }
                        else if (detallebolsa.BODPIDDO.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                            detpedint.DEOSCAAT = cantatendida;
                            detpedint.DEOSPEAT = pesoatendido;
                            detpedint.DEOSPERE = pesoreal;
                            if (iddetpedidostoc.HasValue)
                            {
                                detpedint.DEOSSTOC = iddetpedidostoc;
                            }
                            detpedint.DEOSTADE = tade;
                            detpedint.DEOSPEBR = pebr;
                            //actualiza peso en osa ---PENDIENTE EN AS
                            var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                            if (osa != null)
                            {
                                osa.OSASCAEN = pesoatendido;
                                actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
                            }
                            context.SaveChanges();
                        }
                        if (!sinempaque)
                        {
                            //actualiza el stock de la bolsa
                            var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in todasbolsasprep)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                            if (detemp != null)
                            {
                                //---PENDIENTE EN AS
                                detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                if (emp.CAEMMSPA == "+")
                                {
                                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                }
                                else
                                {
                                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
                                }
                                if (iddetpedidostoc.HasValue)
                                {
                                    detemp.DEEMSTCE = iddetpedidostoc.Value;
                                }
                                if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
                                context.SaveChanges();
                                //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                if (detemp == null)
                                {
                                    bol.BOLSESTA = 9; //ya no se usa la bolsa
                                    actualizaGMCAEM(bol.BOLSCOEM, "B");
                                }
                                else
                                {
                                    bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                    actualizaGMCAEM(bol.BOLSCOEM, "");
                                }
                                context.SaveChanges();
                            }
                        }
                        vpar.VALSAL = new List<string>();
                        vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                        if (!sinempaque)
                        {
                            vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                        }
                        else
                        {
                            vpar.VALSAL.Add("");
                        }
                        resultado = ""; // ent.BODPIDDE.ToString();
                        vpar.ESTOPE = true;
                    }
                    else
                    {
                        resultado = "Código de empaque incorrecto";
                    }
                }
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Util.EscribeLog(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //            //System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                if (ex.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.StackTrace);
                }
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }

        public RESOPE guardaPreparacionBolsa2se(appWcfService.DTO_PEBODP paramOperacion)
        {
            //detallebolsa
            List<appWcfService.PEBODP> listBolsas = null;
            List<decimal> listElimina = null; //Eliminamos previamente antes de guardar

            //

            Nullable<decimal> iddetpedidostoc = null;

            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            decimal almacen;

            try
            {
                //cambios
                listBolsas = paramOperacion.Items;
                listElimina = paramOperacion.Items2;
                //

                partida = articulo = "";
                almacen = 0;

                //if (listBolsas == null)//detallebolsa == null)
                //{
                //    Util.EscribeLog("detallebolsa es null");
                //}
                //else
                //{
                //    Util.EscribeLog("detallebolsa " + listBolsas[0].BODPIDDP.ToString() /*detallebolsa.BODPIDDP.ToString()*/ 
                //        + " " + listBolsas[0].PEBOLS.BOLSCOEM/*detallebolsa.PEBOLS.BOLSCOEM*/ + " USCR " + listBolsas[0].BODPUSCR/*detallebolsa.BODPUSCR*/);
                //}

                using (var context = new PEDIDOSEntities())
                {
                    //06/10/2018
                    //if (listElimina != null)
                    //{
                    //    listElimina.ForEach(ant =>
                    //    {
                    //        Util.EscribeLog(ant.ToString());
                    //        remueveBolsaPedidose(ant, listBolsas[0].BODPUSCR);
                    //    });
                    //    context.SaveChanges();
                    //}
                    //20181019 elimina preparacion anterior
                    var detosas = listBolsas.Where(osas => osas.BODPIDDO.HasValue).GroupBy(id => id.BODPIDDO).Select(grp => grp.Key).ToList();
                    if (detosas != null)
                    {
                        foreach (var iddo in detosas)
                        {
                            var bodpsxdetosa = context.PEBODP.Where(d => d.BODPIDDO == iddo && d.BODPESTA != 4 && d.BODPESTA != 5).ToList();
                            foreach (var item in bodpsxdetosa)
                            {
                                remueveBolsaPedidose(item.BODPIDDE, listBolsas[0].BODPUSCR);
                            }
                        }
                    }
                    var detped = listBolsas.Where(detp => detp.BODPIDDP.HasValue).GroupBy(id => id.BODPIDDP).Select(grp => grp.Key).ToList();
                    if (detped != null)
                    {
                        foreach (var iddp in detped)
                        {
                            var bodpsxdetped = context.PEBODP.Where(d => d.BODPIDDP == iddp && d.BODPESTA != 4 && d.BODPESTA != 5).ToList();
                            foreach (var item in bodpsxdetped)
                            {
                                remueveBolsaPedidose(item.BODPIDDE, listBolsas[0].BODPUSCR);
                            }
                        }
                    }

                    foreach (var detallebolsa in listBolsas)
                    {
                        EFModelo.PEDEPE detpednac = null;
                        EFModelo.PEDEOS detpedint = null;
                        EFModelo.GMCAEM emp = null;
                        //inserta PBOLS si no existe
                        //var bol = context.PEBOLS.Find(detallebolsa.BODPIDBO);
                        bool sinempaque = string.IsNullOrWhiteSpace(detallebolsa.PEBOLS.BOLSCOEM);
                        if (!sinempaque)
                        {
                            emp = context.GMCAEM.FirstOrDefault(b => b.CAEMCIA == 1 && b.CAEMCOEM == detallebolsa.PEBOLS.BOLSCOEM);
                        }

                        if (emp != null || sinempaque)
                        {
                            EFModelo.PEBOLS bol = null;
                            if (!sinempaque)
                            {
                                bol = context.PEBOLS.FirstOrDefault(b => b.BOLSCOEM == detallebolsa.PEBOLS.BOLSCOEM); //.Find(bolsa.BOLSIDBO);

                                if (bol == null) //no existe pbols, insertar
                                {
                                    bol = new EFModelo.PEBOLS();
                                    //inserta = true;
                                    bol.BOLSCOEM = detallebolsa.PEBOLS.BOLSCOEM;
                                    bol.BOLSUSCR = detallebolsa.BODPUSCR;
                                    bol.BOLSFECR = DateTime.Now;
                                    bol.BOLSCOCA = null;
                                    bol.BOLSESTA = 1;
                                    bol.BOLSALMA = emp.CAEMALMA;
                                    bol.BOLSCOAR = "";
                                    Util.EscribeLog("3");

                                    context.PEBOLS.Add(bol);
                                    Util.EscribeLog("4");

                                    context.SaveChanges();
                                    Util.EscribeLog("5");

                                    detallebolsa.BODPIDBO = bol.BOLSIDBO;
                                }
                                else
                                {
                                    if (bol.BOLSALMA == 0)
                                    {
                                        bol.BOLSALMA = emp.CAEMALMA;
                                    }
                                }
                            }

                            if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                            {
                                //bol.BOLSESTA = 9; //ya no se usa la bolsa, pero esta mal por detalle
                            }
                            //bool inserta = false;
                            //
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                detpednac = context.PEDEPE.Find(detallebolsa.BODPIDDP);
                                partida = detpednac.DEPEPART;
                                articulo = detpednac.DEPECOAR;
                                almacen = detpednac.DEPEALMA;
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                                partida = detpedint.DEOSPART;
                                articulo = detpedint.DEOSCOAR;
                                almacen = detpedint.DEOSALMA;
                            }

                            //var ent = context.PEBODP.Find(detallebolsa.BODPIDDE);
                            var ent = context.PEBODP.FirstOrDefault(x => x.BODPIDDE == detallebolsa.BODPIDDE);

                            if (ent == null) //detallebolsa.BODPIDDE != 0)
                            {
                                ent = new EFModelo.PEBODP();
                                //inserta = true;
                                ent.BODPUSCR = detallebolsa.BODPUSCR;
                                ent.BODPFECR = DateTime.Now;
                                ent.BODPAPOR = "M";
                                
                                context.PEBODP.Add(ent);
                                if (!sinempaque)
                                {
                                    //inserta tipo 1 SALIDA
                                    insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                }
                            }
                            else
                            {
                                ent.BODPUSMO = detallebolsa.BODPUSCR;
                                ent.BODPFEMO = DateTime.Now;
                                ent.BODPESTA = 3;
                                ent.BODPAPOR = "X";
                                if (!sinempaque)
                                {
                                    //SOLO si las cantidades o pesos son diferentes
                                    //inserta tipo 3 reingreso
                                    //inserta tipo 1 salida
                                    if (detallebolsa.BODPCANT != ent.BODPCANT || detallebolsa.BODPPESO != ent.BODPPESO)
                                    {
                                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_MODIFICA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                        insertaMovimientoKardex(context, detallebolsa.BODPIDBO, TIPO_MOV_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, detallebolsa.BODPCANT, detallebolsa.BODPPESO, detallebolsa.BODPPEBR - detallebolsa.BODPTADE, detallebolsa.BODPUSCR, detallebolsa.BODPIDDP, detallebolsa.BODPIDDO);
                                    }
                                }
                            }
                            //automatizar el parse sin incluir la PK
                            ent.BODPIDBO = detallebolsa.BODPIDBO;
                            ent.BODPIDDP = detallebolsa.BODPIDDP; //detalle de pedido si es que no es null
                            ent.BODPALMA = almacen; //bol.BOLSALMA;
                            ent.BODPPART = partida;
                            ent.BODPCOAR = articulo;
                            ent.BODPCANT = detallebolsa.BODPCANT;
                            ent.BODPPESO = detallebolsa.BODPPESO;
                            ent.BODPPERE = detallebolsa.BODPPERE;
                            ent.BODPDIFE = detallebolsa.BODPDIFE;
                            ent.BODPSTCE = detallebolsa.BODPSTCE;
                            ent.BODPINBO = detallebolsa.BODPINBO;
                            ent.BODPIDDO = detallebolsa.BODPIDDO; //detalle de osa si es que no es null
                                                                  //2018-04-11
                            ent.BODPTAUN = detallebolsa.BODPTAUN;
                            //iddetpedido = ent.BODPIDDP;
                            //iddetpedidoint = ent.BODPIDDO;
                            if (detallebolsa.PEDEPE != null && detallebolsa.PEDEPE.DEPESTOC.HasValue)
                            {
                                iddetpedidostoc = detallebolsa.PEDEPE.DEPESTOC;
                            }

                            ent.BODPTADE = detallebolsa.BODPTADE;
                            ent.BODPPEBR = detallebolsa.BODPPEBR;
                            ent.BODPESTA = detallebolsa.BODPESTA;

                            context.SaveChanges(); //necesite guardar 1ro labolsa actual para luego recuperar todas las bolsas de la BD

                            decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            if (detallebolsa.BODPIDDP.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == detallebolsa.BODPIDDP).ToList();
                                // decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpednac = context.PEDEPE.Find(iddetpedido);
                                detpednac.DEPECAAT = cantatendida;
                                detpednac.DEPEPEAT = pesoatendido;
                                detpednac.DEPEPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpednac.DEPESTOC = iddetpedidostoc;
                                }
                                detpednac.DEPETADE = tade;
                                detpednac.DEPEPEBR = pebr;

                                context.SaveChanges();
                            }
                            else if (detallebolsa.BODPIDDO.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == detallebolsa.BODPIDDO).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //var detpedint = context.PEDEOS.Find(detallebolsa.BODPIDDO);
                                detpedint.DEOSCAAT = cantatendida;
                                detpedint.DEOSPEAT = pesoatendido;
                                detpedint.DEOSPERE = pesoreal;
                                if (iddetpedidostoc.HasValue)
                                {
                                    detpedint.DEOSSTOC = iddetpedidostoc;
                                }
                                detpedint.DEOSTADE = tade;
                                detpedint.DEOSPEBR = pebr;
                                //actualiza peso en osa ---PENDIENTE EN AS
                                var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                                if (osa != null)
                                {
                                    osa.OSASCAEN = pesoatendido;
                                    actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar si es correcto
                                }
                                context.SaveChanges();
                            }
                            if (!sinempaque)
                            {
                                //actualiza el stock de la bolsa
                                var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in todasbolsasprep)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                                if (detemp != null)
                                {
                                    //---PENDIENTE EN AS
                                    detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                    if (emp.CAEMMSPA == "+")
                                    {
                                        detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                    }
                                    else
                                    {
                                        detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - emp.CAEMDEEM - pesoatendido;
                                    }
                                    if (iddetpedidostoc.HasValue)
                                    {
                                        detemp.DEEMSTCE = iddetpedidostoc.Value;
                                    }
                                    if (detemp.DEEMSTCE == 1 || ent.BODPSTCE == 1)
                                    {
                                        detemp.DEEMESBO = 9;
                                    }
                                    else if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                    {
                                        detemp.DEEMESBO = 9;
                                    }
                                    else
                                    {
                                        detemp.DEEMESBO = 1;
                                    }
                                    actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO);
                                    context.SaveChanges();
                                    //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                    detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                    if (detemp == null)
                                    {
                                        bol.BOLSESTA = 9; //ya no se usa la bolsa
                                        actualizaGMCAEM(bol.BOLSCOEM, "B");
                                    }
                                    else
                                    {
                                        bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                        actualizaGMCAEM(bol.BOLSCOEM, "");
                                    }
                                    context.SaveChanges();
                                }
                            }
                            //vpar.VALSAL = new List<string>();
                            ////vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                            ////vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                            //resultado = ""; // ent.BODPIDDE.ToString();
                            //vpar.ESTOPE = true;
                        }
                        else
                        {
                            resultado = "Código de empaque incorrecto";
                        }
                    }
                }
                vpar.VALSAL = new List<string>();
                //vpar.VALSAL.Add(ent.BODPIDDE.ToString());
                //vpar.VALSAL.Add(detallebolsa.BODPIDBO.ToString());
                resultado = ""; // ent.BODPIDDE.ToString();
                vpar.ESTOPE = true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Util.EscribeLog(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
            //            //System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                if (ex.InnerException != null)
                {
                    Util.EscribeLog(ex.InnerException.StackTrace);
                }
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }///REVISAR LLAMA A UNA FUNCION

        public RESOPE remueveBolsaPedidose(decimal idbolsapedido, string usuario)
        {
            Nullable<decimal> iddetpedido;
            Nullable<decimal> iddetpedidoint;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;
            try
            {
                partida = articulo = "";
                using (var context = new PEDIDOSEntities())
                {
                    EFModelo.PEDEPE detpednac = null;
                    EFModelo.PEDEOS detpedint = null;

                    //bool inserta = false;
                    var ent = context.PEBODP.Find(idbolsapedido);
                    if (ent != null) //detallebolsa.BODPIDDE != 0)
                    {
                        bool sinempaque = !ent.BODPIDBO.HasValue;
                        EFModelo.PEBOLS bol = null;
                        if (!sinempaque)
                        {
                            bol = context.PEBOLS.Find(ent.BODPIDBO);
                        }

                        if (ent.BODPIDDP.HasValue)
                        {
                            detpednac = context.PEDEPE.Find(ent.BODPIDDP);
                            partida = detpednac.DEPEPART;
                            articulo = detpednac.DEPECOAR;
                        }
                        else if (ent.BODPIDDO.HasValue)
                        {
                            detpedint = context.PEDEOS.Find(ent.BODPIDDO);
                            partida = detpedint.DEOSPART;
                            articulo = detpedint.DEOSCOAR;
                        }
                        iddetpedido = ent.BODPIDDP;
                        iddetpedidoint = ent.BODPIDDO;
                        //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                        //{
                        //bol.BOLSESTA = 1; //ya no se usa
                        //}
                        if (!sinempaque)
                        {
                            insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
                        }
                        context.PEBODP.Remove(ent);
                        context.SaveChanges();
                        decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

                        if (iddetpedido.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            //detpednac = context.PEDEPE.Find(iddetpedido);
                            detpednac.DEPECAAT = cantatendida;
                            detpednac.DEPEPEAT = pesoatendido;
                            detpednac.DEPEPERE = pesoreal;
                            detpednac.DEPETADE = tade;
                            detpednac.DEPEPEBR = pebr;

                            //inserta tipo 2 kardex reingreso
                            context.SaveChanges();
                        }
                        else if (iddetpedidoint.HasValue)
                        {
                            var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in listbolsas)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            detpedint.DEOSCAAT = cantatendida;
                            detpedint.DEOSPEAT = pesoatendido;
                            detpedint.DEOSPERE = pesoreal;
                            detpedint.DEOSTADE = tade;
                            detpedint.DEOSPEBR = pebr;
                            //actualiza peso en osa ---PENDIENTE EN AS
                            var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                            if (osa != null)
                            {
                                osa.OSASCAEN = pesoatendido;
                                actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar AS
                            }
                            context.SaveChanges();

                        }
                        if (!sinempaque)
                        {
                            //actualiza el stock de la bolsa
                            var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                            //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                            cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                            foreach (var bolsaprep in todasbolsasprep)
                            {
                                cantatendida += bolsaprep.BODPCANT;
                                pesoatendido += bolsaprep.BODPPESO;
                                pesoreal += bolsaprep.BODPPERE;
                                tade += bolsaprep.BODPTADE;
                                pebr += bolsaprep.BODPPEBR;
                            }
                            var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                            if (detemp != null)
                            {
                                detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                //si la bolsa preparada estaba marcada como stock cero, lo desmarco
                                if (ent.BODPSTCE == 1)
                                {
                                    detemp.DEEMSTCE = 0;
                                }
                                if (detemp.DEEMSTCE == 0)
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                else
                                if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                {
                                    detemp.DEEMESBO = 9;
                                }
                                else
                                {
                                    detemp.DEEMESBO = 1;
                                }
                                context.SaveChanges();
                                actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); //Validar AS
                                                                                                                                                       //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                if (detemp == null)
                                {
                                    bol.BOLSESTA = 9; //ya no se usa la bolsa
                                    actualizaGMCAEM(bol.BOLSCOEM, "B");
                                }
                                else
                                {
                                    bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                    actualizaGMCAEM(bol.BOLSCOEM, "");
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                    vpar.ESTOPE = true;
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }///REVISAR LLAMA A UNA FUNCION

        //DMA 17_10_2018
        public RESOPE remueveBolsaPedidose2(string idbolsapedido, string usuario)
        {
            //Recibira una lista ('1','2','5')

            Nullable<decimal> iddetpedido;
            Nullable<decimal> iddetpedidoint;
            RESOPE vpar;
            vpar = new RESOPE() { ESTOPE = false };

            string resultado = "";
            string partida, articulo;

            
            var listaelimina = idbolsapedido.Split(',').ToList();

            try
            {
                using (var context = new PEDIDOSEntities())
                {

                    foreach (var idbols in listaelimina)
                    {
                        partida = articulo = "";

                        EFModelo.PEDEPE detpednac = null;
                        EFModelo.PEDEOS detpedint = null;

                        //bool inserta = false;
                        var ent = context.PEBODP.Find(Convert.ToDecimal(idbols));
                        if (ent != null) //detallebolsa.BODPIDDE != 0)
                        {
                            bool sinempaque = !ent.BODPIDBO.HasValue;
                            EFModelo.PEBOLS bol = null;
                            if (!sinempaque)
                            {
                                bol = context.PEBOLS.Find(ent.BODPIDBO);
                            }

                            if (ent.BODPIDDP.HasValue)
                            {
                                detpednac = context.PEDEPE.Find(ent.BODPIDDP);
                                partida = detpednac.DEPEPART;
                                articulo = detpednac.DEPECOAR;
                            }
                            else if (ent.BODPIDDO.HasValue)
                            {
                                detpedint = context.PEDEOS.Find(ent.BODPIDDO);
                                partida = detpedint.DEOSPART;
                                articulo = detpedint.DEOSCOAR;
                            }
                            iddetpedido = ent.BODPIDDP;
                            iddetpedidoint = ent.BODPIDDO;
                            //if (detallebolsa.BODPDIFE <= 0 || detallebolsa.BODPSTCE == 1)
                            //{
                            //bol.BOLSESTA = 1; //ya no se usa
                            //}
                            if (!sinempaque)
                            {
                                insertaMovimientoKardex(context, ent.BODPIDBO, TIPO_MOV_CANCELA_SALIDA_PREP_PED, bol.BOLSALMA, partida, articulo, ent.BODPCANT, ent.BODPPESO, ent.BODPPEBR - ent.BODPTADE, usuario, ent.BODPIDDP, ent.BODPIDDO);
                            }
                            context.PEBODP.Remove(ent);
                            context.SaveChanges();
                            decimal cantatendida, pesoatendido, pesoreal, tade, pebr;

                            if (iddetpedido.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDP == iddetpedido).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                //detpednac = context.PEDEPE.Find(iddetpedido);
                                detpednac.DEPECAAT = cantatendida;
                                detpednac.DEPEPEAT = pesoatendido;
                                detpednac.DEPEPERE = pesoreal;
                                detpednac.DEPETADE = tade;
                                detpednac.DEPEPEBR = pebr;

                                //inserta tipo 2 kardex reingreso
                                context.SaveChanges();
                            }
                            else if (iddetpedidoint.HasValue)
                            {
                                var listbolsas = context.PEBODP.Where(ped => ped.BODPIDDO == iddetpedidoint).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in listbolsas)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                detpedint.DEOSCAAT = cantatendida;
                                detpedint.DEOSPEAT = pesoatendido;
                                detpedint.DEOSPERE = pesoreal;
                                detpedint.DEOSTADE = tade;
                                detpedint.DEOSPEBR = pebr;
                                //actualiza peso en osa ---PENDIENTE EN AS
                                var osa = context.PROSAS.Find(1, detpedint.DEOSFOLI, detpedint.DEOSSECU);
                                if (osa != null)
                                {
                                    osa.OSASCAEN = pesoatendido;
                                    actualizaPROSAS(osa.OSASFOLI, osa.OSASSECU, osa.OSASCAEN, ""); //Validar AS
                                }
                                context.SaveChanges();

                            }
                            if (!sinempaque)
                            {
                                //actualiza el stock de la bolsa
                                var todasbolsasprep = context.PEBODP.Where(prep => prep.BODPIDBO == bol.BOLSIDBO && prep.BODPALMA == bol.BOLSALMA && prep.BODPPART == partida && prep.BODPCOAR == articulo).ToList();
                                //decimal cantatendida, pesoatendido, pesoreal, tade, pebr;
                                cantatendida = pesoatendido = pesoreal = tade = pebr = 0;
                                foreach (var bolsaprep in todasbolsasprep)
                                {
                                    cantatendida += bolsaprep.BODPCANT;
                                    pesoatendido += bolsaprep.BODPPESO;
                                    pesoreal += bolsaprep.BODPPERE;
                                    tade += bolsaprep.BODPTADE;
                                    pebr += bolsaprep.BODPPEBR;
                                }
                                var detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMPART == partida && det.DEEMARTI == articulo && det.DEEMTIPE == "N");
                                if (detemp != null)
                                {
                                    detemp.DEEMCAST = detemp.DEEMCANT - cantatendida;
                                    detemp.DEEMPEST = (detemp.DEEMPNET - detemp.DEEMDEST + detemp.DEEMACON) - pesoatendido;
                                    //si la bolsa preparada estaba marcada como stock cero, lo desmarco
                                    if (ent.BODPSTCE == 1)
                                    {
                                        detemp.DEEMSTCE = 0;
                                    }
                                    if (detemp.DEEMSTCE == 0)
                                    {
                                        detemp.DEEMESBO = 1;
                                    }
                                    else
                                    if (detemp.DEEMCAST <= 0 && detemp.DEEMPEST <= 0)
                                    {
                                        detemp.DEEMESBO = 9;
                                    }
                                    else
                                    {
                                        detemp.DEEMESBO = 1;
                                    }
                                    context.SaveChanges();
                                    actualizaGMDEEM(detemp.DEEMCOEM, detemp.DEEMSECU, detemp.DEEMCAST, detemp.DEEMPEST, detemp.DEEMSTCE, detemp.DEEMESBO); //Validar AS
                                                                                                                                                           //buscar una partida en la bolsa que no este vacia, si no hay ninguna actualizar a 9 anulado
                                    detemp = context.GMDEEM.FirstOrDefault(det => det.DEEMCIA == 1 && det.DEEMCOEM == bol.BOLSCOEM && det.DEEMTIPE == "N" && det.DEEMESBO != 9);
                                    if (detemp == null)
                                    {
                                        bol.BOLSESTA = 9; //ya no se usa la bolsa
                                        actualizaGMCAEM(bol.BOLSCOEM, "B");
                                    }
                                    else
                                    {
                                        bol.BOLSESTA = 1; //ya no se usa la bolsa - si se modifica la cant o kilos de la bolsa o si se le quito el stock cero desmarcar la bolsa
                                        actualizaGMCAEM(bol.BOLSCOEM, "");
                                    }
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    vpar.ESTOPE = true;
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                var innerEx = e.InnerException;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Util.EscribeLog(innerEx.Message);
                resultado = innerEx.Message;
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                resultado = ex.Message;
            }
            finally
            {
            }
            vpar.MENERR = resultado;
            return vpar;
        }//NO SE USA

        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> obtieneDetallePreparacionPedidosse(decimal iddetallepedido)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE(iddetallepedido).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse(decimal iddetallepedint)
        {
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> lista = null;
            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE(iddetallepedint).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        //DMA 17/10/2018
        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse2(string iddetallepedint)
        {
            List<object> listaeo = new List<object>();
            List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> lista = null;
            var Listides = iddetallepedint.Split(',').ToList();

            try
            {
                using (var context = new PEDIDOSEntities())
                {
                    foreach (var item in Listides)
                    {
                        var resultado = context.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE(Convert.ToDecimal(item)).ToList<object>();
                        if (resultado != null)
                        {
                            listaeo.AddRange(resultado);
                        }
                    }
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result>(listaeo);
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
            }
            finally
            {
            }
            return lista;
        }//cambiar proc

        #endregion

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

        public bool EnviaCorreoNotificacionPedido(decimal idpedido, out string mensaje)
        {
            string destinatario, cc, bcc, asunto, body;

            bool vpar;
            vpar = false;
            List<object> listaeo = null;
            List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;
            appLogica.appDB2 _appDB2 = null;

            try
            {
                destinatario = bcc = "";

                using (var context = new PEDIDOSEntities())
                {
                    listaeo = context.USP_OBTIENE_PEDIDO_CONSULTA(idpedido).ToList<object>();
                }
                lista = Util.ParseEntityObject<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>(listaeo);

                if (lista.Count > 0)
                {
                    if (lista[0].CAPEIDES == 1)
                    {
                        mensaje = "Pedido no emitido";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(lista[0].CAPEEMAI))
                        {
                            destinatario = lista[0].CAPEEMAI.Trim();
                        }
                        //pruebas
                        destinatario = "ddk_sk@hotmail.com";
                        _appDB2 = new appLogica.appDB2();
                        RFEUSER usuario = _appDB2.ObtieneUsuarioDeFacturacion(lista[0].CAPEUSEM);
                        if (usuario != null)
                        {
                            bcc = usuario.USERMAIL.Trim();
                        }
                        //pruebas
                        bcc = "mlopez@incatops.com,margaret.flores@rextechsolutions.com,daryl.marcapura@rextechsolutions.com";
                        if (PreparaCorreoNotificacionPedido(lista, out cc, out asunto, out body, out mensaje))
                        {
                            if (EnvioCorreo(destinatario, cc, bcc, asunto, body))
                            {
                                vpar = true;
                            }
                            else
                            {
                                mensaje = Mensajes.MENSAJE_CORREO_ERROR_ENVIO;
                            }
                        }
                        else
                        {
                            //vpar.MENERR = mensaje;
                        }
                    }
                }
                else
                {
                    mensaje = Mensajes.MENSAJE_PEDIDO_NO_ENCONTRADO;
                }
            }
            catch (Exception ex)
            {
                Util.EscribeLog(ex.Message);
                mensaje = ErrorGenerico(ex.Message);
                //throw ex;
            }
            finally
            {
                if (_appDB2 != null)
                {
                    _appDB2.Finaliza();
                    _appDB2 = null;
                }
            }
            return vpar;
        }

        private bool PreparaCorreoNotificacionPedido(List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> pedidodet, out string cc, out string asunto, out string body, out string mensaje)
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
    }
}
