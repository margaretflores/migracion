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
using System.Windows.Navigation;
using System.Windows.Shapes;

using   appfe.appServicio;
using appConstantes;


namespace appfe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ParametrosFe _ParametrosIni;

        public MainWindow()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
        }

        private void sumarbutton_Click(object sender, RoutedEventArgs e)
        {
            Sumar();
        }

        private bool Sumar()
        {
            bool resultadoOpe;
            IappServiceClient clt = null;
            resultadoOpe = false;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = "PED001"; // CodigoOperacion.SUMAR_DOS_VALORES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(valor1textBox.Text);
                parEnt.Add(valor2textBox.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //if (resultado.VALSAL[0].Equals("1"))
                    //{
                    resultadotextBox.Text = resultado.VALSAL[0];
                    resultadoOpe = true;
                    //}
                    //else
                    //{
                    //    MostrarMensaje(Mensajes.MENSAJE_TIPO_NO_ENCONTRADO);
                    //}
                }
                else
                {
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    MessageBox.Show (resultado.MENERR);
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

        //private bool Multiplicar()
        //{
        //    bool resultadoOpe;
        //    IappServiceClient clt = null;
        //    resultadoOpe = false;
        //    try
        //    {
        //        RESOPE resultado; //Resultado de Operacion
        //        clt = _ParametrosIni.IniciaNuevoCliente();
        //        //codigo de operacion
        //        PAROPE argumentos = new PAROPE();
        //        argumentos.CODOPE = CodigoOperacion.MULTIPLICAR_DOS_VALORES;
        //        //asigna parametros entrada en orden
        //        List<string> parEnt = new List<string>();
        //        parEnt.Add(textBox1.Text);
        //        parEnt.Add(textBox2.Text);

        //        argumentos.VALENT = parEnt.ToArray();
        //        resultado = clt.EjecutaOperacion(argumentos);

        //        if (resultado.ESTOPE)
        //        {
        //            //if (resultado.VALSAL[0].Equals("1"))
        //            //{
        //            textBox3.Text = resultado.VALSAL[0];
        //            resultadoOpe = true;
        //            //}
        //            //else
        //            //{
        //            //    MostrarMensaje(Mensajes.MENSAJE_TIPO_NO_ENCONTRADO);
        //            //}
        //        }
        //        else
        //        {
        //            //ErrorLabel.Font.Bold = true;
        //            //ErrorLabel.Text = resultado.MENERR;
        //            MessageBox.Show(resultado.MENERR);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
        //        MessageBox.Show(_ParametrosIni.ErrorGenerico(ex.Message));
        //    }
        //    finally
        //    {
        //        _ParametrosIni.FinalizaCliente(clt);
        //    }
        //    return resultadoOpe;
        //}

        private void Multiplicarbutton_Click(object sender, RoutedEventArgs e)
        {
            //Multiplicar();
        }
    }
}
