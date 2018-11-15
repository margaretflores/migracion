<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ErrorPage.aspx.vb" Inherits="PageError" 
MasterPageFile="~/Site.Master" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--<html xmlns="http://www.w3.org/1999/xhtml" >--%>
<%--<head id="Head1" runat="server">
    <title>Ha ocurrido un error en la pagina</title>
    <style type="text/css">
        .style1 {
	        font-family: Tahoma;
	        font-size: large;
        }
</style>
</head>
--%>
<script  type="text/jscript" language="javascript">

//***Deshabilitar el teclado
function detectKey(e)
{
    try
    {
        if (!e)
            e = window.event;
            
        if (e)
        {
            e.cancelBubble = true;
            e.returnValue = false;
            e.keyCode = false; 
            if (e.stopPropagation)
            {
                e.stopPropagation()
            };
            return false;
        }
    }
    catch (err)
    {
        return false;
    }
}

document.onhelp = new Function("return false;");
window.onhelp = new Function("return false;");

document.onkeydown = detectKey;
//***

</script>
<body id="thebody" runat="server" style="margin: 0; color: #0066CC;">
    <%--<form id="form1" runat="server">--%>
    <table align="center">
	    <tr>
		    <td>
		    <img alt="" src="../imagescontent/TtsWincError.gif" width="86" height="79"  /></td>
		    <td class="style1">
                <asp:Label ID="lblError_1" runat="server" Width="409px"></asp:Label>
            </td>
	    </tr>
	    <tr>
	        <td>
	        </td>
	        <td class="style1">
	            <asp:Label ID="lblError_2" runat="server" Width="409px"></asp:Label>
	        </td>
	    </tr>
	    <tr>
	        <td colspan="2"><a href="javascript:history.go(-1)"><img src="../imagescontent/volver.jpg" alt="Volver" border=0/></a></td>
	    </tr>
    </table>
    <%--</form>--%>
</body>
<%--</html>--%>
</asp:Content>