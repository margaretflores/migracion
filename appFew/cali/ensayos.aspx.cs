using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using appFew.appServicio;
using appConstantes;
using appWcfService;
using System.Web.Services;

namespace appFew.cali
{
    public partial class ensayos : System.Web.UI.Page
    {
        private static ParametrosFe _ParametrosIni;

        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        public List<PCVRPR> variablesasig = new List<PCVRPR>();

        protected void Page_Load(object sender, EventArgs e)
        {
            _ParametrosIni = (ParametrosFe)Session["ParametrosFe"];

            if (Session["ParametrosFe"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            else
            {
                try
                {
                    if (!IsPostBack)
                    {
                        CargarLineaProd();
                    }
                }
                catch (Exception ex)
                {
                    string Error_2 = "";
                    Error_2 = ex.Message.Replace(Environment.NewLine, "_");

                    string Error_1 = "Ha ocurrido un error en la pagina.";
                    string url = "..//ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + Error_2;
                    Response.Redirect(url);
                }
            }
        }

        private void CargarLineaProd()  //bool soloOrigen = false)
        {
            IappServiceClient clt = _ParametrosIni.IniciaNuevoCliente();
            try
            {
                RESOPE resultado;
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_LINEAS_PRODUCCION;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add("0");
                argumentos.VALENT = parEnt.ToArray();

                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = lineaDropDownList.Items[0];
                        lineaDropDownList.Items.Clear();
                        lineaDropDownList.Items.Add(firstitem);
                        List<PRLIPR> datos = FuncionesUtil.Deserialize<List<PRLIPR>>(resultado.VALSAL[1]);
                        foreach (PRLIPR item in datos)
                        {
                            lineaDropDownList.Items.Add(new ListItem(item.LIPRDESC, item.LIPRCODI.ToString()));
                        }
                    }
                }
                else
                {
                    //ErrorLabel.Text = resultado.MENERR;
                    //MostrarMensaje(resultado.MENERR);

                }

            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
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
                if (lineaDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    var firstitem = DropDownListProceso.Items[0];
                    DropDownListProceso.Items.Clear();
                    DropDownListProceso.Items.Add(firstitem);
                    return;
                }
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();

                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_TIPOS_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(lineaDropDownList.SelectedValue);
                parEnt.Add(Convert.ToString(Constantes.REGISTROS_TODOS));
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        var firstitem = DropDownListProceso.Items[0];
                        DropDownListProceso.Items.Clear();
                        DropDownListProceso.Items.Add(firstitem);

                        List<PRTTPR> datos = FuncionesUtil.Deserialize<List<PRTTPR>>(resultado.VALSAL[1]);
                        foreach (PRTTPR item in datos)
                        {
                            DropDownListProceso.Items.Add(new ListItem(item.TTPRDESC, item.TTPRCODI.ToString()));
                        }
                    }

                }
                else
                {
                   // MostrarMensaje(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
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
                var firstitem = DropDownListMaquinas.Items[0];
                DropDownListMaquinas.Items.Clear();
                DropDownListMaquinas.Items.Add(firstitem);
                if (DropDownListProceso.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
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
                parEnt.Add(lineaDropDownList.SelectedValue);
                parEnt.Add(DropDownListProceso.SelectedValue);
                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);
                if (resultado.ESTOPE)
                {
                    if (resultado.VALSAL[0].Equals("1")) //encontrado
                    {
                        List<PCMQPR> datos = FuncionesUtil.Deserialize<List<PCMQPR>>(resultado.VALSAL[1]);
                        foreach (PCMQPR item in datos)
                        {
                            DropDownListMaquinas.Items.Add(new ListItem(item.MAQUCOMA + " " + item.MAQUDES2 + " " + item.MAQUMARC, item.MAQUIDMA.ToString()));
                        }
                    }

                }
                else
                {
                    //MostrarMensaje(resultado.MENERR);
                }
            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
        }

