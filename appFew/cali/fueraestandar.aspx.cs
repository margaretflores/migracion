using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using System.Drawing;
using System.Web.Script.Serialization;

using Microsoft.Reporting.WebForms;
using System.Web.Services;

using appFew.appServicio;
using appWcfService;
using appConstantes;

namespace appFew.cali
{
    public partial class fueraestandar : System.Web.UI.Page
    {
        private static ParametrosFe _ParametrosIni;

        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;

        private string url = string.Empty;

        private bool Cargando = false;

        //Hashtable HasDatosPC;
        private static int PageSize = 12;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            _ParametrosIni = (ParametrosFe)Session["ParametrosFe"];

            if (Session["ParametrosFe"] == null)
            {
                Response.Redirect("../login.aspx?ReturnURL=" + Request.Url.AbsoluteUri);
                //Response.Redirect("../login.aspx");
            }
            else
            {

                try
                {
                    //ErrorLabel.Font.Bold = false;
                    AsignaAtributos();
                    
                    if (!Page.IsPostBack)
                    {
                        CargaDatosIniciales();
                    }
                }
                catch (Exception ex)
                {
                    Error_2 = ex.Message.Replace(Environment.NewLine, "<BR>");
                    Error_1 = "Ha ocurrido un error en la pagina.";
                    url = "..//ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + Error_2;
                    Response.Redirect(url);
                }
            }
        }

