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

using Microsoft.Reporting.WebForms;
using System.Collections.Specialized;

using appFew.appServicio;
using appWcfService;
using appConstantes;

namespace appFew.conf
{
    public partial class lineas : System.Web.UI.Page
    {
        private ParametrosFe _ParametrosIni;

        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;

        private string url = string.Empty;

        private bool Cargando = false;


        /// <summary>
        /// There is a ButtonField column and the Id column
        /// therefore first edit cell index is 2
        /// </summary>


        private List<PRLIPR> DetalleBusquedaLote { get; set; }

        //{
        //    get
        //    {
        //        DataTable dt = (DataTable)Session["DetalleBusquedaPartidaCalidad"];

        //        if (dt == null)
        //        {
        //            // Create a DataTable and save it to session
        //            //dt = ObtieneDetalleBusquedaDefault();

        //            DetalleBusquedaLote = dt;
        //        }

        //        return dt;
        //    }

        //    set
        //    {
        //        Session["DetalleBusquedaPartidaCalidad"] = value;
        //    }
        //}

        private void SetDataSourceDetalleBusquedaLote(List<PRLIPR> _detalleDescarga)
        {
            DetalleBusquedaLote = _detalleDescarga;
            busqLoteGridView.DataSource = _detalleDescarga;
            busqLoteGridView.DataBind();
        }

        protected void Page_InitComplete(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            _ParametrosIni = (ParametrosFe)Session["ParametrosFe"];

            if (Session["ParametrosFe"] == null)
            {
                var returnUrl = Request.Url.AbsoluteUri; //Request.Url.AbsolutePath;
                //var returnUrl = "mae/tipomaquina.aspx";
                //Response.Redirect("../login.aspx");
                Response.Redirect("../login.aspx?ReturnURL=" + returnUrl);
            }
            else
            {
                try
                {
                    ErrorLabel.Font.Bold = false;
                    AsignaAtributos();
                    if (!Page.IsPostBack)
                    {
                        CargaLineasProduccion();
                        ValoresIniciales();
                    }
                }
                catch (Exception ex)
                {
                    Error_1 = "Ha ocurrido un error en la pagina.";
                    url = "..//ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + Error_2;
                    Response.Redirect(url);
                }
            }
        }

