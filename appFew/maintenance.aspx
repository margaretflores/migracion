<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="maintenance.aspx.cs" Inherits="appFew.maintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sistema en mantenimiento - INCA TOPS</title>

    <link href="~/Content/swagg.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div id="div_logo_empresa" class="logo_empresa" runat="server">
                <img alt="img" src='<%= ResolveUrl("~/Images/logo.jpg") %>' />
        </div>
  <div style="padding:20px;" >
    <br />
    <h1 style="color:#660000;">
        Sistema en mantenimiento</h1>
    <br />
    Estamos realizando trabajos en el sistema en estos momentos. 
    <br />
    <br />
    Por favor regrese más tarde, disculpe los inconvenientes.
  </div>
    </form>
</body>
</html>
