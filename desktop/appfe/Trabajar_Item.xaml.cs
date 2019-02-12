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
    /// Interaction logic for Trabajar_Item.xaml
    /// </summary>
    public partial class Trabajar_Item : Window
    {
        ParametrosFe _ParametrosIni;
        public Trabajar_Item()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        #region Variables
        public appWcfService.PEDEPE objdetalle = new appWcfService.PEDEPE(); //Objeto para manejar y almacenar valores obtenidos o modificados
        USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result bolsaseleccionada = new USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result();
        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> listadetallegrilla = new List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>();
        List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> Listelimina = new List<USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>(); //Guardamos items para emitirlos
        public appWcfService.PEBOLS bolsa = new appWcfService.PEBOLS();
        decimal incluyebolsa, pcono, pbolsa;
        bool cambio;
        //string usuario = "ADMIN";

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
                DetalleItemsdataGrid.CanUserAddRows = false;
                Ubicacionesdatagrid.CanUserAddRows = false;
                cambio = false;

                Muestraubicacionesarticulo(objdetalle.DEPECOAR, objdetalle.DEPEPART, objdetalle.DEPEALMA.ToString());
                pesosolicitadolabel.Content = objdetalle.DEPEPESO.ToString() + " KG";
                almacenlabel.Content = objdetalle.DEPEALMA.ToString();
                trabajaritemlabel.Text = "Preparar Item" + "    Artículo: " + objdetalle.DEPECOAR + "    Partida: " + objdetalle.DEPEPART + "    Contrato: " + objdetalle.DEPECONT;
                if (objdetalle.DEPESTOC == 1)
                {
                    stockcheckbox.IsChecked = true;
                }
                Obtienedetpreparacion(objdetalle.DEPEIDDP);
                totaliza();
                obtieneDatosPartida(objdetalle.DEPECOAR, objdetalle.DEPEPART, objdetalle.DEPEALMA.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cancelarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!cambio)
                //{
                this.Close();
                //}
                //else
                //{
                //    if (MessageBox.Show("¿Está seguro que desea salir sin guardar? Se perderán todos los cambios ", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //    {
                //        this.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void guardarbutton_Click(object sender, RoutedEventArgs e)
        {
            listadetallegrilla = DetalleItemsdataGrid.ItemsSource.Cast<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>().ToList();
            appWcfService.PEBODP bodp = new appWcfService.PEBODP();
            List<appWcfService.PEBODP> listBolsas = new List<appWcfService.PEBODP>();

            try
            {
                foreach (var item in listadetallegrilla)
                {
                    bodp.BODPIDDE = item.BODPIDDE;
                    bodp.PEBOLS = new PEBOLS();
                    if (!string.IsNullOrWhiteSpace(item.BOLSCOEM))
                    {
                        bodp.PEBOLS.BOLSCOEM = item.BOLSCOEM;
                        bodp.BODPIDBO = item.BOLSIDBO;
                    }
                    else
                    {
                        bodp.PEBOLS.BOLSCOEM = "";
                    }
                   
                    bodp.BODPIDDP = objdetalle.DEPEIDDP;
                    bodp.BODPCANT = item.BODPCANT;
                    bodp.BODPPESO = item.BODPPESO;
                    bodp.BODPUSCR = ParametrosFe.Usuario;//VER COMO PASAR EL USUARIO
                    bodp.BODPPERE = item.BODPPERE;
                    bodp.BODPSTCE = item.BODPSTCE;
                    bodp.BODPINBO = item.BODPINBO;
                    bodp.BODPDIFE = item.BODPDIFE;
                    bodp.BODPTADE = item.BODPTADE;
                    bodp.BODPPEBR = item.BODPPEBR;
                    bodp.BODPTAUN = item.BODPTAUN;
                    bodp.PEDEPE = new PEDEPE();
                    bodp.PEDEPE.DEPESTOC = (stockcheckbox.IsChecked == true) ? 1 : 0;
                    listBolsas.Add(bodp);
                    
                }
                if (guardaPreparacionBolsa(listBolsas))
                {
                    MessageBox.Show("Se ha guardado correctamente.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                    cambio = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //RemueveBolsa();getBODPIDDE(),

            //hacer eliminar aqui :D

        }

        private void AgregarBolsabutton_Click(object sender, RoutedEventArgs e)
        {
            if (CargaBolsa(objdetalle.DEPEIDDP, CodigoBolsatextBox.Text))
                CodigoBolsatextBox.Text = "";
            cambio = true;
            totaliza();
        }

        private void btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            CodigoBolsatextBox.Text = "";
        }

        private void DetalleItemsdataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Editar_Bolsa Formeditarbolsa = new Editar_Bolsa();
            bolsaseleccionada = DetalleItemsdataGrid.SelectedItem as appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result;

            if (bolsaseleccionada != null)
            {
                Formeditarbolsa.objbolsa = bolsaseleccionada;
                Formeditarbolsa.btnmulti = false;
                Formeditarbolsa.Owner = Window.GetWindow(this);
                Formeditarbolsa.ShowDialog();
            }


            DetalleItemsdataGrid.UpdateLayout();
            CollectionViewSource.GetDefaultView(DetalleItemsdataGrid.ItemsSource).Refresh();

            cambio = true;
            totaliza();
            //cantidadlabel.Content = bolsaseleccionada.BODPDIFE.ToString();
        }

        private void Eliminarbutton_Click(object sender, RoutedEventArgs e)
        {
            //getBODPIDDE()

            if (DetalleItemsdataGrid.Items.Count == 0) //Si la grilla no tiene items mandamos un mensaje
            {
                MessageBox.Show("Acción erronea, No hay bolsas registradas.", "Incorrecto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                listadetallegrilla = DetalleItemsdataGrid.ItemsSource.Cast<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>().ToList();
                foreach (var item in listadetallegrilla) //Recorrer la lista de la grilla para separar los que tienen check
                {
                    bolsaseleccionada = DetalleItemsdataGrid.SelectedItem as appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result;
                    if (bolsaseleccionada != null)
                    {
                        Listelimina.Add(item);
                        //listadetallegrilla.Remove(item);
                    }
                }
                if (Listelimina.Count != 0)
                {
                    if (MessageBox.Show("¿Está seguro que desea eliminar la bolsa? Se perderán todos los cambios ", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        RemueveBolsa(bolsaseleccionada.BODPIDDE);
                        listadetallegrilla.Remove(bolsaseleccionada);
                        //establece el nuevo itemsource sin los items eliminados
                        DetalleItemsdataGrid.ItemsSource = listadetallegrilla;
                    }
                    //MessageBox.Show("La bolsa ha sido eliminada correctamente.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
                    totaliza();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar al menos un item para eliminar", "Seleccionar item", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }


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
                    if (CodigoBolsatextBox.IsFocused)
                    {
                        if (CargaBolsa(objdetalle.DEPEIDDP, CodigoBolsatextBox.Text))
                            CodigoBolsatextBox.Text = "";
                        cambio = true;
                        totaliza();
                    }
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
                if (!cambio)
                {
                    e.Cancel = false;
                }
                else
                {
                    if (MessageBox.Show("¿Está seguro que desea salir sin guardar? Se perderán todos los cambios ", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        e.Cancel = false;
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CodigoBolsatextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    e.Handled = false;
                else
                    e.Handled = true;

                if ((e.Key == Key.Escape))
                {
                    if (String.IsNullOrEmpty(CodigoBolsatextBox.Text.ToString()))
                    {
                        this.Close();
                    }
                    else CodigoBolsatextBox.Text = "";
                }
                if ((e.Key == Key.Enter))
                {
                    if (CodigoBolsatextBox.IsFocused)
                    {
                        if (CargaBolsa(objdetalle.DEPEIDDP, CodigoBolsatextBox.Text))
                            CodigoBolsatextBox.Text = "";
                        cambio = true;
                        totaliza();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pesadomultiplebutton_Click(object sender, RoutedEventArgs e)
        {
            Editar_Bolsa Formeditarbolsa = new Editar_Bolsa();
            try
            {
                listadetallegrilla = DetalleItemsdataGrid.ItemsSource.Cast<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>().ToList();

                if (listadetallegrilla.Count >= 1)
                {
                    Formeditarbolsa.listagrilla = listadetallegrilla;
                    Formeditarbolsa.btnmulti = true;
                    Formeditarbolsa.Owner = Window.GetWindow(this);

                    Formeditarbolsa.ShowDialog();
                }
                else
                    MessageBox.Show("Debe tener más de una bolsa registrada.", "Pesado Multiple", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                DetalleItemsdataGrid.UpdateLayout();
                CollectionViewSource.GetDefaultView(DetalleItemsdataGrid.ItemsSource).Refresh();

                cambio = true;
                totaliza();
            }
            catch (Exception)
            {
                MessageBox.Show("Debe tener más de una bolsa registrada.", "Pesado Multiple", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DetalleItemsdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void sinEmpaquebutton_Click(object sender, RoutedEventArgs e)
        {
            var bolsa = listadetallegrilla.Find(x => x.BOLSCOEM == "");
            if (bolsa == null)
            {
                appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result objdet = new appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result();
                objdet.DEPEIDDP = null;
                //objdet.BOLSIDBO = 0;
                objdet.BOLSCOEM = "";
                objdet.BOLSARTI = objdetalle.DEPECOAR;
                objdet.BOLSPART = objdetalle.DEPEPART;
                objdet.BOLSALMA = objdetalle.DEPEALMA;
                objdet.BOLSCANT = 0;
                objdet.BOLSPESO = 0;
                objdet.TIEMTARA = 0.1M;
                objdet.UNIDTARA = 0;
                objdet.BODPCANT = 0;
                objdet.BODPPESO = 0;
                objdet.BODPINBO = 0;
                objdet.BODPPEBR = 0;
                objdet.BODPPERE = 0;
                objdet.BODPTAUN = 0;

                listadetallegrilla.Add(objdet);

                DetalleItemsdataGrid.ItemsSource = null;
                DetalleItemsdataGrid.ItemsSource = listadetallegrilla;
            }
            else
            {
                MessageBox.Show("Ya se ha ingresado el código de empaque, por favor ingrese un código de empaque diferente.", "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        #endregion

        #region Metodos
        public bool Muestraubicacionesarticulo(string articulo, string partida, string almacen)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.MUESTRA_UBICACIONES_ARTICULO;

                List<string> parEnt = new List<string>();
                parEnt.Add(articulo);
                parEnt.Add(partida);
                parEnt.Add(almacen);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_UBICACIONES_Result> Listubicacionesarticulo = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_UBICACIONES_Result>>(resultado.VALSAL[1]);
                        Ubicacionesdatagrid.ItemsSource = Listubicacionesarticulo;
                        resultadoOpe = true;
                    }
                    else
                    {
                        Ubicacionesdatagrid.ItemsSource = null;
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

        public bool CargaBolsa(decimal iddetalle, string empaque)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_BOLSA;
                List<string> parEnt = new List<string>();
                parEnt.Add(iddetalle.ToString());
                parEnt.Add(empaque);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_BOLSA_Result> ListBolsas = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_BOLSA_Result>>(resultado.VALSAL[1]);
                        resultadoOpe = true;
                        //listadetallegrilla.Clear();
                        foreach (var item in ListBolsas)
                        {
                            appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result objdet = new appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result();
                            objdet.DEPEIDDP = item.DEPEIDDP;
                            objdet.BOLSIDBO = item.BOLSIDBO;
                            objdet.BOLSCOEM = item.BOLSCOEM;
                            //objdet.BOLSCOCA = item.BOLSCOCA;
                            objdet.BOLSARTI = item.BOLSARTI;
                            objdet.BOLSPART = item.BOLSPART;
                            objdet.BOLSALMA = item.BOLSALMA;
                            objdet.BOLSCANT = item.BOLSCANT;
                            objdet.BOLSPESO = item.BOLSPESO;
                            objdet.TIEMTARA = item.TIEMTARA;
                            objdet.UNIDTARA = item.UNIDTARA;
                            objdet.BODPCANT = item.BOLSCANT;
                            objdet.BODPPESO = item.BOLSPESO;
                            objdet.BODPINBO = 1;
                            objdet.BODPPEBR = (item.UNIDTARA * objdet.BODPCANT) + objdet.BODPPESO + objdet.TIEMTARA.Value;

                            //objdet.BODPPERE = objdet.BODPPEBR - ((objdet.UNIDTARA * objdet.BODPCANT) + objdet.TIEMTARA);

                            objdet.BODPPERE = item.BOLSPESO;


                            objdet.BODPTAUN = item.UNIDTARA;
                            //objdet.BODPPEBR = item.BOLSPESO;

                            //listadetallegrilla.Add(objdet);
                            var bolsa = listadetallegrilla.Find(x => x.BOLSCOEM == objdet.BOLSCOEM);
                            if (bolsa == null)
                            {
                                listadetallegrilla.Add(objdet);
                            }
                            else
                            {
                                MessageBox.Show("Ya se ha ingresado el código de empaque, por favor ingrese un código de empaque diferente.", "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        DetalleItemsdataGrid.ItemsSource = null;
                        DetalleItemsdataGrid.ItemsSource = listadetallegrilla;
                    }
                    else
                    {
                        MessageBox.Show("Código de empaque incorrecto", "Código de empaque incorrecto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool RemueveBolsa(decimal idbolsapedido)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.REMUEVE_BOLSA_PEDIDO;
                List<string> parEnt = new List<string>();
                parEnt.Add(idbolsapedido.ToString());
                parEnt.Add(ParametrosFe.Usuario);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    MessageBox.Show("La bolsa ha sido eliminada correctamente.", "Eliminar", MessageBoxButton.OK, MessageBoxImage.Information);
                    cambio = false;
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

        public bool Obtienedetpreparacion(decimal iddetalle)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_DETALLE_PREPRACION;
                List<string> parEnt = new List<string>();
                parEnt.Add(iddetalle.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> Listdetpreparacion = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>>(resultado.VALSAL[1]);
                        DetalleItemsdataGrid.ItemsSource = Listdetpreparacion;
                        resultadoOpe = true;
                    }
                    else
                    {
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

        public bool guardaPreparacionBolsa(List<PEBODP> bolsa)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_PREPARACION_BOLSA;
                List<string> parEnt = new List<string>();
                parEnt.Add(Utils.Serialize(bolsa));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //limpiar la lista y cargar denuevo con la funcion de 
                    //bolsas preparadas para obtener el bodpidde de cada bolsa
                    //Double idprep = Double.Parse(resultado.VALSAL[0]);
                    //bolsa.BODPIDDE = Decimal.Parse(idprep.ToString());
                    DetalleItemsdataGrid.ItemsSource = null;
                    Obtienedetpreparacion(objdetalle.DEPEIDDP);
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

        public void totaliza()
        {
            try
            {
                listadetallegrilla = DetalleItemsdataGrid.ItemsSource.Cast<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>().ToList();
                decimal totcant, totpesobru, tottar, totpesonet, diferencia, cantAtend, pesoAtend, totpesobrut;
                totcant = 0;
                totpesobru = 0;
                tottar = 0;
                totpesobrut = 0;
                totpesonet = 0;

                foreach (var item in listadetallegrilla)
                {
                    decimal taracal, brutocal, pesobruto;

                    incluyebolsa = item.BODPINBO;
                    pbolsa = item.TIEMTARA.Value;
                    pcono = item.UNIDTARA.Value;
                    cantAtend = item.BODPCANT;
                    pesoAtend = item.BODPPESO;
                    pesobruto = item.BODPPEBR - item.BODPTADE;//el peso bruto de la bolsa

                    taracal = pcono * cantAtend + (pbolsa * incluyebolsa);

                    brutocal = pesoAtend + taracal;//el peso modificado es diferente del peso bruto, cuando cambien el peso neto  no cmbiaria el calculo???

                    totcant += cantAtend;
                    totpesonet += pesoAtend;
                    totpesobru += brutocal;
                    tottar += taracal;

                    totpesobrut += pesobruto;

                }
                diferencia = objdetalle.DEPEPESO - totpesonet;

                cantidadlabel.Content = Double.Parse(totcant.ToString()) + " UND";
                pesobrutolabel.Content = Double.Parse(totpesobrut.ToString()) + " KG";
                taralabel.Content = Double.Parse(tottar.ToString()) + " KG";
                pesonetolabel.Content = Double.Parse(totpesonet.ToString()) + " KG";
                diferencialabel.Content = Math.Round((Double.Parse(objdetalle.DEPEPESO.ToString()) - Double.Parse(totpesonet.ToString())), 2) + " KG";
            }
            catch (Exception)
            {
                cantidadlabel.Content = " UND";
                pesobrutolabel.Content = " KG";
                taralabel.Content = " KG";
                pesonetolabel.Content = " KG";
                diferencialabel.Content = "00.00 KG";
            }


            //tvdiferenciapeso.setText(String.valueOf(diferencia) + " KG");
        }

        public bool obtieneDatosPartida(string articulo, string partida, string almacen)
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_DATOS_PARTIDA;

                List<string> parEnt = new List<string>();
                parEnt.Add(articulo);
                parEnt.Add(partida);
                parEnt.Add(almacen);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    string color;
                    double peso;

                    peso = Double.Parse(resultado.VALSAL[0]) / 100;
                    totalpartidalabel.Content = peso.ToString() + " KG";

                    color = resultado.VALSAL[1] + " - " + resultado.VALSAL[2];

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
        #endregion

    }
}
