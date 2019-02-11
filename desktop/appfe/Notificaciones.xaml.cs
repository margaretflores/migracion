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
    /// Lógica de interacción para Notificaciones.xaml
    /// </summary>
    public partial class Notificaciones : Window
    {
        ParametrosFe _ParametrosIni;
        public Notificaciones()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
        }

        #region Variables

        int emitido = 0, ipreparacion = 0, fpreparacion = 0, despachado = 0;

        #endregion

        #region Eventos

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaDatosIniciales();
        }
        bool valida = false;
        private void CargaDatosIniciales()
        {

            try
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += (o, e) =>
                  {
                      ObtieneParametros1();
                  };
                bk.RunWorkerCompleted += (o, e) =>
                {
                    if (valida)
                    {
                        Cargacheckbox();
                    }
                };
                bk.RunWorkerAsync();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guardarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActualizaNotificaciones(emitido.ToString(), ipreparacion.ToString(), fpreparacion.ToString(), despachado.ToString()))
                {
                    MessageBox.Show("Se ha guardado correctamente su configuración", "Correcto!", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Cargacheckbox();
                }
                else
                {
                    MessageBox.Show("No se ha podido guardar su configuración", "Falló!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cancelarbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void emisioncheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                emitido = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void emisioncheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                emitido = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void preparacioncheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ipreparacion = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void preparacioncheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ipreparacion = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void finalizacioncheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                fpreparacion = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void finalizacioncheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                fpreparacion = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void despachadocheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                despachado = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void despachadocheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                despachado = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Metodos

        public bool ObtieneParametros()
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
                argumentos.CODOPE = CodigoOperacion.OBTIENE_PARAMETROS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PEPARM> ListParametros = appWcfService.Utils.Deserialize<List<appWcfService.PEPARM>>(resultado.VALSAL[1]);
                        emitido = int.Parse(ListParametros.Find(x => x.PARMIDPA == 21).PARMVAPA);
                        ipreparacion = int.Parse(ListParametros.Find(x=> x.PARMIDPA == 22).PARMVAPA);
                        fpreparacion = int.Parse(ListParametros.Find(x=> x.PARMIDPA == 23).PARMVAPA);
                        despachado = int.Parse(ListParametros.Find(x=> x.PARMIDPA == 24).PARMVAPA);
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
        public  void ObtieneParametros1()
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
                argumentos.CODOPE = CodigoOperacion.OBTIENE_PARAMETROS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        List<appWcfService.PEPARM> ListParametros = appWcfService.Utils.Deserialize<List<appWcfService.PEPARM>>(resultado.VALSAL[1]);
                        emitido = int.Parse(ListParametros.Find(x => x.PARMIDPA == 21).PARMVAPA);
                        ipreparacion = int.Parse(ListParametros.Find(x => x.PARMIDPA == 22).PARMVAPA);
                        fpreparacion = int.Parse(ListParametros.Find(x => x.PARMIDPA == 23).PARMVAPA);
                        despachado = int.Parse(ListParametros.Find(x => x.PARMIDPA == 24).PARMVAPA);
                        valida = true;
                    }
                }
                else
                {
                    valida = false;
                }
            }
            catch (Exception ex)
            {
                valida = false;
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            
        }
        void Cargacheckbox()
        {
            if (emitido == 1)
            {
                emisioncheckbox.IsChecked = true;
            }
            else
            {
                emisioncheckbox.IsChecked = false;
            }
            if (ipreparacion == 1)
            {
                preparacioncheckbox.IsChecked = true;
            }
            else
            {
                preparacioncheckbox.IsChecked = false;
            }
            if (fpreparacion == 1)
            {
                finalizacioncheckbox.IsChecked = true;
            }
            else
            {
                finalizacioncheckbox.IsChecked = false;
            }
            if (despachado == 1)
            {
                despachadocheckbox.IsChecked = true;
            }
            else
            {
                despachadocheckbox.IsChecked = false;
            }
        }

        public bool ActualizaNotificaciones(string emitido, string ipreparacion, string fpreparacion, string despachado)
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
                argumentos.CODOPE = CodigoOperacion.ACTUALIZA_NOTIFICACIONES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(emitido);
                parEnt.Add(ipreparacion);
                parEnt.Add(fpreparacion);
                parEnt.Add(despachado);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    resultadoOpe = true;
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
