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

namespace appFew.ncon
{
    public partial class solprodnoconforme : System.Web.UI.Page
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

        private List<PCPRNC> SolicitudPNC
        {
            get
            {
                List<PCPRNC> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_SOLICITUD_AUTORIZACION_PNC] as List<PCPRNC>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    SolicitudPNC = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_SOLICITUD_AUTORIZACION_PNC] = value;
            }
        }

        private void AsignaAtributos()
        {
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            anularButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ANULACION_PNC + "')){ return false; };";


            //codmaqeditTextBox.Attributes.Add("onKeyPress", "doClick('" + buscaMaquinaButton.ClientID + "',event)");
            //desmaqeditTextBox.Attributes.Add("readonly", "readonly");
            agregarButton.Enabled = true;

        }

        private List<PCPRNC> ObtieneCabeceraDefault()
        {
            List<PCPRNC> datos = new List<PCPRNC>();
            datos.Add(new PCPRNC() { PRNCIDNC  = -1 });
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
                        if (datos.Count == 1)
                        {
                            tipprofilDropDownList.Items.Clear();
                        }

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
            fecfinTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            fechaTextBox.Text = DateTime.Today.AddMonths(-1).ToString(Constantes.FORMATO_FECHA);
            CargaLineasProduccion();
            CargaSolicitudAutorizaPNC(1);
        }

        private void CargaSolicitudAutorizaPNC(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_SOLICITUD_AUTORIZA_PNC;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(txtSearch.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());

                parEnt.Add(linprofilDropDownList.SelectedValue);
                parEnt.Add(estautDropDownList.SelectedValue);
                parEnt.Add(fechaTextBox.Text);
                parEnt.Add(fecfinTextBox.Text);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCPRNC> datos = FuncionesUtil.Deserialize<List<PCPRNC>>(resultado.VALSAL[1]);

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

        private void SetDataSource(List<PCPRNC> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            SolicitudPNC = _listaMaquinas;
            cabeceraGridView.DataSource = SolicitudPNC;
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
            anularButton.Enabled = false;
            descripcion3Label.Text = ""; //accion
            localizacionLabel.Text = ""; //almacen
            feclimLabel.Text = "";
            observacionTextBox.Text = "";

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<PCPRNC> detMaquina = SolicitudPNC;

                if (detMaquina != null && detMaquina[0].PRNCIDNC != -1)
                {
                    PCPRNC maq = detMaquina.Find(x => x.PRNCIDNC == Convert.ToDecimal(corrLiq));

                    //descripcion3Label.Text = maq.MVARNOMB.Trim();
                    //localizacionLabel.Text = maq.TCTEDESC;
                    idspncHiddenField.Value = maq.PRNCIDNC.ToString();
                    iddeHiddenField.Value = maq.PRNCIDED.ToString();

                    switch (maq.PRNCESAC)
                    {
                        case Constantes.ACCION_DESECHAR:
                            descripcion3Label.Text = Mensajes.TEXTO_ACCION_DESECHAR;
                            break;
                        case Constantes.ACCION_REASIGNACION:
                            descripcion3Label.Text = Mensajes.TEXTO_ACCION_REASIGNACION;
                            break;
                        default:
                            descripcion3Label.Text = Mensajes.TEXTO_ACCION_REPROCESAR;
                            break;
                    }

                    localizacionLabel.Text = maq.PRNCALMA.ToString(); //almacen
                    feclimLabel.Text = maq.PRNCFELI.ToString(Constantes.FORMATO_FECHA);
                    observacionTextBox.Text = maq.PRNCOBSS;

                    if (maq.PRNCESTA.Equals(Constantes.ESTADO_SOLICITADO))
                    {
                        anularButton.Enabled = true;
                    }
                    BuscaIdDetProdNoConf();
                }
            }
            else
            {
                idspncHiddenField.Value = "";
                iddeHiddenField.Value = "";
                descripcion3Label.Text = "";
                localizacionLabel.Text = "";
            }
        }

        private void BuscaIdDetProdNoConf()
        {
            IappServiceClient clt = null;
            bool limpiar = true;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_IDDET_ENSAYO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                parEnt.Add(iddeHiddenField.Value);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        limpiar = false;

                        PCATFE datos = FuncionesUtil.Deserialize<PCATFE>(resultado.VALSAL[1]);
                        lineashowLabel.Text = datos.LIPRDESC;
                        procesoshowLabel.Text = datos.TTPRDESC;
                        variableshowLabel.Text = datos.MVARNOMB;
                        partidashowLabel.Text = datos.ENSAPART;
                        resultadoshowLabel.Text = datos.ENVAVALO.ToString();
                        estandarshowLabel.Text = datos.ENVAVAE1.ToString() + " - " + datos.ENVAVAE2.ToString();
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
                if (limpiar)
                {
                    lineashowLabel.Text = "";
                    procesoshowLabel.Text = "";
                    variableshowLabel.Text = "";
                    partidashowLabel.Text = "";
                    resultadoshowLabel.Text = "";
                    estandarshowLabel.Text = "";
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

        protected void cabeceraGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.Cells[0].Text.Equals("-1"))
                {
                    switch (e.Row.Cells[6].Text)
                    {
                        case Constantes.ESTADO_SOLICITADO:
                            e.Row.Cells[6].Text = Mensajes.TEXTO_ESTADO_SOLICITADA;
                            break;
                        case Constantes.ESTADO_ELIMINADO_NO_ACTIVO:
                            e.Row.Cells[6].Text = Mensajes.TEXTO_ESTADO_ANULADA;
                            break;
                        default:
                            e.Row.Cells[6].Text = Mensajes.TEXTO_ESTADO_AUTORIZADA;
                            break;
                    }
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";

                }
                else
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[4].Text = "";
                }
            }
        }

        private bool ValidaSeleccionado()
        {
            bool resultado;
            resultado = true;

            GridViewRow row = fueraestandarGridView.SelectedRow;
            if (row == null)
            {
                return false;
            }


            return resultado;
        }

        private bool ValidaDatos(out string mensajeValidacion)
        {
            //decimal valor, valor2;
            DateTime fecha;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";
            //valor2 = 0;

            if (string.IsNullOrWhiteSpace(obspncTextBox.Text))
            {
                mensajeValidacion = Mensajes.MENSAJE_INGRESE_OBSERVACION;
                return false;
            }

            if (!FuncionesUtil.TryParseDate(feclimTextBox.Text, out fecha))
            {
                    mensajeValidacion = Mensajes.MENSAJE_FECHA_NO_VALIDA;
                    return false;
            }
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
                SetDataSource(SolicitudPNC);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaSolicitudAutorizaPNC(1);
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
                    CargaSolicitudAutorizaPNC(indicepag);
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


                        //lineditLabel.Text = maq.LIPRDESC;
                        //proeditLabel.Text = maq.TTPRDESC;
                        //maqeditLabel.Text = maq.MAQUDES2;
                        //pareditLabel.Text = maq.ENSAPART;
                        //vareditLabel.Text = maq.MVARNOMB;
                        //osoeditTextBox.Text = maq.ATFEOBSE;
                        //obseditTextBox.Text = maq.ATFEOBSA;

                        //reseditLabel.Text = maq.ENVAVALO.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        //es1editLabel.Text = maq.ENVAVAE1.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        //es2editLabel.Text = maq.ENVAVAE2.ToString(Constantes.FORMATO_DECIMAL_0 + "1");

                        //tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                        ////PENDIENTE 20170725
                        //if (maq.PRNCESTA.Equals(Constantes.ESTADO_SOLICITADO))
                        //{
                        //    tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                        //    obseditTextBox.ReadOnly = true;
                        //    aceptarBusqButton.Enabled = false;
                        //}
                        //else
                        //{
                        //    tituloeditLiteral.Text = Mensajes.TITULO_VARIABLE_LIBERADA;
                        //    obseditTextBox.ReadOnly = false;
                        //    aceptarBusqButton.Enabled = true;
                        //}
                        busqfueraestandarModalPopupExtender.Show();
                        //CargaSolicitudAutorizaPNC(1);
                        CargaVariablesFueraEstandar(1);
                        aceptarfuesButton.Enabled = false;

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
                    List<PCPRNC> detMaquina = SolicitudPNC;

                    if (detMaquina != null && detMaquina[0].PRNCIDNC != -1)
                    {
                        PCPRNC maq = detMaquina.Find(x => x.PRNCIDNC == Convert.ToDecimal(corrLiq));
                        idspncHiddenField.Value = maq.PRNCIDNC.ToString();

                        //lineditLabel.Text = maq.LIPRDESC;
                        //proeditLabel.Text = maq.TTPRDESC;
                        //maqeditLabel.Text = maq.MAQUDES2;
                        //pareditLabel.Text = maq.ENSAPART;
                        //vareditLabel.Text = maq.MVARNOMB;
                        //osoeditTextBox.Text = maq.ATFEOBSE;
                        //obseditTextBox.Text = maq.ATFEOBSA;

                        //reseditLabel.Text = maq.ENVAVALO.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        //es1editLabel.Text = maq.ENVAVAE1.ToString(Constantes.FORMATO_DECIMAL_0 + "1");
                        //es2editLabel.Text = maq.ENVAVAE2.ToString(Constantes.FORMATO_DECIMAL_0 + "1");

                        //tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                        ////PENDIENTE 20170725
                        //if (maq.PRNCESTA.Equals(Constantes.ESTADO_SOLICITADO))
                        //{
                        //    tituloeditLiteral.Text = Mensajes.TITULO_LIBERAR_VARIABLE;
                        //    obseditTextBox.ReadOnly = true;
                        //    aceptarBusqButton.Enabled = false;
                        //}
                        //else
                        //{
                        //    tituloeditLiteral.Text = Mensajes.TITULO_VARIABLE_LIBERADA;
                        //    obseditTextBox.ReadOnly = false;
                        //    aceptarBusqButton.Enabled = true;
                        //}
                        busqfueraestandarModalPopupExtender.Show();
                    }
                    else
                    {
                        //modificarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_VARIABLE_MODIFICAR);
                    //modificarButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void anularButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (AnulaSolicitudAutorizaPNC())
                {
                    CargaSolicitudAutorizaPNC(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                //errorPopupfuesLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));

            }
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
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LINEAS_ASIGN_USUARIO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(_ParametrosIni.Usuario);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = linprofilDropDownList.Items[0];
                        linprofilDropDownList.Items.Clear();

                        var firstitemfe = linprofilfeDropDownList.Items[0];
                        linprofilfeDropDownList.Items.Clear();

                        List<PRLIPR> datos = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);
                        if (datos.Count != 1)
                        {
                            linprofilDropDownList.Items.Add(firstitem);
                            linprofilfeDropDownList.Items.Add(firstitemfe);
                        }
                        foreach (PRLIPR item in datos)
                        {
                            //linproDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                            linprofilDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                            linprofilfeDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                        }
                        if (datos.Count == 1)
                        {
                            linprofilDropDownList_SelectedIndexChanged(linprofilfeDropDownList, new EventArgs());
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


        #region genera nueva sol a partir de un fuera de estandar no liberada

        protected void fueraestandarGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        protected void fueraestandarGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                if (fueraestandarGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = fueraestandarGridView.Rows[fueraestandarGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = fueraestandarGridView.Rows[e.NewSelectedIndex];

                //brow.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                nuevorow.Attributes.Add("bgColor", "this.originalstyle");
                nuevorow.ToolTip = string.Empty;

            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void fueraestandarGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MuestraDatosFE();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void MuestraDatosFE()
        {
            aceptarfuesButton.Enabled = false;

            busqfueraestandarModalPopupExtender.Show();

            GridViewRow row = fueraestandarGridView.SelectedRow;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<PCATFE> detMaquina = AutorizaFueraEstandar;

                if (detMaquina != null && detMaquina[0].ATFEIDED != -1)
                {
                    PCATFE maq = detMaquina.Find(x => x.ATFEIDED == Convert.ToDecimal(corrLiq));

                    desvarLabel.Text = maq.MVARNOMB.Trim();
                    //idspncHiddenField.Value = "";
                    iddefuesHiddenField.Value = maq.ATFEIDED.ToString();
                    liprHiddenField.Value = maq.ENSACLPR.ToString();
                    tiprHiddenField.Value = maq.ENSACTPR.ToString();
                    
                    partida2Label.Text = maq.ENSAPART;

                    lineaLabel.Text = maq.LIPRDESC;
                    procesoLabel.Text = maq.TTPRDESC;
                    ensayoLabel.Text = maq.ATFEIDED.ToString();
                    fechaLabel.Text = maq.ENSAFEEN.ToString(Constantes.FORMATO_FECHA);
                    aceptarfuesButton.Enabled = true;

                }
            }
            else
            {
                //idspncHiddenField.Value = "";
                iddefuesHiddenField.Value = "";
                desvarLabel.Text = "";
                partida2Label.Text = "";
                liprHiddenField.Value = "";
                tiprHiddenField.Value = "";
                lineaLabel.Text = "";
                procesoLabel.Text = "";
                ensayoLabel.Text = "";
                fechaLabel.Text = "";
            }
        }

        protected void fueraestandarGridView_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(fueraestandarGridView, "Select$" + e.Row.RowIndex);
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

        protected void buscarfuesButton_Click(object sender, EventArgs e)
        {
            CargaVariablesFueraEstandar(1);
        }

        protected void primerofueraestandarButton_Click(object sender, EventArgs e)
        {
            navegafues(0);
        }

        protected void anteriorfueraestandarButton_Click(object sender, EventArgs e)
        {
            navegafues(-1);
        }

        protected void siguientefueraestandarButton_Click(object sender, EventArgs e)
        {
            navegafues(1);
        }

        protected void ultimofueraestandarButton_Click(object sender, EventArgs e)
        {
            navegafues(99);
        }

        private void EstadoFinalBotonesNavegacionfues()
        {
            int ultimo, total, indicepag;
            total = Convert.ToInt32(totalfuesHiddenField.Value);
            indicepag = Convert.ToInt32(pageIndexfuesHiddenField.Value);
            ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

            anteriorfueraestandarButton.Enabled = false;
            primerofueraestandarButton.Enabled = false;
            siguientefueraestandarButton.Enabled = false;
            ultimofueraestandarButton.Enabled = false;
            if (total > 0)
            {
                if (indicepag > 1)
                {
                    anteriorfueraestandarButton.Enabled = true;
                    primerofueraestandarButton.Enabled = true;
                }
                if (indicepag < ultimo)
                {
                    siguientefueraestandarButton.Enabled = true;
                    ultimofueraestandarButton.Enabled = true;
                }
            }
        }

        private void navegafues(int index)
        {
            try
            {
                int ultimo, total, indicepag;
                total = Convert.ToInt32(totalfuesHiddenField.Value);
                indicepag = Convert.ToInt32(pageIndexfuesHiddenField.Value);
                ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

                if (ultimo > 0)
                {
                    anteriorfueraestandarButton.Enabled = true;
                    primerofueraestandarButton.Enabled = true;
                    siguientefueraestandarButton.Enabled = true;
                    ultimofueraestandarButton.Enabled = true;
                }
                else
                {
                    anteriorfueraestandarButton.Enabled = false;
                    primerofueraestandarButton.Enabled = false;
                    siguientefueraestandarButton.Enabled = false;
                    ultimofueraestandarButton.Enabled = false;
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
                            anteriorfueraestandarButton.Enabled = false;
                            primerofueraestandarButton.Enabled = false;
                        }
                        break;
                    case 1: //siguiente
                        if (indicepag < ultimo)
                        {
                            indicepag++;
                        }
                        if (indicepag == ultimo)
                        {
                            siguientefueraestandarButton.Enabled = false;
                            ultimofueraestandarButton.Enabled = false;
                        }
                        break;
                    case 0: //primero
                        indicepag = 1;
                        anteriorfueraestandarButton.Enabled = false;
                        primerofueraestandarButton.Enabled = false;

                        break;
                    case 99: //ultimo
                        if (total > 0)
                        {
                            indicepag = ultimo;
                            siguientefueraestandarButton.Enabled = false;
                            ultimofueraestandarButton.Enabled = false;
                        }
                        break;
                }
                if (indicepag != Convert.ToInt32(pageIndexfuesHiddenField.Value))
                {
                    CargaVariablesFueraEstandar(indicepag);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private List<PCATFE> AutorizaFueraEstandar
        {
            get
            {
                List<PCATFE> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_FUERA_ESTANDAR] as List<PCATFE>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneFueraEstandarDefault();

                    AutorizaFueraEstandar = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_FUERA_ESTANDAR] = value;
            }
        }

        private void CargaVariablesFueraEstandar(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                busqfueraestandarModalPopupExtender.Show();
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_VARIABLES_NO_LIB_SIN_PNC;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(buscarfueraestandarTextBox.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());

                parEnt.Add(linprofilfeDropDownList.SelectedValue);
                parEnt.Add(tipprofilDropDownList.SelectedValue);
                parEnt.Add(varnomfilDropDownList.SelectedValue);
                parEnt.Add(Constantes.ESTADO_VARIABLE_NO_LIBERADA_RECHAZADA);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCATFE> datos = FuncionesUtil.Deserialize<List<PCATFE>>(resultado.VALSAL[1]);

                        SetDataSourcefues(datos);
                        totalfuesHiddenField.Value = resultado.VALSAL[2];
                    }
                    else
                    {
                        SetDataSourcefues(null);
                        totalfuesHiddenField.Value = "0";
                    }
                    pageIndexfuesHiddenField.Value = pageindex.ToString();
                    totalfueraestandarLabel.Text = " Total: " + totalfuesHiddenField.Value;
                    paginafueraestandarLabel.Text = "Pág: " + pageIndexfuesHiddenField.Value;

                }
                else
                {
                    SetDataSourcefues(null);
                    totalHiddenField.Value = "0";

                    MostrarMensaje(resultado.MENERR);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                }
                EstadoFinalBotonesNavegacionfues();
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

        private void SetDataSourcefues(List<PCATFE> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneFueraEstandarDefault();
            }

            AutorizaFueraEstandar = _listaMaquinas;
            fueraestandarGridView.DataSource = AutorizaFueraEstandar;
            fueraestandarGridView.DataBind();
        }

        private List<PCATFE> ObtieneFueraEstandarDefault()
        {
            List<PCATFE> datos = new List<PCATFE>();
            datos.Add(new PCATFE() { ATFEIDED = -1 });
            return datos;
        }

        protected void siguientefuesButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid )
            {
                up.Update();
                busqfueraestandarModalPopupExtender.Show();
                return;
            }
            try
            {
                if (!ValidaSeleccionado())
                {
                    busqfueraestandarModalPopupExtender.Show();
                    MostrarMensaje(Mensajes.MENSAJE_SELECCIONE_VARIABLE_NO_LIBERADA);
                    return ;
                }
                fues2ModalPopupExtender.Show();
                feclimTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
                //busqfueraestandarModalPopupExtender.Show();
                //if (GuardaSolicitudAutorizaPNC())
                //{
                //    busqfueraestandarModalPopupExtender.Hide();
                //    CargaSolicitudAutorizaPNC(Convert.ToInt32(pageIndexHiddenField.Value));
                //    MuestraDatos();
                //}
            }
            catch (Exception ex)
            {
                errorPopupfuesLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        protected void aceptarfues2Button_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                up.Update();
                fues2ModalPopupExtender.Show();
                return;
            }
            try
            {
                if (GuardaSolicitudAutorizaPNC())
                {
                    //busqfueraestandarModalPopupExtender.Hide();
                    CargaSolicitudAutorizaPNC(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
                else 
                {
                    fues2ModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {
                errorPopupfuesLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaSolicitudAutorizaPNC()
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
                PCPRNC maq = new PCPRNC();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_SOLICITUD_AUTORIZA_PNC;
                //asigna parametros entrada en orden
                //if (string.IsNullOrWhiteSpace(idspncHiddenField.Value))
                //{
                    maq.PRNCIDNC = -1;
                //}
                //else
                //{
                //    maq.PRNCIDNC = Convert.ToDecimal(idspncHiddenField.Value);
                //}

                maq.PRNCLIPR = Convert.ToDecimal(liprHiddenField.Value);

                maq.PRNCNUSO = 0;
                maq.PRNCIDED = Convert.ToDecimal(iddefuesHiddenField.Value);  //es nuevo siempre
                maq.PRNCFEEM = DateTime.Today;
                maq.PRNCPART = partida2Label.Text;
                maq.PRNCESTA = Constantes.ESTADO_SOLICITADO ;
                maq.PRNCOBSS = obspncTextBox.Text;
                maq.PRNCESAC = accionRadioButtonList.SelectedValue;
                maq.PRNCESRP = "";
                maq.PRNCFELI = FuncionesUtil.ParseDate(feclimTextBox.Text, Constantes.FORMATO_FECHA); // new DateTime(1, 1, 1);
                maq.PRNCALMA = decimal.Parse(almdesDropDownList.SelectedValue);
                maq.PRNCUSCR = _ParametrosIni.Usuario;

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

        private bool AnulaSolicitudAutorizaPNC()
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                RESOPE resultado;
                PCPRNC maq = new PCPRNC();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.CAMBIA_ESTADO_SOLICITUD_AUTORIZA_PNC;
                //asigna parametros entrada en orden
                maq.PRNCIDNC = Convert.ToDecimal(idspncHiddenField.Value);

                maq.PRNCIDED = Convert.ToDecimal(iddeHiddenField.Value);
                maq.PRNCESTA = Constantes.ESTADO_ELIMINADO_NO_ACTIVO;
                maq.PRNCUSCR = _ParametrosIni.Usuario;

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