using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

using appfe.appServicio;
using System.ServiceModel;
using System.ServiceModel.Channels;
using JWT;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.IO;
using JWT.Serializers;
using System.ServiceModel.Description;

namespace appfe
{
    public class ParametrosFe
    {
        //public DataTable Menugeneral { get; set; }
        public string PaginaInicial { get; set; }
        public string NombreUsuario { get; set; }
        public static string Usuario { get; set; }
        //public static string Usuario = "REXTECH";
        public string UserHostAddress { get; set; }
        public string IP4Address { get; set; }

        private bool OcultaErrorReal;
        public decimal PorcentajeExcesoCorto { get; internal set; }
        public string CalidadesTipoArtExcluir { get; internal set; }

        public string DefaultOrigenPartido { get; internal set; }

        public IappServiceClient IniciaNuevoCliente()
        {
            string UriServicio;
            UriServicio = ConfigurationManager.AppSettings["UriServicioPedNac"];
            //26062018
            decimal _porcentajeExcesoCorto;

            //OcultaErrorReal = ConfigurationManager.AppSettings["OcultaErrorReal"].Equals("1") ? true : false;

            //decimal.TryParse(ConfigurationManager.AppSettings["PorcentajeExcesoCorto"], out _porcentajeExcesoCorto);
            //PorcentajeExcesoCorto = _porcentajeExcesoCorto / 100;
            //CalidadesTipoArtExcluir = ConfigurationManager.AppSettings["CalidadesTipoExcluir"] as string;
            //DefaultOrigenPartido = ConfigurationManager.AppSettings["DefaultOrigenPartido"] as string;

            //IappServiceClient cliente = new IappServiceClient();
            //cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(UriServicio), cliente.Endpoint.Address.Identity, cliente.Endpoint.Address.Headers);
            //return cliente;

            IappServiceClient cliente = new IappServiceClient();

            cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(UriServicio), cliente.Endpoint.Address.Identity, cliente.Endpoint.Address.Headers);

            //if (HttpContext.Current == null || HttpContext.Current.Session["Token"] == null || HttpContext.Current.Session["Token"].ToString().Trim() == string.Empty)
            //{
            //    HttpContext.Current.Session["Token"] = Security ?? "";
            //}
            EndpointAddressBuilder builder = new EndpointAddressBuilder(cliente.Endpoint.Address);
            AddressHeader header = AddressHeader.CreateAddressHeader("Token", "", appfe.Token.security);
            builder.Headers.Add(header);
            cliente.Endpoint.Address = builder.ToEndpointAddress();
            var mensajeBehavior = new InspectorBehavior(Usuario, "", appfe.Token.security ?? "");
            cliente.Endpoint.Behaviors.Add(mensajeBehavior);

            return cliente;
        }
        public IappServiceClient IniciaNuevoCliente(string _Key)
        {
            decimal _porcentajeExcesoCorto;
            string UriServicio;
            UriServicio = ConfigurationManager.AppSettings["UriServicio"];
            OcultaErrorReal = ConfigurationManager.AppSettings["OcultaErrorReal"].Equals("1") ? true : false;

            decimal.TryParse(ConfigurationManager.AppSettings["PorcentajeExcesoCorto"], out _porcentajeExcesoCorto);
            PorcentajeExcesoCorto = _porcentajeExcesoCorto / 100;
            CalidadesTipoArtExcluir = ConfigurationManager.AppSettings["CalidadesTipoExcluir"] as string;
            DefaultOrigenPartido = ConfigurationManager.AppSettings["DefaultOrigenPartido"] as string;
            IappServiceClient cliente = new IappServiceClient();

            cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(UriServicio), cliente.Endpoint.Address.Identity, cliente.Endpoint.Address.Headers);
            EndpointAddressBuilder builder = new EndpointAddressBuilder(cliente.Endpoint.Address);
            AddressHeader header = AddressHeader.CreateAddressHeader("Token", "", _Key);
            builder.Headers.Add(header);
            cliente.Endpoint.Address = builder.ToEndpointAddress();
            var mensajeBehavior = new InspectorBehavior(Usuario, "", _Key);
            cliente.Endpoint.Behaviors.Add(mensajeBehavior);

