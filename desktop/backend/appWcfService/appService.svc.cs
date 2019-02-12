﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.IO;
using JWT;
using JWT.Serializers;
using System.Configuration;

namespace appWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class appService : IappService
    {
        appLanzador.Principal Lanzador;

        //public RESOPE EjecutaOperacion(PAROPE paramOperacion)
        //{
        //    RESOPE resultado;
        //    Lanzador = new appLanzador.Principal();
        //    resultado = Lanzador.EjecutaOperacion(paramOperacion);
        //    Lanzador = null;
        //    return resultado;
        //}
        //26/06/2018
        public RESOPE EjecutaOperacion(PAROPE paramOperacion)
        {
            try
            {
                RESOPE tokens = new RESOPE();
                tokens.ESTOPE = false;
                tokens.MENERR = string.Empty;
                tokens.VALSAL = new List<string>();

                //if (paramOperacion.CODOPE != "US001" && paramOperacion.CODOPE != "US002")// codigos de validacion de usuarios 
                //{
                //    //tokens = ResultadoValidacion();
                //    tokens.ESTOPE = true;
                //}

                //if (paramOperacion.CODOPE == "US001" || paramOperacion.CODOPE == "US002")// codigos de validacion de usuarios 
                //{
                    return EjecutaLanzador(paramOperacion);
                //}
                //else if (tokens.ESTOPE == true)
                //{
                //    return EjecutaLanzador(paramOperacion);
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (FaultException fe)
            {
                throw new FaultException(fe.Message, fe.Code);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        private RESOPE EjecutaLanzador(PAROPE paramOperacion)
        {
            RESOPE resultado;
            Lanzador = new appLanzador.Principal();
            resultado = Lanzador.EjecutaOperacion(paramOperacion);
            Lanzador = null;
            return resultado;
        }

        private RESOPE ResultadoValidacion()
        {
            string id = string.Empty;
            // look at headers on incoming message  
            for (int i = 0;
                 i < OperationContext.Current.IncomingMessageHeaders.Count;
                 ++i)
            {
                System.ServiceModel.Channels.MessageHeaderInfo h = OperationContext.Current.IncomingMessageHeaders[i];
                // for any reference parameters with the correct name & namespace  
                if (!h.IsReferenceParameter &&
                    h.Name == "Token" &&
                    h.Namespace == "")
                {
                    // read the value of that header  
                    System.Xml.XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    id = xr.ReadElementContentAsString();
                }
            }

            return validarToken(id);
        }


        private RESOPE validarToken(string authorizationeader)
        {

            // verificando que el token sea correcto
            byte[] secretKey = Base64UrlDecode(ConfigurationManager.AppSettings["Aplicacion"]);//pass key to secure and decode it  

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var payload = decoder.DecodeToObject<IDictionary<string, object>>(authorizationeader, secretKey, verify: true);
                //throw new FaultException("Credenciales Expiradas", new FaultCode("CRED_EXPIRED"));
                return new RESOPE()
                {

                    ESTOPE = true,
                    MENERR = "OK",
                    VALSAL = new List<string>() { "OK" }

                };
            }
            catch (TokenExpiredException)
            {
                throw new FaultException("Credenciales Expiradas", new FaultCode("CRED_EXPIRED"));
                //return new RESOPE()
                //{

                //    ESTOPE = false,
                //    MENERR = "Credenciales Expiradas",
                //    VALSAL = new List<string>() { "Credenciales Expiradas" }

                //};
            }
            catch (SignatureVerificationException)
            {
                throw new FaultException("Credenciales Erroneas", new FaultCode("CRED_BAD"));
                //return new RESOPE()
                //{

                //    ESTOPE = false,
                //    MENERR = "Credenciales erroneas",
                //    VALSAL = new List<string>() { "Credenciales erroneas" }

                //};
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("CRED_ERROR"));
                //return new RESOPE()
                //{

                //    ESTOPE = false,
                //    MENERR = ex.Message,
                //    VALSAL = new List<string>() { ex.StackTrace }

                //};

            }
        }
        private byte[] Base64UrlDecode(string arg) // This function is for decoding string to   
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding  
            s = s.Replace('_', '/'); // 63rd char of encoding  
            switch (s.Length % 4) // Pad with trailing '='s  
            {
                case 0: break; // No pad chars in this case  
                case 2: s += "=="; break; // Two pad chars  
                case 3: s += "="; break; // One pad char  
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder  
        }
        //26/06/2018

        //public string validarUsuario(string usuario, string clave)
        //{
        //    string respuesta = "";
        //    appLogica.Principal _spn;

        //    try
        //    {
        //        RESOPE resultado;

        //        PAROPE paramOperacion = new PAROPE() { CODOPE = appConstantes.CodigoOperacion.VALIDA_USUARIO };
        //        paramOperacion.VALENT = new List<string>();
        //        paramOperacion.VALENT.Add(usuario);
        //        paramOperacion.VALENT.Add(clave);
        //        _spn = new appLogica.Principal();
        //        resultado = _spn.ValidaUsuario(paramOperacion);
        //        if (!resultado.ESTOPE)
        //        {
        //            respuesta = resultado.MENERR;
        //        }
        //    }
        //    catch (Exception ex) {
        //        respuesta = ex.Message; 
        //            }
        //    finally {
        //        _spn = null;
        //    }
        //    return respuesta;
        //}

        //public List<PEDEPE> mostrarDetallePedidosApp(decimal idpedido)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.mostrarDetallePedidosApp(idpedido);
        //}

        //public RESOPE cambiaEstadoPedido(PEESPE estadopedido)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.cambiaEstadoPedido(estadopedido);
        //}

        ////public string cambiaEstadoPedidodic(Dictionary<string, string> estadopedido)
        ////{
        ////    appLogica.MKT _spn;
        ////    _spn = new appLogica.MKT();
        ////    return _spn.cambiaEstadoPedidodic(estadopedido);
        ////}


        //public List<PEUBIC> mostrarUbicacionesArticulo(string articulo, string partida, decimal idalmacen)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.mostrarUbicacionesArticulo(articulo, partida, idalmacen);
        //}

        //public List<USP_OBTIENE_BOLSA_Result> obtieneBolsa(decimal iddetalle, string empaque)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.obtieneBolsa(iddetalle, empaque);
        //}

        //public RESOPE guardaPreparacionBolsa(PEBODP detallebolsa)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.guardaPreparacionBolsa(detallebolsa);
        //}

        //public RESOPE remueveBolsaPedido(decimal idbolsapedido, string usuario)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.remueveBolsaPedido(idbolsapedido, usuario);
        //}

        ////public List<PEBODP> obtieneDetallePreparacionPedidos(decimal iddetallepedido)
        ////{
        ////    appLogica.MKT _spn;
        ////    _spn = new appLogica.MKT();
        ////    return _spn.obtieneDetallePreparacionPedidos(iddetallepedido);
        ////}
        //public List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLE_Result> obtieneDetallePreparacionPedidos(decimal iddetallepedido)
        //{
        //    appLogica.MKT _spn;
        //    _spn = new appLogica.MKT();
        //    return _spn.obtieneDetallePreparacionPedidos(iddetallepedido);
        //}

        //public RESOPE obtieneDatosPartida(string articulo, string partida, decimal idalmacen)
        //{
        //    appLogica.appDB2 _appas = null;
        //    try
        //    {
        //        _appas = new appLogica.appDB2();
        //        return _appas.obtieneDatosPartida(articulo, partida, idalmacen);
        //    }
        //    catch (Exception ex)
        //    {
        //        EscribeLog(ex.Message);
        //        return new RESOPE() { ESTOPE = false, MENERR = ex.Message };
        //    }
        //    finally
        //    {
        //        if (_appas != null)
        //        {
        //            _appas.Finaliza();
        //            _appas = null;
        //        }
        //    }
        //}

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
    }
}
