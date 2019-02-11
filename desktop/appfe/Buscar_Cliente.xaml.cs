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
using System.Data;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Buscar_Cliente.xaml
    /// </summary>
    public partial class Buscar_Cliente : Window
    {
        ParametrosFe _ParametrosIni;
        public Buscar_Cliente()
        {
            InitializeComponent();

            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");


        }

        #region "Variables"
        public appWcfService.TCLIE Seleccionado { get; internal set; } //crea un nuevo objeto del tipo TCLIE
        #endregion

        #region "Eventos"
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatosIniciales();
        }
        private void CargaDatosIniciales()
        {
            try
            {
                ClientesdataGrid.CanUserAddRows = false;
                ClientesdataGrid.CanUserDeleteRows = false;
                BuscartextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Buscarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BuscartextBox.Text == "")
                {
                    MessageBox.Show("Debe ingresar al menos un criterio para realizar una búsqueda.", "No se puede realizar búsqueda", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (BuscartextBox.Text.Length <= 3)
                    {
                        MessageBox.Show("Debe ingresar mas de 3 caracteres para realizar una búsqueda.", "No se puede realizar búsqueda", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        BuscaCLiente(BuscartextBox.Text.Trim());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    if (BuscartextBox.IsFocused)
                    {
                        BuscartextBox.Text = "";
                    }
                    else
                    {
                        this.Close();
                    }
                }

                if ((e.Key == Key.Enter))
                {
                    if (BuscartextBox.IsFocused)
                    {
                        if (BuscartextBox.Text.Length <= 3)
                        {
                            MessageBox.Show("Ingrese los datos correctos", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            BuscaCLiente(BuscartextBox.Text);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ClientesdataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                clienteseleccionado();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Aceptarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clienteseleccionado();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Cancelarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ClientesdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region "Metodos"
        public bool BuscaCLiente(string Busqueda)
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_CLIENTE;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Busqueda);
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.TCLIE> ListClientes = appWcfService.Utils.Deserialize<List<appWcfService.TCLIE>>(resultado.VALSAL[1]);
                        this.ClientesdataGrid.ItemsSource = ListClientes;

                        resultadoOpe = true;
                    }
                    else
                    {
                        ClientesdataGrid.ItemsSource = null;
                        //BuscartextBox.Text = "";
                        MessageBox.Show("No se encontraron coincidencias.", "Sin Coincidencias", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        //MostrarMensaje(Mensajes.MENSAJE_TIPO_NO_ENCONTRADO);
                    }
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
        private void clienteseleccionado()
        {
            if (ClientesdataGrid.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar al menos un cliente", "Seleccionar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Seleccionado = ClientesdataGrid.SelectedItem as appWcfService.TCLIE;
                this.DialogResult = true;
                this.Close();
            }
        }

        #endregion


    }
}