            return cliente;
        }

        public void FinalizaCliente(IappServiceClient cliente)
        {
            try
            {
                if (cliente != null)
                {
                    cliente.Close();
                }
            }
            catch { }
            cliente = null;
        }

        public string ErrorGenerico(string exception)
        {
            if (OcultaErrorReal)
            {
                return exception; // Mensajes.MENSAJE_ERROR_GENERICO;
            }
            else
            {
                return exception;
            }
        }

    }

    //
    public class MyMessageInspector : IClientMessageInspector
    {
        private string Usuario { get; set; }
        private string Pass { get; set; }
        private string ViejoToken { get; set; }
        public string NuevoToken { get; set; }
        public MyMessageInspector()
        {
            NuevoToken = string.Empty;
        }
        public MyMessageInspector(string _usuario, string _pass, string _viejoToken)
        {
            Usuario = _usuario;
            Pass = _pass;
            ViejoToken = _viejoToken;
        }
        private bool TokenModificado = false;
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                //HttpContext.Current.Response.Redirect("~/login.aspx");
            }

        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {

            if (ViejoToken.Trim() != string.Empty)
            {
                //validarToken(ViejoToken);
                if (TokenModificado == true)
                {
                    request = TransformMessage2(request, NuevoKey);
                    NuevoToken = NuevoKey;
                    appfe.Token.security = NuevoKey;
                }
            }
            return null;
        }

        private Message TransformMessage2(Message oldMessage, string _security)
        {
            Message newMessage = null;
            MessageBuffer msgbuf = oldMessage.CreateBufferedCopy(int.MaxValue);

            Message tmpMessage = msgbuf.CreateMessage();
            XmlDictionaryReader xdr = tmpMessage.GetReaderAtBodyContents();

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xdr);
            xdr.Close();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);

            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms);
            xdoc.Save(xw);
            xw.Flush();
            xw.Close();

            ms.Position = 0;
            XmlReader xr = XmlReader.Create(ms);
            newMessage = Message.CreateMessage(oldMessage.Version, null, xr);
            newMessage.Headers.Add(MessageHeader.CreateHeader("Token", String.Empty, _security));
            newMessage.Properties.CopyProperties(oldMessage.Properties);
            return newMessage;
        }

        private string NuevoKey { get; set; }
        private void validarToken(string authorizationeader)
        {

            // verificando que el token sea correcto
            byte[] secretKey = Base64UrlDecode(ConfigurationManager.AppSettings["Aplicacion"]);

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var payload = decoder.DecodeToObject<IDictionary<string, object>>(authorizationeader, secretKey, verify: true);
                //if (true)
                //{
                //    var data = 1;
                //    throw new TokenExpiredException("A");
                //}
            }
            catch (TokenExpiredException)
            {
                //string UriServicio = ConfigurationManager.AppSettings["UriServicio"];
                //IappServiceClient cliente = new IappServiceClient();
                //cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(UriServicio), cliente.Endpoint.Address.Identity, cliente.Endpoint.Address.Headers);
                //IappServiceClient clt = cliente;
                //RESOPE resultado;
                ////codigo de operacion
                //PAROPE argumentos = new PAROPE();
                //argumentos.CODOPE = CodigoOperacion.VALIDA_USUARIO;
                //List<string> parEnt = new List<string>();
                //parEnt.Add(ConvertirUTF8(Usuario));  //0 usuario            
                //parEnt.Add(ConvertirUTF8(Pass));       //1 clave, fase cifrar
                //argumentos.VALENT = parEnt.ToArray();


                //resultado = clt.EjecutaOperacion(argumentos);
                //if (resultado.ESTOPE)
                //{
                //    NuevoKey = resultado.VALSAL[resultado.VALSAL.Count() - 1];
                //    TokenModificado = true;
                //}

            }
            catch (SignatureVerificationException)
            {
                //HttpContext.Current.Response.Redirect("~/login.aspx");
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Redirect("~/login.aspx");
            }
        }
        private static string ConvertirUTF8(string value)
        {
            var bytes = Encoding.Default.GetBytes(value);
            var result = Encoding.UTF8.GetString(bytes);
            return result;
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
                    throw new System.Exception("Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder  
        }

    }
    public class InspectorBehavior : IEndpointBehavior
    {
        private string Usuario { get; set; }
        private string Pass { get; set; }
        private string ViejoToken { get; set; }
        public string Token { get; set; }
        public InspectorBehavior()
        {

        }
        public InspectorBehavior(string _usuario, string _pass, string _viejoToken)
        {
            Usuario = _usuario;
            Pass = _pass;
            ViejoToken = _viejoToken;
        }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new MyMessageInspector(Usuario, Pass, ViejoToken);
            clientRuntime.MessageInspectors.Add(inspector);
            Token = inspector.NuevoToken;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
}
