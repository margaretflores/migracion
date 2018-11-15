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

namespace appFew.mae
{
    public partial class calidadtipfib : System.Web.UI.Page
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
                        CargaTiposGrupoPendMaquina();
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

        private List<PPCTGP> CalidadesTiposGruposPendientes
        {
            get
            {
                List<PPCTGP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_CALI_GRUP_PEND] as List<PPCTGP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    CalidadesTiposGruposPendientes = dt;
                }

                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_CALI_GRUP_PEND] = value;
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

        private List<PPCTGP> ObtieneCabeceraDefault()
        {
            List<PPCTGP> datos = new List<PPCTGP>();
            datos.Add(new PPCTGP() { CTGPLICO = -1 });
            return datos;
        }

        private void CargaDatosIniciales()
        {
            CargaCalidadesTipoGrupoPendientes(1);
        }

        public void CargaTiposGrupoPendMaquina()
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
                parEnt.Add(tbgrpeHiddenField.Value);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = tipfibDropDownList.Items[0];
                        tipfibDropDownList.Items.Clear();
                        tipfibDropDownList.Items.Add(firstitem);

                        List<PPTGPM> datos = FuncionesUtil.Deserialize<List<PPTGPM>>(resultado.VALSAL[1]);
                        foreach (PPTGPM item in datos)
                        {
                            tipfibDropDownList.Items.Add(new ListItem(item.TGPMDESC, item.TGPMCODI));
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

        private void CargaCalidadesTipoGrupoPendientes(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_CALIDADES_TIPO_GRUPO_PENDIENTES;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(txtSearch.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());
                parEnt.Add(tbgrpeHiddenField.Value);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PPCTGP> datos = FuncionesUtil.Deserialize<List<PPCTGP>>(resultado.VALSAL[1]);

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

        private void SetDataSource(List<PPCTGP> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            CalidadesTiposGruposPendientes = _listaMaquinas;
            cabeceraGridView.DataSource = CalidadesTiposGruposPendientes;
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
                string corrLiq = row.Cells[1].Text;
                //LENAR TABPAGES
                List<PPCTGP> detMaquina = CalidadesTiposGruposPendientes;

                if (detMaquina != null && !detMaquina[0].CTGPCOCA.Equals("")) // != -1
                {
                    PPCTGP maq = detMaquina.Find(x => x.CTGPCOCA.Equals(corrLiq) && x.CTGPTBGP == row.Cells[2].Text);

                    descripcion3Label.Text = maq.TGPMDESC.Trim(); 
                    localizacionLabel.Text = "";

                    calcodHiddenField.Value = maq.CTGPCOCA.ToString();
                    modificarButton.Enabled = true;
                    eliminarButton.Enabled = true;
                }
            }
            else
            {
                calcodHiddenField.Value = "";
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
            DateTime fecha;
            decimal valor, valor2;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";

            //desmaqeditTextBox.Attributes.Add("readonly", "readonly");
            //
            if (string.IsNullOrWhiteSpace(calcodeditTextBox.Text))
            {
                mensajeValidacion = Mensajes.MENSAJE_INGRESE_DESCRIPCION;
                return false;
            }

            if (tipfibDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
            {
                mensajeValidacion = Mensajes.MENSAJE_SELECCIONE_TIPO_FIBRA;
                return false;
            }

            //DataTable cabDataTable;
            //cabDataTable = Session["TablaLiquidacion"] as DataTable;

            ////detLiquid = Session["DetalleGuia"] as DataTable;
            //    foreach (DataRow rw in cabDataTable.Rows)
            //    {
            //        if (Convert.ToString(rw["DCCASEDO"]).Trim().Equals("") || Convert.ToString(rw["DCCANUDO"]).Equals(""))
            //        {
            //            mensajeValidacion = Mensajes.MENSAJE_INGRESE_SERIE_NUMERO_DOCUMENTOS;
            //            return false;
            //        }
            //        if (Convert.ToString(rw["DCCACOPR"]).Trim().Equals("") && tipoDocDropDownList.SelectedValue.Equals(Constantes.CODIGO_DOCUMENTO_FACTURA)  )
            //        {
            //            mensajeValidacion = Mensajes.MENSAJE_INGRESE_PROVEEDOR;
            //            return false;
            //        }
            //        if (!Funciones.TryParseDate(Convert.ToString(rw["DCCAFECH"]), Constantes.FORMATO_FECHA, out fecha))
            //        {
            //            mensajeValidacion = Mensajes.INGRESE_FECHA_CORRECTA;
            //            return false;
            //        }
            //    }

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
                SetDataSource(CalidadesTiposGruposPendientes);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaCalidadesTipoGrupoPendientes(1);
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
                    CargaCalidadesTipoGrupoPendientes(indicepag);
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
                tituloeditLiteral.Text = Mensajes.TITULO_AGREGAR_CALIDAD_TIPO_FIBRA;
                calcodHiddenField.Value = "";
                calcodeditTextBox.Text = "";
                tipfibDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;
                calcodeditTextBox.Enabled = true;

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
                    string corrLiq = row.Cells[1].Text;
                    //LENAR TABPAGES
                    List<PPCTGP> detMaquina = CalidadesTiposGruposPendientes;

                    if (detMaquina != null && !detMaquina[0].CTGPCOCA.Equals(""))
                    {
                        PPCTGP maq = detMaquina.Find(x => x.CTGPCOCA == corrLiq && x.CTGPTBGP == row.Cells[2].Text);
                        calcodHiddenField.Value = maq.CTGPCOCA.ToString();
                        calcodeditTextBox.Text = maq.CTGPCOCA.ToString();
                        tipfibDropDownList.SelectedValue = maq.CTGPCOGP;

                        tituloeditLiteral.Text = Mensajes.TITULO_MODIFICAR_CALIDAD_TIPO_FIBRA;

                        calcodeditTextBox.Enabled = false;
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
                if (GuardaCalidadTipoFibra())
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaCalidadesTipoGrupoPendientes(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaCalidadTipoFibra()
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
                PPCTGP maq = new PPCTGP();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_CALIDADES_TIPO_GRUPOS_PENDIENTES;
                //asigna parametros entrada en orden
                //asigna parametros entrada en orden
                maq.CTGPCOCAORI = calcodHiddenField.Value;
                maq.CTGPCOCA = calcodeditTextBox.Text;
                maq.CTGPLICO = 0;
                maq.CTGPTBGP = tbgrpeHiddenField.Value;
                maq.CTGPCOGP = tipfibDropDownList.SelectedValue;

                //maq.CATFUSMD = _ParametrosIni.Usuario; 

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
                if (EliminaCalidadTipoFibra())
                {
                    CargaCalidadesTipoGrupoPendientes(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool EliminaCalidadTipoFibra()
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
                        argumentos.CODOPE = CodigoOperacion.ELIMINA_CALIDADES_TIPO_FIBRA;
                        //asigna parametros entrada en orden

                        List<string> parEnt = new List<string>();
                        parEnt.Add(corrLiq);
                        parEnt.Add(row.Cells[1].Text);

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
                    MostrarMensaje(Mensajes.SELECCIONE_REGISTRO_ELIMINAR);
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