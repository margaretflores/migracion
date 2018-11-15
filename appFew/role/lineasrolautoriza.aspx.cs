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

namespace appFew.role
{
    public partial class lineasrolautoriza : System.Web.UI.Page
    {
        private static ParametrosFe _ParametrosIni;

        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;

        private string url = string.Empty;

        //Hashtable HasDatosPC;
        private static int PageSize = 12;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            _ParametrosIni = (ParametrosFe)Session["ParametrosFe"];

            if (Session["ParametrosFe"] == null)
            {
                Response.Redirect("../login.aspx?ReturnURL=" + Request.Url.AbsoluteUri);
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

        private void AsignaAtributos()
        {
            //txtCodigo.Attributes.Add("onKeyPress", "doClick('" + buscaProvButton.ClientID + "',event)");
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            //descargar2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_GUIA + "')){ return false; };";

            //abrir2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ABRIR_GUIA + "')){ return false; };";
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(exportarButton);

            //observTextBox.Attributes.Add("readonly", "readonly");
        }

        private List<GRROAP> ObtieneCabeceraDefault()
        {
            List<GRROAP> datos = new List<GRROAP>();
            datos.Add(new GRROAP() { GRUSCOGR = "" });
            return datos;
        }

        private void CargaDatosIniciales()
        {
            CargaRoles(1);
        }

        private void CargaRoles(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ROLES_ACTIVOS;
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
                        List<GRROAP> datos = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[1]);

                        SetDataSource(datos);
                        totalHiddenField.Value = resultado.VALSAL[2];
                    }
                    else
                    {
                        SetDataSource(null);
                        totalHiddenField.Value = "0";
                    }
                    pageIndexHiddenField.Value = pageindex.ToString();
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

