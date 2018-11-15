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

namespace appFew.role
{
    public partial class rolesproceso : System.Web.UI.Page
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

        private List<GRROAP> RolesSinAsignar
        {
            get
            {
                List<GRROAP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_PROCESO] as List<GRROAP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneDetalleDefault();

                    RolesSinAsignar = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_PROCESO] = value;
            }
        }
    
        private List<GRROAP> RolesAsignados {
            get
            {
                List<GRROAP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_PROCESO_ASIG] as List<GRROAP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneDetalleDefault();

                    RolesAsignados = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_PROCESO_ASIG] = value;
            }
       
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

            txtSearch.Attributes.Add("onKeyPress", "doClick('" + filtraRolesButton.ClientID + "',event)");

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

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private void HabilitaCampos(bool flag, bool esnuevo = false)
        {
            //linproDropDownList.Enabled = !flag;
            //tipproDropDownList.Enabled = !flag;


            grabarButton.Enabled = flag;
            cancelarButton.Enabled = flag;

            //BuscarButton.Enabled = !flag;

            agregarButton.Enabled = false;
            removerButton.Enabled = false;
            dispGridView.Enabled = flag;
            asigvarGridView.Enabled = flag;

            txtSearch.Enabled = flag;
            filtraRolesButton.Enabled = flag;
        }

        //private void MuestraBuscar(bool flag)
        //{
        //    string estilo, visible, estilo2, visible2;
        //    estilo = "visibility";
        //    estilo2 = "display";
        //    if (flag)
        //    {
        //        visible = "visible";
        //        visible2 = "inline";
        //    }
        //    else
        //    {
        //        visible = "hidden";
        //        visible2 = "none";
        //    }
        //    BuscarButton.Style[estilo2] = visible2;
        //}

        protected void cancelarButton_Click(object sender, EventArgs e)
        {
            try
            {
                //MuestraBuscar(true);
                HabilitaCampos(false); 
                ValoresIniciales();
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

            if (tipproDropDownList.SelectedIndex == 0)
            {
                mensajeValidacion = Mensajes.MENSAJE_SELECCIONE_TIPO_PROCESO;
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

                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_ROLES_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                
                parEnt.Add(linproDropDownList.SelectedValue);
                parEnt.Add(tipproDropDownList.SelectedValue);
                parEnt.Add(maquinDropDownList.SelectedValue);
                parEnt.Add(FuncionesUtil.Serialize<List<GRROAP>>(RolesAsignados));
                parEnt.Add(_ParametrosIni.Usuario);

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

        private void ValoresIniciales(bool desdelinea = false)
        {
            //fechaTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            //fechaIniTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            instanciaActualHiddenField.Value = Constantes.INSTANCIA_INICIAL;
            accionLabel.Text = "";

            if (!desdelinea)
            {
                linproDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;
            }
            tipproDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;

            var i = maquinDropDownList.Items[0];
            maquinDropDownList.Items.Clear();
            maquinDropDownList.Items.Add(i);
            maquinDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;

            SetDataSourceDisp(null);
            SetDataSourceAsig(null);

            HabilitaCampos(false); 
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
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_HABILITADOS));
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

        public void CargaTiposProceso()
        {

            IappServiceClient clt = null;
            try
            {
                if (linproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    var firstitem = tipproDropDownList.Items[0];
                    tipproDropDownList.Items.Clear();
                    tipproDropDownList.Items.Add(firstitem);
                    return;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(linproDropDownList.SelectedValue);
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_HABILITADOS));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = tipproDropDownList.Items[0];
                        tipproDropDownList.Items.Clear();
                        tipproDropDownList.Items.Add(firstitem);

                        List<PRTTPR> datos = FuncionesUtil.Deserialize<List<PRTTPR>>(resultado.VALSAL[1]);
                        foreach (PRTTPR item in datos)
                        {
                            tipproDropDownList.Items.Add(new ListItem(item.TTPRDESC, item.TTPRCODI.ToString()));
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

        public void CargaMaquinasProceso()
        {

            IappServiceClient clt = null;
            try
            {
                var firstitem = maquinDropDownList.Items[0];
                maquinDropDownList.Items.Clear();
                maquinDropDownList.Items.Add(firstitem);
                if (tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    return;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_MAQUINAS_XPROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(linproDropDownList.SelectedValue);
                parEnt.Add(tipproDropDownList.SelectedValue);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCMQPR> datos = FuncionesUtil.Deserialize<List<PCMQPR>>(resultado.VALSAL[1]);
                        foreach (PCMQPR item in datos)
                        {
                            maquinDropDownList.Items.Add(new ListItem(item.MAQUCOMA + " " + item.MAQUDES2 + " " + item.MAQUMARC, item.MAQUIDMA.ToString()));
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

        private void SetDataSourceAsig(List<GRROAP> _listaMaquinas)
        {
            if (_listaMaquinas == null || _listaMaquinas.Count == 0)
            {
                _listaMaquinas = ObtieneDetalleDefault();
                asigvarGridView.SelectedIndex = -1;
                removerButton.Enabled = false;
            }

            RolesAsignados = _listaMaquinas;
            asigvarGridView.DataSource = RolesAsignados;
            asigvarGridView.DataBind();
            if (asigvarGridView.SelectedIndex >= asigvarGridView.Rows.Count)
            {
                asigvarGridView.SelectedIndex = asigvarGridView.Rows.Count - 1;
            }
            if (RolesAsignados[0].GRUSCOGR != "")
            {
                removerButton.Enabled = true;
            }
        }

        private void SetDataSourceDisp(List<GRROAP> _listaMaquinas)
        {
            if (_listaMaquinas == null || _listaMaquinas.Count == 0)
            {
                _listaMaquinas = ObtieneDetalleDefault();
                dispGridView.SelectedIndex = -1;
                agregarButton.Enabled = false;
            }

            RolesSinAsignar = _listaMaquinas;
            dispGridView.DataSource = RolesSinAsignar;
            dispGridView.DataBind();
            if (dispGridView.SelectedIndex >= dispGridView.Rows.Count)
            {
                dispGridView.SelectedIndex = dispGridView.Rows.Count - 1;
            }
            if (RolesSinAsignar[0].GRUSCOGR != "")
            {
                agregarButton.Enabled = true;
            }
        }

        private void SetDataSourceDispFiltro(List<GRROAP> _listaMaquinas)
        {
            if (_listaMaquinas == null || _listaMaquinas.Count == 0)
            {
                _listaMaquinas = ObtieneDetalleDefault();
                dispGridView.SelectedIndex = -1;
                agregarButton.Enabled = false;
            }
            dispGridView.DataSource = _listaMaquinas;
            dispGridView.DataBind();
            if (dispGridView.SelectedIndex >= dispGridView.Rows.Count)
            {
                dispGridView.SelectedIndex = dispGridView.Rows.Count - 1;
            }
            if (_listaMaquinas[0].GRUSCOGR != "")
            {
                agregarButton.Enabled = true;
            }

        }

        private List<GRROAP> ObtieneDetalleDefault()
        {
            List<GRROAP> datos = new List<GRROAP>();
            datos.Add(new GRROAP() { GRUSCOGR = "", GRUSDEGR = "" });
            return datos;
        }

        private bool CargaRoles()
        {
            bool resultadoOpe;
            DateTime fecha;
            IappServiceClient clt = null;
            resultadoOpe = false;
            fecha = DateTime.Today;
            try
            {
                if (tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    //cancelarButton_Click(cancelarButton, new EventArgs());
                    HabilitaCampos(false);
                    ValoresIniciales(true);
                    instanciaActualHiddenField.Value = Constantes.INSTANCIA_INICIAL;
                    accionLabel.Text = "";
                    up.Update();

                    return false;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ROLES_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(linproDropDownList.SelectedValue);
                //parEnt.Add(tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE) ? "" : tipproDropDownList.SelectedValue);
                parEnt.Add(tipproDropDownList.SelectedValue);
                parEnt.Add(maquinDropDownList.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //if (resultado.VALSAL[0].Equals("1"))
                    //{
                    RolesSinAsignar = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[0]);
                    RolesAsignados = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[1]);

                    SetDataSourceDisp(RolesSinAsignar);
                    SetDataSourceAsig(RolesAsignados);

                    dispGridView.SelectedIndex = -1;
                    asigvarGridView.SelectedIndex = -1;

                        //if (VariablesSinAsignar.Count > 0)
                        //{
                    HabilitaCampos(true);
                            resultadoOpe = true;
                        //}
                    //}
                    //else
                    //{
                    //    //MostrarMensaje(Mensajes.MENSAJE_HORIZONTE_NO_ENCONTRADO);
                    //    if (!tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                    //    {
                    //        AsignaValores(tipproDropDownList.SelectedValue, 0);
                    //    }
                    //}
                }
                else
                {
                    ValoresIniciales();
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

        private void AsignaValores(string linea, string estado)
        {
            tipproDropDownList.SelectedValue = linea;
            //habilCheckBox.Checked = (!string.IsNullOrWhiteSpace(estado) && estado.Equals(Constantes.ESTADO_ACTIVO)); // ? true :  false;
            //deshabilita
            //MuestraBuscar(false);
            HabilitaCampos(true);
            accionLabel.Text = Mensajes.TEXTO_EDICION_REGISTRO;
        }

        protected void tipproDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                CargaMaquinasProceso();
                CargaRoles();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void linproDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaTiposProceso();
                ValoresIniciales(true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void maquinDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaRoles();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void dispGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text.Equals(""))
                    {
                    }
                    else {
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dispGridView, "Select$" + e.Row.RowIndex);
                        e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
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

        protected void asigvarGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text.Equals(""))
                    {
                        //e.Row.Cells[2].Visible = false;
                    }
                    else
                    {
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(asigvarGridView, "Select$" + e.Row.RowIndex);
                        e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
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

        protected void gridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                GridView senderGridView = sender as GridView;
                if (senderGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = senderGridView.Rows[senderGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    if (previorow.Cells[0].Text != "")
                    {
                        previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                        previorow.Attributes.Add("bgColor", "#FF9999"); //6B69FF
                    }
                }
                GridViewRow nuevorow = senderGridView.Rows[e.NewSelectedIndex];

                //brow.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                nuevorow.Attributes.Add("bgColor", "this.originalstyle");
                nuevorow.ToolTip = string.Empty;

            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridView senderGridView = sender as GridView;
                if (senderGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = senderGridView.Rows[senderGridView.SelectedIndex];

                    if (senderGridView == dispGridView)
                    {
                        agregarButton.Enabled = false;
                        
                        if (previorow.Cells[0].Text != "")
                        {
                            agregarButton.Enabled = true;
                        }
                    }
                    else
                    {
                        removerButton.Enabled = false;

                        if (previorow.Cells[0].Text != "")
                        {
                            removerButton.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void filtraRolesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    SetDataSourceDispFiltro(RolesSinAsignar);
                }
                else
                {
                    List<GRROAP> _varfiltro = RolesSinAsignar.FindAll(x => x.GRUSDEGR.ToUpper().Contains(txtSearch.Text.ToUpper()) || x.GRUSCOGR.Contains(txtSearch.Text));
                    SetDataSourceDispFiltro(_varfiltro);
                }
                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void removerButton_Click(object sender, EventArgs e)
        {
            try
            {
                GridView senderGridView = asigvarGridView;
                if (senderGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = senderGridView.Rows[senderGridView.SelectedIndex];
                    string corrLiq = previorow.Cells[0].Text;
                    if (corrLiq != "" && RolesAsignados != null && RolesAsignados[0].GRUSCOGR != "")
                    {
                        GRROAP var = RolesAsignados.Find(x => x.GRUSCOGR == corrLiq);
                        if (RolesSinAsignar[0].GRUSCOGR == "")
                        {
                            RolesSinAsignar.Clear();
                        }
                        RolesAsignados.Remove(var);
                        RolesSinAsignar.Add(var);
                        SetDataSourceDisp(RolesSinAsignar);
                        SetDataSourceAsig(RolesAsignados);
                        dispGridView.SelectedIndex = dispGridView.Rows.Count - 1; 
                    }
                    else
                    {
                        removerButton.Enabled = false;
                    }
                }
                else
                {
                    removerButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void agregarButton_Click(object sender, EventArgs e)
        {
            try
            {
                GridView senderGridView = dispGridView;
                if (senderGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = senderGridView.Rows[senderGridView.SelectedIndex];
                    string corrLiq = previorow.Cells[0].Text;
                    if (corrLiq != "" && RolesSinAsignar != null && RolesSinAsignar[0].GRUSCOGR != "")
                    {
                        GRROAP var = RolesSinAsignar.Find(x => x.GRUSCOGR == corrLiq);
                        if (RolesAsignados[0].GRUSCOGR == "")
                        {
                            RolesAsignados.Clear();
                        }
                        RolesSinAsignar.Remove(var);
                        RolesAsignados.Add(var);
                        SetDataSourceDisp(RolesSinAsignar);
                        SetDataSourceAsig(RolesAsignados);
                        asigvarGridView.SelectedIndex = asigvarGridView.Rows.Count - 1; 
                    }
                    else
                    {
                        agregarButton.Enabled = false;
                    }
                }
                else
                {
                    agregarButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }
    }
}