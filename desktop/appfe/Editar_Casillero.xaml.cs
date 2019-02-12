using appfe.appServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using appfe.appServicio;
using appConstantes;
using appWcfService;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Editar_Casillero.xaml
    /// </summary>
    public partial class Editar_Casillero : Window
    {
        ParametrosFe _ParametrosIni;


        public Editar_Casillero()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }

        #region Variables
        public appWcfService.PECASI Casillero = new appWcfService.PECASI();
        #endregion

        #region Eventos
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
                if (e.Key == Key.Enter)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void capacidadtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void alturatextbox_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);

        }

        private void anchotextbox_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);

        }

        private void largotextbox_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }
        private void cancelarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aceptarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    Casillero.CASICAPA = decimal.Parse(capacidadtextbox.Text.Trim());
                    Casillero.CASIALTU = decimal.Parse(alturatextbox.Text.Trim());
                    Casillero.CASILARG = decimal.Parse(largotextbox.Text.Trim());
                    Casillero.CASIANCH = decimal.Parse(anchotextbox.Text.Trim());
                    Casillero.CASIUSMO = ParametrosFe.Usuario;
                    if (GuardaCasillero(Casillero))
                    {
                        Close();
                    } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Metodos
        public bool GuardaCasillero(appWcfService.PECASI casillero)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MODIFICA_CASILLERO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(casillero));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //MessageBox.Show("Se han actualizado correctamente la información del casillero", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
                    resultadoOpe = true;
                }
                else
                {
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    MessageBox.Show(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }
        public void noletras(KeyEventArgs e)
        {
            if ((e.Key == Key.Escape))
            {
                this.Close();
            }
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemPeriod)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        
    }
}
