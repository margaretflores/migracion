using appConstantes;
using appfe.appServicio;
using appWcfService;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Atender_Pedido.xaml
    /// </summary>
    public partial class Atender_Pedido : Window
    {
        ParametrosFe _ParametrosIni;
        public Atender_Pedido()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }

        #region "Variables"
        //public string estado; //variable que recibe estado de almacen
        //Pantalla_Principal_Almacen PP_Almacen = new Pantalla_Principal_Almacen();//objeto de almacen
        public List<appWcfService.PEDEPE> listadetallegrilla = new List<appWcfService.PEDEPE>(); //Almacena los articulos del detalle
        public appWcfService.PECAPE Seleccionadopecape { get; internal set; }
        //public appWcfService.PECAPE item = null; //PECAPE de item de almacen 
        //bool cambios = false; //se activara si se realizaron cambios en la grilla
        public appWcfService.PEDEPE seleccionado = new appWcfService.PEDEPE(); //Objeto para manejar y almacenar valores obtenidos o modificados
        string Ruccliente = "";                                                                       //spublic List<appWcfService.PEDEPE> detallegrilla = new List<appWcfService.PEDEPE>();//lista que llena la grilla de detalle
        public List<appWcfService.PECAPE> Listseleccionados = new List<appWcfService.PECAPE>();//lista de pedidos seleccionados
        public bool vistaprev;
        public bool vistaprevAprob;
        decimal brutototal = 0;
        decimal tipodocumento = 0;
        List<string> listatipopedios = new List<string>()
        {
            "Venta",
            "Transf. entre almacenes",
            "Transf. interna",
            "Remates",
            "Consignación"
        };
        decimal tipo_ped = 1;
        decimal nubu, tade = 0;
        decimal tipodoc = 0;
        public int oculta = 0;

        ////TIPOS DE DOCUMENTOS
        //public const int GUIA = 3;
        //public const int NOTA = 5;
        //public const int NINGUNO = 0;
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
                cbmotivo.IsEnabled = false;
                txtbultos.IsEnabled = false;
                txttade.IsEnabled = false;

                cbnotaentrega.IsEnabled = false;
                cbguiaremision.IsEnabled = false;

                if (vistaprevAprob || vistaprev)
                {
                    cbmotivo.ItemsSource = listatipopedios;
                    if (Listseleccionados[0].CAPEIDCP != 0)
                    {
                        nubu = Listseleccionados[0].CAPENUBU;
                        tade = Listseleccionados[0].CAPETADE;
                        if (nubu == 0)
                        {
                            txtbultos.Text = "0";
                        }
                        else
                        {
                            txtbultos.Text = nubu.ToString();
                        }
                        if (tade == 0)
                        {
                            txttade.Text = "0";
                        }
                        else
                        {
                            txttade.Text = tade.ToString();
                        }
                        Ruccliente = Listseleccionados[0].TCLIE.CLIRUC;
                        switch (Listseleccionados[0].CAPETIPO.ToString())
                        {
                            case "2":
                                cbmotivo.SelectedIndex = 1; //TRANSFERENCIA ENTRE ALMACENES
                                devuelveseleccion();

                                break;
                            case "3":
                                cbmotivo.SelectedIndex = 2; //TRANSFERENCIA INTERNA
                                devuelveseleccion();

                                break;
                            case "4":
                                cbmotivo.SelectedIndex = 3; //REMATES
                                devuelveseleccion();
                                break;
                            case "5":
                                cbmotivo.SelectedIndex = 4; //CONSIGNACION
                                devuelveseleccion();
                                break;
                            default:
                                cbmotivo.SelectedIndex = 0; //VENTA
                                devuelveseleccion();
                                break;
                        }
                        if (Listseleccionados[0].CAPEIDES == 4)
                        {
                            if (oculta == 0)
                            {
                                cbmotivo.IsEnabled = true;
                                txtbultos.IsEnabled = true;
                                txttade.IsEnabled = true;
                                cbguiaremision.IsEnabled = true;
                                cbnotaentrega.IsEnabled = true;
                            }
                            else
                            {
                                cbmotivo.IsEnabled = false;
                                txtbultos.IsEnabled = false;
                                txttade.IsEnabled = false;
                                cbguiaremision.IsEnabled = false;
                                cbnotaentrega.IsEnabled = false;
                            }
                        }
                        else
                        {
                            cbmotivo.IsEnabled = false;
                            txtbultos.IsEnabled = false;
                            txttade.IsEnabled = false;
                        }
                    }
                }
                lblbruto.Content = CalculaBruto() + " KG.";
                tipodoc = Listseleccionados[0].CAPEIDTD;
                switch (tipodoc.ToString())
                {
                    case "3": //GUIA
                        cbguiaremision.IsChecked = true;
                        break;

                    case "5": //NOTA ENTREGA
                        cbnotaentrega.IsChecked = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private decimal CalculaBruto()
        {
            decimal brutototal = 0;
            var lista = DetallePedidodataGrid.ItemsSource.Cast<PEDEPE>().ToList();
            if (lista.Count > 0)
            {
                brutototal = lista.Sum(x => x.DEPEPEBR) + tade;
            }
            return brutototal;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*try
            {
                if (/*cambios && !AtenderPedidolabel.Text.Equals("Vista Previa"))
                {
                    if (MessageBox.Show("¿Desea guardar sus cambios antes de salir?", "¿Salir sin guardar?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {*/
            //  e.Cancel = true;
            /*}
        }
    }
    catch (Exception ex)
    {

        MessageBox.Show(ex.Message);
    }*/
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

        /*private void Guardarbutton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<appWcfService.PECAPE> ListPedidos = new List<appWcfService.PECAPE>();
                ListPedidos.Add(item);
                PP_Almacen.MuestraPedidosAlmacen();
                //item.CAPEIDCP;
                listadetallegrilla = DetallePedidodataGrid.ItemsSource.Cast<appWcfService.PEDEPE>().ToList(); //Almacenar los items de la grilla del detalle 

                if (GuardaDetalle(listadetallegrilla))
                {
                    if (item.CAPEIDES.ToString().Equals("3"))
                    {
                        PP_Almacen.MuestraPedidosAlmacen();
                        //aun falta la funcionalidad para guardar lo que se cambio en cantidad peso  y stock 0 ntes de el mensaje
                        MessageBox.Show("Se ha guardado correctamente el pedido.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                        cambios = false;
                    }
                    else
                    {
                        PP_Almacen.CambiaEstaList(ListPedidos, "3","");
                        cambios = false;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }*/

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

        private void AprobarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cliente incatops no puede cambiar a venta 15/06/2018
                bool valida = true;
                devuelveseleccion();
                AprobarButton.IsEnabled = false;
                tipodocumento = 0;
                if (tipo_ped == Constantes.VENTA)
                {
                    if (Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) //Incatops
                    {
                        MessageBox.Show("No es posible realizar una venta para este cliente. Por favor revise el motivo del Pedido", "Revise Motivo de Pedido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        valida = false;
                    }
                    else
                    {
                        if (cbguiaremision.IsChecked == true) //guia
                        {
                            tipodocumento = 3;
                        }
                        else if (cbnotaentrega.IsChecked == true) //Nota de entrega
                        {
                            tipodocumento = 5;
                        }
                    }
                }
                else if (tipo_ped == Constantes.CONSIGNACION)
                {
                    if (cbguiaremision.IsChecked == true) //guia
                    {
                        tipodocumento = 3;
                    }
                    else if (cbnotaentrega.IsChecked == true) //Nota de entrega
                    {
                        tipodocumento = 5;
                    }
                }
                if (valida)
                {
                    if (MessageBox.Show("Está seguro que desea generar la Pre-Guía?", "Generar Pre-Guía?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (GeneraPreguia(Listseleccionados[0].CAPEIDCP, ParametrosFe.Usuario))
                        {
                            AprobarButton.IsEnabled = false;
                            Restituirbutton.IsEnabled = false;
                            cbmotivo.IsEnabled = false;
                            txtbultos.IsEnabled = false;
                            txttade.IsEnabled = false;
                        }
                    }
                    else
                    {
                        AprobarButton.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FinalizarPreparacionbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinalizarPreparacionbutton.IsEnabled = false;
                List<appWcfService.PECAPE> ListPedidosFin = new List<appWcfService.PECAPE>();
                List<appWcfService.PECAPE> ListPedidosIncom = new List<appWcfService.PECAPE>();
                decimal bultos, tade;

                //ListPedidosAprobados.Add(item)
                if (Listseleccionados.Count >= 2)
                {
                    MessageBox.Show("No es posible finalizar la preparación de mas de un Pedido.", "Acción Incorrecta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    listadetallegrilla = DetallePedidodataGrid.ItemsSource.Cast<appWcfService.PEDEPE>().ToList();
                    if (MessageBox.Show("¿Está seguro que desea finalizar este pedido? este pasará a 'En espera de aprobación'. ", "Finalizar pedido", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Finalizapreparacion formfinaliza = new Finalizapreparacion();
                        formfinaliza.Owner = Window.GetWindow(this);
                        Nullable<bool> resultadodialog = formfinaliza.ShowDialog();
                        if (resultadodialog.HasValue && resultadodialog.Value)
                        {
                            bultos = formfinaliza.bultos;
                            tade = formfinaliza.tarades;
                        }
                        else
                        {
                            bultos = 0;
                            tade = 0;
                        }
                        /* if (cambios)//flag de cmbios en la grilla
                         {
                             GuardaDetalle(listadetallegrilla);
                         }*/

                        //bool atend = true; //flag para revisar que todos los items esten atendidos
                        foreach (var list in listadetallegrilla)
                        {
                            Seleccionadopecape = Listseleccionados.Find(x => x.CAPEIDCP == list.DEPEIDCP);

                            if (list.DEPEPEAT > 0 /*&& ((ListPedidosIncom.Find(x => x.CAPEIDCP == list.DEPEIDCP)) == null)*/)
                            {
                                ListPedidosFin.Add(Seleccionadopecape);
                            }
                            //else
                            //{
                            //    if ((ListPedidosFin.Find(x => x.CAPEIDCP == list.DEPEIDCP)) != null)
                            //    {
                            //        ListPedidosFin.Remove(Seleccionadopecape);
                            //    }
                            //    if ((ListPedidosIncom.Find(x => x.CAPEIDCP == list.DEPEIDCP)) == null)
                            //    {
                            //        ListPedidosIncom.Add(Seleccionadopecape);
                            //    }
                            //}
                        }

                        if (ListPedidosFin.Count > 0)
                        {
                            //if (ListPedidosIncom.Count > 0)
                            //{
                            //    if (MessageBox.Show("La preparación de alguno de los items no ha sido completada. ¿Desea finalizar los pedidos completados? estos pasarán a 'En espera de aprobación'. ", "Finalizar pedido", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            //    {
                            //        if (CambiaEstaList(ListPedidosFin, "4", ParametrosFe.Usuario))
                            //        {
                            //            MessageBox.Show("se ha finalizado correctamente el pedido.", "finalizado", MessageBoxButton.OK, MessageBoxImage.Information);
                            //            TrabajarItembutton.IsEnabled = false;
                            //            AprobarButton.IsEnabled = false;
                            //            Restituirbutton.IsEnabled = false;
                            //            this.Close();
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            if (CambiaEstaList(ListPedidosFin, "4", ParametrosFe.Usuario, bultos, tade))
                            {
                                MessageBox.Show("Se ha finalizado correctamente el Pedido.", "finalizado", MessageBoxButton.OK, MessageBoxImage.Information);
                                TrabajarItembutton.IsEnabled = false;
                                AprobarButton.IsEnabled = false;
                                Restituirbutton.IsEnabled = false;
                                this.Close();
                            }
                            //}

                        }
                        else
                        {
                            MessageBox.Show("No es posible finalizar un pedido si no  tiene alguno de sus items completo", "Finalizar pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    else
                    {
                        FinalizarPreparacionbutton.IsEnabled = true;

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void trabajaritem(PEDEPE Listseleccionado)
        {
            if (vistaprev)
            {
                List<appWcfService.PECAPE> Listrabajapedido = new List<appWcfService.PECAPE>();
                //Seleccionadopecape = DetallePedidodataGrid.SelectedItem as appWcfService.PECAPE;
                Listrabajapedido.Add(Listseleccionados[0]);
                switch (Listseleccionados[0].CAPEIDES.ToString())
                {
                    case "2":
                        if (MessageBox.Show("¿Está seguro de atender este pedido?", "Atender pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (CambiaEstaList(Listrabajapedido, "3", ParametrosFe.Usuario))
                            {
                                Listseleccionados[0].CAPEIDES = 3;
                                Trabajar_Item FormTrabajar = new Trabajar_Item();
                                FormTrabajar.objdetalle = Listseleccionado;
                                FormTrabajar.Owner = Window.GetWindow(this);

                                FormTrabajar.ShowDialog();

                                FinalizarPreparacionbutton.IsEnabled = true;
                                AtenderPedidolabel.Text = "Atender Pedido";
                                MuestraDetalle(Listseleccionados);
                            }
                        }
                        break;

                    case "3":
                        if (MessageBox.Show("¿Desea continuar con la preparación de este pedido?", "Continuar preparación?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            Trabajar_Item FormTrabajar = new Trabajar_Item();
                            FormTrabajar.objdetalle = Listseleccionado;
                            FormTrabajar.Owner = Window.GetWindow(this);
                            FormTrabajar.ShowDialog();

                            FinalizarPreparacionbutton.IsEnabled = true;
                            AtenderPedidolabel.Text = "Atender Pedido";
                            MuestraDetalle(Listseleccionados);
                        }
                        break;
                }
            }
            else
            {
                Trabajar_Item FormTrabajar = new Trabajar_Item();
                FormTrabajar.objdetalle = Listseleccionado;
                FormTrabajar.Owner = Window.GetWindow(this);
                FormTrabajar.ShowDialog();

                MuestraDetalle(Listseleccionados);
            }

        }

        private void TrabajarItembutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //{
                //    if (DetallePedidodataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
                //    {

                //        //seleccionado = DetallePedidodataGrid.SelectedItem as appWcfService.PEDEPE;
                //        //Trabajar_Item FormTrabajar = new Trabajar_Item();
                //        //FormTrabajar.objdetalle = seleccionado;
                //        //FormTrabajar.ShowDialog();
                //        //MuestraDetalle(item.CAPEIDCP);
                //        //DetallePedidodataGrid.ItemsSource = detallegrilla;
                //    }
                //    else
                //    {
                //        listadetallegrilla = DetallePedidodataGrid.ItemsSource.Cast<appWcfService.PEDEPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                //        List<PEDEPE> Listseleccionado = new List<PEDEPE>(); //Agrega la lista de seleccionados
                //        //foreach (var item in listadetallegrilla)
                //        //{
                //        //    if (item.CHECKSEL == true)
                //        //    {
                //        //        Listseleccionado.Add(item);
                //        //    }
                //}
                seleccionado = DetallePedidodataGrid.SelectedItem as appWcfService.PEDEPE;
                if (seleccionado != null)
                {
                    trabajaritem(seleccionado);
                    CalculaBruto();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un item para trabajarlo", "Seleccionar item", MessageBoxButton.OK, MessageBoxImage.Information);

                    //if (Listseleccionado.Count > 1)
                    //{
                    //    MessageBox.Show("No es posible trabajar mas de un item", "Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //else
                    //{
                    //}
                }
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetallePedidodataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DetallePedidodataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {/*
            try
            {
                cambios = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            /* try
             {
                 cambios = true;
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }*/
        }

        private void Restituirbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Restituirbutton.IsEnabled = false;
                List<appWcfService.PECAPE> Listretornar = new List<appWcfService.PECAPE>();
                Listretornar.Add(Listseleccionados[0]);
                if (MessageBox.Show("¿Está seguro que desea restituir este pedido? Regresará al estado 'En preparacion' ", "¿Restituir pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (CambiaEstaList(Listretornar, "3", ParametrosFe.Usuario)) //Envia la lista a retornar
                    {
                        MessageBox.Show("Se ha actualizado el estado del pedido correctamente.", "Restituir pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                        TrabajarItembutton.IsEnabled = true;
                        FinalizarPreparacionbutton.IsEnabled = true;
                        AprobarButton.IsEnabled = false;
                        Restituirbutton.IsEnabled = false;
                    }
                    //this.Close();
                }
                else
                {
                    Restituirbutton.IsEnabled = true;
                }
                AtenderPedidolabel.Text = "Atender Pedido";
                txtbultos.IsEnabled = false;
                txttade.IsEnabled = false;
                cbmotivo.IsEnabled = false;
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
                //listadetallegrilla = DetallePedidodataGrid.ItemsSource.Cast<appWcfService.PEDEPE>().ToList(); //Almacenamos todos los pedidos de la grilla en una lista
                //List<PECAPE> Listseleccionado = new List<PECAPE>(); //Agrega la lista de seleccionados
                //foreach (var item in listadetallegrilla)
                //{
                //    if (item.CHECKSEL == true)
                //    {
                //        Seleccionadopecape = Listseleccionados.Find(x => x.CAPEIDCP == item.DEPEIDCP);
                //        Listseleccionado.Add(Seleccionadopecape);
                //    }
                //}
                //if (Listseleccionado.Count == 0)
                //{
                //    MessageBox.Show("Debe seleccionar un item perteneciente a un Pedido para imprimir", "Seleccionar item", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    foreach (var item in Listseleccionado)
                //    {
                ImprimePedido(Listseleccionados[0].CAPEIDCP);
                //}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetallePedidodataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //String PartBase = "";
            //PartBase = Convert.ToString(System.UI.DataBinder.Eval(e.Row.DataContext, "PARTSTPR"));
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            DataGridRow row = e.Row;
            DataRowView rView = row.Item as DataRowView;
            if (rView != null && rView.Row.ItemArray[4].ToString().Contains("ERROR"))
            {//e.Row.Background = new SolidColorBrush();
            }
        }

        #endregion

        #region Metodos

        //private bool GuardaDetalle(List<appWcfService.PEDEPE> listdetalle)
        //{
        //    bool resultadoOpe;
        //    IappServiceClient clt = null;
        //    resultadoOpe = false;
        //    try
        //    {
        //        RESOPE resultado; //Resultado de Operacion
        //        clt = _ParametrosIni.IniciaNuevoCliente();
        //        PAROPE argumentos = new PAROPE();
        //        argumentos.CODOPE = CodigoOperacion.GUARDA_DETALLE;//codigo de operacion
        //        List<string> parEnt = new List<string>();

        //        parEnt.Add(Utils.Serialize(item));
        //        parEnt.Add(Utils.Serialize(listdetalle));
        //        //parEnt.Add(textBox2.Text);

        //        argumentos.VALENT = parEnt.ToArray();
        //        resultado = clt.EjecutaOperacion(argumentos);

        //        if (resultado.ESTOPE)
        //        {
        //            resultadoOpe = true;
        //            MuestraDetalle(item.CAPEIDCP);//Se muestra el detalle del pedido
        //            //Guardarbutton.IsEnabled = true;
        //        }
        //        else
        //        {
        //            MessageBox.Show(resultado.MENERR);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
        //        MessageBox.Show(ex.Message);
        //        //MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
        //    }
        //    finally
        //    {
        //        _ParametrosIni.FinalizaCliente(clt);
        //    }
        //    return resultadoOpe;
        //}

        public bool MuestraDetalle(List<PECAPE> Listpedido)
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
                parEnt.Add(Utils.Serialize(Listpedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        //Con detalle
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listdetallerecuperado = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        resultadoOpe = true;

                        listadetallegrilla.Clear();
                        foreach (var item in Listdetallerecuperado)
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
                            objdet.DEPEUSCR = item.DEPEUSCR;
                            objdet.DEPEFECR = item.DEPEFECR;
                            objdet.DEPEUSMO = item.DEPEUSMO;
                            objdet.DEPEFEMO = item.DEPEFEMO;
                            objdet.DEPEDISP = item.DEPEDISP;
                            objdet.DEPESECU = item.DEPESECU;
                            listadetallegrilla.Add(objdet);
                        }
                        DetallePedidodataGrid.ItemsSource = null;
                        DetallePedidodataGrid.ItemsSource = listadetallegrilla;
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

        public bool CambiaEstaList(List<appWcfService.PECAPE> ListAprobados, string estado, string usuario, decimal bultos = 0, decimal tade = 0)//cambia el estado 1 o mas items
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
                parEnt.Add(bultos.ToString());
                parEnt.Add(tade.ToString());

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
                        MessageBox.Show("No se ha podido cambiar el estado.", "Falló", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool GeneraPreguia(decimal idcabecera, string usuario)
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
                argumentos.CODOPE = CodigoOperacion.GENERA_PREGUIA; //1
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idcabecera.ToString());
                parEnt.Add(usuario);
                parEnt.Add(tipo_ped.ToString());
                parEnt.Add(nubu.ToString());
                parEnt.Add(tade.ToString());
                parEnt.Add(tipodocumento.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("Se ha generado correctamente la Pre-Guía.", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
                    resultadoOpe = true;

                    //if (resultado.VALSAL[0].Equals("1"))
                    //{
                    //    resultadoOpe = true;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("No se ha podido aprobar el pedido.", "Falló", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    //MostrarMensaje(Mensajes.MENSAJE_TIPO_NO_ENCONTRADO);
                    //}
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
                        ExportToPDF(lista, "appfe.FormatoPedidoPreparacion.rdlc", "DataSetPedido");
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


        #endregion

        private void cbmotivo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            devuelveseleccion();
        }

        void devuelveseleccion()
        {
            try
            {
                if (Ruccliente != "")
                {
                    if (Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) //IncaTops
                    {
                        switch (cbmotivo.SelectedIndex)
                        {
                            case 0:
                                MessageBox.Show("No es posible realizar una venta para este cliente. Por favor revise el motivo del Pedido", "Revise Motivo de Pedido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                cbmotivo.SelectedIndex = 1; // transferencia entre almacenes
                                break;
                            case 1:
                                tipo_ped = Constantes.TRANSF_ALMACENES;
                                lblmensaje.Content = "Al aprobar se generará la Guía de Traslado.";
                                break;
                            case 2:
                                tipo_ped = Constantes.TRANSF_INTERNA;
                                lblmensaje.Content = "Al aprobar se generará el documento Interno de Traslado.";
                                break;
                            case 3:
                                tipo_ped = Constantes.REMATES;
                                lblmensaje.Content = "Al aprobar se generará el documento Interno de Traslado.";
                                break;

                        }
                        cbguiaremision.Visibility = Visibility.Hidden;
                        cbnotaentrega.Visibility = Visibility.Hidden;
                        tipodoc = 0;
                    }
                    else
                    {
                        switch (cbmotivo.SelectedIndex)
                        {
                            case 0:
                                tipo_ped = Constantes.VENTA;
                                cbguiaremision.Visibility = Visibility.Visible;
                                cbnotaentrega.Visibility = Visibility.Visible;
                                tipodoc = Listseleccionados[0].CAPEIDTD;
                                switch (tipodoc.ToString())
                                {
                                    case "3":
                                        cbguiaremision.IsChecked = true;
                                        lblmensaje.Content = "Al aprobar se generará la Guía por Venta.";
                                        break;

                                    case "5":
                                        cbnotaentrega.IsChecked = true;
                                        lblmensaje.Content = "Al aprobar se generará Nota de Entrega.";
                                        break;
                                }
                                break;
                            case 4:
                                tipo_ped = Constantes.CONSIGNACION;
                                cbguiaremision.Visibility = Visibility.Visible;
                                cbnotaentrega.Visibility = Visibility.Visible;
                                tipodoc = Listseleccionados[0].CAPEIDTD;
                                switch (tipodoc.ToString())
                                {
                                    case "3":
                                        cbguiaremision.IsChecked = true;
                                        lblmensaje.Content = "Al aprobar se generará la Guía por Venta.";
                                        break;

                                    case "5":
                                        cbnotaentrega.IsChecked = true;
                                        lblmensaje.Content = "Al aprobar se generará Nota de Entrega.";
                                        break;
                                }
                                break;
                            default:
                                MessageBox.Show("Por favor revise el motivo del pedido.", "Revise Motivo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                cbmotivo.SelectedIndex = Convert.ToInt32(Listseleccionados[0].CAPETIPO - 1);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtbultos_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        public void noletras(KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    this.Close();
                }
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
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

        /*variable estatica*/

        static bool conFoco_txtBuscar;

        private void txtbultos_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (Mouse.LeftButton == MouseButtonState.Released)
            //{
            //    txtbultos.SelectAll();
            //    conFoco_txtBuscar = true;
            //}
        }

        private void txtbultos_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //if (!conFoco_txtBuscar && txtbultos.SelectionLength == 0)
            //{
            //    conFoco_txtBuscar = true;
            //    txtbultos.SelectAll();
            //}
        }

        private void txtbultos_LostFocus(object sender, RoutedEventArgs e)
        {
            //conFoco_txtBuscar = false;
            //txtbultos.SelectionLength = 0;
        }

        private void txtbultos_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtbultos.Text))
                {
                    nubu = 0;
                    //txtbultos.Text = "0";
                    txtbultos.SelectionStart = txtbultos.Text.Length;
                }
                else
                {
                    nubu = decimal.Parse(txtbultos.Text);
                }
            }
            catch (Exception ex)
            {
                txtbultos.Text = nubu.ToString();
                txtbultos.SelectionStart = txtbultos.Text.Length;
            }
        }

        //static bool conFoco_txtBuscar2;
        private void txttade_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (Mouse.LeftButton == MouseButtonState.Released)
            //{
            //    txttade.SelectAll();
            //    conFoco_txtBuscar2 = true;
            //}
        }

        private void txttade_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //if (!conFoco_txtBuscar2 && txttade.SelectionLength == 0)
            //{
            //    conFoco_txtBuscar2 = true;
            //    txttade.SelectAll();
            //}
        }

        private void txttade_KeyDown(object sender, KeyEventArgs e)
        {
            noletras2(e);
        }

        private void txttade_LostFocus(object sender, RoutedEventArgs e)
        {
            //conFoco_txtBuscar2 = false;
            //txttade.SelectionLength = 0;
        }

        private void txttade_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txttade.Text))
                {
                    tade = 0;
                    txttade.SelectionStart = txttade.Text.Length;
                    lblbruto.Content = CalculaBruto() + " KG.";
                }
                else
                {
                    tade = Convert.ToDecimal(txttade.Text);
                    lblbruto.Content = CalculaBruto() + " KG.";
                }
            }
            catch (Exception)
            {
                txttade.Text = tade.ToString();
                txttade.SelectionStart = txttade.Text.Length;
                lblbruto.Content = CalculaBruto() + " KG.";
            }
        }



        private void DetallePedidodataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!vistaprev || !vistaprevAprob)
            //{
            //    seleccionado = DetallePedidodataGrid.SelectedItem as appWcfService.PEDEPE;
            //    if (seleccionado != null)
            //    {
            //        Seleccionadopecape = Listseleccionados.Find(x => x.CAPEIDCP == seleccionado.DEPEIDCP);
            //        if (Seleccionadopecape != null)
            //        {
            //            ClienteLabel.Content = "CLIENTE: " + Seleccionadopecape.TCLIE.CLINOM.ToString();
            //            //FormAtender.RucLabel.Content = "RUC: " + Objseleccionado.TCLIE.CLIRUC.ToString();
            //            DireccionLabel.Text = "DIRECCIÓN: " + Seleccionadopecape.CAPEDIRE.ToString();
            //            FechaLabel.Content = "FECHA: " + Seleccionadopecape.CAPEFECH.ToShortDateString();
            //            NumPedidoLabel.Content = "PEDIDO: " + Seleccionadopecape.CAPESERI.ToString() + " " + Seleccionadopecape.CAPENUMC.ToString();
            //            cbmotivo.ItemsSource = listatipopedios;
            //            if (Seleccionadopecape.CAPEIDCP != 0)
            //            {
            //                nubu = Seleccionadopecape.CAPENUBU;
            //                txtbultos.Text = nubu.ToString();
            //                switch (Seleccionadopecape.CAPETIPO.ToString())
            //                {
            //                    case "2":
            //                        cbmotivo.SelectedIndex = 1;
            //                        break;
            //                    case "3":
            //                        cbmotivo.SelectedIndex = 2;
            //                        break;
            //                    case "4":
            //                        cbmotivo.SelectedIndex = 3;
            //                        break;
            //                    default:
            //                        cbmotivo.SelectedIndex = 0;
            //                        break;
            //                }
            //                if (Seleccionadopecape.CAPEIDES == 4)
            //                {
            //                    cbmotivo.IsEnabled = true;
            //                    txtbultos.IsEnabled = true;
            //                }
            //                else
            //                {
            //                    cbmotivo.IsEnabled = false;
            //                    txtbultos.IsEnabled = false;
            //                }
            //            }
            //        }
            //    }
            //}

        }
    }

}
