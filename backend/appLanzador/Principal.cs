using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

using appWcfService;
using appConstantes;
using System.IO;

namespace appLanzador
{
    public class Principal
    {

        public Principal()
        {

        }

      
        public RESOPE EjecutaOperacion(PAROPE paramOperacion)
        {
            appLogica.MKT _spn;
            appLogica.Principal _principal;

            RESOPE vpar;

            _spn = null;
            vpar = new RESOPE();
            vpar.ESTOPE = false;
            try
            {
                switch (paramOperacion.CODOPE)
                {
                    case CodigoOperacion.VALIDA_USUARIO:
                        _principal = new appLogica.Principal();
                        vpar = _principal.ValidaUsuario(paramOperacion);
                        break;
                    case CodigoOperacion.CAMBIAR_PASSWORD:
                        _principal = new appLogica.Principal();
                        vpar = _principal.CambiaClave(paramOperacion);
                        break;
                    case CodigoOperacion.MOSTRAR_PEDIDOS_APP:
                        _spn = new appLogica.MKT();
                        vpar = _spn.MostrarPedidosApp(paramOperacion);
                        break;
                    default:
                        vpar.MENERR = "OPERACION NO DEFINIDA";
                        break;
                }


            }
            catch (Exception ex)
            {
                vpar.MENERR = ex.Message;
                Util.EscribeLog(ex.Message);
                //throw ex;
            }
            finally
            {
                if (_spn != null)
                {
                    _spn.Finaliza();
                    _spn = null;
                }
            }

            return vpar;

        }

    }
}
