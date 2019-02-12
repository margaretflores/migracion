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
    /// Interaction logic for Gestion_Ubicaciones.xaml
    /// </summary>
    public partial class Gestion_Ubicaciones : Window
    {
        ParametrosFe _ParametrosIni;

        public Gestion_Ubicaciones()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }

        #region Variables

        List<appWcfService.PECASI> Listcasilleros = new List<appWcfService.PECASI>(); // Almacena los casilleros
        List<appWcfService.PENIVE> Listgrilla = new List<appWcfService.PENIVE>(); //almaclaniveles
        List<appWcfService.PECOLU> Listgrillacolu = new List<appWcfService.PECOLU>(); //
        List<appWcfService.PEPASI> Listgrillapasi = new List<appWcfService.PEPASI>(); //

        appWcfService.PECASI SelecCasi = new appWcfService.PECASI();
        appWcfService.PEPASI SelecPasillo = new appWcfService.PEPASI();
        appWcfService.PECOLU SelecColu = new appWcfService.PECOLU();
        appWcfService.PENIVE SelecNive = new appWcfService.PENIVE();

        #endregion

        #region Eventos

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatosIniciales();
        }

        private void CargaDatosIniciales()
        {
            try
            {
                PasillosdataGrid.CanUserAddRows = false;
                NivelesdataGrid.CanUserAddRows = false;
                ColumnasdataGrid.CanUserAddRows = false;
                CasillerosdataGrid.CanUserAddRows = false;

                MuestraPasillos(); //Carga los pasillos
                CasMuestraPasillos(); //Carga pasillos en la seccion de casilleros

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NivPasillocomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //////Muestra niveles segun el pasillo seleccionado
            try
            {
                SelecPasillo = NivPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo != null)
                {
                    MuestraNiveles(SelecPasillo.PASIIDPA);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ColPasillocomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo != null)
                {
                    MuestraColumnas(SelecPasillo.PASIIDPA);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PasAgregarbutton_Click(object sender, RoutedEventArgs e)
        {
            decimal pasillo = 0;
            // decimal faltante = 0;

            if (PasillosdataGrid.Items.Count != 0)
            {
                Listgrillapasi = PasillosdataGrid.ItemsSource.Cast<appWcfService.PEPASI>().ToList();
                //int count = 0;

                foreach (var item in Listgrillapasi)
                    //{
                    //    count++;
                    //    if (item.PASIIDPA != count)
                    //    {
                    //        if (faltante == 0)
                    //        {
                    //            faltante = count;
                    //        }
                    //        //break;
                    //        count++;
                    //    }
                    //    else pasillo = item.PASIIDPA;
                    //}
                    pasillo = item.PASIIDPA;
            }
            //if (faltante != 0)
            //{
            //    if (MessageBox.Show("¿Desea agregar un nuevo pasillo?", "Pasillo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //    {
            //        AgregaPasillos(pasillo.ToString());
            //    }
            //    else AgregaPasillos((faltante - 1).ToString());
            //}
            //else AgregaPasillos(pasillo.ToString());
            AgregaPasillos(pasillo.ToString());
            MuestraPasillos(); //Carga los pasillos
            CasMuestraPasillos();
        }

        // CASILLERO
        private void CasPasillocomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo != null)
                {
                    CasMuestraColumnas(SelecPasillo.PASIIDPA);
                    SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                    CasMuestraNiveles(SelecPasillo.PASIIDPA);
                    SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;

                    if (SelecColu != null && SelecNive != null)
                    {
                        MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                    }
                    else
                    {
                        CasillerosdataGrid.ItemsSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CasNivelcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                if (SelecNive != null && SelecColu != null)
                {
                    MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                }
                else
                {
                    CasillerosdataGrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CasColumnacomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                if (SelecNive != null && SelecColu != null)
                {
                    MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                }
                else
                {
                    CasillerosdataGrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CasillerosdataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (CasillerosdataGrid.SelectedItem != null)
                {
                    SelecCasi = CasillerosdataGrid.SelectedItem as appWcfService.PECASI;
                    Editar_Casillero Formeditar = new Editar_Casillero();
                    Formeditar.labeltitulo.Text = "Código de casillero: " + SelecCasi.CASICOCA;
                    Formeditar.capacidadtextbox.Text = SelecCasi.CASICAPA.ToString();
                    Formeditar.alturatextbox.Text = SelecCasi.CASIALTU.ToString();
                    Formeditar.anchotextbox.Text = SelecCasi.CASIANCH.ToString();
                    Formeditar.largotextbox.Text = SelecCasi.CASILARG.ToString();
                    Formeditar.Casillero = SelecCasi;
                    Formeditar.Owner = Window.GetWindow(this);
                    Formeditar.ShowDialog();
                    SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                    SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                    SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                    if (SelecNive != null && SelecColu != null)
                    {
                        MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                    }
                    else
                    {
                        CasillerosdataGrid.ItemsSource = null;
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
            if (e.Key == Key.Escape)
            {
                Close();
            }
            if (e.Key == Key.Enter)
            {

            }
        }

        private void NivAgregarbutton_Click(object sender, RoutedEventArgs e)
        {
            string nivel = "";
            SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
            if (SelecPasillo.PASIESTA == 1)
            {
                if (NivelesdataGrid.Items.Count != 0)
                {
                    Listgrilla = NivelesdataGrid.ItemsSource.Cast<appWcfService.PENIVE>().ToList();
                    foreach (var item in Listgrilla)
                    {
                        nivel = item.NIVEIDNI;
                    }
                }
                if (!nivel.Equals("Z"))
                {
                    AgregaNiveles(NivPasillocomboBox.SelectedValue.ToString(), nivel);
                }
                else
                    MessageBox.Show("No se pueden crear mas niveles.", "Exceso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El pasillo está desactivado no se pueden realizar acciones de agregación.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));
            CasMuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));
        }

        private void ColAgregarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo.PASIESTA == 1)
                {
                    decimal columna = 0;
                    if (ColumnasdataGrid.Items.Count != 0)
                    {
                        Listgrillacolu = ColumnasdataGrid.ItemsSource.Cast<appWcfService.PECOLU>().ToList();
                        foreach (var item in Listgrillacolu)
                        {
                            columna = item.COLUIDCO;
                        }
                    }
                    AgregaColumna(ColPasillocomboBox.SelectedValue.ToString(), columna.ToString());
                    MuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
                    CasMuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
                }
                else
                {
                    MessageBox.Show("El Pasillo está desactivado no se pueden realizar acciones de agregación.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }

        }

        private void CasAgregarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CasNivelcomboBox.IsEnabled && CasColumnacomboBox.IsEnabled)
                {
                    SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                    SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                    SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                    if (SelecPasillo != null && SelecNive != null && SelecColu != null)
                    {
                        if (AgregaCasillero(SelecPasillo.PASIIDPA, SelecNive.NIVEIDNI, SelecColu.COLUIDCO, ParametrosFe.Usuario))
                        {
                            MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                            if (MessageBox.Show("Se agregó correctamente el Casillero, ¿Desea modificar sus Dimensiones?", "Ingresar dimensiones?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                CasillerosdataGrid.SelectedIndex = 0;
                                SelecCasi = CasillerosdataGrid.SelectedItem as appWcfService.PECASI;
                                Editar_Casillero Formeditar = new Editar_Casillero();
                                Formeditar.labeltitulo.Text = "Código de Casillero: " + SelecCasi.CASICOCA;
                                Formeditar.capacidadtextbox.Text = SelecCasi.CASICAPA.ToString();
                                Formeditar.alturatextbox.Text = SelecCasi.CASIALTU.ToString();
                                Formeditar.anchotextbox.Text = SelecCasi.CASIANCH.ToString();
                                Formeditar.largotextbox.Text = SelecCasi.CASILARG.ToString();
                                Formeditar.Casillero = SelecCasi;
                                Formeditar.Owner = Window.GetWindow(this);
                                Formeditar.ShowDialog();
                                SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                                SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                                SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                                MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                            }

                        }
                    }
                }
                else
                {
                    if (CasNivelcomboBox.IsEnabled == false && CasColumnacomboBox.IsEnabled == true)
                    {
                        MessageBox.Show("No es posible agregar un Casillero si no ha seleccionado ningún Nivel", "Sin niveles", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (CasColumnacomboBox.IsEnabled == false && CasNivelcomboBox.IsEnabled == true)
                        {
                            MessageBox.Show("No es posible agregar un Casillero si no ha seleccionado ninguna Columna", "Sin columna", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            MessageBox.Show("No es posible agregar un Casillero si no ha seleccionado ninguna Columna y ningún Nivel", "Sin columna ni nivel", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CasRemoverbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CasillerosdataGrid.SelectedIndex = 0;
                SelecCasi = CasillerosdataGrid.SelectedItem as appWcfService.PECASI;
                if (MessageBox.Show("¿Desea deshabilitar este casillero?", "Deshabilitar?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<PECASI> Listcasilleros = new List<PECASI>();
                    Listcasilleros.Add(SelecCasi);

                    if (DeshabilitarCasillero(Listcasilleros, ParametrosFe.Usuario))
                    {
                        SelecPasillo = CasPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                        SelecNive = CasNivelcomboBox.SelectedItem as appWcfService.PENIVE;
                        SelecColu = CasColumnacomboBox.SelectedItem as appWcfService.PECOLU;
                        MuestraCasilleros(SelecPasillo.PASIIDPA, SelecColu.COLUIDCO, SelecNive.NIVEIDNI);
                        //MessageBox.Show("Se ha deshabilitado el casillero", "Casillero deshabilitado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PasEliminarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Se eliminará el último Pasillo ingresado.", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    decimal pasillo = 0;

                    if (PasillosdataGrid.Items.Count != 0)
                    {
                        Listgrillapasi = PasillosdataGrid.ItemsSource.Cast<appWcfService.PEPASI>().ToList();
                        foreach (var item in Listgrillapasi)
                        {
                            pasillo = item.PASIIDPA;
                        }
                    }
                    if (pasillo != 0)
                    {
                        //elimina el ultimo de la fila
                        if (DevuelveLista(pasillo, "nivel") || DevuelveLista(pasillo, "columna"))
                        {
                            if (MessageBox.Show("No se puede eliminar el Pasillo ya que contiene casilleros activos ¿Desea desactivar el Pasillo (Esto incluirá la desactivación de Niveles, Columnas y Casilleros)?.", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                //deshabilitar pasillos.
                            }
                            //NivEliminarrbutton.Content = "Deshabilitar";
                        }
                        else
                        {
                            eliminaPasillo(pasillo.ToString());
                        }
                    }
                    else
                        MessageBox.Show("No hay pasillo que eliminar.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
                    MuestraPasillos(); //Carga los pasillos
                    CasMuestraPasillos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void NivEliminarrbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo.PASIESTA == 1)
                {
                    if (MessageBox.Show("Se eliminará el último Nivel ingresado", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        string nivel = "";
                        appWcfService.PENIVE Nive = new appWcfService.PENIVE();
                        SelecPasillo = NivPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                        if (NivelesdataGrid.Items.Count != 0)
                        {
                            Listgrilla = NivelesdataGrid.ItemsSource.Cast<appWcfService.PENIVE>().ToList();
                            foreach (var item in Listgrilla)
                            {
                                nivel = item.NIVEIDNI;
                                Nive = item;
                            }
                        }
                        if (!nivel.Equals(""))
                        {
                            //elimina el ultimo de la fila
                            if (DevuelveCasilleros(SelecPasillo.PASIIDPA, 0, nivel))
                            {
                                if (Nive.NIVEESTA == 1)
                                {
                                    if (MessageBox.Show("No se puede eliminar el Nivel ya que contiene Casilleros activos ¿Desea desactivar el nivel (Esto incluirá la desactivación de Casilleros)?.", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        ActivaDesactivaNivel(SelecPasillo.PASIIDPA.ToString(), nivel, false, Listcasilleros);
                                    }
                                }
                                else
                                    MessageBox.Show("No se puede eliminar el Nivel ya que contiene Casilleros asociados.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);

                                //NivEliminarrbutton.Content = "Deshabilitar";
                            }
                            else
                            {
                                eliminaNivel(SelecPasillo.PASIIDPA.ToString(), nivel);
                            }
                        }
                        else
                            MessageBox.Show("No hay Niveles que eliminar.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
                        MuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));
                        CasMuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));
                    }
                }
                else
                {
                    MessageBox.Show("El Pasillo está desactivado no se pueden realizar acciones", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void ColEliminarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo.PASIESTA == 1)
                {
                    if (MessageBox.Show("Se Eliminará la última Columna ingresada.", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        decimal columna = 0;
                        appWcfService.PECOLU Colu = new appWcfService.PECOLU();
                        SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                        if (ColumnasdataGrid.Items.Count != 0)
                        {
                            Listgrillacolu = ColumnasdataGrid.ItemsSource.Cast<appWcfService.PECOLU>().ToList();
                            foreach (var item in Listgrillacolu)
                            {
                                columna = item.COLUIDCO;
                                Colu = item;
                            }
                        }
                        if (columna != 0)
                        {
                            //elimina el ultimo de la fila
                            if (DevuelveCasilleros(SelecPasillo.PASIIDPA, columna, ""))
                            {
                                if (Colu.COLUESTA == 1)
                                {
                                    if (MessageBox.Show("No se puede eliminar la columna ya que contiene casilleros activos ¿Desea desactivar la columna (esto incluira la desactivacion de casilleros)?.", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        ActivaDesactivaColumna(SelecPasillo.PASIIDPA.ToString(), columna, false, Listcasilleros);
                                    }
                                    //NivEliminarrbutton.Content = "Deshabilitar";
                                }
                                else
                                    MessageBox.Show("No se puede eliminar la Columna ya que contiene Casilleros asociados.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                eliminaColumna(SelecPasillo.PASIIDPA.ToString(), columna);
                            }
                        }
                        else
                            MessageBox.Show("No hay Niveles que eliminar.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);

                        MuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
                        CasMuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
                    }
                }
                else
                {
                    MessageBox.Show("El Pasillo está desactivado no se pueden realizar acciones", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void Pasillo_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox habilita = sender as CheckBox;
                PEPASI pasi = habilita.DataContext as PEPASI;

                SelecPasillo = PasillosdataGrid.SelectedItem as appWcfService.PEPASI;

                Listgrilla = null;
                Listgrillacolu = null;
                Listcasilleros = null;

                DevuelveLista(SelecPasillo.PASIIDPA, "nivel");
                DevuelveLista(SelecPasillo.PASIIDPA, "columna");
                DevuelveCasilleros(SelecPasillo.PASIIDPA, 0, "");

                if (Listgrilla != null && Listgrillacolu != null)// pasillo tiene nivees y columnas
                {
                    if (Listcasilleros != null)
                    {
                        if (habilita.IsChecked == true)//todo
                        {
                            if (MessageBox.Show("Este pasillo contiene Niveles y Columnas asociados ¿Desea Activar el Pasillo?, Se activará todo lo asociado al Pasillo.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), Listcasilleros, Listgrilla, Listgrillacolu, true);
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Este pasillo contiene Niveles y Columnas asociados ¿Desea Desactivar el Pasillo?, Se desactivará todo lo asociado al Pasillo.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), Listcasilleros, Listgrilla, Listgrillacolu, false);
                            }
                        }
                    }
                    else
                    {
                        if (habilita.IsChecked == true)//deshabilita nivel y columna y pasillo
                        {
                            if (MessageBox.Show("Este pasillo contiene Niveles y Columnas asociados ¿Desea Activar el Pasillo?, Se activará todo lo asociado al Pasillo.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, Listgrilla, Listgrillacolu, true);
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Este pasillo contiene Niveles y Columnas asociados ¿Desea Desactivar el Pasillo?, Se desactivará todo lo asociado al Pasillo.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, Listgrilla, Listgrillacolu, false);
                            }
                        }
                    }
                }
                else if (Listgrilla != null)//solo nivel y pasillo
                {
                    if (habilita.IsChecked == true)
                    {
                        if (MessageBox.Show("Este Pasillo contiene Niveles asociados ¿Desea Activar el Pasillo?, Se activará todo lo asociado al Pasillo.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, Listgrilla, null, true);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Este Pasillo contiene Niveles asociados ¿Desea Desactivar el Pasillo?, Se desactivará todo lo asociado al Pasillo.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, Listgrilla, null, false);
                        }
                    }
                }
                else //solo columnas y pasillo
                {
                    if (habilita.IsChecked == true)
                    {
                        if (MessageBox.Show("Este Pasillo contiene Columnas asociados ¿Desea Activar el Pasillo?, Se activará todo lo asociado al Pasillo.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, null, Listgrillacolu, true);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Este Pasillo contiene Columnas asociados ¿Desea Desactivar el Pasillo?, Se desactivará todo lo asociado al Pasillo.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ActivaDesactivaPasillo(SelecPasillo.PASIIDPA.ToString(), null, null, Listgrillacolu, false);
                        }
                    }
                }
                CasMuestraPasillos();
                MuestraPasillos();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void Nivel_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox habilita = sender as CheckBox;
                PENIVE nive = habilita.DataContext as PENIVE;

                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                if (SelecPasillo.PASIESTA == 1)
                {
                    if (DevuelveCasilleros(SelecPasillo.PASIIDPA, 0, nive.NIVEIDNI))
                    {
                        if (habilita.IsChecked == true)
                        {
                            if (MessageBox.Show("Este nivel contiene casilleros desactivados ¿Desea Activar el nivel?, Se activarán los casilleros asociados al nivel.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaNivel(SelecPasillo.PASIIDPA.ToString(), nive.NIVEIDNI, true, Listcasilleros);
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Este nivel contiene casilleros activos ¿Desea desactivar el nivel?, Se desactivarán los casilleros asociados al nivel.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaNivel(SelecPasillo.PASIIDPA.ToString(), nive.NIVEIDNI, false, Listcasilleros);
                            }
                        }

                    }
                    else
                    {
                        if (habilita.IsChecked == true)
                        {
                            ActivaDesactivaNivel(SelecPasillo.PASIIDPA.ToString(), nive.NIVEIDNI, true, null);

                        }
                        else
                        {
                            ActivaDesactivaNivel(SelecPasillo.PASIIDPA.ToString(), nive.NIVEIDNI, false, null);

                        }
                    }
                }
                else
                {
                    MessageBox.Show("El pasillo esta desactivado no se pueden realizar acciones", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));
                CasMuestraNiveles(Decimal.Parse(NivPasillocomboBox.SelectedValue.ToString()));

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void Columna_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox habilita = sender as CheckBox;
                PECOLU colu = habilita.DataContext as PECOLU;

                SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;

                if (SelecPasillo.PASIESTA == 1)
                {
                    if (DevuelveCasilleros(SelecPasillo.PASIIDPA, colu.COLUIDCO, ""))
                    {
                        if (habilita.IsChecked == true)
                        {
                            if (MessageBox.Show("Esta columna contiene casilleros desactivados ¿Desea Activar la columna?, Se activarán los casilleros asociados a la columna.", "Activar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaColumna(SelecPasillo.PASIIDPA.ToString(), colu.COLUIDCO, true, Listcasilleros);
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Esta columna contiene casilleros activos ¿Desea desactivar la columna?, Se desactivarán los casilleros asociados a la columna.", "Desactivar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ActivaDesactivaColumna(SelecPasillo.PASIIDPA.ToString(), colu.COLUIDCO, false, Listcasilleros);
                            }
                        }

                    }
                    else
                    {
                        if (habilita.IsChecked == true)
                        {
                            ActivaDesactivaColumna(SelecPasillo.PASIIDPA.ToString(), colu.COLUIDCO, true, null);

                        }
                        else
                        {
                            ActivaDesactivaColumna(SelecPasillo.PASIIDPA.ToString(), colu.COLUIDCO, false, null);

                        }
                    }
                }
                else
                {
                    MessageBox.Show("El pasillo esta desactivado no se pueden realizar acciones", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
                CasMuestraColumnas(Decimal.Parse(ColPasillocomboBox.SelectedValue.ToString()));
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }

        }

        #endregion

        #region Metodos

        public bool MuestraPasillos()
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PASILLOS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PEPASI> ListPasillos = appWcfService.Utils.Deserialize<List<appWcfService.PEPASI>>(resultado.VALSAL[1]);
                        PasillosdataGrid.ItemsSource = ListPasillos;
                        PasillosdataGrid.SelectedIndex = 0;

                        NivPasillocomboBox.ItemsSource = ListPasillos;
                        NivPasillocomboBox.DisplayMemberPath = "PASIIDPA";
                        NivPasillocomboBox.SelectedValuePath = "PASIIDPA";
                        NivPasillocomboBox.SelectedIndex = 0;
                        SelecPasillo = NivPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                        MuestraNiveles(SelecPasillo.PASIIDPA);


                        ColPasillocomboBox.ItemsSource = ListPasillos;
                        ColPasillocomboBox.DisplayMemberPath = "PASIIDPA";
                        ColPasillocomboBox.SelectedValuePath = "PASIIDPA";
                        ColPasillocomboBox.SelectedIndex = 0;
                        SelecPasillo = ColPasillocomboBox.SelectedItem as appWcfService.PEPASI;
                        MuestraColumnas(SelecPasillo.PASIIDPA);

                        resultadoOpe = true;
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

        public bool MuestraNiveles(decimal idpasillo)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_NIVELES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PENIVE> ListNiveles = appWcfService.Utils.Deserialize<List<appWcfService.PENIVE>>(resultado.VALSAL[1]);
                        if (ListNiveles.Count != 0)
                        {
                            NivelesdataGrid.ItemsSource = null;
                            NivelesdataGrid.ItemsSource = ListNiveles;
                            resultadoOpe = true;
                            NivelesdataGrid.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        NivelesdataGrid.ItemsSource = null;
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

        public bool MuestraColumnas(decimal idpasillo)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_COLUMNAS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECOLU> ListColumnas = appWcfService.Utils.Deserialize<List<appWcfService.PECOLU>>(resultado.VALSAL[1]);
                        ColumnasdataGrid.ItemsSource = null;
                        ColumnasdataGrid.ItemsSource = ListColumnas;
                        resultadoOpe = true;
                        ColumnasdataGrid.SelectedIndex = 0;
                    }
                    else
                    {
                        ColumnasdataGrid.ItemsSource = null;
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

        //CASILLEROS
        public bool CasMuestraPasillos()
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PASILLOS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PEPASI> ListPasillos = appWcfService.Utils.Deserialize<List<appWcfService.PEPASI>>(resultado.VALSAL[1]);
                        List<appWcfService.PEPASI> ListPasi = new List<appWcfService.PEPASI>();
                        foreach (var item in ListPasillos)
                        {
                            if (item.PASIESTA == 1)
                            {
                                ListPasi.Add(item);
                            }
                        }

                        CasPasillocomboBox.ItemsSource = ListPasi;
                        CasPasillocomboBox.DisplayMemberPath = "PASIIDPA";
                        CasPasillocomboBox.SelectedValuePath = "PASIIDPA";
                        CasPasillocomboBox.SelectedIndex = 0;
                        resultadoOpe = true;
                    }
                    else
                    {
                        CasPasillocomboBox.ItemsSource = null;
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

        public bool CasMuestraNiveles(decimal idpasillo)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_NIVELES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        CasNivelcomboBox.IsEnabled = true;
                        List<appWcfService.PENIVE> ListNiveles = appWcfService.Utils.Deserialize<List<appWcfService.PENIVE>>(resultado.VALSAL[1]);
                        List<appWcfService.PENIVE> ListNive = new List<appWcfService.PENIVE>();
                        foreach (var item in ListNiveles)
                        {
                            if (item.NIVEESTA == 1)
                            {
                                ListNive.Add(item);
                            }
                        }
                        CasNivelcomboBox.ItemsSource = ListNive;
                        CasNivelcomboBox.DisplayMemberPath = "NIVEIDNI";
                        CasNivelcomboBox.SelectedValuePath = "NIVEIDNI";
                        CasNivelcomboBox.SelectedIndex = 0;
                        resultadoOpe = true;

                    }
                    else
                    {
                        CasNivelcomboBox.ItemsSource = null;
                        CasNivelcomboBox.IsEnabled = false;
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

        public bool CasMuestraColumnas(decimal idpasillo)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_COLUMNAS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        CasColumnacomboBox.IsEnabled = true;
                        List<appWcfService.PECOLU> ListColumnas = appWcfService.Utils.Deserialize<List<appWcfService.PECOLU>>(resultado.VALSAL[1]);
                        List<appWcfService.PECOLU> ListColu = new List<appWcfService.PECOLU>();
                        foreach (var item in ListColumnas)
                        {
                            if (item.COLUESTA == 1)
                            {
                                ListColu.Add(item);
                            }
                        }
                        CasColumnacomboBox.ItemsSource = ListColu;
                        CasColumnacomboBox.DisplayMemberPath = "COLUIDCO";
                        CasColumnacomboBox.SelectedValuePath = "COLUIDCO";
                        CasColumnacomboBox.SelectedIndex = 0;
                        resultadoOpe = true;
                    }
                    else
                    {
                        CasColumnacomboBox.ItemsSource = null;
                        CasColumnacomboBox.IsEnabled = false;
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

        public bool MuestraCasilleros(decimal idpasillo, decimal idcolumna, string idnivel)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_CASILLEROS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());
                parEnt.Add(idcolumna.ToString());
                parEnt.Add(idnivel);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECASI> ListCasilleros = appWcfService.Utils.Deserialize<List<appWcfService.PECASI>>(resultado.VALSAL[1]);
                        CasillerosdataGrid.ItemsSource = null;
                        CasillerosdataGrid.ItemsSource = ListCasilleros;
                        resultadoOpe = true;
                        CasillerosdataGrid.SelectedIndex = 0;
                    }
                    else
                    {
                        CasillerosdataGrid.ItemsSource = null;
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

        public bool AgregaPasillos(string pasillo)
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
                argumentos.CODOPE = CodigoOperacion.AGREGA_PASILLO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(pasillo);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("Se Agregó Pasillo correctamente", "Agregar", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool AgregaNiveles(String pasillo, string nivel)

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
                argumentos.CODOPE = CodigoOperacion.AGREGA_NIVEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(pasillo);
                parEnt.Add(nivel);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("Se Agregó Nivel correctamente", "Niveles", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool AgregaColumna(String pasillo, string columna)
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
                argumentos.CODOPE = CodigoOperacion.AGREGA_COLUMNA;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(pasillo);
                parEnt.Add(columna);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("Se Agregó Columna correctamente", "Columna", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool AgregaCasillero(decimal idpasillo, string idnivel, decimal idcolumna, string usuario)
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
                argumentos.CODOPE = CodigoOperacion.AGREGA_CASILLERO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());
                parEnt.Add(idnivel);
                parEnt.Add(idcolumna.ToString());
                parEnt.Add(usuario);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //MessageBox.Show("Se Agregó el casillero correctamente", "Casillero", MessageBoxButton.OK, MessageBoxImage.Information);
                    resultadoOpe = true;
                }
                else
                {
                    MessageBox.Show("Ya existe un casillero asociado a este pasillo, columna, nivel.", "Casillero Existente", MessageBoxButton.OK, MessageBoxImage.Information);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    //MessageBox.Show(resultado.MENERR);
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

        public bool DeshabilitarCasillero(List<appWcfService.PECASI> casillero, string usuario)
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
                argumentos.CODOPE = CodigoOperacion.DESHABILITA_CASILLERO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(casillero));
                parEnt.Add(usuario);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //MessageBox.Show("Se Agregó el casillero correctamente", "Casillero", MessageBoxButton.OK, MessageBoxImage.Information);
                    resultadoOpe = true;
                }
                else
                {
                    MessageBox.Show("No se ha podido deshabilitar el casillero", "Falló al deshabilitar", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    //MessageBox.Show(resultado.MENERR);
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

        public bool DevuelveCasilleros(decimal idpasillo, decimal idcolumna, string idnivel)
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
                argumentos.CODOPE = CodigoOperacion.DEVUELVE_CASILLEROS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());
                parEnt.Add(idcolumna.ToString());
                parEnt.Add(idnivel);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        Listcasilleros = null;
                        Listcasilleros = appWcfService.Utils.Deserialize<List<appWcfService.PECASI>>(resultado.VALSAL[1]);
                        //List<appWcfService.PECASI> Listcasilleros = appWcfService.Utils.Deserialize<List<appWcfService.PECASI>>(resultado.VALSAL[1]);
                        resultadoOpe = true;
                    }
                }
                else
                {
                    //MessageBox.Show("No se ha podido deshabilitar el casillero", "Falló al deshabilitar", MessageBoxButton.OK, MessageBoxImage.Error);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    //MessageBox.Show(resultado.MENERR);
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

        public bool eliminaNivel(string pasillo, string nivel)
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
                argumentos.CODOPE = CodigoOperacion.ELIMINA_NIVEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(pasillo);
                parEnt.Add(nivel);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {

                    MessageBox.Show("Se eliminó nivel correctamente", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool eliminaColumna(string pasillo, decimal columna)
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
                argumentos.CODOPE = CodigoOperacion.ELIMINA_COLUMNA;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(pasillo);
                parEnt.Add(columna.ToString());
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {

                    MessageBox.Show("Se eliminó la columna correctamente", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool DevuelveLista(decimal idpasillo, string informacion)
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
                argumentos.CODOPE = CodigoOperacion.DEVUELVE_NIVE_COLU;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idpasillo.ToString());
                parEnt.Add(informacion);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        if (informacion.Equals("nivel"))
                        {
                            Listgrilla = null;
                            Listgrilla = appWcfService.Utils.Deserialize<List<appWcfService.PENIVE>>(resultado.VALSAL[1]);
                        }
                        else
                        {
                            Listgrillacolu = null;
                            Listgrillacolu = appWcfService.Utils.Deserialize<List<appWcfService.PECOLU>>(resultado.VALSAL[1]);
                        }
                        resultadoOpe = true;
                    }

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

        public bool eliminaPasillo(string pasillo)
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
                argumentos.CODOPE = CodigoOperacion.ELIMINA_PASILLO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(pasillo);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {

                    MessageBox.Show("Se eliminó el pasillo correctamente", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool ActivaDesactivaColumna(string pasillo, decimal columna, bool activa, List<appWcfService.PECASI> casilleros)
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
                argumentos.CODOPE = CodigoOperacion.DESHABILITA_COLUMNA;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(columna.ToString());
                parEnt.Add(pasillo);
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(activa.ToString());
                parEnt.Add(Utils.Serialize(casilleros));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (activa)
                    {
                        if (casilleros == null)
                        {
                            MessageBox.Show("Se Activó la columna correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("Se Activó la columna y los casilleros que contiene correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        if (casilleros == null)
                        {
                            MessageBox.Show("Se Desactivó la columna correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("Se Desactivó la columna y los casilleros que contiene correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
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

        public bool ActivaDesactivaNivel(string pasillo, string nivel, bool activa, List<appWcfService.PECASI> casilleros)
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
                argumentos.CODOPE = CodigoOperacion.DESHABILITA_NIVEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(nivel);
                parEnt.Add(pasillo);
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(activa.ToString());
                parEnt.Add(Utils.Serialize(casilleros));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (activa)
                    {
                        if (casilleros == null)
                        {
                            MessageBox.Show("Se Activó el nivel correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("Se Activó el nivel y los casilleros que contiene correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        if (casilleros == null)
                        {
                            MessageBox.Show("Se Desactivó el nivel correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("Se Desactivó el nivel y los casilleros que contiene correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
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

        public bool ActivaDesactivaPasillo(string pasillo, List<appWcfService.PECASI> casilleros, List<appWcfService.PENIVE> nivel, List<appWcfService.PECOLU> columna, bool activa)
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
                argumentos.CODOPE = CodigoOperacion.DESHABILITA_PASILLO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                parEnt.Add(pasillo);
                parEnt.Add(ParametrosFe.Usuario);
                parEnt.Add(Utils.Serialize(casilleros));
                parEnt.Add(Utils.Serialize(nivel));
                parEnt.Add(Utils.Serialize(columna));
                parEnt.Add(activa.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (activa)
                    {
                        if (nivel != null && columna != null)
                        {
                            if (casilleros != null)
                            {
                                MessageBox.Show("Se Activó el pasillo asi como niveles, columnas y los casilleros que contiene correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                                MessageBox.Show("Se Activó el pasillo, niveles y columnas correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (nivel != null)
                        {
                            MessageBox.Show("Se Activó el pasillo y los niveles que contiene correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Se Activó el pasillo y las columnas que contiene correctamente", "Activación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                    }
                    else
                    {
                        if (nivel != null && columna != null)
                        {
                            if (casilleros != null)
                            {
                                MessageBox.Show("Se Desactivó el pasillo asi como niveles, columnas y los casilleros que contiene correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                                MessageBox.Show("Se Desactivó el pasillo, niveles y columnas correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (nivel != null)
                        {
                            MessageBox.Show("Se Desactivó el pasillo y los niveles que contiene correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Se Desactivó el pasillo y las columnas que contiene correctamente", "Desactivación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
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
        #endregion

    }
}
