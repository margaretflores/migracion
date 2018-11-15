using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
//using JWT;
//using JWT.Serializers;

namespace appWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class appService : IappService
    {
        //appLanzador.Principal Lanzador;

        //public RESOPE mostrarPedidosApp()
        //{
        //    RESOPE resultado;
        //    Lanzador = new appLanzador.Principal();
        //    resultado = Lanzador.EjecutaOperacion(new PAROPE() { CODOPE = appConstantes.CodigoOperacion.MOSTRAR_PEDIDOS_APP } );
        //    Lanzador = null;
        //    return resultado;
        //}

        public List<PECAPE> mostrarPedidosApp()
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.appDB2 _spn;
            _spn = new appLogica.appDB2();
            return _spn.MostrarPedidosApp2();
        }

        //public RESOPE ejecutaServicio(PAROPE paramOperacion)
        //{
        //    RESOPE resultado;
        //    Lanzador = new appLanzador.Principal();
        //    resultado = Lanzador.EjecutaOperacion(paramOperacion);
        //    Lanzador = null;
        //    return resultado;
        //}

        public string validarUsuario(string usuario, string clave)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            string respuesta = "";
            appLogica.Principal _spn;

            try
            {
                RESOPE resultado;

                PAROPE paramOperacion = new PAROPE() { CODOPE = appConstantes.CodigoOperacion.VALIDA_USUARIO };
                paramOperacion.VALENT = new List<string>();
                paramOperacion.VALENT.Add(usuario);
                paramOperacion.VALENT.Add(clave);
                _spn = new appLogica.Principal();
                resultado = _spn.ValidaUsuario(paramOperacion);
                if (!resultado.ESTOPE)
                {
                    respuesta = resultado.MENERR;
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            finally
            {
                _spn = null;
            }
            return respuesta;
        }

        public RESOPE validarUsuario2(string usuario, string clave)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.Principal _spn;
            RESOPE resultado = null;

            try
            {

                PAROPE paramOperacion = new PAROPE() { CODOPE = appConstantes.CodigoOperacion.VALIDA_USUARIO };
                paramOperacion.VALENT = new List<string>();
                paramOperacion.VALENT.Add(usuario);
                paramOperacion.VALENT.Add(clave);
                _spn = new appLogica.Principal();
                resultado = _spn.ValidaUsuario(paramOperacion);
                //if (!resultado.ESTOPE)
                //{
                //    respuesta = resultado.MENERR;
                //}
            }
            catch (Exception ex)
            {
                if (resultado == null)
                {
                    resultado = new RESOPE() { ESTOPE = false, MENERR = ex.Message };
                }
            }
            finally
            {
                _spn = null;
            }
            return resultado;
        }

        public List<PEDEPE> mostrarDetallePedidosApp(decimal idpedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarDetallePedidosApp(idpedido);
        }

        public List<USP_OBTIENE_DETALLE_PEDIDOS_Result> mostrarDetallePedidos(decimal idpedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarDetallePedidos(idpedido);
        }

        public RESOPE cambiaEstadoPedido(PEESPE estadopedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.cambiaEstadoPedido(estadopedido);
        }

        public RESOPE cambiaEstadoPedido2(PEESPE estadopedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.cambiaEstadoPedido2(estadopedido);
        }

        //public string cambiaEstadoPedidodic(Dictionary<string, string> estadopedido)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.cambiaEstadoPedidodic(estadopedido);
        //}


        public List<PEUBIC> mostrarUbicacionesArticulo(string articulo, string partida, decimal idalmacen)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarUbicacionesArticulo(articulo, partida, idalmacen);
        }

        public List<USP_OBTIENE_BOLSA_Result> obtieneBolsa(decimal iddetalle, string empaque)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneBolsa(iddetalle, empaque);
        }

        public RESOPE guardaPreparacionBolsa(PEBODP detallebolsa)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.guardaPreparacionBolsa(detallebolsa);
        }

        //cambio 26/03/18
        public RESOPE guardaPreparacionBolsa2(DTO_PEBODP paramOperacion)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.guardaPreparacionBolsa2(paramOperacion);
        }
        //

        public RESOPE remueveBolsaPedido(decimal idbolsapedido, string usuario)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.remueveBolsaPedido(idbolsapedido, usuario);
        }

        //20180726
        #region SIN EMPAQUE, NUEVOS METODOS PARA CON EMPAQUE Y SIN EMPAQUE

        public RESOPE guardaPreparacionBolsase(PEBODP detallebolsa)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.guardaPreparacionBolsase(detallebolsa);
        }

        public RESOPE guardaPreparacionBolsa2se(DTO_PEBODP paramOperacion)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.guardaPreparacionBolsa2se(paramOperacion);
        }

        public RESOPE remueveBolsaPedidose(decimal idbolsapedido, string usuario)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.remueveBolsaPedidose(idbolsapedido, usuario);
        }
        //DMA 17_10_2018
        public RESOPE remueveBolsaPedidose2(string idbolsapedido, string usuario)
        {
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.remueveBolsaPedidose2(idbolsapedido, usuario);
        }

        public List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> obtieneDetallePreparacionPedidosse(decimal iddetallepedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetallePreparacionPedidosse(iddetallepedido);
        }

        public List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse(decimal iddetallepedint)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetallePreparacionPedIntse(iddetallepedint);
        }
        //DMA 17/10/2018
        public List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSASE_Result> obtieneDetallePreparacionPedIntse2(string iddetallepedint)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetallePreparacionPedIntse2(iddetallepedint);
        }

        #endregion

        //public List<PEBODP> obtieneDetallePreparacionPedidos(decimal iddetallepedido)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.obtieneDetallePreparacionPedidos(iddetallepedido);
        //}
        public List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> obtieneDetallePreparacionPedidos(decimal iddetallepedido)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetallePreparacionPedidos(iddetallepedido);
        }

        public RESOPE obtieneDatosPartida(string articulo, string partida, decimal idalmacen)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.appDB2 _appas = null;
            try
            {
                _appas = new appLogica.appDB2();
                return _appas.obtieneDatosPartida(articulo, partida, idalmacen);
            }
            catch (Exception ex)
            {
                EscribeLog(ex.Message);
                return new RESOPE() { ESTOPE = false, MENERR = ex.Message };
            }
            finally
            {
                if (_appas != null)
                {
                    _appas.Finaliza();
                    _appas = null;
                }
            }
        }

        private void EscribeLog(string mensaje)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                String dir = Path.GetDirectoryName(path);
                dir += "\\errores";
                string archivo = dir + "\\pednac_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
                Directory.CreateDirectory(dir); // inside the if statement

                //if (System.IO.File.Exists(archivo))
                //{
                using (System.IO.FileStream stream = new System.IO.FileStream(archivo, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + mensaje);
                    writer.Close();
                }
                //}
                //else
                //{
                //    using (System.IO.StreamWriter writer = System.IO.File.CreateText(archivo))
                //    {
                //        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + mensaje);
                //        writer.Close();
                //    }
                //}
            }
            catch (Exception) { }
        }

        #region PEDIDOS INTERNOS
        public List<PETIFO> obtieneTiposFolio()
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneTiposFolio();
        }
        public List<USP_FOLIO_USUARIO_Result> obtieneFoliosUsuario(string usuario)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneFoliosUsuario(usuario);
        }

        public List<USP_OBTIENE_OSAS_PENDIENTES_Result> mostrarPedidosInternos(string tipofolios)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarPedidosInternos(tipofolios);
        }

        public List<USP_OBTIENE_OSAS_PENDIENTES_Result> mostrarPedidosInternosPartida(string tipofolios, string partida)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarPedidosInternos(tipofolios, partida);
        }

        public List<USP_OBTIENE_DETALLE_OSA_Result> mostrarDetallePedidosInternos(string folio)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarDetallePedidosInternos(folio);
        }

        public RESOPE cambiaEstadoPedInt(PECAOS estadopedint)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.cambiaEstadoPedInt(estadopedint);
        }

        public RESOPE actualizaPreparacionItemOSA(PEDEOS estadopedint)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.actualizaPreparacionItemOSA(estadopedint);
        }

        public List<USP_OBTIENE_DETPREPARACION_POR_IDDETOSA_Result> obtieneDetallePreparacionPedInt(decimal iddetallepedint)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetallePreparacionPedInt(iddetallepedint);
        }

        public List<USP_OBTIENE_BOLSA_OSA_Result> obtieneBolsaOsa(decimal iddetalle, string empaque)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneBolsaOsa(iddetalle, empaque);
        }

        #endregion

        #region ubicaciones
        public List<USP_OBTIENE_BOLSA_UBICACION_Result> obtieneBolsaUbicacion(string empaque)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneBolsaUbicacion(empaque);
        }

        public List<USP_CONSULTA_EMPAQUES_PARTIDA_Result> consultaEmpaquesPartida(string partida, string articulo, string empaque)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.consultaEmpaquesPartida(partida, articulo, empaque);
        }

        public RESOPE guardaBolsaUbicacion(PEBOLS bolsa)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.guardaBolsaUbicacion(bolsa);
        }

        public List<USP_OBTIENE_DETALLE_BOLSA_Result> obtieneDetalleBolsa(string empaque)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.obtieneDetalleBolsa(empaque);
        }

        public List<USP_OBTIENE_OSAS_PENDIENTES_PLANTA_Result> mostrarPedidosIntPendRecepcion(string tipofolios, string numfolio)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarPedidosIntPendRecepcion(tipofolios, numfolio);
        }

        public List<USP_OBTIENE_DETALLE_OSA_PLANTA_Result> mostrarDetallePedidosIntPendRecepcion(string folio)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.mostrarDetallePedidosIntPendRecepcion(folio);
        }

        public RESOPE conformidadRecepcionOsa(appWcfService.DET_USP_OBTIENE_DETALLE_OSA_PLANTA_Result Listadetalles)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.conformidadRecepcionOsa(Listadetalles);
        }

        public RESOPE cambiaestaDeosBodp(DTO_USP_OBTIENE_DETALLE_OSA_Result paramOperacion)
        {
            //Authenticate(WebOperationContext.Current.IncomingRequest);
            appLogica.MKT _spn;
            _spn = new appLogica.MKT();
            return _spn.cambiaestaDeosBodp(paramOperacion);
        }

        #endregion

        //private bool Authenticate(IncomingWebRequestContext context)
        //{

        //    string authorizationeader = context.Headers["Authorization"];

        //    if (String.IsNullOrEmpty(authorizationeader))
        //    {
        //        //throw new GlobalApplicationException(HttpStatusCode.Unauthorized, 0, "Credenciales Erroneas", "Credenciales Erroneas");

        //        ErrorMessage error = new ErrorMessage();
        //        error.DeveloperMessage = "";
        //        error.Message = "Debe contener el header Authorization";
        //        error.Status = (int)HttpStatusCode.Unauthorized;
        //        throw new WebFaultException<ErrorMessage>(error, HttpStatusCode.Unauthorized);//Exception("Ingresar el header");
        //    }
        //    else
        //    {
        //        // verificando que el token sea correcto
        //        byte[] secretKey = Base64UrlDecode(ConfigurationManager.AppSettings["SECRET_KEY"]);//pass key to secure and decode it  

        //        try
        //        {
        //            IJsonSerializer serializer = new JsonNetSerializer();
        //            IDateTimeProvider provider = new UtcDateTimeProvider();
        //            IJwtValidator validator = new JwtValidator(serializer, provider);
        //            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        //            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

        //            var payload = decoder.DecodeToObject<IDictionary<string, object>>(authorizationeader, secretKey, verify: true);

        //        }
        //        catch (TokenExpiredException)
        //        {
        //            ErrorMessage error = new ErrorMessage();
        //            error.DeveloperMessage = "";
        //            error.Message = "Las credenciales de acceso han expirado";
        //            error.Status = (int)HttpStatusCode.Unauthorized;
        //            throw new WebFaultException<ErrorMessage>(error, HttpStatusCode.Unauthorized);//Exception("Ingresar el header");
        //        }
        //        catch (SignatureVerificationException)
        //        {
        //            ErrorMessage error = new ErrorMessage();
        //            error.DeveloperMessage = "";
        //            error.Message = "Las credenciales de acceso son incorrectas";
        //            error.Status = (int)HttpStatusCode.Unauthorized;
        //            throw new WebFaultException<ErrorMessage>(error, HttpStatusCode.Unauthorized);//Exception("Ingresar el header");throw new WebFaultException(HttpStatusCode.Unauthorized);//new Exception("Credenciales Erroneas");
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorMessage error = new ErrorMessage();
        //            error.DeveloperMessage = ex.StackTrace;
        //            error.Message = ex.Message;
        //            error.Status = (int)HttpStatusCode.InternalServerError;
        //            throw new WebFaultException<ErrorMessage>(error, HttpStatusCode.InternalServerError);//Exception("Ingresar el header");throw new WebFaultException(HttpStatusCode.Unauthorized);
        //        }
        //    }
        //    return true;
        //}
        //private byte[] Base64UrlDecode(string arg) // This function is for decoding string to   
        //{
        //    string s = arg;
        //    s = s.Replace('-', '+'); // 62nd char of encoding  
        //    s = s.Replace('_', '/'); // 63rd char of encoding  
        //    switch (s.Length % 4) // Pad with trailing '='s  
        //    {
        //        case 0: break; // No pad chars in this case  
        //        case 2: s += "=="; break; // Two pad chars  
        //        case 3: s += "="; break; // One pad char  
        //        default:
        //            throw new System.Exception(
        //        "Illegal base64url string!");
        //    }
        //    return Convert.FromBase64String(s); // Standard base64 decoder  
        //}
    }
}
