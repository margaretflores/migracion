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
    /// Lógica de interacción para Finalizapreparacion.xaml
    /// </summary>
    public partial class Finalizapreparacion : Window
    {
        public Finalizapreparacion()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        #region Variables
        public decimal bultos = 0;
        public decimal tarades = 0;
        #endregion

        #region Eventos
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtbultos.Text = "0";
            txttara.Text = "0";
        }
        private void btnaceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bultos = decimal.Parse(txtbultos.Text.Trim());
                tarades = decimal.Parse(txttara.Text.Trim());
            }
            catch (Exception)
            {
                bultos = 0;
                tarades = 0;
            }
            finally
            {
                DialogResult = true;
                Close();
            }

        }
        private void btncancelar_Click(object sender, RoutedEventArgs e) => Close();
        private void txtbultos_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtbultos.Text.Trim()))
                {
                    txtbultos.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txttara_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(txttara.Text.Trim()))
                {
                    txttara.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtbultos_KeyDown(object sender, KeyEventArgs e) => noletras2(e);
        private void txttara_KeyDown(object sender, KeyEventArgs e) => noletras(e);

        #endregion

        #region Metodos
        public void noletras(KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    this.Close();
                }
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemPeriod || e.Key == Key.Decimal)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void noletras2(KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    this.Close();
                }
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ;
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}
