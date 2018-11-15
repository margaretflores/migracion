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

using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;

using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Services;

using appFew.appServicio;
using appWcfService;
using appConstantes;

namespace appFew.role
{
    public partial class nivelesaprob : System.Web.UI.Page
    {
        private static ParametrosFe _ParametrosIni;

        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;

        private string url = string.Empty;

        decimal SumaTotal = 0;
        decimal SumaaCta = 0;
        decimal SumaSaldo = 0;

        private bool Cargando = false;

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
                        CargaDatosDummie();
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

        private static List<PCNVAP> MaestroNiveles
        {
            get
            {
                List<PCNVAP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_MAE_NIVELES_AUTO] as List<PCNVAP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    MaestroNiveles = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_MAE_NIVELES_AUTO] = value;
            }
        }

        private void AsignaAtributos()
        {
            //txtCodigo.Attributes.Add("onKeyPress", "doClick('" + buscaProvButton.ClientID + "',event)");
            //txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            //descargar2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_GUIA + "')){ return false; };";

            eliminarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ELIMINAR_REGISTRO + "')){ return false; };";
            //abrir2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ABRIR_GUIA + "')){ return false; };";
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(exportarButton);

            //observTextBox.Attributes.Add("readonly", "readonly");
        }

        private static List<PCNVAP> ObtieneCabeceraDefault()
        {
            List<PCNVAP> datos = new List<PCNVAP>();
            //datos.Add(new PCNVAP() { NVAPIDNI = -1, NVAPDESC = "" });
            return datos;
        }

        private void CargaDatosDummie()
        {
            CargaNivelesAprobacion();
        }

        private void CargaNivelesAprobacion()
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_NIVELES_APROBACION;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();

                parEnt.Add(tipconDropDownList.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCNVAP> datos = FuncionesUtil.Deserialize<List<PCNVAP>>(resultado.VALSAL[1]);
                        SetDataSource(datos);
                    }
                    else
                    {
                        SetDataSource(null);
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

        //private void SetDataSource2(List<PCNVAP> _listaPlanes)
        //{
        //    if (_listaPlanes == null)
        //    {
        //        _listaPlanes = ObtieneCabeceraDefault();
        //    }
        //    TreeView1.Nodes.Clear();
        //    MaestroPlanes = _listaPlanes;
        //    Dictionary<decimal, TreeNode> nodos = new Dictionary<decimal, System.Web.UI.WebControls.TreeNode>();
        //    foreach (PCNVAP item in _listaPlanes)
        //    {
        //        TreeNode child = new TreeNode
        //        {
        //            Text = item.NVAPDESC,
        //            Value = item.NVAPIDNI.ToString()
        //        };
        //        if (item.NVAPIDNP == 0)
        //        {
        //            TreeView1.Nodes.Add(child);
        //            nodos.Add(item.NVAPIDNI, child);
        //        }
        //        else
        //        {
        //            if (nodos.ContainsKey(item.NVAPIDNP))
        //            {
        //                nodos[item.NVAPIDNP].ChildNodes.Add(child);
        //                nodos.Add(item.NVAPIDNI, child);
        //            }
        //            else
        //            {
        //                //crear una nueva coleccion para huerfanos y recrrer al final? no deberia darse por el orden
        //            }
        //        }
        //    }
        //}

        private void SetDataSource(List<PCNVAP> _listaPlanes)
        {
            if (_listaPlanes == null)
            {
                _listaPlanes = ObtieneCabeceraDefault();
            }
            TreeView1.Nodes.Clear();
            MaestroNiveles = _listaPlanes;
            Dictionary<decimal, TreeNode> nodos = new Dictionary<decimal, System.Web.UI.WebControls.TreeNode>();
            TreeNode root = new TreeNode
            {
                Text = Constantes.ROOT_LOCALIZACION_INCATOPS,
                Value = Convert.ToString(0)
            };
            TreeView1.Nodes.Add(root);

            foreach (PCNVAP item in _listaPlanes)
            {
                TreeNode child = new TreeNode
                {
                    Text = item.NVAPDESC,
                    Value = item.NVAPIDNI.ToString()
                };
                if (item.NVAPIDNP == 0)
                {
                    TreeView1.Nodes[0].ChildNodes.Add(child);
                    nodos.Add(item.NVAPIDNI, child);
                }
                else
                {
                    if (nodos.ContainsKey(item.NVAPIDNP))
                    {
                        nodos[item.NVAPIDNP].ChildNodes.Add(child);
                        nodos.Add(item.NVAPIDNI, child);
                    }
                    else
                    {
                        //crear una nueva coleccion para huerfanos y recrrer al final? no deberia darse por el orden
                    }
                }
            }
            TreeView1.ExpandAll();
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

        private void MuestraDatos()
        {
            bool habilitaAgrLin = false; 
            TreeNode row = TreeView1.SelectedNode;
            modificarButton.Enabled = false;
            agregarButton.Enabled = false;
            eliminarButton.Enabled = false;

            idnivHiddenField.Value = "0";
            tipconHiddenField.Value = tipconDropDownList.SelectedValue; 
            if (row != null)
            {
                agregarButton.Enabled = true;

                string corrLiq = row.Value;
                //LENAR TABPAGES
                List<PCNVAP> detLocal = MaestroNiveles;

                if (detLocal != null && detLocal[0].NVAPIDNI != -1)
                {
                    PCNVAP maq = detLocal.Find(x => x.NVAPIDNI == Convert.ToDecimal(corrLiq));
                    if (maq != null)
                    {
                        idnivHiddenField.Value = maq.NVAPIDNI.ToString();
                        tipconHiddenField.Value = maq.NVAPTICO.ToString();

                        idparHiddenField.Value = maq.NVAPIDNP.ToString();
                        modificarButton.Enabled = true;
                        eliminarButton.Enabled = true;
                        habilitaAgrLin = true;
                    }
                }
            }
            else
            {
                idnivHiddenField.Value = "";
                idparHiddenField.Value = "";
            }

            CargaLineasAsignadas(habilitaAgrLin);
            CargaRolesAsignados(habilitaAgrLin);
        }

        //protected void cabeceraGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(cabeceraGridView, "Select$" + e.Row.RowIndex);
        //        e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
        //    }
        //}

        private bool ValidaDatos(out string mensajeValidacion)
        {
            DateTime fecha;
            bool resultado;
            resultado = true;
            mensajeValidacion = "";
            //if (int.TryParse(numeroGuiaTextBox.Text, out nrodoc))
            //{
            //    numeroGuiaTextBox.Text = Convert.ToString(nrodoc).PadLeft(Constantes.NUMERO_DIGITOS_NUMERO_DOCUMENTO, '0');
            //}
            //else
            //{
            //    mensajeValidacion = Mensajes.INGRESE_NUMERO_DOCUMENTO_VALIDO;
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(numeroGuiaTextBox.Text))
            //{
            //    mensajeValidacion = Mensajes.INGRESE_NUMERO_DOCUMENTO_VALIDO;
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(serieGuiaTextBox.Text))
            //{
            //    mensajeValidacion = Mensajes.INGRESE_SERIE_DOCUMENTO;
            //    return false;
            //}

            //if (!Funciones.TryParseDate(fechaTextBox.Text, Constantes.FORMATO_FECHA, out fecha) )
            //{
            //    mensajeValidacion = Mensajes.INGRESE_FECHA_CORRECTA;
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(codTransHiddenField.Value))
            //{
            //    mensajeValidacion = Mensajes.SELECCIONE_TRASPORTISTA;
            //    return false;
            //}

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
            //ddlEdit.ClearSelection();
            //ddlAdd.ClearSelection();
            //mpe1.Hide();
            if (cerrar)
            {
                mpe2.Hide();
                //panelEdit.Style["display"] = "none";
            }
            //mpe3.Hide();

        }

        private void MostrarMensaje(string mensaje, bool noalert = false)
        {
            if (!noalert)
            {
                ScriptManager.RegisterStartupScript(up, up.GetType(), "myAlert", "alert('" + mensaje.Replace("<br>", " - " ) + "');", true);
            }
            //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(mensaje );
        }

        private void habilitaNavegacion()
        {

        }

        protected void modificarButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode row = TreeView1.SelectedNode;
                if (row != null)
                {
                    string corrLiq = row.Value;
                    //LENAR TABPAGES
                    List<PCNVAP> detPlan = MaestroNiveles;

                    if (detPlan != null && detPlan[0].NVAPIDNI != -1)
                    {
                        PCNVAP maq = detPlan.Find(x => x.NVAPIDNI == Convert.ToDecimal(corrLiq));
                        if (maq != null)
                        {
                            idnivHiddenField.Value = maq.NVAPIDNI.ToString();
                            idparHiddenField.Value = maq.NVAPIDNP.ToString();

                            desniveditTextBox.Text = maq.NVAPDESC;

                            //if (regimeeditRadioButtonList.Items.FindByValue(maq.LOINREGI) != null)
                            //{
                            //    regimeeditRadioButtonList.SelectedValue = maq.LOINREGI;
                            //}
                            ////descripcionactLabel.Text = maq.LOINDES1;
                            //if (unidadeditDropDownList.Items.FindByValue(maq.LOINCTUL) != null)
                            //{
                            //    unidadeditDropDownList.SelectedValue = maq.LOINCTUL;
                            //}

                            tituloeditLiteral.Text = Mensajes.TITULO_MODIFICAR_NIVEL;
                            nivpadeditTextBox.Text = "";
                            if (row.Parent != null)
                            {
                                nivpadeditTextBox.Text = row.Parent.Text;
                            }
                            mpe2.Show();
                        }
                        else
                        {
                            MostrarMensaje(Mensajes.SELECCIONE_NIVEL_MODIFICAR);
                            modificarButton.Enabled = false;
                        }
                    }
                    else
                    {
                        modificarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_NIVEL_MODIFICAR);
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
                if (GuardaNivelAprobacion())
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaNivelesAprobacion();
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaNivelAprobacion()
        {
            IappServiceClient clt = null;
            bool resguardar = false;
            try
            {
                RESOPE resultado;
                PCNVAP maq = new PCNVAP();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_NIVEL_APROBACION;
                //asigna parametros entrada en orden
                if (string.IsNullOrWhiteSpace(idnivHiddenField.Value))
                {
                    maq.NVAPIDNI = -1;
                }
                else
                {
                    maq.NVAPIDNI = Convert.ToDecimal(idnivHiddenField.Value);
                }
                maq.NVAPTICO = decimal.Parse(tipconHiddenField.Value);
                if (string.IsNullOrWhiteSpace(idparHiddenField.Value))
                {
                    maq.NVAPIDNP = 0;
                }
                else
                {
                    maq.NVAPIDNP = Convert.ToDecimal(idparHiddenField.Value);
                }

                maq.NVAPDESC = desniveditTextBox.Text.ToUpper();

                maq.NVAPUSMD = _ParametrosIni.Usuario; 

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

        protected void agregarButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode row = TreeView1.SelectedNode;

                tituloeditLiteral.Text = Mensajes.TITULO_AGREGAR_NIVEL;
                nivpadeditTextBox.Text = "";
                idnivHiddenField.Value = "";
                idparHiddenField.Value = "";

                if (row != null)
                {
                    idparHiddenField.Value = row.Value;
                    nivpadeditTextBox.Text = row.Text;
                }

                desniveditTextBox.Text = "";

                mpe2.Show();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
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

        protected void eliminarButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (EliminaNivelAprobacion())
                {
                    CargaNivelesAprobacion();
                    MuestraDatos();
                }               
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool EliminaNivelAprobacion()
        {
            IappServiceClient clt = null;
            bool resguardar = false;
            TreeNode row = TreeView1.SelectedNode;
            if (row.ChildNodes.Count > 0)
            {
                MostrarMensaje(Mensajes.MENSAJE_NIVEL_HIJOS);
                return resguardar;
            }
            try
            {

                RESOPE resultado;
                PCNVAP maq = new PCNVAP();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.ELIMINA_NIVEL_APROBACION;
                //asigna parametros entrada en orden
                if (!string.IsNullOrWhiteSpace(idnivHiddenField.Value))
                {
                    List<string> parEnt = new List<string>();
                    parEnt.Add(idnivHiddenField.Value);

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

        protected void tipconDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                CargaNivelesAprobacion();
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        #region lineas por Nivel

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
                List<PRLIPR> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_LINEAS_NIVELES_AUT] as List<PRLIPR>;
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
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_LINEAS_NIVELES_AUT] = value;
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
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LINEAS_NIVEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idnivHiddenField.Value);
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
                TreeNode row = TreeView1.SelectedNode;
                if (row != null)
                {
                    idnivHiddenField.Value = row.Value;
                    desnivapreditTextBox.Text = row.Text;
                    LimpiaDatosLinea();
                    lineaeditModalPopupExtender.Show();
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
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
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
                argumentos.CODOPE = CodigoOperacion.ASIGNA_LINEA_NIVEL;
                //asigna parametros entrada en orden

                List<string> parEnt = new List<string>();
                parEnt.Add(idnivHiddenField.Value);
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
                        argumentos.CODOPE = CodigoOperacion.REMUEVE_LINEA_NIVEL;
                        //asigna parametros entrada en orden
                            List<string> parEnt = new List<string>();
                            parEnt.Add(idnivHiddenField.Value);
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

        #region roles por Nivel

        protected void rolesGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        protected void rolesGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.Cells[0].Text.Equals("-1"))
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(rolesGridView, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";

                    //DateTime fecha;
                    //DataRow item = ((System.Data.DataRowView)e.Row.DataItem).Row;
                    //    e.Row.Cells[3].Text = fecha.ToString(Constantes.FORMATO_FECHA);
                    //e.Row.Cells[5].Text = Convert.ToDecimal(item["CCPVTA"]).ToString(Constantes.FORMATO_IMPORTE);
                }
            }
        }

        protected void rolesGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //habilita botones
                HabilitaEdicionRol();

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

        protected void rolesGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                if (rolesGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = rolesGridView.Rows[rolesGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = rolesGridView.Rows[e.NewSelectedIndex];

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

        private void HabilitaEdicionRol()
        {
            GridViewRow row = rolesGridView.SelectedRow;
            removerRolButton.Enabled = false;

            if (row != null)
            {
                string corrLiq = row.Cells[0].Text;
                //LENAR TABPAGES
                List<GRROAP> detPlan = MaestroRoles;

                if (detPlan != null && detPlan[0].GRUSCOGR != "")
                {
                    //MTACTI maq = detPlan.Find(x => x.ACTIIDAC == Convert.ToDecimal(corrLiq));
                    removerRolButton.Enabled = true;
                }
            }
        }

        private List<GRROAP> ObtieneRolesDefault()
        {
            List<GRROAP> datos = new List<GRROAP>();
            datos.Add(new GRROAP() { GRUSCOGR = "", GRUSDEGR = "" });
            return datos;
        }
        private List<GRROAP> MaestroRoles
        {
            get
            {
                List<GRROAP> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_NIVELES_AUT] as List<GRROAP>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneRolesDefault();

                    MaestroRoles = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_ROLES_NIVELES_AUT] = value;
            }
        }

        private void CargaRolesAsignados(bool habilitaAgrLin = true)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ROLES_NIVEL;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(idnivHiddenField.Value);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    List<GRROAP> datos = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[0]);
                    List<GRROAP> linnoasi = FuncionesUtil.Deserialize<List<GRROAP>>(resultado.VALSAL[1]);

                    MuestraRolesAsignadas(datos);

                    var firstitem = roleditDropDownList.Items[0];
                    roleditDropDownList.Items.Clear();
                    roleditDropDownList.Items.Add(firstitem);
                    foreach (GRROAP item in linnoasi)
                    {
                        roleditDropDownList.Items.Add(new ListItem(item.GRUSDEGR, item.GRUSCOGR.ToString()));
                    }
                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                }
                agregarRolButton.Enabled = habilitaAgrLin;
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

        private void MuestraRolesAsignadas(List<GRROAP> _listaPlanes)
        {
            if (_listaPlanes == null || _listaPlanes.Count == 0)
            {
                _listaPlanes = ObtieneRolesDefault();
            }

            MaestroRoles = _listaPlanes;
            rolesGridView.DataSource = MaestroRoles;
            rolesGridView.DataBind();
        }


        private void LimpiaDatosRol()
        {
            roleditDropDownList.SelectedValue = Constantes.CODIGO_LISTA_SELECCIONE;
        }

        protected void agregarRolButton_Click(object sender, EventArgs e)
        {
            try
            {
                tituloRolEditLiteral.Text = Mensajes.TITULO_AGREGAR_ROL;
                TreeNode row = TreeView1.SelectedNode;
                if (row != null)
                {
                    idnivHiddenField.Value = row.Value;
                    desnivaprReditLabel.Text = row.Text;
                    LimpiaDatosRol();
                    roleditModalPopupExtender.Show();
                }
                else
                {
                    agregarRolButton.Enabled = false;
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

        protected void removerRolButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (RemueveRol())
                {
                    CargaRolesAsignados();
                }

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        //PENDIENTE ADD VALIDACIONES: FRECUENCIA > 0, NUMEROS FRECUENCIA, DURACIONES DIAS HORAS MINUTOS, DIAS PARO
        protected void aceptareditRolButton_Click(object sender, EventArgs e)
        {
            try
            {
                string mensajevalida;
                if (!Page.IsValid)
                {
                    up.Update();
                    roleditModalPopupExtender.Show();
                    return;
                }

                if (GuardaRol())
                {
                    CargaRolesAsignados();
                }
                else
                {
                    roleditModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {
                errorgrupoLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
            finally
            {
            }
        }

        private bool GuardaRol()
        {
            IappServiceClient clt = null;
            bool resguardar = false;
            try
            {

                RESOPE resultado;
                GRROAP maq = new GRROAP();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.ASIGNA_ROL_NIVEL;
                //asigna parametros entrada en orden

                List<string> parEnt = new List<string>();
                parEnt.Add(idnivHiddenField.Value);
                parEnt.Add(roleditDropDownList.SelectedValue);
                parEnt.Add(_ParametrosIni.Usuario);

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

        private bool RemueveRol()
        {
            IappServiceClient clt = null;
            bool resguardar = false;

            try
            {
                GridViewRow row = rolesGridView.SelectedRow;
                if (row != null)
                {
                    string corrLiq = row.Cells[0].Text;
                    if (!string.IsNullOrWhiteSpace(corrLiq) && !corrLiq.Equals(""))
                    {
                        RESOPE resultado;
                        MTLOIN maq = new MTLOIN();
                        clt = _ParametrosIni.IniciaNuevoCliente();
                        //codigo de operacion
                        PAROPE argumentos = new PAROPE();
                        argumentos.CODOPE = CodigoOperacion.REMUEVE_ROL_NIVEL;
                        //asigna parametros entrada en orden
                        List<string> parEnt = new List<string>();
                        parEnt.Add(idnivHiddenField.Value);
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
                        removerRolButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_ROL_REMOVER);
                    removerRolButton.Enabled = false;
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