        private List<PCATFE> AutorizaFueraEstandar
        {
            get
            {
                List<PCATFE> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_AUTORIZA_FUERA_ESTANDAR] as List<PCATFE>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    AutorizaFueraEstandar = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_AUTORIZA_FUERA_ESTANDAR] = value;
            }
        }

        private void AsignaAtributos()
        {
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            aceptarBusqButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_LIBERAR_VARIABLE + "')){ return false; };";
            rechazarlibvarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_RECHAZAR_LIBERAR_VARIABLE + "')){ return false; };";

            //codmaqeditTextBox.Attributes.Add("onKeyPress", "doClick('" + buscaMaquinaButton.ClientID + "',event)");
            //desmaqeditTextBox.Attributes.Add("readonly", "readonly");
        }

        private List<PCATFE> ObtieneCabeceraDefault()
        {
            List<PCATFE> datos = new List<PCATFE>();
            datos.Add(new PCATFE() { ATFEIDED  = -1 });
            return datos;
        }

        protected void linprofilDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaTiposProcesoLineaFiltro();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        public void CargaTiposProcesoLineaFiltro()
        {

            IappServiceClient clt = null;
            try
            {
                var firstitem = tipprofilDropDownList.Items[0];
                tipprofilDropDownList.Items.Clear();
                tipprofilDropDownList.Items.Add(firstitem);

                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(linprofilDropDownList.SelectedValue);
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_HABILITADOS));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PRTTPR> datos = FuncionesUtil.Deserialize<List<PRTTPR>>(resultado.VALSAL[1]);
                        foreach (PRTTPR item in datos)
                        {
                            tipprofilDropDownList.Items.Add(new ListItem(item.TTPRDESC, item.TTPRCODI.ToString()));
                        }
                    }

                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }


        private void CargaDatosIniciales()
        {
            //CargaTiposAux(Constantes.TABLA_TIPO_ESTANDAR_VALOR);
            CargaLiberarVariables(1);
            CargaLineasProduccion();
            CargaVariables();
        }

        //public void CargaTiposAux(string tabla)
        //{

        //    IappServiceClient clt = null;
        //    try
        //    {
        //        RESOPE resultado;
        //        clt = _ParametrosIni.IniciaNuevoCliente();

        //        //codigo de operacion
        //        PAROPE argumentos = new PAROPE();
        //        argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_AUXILIARES;
        //        //asigna parametros entrada en orden
        //        List<string> parEnt = new List<string>();
        //        parEnt.Add(tabla);
        //        argumentos.VALENT = parEnt.ToArray();
        //        resultado = clt.EjecutaOperacion(argumentos);
        //        if (resultado.ESTOPE)
        //        {
        //            if (resultado.VALSAL[0].Equals("1")) //encontrado
        //            {
        //                if (tabla.Equals(Constantes.TABLA_TIPO_ESTANDAR_VALOR))
        //                {
        //                    var firstitem = tipvaleditDropDownList.Items[0];
        //                    tipvaleditDropDownList.Items.Clear();
        //                    tipvaleditDropDownList.Items.Add(firstitem);
        //                }

        //                List<PCTAUX> datos = FuncionesUtil.Deserialize<List<PCTAUX>>(resultado.VALSAL[1]);
        //                foreach (PCTAUX item in datos)
        //                {
        //                    if (tabla.Equals(Constantes.TABLA_TIPO_ESTANDAR_VALOR))
        //                    {
        //                        tipvaleditDropDownList.Items.Add(new ListItem(item.TAUXDESC, item.TAUXCODI));
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MostrarMensaje(resultado.MENERR);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
        //    }
        //    finally
        //    {
        //        _ParametrosIni.FinalizaCliente(clt);
        //    }
        //}

        private void CargaLiberarVariables(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LIBERAR_VARIABLES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(txtSearch.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());

                parEnt.Add(linprofilDropDownList.SelectedValue);
                parEnt.Add(tipprofilDropDownList.SelectedValue);
                parEnt.Add(varnomfilDropDownList.SelectedValue);
                parEnt.Add(estautDropDownList.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCATFE> datos = FuncionesUtil.Deserialize<List<PCATFE>>(resultado.VALSAL[1]);

                        SetDataSource(datos);
                        totalHiddenField.Value = resultado.VALSAL[2];
                    }
                    else
                    {
                        SetDataSource(null);
                        totalHiddenField.Value = "0";
                    }
                    pageIndexHiddenField.Value =  pageindex.ToString();
                    totalLabel.Text = " Total: " + totalHiddenField.Value;
                    paginaLabel.Text = "Pág: " + pageIndexHiddenField.Value;

                }
                else
                {
                    SetDataSource(null);
                    totalHiddenField.Value = "0";

                    MostrarMensaje(resultado.MENERR);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                }
                EstadoFinalBotonesNavegacion();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        private void SetDataSource(List<PCATFE> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            AutorizaFueraEstandar = _listaMaquinas;
            cabeceraGridView.DataSource = AutorizaFueraEstandar;
            cabeceraGridView.DataBind();
        }

        public void RedirectPage(string url)
        {
            string script = "<script language=\"JavaScript\">";
            script += "window.location.href=\"";
            script += url + "\";</";
            script += "script>";

            if (!Page.ClientScript.IsStartupScriptRegistered("redirectPage"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "redirectPage", script);
            }
        }

        protected void cabeceraGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        private void LimpiaDatos()
        {
        }

        protected void cabeceraGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                if (cabeceraGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = cabeceraGridView.Rows[cabeceraGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = cabeceraGridView.Rows[e.NewSelectedIndex];

                //brow.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                nuevorow.Attributes.Add("bgColor", "this.originalstyle");
                nuevorow.ToolTip = string.Empty;

            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void cabeceraGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               MuestraDatos();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void MuestraDatos()
        {
            GridViewRow row = cabeceraGridView.SelectedRow;
            modificarButton.Enabled = false;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<PCATFE> detMaquina = AutorizaFueraEstandar;

                if (detMaquina != null && detMaquina[0].ATFEIDED != -1)
                {
                    PCATFE maq = detMaquina.Find(x => x.ATFEIDED == Convert.ToDecimal(corrLiq));

                    descripcion3Label.Text = maq.MVARNOMB.Trim();
                    //localizacionLabel.Text = maq.TCTEDESC;
                    iddeHiddenField.Value = maq.ATFEIDED.ToString();
                    observacionTextBox.Text = maq.ATFEOBSE.Trim();

                    modificarButton.Enabled = true;
                }
            }
            else
            {
                iddeHiddenField.Value = "";
                descripcion3Label.Text = "";
                localizacionLabel.Text = "";

            }
        }

        protected void cabeceraGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.Cells[0].Text.Equals("-1"))
                {
                    switch (e.Row.Cells[12].Text)
                    {
                        case Constantes.ESTADO_VARIABLE_LIBERADA:
                            e.Row.Cells[12].Text = Mensajes.TEXTO_VARIABLE_LIBERADA;
                            break;
                        case Constantes.ESTADO_VARIABLE_PENDIENTE_LIBERAR:
                            e.Row.Cells[12].Text = Mensajes.TEXTO_VARIABLE_PENDIENTE_LIBERAR;
                            break;
                        default:
                            e.Row.Cells[12].Text = Mensajes.TEXTO_VARIABLE_NO_LIBERADA;
                            break;
                    }
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";

                }
                else
                {
                    e.Row.Cells[7].Text = "";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Text = "";
                    e.Row.Cells[10].Text = "";
                }
            }
        }

        private bool ValidaDatos(out string mensajeValidacion)
        {
            decimal valor, valor2;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";
            valor2 = 0;

            //if (string.IsNullOrWhiteSpace(valor1editTextBox.Text) || !decimal.TryParse(valor1editTextBox.Text, out valor))
            //{
            //    mensajeValidacion = string.Format(Mensajes.DATOS_INCORRECTOS, "VALOR1");
            //    return false;
            //}

            //if (tipvaleditDropDownList.SelectedValue.Equals(Constantes.COD_TIPO_ESTANDAR_RANGO_VALORES) || tipvaleditDropDownList.SelectedValue.Equals(Constantes.COD_TIPO_ESTANDAR_VALOR_FIJO_PORCENTAJE))
            //{
            //    if (string.IsNullOrWhiteSpace(valor2editTextBox.Text) || !decimal.TryParse(valor1editTextBox.Text, out valor2))
            //    {
            //        mensajeValidacion = string.Format(Mensajes.DATOS_INCORRECTOS, "VALOR2");
            //        return false;
            //    }
            //}

            //if (tipvaleditDropDownList.SelectedValue.Equals(Constantes.COD_TIPO_ESTANDAR_RANGO_VALORES))
            //{
            //    if (valor >= valor2)
            //    {
            //        mensajeValidacion = Mensajes.MENSAJE_VALOR1_DEBE_SER_MENOR;
            //        return false;
            //    }
            //}
            //else if (tipvaleditDropDownList.SelectedValue.Equals(Constantes.COD_TIPO_ESTANDAR_MENOR_IGUAL_VALOR_FIJO))
            //{
            //    if (valor <= 0)
            //    {
            //        mensajeValidacion = Mensajes.MENSAJE_VALOR1_MAYOR_CERO;
            //        return false;
            //    }
            //}
            //else if (tipvaleditDropDownList.SelectedValue.Equals(Constantes.COD_TIPO_ESTANDAR_VALOR_FIJO_PORCENTAJE))
            //{
            //    if (valor <= 0)
            //    {
            //        mensajeValidacion = Mensajes.MENSAJE_VALOR1_MAYOR_CERO;
            //        return false;
            //    }
            //    if (valor2 > 100)
            //    {
            //        mensajeValidacion = Mensajes.MENSAJE_VALOR2_MENOR_CIEN;
            //        return false;
            //    }
            //}
            //else if (tipvaleditDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
            //{
            //    mensajeValidacion = Mensajes.MENSAJE_SELECCIONE_TIPO_ESTANDAR;
            //    return false;
            //}
            return resultado;
        }

        protected void Cancel2_Click(object sender, EventArgs e)
        {
            End_Block(panelEdit); //udpInnerUpdatePanel);
        }

        void End_Block(Panel p, bool cerrar = true)
        {
            foreach (dynamic txtBox in p.Controls)
            {

                if (txtBox is TextBox)
                    txtBox.Text = String.Empty;

            }
            if (cerrar)
            {
                mpe2.Hide();
            }

        }

        void End_Block(UpdatePanel p, bool cerrar = true)
        {
            foreach (dynamic contenedor in p.Controls)
            {

                if (contenedor is Control)
                {
                    foreach (dynamic txtBox in contenedor.Controls)
                    {

                        if (txtBox is TextBox)
                            txtBox.Text = String.Empty;

                    }
                }
            }
            if (cerrar)
            {
                mpe2.Hide();
            }

        }

        private void MostrarMensaje(string mensaje, bool noalert = false)
        {
            if (!noalert)
            {
                ScriptManager.RegisterStartupScript(up, up.GetType(), "myAlert", "alert('" + mensaje.Replace("<br>", " - " ) + "');", true);
            }
            //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(mensaje );
        }

        protected void cabeceraGridView_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                cabeceraGridView.PageIndex = e.NewPageIndex;
                pageIndexHiddenField.Value = e.NewPageIndex.ToString();
                SetDataSource(AutorizaFueraEstandar);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaLiberarVariables(1);
        }

        protected void primeroButton_Click(object sender, EventArgs e)
        {
            navega(0);
        }

        protected void anteriorButton_Click(object sender, EventArgs e)
        {
            navega(-1);
        }

        protected void siguienteButton_Click(object sender, EventArgs e)
        {
            navega(1);
        }

        protected void ultimoButton_Click(object sender, EventArgs e)
        {
            navega(99);
        }

        private void EstadoFinalBotonesNavegacion()
        {
            int ultimo, total, indicepag;
            total = Convert.ToInt32(totalHiddenField.Value);
            indicepag = Convert.ToInt32(pageIndexHiddenField.Value);
            ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

            anteriorButton.Enabled = false;
            primeroButton.Enabled = false;
            siguienteButton.Enabled = false;
            ultimoButton.Enabled = false;
            if (total > 0)
            {
                if (indicepag > 1)
                {
                    anteriorButton.Enabled = true;
                    primeroButton.Enabled = true;
                }
                if (indicepag < ultimo)
                {
                    siguienteButton.Enabled = true;
                    ultimoButton.Enabled = true;
                }
            }
        }

        private void navega(int index)
        {
            try
            {
                int ultimo, total, indicepag;
                total = Convert.ToInt32(totalHiddenField.Value);
                indicepag = Convert.ToInt32(pageIndexHiddenField.Value);
                ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

                if (ultimo > 0)
                {
                    anteriorButton.Enabled = true;
                    primeroButton.Enabled = true;
                    siguienteButton.Enabled = true;
                    ultimoButton.Enabled = true;
                }
                else
                {
                    anteriorButton.Enabled = false;
                    primeroButton.Enabled = false;
                    siguienteButton.Enabled = false;
                    ultimoButton.Enabled = false;
                }

                switch (index)
                {
                    case -1: //anterior
                        if (indicepag > 1)
                        {
                            indicepag--;
                        }
                        if (indicepag == 1)
                        {
                            anteriorButton.Enabled = false; 
                            primeroButton.Enabled = false;
                        }
                        break;
                    case 1: //siguiente
                        if (indicepag < ultimo)
                        {
                            indicepag++;
                        }
                        if (indicepag == ultimo)
                        {
                            siguienteButton.Enabled = false;
                            ultimoButton.Enabled = false;
                        }
                        break;
                    case 0: //primero
                        indicepag = 1;
                        anteriorButton.Enabled = false; 
                        primeroButton.Enabled = false;

                        break;
                    case 99: //ultimo
                        if (total > 0)
                        {
                            indicepag = ultimo;
                            siguienteButton.Enabled = false;
                            ultimoButton.Enabled = false;
                        }
                        break;
                }
                if (indicepag != Convert.ToInt32(pageIndexHiddenField.Value))
                {
                    CargaLiberarVariables(indicepag);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void habilitaNavegacion()
        {

        }

        protected void modificarButton_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = cabeceraGridView.SelectedRow;
                if (row != null)
                {
                    string corrLiq = row.Cells[0].Text;
                    //LENAR TABPAGES
                    List<PCATFE> detMaquina = AutorizaFueraEstandar;

                    if (detMaquina != null && detMaquina[0].ATFEIDED != -1)
                    {
                        PCATFE maq = detMaquina.Find(x => x.ATFEIDED == Convert.ToDecimal(corrLiq));
                        iddeHiddenField.Value = maq.ATFEIDED.ToString();

                        lineditLabel.Text = maq.LIPRDESC;
                        proeditLabel.Text = maq.TTPRDESC;
                        maqeditLabel.Text = maq.MAQUDES2;
                        pareditLabel.Text = maq.ENSAPART;
                        vareditLabel.Text = maq.MVARNOMB;
                        osoeditTextBox.Text = maq.ATFEOBSE;
                        obseditTextBox.Text = maq.ATFEOBSA;

                        reseditLabel.Text = maq.ENVAVALO.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        es1editLabel.Text = maq.ENVAVAE1.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        es2editLabel.Text = maq.ENVAVAE2.ToString(Constantes.FORMATO_DECIMAL_0 + "1");

                        tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                        if (maq.ATFEESTA.Equals(Constantes.ESTADO_VARIABLE_LIBERADA) || maq.ATFEESTA.Equals(Constantes.ESTADO_VARIABLE_NO_LIBERADA_RECHAZADA))
                        {
                            obseditTextBox.ReadOnly = true;
                            aceptarBusqButton.Enabled = false;
                            rechazarlibvarButton.Enabled = false;
                            if (maq.ATFEESTA.Equals(Constantes.ESTADO_VARIABLE_LIBERADA))
                            {
                                esteditLabel.Text = Mensajes.TEXTO_VARIABLE_LIBERADA;
                            }
                            else
                            {
                                tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                                esteditLabel.Text = Mensajes.TEXTO_VARIABLE_NO_LIBERADA;
                            }
                        }
                        else
                        {
                            tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                            obseditTextBox.ReadOnly = false;
                            aceptarBusqButton.Enabled = true;
                            rechazarlibvarButton.Enabled = true;
                                esteditLabel.Text = Mensajes.TEXTO_VARIABLE_PENDIENTE_LIBERAR;
                        }
                        mpe2.Show();
                    }
                    else
                    {
                        modificarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_VARIABLE_MODIFICAR);
                    modificarButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void liberarvarButton_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                up.Update();
                mpe2.Show();
                return;
            }
            try
            {
                mpe2.Show();
                if (GuardaLiberarVariable(false))
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaLiberarVariables(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        protected void rechazarlibvarButton_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                up.Update();
                mpe2.Show();
                return;
            }
            try
            {
                mpe2.Show();
                if (GuardaLiberarVariable(true))
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaLiberarVariables(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaLiberarVariable(bool rechazaliberacion)
        {
            IappServiceClient clt = null;
            bool resguardar = false;
            string mensajeValidacion;

            try
            {
                if (!ValidaDatos(out mensajeValidacion))
                {
                    MostrarMensaje(mensajeValidacion);
                    return false;
                }
                RESOPE resultado;
                PCATFE maq = new PCATFE();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_LIBERAR_VARIABLE;
                //asigna parametros entrada en orden
                maq.ATFEIDED = Convert.ToDecimal(iddeHiddenField.Value);
                if (rechazaliberacion)
                {
                    maq.ATFEESTA = Constantes.ESTADO_VARIABLE_NO_LIBERADA_RECHAZADA;
                }
                else
                {
                    maq.ATFEESTA = Constantes.ESTADO_VARIABLE_LIBERADA;
                }
                maq.ATFEOBSA = obseditTextBox.Text;
                maq.ATFEUSAT = _ParametrosIni.Usuario; 

                List<string> parEnt = new List<string>();
                parEnt.Add(FuncionesUtil.Serialize(maq));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    resguardar = true;
                    if (!string.IsNullOrWhiteSpace(resultado.MENERR))
                    {
                        MostrarMensaje(resultado.MENERR);
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resguardar;
        }

        public void CargaLineasProduccion()
        {

            IappServiceClient clt = null;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LINEAS_PRODUCCION;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_HABILITADOS));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = linprofilDropDownList.Items[0];
                        linprofilDropDownList.Items.Clear();
                        linprofilDropDownList.Items.Add(firstitem);

                        List<PRLIPR> datos = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);
                        foreach (PRLIPR item in datos)
                        {
                            //linproDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                            linprofilDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                        }
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        public void CargaVariables()
        {

            IappServiceClient clt = null;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_VARIABLES_ACTIVAS;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = varnomfilDropDownList.Items[0];
                        varnomfilDropDownList.Items.Clear();
                        varnomfilDropDownList.Items.Add(firstitem);

                        List<PCMVAR> datos = FuncionesUtil.Deserialize<List<PCMVAR>>(resultado.VALSAL[1]);
                        foreach (PCMVAR item in datos)
                        {
                            varnomfilDropDownList.Items.Add(new ListItem(item.MVARNOMB, item.MVARIDVA.ToString()));
                        }
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

    }
}