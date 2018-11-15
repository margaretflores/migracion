<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Site.Master" CodeBehind="login.aspx.cs" Inherits="appFew._Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
	                            
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script language="javascript" type="text/javascript">
        javascript: window.history.forward(1);
    </script>
    <script type="text/JavaScript">
<!--
    function elementOnBlur(elementRef) {
        //document.getElementById("MainContent_LoginButton").focus();
    }

    if (document.getElementById('MainContent_txtPassword') != null) {
        document.getElementById('MainContent_txtPassword').setAttribute('autocomplete', 'off');
    }
 
// -->
</script>
    <asp:Label ID="lblErrorLogin" runat="server" CssClass="Title" />
    <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="Title"
        ValidationGroup="LoginUserValidationGroup" />
<div >
<table id="TBL_MAIN" class="Table" style="height: 420px; margin-right: 180px;" 
        cellpadding="0" cellspacing="0"><tbody>
<tr>
<td style="WIDTH: 10%" width="10%"></td>
<td style="HEIGHT: 5%" height="5%"></td>
<td align="right"></td>
</tr>
<tr>
<td style="WIDTH: 10%" width="10%"></td>
<td style="HEIGHT: 10%" height="10%"><strong><font size="3"><span id="ACCESO" class="Title">Bienvenido al Sistema de Control de Calidad</span></font></strong></td>
<td align="right"></td>
</tr>
<tr>
<td><p>&nbsp;</p></td>
<td id="celda_msg" style="HEIGHT: 5%" colspan="2" height="5%"><p></p></td>
<td align="right"></td>
</tr>
<tr>
  <td></td>
  <td style="HEIGHT: 10%" colspan="2" height="10%" valign="bottom"><span id="INGRESE" class="TextBlock">Por favor ingrese su usuario y clave</span><br/></td>
  <td align="right"></td>
</tr>
<tr>
  <td></td>
  <td style="HEIGHT: 22%; BORDER-TOP-COLOR: #000000; BORDER-BOTTOM-COLOR: #000000; BORDER-RIGHT-COLOR: #000000; BORDER-LEFT-COLOR: #000000" colspan="2" height="22%" valign="bottom">
    <p>&nbsp;</p>
    <table id="TABLE2" class="Table" style="border-color: #000033" border="  0" cellpadding="1" cellspacing="2"><tbody>
    <tr>
        <td valign="top">
            <table id="TABLE1" class="Table" style="background-color: #ffffff" align="left" border="  0" cellpadding="0" cellspacing="0"><tbody>

            <tr>
                <td align="center"><span id="USUARIO" class="SubTitle">Usuario: </span></td>
                <td><asp:TextBox ID="_USUARIO" runat="server" Width="168px" onfocus="this.select()" 
                        MaxLength="19" autocomplete="off" 
                        Style="font-family:'Arial'; font-size:15px; font-weight:normal; font-style:normal; padding:2px; text-transform:uppercase;" 
                        ValidationGroup="LoginUserValidationGroup"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidatorUsuario"  runat="server" ValidationGroup="LoginUserValidationGroup" ControlToValidate="_USUARIO" ErrorMessage='Ingrese su nombre de usuario' EnableClientScript="true" SetFocusOnError="true" Text="*" >  
                    </asp:RequiredFieldValidator>
                </td>
                <td></td>
                <td></td>
                <td rowspan="6"> 
                     <asp:Image ID="Image2" runat="server" ImageUrl="Images/alpaca.gif" />
                     </td>

            </tr>

            <tr>
                <td align="center" valign="top"><span id="CONTRASENA" class="SubTitle">Contrase&ntilde;a:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>
                <td valign="top">
                <asp:TextBox ID="txtPassword" runat="server" Width="168px" maxlength="20" 
                        TextMode="Password" autocomplete="off" 
                        
                        style="font-family:'Arial'; font-size:medium; font-weight:normal; font-style:normal"  onblur="JavaScript: elementOnBlur(this);" 
                        ValidationGroup="LoginUserValidationGroup">U01SMLR</asp:TextBox>

                <asp:RequiredFieldValidator  ID="RequiredFieldValidatorPass"  runat="server" ValidationGroup="LoginUserValidationGroup" ControlToValidate="txtPassword" ErrorMessage='Ingrese su clave' EnableClientScript="true" SetFocusOnError="true" Text="*" >
                    </asp:RequiredFieldValidator>
                </td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td align="center"></td>
                <td></td>
                <td></td>
                <td style="height:35px" >
                    <p class="submitButton">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Ingresar" 
                            ValidationGroup="LoginUserValidationGroup" OnClick="LoginButton_Click" />
                    </p>
                </td>
                <td ></td>
            </tr>
                        <tr>
                <td align="center">&nbsp;</td>
                <td align="center">

                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td ></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td ></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            </tbody>
            </table>
       </td>
   </tr>
   </tbody>
   </table>
   

   <script>
       var letra;
       function newChar() {
           var _RC = document.getElementById('MainContent_txtPassword').value;
           if (_RC.length >= 6) {
               alert("Solo se puede ingresar como maximo 6 digitos.");
               return false;
           }
           else
               return true;
       }
       function chgColor(objname) {
           obj = document.getElementById(objname);
           obj.parentElement.style.backgroundColor = 'D7ECE0';
           obj.style.backgroundColor = 'D7ECE0';
       }
       function returnOrigColor(objname) {
           obj = document.getElementById(objname);
           obj.parentElement.style.backgroundColor = 'D7ECE0';
           obj.style.backgroundColor = 'D7ECE0';
       }
       function conc(letra) {
           objname = 'TV' + letra;
           chgColor(objname);
           if (newChar()) {
               document.getElementById('MainContent_txtPassword').value = document.getElementById('MainContent_txtPassword').value + letra;
           }
       }
       //document.getElementById('MainContent_txtPassword').readOnly = true;
       document.getElementById('MainContent__USUARIO').focus();
   </script>
   <script>
       function borrar() {
           document.getElementById('MainContent_txtPassword').value = '';
       }
   </script>
  
    <input id="_EventName" name="_EventName" value="" type="hidden"/>
   
   <p></p>
<p>
<script language="JavaScript" type="text/JavaScript" src="IBfiles/hs900.js"></script> </p>
<p></p></td>
  <td align="right"></td>
</tr>
<tr>
  <td></td>
  <td>

  </td>
  <td width="10%">&nbsp;&nbsp;&nbsp;&nbsp; </td>

</tr>
<tr>
  <td></td>
  <td valign="top"></td>
  <td align="right"></td>
</tr>
<tr>
  <td></td>
  <td colspan="2" valign="top" align="center"></td>
  <td align="right"></td>  
</tr>
<tr>
  <td></td>
  <td colspan="2" valign="top" align="center"></td>
  <td align="right"></td>  
</tr>
</tbody>
</table>
    </div>
<%--<div id="div_foto_usuario"  runat="server" >
    
</div>--%>

<script>
function printDiv(divName) {
     window.print();
}
</script>

</asp:Content>
