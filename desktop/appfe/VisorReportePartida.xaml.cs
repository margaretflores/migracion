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
    public partial class VisorReportePartida : Window
    {
        ParametrosFe _ParametrosIni;

        public VisorReportePartida()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

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
                        VisualizarReporte(lista, "ReportePartida.rdlc", "DataSetReportePartida");
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
                        MessageBox.Show("No se encontró el pedido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            _reportViewer.LocalReport.ReportPath = path;
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
    }
}
