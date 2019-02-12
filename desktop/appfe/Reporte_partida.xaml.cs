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
using appWcfService;

namespace appfe
{
    /// <summary>
    /// Lógica de interacción para Reporte_partida.xaml
    /// </summary>
    public partial class Reporte_partida : Window
    {
        ParametrosFe _ParametrosIni;

        public Reporte_partida()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }

        #region Variables

        #endregion

        #region Eventos
        private void buscarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Realizareporte();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //VisorReportePartida Formvisor = new VisorReportePartida();
            //Formvisor.GeneraReporte("7C5564", 30);
            //Formvisor.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //Lanzador formlanzador = new Lanzador();
                //formlanzador.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            selcombobox.Items.Add("Empaques por Partida");
            selcombobox.Items.Add("Movimientos por Partida");
            selcombobox.SelectedIndex = 0;
            partidatextbox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    if (partidatextbox.IsFocused)
                    {
                        partidatextbox.Text = "";
                    }
                    else
                    {
                        Close();
                    }
                }
                if ((e.Key == Key.Enter))
                {
                    if (partidatextbox.IsFocused)
                    {
                        Realizareporte();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Metodos
        void Realizareporte()
        {
            if (partidatextbox.Text == "")
            {
                MessageBox.Show("Debe ingresar un número de partida", "Ingresar partida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                switch (selcombobox.SelectedIndex)
                {
                    case 0:
                        GeneraReporte(partidatextbox.Text.Trim(), decimal.Parse(almacenescombobox.SelectionBoxItem.ToString()));
                        break;

                    case 1:
                        ReporteMovimientosPartida(partidatextbox.Text.Trim(), decimal.Parse(almacenescombobox.SelectionBoxItem.ToString()));
                        break;

                    default:
                        break;
                }
            }
        }
        public bool GeneraReporte(string partida, decimal almacen)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                List<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result> lista = null;

                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.REPORTE_PARTIDA_ALMACEN; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(partida);
                parEnt.Add(Convert.ToString(almacen));
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        lista = Utils.Deserialize<List<appWcfService.USP_REPORTE_EMPAQUES_PARTIDA_Result>>(resultado.VALSAL[1]);
                        VisualizarReporte(lista, "appfe.ReportePartida.rdlc", "DataSetReportePartida");
                        //webBrowser1.NavigateToString(@"<HTML><IFRAME SCROLLING=""YES"" SRC=""MyPDF.pdf""></IFRAME></HTML>");
                        //selestado = DevuelveSeleccionEstado();
                        //Muestrapedidos(selestado); //Actualiza la grilla segun el combo seleccionado
                        //if (ListAnulados.Count > 1) //Envia los mensajes de acuerdo al tamaño de lista enviada.
                        //{
                        //    MessageBox.Show("Se han anulado correctamente los pedidos.", "Anulados", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Se ha anulado correctamente el pedido.", "Anulado", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}

                    }
                    else
                    {
                        //MessageBox.Show("Ha ocurrido un error.", "Falló", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show("No se encontrarón empaques asociados a la partida ingresada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        public bool ReporteMovimientosPartida(string partida, decimal almacen)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                List<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result> lista = null;

                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.REPORTE_MOVIMIENTOS_PARTIDA; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(partida);
                parEnt.Add(Convert.ToString(almacen));
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        lista = Utils.Deserialize<List<appWcfService.USP_REPORTE_MOVIMIENTOS_PARTIDA_Result>>(resultado.VALSAL[1]);
                        VisualizarReporte(lista, "appfe.MovimientosPartida.rdlc", "DataSetMovimientosPartida");
                        //webBrowser1.NavigateToString(@"<HTML><IFRAME SCROLLING=""YES"" SRC=""MyPDF.pdf""></IFRAME></HTML>");
                        //selestado = DevuelveSeleccionEstado();
                        //Muestrapedidos(selestado); //Actualiza la grilla segun el combo seleccionado
                        //if (ListAnulados.Count > 1) //Envia los mensajes de acuerdo al tamaño de lista enviada.
                        //{
                        //    MessageBox.Show("Se han anulado correctamente los pedidos.", "Anulados", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Se ha anulado correctamente el pedido.", "Anulado", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}

                    }
                    else
                    {
                        //MessageBox.Show("Ha ocurrido un error.", "Falló", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show("No se encontrarón movimientos de la partida ingresada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        public void VisualizarReporte(object datos, string path, string nombreDataSet)
        {
            string fileName;
            Microsoft.Reporting.WinForms.Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            //var viewer = _reportViewer; //  new Microsoft.Reporting.WinForms.ReportViewer();
            _reportViewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            _reportViewer.LocalReport.ReportEmbeddedResource = path;
            Microsoft.Reporting.WinForms.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource(nombreDataSet, datos);

            //rds.Name = nombreDataSet;
            //rds.Value = datos;
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.DataSources.Add(rds);
            //viewer.LocalReport.Refresh();
            _reportViewer.RefreshReport();

            //byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
            //    out streamIds, out warnings);
            //fileName = GetTemporaryDirectory() + ".pdf";
            //using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            //System.Diagnostics.Process.Start(fileName);
        }

        #endregion

    }
}
