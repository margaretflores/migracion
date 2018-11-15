using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.IO;

using appFew.appServicio;
using appConstantes;
using appFew;

namespace appFew.reportes
{
    public partial class formatodoc2 : System.Web.UI.Page
    {
        private ParametrosFe _ParametrosIni;
        private string Error_1 = string.Empty;
        private string Error_2 = string.Empty;
        private string url = string.Empty;

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
                    if (!Page.IsPostBack)
                    {
                        CargaDatos();
                    }
                }
                catch (Exception ex)
                {
                    Error_1 = "Ha ocurrido un error en la pagina.";
                    url = "ErrorPage.aspx?Error_1=" + Error_1 + "&Error_2=" + Error_2;
                    Response.Redirect(url);
                }
            }
        }

        private void CargaDatos()
        {
            //List<ImpresionLiquidacion> datos = null;
            //datos = Session["Datos"] as List<ImpresionLiquidacion>;
            ImpresionDatos datos = null;
            datos = Session[Constantes.NOMBRE_SESION_DATOS_PDB] as ImpresionDatos;
            string nombreArchivo;
            if (datos != null)
            {
                nombreArchivo = datos.NombreArchivo;
                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    nombreArchivo = System.Guid.NewGuid().ToString();
                }
                switch (datos.Destino)
                {
                    case ImpresionDatos.ADestino.APDF:
                        ExportToPDF(nombreArchivo, datos.Datos, datos.Path, datos.NombreDataSet);
                        break;
                    case ImpresionDatos.ADestino.AExcel:
                        ExportToExcel(nombreArchivo, datos.Datos, datos.Path, datos.NombreDataSet);
                        break;
                    default:
                        CargaReporte(datos.Datos, datos.Path, datos.NombreDataSet);

                        break;
                }
            }
        }

        private void CargaReporte(object datos, string path, string nombreDataSet)
        {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath(path); //  "~/reportes/FormatoLiquidacion.rdlc");
                ReportDataSource rds = new ReportDataSource(nombreDataSet, datos); //   "DataSetLiquidacion", datos);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);

                ReportViewer1.LocalReport.Refresh();
        }

        //public void ExportToPDF(string fileName, List<ImpresionLiquidacion> datos)
        public void ExportToPDF(string fileName, object datos, string path, string nombreDataSet)
        {

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.ServerReport.ReportServerUrl = new Uri("http://localhost/w004cac");
            //viewer.ServerReport.ReportPath = "/" + Server.MapPath("~/reportes/FormatoLiquidacion.rdlc"); ;
            //viewer.ServerReport.SetParameters(reportParams);
            viewer.LocalReport.ReportPath = Server.MapPath(path); //"~/reportes/FormatoLiquidacion.rdlc");
            ReportDataSource rds = new ReportDataSource(nombreDataSet, datos); //"DataSetLiquidacion", datos);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rds);
            //viewer.LocalReport.Refresh();


            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download

        }

        public void ExportToExcel(string fileName, object datos, string path, string nombreDataSet)
        {

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.ServerReport.ReportServerUrl = new Uri("http://localhost/w004cac");
            //viewer.ServerReport.ReportPath = "/" + Server.MapPath("~/reportes/FormatoLiquidacion.rdlc"); ;
            //viewer.ServerReport.SetParameters(reportParams);
            viewer.LocalReport.ReportPath = Server.MapPath(path); //"~/reportes/FormatoLiquidacion.rdlc");
            ReportDataSource rds = new ReportDataSource(nombreDataSet, datos); //"DataSetLiquidacion", datos);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(rds);
            //viewer.LocalReport.Refresh();


            byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
            //Response.End(); 
        }
    }
}