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
    public partial class maquinasfantasia : System.Web.UI.Page
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

        private static List<PPMQFA> ArticuloMaquina
        {
            get
            {
                List<PPMQFA> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_ARTICULO_MAQUINAS_FANTASIA] as List<PPMQFA>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneCabeceraDefault();

                    ArticuloMaquina = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_ARTICULO_MAQUINAS_FANTASIA] = value;
            }
        }

        private void AsignaAtributos()
        {
            txtSearch.Attributes.Add("onKeyPress", "doClick('" + buscarButton.ClientID + "',event)");
            
            //btnUpdate.OnClientClick = String.Format("fnClickUpdate('{0}','{1}')", btnUpdate.UniqueID, "");

            //descargarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_LIQUIDACION + "')){ return false; };";
            //descargar2Button.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_DESCARGA_GUIA + "')){ return false; };";

            eliminarButton.Attributes["onclick"] = "if(!confirm('" + Mensajes.CONFIRMACION_ELIMINAR_REGISTRO + "')){ return false; };";

            codmaqeditTextBox.Attributes.Add("onKeyPress", "doClick('" + buscaMaquinaButton.ClientID + "',event)");
            //desarteditTextBox.Attributes.Add("readonly", "readonly");
            desmaqeditTextBox.Attributes.Add("readonly", "readonly");
            codmaqeditTextBox.Attributes.Add("readonly", "readonly");

            agregarButton.Enabled = true;
        }

        private static List<PPMQFA> ObtieneCabeceraDefault()
        {
            List<PPMQFA> datos = new List<PPMQFA>();
            datos.Add(new PPMQFA() { MQFAIDMF = -1, MQFAIDMA = -1, ARTDES = "",  MAQUDES2  = "" });
            return datos;
        }

        private void CargaDatosIniciales()
        {
            CargaArticuloMaquinas(1);
        }

        private void CargaArticuloMaquinas(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ARTICULO_MAQUINA_FANTASIA;
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
                        List<PPMQFA> datos = FuncionesUtil.Deserialize<List<PPMQFA>>(resultado.VALSAL[1]);

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

        private void SetDataSource(List<PPMQFA> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneCabeceraDefault();
            }

            ArticuloMaquina = _listaMaquinas;
            cabeceraGridView.DataSource = ArticuloMaquina;
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
                List<PPMQFA> detMaquina = ArticuloMaquina;

                if (detMaquina != null && detMaquina[0].MQFAIDMF != -1)
                {
                    PPMQFA maq = detMaquina.Find(x => x.MQFAIDMF == Convert.ToDecimal(corrLiq));

                    descripcion3Label.Text = maq.MQFAITEM.Trim(); // +" " + maq.ARTDES.Trim();
                    localizacionLabel.Text = maq.MAQUCOMA.Trim() + " " + maq.MAQUDES2.Trim();

                    idmfanHiddenField.Value = maq.MQFAIDMF.ToString();
                    articuloLabel.Text = maq.MQFAITEM;
                    //descripcionLabel.Text = maq.ARTDES;
                    codigoLabel.Text = maq.MAQUCOMA;
                    maquinaLabel.Text = maq.MAQUDES2.Trim();
                    rangodeLabel.Text = maq.MQFARNDE.ToString();
                    rangoaLabel.Text = maq.MQFARANA.ToString();
                    modificarButton.Enabled = true;
                    eliminarButton.Enabled = true;
                }
            }
            else
            {
                idmfanHiddenField.Value = "";
                articuloLabel.Text = "";
                //descripcionLabel.Text = "";
                codigoLabel.Text = "";
                maquinaLabel.Text = "";
                rangodeLabel.Text = "";
                rangoaLabel.Text = "";
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
                    e.Row.Cells[3].Text = "";
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

            if (!decimal.TryParse(rangdeeditTextBox.Text, out valor))
            {
                mensajeValidacion = string.Format(Mensajes.DATOS_INCORRECTOS, "RANGO DE");
                return false;
            }

            if (!decimal.TryParse(rangoaTextBox.Text, out valor2))
            {
                mensajeValidacion = string.Format(Mensajes.DATOS_INCORRECTOS, "RANGO A");
                return false;
            }

            if (valor >= valor2)
            {
                mensajeValidacion = Mensajes.MENSAJE_RANGO_NO_VALIDO;
                return false;
            }

            if (string.IsNullOrWhiteSpace(articuloeditTextBox.Text))
            {
                mensajeValidacion = Mensajes.INGRESE_ARTICULO;
                return false;
            }

            if (string.IsNullOrWhiteSpace(codmaqeditTextBox.Text))
            {
                mensajeValidacion = Mensajes.INGRESE_MAQUINA;
                return false;
            }

            if (!BuscaArticuloFantasia(out mensajeValidacion))
            {
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

        //protected void buscaArticuloButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(articuloeditTextBox.Text))
        //        {
        //            BuscaArticulo();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
        //        MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
        //    }
        //    mpe2.Show();
        //}

        private bool BuscaArticuloFantasia(out string mensajeerror)
        {
            IappServiceClient clt = _ParametrosIni.IniciaNuevoCliente();
            mensajeerror = "";
            try
            {
                RESOPE resultado;
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.BUSCAR_ARTICULO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                articuloeditTextBox.Text = articuloeditTextBox.Text.ToUpper();
                parEnt.Add(articuloeditTextBox.Text);
                argumentos.VALENT = parEnt.ToArray();

                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    //desarteditTextBox.Text = resultado.VALSAL[0].Trim();
                    //if (!string.IsNullOrWhiteSpace(resultado.MENERR))
                    //{
                    //    ErrorLabelPopup.Text = resultado.MENERR;
                    //}
                    return true;
                }
                else
                {
                    //desarteditTextBox.Text = "";
                    //ErrorLabelPopup.Text = resultado.MENERR;
                    //MostrarMensaje(resultado.MENERR);
                    mensajeerror = resultado.MENERR;
                    return false;
                }
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        protected void buscaMaquinaButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Inicializa valores busqueda?
                CargaMaquinas(1);
                MuestraBusMaq();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        //private void BuscaMaquina()
        //{
        //    IappServiceClient clt = _ParametrosIni.IniciaNuevoCliente();
        //    try
        //    {
        //        RESOPE resultado;
        //        //codigo de operacion
        //        PAROPE argumentos = new PAROPE();
        //        argumentos.CODOPE = CodigoOperacion.BUSCAR_MAQUINA_CODIGO_MANT;
        //        //asigna parametros entrada en orden
        //        List<string> parEnt = new List<string>();
        //        codmaqeditTextBox.Text = codmaqeditTextBox.Text.ToUpper();
        //        parEnt.Add(codmaqeditTextBox.Text);
        //        argumentos.VALENT = parEnt.ToArray();

        //        resultado = clt.EjecutaOperacion(argumentos);
        //        if (resultado.ESTOPE)
        //        {
        //            idmaqHiddenField.Value = resultado.VALSAL[0].Trim();
        //            desmaqeditTextBox.Text = resultado.VALSAL[1].Trim();
        //            //if (!string.IsNullOrWhiteSpace(resultado.MENERR))
        //            //{
        //            //    ErrorLabelPopup.Text = resultado.MENERR;
        //            //}
        //        }
        //        else
        //        {
        //            //End_Block(panelEdit, false); //udpInnerUpdatePanel, false);
        //            desmaqeditTextBox.Text = "";
        //            //codmaqeditTextBox.Text = parEnt[0];
        //            //ErrorLabelPopup.Text = resultado.MENERR;
        //            MostrarMensaje(resultado.MENERR);
        //        }
        //    }
        //    finally
        //    {
        //        _ParametrosIni.FinalizaCliente(clt);
        //    }
        //}

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
                SetDataSource(ArticuloMaquina);
            }
            catch 
            { 
            }
        }

        protected void buscarButton_Click(object sender, EventArgs e)
        {
            CargaArticuloMaquinas(1);
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
                    CargaArticuloMaquinas(indicepag);
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
                tituloeditLiteral.Text = Mensajes.TITULO_AGREGAR_ARTICULO_MAQUINA_FANTASIA;
                idmfanHiddenField.Value = "";
                idmaqHiddenField.Value = "";
                articuloeditTextBox.Text = "";
                //desarteditTextBox.Text = "";
                codmaqeditTextBox.Text = "";
                desmaqeditTextBox.Text = "";

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
                    List<PPMQFA> detMaquina = ArticuloMaquina;

                    if (detMaquina != null && detMaquina[0].MQFAIDMF != -1)
                    {
                        PPMQFA maq = detMaquina.Find(x => x.MQFAIDMF == Convert.ToDecimal(corrLiq));
                        idmfanHiddenField.Value = maq.MQFAIDMF.ToString();
                        articuloeditTextBox.Text = maq.MQFAITEM.Trim();
                        //desarteditTextBox.Text = maq.ARTDES;
                        idmaqHiddenField.Value = maq.MQFAIDMA.ToString();
                        codmaqeditTextBox.Text = maq.MAQUCOMA;
                        desmaqeditTextBox.Text = maq.MAQUDES2;

                        rangdeeditTextBox.Text = maq.MQFARNDE.ToString(Constantes.FORMATO_DECIMAL_0 + "1" );
                        rangoaTextBox.Text = maq.MQFARANA.ToString(Constantes.FORMATO_DECIMAL_0 + "1");

                        tituloeditLiteral.Text = Mensajes.TITULO_MODIFICAR_ARTICULO_MAQUINA_FANTASIA; 
 
                        mpe2.Show();
                    }
                    else
                    {
                        modificarButton.Enabled = false;
                    }
                }
                else
                {
                    MostrarMensaje(Mensajes.SELECCIONE_ARTICULO_MAQUINA_MODIFICAR);
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
                if (GuardaMueble())
                {
                    End_Block(panelEdit); //udpInnerUpdatePanel);
                    CargaArticuloMaquinas(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                ErrorLabelPopup.Text = _ParametrosIni.ErrorGenerico(ex.Message);
            }
        }

        private bool GuardaMueble()
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
                PPMQFA maq = new PPMQFA();
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.GUARDA_ARTICULO_MAQUINA_FANTASIA;
                //asigna parametros entrada en orden
                //asigna parametros entrada en orden
                if (string.IsNullOrWhiteSpace(idmfanHiddenField.Value))
                {
                    maq.MQFAIDMF = -1;
                }
                else
                {
                    maq.MQFAIDMF = Convert.ToDecimal(idmfanHiddenField.Value);
                }
                maq.MQFAITEM = articuloeditTextBox.Text;
                maq.MQFAIDMA = Convert.ToDecimal(idmaqHiddenField.Value);
                maq.MQFARNDE = decimal.Parse(rangdeeditTextBox.Text);
                maq.MQFARANA = decimal.Parse(rangoaTextBox.Text);

                maq.MQFAUSMD = _ParametrosIni.Usuario; 

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
                if (EliminaPlan())
                {
                    CargaArticuloMaquinas(Convert.ToInt32(pageIndexHiddenField.Value));
                    MuestraDatos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        private bool EliminaPlan()
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
                        argumentos.CODOPE = CodigoOperacion.ELIMINA_ARTICULO_MAQUINA_FANTASIA;
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
                    MostrarMensaje(Mensajes.SELECCIONE_ARTICULO_MAQUINA_ELIMINAR);
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

        ////////////////////////////
        ////////////BUSQUEDA MAQUINA
        #region BUSQUEDA MAQUINA

        protected void maquinasGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    e.Row.Cells[0].ColumnSpan = 5;
            //    //now make up for the colspan from cell2
            //    e.Row.Cells.RemoveAt(1);
            //    e.Row.Cells.RemoveAt(2);
            //    e.Row.Cells.RemoveAt(3);
            //    e.Row.Cells.RemoveAt(4);
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            //}
            //else 
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#FF9999'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


            }
        }

        protected void maquinasGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(maquinasGridView, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";

                //Find the TextBox control.
                //CheckBox selectCheckBox = (e.Row.FindControl("checkBoxProv") as CheckBox);
                //if (selectCheckBox != null && selectCheckBox.Checked)
                //{
                //    (e.Row.DataItem as DataRowView)["DCCAREAS"] = true;
                //}
                //else
                //{
                //    (e.Row.DataItem as DataRowView)["DCCAREAS"] = false;
                //}
                //Find the DropDownList control.
                //DropDownList ddlCountries = (e.Row.FindControl("ddlCountries") as DropDownList);
                //string country = (e.Row.DataItem as DataRowView)["Country"].ToString();
                //ddlCountries.Items.FindByValue(country).Selected = true;
            }
        }

        protected void maquinasGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                MuestraBusMaq();
                if (maquinasGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = cabeceraGridView.Rows[maquinasGridView.SelectedIndex];
                    //nrow.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    previorow.ToolTip = Mensajes.MENSAJE_CLIC_A_SELECCIONAR; //"Click to select this row.";
                    previorow.Attributes.Add("bgColor", "#FF9999");
                }
                GridViewRow nuevorow = maquinasGridView.Rows[e.NewSelectedIndex];

                //brow.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                nuevorow.Attributes.Add("bgColor", "this.originalstyle");
                nuevorow.ToolTip = string.Empty;

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void maquinasGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MuestraBusMaq();
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }


        private void MuestraBusMaq()
        {
            mpe2.Show();
            busqMaquinasModalPopupExtender.Show();
        }

        protected void buscarMaquinasButton_Click(object sender, EventArgs e)
        {
            CargaMaquinas(1);
        }

        protected void primeromaqButton_Click(object sender, EventArgs e)
        {
            navegaMaq(0);
        }

        protected void anteriormaqButton_Click(object sender, EventArgs e)
        {
            navegaMaq(-1);
        }

        protected void siguientemaqButton_Click(object sender, EventArgs e)
        {
            navegaMaq(1);
        }

        protected void ultimomaqButton_Click(object sender, EventArgs e)
        {
            navegaMaq(99);
        }

        private void EstadoFinalBotonesNavegacionMaq()
        {
            int ultimo, total, indicepag;
            total = Convert.ToInt32(totalmaqHiddenField.Value);
            indicepag = Convert.ToInt32(pageIndexmaqHiddenField.Value);
            ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

            anteriormaqButton.Enabled = false;
            primeromaqButton.Enabled = false;
            siguientemaqButton.Enabled = false;
            ultimomaqButton.Enabled = false;
            if (total > 0)
            {
                if (indicepag > 1)
                {
                    anteriormaqButton.Enabled = true;
                    primeromaqButton.Enabled = true;
                }
                if (indicepag < ultimo)
                {
                    siguientemaqButton.Enabled = true;
                    ultimomaqButton.Enabled = true;
                }
            }
        }

        private void navegaMaq(int index)
        {
            try
            {
                MuestraBusMaq();

                int ultimo, total, indicepag;
                total = Convert.ToInt32(totalmaqHiddenField.Value);
                indicepag = Convert.ToInt32(pageIndexmaqHiddenField.Value);
                ultimo = Convert.ToInt32(Math.Ceiling(total / Convert.ToDecimal(PageSize)));

                if (ultimo > 0)
                {
                    anteriormaqButton.Enabled = true;
                    primeromaqButton.Enabled = true;
                    siguientemaqButton.Enabled = true;
                    ultimomaqButton.Enabled = true;
                }
                else
                {
                    anteriormaqButton.Enabled = false;
                    primeromaqButton.Enabled = false;
                    siguientemaqButton.Enabled = false;
                    ultimomaqButton.Enabled = false;
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
                            anteriormaqButton.Enabled = false;
                            primeromaqButton.Enabled = false;
                        }
                        break;
                    case 1: //siguiente
                        if (indicepag < ultimo)
                        {
                            indicepag++;
                        }
                        if (indicepag == ultimo)
                        {
                            siguientemaqButton.Enabled = false;
                            ultimomaqButton.Enabled = false;
                        }
                        break;
                    case 0: //primero
                        indicepag = 1;
                        anteriormaqButton.Enabled = false;
                        primeromaqButton.Enabled = false;

                        break;
                    case 99: //ultimo
                        if (total > 0)
                        {
                            indicepag = ultimo;
                            siguientemaqButton.Enabled = false;
                            ultimomaqButton.Enabled = false;
                        }
                        break;
                }
                if (indicepag != Convert.ToInt32(pageIndexmaqHiddenField.Value))
                {
                    CargaMaquinas(indicepag);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CargaMaquinas(int pageindex)
        {
            IappServiceClient clt = null;

            try
            {
                MuestraBusMaq();

                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.BUSCA_MAQUINAS_FANTASIA;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(buscarMaquinasTextBox.Text.Trim());
                parEnt.Add(pageindex.ToString());
                parEnt.Add(PageSize.ToString());

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<MTMAQU> datos = FuncionesUtil.Deserialize<List<MTMAQU>>(resultado.VALSAL[1]);

                        SetDataSourceMaq(datos);
                        totalmaqHiddenField.Value = resultado.VALSAL[2];
                    }
                    else
                    {
                        SetDataSourceMaq(null);
                        totalmaqHiddenField.Value = "0";
                    }
                    pageIndexmaqHiddenField.Value = pageindex.ToString();
                    totalmaqLabel.Text = " Total: " + totalmaqHiddenField.Value;
                    paginamaqLabel.Text = "Pág: " + pageIndexmaqHiddenField.Value;

                }
                else
                {
                    MostrarMensaje(resultado.MENERR);
                    //ErrorLabel.Font.Bold = true;
                    //ErrorLabel.Text = resultado.MENERR;
                }
                EstadoFinalBotonesNavegacionMaq();
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

        private void SetDataSourceMaq(List<MTMAQU> _listaMaquinas)
        {
            if (_listaMaquinas == null)
            {
                _listaMaquinas = ObtieneMaquinasDefault();
            }

            MaestroMaquinas = _listaMaquinas;
            maquinasGridView.DataSource = MaestroMaquinas;
            maquinasGridView.DataBind();
        }

        private List<MTMAQU> MaestroMaquinas
        {
            get
            {
                List<MTMAQU> dt = HttpContext.Current.Session[Constantes.NOMBRE_SESION_BUS_MAQUINAS] as List<MTMAQU>;
                if (dt == null)
                {
                    // Create a DataTable and save it to session
                    dt = ObtieneMaquinasDefault();

                    MaestroMaquinas = dt;
                }

                return dt;
            }

            set
            {
                HttpContext.Current.Session[Constantes.NOMBRE_SESION_BUS_MAQUINAS] = value;
            }
        }

        private List<MTMAQU> ObtieneMaquinasDefault()
        {
            List<MTMAQU> datos = new List<MTMAQU>();
            datos.Add(new MTMAQU() { MAQUIDMA = -1, MAQUDES2 = "", MAQUMARC = ""});
            return datos;
        }

        protected void aceptarMaquinaButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (maquinasGridView.SelectedIndex != -1)
                {
                    GridViewRow previorow = maquinasGridView.Rows[maquinasGridView.SelectedIndex];
                    AsignaValores(Server.HtmlDecode(previorow.Cells[1].Text).Trim(), Server.HtmlDecode(previorow.Cells[2].Text).Trim(), decimal.Parse(Server.HtmlDecode(previorow.Cells[0].Text).Trim()));
                    mpe2.Show();
                }
                else
                {
                    up.Update();
                    MuestraBusMaq();
                    //errorPopupBusqLoteLabel.Text = Mensajes.MENSAJE_SELECCIONE_REGISTRO_BUSCAR;

                    return;
                }
            }
            catch (Exception ex)
            {
                //errorPopupBusqLoteLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));

            }
        }

        private void AsignaValores(string codigo, string descripcion, decimal idmaq)
        {
            codmaqeditTextBox.Text = codigo;
            desmaqeditTextBox.Text = descripcion;
            idmaqHiddenField.Value = idmaq.ToString();
        }
        #endregion

    }
}