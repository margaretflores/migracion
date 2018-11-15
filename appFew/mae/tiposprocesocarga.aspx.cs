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
    public partial class tiposprocesocarga : System.Web.UI.Page
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

        private List<PPRUPR> DiasNoLaborables
        {
            get
            {
                List<PPRUPR> dt = HttpContext.Current.Session[Constantes.NOMBRE_TIPOS_PROCESO_CARGA] as List<PPRUPR>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    DiasNoLaborables = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_TIPOS_PROCESO_CARGA] = value;
            }
        }

        private void AsignaAtributos()
        {
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            //descargar2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_GUIA + "')){ return false; };";
            modificarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ASIGNAR_REGISTRO + "')){ return false; };";
            eliminarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_REMOVER_REGISTRO + "')){ return false; };";

            //codmaqeditTextBox.Attributes.Add("onKeyPress", "doClick('" + buscaMaquinaButton.ClientID + "',event)");
            //desmaqeditTextBox.Attributes.Add("readonly", "readonly");
            //agregarButton.Enabled = true;
        }

        private List<PPRUPR> ObtieneCabeceraDefault()
        {
            List<PPRUPR> datos = new List<PPRUPR>();
            datos.Add(new PPRUPR() { RUTASETP = -1 });
            return datos;
        }

        private void CargaDatosIniciales()
        {
            CargaTiposProcesoCarga(1);
        }

        private void CargaTiposProcesoCarga(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_PROCESO_CARGA;
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
                        List<PPRUPR> datos = FuncionesUtil.Deserialize<List<PPRUPR>>(resultado.VALSAL[1]);

                        SetDataSource(datos);
                        if (pageindex != Convert.ToInt32(pageIndexHiddenField.Value))
                        {
                            cabeceraGridView.SelectRow(-1);
                        }
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

        private void SetDataSource(List<PPRUPR> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
                cabeceraGridView.SelectRow(-1);
            }

            DiasNoLaborables = _listaMaquinas;
            cabeceraGridView.DataSource = DiasNoLaborables;
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
                List<PPRUPR> detMaquina = DiasNoLaborables;

                if (detMaquina != null && detMaquina[0].RUTASETP != -1)
                {
                    PPRUPR maq = detMaquina.Find(x => x.RUTASETP == Convert.ToDecimal(corrLiq) && x.ARBOSEPE == Convert.ToDecimal(row.Cells[1].Text) && x.ARBOSEPI == Convert.ToDecimal(row.Cells[2].Text) && x.ARBONUPE == row.Cells[3].Text );

                    descripcion3Label.Text = maq.TTPRDESC.Trim(); //tipo proceso
                    localizacionLabel.Text = maq.RUPRSETC.Equals("1") ? Mensajes.TEXTO_SI : ""; //asignado/interviene

                    nupeLabel.Text = maq.ARBONUPE;
                    statLabel.Text = maq.PENDSTAT;
                    tipeLabel.Text = maq.ARBOTIPE;
                    lindesLabel.Text = maq.LIPRDESC;
                    artiLabel.Text = maq.ARBOARTI.Trim();
                    unidesLabel.Text = maq.TUPRDESC;
                    ferpLabel.Text = maq.PENDFERP.ToString(Constantes.FORMATO_FECHA);
                    setpLabel.Text = maq.RUTASETP.ToString();
                    siclLabel.Text = maq.MCSICL.Trim();
                    prodesLabel.Text = maq.TTPRDESC.Trim();
                    cappLabel.Text = maq.PENDCAPP.ToString(Constantes.FORMATO_DECIMAL_0 + "2");
                    setcLabel.Text = maq.RUPRSETC == "1" ? Mensajes.TEXTO_SI : "";

                    modificarButton.Enabled = maq.RUPRSETC == "0" ? true : false; //agregar
                    eliminarButton.Enabled = maq.RUPRSETC == "1" ? true : false; //remover
                }
            }
            else
            {
                nupeLabel.Text = "";
                statLabel.Text = "";
                tipeLabel.Text = "";
                lindesLabel.Text = "";
                artiLabel.Text = "";
                unidesLabel.Text = "";
                ferpLabel.Text = "";
                setpLabel.Text = "";
                siclLabel.Text = "";
                prodesLabel.Text = "";
                cappLabel.Text = "";
                setcLabel.Text = "";

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
                    if (e.Row.Cells[15].Text.Equals("1"))
                    {
                        e.Row.Cells[15].Text = Mensajes.TEXTO_INTERVIENE; // TEXTO_SI;
                    }
                    else
                    {
                        e.Row.Cells[15].Text = Mensajes.TEXTO_NO;
                    }

                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                }
                else
                {
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[12].Text = "";
                }
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
                SetDataSource(DiasNoLaborables);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaTiposProcesoCarga(1);
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
                    CargaTiposProcesoCarga(indicepag);
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
                    List<PPRUPR> detMaquina = DiasNoLaborables;
                    if (detMaquina != null && detMaquina[0].RUTASETP != -1)
                    {
                        //PPRUPR maq = detMaquina.Find(x => x.RUTASETP == Convert.ToDecimal(corrLiq));
                        PPRUPR maq = detMaquina.Find(x => x.RUTASETP == Convert.ToDecimal(corrLiq) && x.ARBOSEPE == Convert.ToDecimal(row.Cells[1].Text) && x.ARBOSEPI == Convert.ToDecimal(row.Cells[2].Text) && x.ARBONUPE == row.Cells[3].Text);

                        if (GuardaTipoProcesoCarga(maq))
                        {
                            CargaTiposProcesoCarga(Convert.ToInt32(pageIndexHiddenField.Value));
                            MuestraDatos();
                        }

                    }
                    else
                    {
                        modificarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_REGISTRO_MODIFICAR);
                    modificarButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool GuardaTipoProcesoCarga(PPRUPR maq)
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                if (maq == null)
                {
                    //MostrarMensaje(mensajeValidacion);
                    return false;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_TIPOS_PROCESO_CARGA;
                //asigna parametros entrada en orden

                maq.RUPRUSCR = _ParametrosIni.Usuario; 

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
                if (EliminaTipoProcesoCarga())
                {
                    CargaTiposProcesoCarga(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool EliminaTipoProcesoCarga()
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                GridViewRow row = cabeceraGridView.SelectedRow;
                if (row != null)
                {
                    string corrLiq = row.Cells[0].Text;
                    List<PPRUPR> detMaquina = DiasNoLaborables;
                    if (detMaquina != null && detMaquina[0].RUTASETP != -1)
                    {
                        //PPRUPR maq = detMaquina.Find(x => x.RUTASETP == Convert.ToDecimal(corrLiq));
                        PPRUPR maq = detMaquina.Find(x => x.RUTASETP == Convert.ToDecimal(corrLiq) && x.ARBOSEPE == Convert.ToDecimal(row.Cells[1].Text) && x.ARBOSEPI == Convert.ToDecimal(row.Cells[2].Text) && x.ARBONUPE == row.Cells[3].Text);
                        RESOPE resultado;
                        clt = _ParametrosIni.IniciaNuevoCliente();

                        //codigo de operacion
                        PAROPE argumentos = new PAROPE();
                        argumentos.CODOPE = CodigoOperacion.ELIMINA_TIPOS_PROCESO_CARGA;
                        //asigna parametros entrada en orden

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