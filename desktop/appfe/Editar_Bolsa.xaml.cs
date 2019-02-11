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
    /// Lógica de interacción para Editar_Bolsa.xaml
    /// </summary>
    public partial class Editar_Bolsa : Window
    {
        ParametrosFe _ParametrosIni;
        public Editar_Bolsa()
        {
            InitializeComponent();
            _ParametrosIni = new ParametrosFe();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");

        }
        #region Variables
        public USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result objbolsa = new USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result();
        public List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result> listagrilla = new List<appWcfService.USP_OBTIENE_DETPREPARACION_POR_IDDETALLESE_Result>();
        decimal incluyestock, incluyebolsa, pcono, pbolsa;
        bool FirstTime = true, multi = false;
        decimal totcant = 0;
        int cantlista;
        bool cambios;
        public bool btnmulti;
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
                if (!btnmulti)
                {
                    multi = false;
                    cambios = false;
                    btnmulti = false;
                    partidalabel.Content = "PARTIDA: " + objbolsa.BOLSPART.ToString();
                    almacenlabel.Content = "ALMACEN: " + objbolsa.BOLSALMA.ToString();

                    if (objbolsa.BOLSCOEM != null)
                    {
                        bolsalabel.Content = "BOLSA: " + objbolsa.BOLSCOEM.ToString();
                        pcono = objbolsa.UNIDTARA.Value;
                        pbolsa = objbolsa.TIEMTARA.Value;
                    }
                    else
                    {
                        bolsalabel.Content = "BOLSA: ";
                        pcono =0;
                        pbolsa = 0.1M;
                    }


                    incluyestock = objbolsa.BODPSTCE;
                    incluyebolsa = objbolsa.BODPINBO;

                    if (incluyestock == 1)
                        stock0checkbox.IsChecked = true;
                    if (incluyebolsa == 1)
                        pesobolsacheckbox.IsChecked = true;

                    

                    decimal taracal = 0;
                    decimal brutocal = 0;

                    taracal = Math.Round(objbolsa.BODPTAUN, 3) * objbolsa.BODPCANT + (pbolsa * incluyebolsa);
                    brutocal = objbolsa.BODPPESO + taracal;


                    cantidadtextbox.Text = objbolsa.BODPCANT.ToString();
                    pesobrutotext.Text = (Math.Round(Double.Parse(objbolsa.BODPPEBR.ToString()),2)).ToString();
                    taratext.Text = (Math.Round(Double.Parse(taracal.ToString()), 3)).ToString();
                    taradespachotext.Text = objbolsa.BODPTADE.ToString();
                    pesonetorealtext.Text = Math.Round(objbolsa.BODPPERE,2).ToString();
                    //pesobrutofinaltext.Text = (Double.Parse((brutocal + objbolsa.BODPTADE).ToString())).ToString();
                    pesobrutofinaltext.Text = (Double.Parse(objbolsa.BODPPEBR.ToString())).ToString();
                    pesonetomodificadotext.Text = Math.Round(objbolsa.BODPPERE, 2).ToString();
                    taraunitext.Text = objbolsa.BODPTAUN.ToString();

                    FirstTime = false;
                }
                else
                {
                    multi = true;
                    cambios = false;
                    cantlista = listagrilla.Count - 1;

                    bolsalabel.Content = "BOLSAS ";
                    partidalabel.Content = "PARTIDA: " + listagrilla[0].BOLSPART.ToString();
                    almacenlabel.Content = "ALMACEN: " + listagrilla[0].BOLSALMA.ToString();
                    pesobolsacheckbox.IsChecked = true;
                    incluyebolsa = 1;

                    // totalizando cantidades

                    try
                    {
                        decimal totpere, totpesobru, tottar, totpesonet, tade, tottade, cantAtend, pesoAtend;
                        totcant = 0;
                        totpesobru = 0;
                        tottar = 0;
                        totpesonet = 0;
                        tottade = 0;
                        totpere = 0;

                        foreach (var item in listagrilla)
                        {
                            decimal taracal, pesobruto;//brutocal

                            //incluyebolsa = item.BODPINBO;
                            pbolsa = item.TIEMTARA.Value;
                            pcono = item.BODPTAUN;
                            cantAtend = item.BODPCANT;
                            pesoAtend = item.BODPPESO;
                            pesobruto = item.BODPPEBR - item.BODPTADE;//el peso bruto de la bolsa
                            tade = item.BODPTADE;
                            totpere = item.BODPPERE;

                            taracal = Math.Round(pcono, 3) * cantAtend + (pbolsa * incluyebolsa);

                            //brutocal = pesoAtend + taracal;//el peso modificado es diferente del peso bruto, cuando cambien el peso neto  no cmbiaria el calculo???

                            totcant += cantAtend;
                            totpesonet += pesoAtend;

                            //totpesobru += brutocal;
                            totpesobru += totpere;

                            tottar += taracal;
                            tottade += tade;
                        }


                        cantidadtextbox.Text = totcant.ToString();
                        pesobrutotext.Text = (Math.Round(Double.Parse(totpesobru.ToString()), 2)).ToString();
                        taratext.Text = (Math.Round(Double.Parse(tottar.ToString()),3)).ToString();
                        taradespachotext.Text = tottade.ToString();
                        pesonetorealtext.Text = (Math.Round(totpesonet, 2)).ToString();
                        pesobrutofinaltext.Text = (Double.Parse(totpesobru.ToString())).ToString();
                        pesonetomodificadotext.Text = (Math.Round(totpesonet, 2)).ToString();
                        taraunitext.Text = (Math.Round(Double.Parse(pcono.ToString()), 3)).ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    FirstTime = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void taradespachotext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!FirstTime)
            {
                cambios = true;
                double taradespa = 0; //taradespacho
                double tara = 0;
                double pesobruto = 0;
                double pesoneto = 0;

                try
                {
                    taradespa = Double.Parse(taradespachotext.Text.ToString());
                }
                catch (Exception)
                {
                    taradespa = 0;
                }
                try
                {
                    tara = Double.Parse(taratext.Text.ToString());
                }
                catch (Exception)
                {
                    tara = 0;
                }
                try
                {
                    pesobruto = Double.Parse(pesobrutotext.Text.ToString());
                }
                catch (Exception)
                {
                    pesobruto = 0;
                }

                //pesobruto = pesobruto + taradespa;
                //pesobrutofinaltext.Text = pesobruto.ToString();
                pesoneto = pesobruto - tara - taradespa;
                pesonetorealtext.Text = Math.Round(pesoneto, 2).ToString();
                pesonetomodificadotext.Text = pesonetorealtext.Text.ToString();

            }


        }

        private void pesobolsacheckbox_Click(object sender, RoutedEventArgs e)
        {
            cambios = true;
            CheckBox cb = sender as CheckBox;
            //MessageBox.Show("pesobolsacheckbox_Click    " + cb.IsChecked.ToString(), "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
            if (cb.IsChecked.Value)
            {
                incluyebolsa = 1;
            }
            else
            {
                incluyebolsa = 0;
            }
            decimal tara = 0;
            double taradespa = 0;
            double pesoneto;
            decimal value = 0; //cantidad
            double pesobruto = 0;
            try
            {
                value = Decimal.Parse(cantidadtextbox.Text.ToString());
            }
            catch (Exception)
            {
                value = 0;
            }
            try
            {
                pesobruto = Double.Parse(pesobrutotext.Text.ToString());
            }
            catch (Exception)
            {
                pesobruto = 0;
            }
            try
            {
                taradespa = Double.Parse(taradespachotext.Text.ToString());
            }
            catch (Exception)
            {
                taradespa = 0;
            }

            if (multi)
            {
                tara = Math.Round(pcono, 3) * value + (pbolsa * incluyebolsa * (cantlista + 1));
                pesoneto = pesobruto - Double.Parse(tara.ToString()) - taradespa;

                taratext.Text = Math.Round(Double.Parse(tara.ToString()),3).ToString();
            }
            else
            {
                tara = Math.Round(pcono,3) * value + (pbolsa * incluyebolsa);
                pesoneto = pesobruto - Double.Parse(tara.ToString()) - taradespa;

                taratext.Text = Math.Round(Double.Parse(tara.ToString()), 3).ToString();
            }

            pesonetorealtext.Text = Math.Round(pesoneto, 2).ToString();
            pesonetomodificadotext.Text = pesonetorealtext.Text.ToString();

        }

        private void stock0checkbox_Click(object sender, RoutedEventArgs e)
        {
            cambios = true;
            CheckBox cb = sender as CheckBox;
            decimal cantidad = 0; //cantidad
            //MessageBox.Show("pesobolsacheckbox_Click    " + cb.IsChecked.ToString(), "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
            try
            {
                cantidad = Decimal.Parse(cantidadtextbox.Text.ToString());
            }
            catch (Exception)
            {
                cantidad = 0;
            }
            if (cb.IsChecked.Value)
            {
                //incluyestock = 1;
                if (multi)
                {
                    if (totcant - cantidad <= 0)
                    {
                        incluyestock = 1;
                    }
                    else
                    {
                        if (MessageBox.Show("Aún hay existencias en la última bolsa, ¿Desea activar la casilla de  Stock 0? ", "Stock 0", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            incluyestock = 1;
                        }
                        else
                        {
                            cb.IsChecked = false;
                        }
                    }
                }
                else
                {
                    if (objbolsa.BOLSCANT == null || (objbolsa.BOLSCANT - cantidad <= 0))
                    {
                        incluyestock = 1;
                    }
                    else
                    {
                        if (MessageBox.Show("Aún hay existencias en la bolsa, ¿Desea activar la casilla de  Stock 0?. ", "Stock 0", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            incluyestock = 1;
                        }
                        else
                        {
                            cb.IsChecked = false;
                        }
                    }
                }
            }
            else
            {
                incluyestock = 0;
            }


        }

        private void aceptarbutton_Click(object sender, RoutedEventArgs e)
        {
            if (!btnmulti)
                editapeso();
            else
                editapesomultiple();//revisar

        }

        private void pesobrutotext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!FirstTime)
            {
                cambios = true;
                // MessageBox.Show("text changed" + pesobrutotext.Text.ToString(), "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
                double tara = 0;
                double value, taradesp;
                double pesoneto;
                try
                {
                    value = Double.Parse(pesobrutotext.Text.ToString());
                }
                catch (Exception)
                {
                    value = 0;
                }
                try
                {
                    taradesp = Double.Parse(taradespachotext.Text.ToString());
                }
                catch (Exception)
                {
                    taradesp = 0;
                }

                tara = Double.Parse(taratext.Text.ToString());
                pesoneto = value - tara - taradesp;

                pesonetorealtext.Text = (Math.Round(pesoneto, 2)).ToString();
                pesonetomodificadotext.Text = pesonetorealtext.Text.ToString();
                pesobrutofinaltext.Text = value.ToString();
            }

        }

        private void cantidadtextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!FirstTime)
            {
                cambios = true;
                //MessageBox.Show("text changed" + cantidadtextbox.Text.ToString(), "Código de empaque duplicado", MessageBoxButton.OK, MessageBoxImage.Information);
                decimal tara = 0;
                double pesoneto;
                decimal value = 0; //cantidad
                double pesobruto = 0;
                double taradesp = 0;
                try
                {
                    value = Decimal.Parse(cantidadtextbox.Text.ToString());
                }
                catch (Exception)
                {
                    value = 0;
                }
                try
                {
                    pesobruto = Double.Parse(pesobrutotext.Text.ToString());
                }
                catch (Exception)
                {
                    pesobruto = 0;
                }
                try
                {
                    taradesp = Double.Parse(taradespachotext.Text.ToString());
                }
                catch (Exception)
                {
                    taradesp = 0;
                }

                if (multi)
                {
                    tara = Math.Round(pcono, 3) * value + (pbolsa * incluyebolsa * (cantlista + 1));
                    pesoneto = pesobruto - Double.Parse(tara.ToString()) - taradesp;

                }
                else
                {
                    tara = Math.Round(pcono, 3) * value + (pbolsa * incluyebolsa);
                    pesoneto = pesobruto - Double.Parse(tara.ToString()) - taradesp;

                }


                taratext.Text = Math.Round(Double.Parse(tara.ToString()),3).ToString();
                pesonetorealtext.Text = (Math.Round(pesoneto, 2)).ToString();
                pesonetomodificadotext.Text = pesonetorealtext.Text.ToString();
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
                    //hacer que pase a sgueinte formulario?
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void pesobrutotext_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void taradespachotext_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void pesonetomodificadotext_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void cantidadtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            noletrasnopunto(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cambios)
            {
                if (MessageBox.Show("¿Está seguro que desea salir? Se perderán todos los cambios ", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                    e.Cancel = true;
            }
        }

        private void taratext_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (checkmodificable.IsChecked.Value)
            //{
            double tara = 0;
            double pesoneto;
            double pesobruto = 0;
            double taradesp = 0;

            try
            {
                pesobruto = Double.Parse(pesobrutotext.Text.ToString());
            }
            catch (Exception)
            {
                pesobruto = 0;
            }
            try
            {
                tara = Double.Parse(taratext.Text.ToString());
            }
            catch (Exception)
            {
                tara = 0;
            }
            try
            {
                taradesp = Double.Parse(taradespachotext.Text.ToString());
            }
            catch (Exception)
            {
                taradesp = 0;
            }
            pesoneto = Math.Round(pesobruto - tara - taradesp, 2);
            pesonetorealtext.Text = (pesoneto).ToString();
            pesonetomodificadotext.Text = pesonetorealtext.Text.ToString();
            //}
        }

        private void taraunitext_KeyDown(object sender, KeyEventArgs e)
        {
            noletras(e);
        }

        private void taraunitext_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal tara = 0;
            decimal tara_cono = 0;
            decimal cantidad = 0;

            try
            {
                tara_cono = Decimal.Parse(taraunitext.Text.ToString());
            }
            catch (Exception)
            {
                tara_cono = 0;
            }
            try
            {
                cantidad = Decimal.Parse(cantidadtextbox.Text.ToString());
            }
            catch (Exception)
            {
                cantidad = 0;
            }

            if (multi)
            {
                tara = Math.Round(tara_cono, 3) * cantidad + (pbolsa * incluyebolsa * (cantlista + 1));
                taratext.Text = Math.Round(Double.Parse(tara.ToString()),3).ToString();
            }
            else
            {
                tara = Math.Round(tara_cono, 3) * cantidad + (pbolsa * incluyebolsa);
                taratext.Text = Math.Round(Double.Parse(tara.ToString()),3).ToString();
            }
            pcono = tara_cono;

        }

        private void cancelarbutton_Click(object sender, RoutedEventArgs e)
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

        #endregion

        #region Metodos
        public void editapeso()
        {
            decimal cantidad, pesomodif, pesoreal, pesobolsa, taradesp, pesobruto, taracono;
            try
            {
                cantidad = Decimal.Parse(cantidadtextbox.Text.ToString());
                pesomodif = Decimal.Parse(pesonetomodificadotext.Text.Equals("0") ? "" : pesonetomodificadotext.Text.ToString());
                pesoreal = Decimal.Parse(pesonetorealtext.Text.ToString());
                pesobolsa = objbolsa.BOLSPESO.Value;
                taradesp = Decimal.Parse(taradespachotext.Text.Equals("") ? "0" : taradespachotext.Text.ToString());
                pesobruto = Decimal.Parse(pesobrutotext.Text.ToString());
                taracono = Decimal.Parse(taraunitext.Text.Equals("") ? objbolsa.UNIDTARA.ToString() : taraunitext.Text.ToString());


                objbolsa.BODPCANT = Math.Round(cantidad, 2);
                objbolsa.BODPPESO = Math.Round(pesomodif, 2);
                objbolsa.BODPPERE = Math.Round(pesoreal, 2);
                objbolsa.BODPSTCE = Math.Round(Decimal.Parse(incluyestock.ToString()), 2);
                objbolsa.BODPINBO = Math.Round(Decimal.Parse(incluyebolsa.ToString()), 2);
                objbolsa.BODPDIFE = Math.Round(pesobolsa - pesomodif, 2);//PESO DE LA BOLSA - PESO NETO MOODIFICADO
                objbolsa.BODPPEBR = Math.Round(pesobruto, 2);//(bruto + taraempaque);
                objbolsa.BODPTADE = Math.Round(taradesp, 2);// (taraempaque);
                objbolsa.BODPTAUN = Math.Round(taracono, 3);

                //MessageBox.Show("Se han guardado", "Aceptar", MessageBoxButton.OK, MessageBoxImage.Information);
                cambios = false;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Las cantidades ingresadas no son correctas", "Cantidades Incorrectas", MessageBoxButton.OK, MessageBoxImage.Information);
                //throw;
            }
        }

        public void editapesomultiple()
         {
            try
            {
                ////
                //bool check0 = false;
                bool checkbols = false;
                //if (stock0checkbox.IsChecked == true) check0 = true;
                if (pesobolsacheckbox.IsChecked == true) checkbols = true;
                int count = 0;
                ////

                decimal fincant, finbrutofin, finreal, finpemodif, taraempaque, taracono;

                fincant = Decimal.Parse(cantidadtextbox.Text.ToString());
                finbrutofin = Decimal.Parse(pesobrutofinaltext.Text.ToString());
                finreal = Decimal.Parse(pesonetorealtext.Text.ToString());
                finpemodif = Decimal.Parse(pesonetomodificadotext.Text.ToString());
                taraempaque = Decimal.Parse(taradespachotext.Text.ToString());
                taracono = Decimal.Parse(taraunitext.Text.ToString());

                ////

                if (totcant >= Decimal.Parse(cantidadtextbox.Text.ToString()))
                {
                    foreach (var item in listagrilla)
                    {
                        //count++;
                        //if (count != cantlista)
                        //{
                        pcono = item.BODPTAUN;
                        pbolsa = item.TIEMTARA.Value;

                        decimal taracal, brutocal;
                        taracal = Math.Round(pcono, 3) * item.BODPCANT + (pbolsa * incluyebolsa); //taracal = round(taracal, 3);¿?

                        brutocal = item.BODPPESO + taracal; //brutocal = round(brutocal, 2);¿?

                        if (checkbols) item.BODPINBO = 1;
                        else item.BODPINBO = 0;

                        if (fincant > 0)
                        {
                            if (fincant <= item.BODPCANT)
                            {
                                item.BODPCANT = Math.Round(fincant, 2);
                            }
                            fincant = fincant - item.BODPCANT;
                        }
                        else
                        {
                            item.BODPCANT = fincant;
                        }

                        if (finreal > 0)
                        {
                            if (finreal <= item.BODPPERE)
                            {
                                item.BODPPERE = Math.Round(brutocal, 2);
                            }
                            finreal = finreal - brutocal;
                        }
                        else
                        {
                            item.BODPPERE = finreal;
                        }

                        if (finbrutofin > 0)
                        {
                            if (finbrutofin <= item.BODPPEBR)
                            {
                                item.BODPPEBR = Math.Round(finbrutofin, 2);
                            }
                            finbrutofin = finbrutofin - item.BODPPEBR;
                        }
                        else
                        {
                            item.BODPPEBR = finbrutofin;
                        }

                        if (finpemodif > 0)
                        {
                            if (finpemodif <= item.BODPPESO)
                            {
                                item.BODPPESO = Math.Round(finpemodif, 2);
                            }
                            finpemodif = finpemodif - item.BODPPESO;
                        }
                        else
                        {
                            item.BODPPESO = Math.Round(finpemodif, 2);
                            if (finpemodif == 0) count = cantlista;
                        }
                        //}
                        //else
                        //{
                        //}
                    }
                    //SOLO LA ULTIMA
                    listagrilla[cantlista].BODPTADE = Math.Round(taraempaque, 2);

                    listagrilla[cantlista].BODPTAUN = Math.Round(taracono, 2);

                    //listagrilla[cantlista].BODPPEBR = finbrutofin;
                    //listagrilla[cantlista].BODPPESO = finpemodif;
                    //listagrilla[cantlista].BODPCANT = fincant;
                    //listagrilla[cantlista].BODPPERE = finreal;
                    listagrilla[cantlista].BODPDIFE = Math.Round(listagrilla[cantlista].BOLSPESO.Value - listagrilla[cantlista].BODPPESO, 2);
                    if (stock0checkbox.IsChecked == true) listagrilla[cantlista].BODPSTCE = 1;
                    else listagrilla[cantlista].BODPSTCE = 0;
                }
                else
                    MessageBox.Show("Las cantidades ingresadas son incorrectas", "Cantidades Incorrectas", MessageBoxButton.OK, MessageBoxImage.Information);
                ////
                //
                //MessageBox.Show("se ha guardado", "Aceptar", MessageBoxButton.OK, MessageBoxImage.Information);
                cambios = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Las cantidades ingresadas son incorrectas" + ex.ToString(), "Cantidades Incorrectas", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        public void noletras(KeyEventArgs e)
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
        public void noletrasnopunto(KeyEventArgs e)
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

        #endregion
    }
}