        private bool CargaVariables()
        {
            bool resultadoOpe;
            DateTime fecha;
            IappServiceClient clt = null;
            resultadoOpe = false;
            fecha = DateTime.Today;
            try
            {
                if (DropDownListProceso.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE))
                {
                    //cancelarButton_Click(cancelarButton, new EventArgs());
                    Session["variables"] = null;
                    return false;
                }
                
                List<columns> colgrilla = new List<columns>
                {
                    new columns(){ datafield = "", text="#",width ="30",editable =false }
                };
                List<fieldsgrilla> fieldgrilla = new List<fieldsgrilla>();
                //{
                //    new fieldsgrilla(){ name = "id", type = "number"  }
                //};
               
                RESOPE resultado;
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_VARIABLES_PROCESO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(lineaDropDownList.SelectedValue);
                //parEnt.Add(tipproDropDownList.SelectedValue.Equals(Constantes.CODIGO_LISTA_SELECCIONE) ? "" : tipproDropDownList.SelectedValue);
                parEnt.Add(DropDownListProceso.SelectedValue);
                parEnt.Add(DropDownListMaquinas.SelectedValue);

                argumentos.VALENT = parEnt.ToArray();
                resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {
                    
                    variablesasig = FuncionesUtil.Deserialize<List<PCVRPR>>(resultado.VALSAL[1]);

                    foreach (var it in variablesasig)
                    {
                        colgrilla.Add(new columns() { datafield = it.COLGRILLA.ToString(), text = it.MVARNOMB, width = "250", editable = true });
                        fieldgrilla.Add(new fieldsgrilla() { name = it.COLGRILLA.ToString(), type = "number" });
                    }

                    colgrilla.Add(new columns() { datafield = "Quitar", text = "Quitar", width = "100", editable = false, columntype = "button" });

                    object[] sd = new object[2];

                    sd[0] = colgrilla;
                    sd[1] = fieldgrilla;

                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(variablesasig);
                    var jsoncolgrilla = jsonSerialiser.Serialize(colgrilla);
                    var jsonfieldgrilla = jsonSerialiser.Serialize(fieldgrilla);

                    Session["variables"] = json;
                    Session["colgrilla"] = jsoncolgrilla;
                    Session["fieldgrilla"] = jsonfieldgrilla;
                    ClientScript.RegisterStartupScript(GetType(), "ejecutar", "pedidoApp.combovariables();", true);

                    //ScriptManager.RegisterStartupScript(GetType(), "ejecutar", "_combovariables", true);

                }
                else
                {
                    //ErrorLabel.Text = resultado.MENERR;
                    //MostrarMensaje(resultado.MENERR);
                }

            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = _ParametrosIni.ErrorGenerico(ex.Message);
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultadoOpe;
        }


        protected void lineaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaTiposProceso();
            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void DropDownListProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaMaquinasProceso();
                CargaVariables();
            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }

        protected void DropDownListMaquinas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargaVariables();
            }
            catch (Exception ex)
            {
                //MostrarMensaje(_ParametrosIni.ErrorGenerico(ex.Message));
            }
        }


        [WebMethod]
        public static RESOPE buscarensayo(string partida, string nroensayo, string liprod)
        {
            IappServiceClient clt = _ParametrosIni.IniciaNuevoCliente();
            RESOPE resultado = new RESOPE() { ESTOPE = false };
            try
            {
                clt = _ParametrosIni.IniciaNuevoCliente();
                //codigo de operacion
                PAROPE argumentos = new PAROPE();
                argumentos.CODOPE = CodigoOperacion.OBTIENE_ENSAYO;
                //asigna parametros entrada en orden
                List<string> parEnt = new List<string>();
                parEnt.Add(partida);
                parEnt.Add(nroensayo);
                parEnt.Add(liprod);

                argumentos.VALENT = parEnt.ToArray();
                //resultado = clt.EjecutaOperacion(argumentos);

                if (resultado.ESTOPE)
                {

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultado;
        }


        [WebMethod]
        public static RESOPE llenargrilla(string linea, string proceso, string maquina)
        {
            IappServiceClient clt = _ParametrosIni.IniciaNuevoCliente();
            RESOPE resultado = new RESOPE() { ESTOPE = false };
            try
            {
                //List<PCVRPR>
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _ParametrosIni.FinalizaCliente(clt);
            }
            return resultado;
        }
    }
}