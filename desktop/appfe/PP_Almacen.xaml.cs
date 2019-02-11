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
using System.ComponentModel;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Pantalla_Principal_Almacen.xaml
    /// </summary>
    public partial class Pantalla_Principal_Almacen : Window
    {
        ParametrosFe _ParametrosIni;

        public Pantalla_Principal_Almacen()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        #region "Variables"
        public appWcfService.PECAPE Seleccionado { get; internal set; } //crea un nuevo objeto del tipo PECAPE
        string estado; //Segun el indice se va asignar un estado 
        List<appWcfService.PECAPE> Listgrilla = new List<appWcfService.PECAPE>(); //lista para contener o agregar todos los items de la grilla actual del tipo PECAPE
        List<appWcfService.PEDEPE> Listdetalle = new List<appWcfService.PEDEPE>();
        int index1 = 0; //Indice para datagrid principal
        public int oculta = 0;

        List<PECAPE> lispedidosalmacen = new List<PECAPE>(); // para hilos
        List<PECAPE> listsolicitudes = new List<PECAPE>();
        bool valida = false;
        bool valida1 = false;
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
                BackgroundWorker bk = new BackgroundWorker();
                BusyBar.IsBusy = true;
                bk.DoWork += (o, e) =>
                {
                    MuestraPedidosAlmacen1(); //Muestra los pedidos con estado 2 y 3 
                    MuestrapedidosPendientes1("4"); //Mostrar los pedidos con estado 4
                };
                bk.RunWorkerCompleted += (o, e) =>
                {
                    if (valida)
                    {
                        ListaPedidosdataGrid.ItemsSource = lispedidosalmacen;
                        ListaPedidosdataGrid.CanUserAddRows = false;
                        ListaPedidosdataGrid.SelectedIndex = 0;
                    }
                    //if (oculta == 1)
                    //{
                    //    buscasolgroup.IsEnabled = false;
                    //    solgroup.IsEnabled = false;
                    //}
                    if (valida1)
                    {
                        ListaSolicitudesdataGrid.ItemsSource = listsolicitudes;
                        ListaSolicitudesdataGrid.CanUserAddRows = false;
                    }
                    BuscarPedidotextBox.Focus();
                    estadocomboBox.Items.Add("Pendientes (Emitidos, En Preparación)");
                    estadocomboBox.Items.Add("Emitido");
                    estadocomboBox.Items.Add("En preparación");
                    estadocomboBox.Items.Add("Espera de Aprob.");
                    estadocomboBox.Items.Add("Completado");
                    estadocomboBox.Items.Add("Anulado");
                    estadocomboBox.SelectedIndex = 0;

                    Validaform();
                    validacombo();

                    //DMA 07/05/2018
                    FechaIniciodtp.SelectedDate = primerdiames();
                    FechaFindtp.SelectedDate = DateTime.Now;

                    BusyBar.IsBusy = false;
                };
                bk.RunWorkerAsync();

                //if (oculta == 1)
                //{
                //    buscasolgroup.IsEnabled = false;
                //    solgroup.IsEnabled = false;
                //}
                //BuscarPedidotextBox.Focus();
                //MuestraPedidosAlmacen(); //Muestra los pedidos con estado 2 y 3 
                //MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                //ListaPedidosdataGrid.CanUserAddRows = false;
                //ListaSolicitudesdataGrid.CanUserAddRows = false;

                //estadocomboBox.Items.Add("Todos los estados");
                //estadocomboBox.Items.Add("Emitido");
                //estadocomboBox.Items.Add("En preparación");
                //estadocomboBox.Items.Add("Espera de Aprob.");
                //estadocomboBox.Items.Add("Completado");
                //estadocomboBox.SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void Atenderpedidobutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe Marcar al menos un Pedido para Atenderlo", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listseleccionado = new List<PECAPE>(); //Agrega la lista de seleccionados
                    List<PECAPE> ListCambiaestaseleccionado = new List<PECAPE>(); //Agrega la lista de seleccionados

                    foreach (var item in Listgrilla)
                    {
                        if (item.CAPECHECK == true)
                        {
                            if (item.CAPEIDES == 2)
                            {
                                ListCambiaestaseleccionado.Add(item);
                            }
                            Listseleccionado.Add(item);
                        }
                    }
                    if (Listseleccionado.Count == 0)
                    {
                        MessageBox.Show("Debe Marcar al menos un Pedido para Atenderlo", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (Listseleccionado.Count > 0)
                        {
                            if (Listseleccionado.Count == 1)
                            {
                                if (ListCambiaestaseleccionado.Count == 1)
                                {
                                    if (MessageBox.Show("¿Está seguro de atender este pedido? Su estado cambiará a 'En Preparación'", "Atender pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        if (TrabajaPedido(Listseleccionado))
                                        {
                                            //cambia estado
                                            CambiaEstaList(Listseleccionado, "3", ParametrosFe.Usuario); //Envia la lista a aprobar
                                            //Llenamos el formulario con los datos obtenidos
                                            Atender_Pedido FormAtender = new Atender_Pedido();
                                            FormAtender.ClienteLabel.Content = "CLIENTE: " + Listseleccionado[0].TCLIE.CLINOM.ToString();
                                            FormAtender.DireccionLabel.Text = "DIRECCIÓN: " + Listseleccionado[0].CAPEDIRE.ToString();
                                            FormAtender.FechaLabel.Content = "FECHA: " + Listseleccionado[0].CAPEFECH.ToShortDateString();
                                            FormAtender.NumPedidoLabel.Content = "PEDIDO: " + Listseleccionado[0].CAPESERI.ToString() + " " + Listseleccionado[0].CAPENUMC.ToString();
                                            FormAtender.vistaprevAprob = true;
                                            FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                            FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                            FormAtender.Restituirbutton.IsEnabled = false;
                                            FormAtender.AprobarButton.IsEnabled = false;
                                            FormAtender.Listseleccionados = Listseleccionado;
                                            FormAtender.Owner = Window.GetWindow(this);
                                            FormAtender.ShowDialog();
                                            MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                            Busquedaalmacen();
                                            //MuestraPedidosAlmacen();
                                            FormAtender.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    if (MessageBox.Show("¿Desea continuar con la preparación de este pedido?", "Continuar preparación?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        if (TrabajaPedido(Listseleccionado))
                                        {
                                            //Llenamos el formulario con los datos obtenidos
                                            Atender_Pedido FormAtender = new Atender_Pedido();
                                            FormAtender.ClienteLabel.Content = "CLIENTE: " + Listseleccionado[0].TCLIE.CLINOM.ToString();
                                            FormAtender.DireccionLabel.Text = "DIRECCIÓN: " + Listseleccionado[0].CAPEDIRE.ToString();
                                            FormAtender.FechaLabel.Content = "FECHA: " + Listseleccionado[0].CAPEFECH.ToShortDateString();
                                            FormAtender.NumPedidoLabel.Content = "PEDIDO: " + Listseleccionado[0].CAPESERI.ToString() + " " + Listseleccionado[0].CAPENUMC.ToString();
                                            FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                            FormAtender.vistaprevAprob = true;
                                            FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                            FormAtender.Restituirbutton.IsEnabled = false;
                                            FormAtender.AprobarButton.IsEnabled = false;
                                            FormAtender.Listseleccionados = Listseleccionado;
                                            FormAtender.Owner = Window.GetWindow(this);
                                            FormAtender.ShowDialog();
                                            //FormAtender.Close();
                                            //MuestraPedidosAlmacen();
                                            Busquedaalmacen();
                                            MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                            FormAtender.Close();

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Listseleccionado.Count == ListCambiaestaseleccionado.Count)
                                {
                                    if (MessageBox.Show("¿Está seguro de atender estos pedidos? Sus estados cambiarán a 'En Preparación'", "Atender pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        if (TrabajaPedido(Listseleccionado))
                                        {
                                            //cambia estado
                                            CambiaEstaList(Listseleccionado, "3", ParametrosFe.Usuario); //Envia la lista a aprobar
                                            //Llenamos el formulario con los datos obtenidos
                                            Atender_Pedido FormAtender = new Atender_Pedido();
                                            FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                            FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                            FormAtender.Restituirbutton.IsEnabled = false;
                                            FormAtender.AprobarButton.IsEnabled = false;
                                            FormAtender.Listseleccionados = Listseleccionado;
                                            FormAtender.Owner = Window.GetWindow(this);
                                            FormAtender.ShowDialog();
                                            Busquedaalmacen();
                                            MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                            //MuestraPedidosAlmacen();
                                            FormAtender.Close();

                                        }
                                    }
                                }
                                else
                                {
                                    if (ListCambiaestaseleccionado.Count > 0)
                                    {
                                        if (ListCambiaestaseleccionado.Count == 1)
                                        {
                                            if (MessageBox.Show("En la lista hay un pedido sin preparar,¿Está seguro de preparar este pedido? Su estado cambiará a 'En Preparación'", "Atender pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                            {
                                                if (TrabajaPedido(Listseleccionado))
                                                {
                                                    //cambia estado
                                                    CambiaEstaList(ListCambiaestaseleccionado, "3", ParametrosFe.Usuario); //Envia la lista a aprobar
                                                                                                                           //Llenamos el formulario con los datos obtenidos
                                                    Atender_Pedido FormAtender = new Atender_Pedido();
                                                    FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                                    FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                                    FormAtender.Restituirbutton.IsEnabled = false;
                                                    FormAtender.AprobarButton.IsEnabled = false;
                                                    FormAtender.Listseleccionados = Listseleccionado;
                                                    FormAtender.Owner = Window.GetWindow(this);

                                                    FormAtender.ShowDialog();
                                                    Busquedaalmacen();
                                                    MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                                    //MuestraPedidosAlmacen();
                                                    FormAtender.Close();

                                                }
                                            }
                                        }
                                        else
                                        {

                                            if (MessageBox.Show("En la lista hay " + ListCambiaestaseleccionado.Count + " pedidos sin preparar,¿Está seguro de preparar estos pedidos? Sus estados cambiarán a 'En Preparación'.", "Atender pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                            {
                                                if (TrabajaPedido(Listseleccionado))
                                                {
                                                    //cambia estado
                                                    CambiaEstaList(ListCambiaestaseleccionado, "3", ParametrosFe.Usuario); //Envia la lista a aprobar
                                                                                                                           //Llenamos el formulario con los datos obtenidos
                                                    Atender_Pedido FormAtender = new Atender_Pedido();
                                                    FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                                    FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                                    FormAtender.Restituirbutton.IsEnabled = false;
                                                    FormAtender.AprobarButton.IsEnabled = false;
                                                    FormAtender.Listseleccionados = Listseleccionado;
                                                    FormAtender.Owner = Window.GetWindow(this);

                                                    FormAtender.ShowDialog();
                                                    Busquedaalmacen();
                                                    MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                                    //MuestraPedidosAlmacen();
                                                    FormAtender.Close();

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (MessageBox.Show("¿Desea continuar con la preparación de estos pedidos?", "Continuar preparación?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                        {
                                            if (TrabajaPedido(Listseleccionado))
                                            {
                                                //Llenamos el formulario con los datos obtenidos
                                                Atender_Pedido FormAtender = new Atender_Pedido();
                                                FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                                                FormAtender.DetallePedidodataGrid.CanUserAddRows = false;
                                                FormAtender.Restituirbutton.IsEnabled = false;
                                                FormAtender.AprobarButton.IsEnabled = false;
                                                FormAtender.Listseleccionados = Listseleccionado;
                                                FormAtender.Owner = Window.GetWindow(this);

                                                FormAtender.ShowDialog();
                                                FormAtender.Close();
                                                Busquedaalmacen();
                                                //MuestraPedidosAlmacen();
                                                MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                                FormAtender.Close();

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuscarPedidobutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Busquedaalmacen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuscarSolicitudbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busquedaenaprobacion(BuscarSolicitudtextBox.Text.Trim());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void limpiarsolicitudestextbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BuscarSolicitudtextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void limpiarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BuscarPedidotextBox.Text = "";
                TextBoxSerie.Text = "";
                estadocomboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Imprimirbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar al menos un Pedido para Imprimir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listimprimir = new List<PECAPE>(); //Guardamos items para imprimir
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listimprimir.Add(item);
                        }
                    }
                    if (Listimprimir.Count > 0)
                    {
                        if (Listimprimir.Count > 1)
                        {
                            //if (MessageBox.Show("¿Está seguro que desea imprimir estos pedidos?", "Imprimir pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            //{
                            foreach (var item in Listimprimir)
                            {
                                ImprimePedido(item.CAPEIDCP);
                            }
                            //}
                        }
                        else
                        {
                            //if (MessageBox.Show("¿Está seguro que desea imprimir este pedido?", "Imprimir pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            //{
                            foreach (var item in Listimprimir)
                            {
                                ImprimePedido(item.CAPEIDCP);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un Pedido para Imprimir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                Busquedaalmacen();
                MuestrapedidosPendientes("4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListaPedidosdataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
                List<PECAPE> Listseleccionado = new List<PECAPE>(); //Agrega la lista de seleccionados
                index1 = ListaPedidosdataGrid.SelectedIndex;
                if (Seleccionado != null)
                {
                    Listseleccionado.Add(Seleccionado);
                    VistaPrevia(Listseleccionado);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ListaSolicitudesdataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Seleccionado = ListaSolicitudesdataGrid.SelectedItem as appWcfService.PECAPE;
                List<PECAPE> Listseleccionado = new List<PECAPE>(); //Agrega la lista de seleccionados
                if (Seleccionado != null)
                {
                    Listseleccionado.Add(Seleccionado);
                    VistaPrevia(Listseleccionado);
                }
                busquedaenaprobacion(BuscarSolicitudtextBox.Text.Trim());
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
                if (e.Key == Key.Escape)
                {
                    if (BuscarPedidotextBox.IsFocused)
                    {
                        BuscarPedidotextBox.Text = "";
                    }
                    else if (BuscarSolicitudtextBox.IsFocused)
                    {
                        BuscarSolicitudtextBox.Text = "";
                    }
                    else if (TextBoxSerie.IsFocused)
                    {
                        TextBoxSerie.Text = "";
                    }
                    else
                    {
                        Close();
                    }
                }
                if (e.Key == Key.Enter)
                {
                    if (BuscarPedidotextBox.IsFocused || TextBoxSerie.IsFocused)
                    {
                        Busquedaalmacen();
                    }
                    else if (BuscarSolicitudtextBox.IsFocused)
                    {
                        busquedaenaprobacion(BuscarSolicitudtextBox.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ListaPedidosdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ListaSolicitudesdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void ListaPedidosdataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Seleccionado = ListaPedidosdataGrid.SelectedItem as PECAPE;
                    index1 = ListaPedidosdataGrid.SelectedIndex;
                    if (Seleccionado != null)
                    {
                        List<PECAPE> seleccion = new List<PECAPE>()
                        {
                            Seleccionado
                        };
                        VistaPrevia(seleccion);
                        ListaPedidosdataGrid.SelectedIndex = index1;
                    }
                }
                Busquedaalmacen();
                MuestrapedidosPendientes("4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ListaSolicitudesdataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Seleccionado = ListaSolicitudesdataGrid.SelectedItem as PECAPE;
                    if (Seleccionado != null)
                    {
                        List<PECAPE> seleccion = new List<PECAPE>()
                        {
                            Seleccionado
                        };
                        VistaPrevia(seleccion);
                    }
                }
                Busquedaalmacen();
                busquedaenaprobacion(BuscarSolicitudtextBox.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btneditarpedido_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar un Pedido para editar.", "Marcar Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listeditar = new List<PECAPE>(); //Guardamos items para imprimir
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listeditar.Add(item);
                        }
                    }
                    if (Listeditar.Count == 1)
                    {
                        if (MessageBox.Show("¿Está seguro que desea editar el Pedido marcado?", "Editar Pedido", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            index1 = ListaPedidosdataGrid.SelectedIndex;
                            EditaPedido(Listeditar);
                            Busquedaalmacen();
                            MuestrapedidosPendientes("4");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar un Pedido.", "Marcar un Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reabrirbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar un Pedido para Reabrir.", "Marcar Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listreabrir = new List<PECAPE>(); //Guardamos items para imprimir
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listreabrir.Add(item);
                        }
                    }
                    if (Listreabrir.Count == 1)
                    {
                        if (MessageBox.Show("¿Está seguro de 'Reabrir' este Pedido? Para llevar a cabo esta acción deberá ANULAR PREVIAMENTE la guía asociada a este Pedido.  ", "¿Reabrir?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (CambiaEstaList(Listreabrir, "4", ParametrosFe.Usuario))
                            {
                                Busquedaalmacen();
                                MuestrapedidosPendientes("4");
                                MessageBox.Show("Se ha modificado correctamente el estado del Pedido.", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar un Pedido para Reabrir.", "Marcar Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void estadocomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AnularButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count > 0)
                {
                    //Funcionalidad preparada para multiple anulacion
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listanular = new List<PECAPE>(); //Guardamos items para anular
                    List<PECAPE> Listfallas = new List<PECAPE>(); //Guardamos items para errores

                    foreach (var item in Listgrilla)
                    {
                        //Recorremos la lista para obtener los checks
                        if (item.CAPECHECK == true)
                        {
                            if (item.CAPEIDES == 3) //Anular solo en preparacion
                            {
                                Listanular.Add(item);
                            }
                            else
                            {
                                Listfallas.Add(item);
                            }

                        }

                    }
                    if (Listanular.Count == 1) //Anular solo un item
                    {
                        if (MessageBox.Show("¿Está seguro que desea anular el Pedido seleccionado? Para llevar a cabo esta acción debe eliminar previamente la preparación del Pedido.", "Anular?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            //Obtener el detalle del pedido
                            List<PEDEPE> detalle = Devuelvedetalle(Listanular);
                            if (detalle.Count > 0)
                            {
                                //Primero valido que no tenga trabajado ningun item
                                if (ValidaPreparacionList(detalle))
                                {
                                    //Cambio el estado al pedido
                                    if (CambiaEstaList(Listanular, "9", ParametrosFe.Usuario))
                                    {
                                        MessageBox.Show("Se Anulado correctamente su Pedido.", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        Listfallas.Add(Listanular[0]);
                                    }
                                }
                                else
                                {
                                    Listfallas.Add(Listanular[0]);
                                }
                            }
                            else
                            {
                                MessageBox.Show("El pedido no tiene ningún detalle", "Incorrecto!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    else if (Listanular.Count == 0 && Listfallas.Count == 0)
                    {
                        MessageBox.Show("Debe marcar un Pedido para llevar a cabo la acción.", "Marcar un Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (Listfallas.Count > 0)
                    {
                        //imprimir lista de fallas
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in Listfallas)
                        {
                            sb.AppendLine("Pedido: " + item.CAPESERI.ToString() + "-" + item.CAPENUME.ToString().PadLeft(7, '0') + " Estado: " + item.CAPEESTA);
                        }
                        if (Listfallas.Count > 1)
                        {
                            MessageBox.Show("No se pudieron Anular los siguientes Pedidos: "
                          + Environment.NewLine
                          + Environment.NewLine + sb.ToString(), "Error al Anular", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo Anular el siguiente Pedido: "
                            + Environment.NewLine
                            + Environment.NewLine + sb.ToString(), "Error al Anular", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    Busquedaalmacen();
                    MuestrapedidosPendientes("4");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region "Metodos"

        public bool MuestraPedidosAlmacen()
        {
            //cARGA TODOS LOS PEDIDOS CON ESTADO  1
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PEDIDOS_ALMACEN;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                //parEnt.Add(Estado); //parametro de entrada 
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        ListaPedidosdataGrid.ItemsSource = ListPedidos;
                        ListaPedidosdataGrid.SelectedIndex = 0;
                        resultadoOpe = true;
                    }
                    else
                    {
                        ListaPedidosdataGrid.ItemsSource = null;
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
        public void MuestraPedidosAlmacen1()
        {
            //cARGA TODOS LOS PEDIDOS CON ESTADO  1
            IappServiceClient clt = null;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PEDIDOS_ALMACEN;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(primerdiames().ToShortDateString()); //parametro de entrada 
                parEnt.Add(DateTime.Now.ToShortDateString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        lispedidosalmacen = ListPedidos;
                        valida = true;
                    }
                    else
                    {
                        valida = false;
                    }
                }

                else
                {
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    valida = false;
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

        }
        public bool MuestrapedidosPendientes(string Estado)
        {
            //cARGA TODOS LOS PEDIDOS CON ESTADO  1
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PEDIDOS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Estado); //parametro de entrada 
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPendientes = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        ListaSolicitudesdataGrid.ItemsSource = ListPendientes;
                        ListaSolicitudesdataGrid.CanUserAddRows = false;
                        resultadoOpe = true;
                    }
                    else
                    {
                        ListaSolicitudesdataGrid.ItemsSource = null;
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
        public void MuestrapedidosPendientes1(string Estado)
        {
            //cARGA TODOS LOS PEDIDOS CON ESTADO  1
            IappServiceClient clt = null;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PEDIDOS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Estado); //parametro de entrada 
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPendientes = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        listsolicitudes = ListPendientes;
                        valida1 = true;
                    }
                    else
                    {
                        valida1 = false;
                    }
                }

                else
                {
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    valida1 = false;
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
        }

        public bool BuscaPedido(string Busqueda, string estado, string serie = "", string fechainicio = "", string fechafin = "")
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_PEDIDOS_ALMACEN;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>
                {
                    Busqueda,
                    estado,
                    serie,
                    fechainicio,
                    fechafin
                };

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        this.ListaPedidosdataGrid.ItemsSource = ListPedidos;
                        ListaPedidosdataGrid.SelectedIndex = index1;
                        resultadoOpe = true;
                    }
                    else
                    {
                        if (ListaPedidosdataGrid.Items.Count > 0 && (!string.IsNullOrWhiteSpace(BuscarPedidotextBox.Text) || !string.IsNullOrWhiteSpace(TextBoxSerie.Text)))
                        {
                            MessageBox.Show("No se encontraron Coincidencias.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        this.ListaPedidosdataGrid.ItemsSource = null;
                        //MessageBox.Show("No se encontraron coincidencias.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public bool TrabajaPedido(List<PECAPE> Listpedido)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            Listdetalle = new List<PEDEPE>();
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_DETALLE_PEDIDO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(Listpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listdetobtenida = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        foreach (var item in Listdetobtenida)
                        {
                            appWcfService.PEDEPE objdetalle = new appWcfService.PEDEPE();
                            objdetalle.DEPEIDDP = item.DEPEIDDP;
                            objdetalle.DEPEIDCP = item.DEPEIDCP;
                            objdetalle.DEPECOAR = item.DEPECOAR;
                            objdetalle.DEPEPART = item.DEPEPART;
                            objdetalle.DEPECONT = item.DEPECONT;
                            objdetalle.DEPEALMA = item.DEPEALMA;
                            objdetalle.DEPECASO = item.DEPECASO;
                            objdetalle.DEPEPESO = item.DEPEPESO;
                            objdetalle.DEPECAAT = item.DEPECAAT;
                            objdetalle.DEPEPEAT = item.DEPEPEAT;
                            objdetalle.DEPEPERE = item.DEPEPERE;
                            objdetalle.DEPETADE = item.DEPETADE;
                            objdetalle.DEPEPEBR = item.DEPEPEBR;
                            objdetalle.DEPESTOC = item.DEPESTOC;
                            objdetalle.DEPEESTA = item.DEPEESTA;
                            objdetalle.DEPEUSCR = item.DEPEUSCR;
                            objdetalle.DEPEFECR = item.DEPEFECR;
                            objdetalle.DEPEUSMO = item.DEPEUSMO;
                            objdetalle.DEPEFEMO = item.DEPEFEMO;
                            objdetalle.DEPEDISP = item.DEPEDISP;
                            objdetalle.PARTSTPR = item.PARTSTPR;
                            objdetalle.DEPESECU = item.DEPESECU;
                            Listdetalle.Add(objdetalle);
                        }
                        //grilla proveniente del stored procedure
                        resultadoOpe = true;
                    }
                }
                else
                {
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

        public bool VistaPrevia(List<PECAPE> Listpedido)
        {
            List<PECAPE> Listvistaprevi = new List<PECAPE>();
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_DETALLE_PEDIDO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(Listpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listdetobtenida = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        List<appWcfService.PEDEPE> Listdetalle = new List<appWcfService.PEDEPE>();
                        foreach (var item in Listdetobtenida)
                        {
                            appWcfService.PEDEPE objdetalle = new appWcfService.PEDEPE();
                            objdetalle.DEPEIDDP = item.DEPEIDDP;
                            objdetalle.DEPEIDCP = item.DEPEIDCP;
                            objdetalle.DEPECOAR = item.DEPECOAR;
                            objdetalle.DEPEPART = item.DEPEPART;
                            objdetalle.DEPECONT = item.DEPECONT;
                            objdetalle.DEPEALMA = item.DEPEALMA;
                            objdetalle.DEPECASO = item.DEPECASO;
                            objdetalle.DEPEPESO = item.DEPEPESO;
                            objdetalle.DEPECAAT = item.DEPECAAT;
                            objdetalle.DEPEPEAT = item.DEPEPEAT;
                            objdetalle.DEPEPERE = item.DEPEPERE;
                            objdetalle.DEPETADE = item.DEPETADE;
                            objdetalle.DEPEPEBR = item.DEPEPEBR;
                            objdetalle.DEPESTOC = item.DEPESTOC;
                            objdetalle.DEPEESTA = item.DEPEESTA;
                            objdetalle.DEPEUSCR = item.DEPEUSCR;
                            objdetalle.DEPEFECR = item.DEPEFECR;
                            objdetalle.DEPEUSMO = item.DEPEUSMO;
                            objdetalle.DEPEFEMO = item.DEPEFEMO;
                            objdetalle.DEPEDISP = item.DEPEDISP;
                            objdetalle.PARTSTPR = item.PARTSTPR;
                            objdetalle.DEPESECU = item.DEPESECU;
                            Listdetalle.Add(objdetalle);
                        }
                        resultadoOpe = true;
                        Atender_Pedido FormAtender = new Atender_Pedido();
                        FormAtender.DetallePedidodataGrid.ItemsSource = Listdetalle;
                        FormAtender.DetallePedidodataGrid.CanUserAddRows = false;

                        switch (Listpedido[0].CAPEIDES.ToString())
                        {
                            case "2":
                                FormAtender.AtenderPedidolabel.Text = "Vista Previa: Pedido Emitido";
                                FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                FormAtender.vistaprev = true;
                                FormAtender.AprobarButton.IsEnabled = false;
                                FormAtender.Restituirbutton.IsEnabled = false;
                                break;

                            case "3":
                                FormAtender.AtenderPedidolabel.Text = "Vista Previa: Pedido en Preparación";
                                FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                FormAtender.vistaprev = true;
                                FormAtender.AprobarButton.IsEnabled = false;
                                FormAtender.Restituirbutton.IsEnabled = false;
                                break;
                            case "4":
                                if (oculta == 1)
                                {
                                    FormAtender.AtenderPedidolabel.Text = "Vista Previa: Pedido en espera de Aprobación";
                                    FormAtender.TrabajarItembutton.IsEnabled = false;
                                    FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                    FormAtender.Restituirbutton.IsEnabled = false;
                                    FormAtender.AprobarButton.IsEnabled = false;
                                    FormAtender.vistaprev = true;
                                }
                                else
                                {
                                    FormAtender.AtenderPedidolabel.Text = "Vista Previa: Aprobar Solicitud";
                                    FormAtender.TrabajarItembutton.IsEnabled = false;
                                    FormAtender.vistaprevAprob = true;
                                    FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                }
                                break;
                            case "5":
                                FormAtender.AtenderPedidolabel.Text = "Vista Previa: Pedido Aprobado";
                                FormAtender.TrabajarItembutton.IsEnabled = false;
                                FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                FormAtender.Restituirbutton.IsEnabled = false;
                                FormAtender.AprobarButton.IsEnabled = false;
                                FormAtender.vistaprev = true;
                                break;
                            case "9":
                                FormAtender.AtenderPedidolabel.Text = "Vista Previa: Pedido Anulado";
                                FormAtender.TrabajarItembutton.IsEnabled = false;
                                FormAtender.FinalizarPreparacionbutton.IsEnabled = false;
                                FormAtender.Restituirbutton.IsEnabled = false;
                                FormAtender.AprobarButton.IsEnabled = false;
                                FormAtender.vistaprev = true;
                                FormAtender.imprimirbutton.IsEnabled = false;
                                break;
                        }
                        Listvistaprevi.Add(Listpedido[0]);
                        FormAtender.Listseleccionados = Listvistaprevi;
                        FormAtender.ClienteLabel.Content = "CLIENTE: " + Listpedido[0].TCLIE.CLINOM.ToString();
                        //FormAtender.RucLabel.Content = "RUC: " + Listseleccionado[0].TCLIE.CLIRUC.ToString();
                        FormAtender.DireccionLabel.Text = "DIRECCIÓN: " + Listpedido[0].CAPEDIRE.ToString();
                        FormAtender.FechaLabel.Content = "FECHA: " + Listpedido[0].CAPEFECH.ToShortDateString();
                        FormAtender.NumPedidoLabel.Content = "PEDIDO: " + Listpedido[0].CAPESERI.ToString() + " " + Listpedido[0].CAPENUMC.ToString();
                        FormAtender.oculta = oculta;
                        FormAtender.Owner = Window.GetWindow(this);

                        FormAtender.ShowDialog();
                        MuestrapedidosPendientes("4");
                        if (ListaPedidosdataGrid.Items.Count > 0)
                        {
                            Busquedaalmacen();
                        }
                    }
                    ListaPedidosdataGrid.SelectedIndex = index1;
                }
                else
                {
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

        public bool busquedaenaprobacion(string busqueda)
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_SOLICITUD;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(busqueda);
                //parEnt.Add(estado);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        this.ListaSolicitudesdataGrid.ItemsSource = ListPedidos;
                        resultadoOpe = true;
                    }
                    else
                    {
                        if (ListaSolicitudesdataGrid.Items.Count > 0 && !string.IsNullOrWhiteSpace(BuscarSolicitudtextBox.Text))
                        {
                            MessageBox.Show("No se encontraron Coincidencias.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        this.ListaSolicitudesdataGrid.ItemsSource = null;
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

        public bool CambiaEstaList(List<appWcfService.PECAPE> ListAprobados, string estado, string usuario)//cambia el estado 1 o mas items
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
                argumentos.CODOPE = CodigoOperacion.CAMBIA_ESTADO_LISTA; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(ListAprobados));
                //parEnt.Add(textBox2.Text);
                parEnt.Add(estado);
                parEnt.Add(usuario);
                parEnt.Add(ListAprobados[0].CAPENUBU.ToString());
                parEnt.Add(ListAprobados[0].CAPETADE.ToString());


                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        resultadoOpe = true;

                        //selestado = DevuelveSeleccionEstado();
                        //Muestrapedidos(selestado); //Actualiza la grilla segun el combo seleccionado
                        /* switch (estado)
                         {

                             case "3":
                                 MuestraPedidosAlmacen();
                                 MessageBox.Show("Se ha guardado correctamente el pedido.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                                 break;
                             case "4":
                                 MuestraPedidosAlmacen();
                                 MessageBox.Show("Se ha actualizado el estado del pedido correctamente.", "Solicitud de Aprobación", MessageBoxButton.OK, MessageBoxImage.Information);
                                 break;
                             case "5":
                                 MuestrapedidosPendientes("4"); //Mostrar los pedidos con estado 4
                                 if (ListAprobados.Count > 1) //Envia los mensajes de acuerdo al tamaño de lista enviada.
                                 {
                                     MessageBox.Show("Se han aprobado correctamente los pedidos.", "Aprobado", MessageBoxButton.OK, MessageBoxImage.Information);
                                 }
                                 else
                                 {
                                     MessageBox.Show("Se ha aprobado correctamente el pedido.", "Aprobado", MessageBoxButton.OK, MessageBoxImage.Information);
                                 }
                                 break;
                         }*/
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido cambiar el estado.", "Falló", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public void Busquedaalmacen()
        {
            try
            {
                switch (estadocomboBox.SelectedIndex)
                {
                    case 0: //Busca todos los pedidos con estado 2 y 3 
                        estado = "0";
                        Atenderpedidobutton.IsEnabled = true;
                        break;

                    case 1: //Busca los pedidos con estado 2
                        estado = "2";
                        Atenderpedidobutton.IsEnabled = true;
                        break;

                    case 2: //Busca los pedidos con estado 3 
                        estado = "3";
                        Atenderpedidobutton.IsEnabled = true;
                        break;

                    case 3: //Busca los pedidos con estado 4 
                        estado = "4";
                        Atenderpedidobutton.IsEnabled = false;
                        break;
                    case 4: //Busca los pedidos con estado 5 
                        estado = "5";
                        Atenderpedidobutton.IsEnabled = false;
                        break;
                    case 5: //Busca los pedidos con estado 9 
                        estado = "9";
                        Atenderpedidobutton.IsEnabled = false;
                        break;
                }
                //DMA 07/05/2018
                //Busqueda por fechas

                if (FechaIniciodtp.SelectedDate != null || FechaFindtp.SelectedDate != null) //Si alguna de las dos fechas se ha seleccionado
                {
                    //Si alguna de las dos fechas esta vacia le coloca su contraparte
                    BuscaPedido(BuscarPedidotextBox.Text.Trim(), estado, TextBoxSerie.Text.Trim(),
                        (FechaIniciodtp.SelectedDate == null ? FechaFindtp.SelectedDate.Value : FechaIniciodtp.SelectedDate.Value).ToShortDateString(),
                        (FechaFindtp.SelectedDate == null ? FechaIniciodtp.SelectedDate.Value : FechaFindtp.SelectedDate.Value).ToShortDateString());
                }
                else
                {
                    BuscaPedido(BuscarPedidotextBox.Text.Trim(), estado, TextBoxSerie.Text.Trim());
                }
                MuestrapedidosPendientes("4");
                validacombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ImprimePedido(decimal idpedido)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;

                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_PEDIDO_CONSULTA; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Convert.ToString(idpedido));
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        lista = Utils.Deserialize<List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>>(resultado.VALSAL[1]);
                        ExportToPDF(lista, "appfe.FormatoPedido.rdlc", "DataSetPedido");
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

        public void ExportToPDF(object datos, string path, string nombreDataSet)
        {
            string fileName;
            Microsoft.Reporting.WinForms.Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            var viewer = new Microsoft.Reporting.WinForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            viewer.LocalReport.ReportEmbeddedResource = path;
            Microsoft.Reporting.WinForms.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource(); //"DataSetLiquidacion", datos);
            rds.Name = nombreDataSet;
            rds.Value = datos;
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rds);
            //viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);
            fileName = GetTemporaryDirectory() + ".pdf";
            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            System.Diagnostics.Process.Start(fileName);
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private void validacombo()
        {
            try
            {
                switch (estadocomboBox.SelectedIndex)
                {
                    case 0: //Emitidos en preparacion
                        reabrirbutton.IsEnabled = false;
                        btneditarpedido.IsEnabled = true;
                        if (oculta == 1)
                        {
                            AnularButton.IsEnabled = false;
                        }
                        else
                        {
                            AnularButton.IsEnabled = true;
                        }
                        break;
                    case 1: //Emitidos
                        reabrirbutton.IsEnabled = false;
                        btneditarpedido.IsEnabled = true;
                        AnularButton.IsEnabled = false;
                        break;
                    case 2: //en preparacion
                        reabrirbutton.IsEnabled = false;
                        btneditarpedido.IsEnabled = true;
                        if (oculta == 1)
                        {
                            AnularButton.IsEnabled = false;
                        }
                        else
                        {
                            AnularButton.IsEnabled = true;
                        }

                        break;
                    case 3://Espera de aprobacion
                        if (oculta == 1)
                        {
                            Imprimirbutton.IsEnabled = false;
                        }
                        else
                        {
                            Imprimirbutton.IsEnabled = true;
                        }
                        reabrirbutton.IsEnabled = false;
                        btneditarpedido.IsEnabled = false;
                        AnularButton.IsEnabled = false;
                        break;
                    case 4: //Completados
                        if (oculta == 1)
                        {
                            Imprimirbutton.IsEnabled = false;
                        }
                        else
                        {
                            reabrirbutton.IsEnabled = true;
                        }
                        btneditarpedido.IsEnabled = false;
                        AnularButton.IsEnabled = false;
                        break;

                    case 5: //Anulados
                        reabrirbutton.IsEnabled = false;
                        Atenderpedidobutton.IsEnabled = false;
                        Imprimirbutton.IsEnabled = false;
                        btneditarpedido.IsEnabled = false;
                        AnularButton.IsEnabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //
            //estadocomboBox.Items.Add("Pendientes (Emitidos, En Preparación)"); 0
            //estadocomboBox.Items.Add("Emitido"); 1
            //estadocomboBox.Items.Add("En preparación"); 2
            //estadocomboBox.Items.Add("Espera de Aprob."); 3
            //estadocomboBox.Items.Add("Completado"); 4
            //
        }

        private void Validaform()
        {
            try
            {
                if (oculta == 1) //vista trabajador
                {
                    buscasolgroup.IsEnabled = false;
                    solgroup.IsEnabled = false;
                    Imprimirbutton.IsEnabled = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidaPreparacionList(List<PEDEPE> listdetalle)
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
                argumentos.CODOPE = CodigoOperacion.VALIDA_PREPARACION_LIST;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(listdetalle));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    resultadoOpe = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }
        public List<PEDEPE> Devuelvedetalle(List<PECAPE> Listpedido)
        {
            List<PEDEPE> Detalle = new List<PEDEPE>();
            //bool resultadoOpe;
            IappServiceClient clt = null;
            //resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_DETALLE_PEDIDO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(Listpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listservicio = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        foreach (var item in Listservicio)
                        {
                            appWcfService.PEDEPE objdetalle = new appWcfService.PEDEPE();
                            objdetalle.DEPEIDDP = item.DEPEIDDP;
                            objdetalle.DEPEIDCP = item.DEPEIDCP;
                            objdetalle.DEPECOAR = item.DEPECOAR;
                            objdetalle.DEPEPART = item.DEPEPART;
                            objdetalle.DEPECONT = item.DEPECONT;
                            objdetalle.DEPEALMA = item.DEPEALMA;
                            objdetalle.DEPECASO = item.DEPECASO;
                            objdetalle.DEPEPESO = item.DEPEPESO;
                            objdetalle.DEPECAAT = item.DEPECAAT;
                            objdetalle.DEPEPEAT = item.DEPEPEAT;
                            objdetalle.DEPEPERE = item.DEPEPERE;
                            objdetalle.DEPETADE = item.DEPETADE;
                            objdetalle.DEPEPEBR = item.DEPEPEBR;
                            objdetalle.DEPESTOC = item.DEPESTOC;
                            objdetalle.DEPEESTA = item.DEPEESTA;
                            objdetalle.DEPEUSCR = item.DEPEUSCR;
                            objdetalle.DEPEFECR = item.DEPEFECR;
                            objdetalle.DEPEUSMO = item.DEPEUSMO;
                            objdetalle.DEPEFEMO = item.DEPEFEMO;
                            objdetalle.DEPEDISP = item.DEPEDISP;
                            objdetalle.PARTSTPR = item.PARTSTPR;
                            objdetalle.DEPEDSAR = item.DEPEDSAR;
                            objdetalle.DEPESECU = item.DEPESECU;
                            Detalle.Add(objdetalle);
                        }
                    }
                    else
                    {
                        MessageBox.Show(resultado.MENERR);
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
            return Detalle;
        }
        public bool EditaPedido(List<PECAPE> Listpedido)
        {
            List<PECAPE> Listvistaprevi = new List<PECAPE>();
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_DETALLE_PEDIDO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(Listpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listdetobtenida = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        List<appWcfService.PEDEPE> Listdetalle = new List<appWcfService.PEDEPE>();
                        foreach (var item in Listdetobtenida)
                        {
                            appWcfService.PEDEPE objdetalle = new appWcfService.PEDEPE();
                            objdetalle.DEPEIDDP = item.DEPEIDDP;
                            objdetalle.DEPEIDCP = item.DEPEIDCP;
                            objdetalle.DEPECOAR = item.DEPECOAR;
                            objdetalle.DEPEPART = item.DEPEPART;
                            objdetalle.DEPECONT = item.DEPECONT;
                            objdetalle.DEPEALMA = item.DEPEALMA;
                            objdetalle.DEPECASO = item.DEPECASO;
                            objdetalle.DEPEPESO = item.DEPEPESO;
                            objdetalle.DEPECAAT = item.DEPECAAT;
                            objdetalle.DEPEPEAT = item.DEPEPEAT;
                            objdetalle.DEPEPERE = item.DEPEPERE;
                            objdetalle.DEPETADE = item.DEPETADE;
                            objdetalle.DEPEPEBR = item.DEPEPEBR;
                            objdetalle.DEPESTOC = item.DEPESTOC;
                            objdetalle.DEPEESTA = item.DEPEESTA;
                            objdetalle.DEPEUSCR = item.DEPEUSCR;
                            objdetalle.DEPEFECR = item.DEPEFECR;
                            objdetalle.DEPEUSMO = item.DEPEUSMO;
                            objdetalle.DEPEFEMO = item.DEPEFEMO;
                            objdetalle.DEPEDISP = item.DEPEDISP;
                            objdetalle.PARTSTPR = item.PARTSTPR;
                            objdetalle.DEPEDSAR = item.DEPEDSAR;
                            objdetalle.DEPESECU = item.DEPESECU;
                            Listdetalle.Add(objdetalle);
                        }
                        Nuevo_Pedido Formnuevop = new Nuevo_Pedido();
                        Formnuevop.DetallePedidodataGrid.ItemsSource = Listdetalle;
                        Formnuevop.listadetallegrilla = Listdetalle;
                        Formnuevop.Cabecera = Listpedido[0];
                        Formnuevop.nuevoped = false;
                        Formnuevop.editarpedido = true;
                        Formnuevop.Owner = Window.GetWindow(this);

                        Formnuevop.ShowDialog();
                        resultadoOpe = true;
                        ListaPedidosdataGrid.SelectedIndex = index1;
                    }
                    else
                    {
                        MessageBox.Show(resultado.MENERR);
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

        private DateTime primerdiames()
        {
            DateTime DiaActual = DateTime.Now;
            int año = DiaActual.Year;
            int mes = DiaActual.Month;

            DateTime PrimerDia = new DateTime(año, mes, 1);

            return PrimerDia;
        }

        #endregion

        #region "MARCAS NACIONALES"
         private void btnimprimemarca_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar un Pedido para Imprimir.", "Marcar Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listimprimir = new List<PECAPE>(); //Guardamos items para imprimir
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listimprimir.Add(item);
                        }
                    }
                    if (Listimprimir.Count > 0) //Soporta imprimir Multiple 
                    {
                        foreach (var item in Listimprimir)
                        {
                            obtDatosPedImprMarca(item.CAPEIDCP); //, item.CAPEDEST, item.TCLIE.CLCNOM);
                        }
                    }
                }
                //Actualiza las grillas
                //Busquedaalmacen();
                //MuestrapedidosPendientes("4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool obtDatosPedImprMarca(decimal idpedido) //, string destino, string rezonsoc)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result> lista = null;

                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_PEDIDO_CONSULTA; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Convert.ToString(idpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        string destino;
                        lista = Utils.Deserialize<List<appWcfService.USP_OBTIENE_PEDIDO_CONSULTA_Result>>(resultado.VALSAL[1]);
                        destino = lista[0].CAPEDEST == null ? "" : lista[0].CAPEDEST;
                        ImprimeMarcas(destino, lista[0].CLINOM, lista, 0);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el pedido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        private void ImprimeMarcas(string destino, string razonsoc, List<USP_OBTIENE_PEDIDO_CONSULTA_Result> detped, decimal iniciaren)
        {
            bool massticker = false;
            string nombreImpresora;
            System.IO.StreamReader myFile = new System.IO.StreamReader("Formatos\\MARCANAC.txt");
            string myString = myFile.ReadToEnd();
            nombreImpresora = System.Configuration.ConfigurationManager.AppSettings["NombreImpresoraPesos"];
            myFile.Close();
            if (!Impresion.ImpresionDirecta.Imprimir(myString, true, ref nombreImpresora))
            {
                return;
            }

            decimal pnet, campo;
            string lote;

            System.Globalization.CultureInfo peCulture = new System.Globalization.CultureInfo("es-PE"); //ES???
            StringBuilder Stkr1 = new StringBuilder();
            Stkr1.Append("^XA^XFMARCANAC");
            Stkr1.Append("^FN1^FD");
            //-------LIMA-------
            Stkr1.Append(destino.PadCenter(18)).Append("^FS"); //DESTINO DEPARTAMENTO

            //Stkr1.Append("^FN2^FD").Append(razonsoc.PadCenter(30).Substring(0, 30)).Append("^FS"); //RAZON SOCIAL
            Stkr1.Append("^FN2^FD").Append(razonsoc.Trim()).Append("^FS"); //RAZON SOCIAL

            Impresion.ImpresionDirecta.Imprimir(Stkr1.ToString(), false, ref nombreImpresora); //PRUEBA

            campo = 4;
            int linea = 0;
            foreach (var itemdetemp in detped)
            {
                linea++;
                if (linea > iniciaren) // 1 > 0
                {
                    if (linea - iniciaren > 6)
                    {
                        massticker = true;
                        break;
                    }
                    //detalle
                    Stkr1 = new StringBuilder();

                    lote = itemdetemp.DEPEPART;  //"7A8144";
                    pnet = itemdetemp.DEPEPEAT;

                    Stkr1.Append("^FN" + Convert.ToString(campo) + "^FD").Append(lote).Append("^FS");
                    campo++;
                    Stkr1.Append("^FN" + Convert.ToString(campo) + "^FD").Append(pnet.ToString("N2", peCulture).PadLeft(10, ' ')).Append("^FS");

                    campo++;

                    Impresion.ImpresionDirecta.Imprimir(Stkr1.ToString(), false, ref nombreImpresora); //PRUEBA
                }
            }
            Stkr1 = new StringBuilder();
            Stkr1.Append("^XZ");
            Impresion.ImpresionDirecta.Imprimir(Stkr1.ToString(), false, ref nombreImpresora);
            if (massticker)
            {
                ImprimeMarcas(destino, razonsoc, detped, linea - 1);
            }
        }

        #endregion

    }

    public static class StringExtensions
    {
        public static string PadCenter(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }

}
