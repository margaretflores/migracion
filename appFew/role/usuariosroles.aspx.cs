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
    public partial class usuariosroles : System.Web.UI.Page
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

        private List<GAUSUA> UsuariosSinAsignar
        {
            get
            {
                List<GAUSUA> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_USUARIOS_ROL] as List<GAUSUA>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneDetalleDefault();

                    UsuariosSinAsignar = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_USUARIOS_ROL] = value;
            }
        }
    
        private List<GAUSUA> UsuariosAsignados {
            get
            {
                List<GAUSUA> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_USUARIOS_ROL_ASIG] as List<GAUSUA>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneDetalleDefault();

                    UsuariosAsignados = dt;
                }
                return dt;
            }
            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_USUARIOS_ROL_ASIG] = value;
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
                        CargaRolesCalidad();
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

            txtSearch.Attributes.Add("onKeyPress", "doClick('" + filtraUsersButton.ClientID + "',event)");

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
            //rolDropDownList.Enabled = !flag;
            //tipproDropDownList.Enabled = !flag;


            grabarButton.Enabled = flag;
            cancelarButton.Enabled = flag;
            grabar2Button.Enabled = flag;
            cancelar2Button.Enabled = flag;

            //BuscarButton.Enabled = !flag;

            agregarButton.Enabled = false;
            removerButton.Enabled = false;
            dispGridView.Enabled = flag;
            asigvarGridView.Enabled = flag;

            txtSearch.Enabled = flag;
            filtraUsersButton.Enabled = flag;
        }

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
            bool resultado;
            resultado = true;
            mensajeValidacion = "";

            if (rolDropDownList.SelectedIndex == 0)
            {
                mensajeValidacion = Mensajes.MENSAJE_SELECCIONE_ROL;
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
                argumentos.CODOPE = CodigoOperacion.GUARDA_USUARIOS_ROL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                
                parEnt.Add(rolDropDownList.SelectedValue);
                parEnt.Add(FuncionesUtil.Serialize<List<GAUSUA>>(UsuariosAsignados));

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

        private void ValoresIniciales(bool desderol = false)
        {
            //fechaTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            //fechaIniTextBox.Text = DateTime.Today.ToString(Constantes.FORMATO_FECHA);
            instanciaActualHiddenField.Value = Constantes.INSTANCIA_INICIAL;
            txtSearch.Text = "";
            accionLabel.Text = "";

            if (!desderol)
            {
                rolDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;
            }

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

        public void CargaRolesCalidad()
        {

            IappServiceClient clt = null;
            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ROLES_APP;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_HABILITADOS));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = rolDropDownList.Items[0];
                        rolDropDownList.Items.Clear();
                        rolDropDownList.Items.Add(firstitem);

                        List<GRROAP> datos = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[1]);
                        foreach (GRROAP item in datos)
                        {
                            rolDropDownList.Items.Add(new ListItem(item.GRUSDEGR, item.GRUSCOGR.ToString()));
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

        private void SetDataSourceAsig(List<GAUSUA> _listaUsuarios)
        {
            if (_listaUsuarios == null || _listaUsuarios.Count == 0)
            {
                _listaUsuarios = ObtieneDetalleDefault();
                asigvarGridView.SelectedIndex = -1;
                removerButton.Enabled = false;
            }

            UsuariosAsignados = _listaUsuarios;
            asigvarGridView.DataSource = UsuariosAsignados;
            asigvarGridView.DataBind();
            if (asigvarGridView.SelectedIndex >= asigvarGridView.Rows.Count)
            {
                asigvarGridView.SelectedIndex = asigvarGridView.Rows.Count - 1;
            }
            if (UsuariosAsignados[0].USUACOUS != "")
            {
                removerButton.Enabled = true;
            }
        }

        private void SetDataSourceDisp(List<GAUSUA> _listaUsuarios)
        {
            if (_listaUsuarios == null || _listaUsuarios.Count == 0)
            {
                _listaUsuarios = ObtieneDetalleDefault();
                dispGridView.SelectedIndex = -1;
                agregarButton.Enabled = false;
            }

            UsuariosSinAsignar = _listaUsuarios;
            dispGridView.DataSource = UsuariosSinAsignar;
            dispGridView.DataBind();
            if (dispGridView.SelectedIndex >= dispGridView.Rows.Count)
            {
                dispGridView.SelectedIndex = dispGridView.Rows.Count - 1;
            }
            if (UsuariosSinAsignar[0].USUACOUS != "")
            {
                agregarButton.Enabled = true;
            }
        }

        private void SetDataSourceDispFiltro(List<GAUSUA> _listaUsuarios)
        {
            if (_listaUsuarios == null || _listaUsuarios.Count == 0)
            {
                _listaUsuarios = ObtieneDetalleDefault();
                dispGridView.SelectedIndex = -1;
                agregarButton.Enabled = false;
            }
            dispGridView.DataSource = _listaUsuarios;
            dispGridView.DataBind();
            if (dispGridView.SelectedIndex >= dispGridView.Rows.Count)
            {
                dispGridView.SelectedIndex = dispGridView.Rows.Count - 1;
            }
            if (_listaUsuarios[0].USUACOUS != "")
            {
                agregarButton.Enabled = true;
            }

        }

        private List<GAUSUA> ObtieneDetalleDefault()
        {
            List<GAUSUA> datos = new List<GAUSUA>();
            datos.Add(new GAUSUA() { USUACOUS = "", USUANOUS = "" });
            return datos;
        }

        private bool CargaUsuarios()
        {
            bool resultadoOpe;
            DateTime fecha;
            IappServiceClient clt = null;
            resultadoOpe = false;
            fecha = DateTime.Today;
            try
            {
                if (rolDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    cancelarButton_Click(cancelarButton, new EventArgs());
                    return false;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_USUARIOS_ROL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(rolDropDownList.SelectedValue);
                //parEnt.Add(tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE) ? "" : tipproDropDownList.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    //if (resultado.VALSAL[0].Equals("1"))
                    //{
                    UsuariosSinAsignar = FuncionesUtil.Deserialize<List<GAUSUA>>(resultado.VALSAL[0]);
                    UsuariosAsignados = FuncionesUtil.Deserialize<List<GAUSUA>>(resultado.VALSAL[1]);

                    SetDataSourceDisp(UsuariosSinAsignar);
                    SetDataSourceAsig(UsuariosAsignados);

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

        protected void rolDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaUsuarios();
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
                        e.Row.Cells[2].Visible = false;
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

        protected void filtraUsersButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    SetDataSourceDispFiltro(UsuariosSinAsignar);
                }
                else
                {
                    List<GAUSUA> _varfiltro = UsuariosSinAsignar.FindAll(x => x.USUANOUS.ToUpper().Contains(txtSearch.Text.ToUpper()) || x.USUACOUS.Contains(txtSearch.Text));
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
                    if (corrLiq != "" && UsuariosAsignados != null && UsuariosAsignados[0].USUACOUS != "")
                    {
                        GAUSUA var = UsuariosAsignados.Find(x => x.USUACOUS == corrLiq);
                        if (UsuariosSinAsignar[0].USUACOUS == "")
                        {
                            UsuariosSinAsignar.Clear();
                        }
                        UsuariosAsignados.Remove(var);
                        UsuariosSinAsignar.Add(var);
                        if (string.IsNullOrWhiteSpace(txtSearch.Text))
                        {
                            SetDataSourceDispFiltro(UsuariosSinAsignar);
                        }
                        else
                        {
                            List<GAUSUA> _varfiltro = UsuariosSinAsignar.FindAll(x => x.USUANOUS.ToUpper().Contains(txtSearch.Text.ToUpper()) || x.USUACOUS.Contains(txtSearch.Text));
                            SetDataSourceDispFiltro(_varfiltro);
                        }
                        //SetDataSourceDisp(UsuariosSinAsignar);
                        SetDataSourceAsig(UsuariosAsignados);
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
                    if (corrLiq != "" && UsuariosSinAsignar != null && UsuariosSinAsignar[0].USUACOUS != "")
                    {
                        GAUSUA var = UsuariosSinAsignar.Find(x => x.USUACOUS == corrLiq);
                        if (UsuariosAsignados[0].USUACOUS == "")
                        {
                            UsuariosAsignados.Clear();
                        }
                        UsuariosSinAsignar.Remove(var);

                        UsuariosAsignados.Add(var);

                        if (string.IsNullOrWhiteSpace(txtSearch.Text))
                        {
                            SetDataSourceDispFiltro(UsuariosSinAsignar);
                        }
                        else
                        {
                            List<GAUSUA> _varfiltro = UsuariosSinAsignar.FindAll(x => x.USUANOUS.ToUpper().Contains(txtSearch.Text.ToUpper()) || x.USUACOUS.Contains(txtSearch.Text));
                            SetDataSourceDispFiltro(_varfiltro);
                        }
                        //SetDataSourceDisp(UsuariosSinAsignar);
                        SetDataSourceAsig(UsuariosAsignados);
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