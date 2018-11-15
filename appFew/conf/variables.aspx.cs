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

namespace appFew.conf
{
    public partial class variables : System.Web.UI.Page
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

        private List<PCMVAR> MaestroVariables
        {
            get
            {
                List<PCMVAR> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_MAE_VARIABLES] as List<PCMVAR>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    MaestroVariables = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_MAE_VARIABLES] = value;
            }
        }

        private void AsignaAtributos()
        {
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            //descargar2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_GUIA + "')){ return false; };";

            eliminarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ELIMINAR_REGISTRO + "')){ return false; };";

            //codmaqeditTextBox.Attributes.Add("onKeyPress", "doClick('" + buscaMaquinaButton.ClientID + "',event)");
            //desmaqeditTextBox.Attributes.Add("readonly", "readonly");
            agregarButton.Enabled = true;
        }

        private List<PCMVAR> ObtieneCabeceraDefault()
        {
            List<PCMVAR> datos = new List<PCMVAR>();
            datos.Add(new PCMVAR() { MVARIDVA = -1 });
            return datos;
        }

        private void CargaDatosIniciales()
        {
            CargaTiposAux(Constantes.TABLA_TIPO_UNIDAD_VARIABLE);
            CargaTiposAux(Constantes.TABLA_TIPO_VALOR_VARIABLE);
            CargaTiposAux(Constantes.TABLA_MET_CALCULO_VARIABLE);
            CargaVariables(1);
        }

        //1 combo unid med, 2 combo tipo valor, 3, combo met calculo
        public void CargaTiposAux(string tabla)
        {

            IappServiceClient clt = null;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_AUXILIARES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(tabla);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        if (tabla.Equals(Constantes.TABLA_TIPO_UNIDAD_VARIABLE))
                        {
                            var firstitem = unimededitDropDownList.Items[0];
                            unimededitDropDownList.Items.Clear();
                            unimededitDropDownList.Items.Add(firstitem);
                        }
                        else if (tabla.Equals(Constantes.TABLA_TIPO_VALOR_VARIABLE))
                        {
                            var firstitem = tipvaleditDropDownList.Items[0];
                            tipvaleditDropDownList.Items.Clear();
                            tipvaleditDropDownList.Items.Add(firstitem);
                        }
                        else if (tabla.Equals(Constantes.TABLA_MET_CALCULO_VARIABLE))
                        {
                            var firstitem = metcaleditDropDownList.Items[0];
                            metcaleditDropDownList.Items.Clear();
                            metcaleditDropDownList.Items.Add(firstitem);
                        }

                        List<PCTAUX> datos = FuncionesUtil.Deserialize<List<PCTAUX>>(resultado.VALSAL[1]);
                        foreach (PCTAUX item in datos)
                        {
                            if (tabla.Equals(Constantes.TABLA_TIPO_UNIDAD_VARIABLE))
                            {
                                unimededitDropDownList.Items.Add(new ListItem(item.TAUXDESC, item.TAUXCODI));
                            }
                            else if (tabla.Equals(Constantes.TABLA_TIPO_VALOR_VARIABLE))
                            {
                                tipvaleditDropDownList.Items.Add(new ListItem(item.TAUXDESC, item.TAUXCODI));
                            }
                            else if (tabla.Equals(Constantes.TABLA_MET_CALCULO_VARIABLE))
                            {
                                metcaleditDropDownList.Items.Add(new ListItem(item.TAUXDESC, item.TAUXCODI));
                            }
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


        private void CargaVariables(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_MAESTRO_VARIABLES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(txtSearch.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCMVAR> datos = FuncionesUtil.Deserialize<List<PCMVAR>>(resultado.VALSAL[1]);

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

        private void SetDataSource(List<PCMVAR> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            MaestroVariables = _listaMaquinas;
            cabeceraGridView.DataSource = MaestroVariables;
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
            eliminarButton.Enabled = false;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<PCMVAR> detMaquina = MaestroVariables;

                if (detMaquina != null && detMaquina[0].MVARIDVA != -1)
                {
                    PCMVAR maq = detMaquina.Find(x => x.MVARIDVA == Convert.ToDecimal(corrLiq));

                    descripcion3Label.Text = maq.MVARNOMB.Trim(); 
                    localizacionLabel.Text = maq.MVARDECO.Trim();

                    iddeHiddenField.Value = maq.MVARIDVA.ToString();
                    codigoLabel.Text = maq.MVARDECO.Trim();

                    estadoLabel.Text = maq.MVARESTA.Equals(Constantes.ESTADO_ACTIVO) ? Mensajes.TEXTO_SI : Mensajes.TEXTO_NO;
                    noreqvalLabel.Text = maq.MVARRQVA.Equals(Constantes.INDICADOR_SI) ? Mensajes.TEXTO_SI : Mensajes.TEXTO_NO;
                    modificarButton.Enabled = true;
                    eliminarButton.Enabled = true;
                }
            }
            else
            {
                iddeHiddenField.Value = "";
                codigoLabel.Text = "";
                estadoLabel.Text = "";
                noreqvalLabel.Text = "";
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
                    if (e.Row.Cells[7].Text.Equals(Constantes.INDICADOR_SI))
                    {
                        e.Row.Cells[7].Text = Mensajes.TEXTO_SI;
                    }
                    else
                    {
                        e.Row.Cells[7].Text = Mensajes.TEXTO_NO;
                    }
                    if (e.Row.Cells[9].Text.Equals(Constantes.INDICADOR_SI))
                    {
                        e.Row.Cells[9].Text = Mensajes.TEXTO_SI;
                    }
                    else
                    {
                        e.Row.Cells[9].Text = Mensajes.TEXTO_NO;
                    }

                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                }
                else
                {
                    e.Row.Cells[1].Text = "";
                }
            }
        }

        private bool ValidaDatos(out string mensajeValidacion)
        {
            decimal valor, valor2;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";

            if (string.IsNullOrWhiteSpace(descoreditTextBox.Text))
            {
                mensajeValidacion = Mensajes.MENSAJE_INGRESE_DESC_CORTA;
                return false;
            }

            if (string.IsNullOrWhiteSpace(descoreditTextBox.Text))
            {
                mensajeValidacion = Mensajes.MENSAJE_INGRESE_NOMBRE_VAR;
                return false;
            }

            if (!varccaeditCheckBox.Checked && !varcpreditCheckBox.Checked)
            {
                mensajeValidacion = Mensajes.MENSAJE_VAR_CALID_PROCESO;
                return false;
            }
            

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
                SetDataSource(MaestroVariables);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaVariables(1);
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
                    CargaVariables(indicepag);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void habilitaNavegacion()
        {

        }

        protected void agregarButton_Click(object sender, EventArgs e)
        {
            try
            {
                tituloeditLiteral.Text = Mensajes.TITULO_AGREGAR_VARIABLE;
                iddeHiddenField.Value = "";
                descoreditTextBox.Text = "";
                nomvareditTextBox.Text = "";
                unimededitDropDownList.SelectedIndex = 0;
                varccaeditCheckBox.Checked = false;
                varcpreditCheckBox.Checked = false;
                norqvaeditCheckBox.Checked = false;
                tipvaleditDropDownList.SelectedIndex = 0;
                metcaleditDropDownList.SelectedIndex = 0;
                estacteditCheckBox.Checked = true;
                mpe2.Show();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
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
                    List<PCMVAR> detMaquina = MaestroVariables;

                    if (detMaquina != null && detMaquina[0].MVARIDVA != -1)
                    {
                        PCMVAR maq = detMaquina.Find(x => x.MVARIDVA == Convert.ToDecimal(corrLiq));
                        iddeHiddenField.Value = maq.MVARIDVA.ToString();

                        descoreditTextBox.Text = maq.MVARDECO.Trim();
                        nomvareditTextBox.Text = maq.MVARNOMB.Trim();
                        if (unimededitDropDownList.Items.FindByValue(maq.MVARTCUM) != null)
                        {
                            unimededitDropDownList.SelectedValue = maq.MVARTCUM;
                        }
                        varccaeditCheckBox.Checked = maq.MVARVACC.Equals(Constantes.INDICADOR_SI);
                        varcpreditCheckBox.Checked = maq.MVARVACP.Equals(Constantes.INDICADOR_SI);
                        norqvaeditCheckBox.Checked = maq.MVARRQVA.Equals(Constantes.INDICADOR_SI);

                        tipvaleditDropDownList.SelectedIndex = 0;
                        if (tipvaleditDropDownList.Items.FindByValue(maq.MVARTCTV) != null)
                        {
                            tipvaleditDropDownList.SelectedValue = maq.MVARTCTV;
                        }

                        if (metcaleditDropDownList.Items.FindByValue(maq.MVARTCMC) != null)
                        {
                            metcaleditDropDownList.SelectedValue = maq.MVARTCMC;
                        }

                        estacteditCheckBox.Checked = maq.MVARESTA.Equals(Constantes.ESTADO_ACTIVO);

                        tituloeditLiteral.Text = Mensajes.TITULO_MODIFICAR_VARIABLE; 
 
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

        protected void aceptarBusqButton_Click(object sender, EventArgs e)
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
                if (GuardaVariable())
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaVariables(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaVariable()
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
                PCMVAR maq = new PCMVAR();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_VARIABLE;
                //asigna parametros entrada en orden
                //asigna parametros entrada en orden
                if (string.IsNullOrWhiteSpace(iddeHiddenField.Value))
                {
                    maq.MVARIDVA = -1;
                }
                else
                {
                    maq.MVARIDVA = Convert.ToDecimal(iddeHiddenField.Value);
                }

                maq.MVARDECO = descoreditTextBox.Text.Trim();
                maq.MVARNOMB = nomvareditTextBox.Text.Trim();
                maq.MVARTBUM = Constantes.TABLA_TIPO_UNIDAD_VARIABLE;
                maq.MVARTCUM = unimededitDropDownList.SelectedValue;
                maq.MVARVACC = varccaeditCheckBox.Checked ? Constantes.INDICADOR_SI : Constantes.INDICADOR_NO;
                maq.MVARVACP = varcpreditCheckBox.Checked ? Constantes.INDICADOR_SI : Constantes.INDICADOR_NO;
                maq.MVARRQVA = norqvaeditCheckBox.Checked ? Constantes.INDICADOR_SI : Constantes.INDICADOR_NO;

                maq.MVARTBTV = Constantes.TABLA_TIPO_VALOR_VARIABLE;
                maq.MVARTCTV = tipvaleditDropDownList.SelectedValue;
                maq.MVARTBMC = Constantes.TABLA_MET_CALCULO_VARIABLE;
                maq.MVARTCMC = metcaleditDropDownList.SelectedValue;

                maq.MVARESTA = estacteditCheckBox.Checked ? Constantes.ESTADO_ACTIVO : Constantes.ESTADO_ELIMINADO_NO_ACTIVO;

                maq.MVARUSMD = _ParametrosIni.Usuario; 

                List<string> parEnt = new List<string>();
                parEnt.Add(FuncionesUtil.Serialize(maq));

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    resguardar = true;
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

        protected void eliminarButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (EliminaVariable())
                {
                    CargaVariables(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool EliminaVariable()
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                GridViewRow row = cabeceraGridView.SelectedRow;
                if (row != null)
                {
                    string corrLiq = row.Cells[0].Text;
                    if (!string.IsNullOrWhiteSpace(corrLiq) && !corrLiq.Equals("-1"))
                    {
                        RESOPE resultado;
                        MTLOIN maq = new MTLOIN();
                        clt = _ParametrosIni.IniciaNuevoCliente();

                        //codigo de operacion
                        PAROPE argumentos = new PAROPE();
                        argumentos.CODOPE = CodigoOperacion.ELIMINA_VARIABLE;
                        //asigna parametros entrada en orden

                        List<string> parEnt = new List<string>();
                        parEnt.Add(corrLiq);

                        argumentos.VALENT = parEnt.ToArray();
                        resultado = clt.EjecutaOperacion(argumentos);
                        if (resultado.ESTOPE)
                        {
                            resguardar = true;
                        }
                        else
                        {
                            MostrarMensaje(resultado.MENERR);
                        }
                    }
                    else
                    {
                        eliminarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_VARIABLE_ELIMINAR);
                    eliminarButton.Enabled = false;
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

    }
}