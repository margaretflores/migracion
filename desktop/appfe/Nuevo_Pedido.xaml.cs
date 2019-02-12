using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using appfe.appServicio;
using appConstantes;
using appWcfService;
using Microsoft.Win32;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.ComponentModel;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Nuevo_Pedido.xaml
    /// </summary>
    public partial class Nuevo_Pedido : Window
    {

        ParametrosFe _ParametrosIni;
        public Nuevo_Pedido()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        #region "Variables"  

        //Pedidos V2
        public PECAPE Cabecera = new PECAPE(); //Almacena en un objeto los datos del form
        string Ruccliente; // Almacena el RUC cuando se hace una busqueda por cliente
        public bool nuevoped = true; //Validar si se tiene un nuevo pedido
        public List<PEDEPE> listadetallegrilla = new List<PEDEPE>(); //Almacena los articulos del detalle
        public bool editarpedido = false; //Modo de edición del Pedido
        List<PEDEPE> listaeliminar = new List<PEDEPE>(); //Guarda los items para luego ser eliminados 
        decimal tipo_ped = 1; //Tipo de pedido
        List<CARGACOMBO> listacombo = new List<CARGACOMBO>
        {
            new CARGACOMBO
            {
                DESCRIPCION="Venta",
                VALOR = 1,
            },
            new CARGACOMBO
            {
                DESCRIPCION="Transf. entre almacenes",
                VALOR = 2,
            },
             new CARGACOMBO
            {
                DESCRIPCION="Transf. interna",
                VALOR = 3,
            },
             new CARGACOMBO
            {
                DESCRIPCION="Remates",
                VALOR = 4,
            }
             ,
             new CARGACOMBO
             {
                 DESCRIPCION="Consignación",
                 VALOR=5,
             }
        };
        List<TCLIE> listaclientes = new List<TCLIE>(); //Almacena los clientes de la busqueda
        #endregion

        #region "Eventos"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatosIniciales();
            ClientetextBox.Focus();
        }

        private void BuscarClientebutton_Click(object sender, RoutedEventArgs e)
        {
            RealizabusquedaCliente();
            ClientetextBox.Focus();
        }

        private void AgregarArticulobutton_Click(object sender, RoutedEventArgs e) => AgregaArticulos();

        private void EliminarArticulobutton_Click(object sender, RoutedEventArgs e) => EliminaArticulos();

        private void cbtipopedido_SelectionChanged(object sender, SelectionChangedEventArgs e) => AsignatipoPedido();

        private void imprimirbutton_Click(object sender, RoutedEventArgs e) => RealizaImpresion();

        private void Guardarbutton_Click(object sender, RoutedEventArgs e) => RealizaGuardado();

        private void Emitirbutton_Click(object sender, RoutedEventArgs e) => RealizaCambioEstado(Constantes.ESTADO_EMITIDO);

        private void Anularbutton_Click(object sender, RoutedEventArgs e) => RealizaCambioEstado(Constantes.ESTADO_ANULADO);

        private void Reabrirbutton_Click(object sender, RoutedEventArgs e) => RealizaCambioEstado(Constantes.ESTADO_CREADO);

        private void Limpiarbutton_Click(object sender, RoutedEventArgs e) => Limpiar();

        private void Window_KeyDown(object sender, KeyEventArgs e) => CapturaEventos(sender, e);

        private void Cancelarbutton_Click(object sender, RoutedEventArgs e) => Close();

        private void DetallePedidodataGrid_LoadingRow(object sender, DataGridRowEventArgs e) => e.Row.Header = (e.Row.GetIndex() + 1).ToString();

        private void btnimportar_Click(object sender, RoutedEventArgs e) => RealizaImportacion();

        #endregion

        #region "Metodos"
        // PEDIDOS V2
        private void CargaDatosIniciales()
        {
            try
            {
                BackgroundWorker bk = new BackgroundWorker();
                BusyBar.IsBusy = true;
                bk.DoWork += (o, e) =>
                {
                    //
                };
                bk.RunWorkerCompleted += (o, e) =>
                {
                    cbtipopedido.ItemsSource = listacombo;
                    cbtipopedido.DisplayMemberPath = "DESCRIPCION";
                    cbtipopedido.SelectedValuePath = "VALOR";
                    cbtipopedido.SelectedIndex = 0;
                    DetallePedidodataGrid.CanUserAddRows = false;
                    //obtenemos el tipo del pedido para validar el formulario
                    if (Cabecera.CAPEIDCP != 0)
                    {
                        tipo_ped = Cabecera.CAPETIPO;
                        nuevoped = false;

                    }
                    else
                    {
                        CargaSerie("REASONS ");
                        fechapedido.SelectedDate = DateTime.Today;
                    }
                    Validaform();
                    BusyBar.IsBusy = false;
                };
                bk.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RealizabusquedaCliente()
        {
            if (ClientetextBox.Text == "")
            {
                MessageBox.Show("Debe ingresar al menos un criterio para realizar una búsqueda.", "No se puede realizar búsqueda", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ClientetextBox.Text.Length <= 3)
            {

                MessageBox.Show("Debe ingresar mas de 3 caracteres para realizar una búsqueda.", "No se puede realizar búsqueda", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    listaclientes.Clear();
                    string busqueda = ClientetextBox.Text.Trim();
                    BackgroundWorker bk = new BackgroundWorker();
                    BusyBar.IsBusy = true;
                    bk.DoWork += (o, e) =>
                    {
                        BuscaCLiente(busqueda);
                    };
                    bk.RunWorkerCompleted += (o, e) =>
                    {
                        try
                        {
                            //Abrir el otro formulario para elegir el cliente
                            if (listaclientes.Count > 1)
                            {
                                Buscar_Cliente Formclie = new Buscar_Cliente();
                                Formclie.ClientesdataGrid.ItemsSource = listaclientes; //llenar la grilla del otro form 
                                Formclie.Owner = Window.GetWindow(this);
                                Nullable<bool> resultadodialog = Formclie.ShowDialog();
                                if (resultadodialog.HasValue && resultadodialog.Value)
                                {
                                    if (Formclie.Seleccionado != null)
                                    {
                                        Cabecera.CAPEIDCL = Formclie.Seleccionado.CLICVE;
                                        ClientetextBox.Text = string.IsNullOrEmpty(Formclie.Seleccionado.CLINOM) ? "" : Formclie.Seleccionado.CLINOM.Trim();
                                        RazonSociallabel.Content = string.IsNullOrEmpty(Formclie.Seleccionado.CLINOM) ? "" : Formclie.Seleccionado.CLINOM.Trim();
                                        RucLabel.Content = "RUC: " + (string.IsNullOrEmpty(Formclie.Seleccionado.CLIRUC) ? "" : Formclie.Seleccionado.CLIRUC.Trim());
                                        DirecciontextBox.Text = (string.IsNullOrEmpty(Formclie.Seleccionado.CLIDIR) ? "" : Formclie.Seleccionado.CLIDIR.Trim()) + " "
                                        + (string.IsNullOrEmpty(Formclie.Seleccionado.CLDIRF) ? "" : Formclie.Seleccionado.CLDIRF.Trim()) + " - "
                                        + (string.IsNullOrEmpty(Formclie.Seleccionado.CLIDIS) ? "" : Formclie.Seleccionado.CLIDIS.Trim()) + " - "
                                        + (string.IsNullOrEmpty(Formclie.Seleccionado.CLIPRO) ? "" : Formclie.Seleccionado.CLIPRO.Trim()) + " - "
                                        + (string.IsNullOrEmpty(Formclie.Seleccionado.CLIDPT) ? "" : Formclie.Seleccionado.CLIDPT.Trim()) + " - "
                                        + (string.IsNullOrEmpty(Formclie.Seleccionado.CLIPAI) ? "" : Formclie.Seleccionado.CLIPAI.Trim());
                                        txtemail.Text = (string.IsNullOrEmpty(Formclie.Seleccionado.CLMAIL) ? "" : Formclie.Seleccionado.CLMAIL.Trim());
                                        Ruccliente = (string.IsNullOrEmpty(Formclie.Seleccionado.CLIRUC) ? "" : Formclie.Seleccionado.CLIRUC.Trim());
                                        txtdepartamento.Text = (string.IsNullOrEmpty(Formclie.Seleccionado.CLIDPT) ? "" : Formclie.Seleccionado.CLIDPT.Trim());
                                        if (Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) //INCA TOPS
                                        {
                                            cbtipopedido.SelectedValue = Constantes.TRANSF_ALMACENES;
                                        }
                                        else if (Ruccliente.Equals("20527153001")) // DACARAU
                                        {
                                            cbtipopedido.SelectedValue = Constantes.CONSIGNACION;
                                        }
                                        else
                                        {
                                            cbtipopedido.SelectedValue = Constantes.VENTA;
                                        }
                                    }
                                }
                            }
                            else if (listaclientes.Count == 1)
                            {
                                Cabecera.CAPEIDCL = listaclientes[0].CLICVE;
                                ClientetextBox.Text = (string.IsNullOrEmpty(listaclientes[0].CLINOM) ? "" : listaclientes[0].CLINOM.Trim());
                                RazonSociallabel.Content = (string.IsNullOrEmpty(listaclientes[0].CLINOM) ? "" : listaclientes[0].CLINOM.Trim());
                                RucLabel.Content = "RUC: " + (string.IsNullOrEmpty(listaclientes[0].CLIRUC) ? "" : listaclientes[0].CLIRUC.Trim());
                                DirecciontextBox.Text = (string.IsNullOrEmpty(listaclientes[0].CLIDIR) ? "" : listaclientes[0].CLIDIR.Trim()) + " "
                                + (string.IsNullOrEmpty(listaclientes[0].CLDIRF) ? "" : listaclientes[0].CLDIRF.Trim()) + " - "
                                + (string.IsNullOrEmpty(listaclientes[0].CLIDIS) ? "" : listaclientes[0].CLIDIS.Trim()) + " - "
                                + (string.IsNullOrEmpty(listaclientes[0].CLIPRO) ? "" : listaclientes[0].CLIPRO.Trim()) + " - "
                                + (string.IsNullOrEmpty(listaclientes[0].CLIDPT) ? "" : listaclientes[0].CLIDPT.Trim()) + " - "
                                + (string.IsNullOrEmpty(listaclientes[0].CLIPAI) ? "" : listaclientes[0].CLIPAI.Trim());
                                txtemail.Text = (string.IsNullOrEmpty(listaclientes[0].CLMAIL) ? "" : listaclientes[0].CLMAIL.Trim());
                                Ruccliente = (string.IsNullOrEmpty(listaclientes[0].CLIRUC) ? "" : listaclientes[0].CLIRUC.Trim());
                                txtdepartamento.Text = (string.IsNullOrEmpty(listaclientes[0].CLIDPT) ? "" : listaclientes[0].CLIDPT.Trim());
                                if (Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) //INCA TOPS
                                {
                                    cbtipopedido.SelectedValue = Constantes.TRANSF_ALMACENES;
                                }
                                else if (Ruccliente.Equals("20527153001"))
                                {
                                    cbtipopedido.SelectedValue = Constantes.CONSIGNACION; //DACARAU
                                }
                                else
                                {
                                    cbtipopedido.SelectedValue = Constantes.VENTA;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        AsignatipoPedido();
                        BusyBar.IsBusy = false;
                    };
                    bk.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AgregaArticulos()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ClientetextBox.Text) || Cabecera.CAPEIDCL == null)
                {
                    MessageBox.Show("Debe seleccionar previamente el Cliente.", "Seleccionar cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Agregar_Item FormAgregarItm = new Agregar_Item();
                    FormAgregarItm.rucclie = Ruccliente;
                    FormAgregarItm.tipoped = tipo_ped;
                    FormAgregarItm.Owner = Window.GetWindow(this);
                    Nullable<bool> resultadodialog = FormAgregarItm.ShowDialog();
                    if (resultadodialog.HasValue && resultadodialog.Value)
                    {
                        if (FormAgregarItm.ListArticulos != null) //Si la lista del otro formulario tiene items seleccionados
                        {
                            List<PEDEPE> Listarepetidos = new List<PEDEPE>(); //Almacena los items repetidos
                                                                              //Recorremos la lista de los articulos para agregarla a nuestro Pedido
                            FormAgregarItm.ListArticulos.ForEach(x =>
                            {
                                var repetido = listadetallegrilla.Find(z => z.DEPECONT == x.DEPECONT && z.DEPEPART == x.DEPEPART && z.DEPEALMA == x.DEPEALMA && z.DEPECOAR == x.DEPECOAR);
                                //los articulos repetidos se deben agregar a otra lista para mostrarla luego
                                if (repetido != null)
                                {
                                    Listarepetidos.Add(repetido);
                                }
                                else
                                {
                                    listadetallegrilla.Add(x);
                                }
                            });
                            //Añadimos los articulos
                            DetallePedidodataGrid.ItemsSource = null;
                            DetallePedidodataGrid.ItemsSource = listadetallegrilla;
                            //Verificacion de Repetidos
                            if (Listarepetidos.Count > 0)
                            {
                                //Si tenemos repetidos los mostramos en un messagebox
                                StringBuilder sb = new StringBuilder();
                                foreach (var item in Listarepetidos)
                                {
                                    //sb.AppendLine("Código: " + item.DEPECOAR.ToString() + " Descripción: " + item.DEPEDSAR + " Contrato: " + item.DEPECONT);
                                    sb.AppendLine("Artic.: " + item.DEPECOAR + " Partida: " + item.DEPEPART + " Contrato: " + item.DEPECONT);
                                    //Partida Contrato Almacen
                                }
                                if (Listarepetidos.Count > 1)
                                {
                                    MessageBox.Show("Los Artículos que desea agregar ya existen en su pedido."
                                  + Environment.NewLine
                                  + Environment.NewLine + sb.ToString(), "No se pudo agregar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                                else
                                {
                                    MessageBox.Show("El Artículo que desea agregar ya existe en su pedido."
                                    + Environment.NewLine
                                    + Environment.NewLine + sb.ToString(), "No se pudo agregar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                }
                                Listarepetidos.Clear();
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

        private void EliminaArticulos()
        {
            try
            {
                if (listadetallegrilla.Count > 0)
                {
                    var checkeados = listadetallegrilla.Count(x => x.CHECKDEL == true);
                    if (checkeados > 0) //Si tiene chekeados recien entra
                    {
                        List<PEDEPE> Listfallas = new List<PEDEPE>(); //Almacena los que no se podrán eliminar porque tienen preparación
                        for (int i = 0; i < listadetallegrilla.Count; i++)
                        {
                            //Si hay modo edicion hay que verificar que no se ha trabajado con el item
                            if (editarpedido)
                            {
                                //Si tiene id de detalle y si está checkeado tendriamos que verificar que no tenga preparación
                                if (listadetallegrilla[i].DEPEIDDP != 0 && listadetallegrilla[i].CHECKDEL == true)
                                {
                                    if (ValidaPreparacion(listadetallegrilla[i]))
                                    {
                                        //Si pasa es porque no tiene preparación y se puede eliminar
                                        listaeliminar.Add(listadetallegrilla[i]); //Para eliminar en un futuro al momento de guardar el pedido
                                        listadetallegrilla.Remove(listadetallegrilla[i]);
                                        i--;
                                    }
                                    else
                                    {
                                        //El item tiene preparación y Tenemos que agregar a la lista de fallas y no se elimina
                                        Listfallas.Add(listadetallegrilla[i]);
                                    }
                                }
                                //No tiene id pero está checkeado
                                else if (listadetallegrilla[i].CHECKDEL == true)
                                {
                                    listadetallegrilla.Remove(listadetallegrilla[i]);
                                    i--;
                                }
                            }
                            //No tendria porque hacerse la verificacion de la preparación
                            else
                            {
                                if (listadetallegrilla[i].CHECKDEL == true && listadetallegrilla[i].DEPEIDDP != 0)
                                {
                                    //Agregarlo a la lista de eliminados
                                    listaeliminar.Add(listadetallegrilla[i]); //Para eliminar en un futuro al momento de guardar el pedido
                                    listadetallegrilla.Remove(listadetallegrilla[i]);
                                    i--;

                                }
                                else if (listadetallegrilla[i].CHECKDEL == true)
                                {
                                    //Eliminarlo directamente
                                    listadetallegrilla.Remove(listadetallegrilla[i]);
                                    i--;
                                }
                            }
                        }
                        DetallePedidodataGrid.ItemsSource = null;
                        DetallePedidodataGrid.ItemsSource = listadetallegrilla;
                        //Imprimir los que no se pudieron eliminar
                        if (Listfallas.Count > 0)
                        {
                            //Imprimimos las fallas
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in Listfallas)
                            {
                                sb.AppendLine("Artículo: " + item.DEPECOAR + " " + "KG:" + item.DEPEPEAT.ToString() + " " + "Conos:" + item.DEPECAAT.ToString());
                            }
                            if (Listfallas.Count > 1)
                            {
                                MessageBox.Show("No se Pueden Eliminar los Siguientes Artículos"
                              + Environment.NewLine
                              + Environment.NewLine + sb.ToString(), "No deben estar preparados.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                            else
                            {
                                MessageBox.Show("No se puede Eliminar el siguiente Artículo: "
                                + Environment.NewLine
                                + Environment.NewLine + sb.ToString(), "No debe estar preparado.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                            Listfallas.Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe marcar al menos un Artículo para Eliminar", "Marcar Artículo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Su detalle de Pedido está vacío no es posible eliminar", "Sin Artículos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RealizaGuardado()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ClientetextBox.Text) || string.IsNullOrWhiteSpace(DirecciontextBox.Text) || fechapedido.SelectedDate == null)
                {
                    MessageBox.Show("Los datos del cliente son incorrectos o estan vacíos", "Verifique Cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //Sección detalle
                    if (DetallePedidodataGrid.Items.Count > 0)
                    {
                        listadetallegrilla = DetallePedidodataGrid.ItemsSource.Cast<appWcfService.PEDEPE>().ToList(); //Almacenar los items de la grilla del detalle 
                        tipo_ped = decimal.Parse(cbtipopedido.SelectedValue.ToString()); //Asigna el tipo de pedido
                        if (!((Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) && tipo_ped == Constantes.VENTA)) //No permnitir ventas a incatops 
                        {
                            //decimal tiped = cbtipopedido.SelectedIndex + 1;
                            //if (!(Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")))
                            //{
                            //    tipo_ped = Constantes.VENTA;
                            //}
                            //Prepara Cabecera
                            if (nuevoped) // Verificar si es un nuevo pedido
                            {
                                Cabecera.CAPEIDCP = -1;
                                Cabecera.CAPEIDES = 1;
                                Cabecera.CAPEUSCR = ParametrosFe.Usuario;
                                Cabecera.CAPEFECR = DateTime.Today;
                            }
                            Cabecera.CAPESERI = seriecomboBox.SelectedValue.ToString();
                            Cabecera.CAPEDIRE = DirecciontextBox.Text.Trim();
                            Cabecera.CAPEEMAI = txtemail.Text;
                            Cabecera.CAPEFECH = fechapedido.SelectedDate.Value;
                            Cabecera.CAPENOTG = NotastextBox.Text.Trim();
                            Cabecera.CAPENOTI = NotasInternastextBox.Text.Trim();
                            Cabecera.CAPETIPO = tipo_ped;
                            Cabecera.CAPEDEST = txtdepartamento.Text.Trim();
                            //Asigna el tipo de documento que se generará
                            if (Cabecera.CAPETIPO == Constantes.VENTA || Cabecera.CAPETIPO == Constantes.CONSIGNACION) // Tipo venta
                            {
                                if (cbguia.IsChecked == true)
                                {
                                    Cabecera.CAPEIDTD = Constantes.ID_TIPO_DOC_GUIA; //Guia de remision
                                }
                                else if (cbnotaentrega.IsChecked == true)
                                {
                                    Cabecera.CAPEIDTD = Constantes.ID_TIPO_DOC_NE; //Nota de Entrega
                                }
                                else //Por si las moscas 
                                {
                                    MessageBox.Show("Debe marcar el tipo de documento que se generará.", "Marcar tipo de Documento", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                Cabecera.CAPEIDTD = Constantes.ID_TIPO_SIN_DOC;
                            }
                            //Realiza el guardado
                            bool valida = false;
                            try
                            {
                                BackgroundWorker bk = new BackgroundWorker();
                                BusyBar.IsBusy = true;
                                bk.DoWork += (o, e) =>
                                {
                                    if (ValidaReserva(listadetallegrilla))
                                    {
                                        valida = (GuardaPedido(Cabecera, listadetallegrilla, listaeliminar));
                                    }
                                };
                                bk.RunWorkerCompleted += (o, e) =>
                                {
                                    if (valida)
                                    {
                                        DetallePedidodataGrid.ItemsSource = null;
                                        DetallePedidodataGrid.ItemsSource = listadetallegrilla;
                                        BusyBar.IsBusy = false;
                                        MessageBox.Show("Se ha Guardado correctamente el pedido", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);

                                        Cabecera.TCLIE = new TCLIE();
                                        Cabecera.TCLIE.CLIRUC = Ruccliente;
                                        Cabecera.TCLIE.CLINOM = RazonSociallabel.Content.ToString();
                                    }

                                    Validaform();
                                };
                                bk.RunWorkerAsync();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No es posible realizar una venta para este cliente. Por favor revise el motivo del Pedido", "Revise Motivo de Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No es posible guardar un pedido sin un detalle de Artículos", "Debe agregar Artículos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidaReserva(List<PEDEPE> ListArticulos)
        {
            bool valida = true;
            try
            {
                //Volvemos a verificar los articulos con disponible
                ListArticulos.ForEach(x =>
                {
                    if (x.DEPEDISP - x.LOTCANRE < x.DEPEPESO)
                    {
                        x.CHECKRESE = true;
                    }
                    else
                    {
                        x.CHECKRESE = false;
                    }
                });

                if (ListArticulos.Where(x => x.CHECKRESE == true).Count() >= 1)
                {
                    StringBuilder sb = new StringBuilder();
                    ListArticulos.ForEach(x =>
                    {
                        if (x.CHECKRESE)
                        {
                            sb.AppendLine("Artic.: " + x.DEPECOAR + " Partida: " + x.DEPEPART + " Contrato: " + x.DEPECONT);
                        }
                    });
                    if (MessageBox.Show("Los siguientes Articulos sobrepasan la reserva ¿Desea Guardar?"
                      + Environment.NewLine
                      + Environment.NewLine + sb.ToString(), "Se sobrepasó la reserva.", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        valida = false;
                    }
                }
                //Eliminar los que sobrepasan
                //if (MessageBox.Show("Uno o mas de los Articulos seleccionados superan las reservas ¿Desea Agregarlos a su Pedido?", "Algunos Articulos supera la Reserva", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //{
                //    //eliminar los que superen reservas
                //    ListArticulos.RemoveAll(x => x.CHECKRESE == true);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return valida;
        }

        private void RealizaImpresion()
        {
            try
            {
                try
                {
                    if (Cabecera.CAPEIDES != 0)
                    {
                        BackgroundWorker bk = new BackgroundWorker();
                        BusyBar.IsBusy = true;
                        bk.DoWork += (o, e) =>
                        {
                            ImprimePedido(Cabecera.CAPEIDCP);
                        };
                        bk.RunWorkerCompleted += (o, e) =>
                        {
                            BusyBar.IsBusy = false;
                        };
                        bk.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("No es posible Imprimir el Pedido, por favor asegurese de Guardar.", "No se pudo imprimir", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AsignatipoPedido()
        {
            try
            {
                if (Ruccliente != null)
                {
                    //Bug: Cuando tu tipo de pedido es transf interna y seleccionas venta se pone transf ente almacenes
                    if (Ruccliente.Equals("20100199743") || Ruccliente.Equals("00008600061")) //IncaTops
                    {
                        //Validación de combo para INCA TOPS
                        switch (int.Parse(cbtipopedido.SelectedValue.ToString()))
                        {
                            case Constantes.VENTA:
                            case Constantes.CONSIGNACION:
                                if (Cabecera.CAPEIDCP != Constantes.SIN_ESTADO)
                                {
                                    cbtipopedido.SelectedValue = Cabecera.CAPETIPO;
                                }
                                else
                                {
                                    cbtipopedido.SelectedValue = Constantes.TRANSF_ALMACENES;
                                }
                                cbguia.Visibility = Visibility.Hidden;
                                cbnotaentrega.Visibility = Visibility.Hidden;
                                break;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(cbtipopedido.SelectedValue) == Constantes.TRANSF_ALMACENES ||
                            Convert.ToInt32(cbtipopedido.SelectedValue) == Constantes.TRANSF_INTERNA ||
                            Convert.ToInt32(cbtipopedido.SelectedValue) == Constantes.REMATES)
                        {
                            if (Cabecera.CAPETIPO == Constantes.SIN_ESTADO && Ruccliente.Equals("20527153001")) // DACARAU
                            {
                                cbtipopedido.SelectedValue = Constantes.CONSIGNACION;
                            }
                            else if (Cabecera.CAPETIPO != Constantes.SIN_ESTADO)
                            {
                                cbtipopedido.SelectedValue = Cabecera.CAPETIPO;
                            }
                            else
                            {
                                cbtipopedido.SelectedValue = Constantes.VENTA;
                            }
                        }

                        //switch (int.Parse(cbtipopedido.SelectedValue.ToString()))
                        //{
                        //    case Constantes.VENTA:
                        //    case Constantes.TRANSF_ALMACENES:
                        //    case Constantes.TRANSF_INTERNA:
                        //    case Constantes.REMATES:
                        //        cbtipopedido.SelectedValue = Constantes.VENTA;
                        //        cbguia.Visibility = Visibility.Visible;
                        //        cbnotaentrega.Visibility = Visibility.Visible;
                        //        break;

                        //    case Constantes.CONSIGNACION:
                        //        cbtipopedido.SelectedValue = Constantes.CONSIGNACION;
                        //        cbguia.Visibility = Visibility.Visible;
                        //        cbnotaentrega.Visibility = Visibility.Visible;
                        //        break;

                        //        //}
                        //}
                    }
                }
                else
                {
                    switch (int.Parse(cbtipopedido.SelectedValue.ToString()))
                    {
                        case Constantes.VENTA:
                        case Constantes.CONSIGNACION:
                            cbguia.Visibility = Visibility.Visible;
                            cbnotaentrega.Visibility = Visibility.Visible;
                            break;
                        case Constantes.TRANSF_ALMACENES:
                        case Constantes.TRANSF_INTERNA:
                        case Constantes.REMATES:
                            cbguia.Visibility = Visibility.Hidden;
                            cbnotaentrega.Visibility = Visibility.Hidden;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Validaform()
        {
            try
            {
                switch (int.Parse(Cabecera.CAPEIDES.ToString()))
                {
                    case Constantes.SIN_ESTADO:
                        NuevoPedidolabel.Text = "Nuevo Pedido";
                        ClientetextBox.IsEnabled = true;
                        DirecciontextBox.IsEnabled = true;
                        txtemail.IsEnabled = true;
                        BuscarClientebutton.IsEnabled = true;
                        fechapedido.IsEnabled = true;
                        seriecomboBox.IsEnabled = true;
                        AgregarArticulobutton.IsEnabled = true;
                        EliminarArticulobutton.IsEnabled = true;
                        NotasInternastextBox.IsEnabled = true;
                        NotastextBox.IsEnabled = true;
                        Reabrirbutton.IsEnabled = false;
                        Anularbutton.IsEnabled = false;
                        Guardarbutton.IsEnabled = true;
                        Emitirbutton.IsEnabled = false;
                        btnimportar.IsEnabled = true;
                        cbtipopedido.IsEnabled = true;
                        txtdepartamento.IsEnabled = true;
                        cbguia.IsEnabled = true;
                        cbnotaentrega.IsEnabled = true;
                        DetallePedidodataGrid.Columns[10].Visibility = Visibility.Hidden;
                        DetallePedidodataGrid.Columns[11].Visibility = Visibility.Hidden;
                        break;
                    case Constantes.ESTADO_CREADO:
                        NuevoPedidolabel.Text = "Pedido Creado";
                        ClientetextBox.IsEnabled = true;
                        DirecciontextBox.IsEnabled = true;
                        txtemail.IsEnabled = true;
                        BuscarClientebutton.IsEnabled = true;
                        fechapedido.IsEnabled = true;
                        seriecomboBox.IsEnabled = true;
                        AgregarArticulobutton.IsEnabled = true;
                        EliminarArticulobutton.IsEnabled = true;
                        NotasInternastextBox.IsEnabled = true;
                        NotastextBox.IsEnabled = true;
                        Reabrirbutton.IsEnabled = false;
                        Anularbutton.IsEnabled = true;
                        Guardarbutton.IsEnabled = true;
                        Emitirbutton.IsEnabled = true;
                        btnimportar.IsEnabled = true;
                        cbtipopedido.IsEnabled = true;
                        txtdepartamento.IsEnabled = true;
                        cbguia.IsEnabled = true;
                        cbnotaentrega.IsEnabled = true;
                        DetallePedidodataGrid.Columns[10].Visibility = Visibility.Hidden;
                        DetallePedidodataGrid.Columns[11].Visibility = Visibility.Hidden;
                        break;
                    case Constantes.ESTADO_EMITIDO:
                        if (editarpedido) //Modo Edición
                        {
                            NuevoPedidolabel.Text = "Editar Pedido Emitido";
                            AgregarArticulobutton.IsEnabled = true;
                            EliminarArticulobutton.IsEnabled = true;
                            Guardarbutton.IsEnabled = true;
                            cbguia.IsEnabled = true;
                            cbnotaentrega.IsEnabled = true;
                        }
                        else
                        {
                            NuevoPedidolabel.Text = "Vista Previa: Pedido Emitido";
                            AgregarArticulobutton.IsEnabled = false;
                            EliminarArticulobutton.IsEnabled = false;
                            Guardarbutton.IsEnabled = false;
                            cbguia.IsEnabled = false;
                            cbnotaentrega.IsEnabled = false;
                            DetallePedidodataGrid.Columns[10].Visibility = Visibility.Hidden;
                            DetallePedidodataGrid.Columns[11].Visibility = Visibility.Hidden;
                        }
                        ClientetextBox.IsEnabled = false;
                        DirecciontextBox.IsEnabled = false;
                        txtemail.IsEnabled = false;
                        BuscarClientebutton.IsEnabled = false;
                        fechapedido.IsEnabled = false;
                        seriecomboBox.IsEnabled = false;
                        NotasInternastextBox.IsEnabled = false;
                        NotastextBox.IsEnabled = false;
                        Emitirbutton.IsEnabled = false;
                        Anularbutton.IsEnabled = true;
                        Reabrirbutton.IsEnabled = true;
                        btnimportar.IsEnabled = false;
                        cbtipopedido.IsEnabled = false;
                        txtdepartamento.IsEnabled = false;
                        break;
                    case Constantes.ESTADO_PREPARACION:
                        if (editarpedido)
                        {
                            NuevoPedidolabel.Text = "Editar Pedido en Preparación";
                            AgregarArticulobutton.IsEnabled = true;
                            EliminarArticulobutton.IsEnabled = true;
                            Guardarbutton.IsEnabled = true;
                            cbguia.IsEnabled = true;
                            cbnotaentrega.IsEnabled = true;
                        }
                        else
                        {
                            NuevoPedidolabel.Text = "Vista Previa: Pedido en Preparación";
                            AgregarArticulobutton.IsEnabled = false;
                            EliminarArticulobutton.IsEnabled = false;
                            Guardarbutton.IsEnabled = false;
                            cbguia.IsEnabled = false;
                            cbnotaentrega.IsEnabled = false;
                        }
                        ClientetextBox.IsEnabled = false;
                        DirecciontextBox.IsEnabled = false;
                        txtemail.IsEnabled = false;
                        BuscarClientebutton.IsEnabled = false;
                        fechapedido.IsEnabled = false;
                        seriecomboBox.IsEnabled = false;
                        NotasInternastextBox.IsEnabled = false;
                        NotastextBox.IsEnabled = false;
                        Reabrirbutton.IsEnabled = false;
                        Anularbutton.IsEnabled = false;
                        Emitirbutton.IsEnabled = false;
                        btnimportar.IsEnabled = false;
                        cbtipopedido.IsEnabled = false;
                        txtdepartamento.IsEnabled = false;
                        break;
                    case Constantes.ESTADO_ESPERA_APROBACION:
                    case Constantes.ESTADO_COMPLETADO:
                    case Constantes.ESTADO_ANULADO:
                        ClientetextBox.IsEnabled = false;
                        DirecciontextBox.IsEnabled = false;
                        txtemail.IsEnabled = false;
                        BuscarClientebutton.IsEnabled = false;
                        fechapedido.IsEnabled = false;
                        seriecomboBox.IsEnabled = false;
                        AgregarArticulobutton.IsEnabled = false;
                        EliminarArticulobutton.IsEnabled = false;
                        NotasInternastextBox.IsEnabled = false;
                        NotastextBox.IsEnabled = false;
                        Reabrirbutton.IsEnabled = false;
                        Anularbutton.IsEnabled = false;
                        Guardarbutton.IsEnabled = false;
                        Emitirbutton.IsEnabled = false;
                        btnimportar.IsEnabled = false;
                        cbtipopedido.IsEnabled = false;
                        txtdepartamento.IsEnabled = false;
                        cbguia.IsEnabled = false;
                        cbnotaentrega.IsEnabled = false;
                        switch (int.Parse(Cabecera.CAPEIDES.ToString()))
                        {
                            case Constantes.ESTADO_ESPERA_APROBACION:
                                NuevoPedidolabel.Text = "Vista Previa: Pedido en espera de Aprobación";
                                break;
                            case Constantes.ESTADO_COMPLETADO:
                                NuevoPedidolabel.Text = " Vista Previa: Pedido Completado";
                                break;
                            case Constantes.ESTADO_ANULADO:
                                NuevoPedidolabel.Text = "Vista Previa: Pedido Anulado";
                                break;
                        }
                        break;
                }
                //Carga la información del Pedido
                if (Cabecera.CAPEIDCP != 0)
                {

                    Ruccliente = Cabecera.TCLIE.CLIRUC.Trim();
                    cbtipopedido.SelectedValue = Cabecera.CAPETIPO;
                    ClientetextBox.Text = string.IsNullOrWhiteSpace(Cabecera.TCLIE.CLINOM) ? "" : Cabecera.TCLIE.CLINOM.Trim();
                    RazonSociallabel.Content = string.IsNullOrEmpty(Cabecera.TCLIE.CLINOM) ? "" : Cabecera.TCLIE.CLINOM.Trim();
                    seriecomboBox.ItemsSource = null;
                    seriecomboBox.Items.Add(Cabecera.CAPESERI);
                    seriecomboBox.SelectedIndex = 0;
                    fechapedido.SelectedDate = Cabecera.CAPEFECH;
                    DirecciontextBox.Text = string.IsNullOrWhiteSpace(Cabecera.CAPEDIRE) ? "" : Cabecera.CAPEDIRE.Trim();
                    txtdepartamento.Text = string.IsNullOrWhiteSpace(Cabecera.CAPEDEST) ? "" : Cabecera.CAPEDEST.Trim();
                    txtemail.Text = string.IsNullOrWhiteSpace(Cabecera.CAPEEMAI) ? "" : Cabecera.CAPEEMAI.Trim();
                    numpedlabel.Content = Cabecera.CAPENUME.ToString().Trim().PadLeft(7, '0');
                    NotastextBox.Text = string.IsNullOrWhiteSpace(Cabecera.CAPENOTG) ? "" : Cabecera.CAPENOTG.Trim();
                    NotasInternastextBox.Text = string.IsNullOrWhiteSpace(Cabecera.CAPENOTI) ? "" : Cabecera.CAPENOTI.Trim();
                    RucLabel.Content = "RUC: " + Ruccliente;
                    //Cargar los radiobutton
                    switch (int.Parse(Cabecera.CAPEIDTD.ToString()))
                    {
                        case Constantes.ID_TIPO_DOC_GUIA:
                            cbguia.IsChecked = true;
                            cbnotaentrega.IsChecked = false;
                            break;
                        case Constantes.ID_TIPO_DOC_NE:
                            cbnotaentrega.IsChecked = true;
                            cbguia.IsChecked = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            //Limpia todo el form, variables y listas
            try
            {
                tipo_ped = 1;
                Ruccliente = null;
                editarpedido = false;
                Cabecera = new PECAPE();
                listadetallegrilla = null;
                DetallePedidodataGrid.ItemsSource = null;
                cbtipopedido.SelectedValue = Constantes.VENTA;
                ClientetextBox.Text = "";
                DirecciontextBox.Text = "";
                fechapedido.SelectedDate = DateTime.Now;
                nuevoped = true;
                txtemail.Text = "";
                NotasInternastextBox.Text = "";
                NotastextBox.Text = "";
                RucLabel.Content = "";
                RazonSociallabel.Content = "";
                numpedlabel.Content = "";
                listaeliminar.Clear();
                CargaSerie("REASONS ");
                txtdepartamento.Text = "";
                cbguia.IsChecked = true;
                Validaform();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CapturaEventos(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.Escape))
                {
                    if (ClientetextBox.IsFocused)
                    {
                        ClientetextBox.Text = "";
                        ClientetextBox.Focus();
                    }
                    else if (DirecciontextBox.IsFocused)
                    {
                        DirecciontextBox.Text = "";
                    }
                    else if (NotasInternastextBox.IsFocused)
                    {
                        NotasInternastextBox.Text = "";
                    }
                    else if (NotastextBox.IsFocused)
                    {
                        NotastextBox.Text = "";
                    }
                    else if (txtemail.IsFocused)
                    {
                        txtemail.Text = "";
                    }
                    else
                    {
                        this.Close();
                    }
                }
                if ((e.Key == Key.Enter))
                {
                    if (ClientetextBox.IsFocused)
                    {
                        RealizabusquedaCliente();
                        ClientetextBox.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RealizaCambioEstado(int estado)
        {
            try
            {
                bool valida = false;
                switch (estado)
                {
                    case Constantes.ESTADO_CREADO:
                        if (MessageBox.Show("¿Está seguro que desea Reabrir este Pedido?", "¿Reabrir Pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            valida = true;
                            Cabecera.CAPEIDES = Constantes.ESTADO_CREADO;
                        }
                        break;
                    case Constantes.ESTADO_EMITIDO:
                        if (string.IsNullOrWhiteSpace(ClientetextBox.Text) || string.IsNullOrWhiteSpace(DirecciontextBox.Text) || fechapedido.SelectedDate == null)
                        {
                            MessageBox.Show("Los datos del cliente son incorrectos o estan vacíos", "Verifique Cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (DetallePedidodataGrid.Items.Count > 0)
                            {
                                valida = true;
                                Cabecera.CAPEIDES = Constantes.ESTADO_EMITIDO;
                            }
                            else
                            {
                                MessageBox.Show("No es posible Emitir un pedido sin un detalle de Artículos", "Debe agregar Artículos", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        break;
                    case Constantes.ESTADO_ANULADO:
                        if (MessageBox.Show("¿Está seguro que desea Anular este Pedido?", "¿Anular Pedido?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            valida = true;
                            Cabecera.CAPEIDES = Constantes.ESTADO_EMITIDO;
                        }
                        break;
                }
                if (valida)
                {
                    List<PECAPE> ListCambiaestado = new List<PECAPE>();
                    ListCambiaestado.Add(Cabecera);
                    Cambiaestalist(ListCambiaestado, estado.ToString(), ParametrosFe.Usuario.Trim());
                    Validaform();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RealizaImportacion()
        {
            string Path; //Almacena la direccion del archivo excel
            string Path2 = ""; //Almacena la direccion de destino para exportar errores

            try
            {
                DataTable dtgenerado;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Excel Files | *.xlsx; *.xls;";// *.xlsm; *.xlsb; *.xltx; *.xltm; *.xlt; *.xls; *‌​.xml; *.xml; *.xlam; *.‌​xla; *.xlw; *.xlr|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == true)
                {
                    int items = 1;
                    Path = openFileDialog.FileName;
                    dtgenerado = ImportarExcel(Path, 0);
                    List<PEDEPE> listerrores = new List<PEDEPE>(); //almacena los errores
                    DataTable Errores = new DataTable();
                    Errores.Columns.Add("ITEMS");
                    Errores.Columns.Add("ARTICULO");
                    Errores.Columns.Add("PARTIDA");
                    Errores.Columns.Add("ALMACEN");
                    Errores.Columns.Add("UNID");
                    Errores.Columns.Add("PESO NETO");
                    Errores.Columns.Add("REPETIDO");

                    for (int i = 0; i < dtgenerado.Rows.Count; i++)
                    {
                        items++;
                        PEDEPE objdetalle = new PEDEPE();
                        objdetalle.DEPECOAR = dtgenerado.Rows[i]["ARTICULO"] is DBNull ? "" : dtgenerado.Rows[i]["ARTICULO"].ToString();
                        objdetalle.DEPEPART = dtgenerado.Rows[i]["PARTIDA"] is DBNull ? "" : dtgenerado.Rows[i]["PARTIDA"].ToString();
                        objdetalle.DEPEALMA = Convert.ToInt32(dtgenerado.Rows[i]["ALMACEN"] is DBNull ? 0 : dtgenerado.Rows[i]["ALMACEN"]);
                        objdetalle.DEPECONT = "999999";
                        decimal cantidad;
                        if (decimal.TryParse(dtgenerado.Rows[i]["UNID"].ToString(), out cantidad))
                        {
                            objdetalle.DEPECASO = cantidad;
                        }
                        else
                        {
                            objdetalle.DEPECASO = 0;
                        }
                        decimal peso;
                        if (decimal.TryParse(dtgenerado.Rows[i]["PESO NETO"].ToString(), out peso)) // && dtgenerado.Rows[i]["SOLICITADO"] != DBNull.Value
                        {
                            objdetalle.DEPEPESO = peso;
                        }
                        else
                        {
                            objdetalle.DEPEPESO = 0;
                        }
                        objdetalle.DEPEDSAR = "";
                        objdetalle.DEPEDISP = 0;

                        var repetido = listadetallegrilla.FirstOrDefault(x => x.DEPECOAR == objdetalle.DEPECOAR && x.DEPEPART == objdetalle.DEPEPART && x.DEPEALMA == objdetalle.DEPEALMA);
                        if ((ValidaItemsExcel(objdetalle) && objdetalle.DEPEPESO > 0) && repetido == null)
                        {
                            listadetallegrilla.Add(objdetalle);
                        }
                        else
                        {
                            DataRow row = Errores.NewRow();
                            row["ITEMS"] = items;
                            row["ARTICULO"] = objdetalle.DEPECOAR;
                            row["PARTIDA"] = objdetalle.DEPEPART;
                            row["ALMACEN"] = objdetalle.DEPEALMA;
                            row["UNID"] = objdetalle.DEPECASO;
                            row["PESO NETO"] = objdetalle.DEPEPESO;
                            if (repetido != null)
                            {
                                row["REPETIDO"] = true;
                            }
                            Errores.Rows.Add(row);
                        }
                    }
                    DetallePedidodataGrid.ItemsSource = null;
                    DetallePedidodataGrid.ItemsSource = listadetallegrilla;
                    if (Errores.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Una o mas filas del documento contienen datos inválidos ¿Desea crear un nuevo documento con las filas erroneas?", "Errores durante la subida", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "Excel Files | *.xlsx; *.xls;";
                            saveFileDialog.Title = "Guardar Lista de Errores";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                Path2 = saveFileDialog.FileName;
                            }
                            EXPORT_DATATABLE_TO_EXCEL_XLSX_USE_EPPLUS(Errores, "Lista de errores", Path2);
                            //ExportarExcel(Errores, Path2); // genera un archivo excel con errores pero funciona
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CargaSerie(string codcliente)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.CARGA_SERIE_CLIENTE;
                List<string> parEnt = new List<string>();
                parEnt.Add(codcliente);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0] != null)
                    {
                        seriecomboBox.ItemsSource = null;
                        if (seriecomboBox.Items.Count > 0)
                        {
                            seriecomboBox.Items.Clear();
                        }
                        seriecomboBox.ItemsSource = resultado.VALSAL;
                        if (seriecomboBox.Items.Count > 0)
                        {
                            seriecomboBox.SelectedIndex = 0;
                        }
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
                MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        private bool BuscaCLiente(string Busqueda)
        {

            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.BUSCA_CLIENTE;
                List<string> parEnt = new List<string>();
                parEnt.Add(Busqueda);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.TCLIE> listaobtenida = appWcfService.Utils.Deserialize<List<appWcfService.TCLIE>>(resultado.VALSAL[1]);
                        listaclientes = listaobtenida;
                        resultadoOpe = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró cliente.", "Sin coincidencias", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show(resultado.MENERR);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        private bool GuardaPedido(appWcfService.PECAPE objcabecera, List<appWcfService.PEDEPE> listdetalle, List<appWcfService.PEDEPE> listeliminados)
        {

            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_PEDIDO;
                List<string> parEnt = new List<string>();

                parEnt.Add(Utils.Serialize(objcabecera));
                parEnt.Add(Utils.Serialize(listdetalle));
                parEnt.Add(Utils.Serialize(listeliminados));





                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //Se va a trabajar sobre el mismo pedido 
                    Cabecera.CAPEIDCP = decimal.Parse(resultado.VALSAL[0]);
                    Cabecera.CAPENUME = decimal.Parse(resultado.VALSAL[1]);
                    Cabecera.CAPEIDES = decimal.Parse(resultado.VALSAL[2]);
                    List<PECAPE> pedido = new List<PECAPE>();
                    pedido.Add(Cabecera);
                    if (BuscaDetalle(pedido))
                    {
                        nuevoped = false;
                        listaeliminar.Clear();
                        resultadoOpe = true;
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el Pedido", "Falló", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        public bool Cambiaestalist(List<appWcfService.PECAPE> Listapedidos, string estado, string usuario)//cambia el estado 1 o mas items
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.CAMBIA_ESTADO_LISTA; //1
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(Listapedidos));
                parEnt.Add(estado);
                parEnt.Add(usuario);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        MessageBox.Show("Se ha actualizado correctamente el estado del Pedido", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultadoOpe = true;
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido cambiar el estado.", "Falló", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool BuscaDetalle(List<PECAPE> pedido)
        {
            //Obtiene el detalle del pedido seleccionado
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado; //Resultado de Operacion
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_DETALLE_PEDIDO;
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(pedido));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result> Listdetallerecuperado = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETALLE_PEDIDOS_Result>>(resultado.VALSAL[1]);
                        listadetallegrilla.Clear();
                        Listdetallerecuperado.ForEach(x =>
                        {
                            PEDEPE objdet = new PEDEPE();
                            objdet.DEPEIDDP = x.DEPEIDDP;
                            objdet.DEPEIDCP = x.DEPEIDCP;
                            objdet.DEPECOAR = x.DEPECOAR;
                            objdet.DEPEPART = x.DEPEPART;
                            objdet.DEPECONT = x.DEPECONT;
                            objdet.DEPEALMA = x.DEPEALMA;
                            objdet.DEPECASO = x.DEPECASO;
                            objdet.DEPEPESO = x.DEPEPESO;
                            objdet.DEPECAAT = x.DEPECAAT;
                            objdet.DEPEPEAT = x.DEPEPEAT;
                            objdet.DEPEPERE = x.DEPEPERE;
                            objdet.DEPETADE = x.DEPETADE;
                            objdet.DEPEPEBR = x.DEPEPEBR;
                            objdet.DEPESTOC = x.DEPESTOC;
                            objdet.DEPEESTA = x.DEPEESTA;
                            objdet.DEPEDISP = x.DEPEDISP;
                            objdet.DEPEDSAR = x.DEPEDSAR;
                            objdet.DEPENUBU = x.DEPENUBU;
                            objdet.DEPEUSCR = x.DEPEUSCR;
                            objdet.DEPEFECR = x.DEPEFECR;
                            objdet.DEPEUSMO = x.DEPEUSMO;
                            objdet.DEPEFEMO = x.DEPEFEMO;
                            objdet.DEPESERS = x.DEPESERS;
                            objdet.DEPESECU = x.DEPESECU;
                            listadetallegrilla.Add(objdet);
                        });
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
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_PEDIDO_CONSULTA; //1
                List<string> parEnt = new List<string>();
                parEnt.Add(Convert.ToString(idpedido));

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
                        MessageBox.Show("No se encontró el Pedido, por favor verifique", "No se pudo Imprimir", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("No se Pudo imprimir Pedido", "Falló al imprimir", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private DataTable ImportarExcel(string pRutaArchivo, int pHojaIndex)
        {
            DataTable Tabla = null;
            try
            {
                if (System.IO.File.Exists(pRutaArchivo))
                {

                    IWorkbook workbook = null;  //IWorkbook determina si es xls o xlsx              
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(pRutaArchivo, FileMode.Open, FileAccess.Read))
                    {
                        workbook = WorkbookFactory.Create(FS);          //Abre tanto XLS como XLSX
                        worksheet = workbook.GetSheetAt(pHojaIndex);    //Obtener Hoja por indice
                        first_sheet_name = worksheet.SheetName;         //Obtener el nombre de la Hoja

                        Tabla = new DataTable(first_sheet_name);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        // Leer Fila por fila desde la primera
                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            DataRow NewReg = null;
                            IRow row = worksheet.GetRow(rowIndex);
                            IRow row2 = null;
                            //IRow row3 = null;

                            if (rowIndex == 0)
                            {
                                row2 = worksheet.GetRow(rowIndex + 1); //Si es la Primera fila, obtengo tambien la segunda para saber el tipo de datos
                                                                       //row3 = worksheet.GetRow(rowIndex + 2); //Y la tercera tambien por las dudas
                            }

                            if (row != null) //null is when the row only contains empty cells 
                            {
                                if (rowIndex > 0) NewReg = Tabla.NewRow();

                                int colIndex = 0;
                                //Leer cada Columna de la fila
                                foreach (ICell cell in row.Cells)
                                {
                                    object valorCell = null;
                                    string cellType = "";
                                    string[] cellType2 = new string[2];

                                    if (rowIndex == 0) //Asumo que la primera fila contiene los titlos:
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            ICell cell2 = null;
                                            if (i == 0)
                                            {
                                                if (row2 != null)
                                                {
                                                    cell2 = row2.GetCell(cell.ColumnIndex);
                                                }
                                                else
                                                {
                                                    cell2 = null;
                                                }
                                            }
                                            //else { cell2 = row3.GetCell(cell.ColumnIndex); }

                                            if (cell2 != null)
                                            {
                                                switch (cell2.CellType)
                                                {
                                                    case CellType.Blank: break;
                                                    case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                    case CellType.String: cellType2[i] = "System.String"; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                        else
                                                        {
                                                            cellType2[i] = "System.Double";  //valorCell = cell2.NumericCellValue;
                                                        }
                                                        break;

                                                    case CellType.Formula:
                                                        bool continuar = true;
                                                        switch (cell2.CachedFormulaResultType)
                                                        {
                                                            case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                            case CellType.String: cellType2[i] = "System.String"; break;
                                                            case CellType.Numeric:
                                                                if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                                else
                                                                {
                                                                    try
                                                                    {
                                                                        //DETERMINAR SI ES BOOLEANO
                                                                        if (cell2.CellFormula == "TRUE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar && cell2.CellFormula == "FALSE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar) { cellType2[i] = "System.Double"; continuar = false; }
                                                                    }
                                                                    catch { }
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        cellType2[i] = "System.String"; break;
                                                }
                                            }
                                        }

                                        //Resolver las diferencias de Tipos
                                        if (cellType2[0] == null && cellType2[1] == null)
                                        {
                                            cellType = "System.String";
                                        }
                                        else
                                        {
                                            if (cellType2[0] == cellType2[1]) { cellType = cellType2[0]; }
                                            else
                                            {
                                                if (cellType2[0] == null) cellType = cellType2[1];
                                                if (cellType2[1] == null) cellType = cellType2[0];
                                                if (cellType == "") cellType = "System.String";
                                            }
                                        }
                                        //Obtener el nombre de la Columna
                                        string colName = "Column_{0}";
                                        try
                                        {
                                            colName = cell.StringCellValue;
                                        }
                                        catch
                                        {
                                            colName = string.Format(colName, colIndex);
                                        }

                                        //Verificar que NO se repita el Nombre de la Columna
                                        foreach (DataColumn col in Tabla.Columns)
                                        {
                                            if (col.ColumnName == colName) colName = string.Format("{0}_{1}", colName, colIndex);
                                        }

                                        //Agregar el campos de la tabla:
                                        DataColumn codigo = new DataColumn(colName, System.Type.GetType(cellType));
                                        Tabla.Columns.Add(codigo);
                                        colIndex++;
                                    }
                                    else
                                    {
                                        //Las demas filas son registros:
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                else { valorCell = cell.NumericCellValue; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                }
                                                break;
                                            default: valorCell = cell.StringCellValue; break;
                                        }
                                        //Agregar el nuevo Registro
                                        if (cell.ColumnIndex <= Tabla.Columns.Count - 1) NewReg[cell.ColumnIndex] = valorCell;
                                    }
                                }
                            }
                            if (rowIndex > 0 && NewReg != null) Tabla.Rows.Add(NewReg);
                        }
                        Tabla.AcceptChanges();
                        if (worksheet.LastRowNum == 0)
                        {
                            MessageBox.Show("No es posible importar un archivo vacío.", "Documento Vacio", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else
                {
                    throw new Exception("El archivo especificado NO existe.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tabla;
        }

        private bool ValidaItemsExcel(PEDEPE objdetalle)
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
                argumentos.CODOPE = CodigoOperacion.VALIDA_ITEMS_EXCEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(objdetalle));


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

        private bool ValidaPreparacion(PEDEPE objdetalle)
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
                argumentos.CODOPE = CodigoOperacion.VALIDA_PREPARACION;
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(objdetalle));
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

        public string[] EXPORT_DATATABLE_TO_EXCEL_XLSX_USE_EPPLUS(DataTable DTABLE, string SHEET_NAME, string FILE_EXCEL_XLSX, bool EXPORT_HEADER = true, int Row_Begin = 1, int Column_Begin = 1)
        {
            /*  PLEASE NOTE FOR USE
                -------------------
                STEP 1 - DOWNLOAD EPPLUS FROM EPPLUS.CODEPLEX.COM
                STEP 2 - ADD REFERENCE EPPLUS.DLL TO VISUAL STUDIO
                STEP 3 - SET COPY LOCAL = TRUE
                -------------------

                EXAMPLE FOR USE
                ---------------
                DataTable DTABLE = new DataTable();
                DTABLE.Columns.Add("ID", typeof(string));
                DTABLE.Columns.Add("NAME", typeof(string));
                DTABLE.Rows.Add("1", "A");
                DTABLE.Rows.Add("2", "B");
                DTABLE.Rows.Add("3", "C");
                DTABLE.Rows.Add("4", "D");
                DTABLE.Rows.Add("5", "E");
                DTABLE.Rows.Add("6", "F");
                string Excel_File = @"D:\DTable_Excel.xlsx";
                string[] STATUS = EXPORT_DATATABLE_TO_EXCEL_XLSX_USE_EPPLUS(DTABLE, "SHEET 1", Excel_File, true, 2, 3);
                if (STATUS[0] == "ERROR") { MessageBox.Show(STATUS[1], "ALERT"); }
                if (STATUS[0] == "OK") { System.Diagnostics.Process.Start(Excel_File); }
                ---------------
            */
            string[] KQ = { "OK", "" };

            if (Row_Begin <= 0 || Column_Begin <= 0)
            {
                KQ[0] = "Error";
                KQ[1] = "Se requiere que la fila y columna comiencen con más de cero";
                return KQ;
            }
            if (SHEET_NAME.Trim() == "")
            {
                KQ[0] = "Error";
                KQ[1] = "El nombre no puede estar vacío";
                return KQ;
            }
            if (FILE_EXCEL_XLSX.Trim() == "")
            {
                KQ[0] = "Error";
                KQ[1] = "La extensión de la fila no puede estar vacía";
                return KQ;
            }
            try
            {
                System.IO.FileStream XFile = new System.IO.FileStream(FILE_EXCEL_XLSX.Trim(), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                using (OfficeOpenXml.ExcelPackage ExPCK = new OfficeOpenXml.ExcelPackage(XFile))
                {
                    OfficeOpenXml.ExcelWorksheet EWS = ExPCK.Workbook.Worksheets.Add(SHEET_NAME.Trim());
                    EWS.Cells[Row_Begin, Column_Begin].LoadFromDataTable(DTABLE, EXPORT_HEADER);
                    ExPCK.Save();
                }
                XFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.Message.ToLower().IndexOf("No se puede acceder al archivo") != -1 && ex.Message.ToLower().IndexOf("porque esta siendo usado por otro programa") != -1)
                {
                    KQ[0] = "ERROR";
                    KQ[1] = "NO PUEDE ESCRIBIR DATOS. LA PÁGINA ESTA SIENDO UTILIZADAS POR OTRO PROGRAMA.";
                }
                else
                {
                    KQ[0] = "ERROR";
                    KQ[1] = ex.ToString();
                }
            }
            return KQ;
        }

        #endregion
    }
}