        private void AsignaAtributos()
        {

            //nrodiaTextBox.Attributes.Add("onKeyPress", "doClick('" + BuscarButton.ClientID + "',event)");

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

        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            try
            {
                VistaBusquedaRegistro();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void LimpiaDatos()
        {
            habilCheckBox.Checked = false; 
            linproDropDownList.SelectedValue = "0";
        }

        private void HabilitaCampos(bool flag, bool esnuevo = false)
        {
            linproDropDownList.Enabled = !flag;
            habilCheckBox.Enabled = flag;

            grabarButton.Enabled = flag;
            cancelarButton.Enabled = flag;

            BuscarButton.Enabled = !flag;
        }

        private void MuestraBuscar(bool flag)
        {
            string estilo, visible, estilo2, visible2;
            estilo = "visibility";
            estilo2 = "display";
            if (flag)
            {
                visible = "visible";
                visible2 = "inline";
            }
            else
            {
                visible = "hidden";
                visible2 = "none";
            }
            BuscarButton.Style[estilo2] = visible2;
        }

        protected void cancelarButton_Click(object sender, EventArgs e)
        {
            try
            {
                MuestraBuscar(true);
                HabilitaCampos(false); 
                LimpiaDatos();
                instanciaActualHiddenField.Value = Constantes.INSTANCIA_INICIAL;
                accionLabel.Text = "";
                up.Update();

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool ValidaDatos(out string mensajeValidacion)
        {
            int nrodoc;
            DateTime fecha;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";

            if (linproDropDownList.SelectedIndex == 0)
            {
                mensajeValidacion = Mensajes.MENSAJE_SELECCIONE_LINEA_PRODUCCION;
                return false;
            }

            return resultado;
        }

        protected void grabarButton_Click(object sender, EventArgs e)
        {
            try
            {
                Guardar(); 
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool Guardar()
        {
            bool resultadoOpe;
            string mensajeValidacion;
            DateTime fecha;

            IappServiceClient clt = null;
            resultadoOpe = false;
            fecha = DateTime.Today;
            try
            {
                if (!ValidaDatos(out mensajeValidacion))
                {
                    //ErrorLabel.Text = mensajeValidacion;
                    MostrarMensaje(mensajeValidacion);

                    return false;
                }

                PRLIPR registro;
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.ACTUALIZA_LINEA_PRODUCCION;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                registro = new PRLIPR() { LIPRCODI = decimal.Parse(linproDropDownList.SelectedValue), CLIPESTA = habilCheckBox.Checked ? Constantes.ESTADO_ACTIVO : Constantes.ESTADO_ELIMINADO_NO_ACTIVO };
                parEnt.Add(FuncionesUtil.Serialize<PRLIPR>(registro));

                argumentos.VALENT = parEnt.ToArray();
                //PROCESA
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    resultadoOpe = true;
                    MostrarMensaje(Mensajes.MENSAJE_GRABADO_EXITOSO);

                    instanciaActualHiddenField.Value = Constantes.INSTANCIA_GENERADO;
                    HabilitaCampos(true, false);
                    accionLabel.Text = Mensajes.TEXTO_EDICION_REGISTRO;
                }
                else
                {
                    ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    MostrarMensaje(resultado.MENERR);
                }

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        //protected void nuevoButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        HabilitaCampos(true, true);
        //        MuestraBuscar(false);
        //        LimpiaDatos();
        //        ValoresIniciales();
        //        instanciaActualHiddenField.Value = Constantes.INSTANCIA_NUEVO;
        //        accionLabel.Text = Mensajes.TEXTO_NUEVO_REGISTRO;
        //        up.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
        //    }
        //}

        private void ValoresIniciales()
        {
            //fechaTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            //fechaIniTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            instanciaActualHiddenField.Value = Constantes.INSTANCIA_INICIAL;
            accionLabel.Text = "";
        }

        //protected void eliminarButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Eliminar())
        //        {
        //            cancelarButton_Click(cancelarButton, new EventArgs());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
        //        MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message)); 
        //    }
        //}

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
                parEnt.Add("1");
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = linproDropDownList.Items[0];
                        linproDropDownList.Items.Clear();
                        linproDropDownList.Items.Add(firstitem);

                        List<PRLIPR> datos = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);
                        foreach (PRLIPR item in datos)
                        {
                            linproDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
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

        private void MostrarMensaje(string mensaje)
        {
            ScriptManager.RegisterStartupScript(up, up.GetType(), "myAlert", "alert('" + mensaje + "');", true);
            ErrorLabel.Text = _ParametrosIni.ErrorGenerico(mensaje);
        }

        private bool BuscaRegistro()
        {
            bool resultadoOpe;
            DateTime fecha;
            IappServiceClient clt = null;
            resultadoOpe = false;
            fecha = DateTime.Today;
            try
            {

                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.BUSCA_LINEA_PRODUCCION;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                //parEnt.Add(linproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE) ? "" : linproDropDownList.SelectedValue);
                parEnt.Add(linproDropDownList.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1"))
                    {
                        DetalleBusquedaLote = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);
                        if (DetalleBusquedaLote.Count > 0)
                        {
                            resultadoOpe = true;
                        }
                    }
                    //else
                    //{
                    //    //MostrarMensaje(Mensajes.MENSAJE_HORIZONTE_NO_ENCONTRADO);
                    //    if (!linproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                    //    {
                    //        AsignaValores(linproDropDownList.SelectedValue, 0);
                    //    }
                    //}
                }
                else
                {
                    ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                    MostrarMensaje(resultado.MENERR);
                }

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }

        private void VistaBusquedaRegistro()
        {
            if (BuscaRegistro())
            {
                if (DetalleBusquedaLote.Count == 1)
                {
                    PRLIPR item = DetalleBusquedaLote[0];
                    AsignaValores(item.LIPRCODI.ToString(), item.CLIPESTA);
                }
                else
                {
                    SetDataSourceDetalleBusquedaLote(DetalleBusquedaLote);

                    busqLotesModalPopupExtender.Show();
                    busqLoteGridView.Focus();
                }
            }
        }

        protected void busqLoteGridView_SelectedIndexChanging(object sender, System.Web.UI.WebControls.GridViewSelectEventArgs e)
        {
            try
            {
                if (busqLoteGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = busqLoteGridView.Rows[busqLoteGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = busqLoteGridView.Rows[e.NewSelectedIndex];

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

        protected void busqLoteGridView_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        private void AsignaValores(string linea, string estado)
        {
            linproDropDownList.SelectedValue = linea;
            habilCheckBox.Checked = (!string.IsNullOrWhiteSpace(estado) && estado.Equals(Constantes.ESTADO_ACTIVO)); // ? true :  false;
            //deshabilita
            MuestraBuscar(false);
            HabilitaCampos(true);
            accionLabel.Text = Mensajes.TEXTO_EDICION_REGISTRO;
        }

        protected void aceptarBusqButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (busqLoteGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = busqLoteGridView.Rows[busqLoteGridView.SelectedIndex];
                    AsignaValores(Server.HtmlDecode(previorow.Cells[0].Text).Trim(), Server.HtmlDecode(previorow.Cells[3].Text).Trim());
                }
                else
                {
                    up.Update();
                    busqLotesModalPopupExtender.Show();
                    errorPopupBusqLoteLabel.Text = Mensajes.MENSAJE_SELECCIONE_REGISTRO_BUSCAR;

                    return;
                }
            }
            catch (Exception ex)
            {
                errorPopupBusqLoteLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        protected void busqLoteGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (!e.Row.Cells[0].Text.Equals("-1"))
                    {
                        if (e.Row.Cells[2].Text.Equals(Constantes.ESTADO_ACTIVO))
                        {
                            e.Row.Cells[2].Text = Mensajes.TEXTO_SI;
                        }
                        else
                        {
                            e.Row.Cells[2].Text = Mensajes.TEXTO_NO;
                        }

                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(busqLoteGridView, "Select$" + e.Row.RowIndex);
                        e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //DataRow item = ((System.Data.DataRowView)e.Row.DataItem).Row;

                    //e.Row.Cells[2].Text = Convert.ToDateTime(item["PRCLFEIN"]).ToString(Constantes.FORMATO_FECHA);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void busqLoteGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            busqLotesModalPopupExtender.Show();
        }

        protected void linproDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                if (!linproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    VistaBusquedaRegistro();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }
    }
}