using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net;
using System.Configuration;
using System.Data;

using appFew.appServicio;
using appConstantes; 

namespace appFew
{
    public class ParametrosFe
    {
        public DataTable Menugeneral { get; set; }
        public string PaginaInicial { get; set; }
        public string NombreUsuario { get; set; }
        public string Usuario { get; set; }

        public string UserHostAddress { get; set; }
        public string IP4Address { get; set; }

        private bool OcultaErrorReal;
        public decimal PorcentajeExcesoCorto { get; internal set; }
        public string CalidadesTipoArtExcluir { get; internal set; }

        public string DefaultOrigenPartido { get; internal set; }

        public IappServiceClient IniciaNuevoCliente()
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
                return Mensajes.MENSAJE_ERROR_GENERICO;
            }
            else
            {
                return exception;
            }
        }

    }
}