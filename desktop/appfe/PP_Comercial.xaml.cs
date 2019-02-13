using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using appfe.appServicio;
using appConstantes;
using System.Data;
using appWcfService;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Pantalla_Principal_Comercial.xaml
    /// </summary>
    public partial class Pantalla_Principal_Comercial : Window
    {
        ParametrosFe _ParametrosIni;

        public Pantalla_Principal_Comercial()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }

        #region "Variables"
        public appWcfService.PECAPE Seleccionado { get; internal set; } //crea un nuevo objeto del tipo PECAPE
        List<appWcfService.PECAPE> Listgrilla = new List<appWcfService.PECAPE>(); //lista para contener o agregar todos los items de la grilla actual del tipo PECAPE
        List<PECAPE> Listseleccionados = new List<PECAPE>(); //Almacenaremos todos los pedidos con check para realizar acciones de emitir, anular, etc.
        List<PECAPE> Listfallas = new List<PECAPE>(); //Almacenaremos todos los pedidos que no se puedan emitir, anular, etc
        string selestado; //Se almacena la seleccion actual del combobox para realizar las busquedas
        int maxprio;// marca el numero de prioridad maxima
        bool countcheck; // para saber en cual de los estados se puede priorizar y realizar busquedas
        int numseleccion;
        int index; //index seleccionada
        bool valida = false;
        List<PECAPE> listpedidos = new List<PECAPE>();
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
                    Muestrapedidos1("2");
                };
                bk.RunWorkerCompleted += (o, e) =>
                {
                    if (valida)
                    {
                        ListaPedidosdataGrid.CanUserAddRows = false;
                        ListaPedidosdataGrid.CanUserDeleteRows = false;
                        ListaPedidosdataGrid.ItemsSource = listpedidos;
                        ListaPedidosdataGrid.SelectedIndex = index;
                        BuscarPedidotextBox.Focus();
                        //txtNum.IsEnabled = false;
                        //cmdDown.IsEnabled = false;
                        //cmdUp.IsEnabled = false;
                        //moverbutton.IsEnabled = false;
                        FiltroBusquedacomboBox.Items.Add("Todos los estados"); //Filtro segun los estados de los pedidos
                        FiltroBusquedacomboBox.Items.Add("Creado"); //Filtro segun los estados de los pedidos
                        FiltroBusquedacomboBox.Items.Add("Emitido");
                        FiltroBusquedacomboBox.Items.Add("En preparación");
                        FiltroBusquedacomboBox.Items.Add("Espera de Aprobación");
                        FiltroBusquedacomboBox.Items.Add("Completado");
                        FiltroBusquedacomboBox.Items.Add("Anulado");
                        FiltroBusquedacomboBox.SelectedIndex = 2;
                        //fechapedido.SelectedDate = DateTime.Today;
                        FechaIniciodtp.SelectedDate = primerdiames();
                        FechaFindtp.SelectedDate = DateTime.Now;
                        RealizaBusqueda();
                    }
                    BusyBar.IsBusy = false;
                };
                bk.RunWorkerAsync();

                //ListaPedidosdataGrid.CanUserAddRows = false;
                //ListaPedidosdataGrid.CanUserDeleteRows = false;
                //BuscarPedidotextBox.Focus();
                //Muestrapedidos("0"); //Muestra todos los pedidos
                //txtNum.IsEnabled = false;
                //cmdDown.IsEnabled = false;
                //cmdUp.IsEnabled = false;
                //moverbutton.IsEnabled = false;
                //FiltroBusquedacomboBox.Items.Add("Todos los estados"); //Filtro segun los estados de los pedidos
                //FiltroBusquedacomboBox.Items.Add("Creado"); //Filtro segun los estados de los pedidos
                //FiltroBusquedacomboBox.Items.Add("Emitido");
                //FiltroBusquedacomboBox.Items.Add("En preparación");
                //FiltroBusquedacomboBox.Items.Add("Espera de Aprobación");
                //FiltroBusquedacomboBox.Items.Add("Completado");
                //FiltroBusquedacomboBox.Items.Add("Anulado");
                //FiltroBusquedacomboBox.SelectedIndex = 0;
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
                RealizaBusqueda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AnularPedidobutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count > 0)
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listanular = new List<PECAPE>(); //Guardamos items para emitirlos
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listseleccionados.Add(item);
                        }
                    }
                    foreach (var item in Listseleccionados) //Recorremos todos los seleccionados
                    {
                        if (item.CAPEIDES == 1 || item.CAPEIDES == 2)
                        {
                            Listanular.Add(item);
                        }
                        else
                        {
                            Listfallas.Add(item); //Se agrega a una lista que se mostrará mas adelante
                        }
                    }
                    if (Listseleccionados.Count > 0)
                    {
                        if (Listseleccionados.Count == 1)
                        {
                            if (MessageBox.Show("¿Está seguro que desea Anular este pedido?", "Anular Pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listanular.Count > 0)
                                {
                                    CambiaestadoPedidos(Listanular, "9", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("¿Está seguro que desea Anular los pedidos seleccionados?", "Anular Pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listanular.Count > 0)
                                {
                                    CambiaestadoPedidos(Listanular, "9", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un Pedido para Anular.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    Fallas:
                    if (Listfallas.Count > 0)
                    {
                        //Si tenemos fallas los mostramos en un messagebox
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
                    Finish:
                    RealizaBusqueda();
                    //Muestrapedidos(DevuelveSeleccionEstado());
                    Listseleccionados.Clear();
                    Listfallas.Clear();
                }
                else
                {
                    MessageBox.Show("Debe marcar al menos un Pedido para Anular.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void emitirbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar al menos un Pedido para Emitir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listemitir = new List<PECAPE>(); //Guardamos items para emitirlos
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listseleccionados.Add(item);
                        }
                    }
                    foreach (var item in Listseleccionados) //Recorremos todos los seleccionados
                    {
                        if (item.CAPEIDES == 1)
                        {
                            Listemitir.Add(item);
                        }
                        else
                        {
                            Listfallas.Add(item); //Se agrega a una lista que se mostrará mas adelante
                        }
                    }
                    if (Listseleccionados.Count > 0)
                    {
                        if (Listseleccionados.Count == 1)
                        {
                            if (MessageBox.Show("¿Está seguro que desea Emitir este pedido?", "Emitir Pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listemitir.Count > 0)
                                {
                                    CambiaestadoPedidos(Listemitir, "2", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("¿Está seguro que desea Emitir los Pedidos seleccionados?", "Emitir Pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listemitir.Count > 0)
                                {
                                    CambiaestadoPedidos(Listemitir, "2", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un Pedido para Emitir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    Fallas:
                    if (Listfallas.Count > 0)
                    {
                        //Si tenemos fallas los mostramos en un messagebox
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in Listfallas)
                        {
                            sb.AppendLine("Pedido: " + item.CAPESERI.ToString() + "-" + item.CAPENUME.ToString().PadLeft(7, '0') + " Estado: " + item.CAPEESTA);
                        }
                        if (Listfallas.Count > 1)
                        {
                            MessageBox.Show("No se pudieron Emitir los siguientes Pedidos: "
                          + Environment.NewLine
                          + Environment.NewLine + sb.ToString(), "Error al emitir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo Emitir el siguiente Pedido: "
                            + Environment.NewLine
                            + Environment.NewLine + sb.ToString(), "Error al emitir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    Finish:
                    RealizaBusqueda();
                    //Muestrapedidos(DevuelveSeleccionEstado());
                    Listseleccionados.Clear();
                    Listfallas.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void imprimirbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar al menos un pedido para imprimir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            //ImprimePedido(Listimprimir[0].CAPEIDCP);
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
                            //}
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un pedido para imprimir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                RealizaBusqueda();
                //Muestrapedidos(DevuelveSeleccionEstado());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reabributton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListaPedidosdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                {
                    MessageBox.Show("Debe marcar al menos un Pedido para Reabrir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                    List<PECAPE> Listreabrir = new List<PECAPE>(); //Guardamos items para emitirlos
                    foreach (var item in Listgrilla) //Recorrer la lista de la grilla para separar los que tienen check
                    {
                        if (item.CAPECHECK == true)
                        {
                            Listseleccionados.Add(item);
                        }
                    }
                    foreach (var item in Listseleccionados) //Recorremos todos los seleccionados
                    {
                        if (item.CAPEIDES == 2)
                        {
                            Listreabrir.Add(item);
                        }
                        else
                        {
                            Listfallas.Add(item); //Se agrega a una lista que se mostrará mas adelante
                        }
                    }
                    if (Listseleccionados.Count > 0)
                    {
                        if (Listseleccionados.Count == 1)
                        {
                            if (MessageBox.Show("¿Está seguro que desea Reabrir este Pedido?", "Reabrir Pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listreabrir.Count > 0)
                                {
                                    CambiaestadoPedidos(Listreabrir, "1", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("¿Está seguro que desea Reabrir los Pedidos seleccionados?", "Reabrir Pedidos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (Listreabrir.Count > 0)
                                {
                                    CambiaestadoPedidos(Listreabrir, "1", ParametrosFe.Usuario);
                                }
                                else
                                {
                                    goto Fallas;
                                }
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un Pedido para Reabrir.", "Seleccionar Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    Fallas:
                    if (Listfallas.Count > 0)
                    {
                        //Si tenemos fallas los mostramos en un messagebox
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in Listfallas)
                        {
                            sb.AppendLine("Pedido: " + item.CAPESERI.ToString() + "-" + item.CAPENUME.ToString().PadLeft(7, '0') + " Estado: " + item.CAPEESTA);
                        }
                        if (Listfallas.Count > 1)
                        {
                            MessageBox.Show("No se pudieron Reabrir los siguientes Pedidos: "
                          + Environment.NewLine
                          + Environment.NewLine + sb.ToString(), "Error al reabrir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo Reabrir el siguiente Pedido: "
                            + Environment.NewLine
                            + Environment.NewLine + sb.ToString(), "Error al reabrir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    Finish:
                    RealizaBusqueda();
                    //Muestrapedidos(DevuelveSeleccionEstado());
                    Listseleccionados.Clear();
                    Listfallas.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NuevoPedidobutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nuevo_Pedido Formnuevoped = new Nuevo_Pedido();
                Formnuevoped.Owner = Window.GetWindow(this);
                Formnuevoped.ShowDialog();
                RealizaBusqueda();
                //Muestrapedidos(DevuelveSeleccionEstado());
                ListaPedidosdataGrid.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void limpiarbutton_Click(object sender, RoutedEventArgs e)
        {
            BuscarPedidotextBox.Text = "";
            FechaIniciodtp.SelectedDate = null;
            FechaFindtp.SelectedDate = null;
            TextBoxSerie.Text = "";
            FiltroBusquedacomboBox.SelectedIndex = 0;
            BuscarPedidotextBox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    if (BuscarPedidotextBox.IsFocused)
                    {
                        BuscarPedidotextBox.Text = "";
                    }
                    else if (FiltroBusquedacomboBox.IsFocused)
                    {
                        FiltroBusquedacomboBox.SelectedIndex = 0;
                    }
                    else if (TextBoxSerie.IsFocused)
                    {
                        TextBoxSerie.Text = "";
                    }
                    //else if (FechaIniciodtp.IsFocused)
                    //{
                    //    FechaIniciodtp.Text = "";
                    //}
                    //else if (FechaFindtp.IsFocused)
                    //{
                    //    FechaFindtp.Text = "";
                    //}
                    else
                    {
                        Close();
                    }
                }
                if ((e.Key == Key.Enter))
                {
                    if (BuscarPedidotextBox.IsFocused || TextBoxSerie.IsFocused || FechaIniciodtp.IsFocused || FechaFindtp.IsFocused || FiltroBusquedacomboBox.IsFocused)
                    {
                        RealizaBusqueda();
                    }
                    //else if (ListaPedidosdataGrid.IsFocused)
                    //{
                    //    Seleccionado = ListaPedidosdataGrid.SelectedItem as PECAPE;
                    //    index = ListaPedidosdataGrid.SelectedIndex;
                    //    if (Seleccionado != null)
                    //    {
                    //        List<PECAPE> seleccion = new List<PECAPE>()
                    //    {
                    //        Seleccionado
                    //    };
                    //        BuscaDetalle(seleccion);
                    //        ListaPedidosdataGrid.SelectedIndex = index;
                    //    }
                    //}
                }
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
                //Lanzador PLanzador = new Lanzador();
                //PLanzador.Show();
                e.Cancel = false;
                // base.Close();
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
                if (ListaPedidosdataGrid.SelectedItem != null)
                {
                    index = ListaPedidosdataGrid.SelectedIndex;
                    Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
                    if (Seleccionado != null)
                    {
                        List<PECAPE> listpedidos = new List<PECAPE>()
                        {
                            Seleccionado,
                        };
                        BuscaDetalle(listpedidos);
                    }
                    //countcheck = true;
                    RealizaBusqueda();
                    ListaPedidosdataGrid.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FiltroBusquedacomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
            if (numseleccion != 0)
            {
                if (txtNum.Text.Equals(""))
                {
                    txtNum.Text = numseleccion.ToString();
                }
                if (NumValue > 1)//el valor en el text box tiene como minimo 1 que s el primero de la grilla
                {
                    //NumValue = numseleccion;
                    NumValue--;
                }
                else
                {
                    cmdUp.IsEnabled = false;
                }
                cmdDown.IsEnabled = true;
            }
            else
                MessageBox.Show("Debe seleccionar un pedido.", "Prioridad", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
            if (numseleccion != 0)//valida que se haya seleccionada un itm
            {
                if (txtNum.Text.Equals(""))
                {
                    txtNum.Text = numseleccion.ToString();
                }
                if (NumValue < maxprio)//el valor en el text box tiene como maximo el tamaño de la grilla
                {
                    //NumValue = numseleccion;
                    NumValue++;
                }
                else
                {
                    cmdDown.IsEnabled = false;
                }
                cmdUp.IsEnabled = true;
            }
            else
                MessageBox.Show("Debe seleccionar un pedido.", "Prioridad", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Regex regnum = new Regex("[0-9]"); //Expresión que solo acepta números.
                Regex reglet = new Regex("[A-Za-z]"); //Expresión que solo acepta letras.


                bool num = regnum.IsMatch(txtNum.Text.ToString()); //que tenga numeros
                bool letra = reglet.IsMatch(txtNum.Text.ToString()); //que tenga letras

                //Seleccionadoprio = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
                numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
                if (txtNum == null)
                {
                    // NumValue = 0;//cambiar
                    return;
                }

                if (letra)//|| txtNum.Text.Equals(""))//si el texto contiene letras  o esta vacio
                {
                    txtNum.Text = numseleccion > 0 ? numseleccion.ToString() : "";
                    cmdUp.IsEnabled = true;
                    cmdDown.IsEnabled = true;
                    //txtNum.Text = _numValue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //solo cuando hay un check activado
            //if (countcheck)
            //{
            //    //

            //    CheckBox chk = sender as CheckBox;

            //    PECAPE O = chk.DataContext as PECAPE;
            //    if (ParametrosFe.Usuario.Equals(O.CAPEUSCR.ToString()))
            //    {
            //        numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
            //    }
            //}

            //Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;

        }

        private void ListaPedidosdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void txtNum_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void ListaPedidosdataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (countcheck)//se activa solo cuando se esta en la pantalla de emitidos
            {
                numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
                txtNum.Text = numseleccion.ToString();
                cmdDown.IsEnabled = true;
                cmdUp.IsEnabled = true;
            }
        }

        private void moverbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                numseleccion = ListaPedidosdataGrid.SelectedIndex + 1;
                List<appWcfService.PECAPE> Listprioridad = new List<appWcfService.PECAPE>();
                Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList();
                Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;

                if (true)//ParametrosFe.Usuario.Equals(Seleccionado.CAPEUSCR.ToString()))
                {
                    if (!txtNum.Text.Equals(numseleccion.ToString()))
                    {
                        if (Int32.Parse(txtNum.Text.ToString()) < numseleccion)
                        {
                            int ultimo = ultipriorizado();
                            if (ultimo == 0)
                            {
                                subeprioridad(Listprioridad);
                            }
                            else
                                MessageBox.Show("No se puede llevar a cabo la priorización sobre el Pedido N° " + (ultimo + 1) + ".", "Error Prioridad", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        else
                            bajaprioridad(Listprioridad);
                    }
                    else
                        MessageBox.Show("No se ha cambiado la prioridad del pedido.", "Prioridad", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("No se puede priorizar este pedido, su usuario no coincide con el usuario de creación del pedido seleccionado.", "Prioridad", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                ListaPedidosdataGrid.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("Debe seleccionar un pedido.", "Prioridad", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

        }
        private void ListaPedidosdataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Seleccionado = ListaPedidosdataGrid.SelectedItem as PECAPE;
                    index = ListaPedidosdataGrid.SelectedIndex;
                    if (Seleccionado != null)
                    {
                        List<PECAPE> seleccion = new List<PECAPE>()
                        {
                            Seleccionado
                        };
                        BuscaDetalle(seleccion);
                        ListaPedidosdataGrid.SelectedIndex = index;
                    }
                    RealizaBusqueda();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region "Metodos"

        void RealizaBusqueda()
        {
            selestado = DevuelveSeleccionEstado();
            //Primero obtener el estado seleccionado 
            if (FechaIniciodtp.SelectedDate != null || FechaFindtp.SelectedDate != null)//si una de las dos fechas tiene datos
            {
                //mand a la funcion para realizar una busqueda teniendo en cuenta que si alguno es nulo manda su contraparte
                BuscaPedidoFechas(FechaIniciodtp.SelectedDate == null ? FechaFindtp.SelectedDate.Value : FechaIniciodtp.SelectedDate.Value,
                    FechaFindtp.SelectedDate == null ? FechaIniciodtp.SelectedDate.Value : FechaFindtp.SelectedDate.Value,//FechaFindtp.SelectedDate == null ? DateTime.Now : FechaFindtp.SelectedDate.Value, 
                    BuscarPedidotextBox.Text.Trim(), selestado, TextBoxSerie.Text.Trim());
            }
            else
            {
                BuscaPedido(BuscarPedidotextBox.Text.Trim(), selestado, TextBoxSerie.Text.Trim());
            }
            switch (FiltroBusquedacomboBox.SelectedIndex)
            {
                case 0:
                    emitirbutton.IsEnabled = true;
                    reabributton.IsEnabled = true;
                    AnularPedidobutton.IsEnabled = true;
                    //txtNum.IsEnabled = false;
                    //cmdDown.IsEnabled = false;
                    //cmdUp.IsEnabled = false;
                    //moverbutton.IsEnabled = false;
                    //countcheck = false;

                    //SubirPrioridadbutton.IsEnabled = false;
                    //BajarPrioridadbutton.IsEnabled = false;
                    //InicioPrioridadbutton.IsEnabled = false;
                    //FinalPrioridadbutton.IsEnabled = false;
                    break;

                case 1:
                    txtNum.IsEnabled = false;
                    //cmdDown.IsEnabled = false;
                    //cmdUp.IsEnabled = false;
                    //moverbutton.IsEnabled = false;
                    reabributton.IsEnabled = false;
                    emitirbutton.IsEnabled = true;
                    //countcheck = false;

                    //SubirPrioridadbutton.IsEnabled = false;
                    //BajarPrioridadbutton.IsEnabled = false;
                    //InicioPrioridadbutton.IsEnabled = false;
                    //FinalPrioridadbutton.IsEnabled = false;
                    break;

                case 2:
                    if (FechaIniciodtp.SelectedDate != null || FechaFindtp.SelectedDate != null || !BuscarPedidotextBox.Text.Trim().Equals("") || !string.IsNullOrWhiteSpace(TextBoxSerie.Text.Trim()))
                    {
                        //txtNum.IsEnabled = false;
                        //cmdDown.IsEnabled = false;
                        //cmdUp.IsEnabled = false;
                        //moverbutton.IsEnabled = false;
                        //countcheck = false;
                    }
                    else
                    {
                        txtNum.IsEnabled = true;
                        cmdDown.IsEnabled = true;
                        cmdUp.IsEnabled = true;
                        moverbutton.IsEnabled = true;
                        countcheck = true;
                    }
                    emitirbutton.IsEnabled = false;
                    reabributton.IsEnabled = true;
                    AnularPedidobutton.IsEnabled = true;
                    //SubirPrioridadbutton.IsEnabled = true;
                    //BajarPrioridadbutton.IsEnabled = true;
                    //InicioPrioridadbutton.IsEnabled = true;
                    //FinalPrioridadbutton.IsEnabled = true;
                    break;

                default:
                    emitirbutton.IsEnabled = false;
                    reabributton.IsEnabled = false;
                    AnularPedidobutton.IsEnabled = false;
                    //countcheck = false;


                    //txtNum.IsEnabled = false;
                    //cmdDown.IsEnabled = false;
                    //cmdUp.IsEnabled = false;
                    //moverbutton.IsEnabled = false;

                    //SubirPrioridadbutton.IsEnabled = false;
                    //BajarPrioridadbutton.IsEnabled = false;
                    //InicioPrioridadbutton.IsEnabled = false;
                    //FinalPrioridadbutton.IsEnabled = false;
                    break;
            }
        }
        public bool Muestrapedidos(string Estado)
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
                argumentos.CODOPE = CodigoOperacion.MUESTRA_PEDIDOS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Estado);
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        ListaPedidosdataGrid.ItemsSource = ListPedidos;
                        resultadoOpe = true;
                        ListaPedidosdataGrid.SelectedIndex = index;
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
            countcheck = false;

            return resultadoOpe;
        }
        public void Muestrapedidos1(string Estado)
        {
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
                parEnt.Add(Estado);
                //parEnt.Add(textBox2.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> ListPedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        listpedidos = ListPedidos;

                        valida = true;
                    }
                    else
                    {
                        valida = true;
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
            countcheck = false;
        }
        public bool BuscaPedidoFechas(DateTime fechainicio, DateTime fechafin, string busqueda, string estado, string serie = "")
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_PEDIDOS_FECHAS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                parEnt.Add(busqueda);
                parEnt.Add(estado);
                parEnt.Add(fechainicio.ToShortDateString());
                parEnt.Add(fechafin.ToShortDateString());
                parEnt.Add(serie);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> listpedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        ListaPedidosdataGrid.ItemsSource = listpedidos;
                        ListaPedidosdataGrid.SelectedIndex = index;
                        resultadoOpe = true;
                    }
                    else
                    {
                        ListaPedidosdataGrid.ItemsSource = null;
                        //MessageBox.Show("No se encontraron coincidencias", "Sin Coincidencias", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool BuscaPedido(string busqueda, string estado, string serie = "")
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_PEDIDOS_FECHAS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(busqueda);
                parEnt.Add(estado);
                parEnt.Add(""); //Fecha inicio
                parEnt.Add(""); //Fecha fin
                parEnt.Add(serie);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PECAPE> listpedidos = appWcfService.Utils.Deserialize<List<appWcfService.PECAPE>>(resultado.VALSAL[1]);
                        ListaPedidosdataGrid.ItemsSource = listpedidos;
                        maxprio = listpedidos.Count;
                        ListaPedidosdataGrid.SelectedIndex = index;
                        resultadoOpe = true;
                    }
                    else
                    {
                        ListaPedidosdataGrid.ItemsSource = null;
                        //MessageBox.Show("No se encontraron coincidencias", "Sin Coincidencias", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool BuscaDetalle(List<PECAPE> objcabecera)
        {
            //Obtiene el detalle del pedido seleccionado           
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
                parEnt.Add(Utils.Serialize(objcabecera));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        //Con detalle
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> ListDetalle = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        //Lista proveniente del stored procedure
                        resultadoOpe = true;
                        List<appWcfService.PEDEPE> Lisdetalle = new List<appWcfService.PEDEPE>();
                        foreach (var item in ListDetalle) //Pasar los items del tipo USP_OBTIENE_DETALLE_PEDIDOS_Result al tipo PEDEPE
                        {
                            appWcfService.PEDEPE objdet = new appWcfService.PEDEPE();
                            objdet.DEPEIDDP = item.DEPEIDDP;
                            objdet.DEPEIDCP = item.DEPEIDCP;
                            objdet.DEPECOAR = item.DEPECOAR;
                            objdet.DEPEPART = item.DEPEPART;
                            objdet.DEPECONT = item.DEPECONT;
                            objdet.DEPEALMA = item.DEPEALMA;
                            objdet.DEPECASO = item.DEPECASO;
                            objdet.DEPEPESO = item.DEPEPESO;
                            objdet.DEPECAAT = item.DEPECAAT;
                            objdet.DEPEPEAT = item.DEPEPEAT;
                            objdet.DEPEPERE = item.DEPEPERE;
                            objdet.DEPETADE = item.DEPETADE;
                            objdet.DEPEPEBR = item.DEPEPEBR;
                            objdet.DEPESTOC = item.DEPESTOC;
                            objdet.DEPEESTA = item.DEPEESTA;
                            objdet.DEPEDISP = item.DEPEDISP;
                            objdet.DEPEDSAR = item.DEPEDSAR;
                            objdet.DEPENUBU = item.DEPENUBU;
                            objdet.DEPEUSCR = item.DEPEUSCR;
                            objdet.DEPEFECR = item.DEPEFECR;
                            objdet.DEPEUSMO = item.DEPEUSMO;
                            objdet.DEPEFEMO = item.DEPEFEMO;
                            objdet.DEPESERS = item.DEPESERS;
                            objdet.DEPESECU = item.DEPESECU;

                            //MODIFICAR EL PROCEDIMIENTO PARA OBTENER LA DESCRIPCION DEL ARTICULO
                            Lisdetalle.Add(objdet);
                        }
                        Nuevo_Pedido Formnuevop = new Nuevo_Pedido();
                        Formnuevop.DetallePedidodataGrid.ItemsSource = Lisdetalle;
                        Formnuevop.listadetallegrilla = Lisdetalle;
                        Formnuevop.Cabecera = Seleccionado;
                        Formnuevop.nuevoped = false;
                        Formnuevop.Owner = Window.GetWindow(this);
                        Formnuevop.ShowDialog();
                        //selestado = DevuelveSeleccionEstado();
                        ListaPedidosdataGrid.SelectedIndex = index;
                        if (countcheck)
                        {
                            RealizaBusqueda();
                        }
                        else
                        {
                            if (DevuelveSeleccionEstado() == "0")
                            {
                                BuscaPedido(BuscarPedidotextBox.Text, "0");
                            }
                            else
                            {
                                Muestrapedidos(DevuelveSeleccionEstado());
                            }
                        }
                    }
                    else
                    {
                        //Sin detalle de items 
                        Nuevo_Pedido Formnuevop = new Nuevo_Pedido();
                        //Formnuevop.DetallePedidodataGrid.ItemsSource = Lisdetalle;
                        //Formnuevop.listadetallegrilla = Lisdetalle;
                        Formnuevop.Cabecera = Seleccionado;
                        Formnuevop.nuevoped = false;
                        Formnuevop.Owner = Window.GetWindow(this);
                        Formnuevop.ShowDialog();
                        selestado = DevuelveSeleccionEstado();
                        ListaPedidosdataGrid.SelectedIndex = index;
                        if (countcheck)
                        {
                            RealizaBusqueda();
                        }
                        else Muestrapedidos(selestado);
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

        public bool CambiaestadoPedidos(List<appWcfService.PECAPE> Listpedidos, string estado, string usuario)
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
                parEnt.Add(Utils.Serialize(Listpedidos));
                parEnt.Add(estado);
                parEnt.Add(usuario);
                //parEnt.Add(textBox2.Text);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        Muestrapedidos(DevuelveSeleccionEstado()); //Actualiza la grilla segun el combo seleccionado
                        if (Listpedidos.Count > 1) //Envia los mensajes de acuerdo al tamaño de lista enviada.
                        {
                            MessageBox.Show("Se ha actualizado el estado de los pedidos correctamente.", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Se ha actualizado el estado del pedido correctamente.", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido actualizar el estado del pedido.", "Falló", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public string DevuelveSeleccionEstado()
        {
            //Devuelve el valor del estado seleccionado
            string estado = "";
            switch (FiltroBusquedacomboBox.SelectedIndex)
            {
                case 1:
                    estado = "1"; //Guardado
                    break;
                case 2:
                    estado = "2"; //Emitido
                    break;
                case 3:
                    estado = "3"; //En preparación
                    break;
                case 4:
                    estado = "4"; //Espera de aprobación
                    break;
                case 5:
                    estado = "5"; //Completado
                    break;
                case 6:
                    estado = "9"; //Anulado
                    break;
                default:
                    estado = "0"; //Muestra todos
                    break;
            }
            return estado;
        }

        //Reporting 
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

        public int NumValue
        {
            get { return Int32.Parse(txtNum.Text.Equals("") ? "0" : txtNum.Text.ToString()); }
            set
            {
                //_numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        public void noletras(KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemPeriod)
                e.Handled = false;
            else
                e.Handled = true;
        }

        public void subeprioridad(List<appWcfService.PECAPE> Listprioridad)
        {
            Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
            int priorizado = Int32.Parse(txtNum.Text.ToString()); //se pone  -1 porque la grilla empiezaen 0
            int count = 1;

            foreach (var item in Listgrilla)
            {
                if (item != Seleccionado)
                {
                    if (count != priorizado)
                    {
                        item.CAPEPRIO = count;
                        Listprioridad.Add(item);
                    }
                    else
                    {
                        Seleccionado.CAPEPRIO = count;
                        Seleccionado.CAPEEPRI = 1;

                        Listprioridad.Add(Seleccionado);
                        count++;
                        item.CAPEPRIO = count;
                        Listprioridad.Add(item);
                    }
                    count++;

                }
                else
                    break;
            }
            //MessageBox.Show("hola." + Listprioridad, "Prioridad", MessageBoxButton.OK, MessageBoxImage.Information);//mandar a la bd?
            prioriza(Listprioridad, ParametrosFe.Usuario);
            RealizaBusqueda();
        }

        public int ultipriorizado()
        {
            int priorizado = Int32.Parse(txtNum.Text.ToString()) - 1; //se pone  -1 porque la grilla empiezaen 0
            Listgrilla = ListaPedidosdataGrid.ItemsSource.Cast<appWcfService.PECAPE>().ToList();
            numseleccion = ListaPedidosdataGrid.SelectedIndex;
            Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
            int ultimoprio = 0;

            int count = 0;

            foreach (var item in Listgrilla)
            {
                if (count >= priorizado && count < numseleccion)
                {
                    if (item.CAPEEPRI == 1)
                    {
                        ultimoprio = count;
                    }
                }
                if (item == Seleccionado)
                {
                    break;
                }
                count++;
            }
            return ultimoprio;
        }

        public void bajaprioridad(List<appWcfService.PECAPE> Listprioridad)
        {
            Seleccionado = ListaPedidosdataGrid.SelectedItem as appWcfService.PECAPE;
            int priorizado = Int32.Parse(txtNum.Text.ToString()); //se pone  -1 porque la grilla empiezaen 0
            int count = 1;

            foreach (var item in Listgrilla)
            {
                if (item != Seleccionado)
                {
                    item.CAPEPRIO = count;
                    Listprioridad.Add(item);
                    count++;
                }
                if (count == priorizado)
                {
                    Seleccionado.CAPEPRIO = count;
                    Seleccionado.CAPEEPRI = 1;
                    Listprioridad.Add(Seleccionado);
                    break;
                }
            }
            //MessageBox.Show("hola." + Listprioridad, "Prioridad", MessageBoxButton.OK, MessageBoxImage.Information);//mandar a la bd?
            prioriza(Listprioridad, ParametrosFe.Usuario);
            RealizaBusqueda();
        }

        public bool prioriza(List<appWcfService.PECAPE> lista, string usuario)
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
                argumentos.CODOPE = CodigoOperacion.GUARDA_PRIORIDAD;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(lista));
                parEnt.Add(usuario);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("Su pedido ha sido priorizado Correctamente.", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private DateTime primerdiames()
        {
            DateTime DiaActual = DateTime.Now;
            int año = DiaActual.Year;
            int mes = DiaActual.Month;

            DateTime PrimerDia = new DateTime(año, mes, 1);

            return PrimerDia;
        }

        #endregion
    }
}
