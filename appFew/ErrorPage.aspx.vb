Imports System.Threading

Partial Class PageError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-PE", False)
            Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("es", False)

            lblError_1.Text = HttpUtility.HtmlEncode(Request.QueryString("Error_1").ToString())
            lblError_2.Text = Request.QueryString("Error_2").ToString()
        Catch ex As Exception
        End Try
    End Sub
End Class
