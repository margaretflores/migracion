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

namespace appfe
{
    /// <summary>
    /// Lógica de interacción para Lanzador.xaml
    /// </summary>
    public partial class Lanzador : Window
    {
        ParametrosFe _ParametrosIni;
        //Cada vez que se hagan pruebas pedir un token a Pedro, Amen. 
        string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJSRUFTT05TIiwiaWF0IjoiMTUzNDgwMTM1NCIsImV4cCI6IjE1MzQ4MzczNTQifQ.8tnRGg2yC5KA1HPdNrnam8V24ZYaLubEp1jqc9Hm36U";
        public Lanzador()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            ParametrosFe.Usuario = "REASONS";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        private void BTNComercial_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;
            Pantalla_Principal_Comercial FormPPComercial = new Pantalla_Principal_Comercial();
            FormPPComercial.Owner = Window.GetWindow(this);
            FormPPComercial.Show();
        }

        private void BTNAlmacen_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Pantalla_Principal_Almacen FormPPAlmacen = new Pantalla_Principal_Almacen();
            FormPPAlmacen.Owner = Window.GetWindow(this);
            FormPPAlmacen.Show();
        }

        private void BTNPedidos_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Nuevo_Pedido FormPedidos = new Nuevo_Pedido();
            FormPedidos.Owner = Window.GetWindow(this);
            FormPedidos.Show();
        }

        private void BTNUbicaciones_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Gestion_Ubicaciones FormUbicaciones = new Gestion_Ubicaciones();
            FormUbicaciones.Owner = Window.GetWindow(this);
            FormUbicaciones.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    this.Close();
                }
                if ((e.Key == Key.Enter))
                {

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void reportepartidabutton_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Reporte_partida Formreportepartida = new Reporte_partida();
            Formreportepartida.Owner = Window.GetWindow(this);
            Formreportepartida.Show();
           
        }

        private void notificacionesbutton_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Notificaciones Formnotificaciones = new Notificaciones();
            Formnotificaciones.Owner = Window.GetWindow(this);
            Formnotificaciones.Show();
        }

        //private void rubicpartButton_Click(object sender, RoutedEventArgs e)
        //{
        //    EmpaquesPartida formempaques = new EmpaquesPartida();
        //    formempaques.Show();
        //}

        private void rmovimalmaButton_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            SalidasAlmacen formsalidas = new SalidasAlmacen();
            formsalidas.Owner = Window.GetWindow(this);
            formsalidas.Show();
        }

        private void BTNPrpearacion_Click(object sender, RoutedEventArgs e)
        {
            appfe.Token.security = token;

            Pantalla_Principal_Almacen formalmacen = new Pantalla_Principal_Almacen();
            formalmacen.Owner = Window.GetWindow(this);
            formalmacen.oculta = 1;
            formalmacen.Show();
        }
    }
}