        private List<GRROAP> MaestroRoles
        {
            get
            {
                List<GRROAP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_LINEA] as List<GRROAP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    MaestroRoles = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_LINEA] = value;
            }
        }

        private void SetDataSource(List<GRROAP> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            MaestroRoles = _listaMaquinas;
            cabeceraGridView.DataSource = MaestroRoles;
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

        protected void cabeceraGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!Server.HtmlDecode(e.Row.Cells[0].Text).Trim().Equals(""))
                {
                    //if (e.Row.Cells[3].Text.Equals(Constantes.ESTADO_ACTIVO))
                    //{
                    //    e.Row.Cells[3].Text = Mensajes.TEXTO_SI;
                    //}
                    //else
                    //{
                    //    e.Row.Cells[3].Text = Mensajes.TEXTO_NO;
                    //}

                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                }
                else
                {
                    //e.Row.Cells[1].Text = "";
                }
            }
        }

        private void MuestraDatos()
        {
            bool habilitaAgrLin = false;

            GridViewRow row = cabeceraGridView.SelectedRow;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<GRROAP> detPlan = MaestroRoles;

                if (detPlan != null && detPlan[0].GRUSCOGR != "")
                {
                    GRROAP maq = detPlan.Find(x => x.GRUSCOGR == corrLiq);
                    codgruHiddenField.Value = maq.GRUSCOGR;
                    habilitaAgrLin = true;

                }
            }
            else
            {
                codgruHiddenField.Value = "";
            }
            CargaLineasAsignadas(habilitaAgrLin);
        }

        private bool ValidaDatos(out string mensajeValidacion)
        {
            bool resultado;
            resultado = true;
            mensajeValidacion = "";

            return resultado;
        }

        private void MostrarMensaje(string mensaje, bool noalert = false)
        {
            if (!noalert)
            {
                ScriptManager.RegisterStartupScript(up, up.GetType(), "myAlert", "alert('" + mensaje.Replace("<br>", " - " ) + "');", true);
            }
            //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(mensaje );
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaRoles(1);
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
                    CargaRoles(indicepag);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region lineas por Rol

        protected void lineasGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        protected void lineasGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.Cells[0].Text.Equals("-1"))
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(lineasGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";

                    //DateTime fecha;
                    //DataRow item = ((System.Data.DataRowView)e.Row.DataItem).Row;
                    //    e.Row.Cells[3].Text = fecha.ToString(Constantes.FORMATO_FECHA);
                    //e.Row.Cells[5].Text = Convert.ToDecimal(item["CCPVTA"]).ToString(Constantes.FORMATO_IMPORTE);
                }
            }
        }

        protected void lineasGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //habilita botones
                HabilitaEdicionLinea();

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
            }
        }

        protected void lineasGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                if (lineasGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = lineasGridView.Rows[lineasGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = lineasGridView.Rows[e.NewSelectedIndex];

                //brow.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                nuevorow.Attributes.Add("bgColor", "this.originalstyle");
                nuevorow.ToolTip = string.Empty;

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                //e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void HabilitaEdicionLinea()
        {
            GridViewRow row = lineasGridView.SelectedRow;
            removerLineaButton.Enabled = false;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<PRLIPR> detPlan = MaestroLineas;

                if (detPlan != null && detPlan[0].LIPRCODI != -1)
                {
                    //MTACTI maq = detPlan.Find(x => x.ACTIIDAC == Convert.ToDecimal(corrLiq));
                    removerLineaButton.Enabled = true;
                }
            }
        }

        private List<PRLIPR> ObtieneLineasDefault()
        {
            List<PRLIPR> datos = new List<PRLIPR>();
            datos.Add(new PRLIPR() { LIPRCODI = -1,  LIPRDESC = "" });
            return datos;
        }
        private List<PRLIPR> MaestroLineas
        {
            get
            {
                List<PRLIPR> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_LINEAS_ROL] as List<PRLIPR>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneLineasDefault();

                    MaestroLineas = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_LINEAS_ROL] = value;
            }
        }

        private void CargaLineasAsignadas(bool habilitaAgrLin = true)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LINEAS_ROL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(codgruHiddenField.Value);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                        List<PRLIPR> datos = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[0]);
                        List<PRLIPR> linnoasi = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);

                        MuestraLineasAsignadas(datos);

                        var firstitem = limproeditDropDownList.Items[0];
                        limproeditDropDownList.Items.Clear();
                        limproeditDropDownList.Items.Add(firstitem);
                        foreach (PRLIPR item in linnoasi)
                        {
                            limproeditDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                        }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
                agregarLineaButton.Enabled = habilitaAgrLin;
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

        private void MuestraLineasAsignadas(List<PRLIPR> _listaPlanes)
        {
            if (_listaPlanes == null || _listaPlanes.Count == 0)
            {
                _listaPlanes = ObtieneLineasDefault();
            }

            MaestroLineas = _listaPlanes;
            lineasGridView.DataSource = MaestroLineas;
            lineasGridView.DataBind();
        }


        private void LimpiaDatosLinea()
        {
            limproeditDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;
        }

        protected void agregarLineaButton_Click(object sender, EventArgs e)
        {
            try
            {
                tituloLineaEditLiteral.Text = Mensajes.TITULO_AGREGAR_LINEA;
                GridViewRow row = cabeceraGridView.SelectedRow;

                //LENAR TABPAGES
                List<GRROAP> detPlan = MaestroRoles;

                if (row != null)
                {
                    string corrLiq = row.Cells[0].Text;
                    if (detPlan != null && detPlan[0].GRUSCOGR != "")
                    {
                        GRROAP maq = detPlan.Find(x => x.GRUSCOGR == corrLiq);

                        codgruHiddenField.Value = corrLiq;
                        desnivapreditTextBox.Text = row.Cells[1].Text;
                        LimpiaDatosLinea();
                        lineaeditModalPopupExtender.Show();
                    }
                }
                else
                {
                    agregarLineaButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
            }
        }

        protected void removerLineaButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemueveLinea())
                {
                    CargaLineasAsignadas();
                }

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        //PENDIENTE ADD VALIDACIONES: FRECUENCIA > 0, NUMEROS FRECUENCIA, DURACIONES DIAS HORAS MINUTOS, DIAS PARO
        protected void aceptareditLineaButton_Click(object sender, EventArgs e)
        {
            try
            {
                string mensajevalida;
                if (!Page.IsValid)
                {
                    up.Update();
                    lineaeditModalPopupExtender.Show();
                    return;
                }

                if (GuardaLinea())
                {
                    CargaLineasAsignadas();
                }
                else
                {
                    lineaeditModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {
                errortarLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
            finally
            {
            }
        }

        private bool GuardaLinea()
        {
            IappServiceClient clt = null;
            bool resguardar = false;
            try
            {

                RESOPE resultado;
                PRLIPR maq = new PRLIPR();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.ASIGNA_LINEA_ROL;
                //asigna parametros entrada en orden

                List<string> parEnt = new List<string>();
                parEnt.Add(codgruHiddenField.Value);
                parEnt.Add(limproeditDropDownList.SelectedValue);

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

        private bool RemueveLinea()
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                GridViewRow row = lineasGridView.SelectedRow;
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
                        argumentos.CODOPE = CodigoOperacion.REMUEVE_LINEA_ROL;
                        //asigna parametros entrada en orden
                            List<string> parEnt = new List<string>();
                            parEnt.Add(codgruHiddenField.Value);
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
                        removerLineaButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_LINEA_REMOVER);
                    removerLineaButton.Enabled = false;
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

        #endregion

  

    }
}