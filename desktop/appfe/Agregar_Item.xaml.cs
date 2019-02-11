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
using System.ComponentModel;

namespace appfe
{
    /// <summary>
    /// Interaction logic for Agregar_Item.xaml
    /// </summary>
    public partial class Agregar_Item : Window
    {
        ParametrosFe _ParametrosIni;
        public Agregar_Item()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");
        }

        #region "Variables"
        List<CARGACOMBO> listacombo = new List<CARGACOMBO>
        {
            new CARGACOMBO
            {
                DESCRIPCION="Partidas con stock disponible",
                VALOR = 0,
            },
            new CARGACOMBO
            {
                DESCRIPCION="Partidas sin stock Emitidas",
                VALOR = 1,
            },
             new CARGACOMBO
            {
                DESCRIPCION="Partidas sin stock Terminadas/Rematadas",
                VALOR = 2,
            }
        };
        public string rucclie;
        public decimal tipoped = 0;
        public List<PEDEPE> ListArticulos = new List<PEDEPE>(); //ListaPrincipal que almacena los articulos

        #endregion

        #region "Eventos"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatosIniciales();
        }
        private void CargaDatosIniciales()
        {
            //no permitir agregar repetidos a la lista
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
                    Partidascombobox.ItemsSource = listacombo;
                    Partidascombobox.DisplayMemberPath = "DESCRIPCION";
                    Partidascombobox.SelectedValuePath = "VALOR";
                    Partidascombobox.SelectedIndex = 0;
                    BusyBar.IsBusy = false;
                };
                bk.RunWorkerAsync();
                PartidatextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Buscarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                realizarbusquedarticulo();
                PartidatextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void limpiartextsbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ArticulotextBox.Text = "";
                PartidatextBox.Text = "";
                ContratotextBox.Text = "";
                PartidatextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AgregarSeleccionbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AgregarSeleccionados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Cancelarbutton_Click(object sender, RoutedEventArgs e) => Close();

        private void limpiarbusquedabutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Limpia la grilla y elimina todos los que no se hayan seleccionado
                ListArticulos.RemoveAll(x => x.CHECKSEL == false);
                ArticulosdataGrid.ItemsSource = null;
                ArticulosdataGrid.ItemsSource = ListArticulos;
                PartidatextBox.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Escape))
            {
                if (ArticulotextBox.IsFocused)
                {
                    ArticulotextBox.Text = "";
                }
                else if (PartidatextBox.IsFocused)
                {
                    PartidatextBox.Text = "";
                }
                else if (ContratotextBox.IsFocused)
                {
                    ContratotextBox.Text = "";
                }
                else
                {
                    Close();
                }
            }
            else if ((e.Key == Key.Enter))
            {
                if (ArticulotextBox.IsFocused || PartidatextBox.IsFocused || ContratotextBox.IsFocused)
                {
                    realizarbusquedarticulo();
                    PartidatextBox.Focus();
                }
            }
        }
        private void ArticulosdataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion

        #region "Metodos"

        private void AgregarSeleccionados()
        {
            try
            {
                bool valida = true; //Flag de validación
                ListArticulos = ArticulosdataGrid.ItemsSource.Cast<PEDEPE>().ToList(); //Volvemos a asignar a la lista lo de la Grilla
                if (ListArticulos.Count > 0)
                {
                    ListArticulos.RemoveAll(x => x.CHECKSEL == false); // Solo conservamos los checkeados
                    ListArticulos.ForEach(x => // Marcamos los que sobrepasan la reserva
                    {
                        if (x.DEPEDISP - x.LOTCANRE < x.DEPEPESO)
                        {
                            x.CHECKRESE = true;
                        }
                        x.DEPECASO = decimal.Floor(x.DEPECASO);
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
                        if (MessageBox.Show("Los siguientes Articulos sobrepasan la Reserva ¿Desea continuar?"
                          + Environment.NewLine
                          + Environment.NewLine + sb.ToString(), "Se sobrepasó la Reserva.", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        {
                            valida = false;
                        }
                    }
                }
                if (ListArticulos.Count >= 1)
                {
                    if (valida)
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Debe marcar al menos un Artículo para agregarlo al Pedido.", "Marcar artículos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void realizarbusquedarticulo()
        {
            try
            {
                if (ContratotextBox.Text == "" && ArticulotextBox.Text == "" && PartidatextBox.Text == "")
                {
                    MessageBox.Show("Para realizar una búsqueda debe ingresar al menos un Contrato, Artículo o Partida.", "Acción incorrecta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string contrato = ContratotextBox.Text.Trim();
                    string articulo = ArticulotextBox.Text.Trim();
                    string partida = PartidatextBox.Text.Trim();
                    string seleccion = Partidascombobox.SelectedValue.ToString();
                    BackgroundWorker bk = new BackgroundWorker();
                    BusyBar.IsBusy = true;
                    bk.DoWork += (o, e) =>
                    {
                        try
                        {
                            BuscaArticulo(contrato, articulo, partida, seleccion);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    };
                    bk.RunWorkerCompleted += (o, e) =>
                    {
                        ArticulosdataGrid.ItemsSource = null;
                        ArticulosdataGrid.ItemsSource = ListArticulos;
                        BusyBar.IsBusy = false;
                    };
                    bk.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool BuscaArticulo(string contrato, string articulo, string partida, string seleccion)
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
                argumentos.CODOPE = CodigoOperacion.BUSCA_ARTICULO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(contrato);
                parEnt.Add(articulo);
                parEnt.Add(partida);
                parEnt.Add(seleccion);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.USP_OBTIENE_ARTICULOS_Result> Listaconsulta = appWcfService.Utils.Deserialize<List<appWcfService.USP_OBTIENE_ARTICULOS_Result>>(resultado.VALSAL[1]);
                        //si no selecciona el cliente no hace nada 
                        if (tipoped == Constantes.VENTA)
                        {
                            if (rucclie != null && rucclie != "")
                            {
                                if (string.IsNullOrWhiteSpace(contrato)) //&& tipo venta
                                {
                                    Listaconsulta = Listaconsulta.Where(x => (x.MCRUC == rucclie && x.ASIGNUPE != "999999") || x.MCRUC != rucclie && x.ASIGNUPE == "999999").ToList();
                                }
                                else
                                { //por validar para solo mostrar los contratos del cliente. 20/04/2018
                                    Listaconsulta = Listaconsulta.Where(x => (x.MCRUC == rucclie)).ToList();
                                }
                            }
                        }
                        Listaconsulta.ForEach(x =>
                        {
                            var objrepetido = ListArticulos.Find(z => z.DEPECOAR == x.LOTITEM && z.DEPEPART == x.LOTPARTI && z.DEPEALMA == x.LOTALM);
                            if (objrepetido == null)
                            {
                                PEDEPE objart = new PEDEPE
                                {
                                    DEPECOAR = x.LOTITEM,
                                    DEPEPART = x.LOTPARTI,
                                    DEPEDISP = x.LOTSTOCK,
                                    DEPEALMA = x.LOTALM,
                                    DEPEPESO = x.PESOSOL,
                                    DEPECASO = x.CANTSOL,
                                    DEPECONT = x.ASIGNUPE,
                                    DEPEDSAR = x.CALIDEAB + "" + x.CALICOMP,
                                    LOTCANRE = x.LOTCANRE,
                                    DEPESECU = x.CVTDSECU,
                                };
                                ListArticulos.Add(objart);
                            }
                        });
                        resultadoOpe = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron coincidencias", "Sin Coincidencias", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
        #endregion
    }
}