<%@ Page Title="Usuarios asignados a un Rol" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="usuariosroles.aspx.cs" Inherits="appFew.role.usuariosroles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery.blockUI.js" type="text/javascript" ></script>

    <style type="text/css">
        .modalBackground {
            background-color:silver;
            opacity:0.7;
        }
        .hideGridColumn
        {
            display:none;
        }        
        .column-left{ float: left; width: 43%; padding-left: 1%; }
        .column-center{ display: inline-block; width: 13%; }
        .column-right{ float: right; width: 43%; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
         prm.add_beginRequest(BeginRequestHandler);
         // Raised after an asynchronous postback is finished and control has been returned to the browser.
         prm.add_endRequest(EndRequestHandler);
         function BeginRequestHandler(sender, args) {
             //Shows the modal popup - the update progress
             var popup = $find('<%= modalPopup.ClientID %>');
             if (popup != null) {
                 popup.show();
             }
         }

         function EndRequestHandler(sender, args) {
             //Hide the modal popup - the update progress
             var popup = $find('<%= modalPopup.ClientID %>');
             if (popup != null) {
                 popup.hide();
             }
         }
         //            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

         function doClick(buttonName, e) {//the purpose of this function is to allow the enter key to point to the correct button to click.
             var key;

             if (window.event)
                 key = window.event.keyCode;     //IE
             else
                 key = e.which;     //firefox

             if (key == 13) {
                 //Get the button the user wants to have clicked
                 var btn = document.getElementById(buttonName);
                 if (btn != null) { //If we find the button click it
                     btn.click();
                     event.keyCode = 0
                 }
             }
         }

         $("#varasiCheckBoxList").css({
                    "borderBottomWidth": "2px",
                    "backgroundColor": "red",
                    "color": "black",
                    "textAlign": "right",
                    "fontWeight": "bold"
        });

        </script>
    <style type="text/css">
        .modalPopup
        {
        background-color: #696969;
        filter: alpha(opacity=40);
        opacity: 0.7;
        xindex:-1;
        }
        
        .myClass
        {
            border: solid 1px black;
            background-color: white;
        }
        .myClass input
        {
            background-color:Gray;	
        }
        .myClass label
        {
            font-weight:normal;
            font-family: Arial;
            font-size:small;
            color:Black;
        }
        
    </style>
    <asp:ScriptManager ID="tsm" runat="server">
    </asp:ScriptManager>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing" runat="server" />
        </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground"  />

    <asp:UpdatePanel runat="server" ID="up" UpdateMode="Conditional">
        <ContentTemplate>
        <div >
        <table style="width: 700px;" >

            <tr>
                <td style="width: auto; height: 24px;" colspan="3">
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1ddl" 
                        Font-Names="Verdana" Text="Asignación de Usuarios a Rol"></asp:Label>
                </td>
            </tr>
        <tr align="left" >
            <td colspan="2" style=" text-align:left">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    CssClass="validmessage" ValidationGroup="GuardarGroup" />
                <br />
            </td>
            <td></td>
        </tr>
    </table>
        <table style="width: 800px" cellspacing="0">
            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                    <asp:Label ID="accionLabel" runat="server" CssClass="encabezado1" Font-Bold="True" 
                        Font-Names="Verdana" Text=""></asp:Label>
                </td>
            </tr>
           <tr style="background-color:#F3F3F3; ">
                <td style="height: 16px;" colspan="">
                </td>
                <td >
                </td>
                <td style="height: 16px;" colspan="2">
                    
                </td>
            </tr>  
            <tr style="background-color:#E3E3E3">
                <td style="text-align:left; padding-left:10px;" valign="middle">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Rol:" ></asp:Label>
                </td>
                <td colspan="3" align="left">
                    <asp:DropDownList ID="rolDropDownList" runat="server" AutoPostBack="True" onselectedindexchanged="rolDropDownList_SelectedIndexChanged" style="width:250px; ">
                            <asp:ListItem Value="0" Text="[Seleccione el Rol]"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="rolDropDownList" CssClass="validmessage" 
                    EnableClientScript="false" ErrorMessage="Seleccione el Rol" InitialValue="0" 
                    SetFocusOnError="true" ValidationGroup="BuscarGroup"></asp:RequiredFieldValidator>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td style="height: 16px;" colspan="1">
               </td>
                <td >
                    
                </td>
                <td style="height: 16px;" colspan="1">
                    
                </td>
                <td style="height: 16px; text-align: right;" colspan="1">
                    <asp:Button ID="BuscarButton" runat="server" CommandName="Search" Text="Buscar" ValidationGroup="BuscarGroup" OnClick="BuscarButton_Click" style="display:none;" />
                </td>
            </tr>  
            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                </td>
            </tr>
            <tr>
                <td style="height: 35px" colspan="3">
                     <table id="Table1" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" cellpadding="5" cellspacing="0">
                        <tr>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="grabar2Button" runat="server" CommandName="Save" Text="Guardar" CausesValidation="True"
                                    ValidationGroup="GuardarGroup" Enabled="False" style="width:72px;"
                                onclick="grabarButton_Click"  />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="cancelar2Button" runat="server" CommandName="Cancel" Text="Limpiar" 
                                Enabled="False" style="width:72px;" onclick="cancelarButton_Click" CausesValidation="False"  />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                            <td style="padding-left: 5px; " align="left">
                                
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: auto; height: 16px;"></td>
            </tr>

            <tr >
                <td colspan="4" >
                    <div>
                        <div runat="server" id="div2" style="padding-left: 1%;">
                            <asp:TextBox ID="txtSearch" runat="server" />
                            <asp:Button ID="filtraUsersButton" runat="server" CommandName="Search" 
                                Text="Buscar" ClientIDMode="Static" onclick="filtraUsersButton_Click" />
                        </div>
                        <hr />
                        <div id="the-whole-thing" >
                            <div id="leftThing" class="column-left"  >

                    <asp:GridView ID="dispGridView" runat="server" style="height: 85%; width:95%" DataKeyNames="USUACOUS"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" AllowPaging="false" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        onrowdatabound="dispGridView_RowDataBound" 
                        onselectedindexchanging="gridView_SelectedIndexChanging" 
                        onselectedindexchanged="gridView_SelectedIndexChanged" 
                        Font-Size="11px" >
                        <Columns>
                            <asp:BoundField DataField="USUACOUS" HeaderText="Usuario" >
                            <ItemStyle CssClass="isavas" />
                            </asp:BoundField>
                            <asp:BoundField DataField="USUANOUS" HeaderText="Nombre" >
                            <ItemStyle CssClass="isavas" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderColor="#6B696B" BorderStyle="Solid" />
                        <RowStyle BackColor="#F3F3F3" />
                        <SelectedRowStyle BackColor="#8D8B8D" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>

                                </div>
                                <div id="content" class="column-center">
                                    <div runat="server" id="div1" style="padding-left: 2%;">
                                            <asp:Button ID="agregarButton" runat="server" CommandName="Search" style="width:85%;"
                                                Text="Agregar --&gt;" ClientIDMode="Static" onclick="agregarButton_Click" />
                                            <asp:Button ID="removerButton" runat="server" CommandName="Search" style="width:85%;"
                                                Text="&lt;-- Remover" ClientIDMode="Static" onclick="removerButton_Click" />        
		                            </div>
                                </div>
                            <div id="rightThing" class="column-right">
                    <asp:GridView ID="asigvarGridView" runat="server" style="height: 85%; width:95%" DataKeyNames="USUACOUS"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#AAAAAA" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" AllowPaging="false" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        onrowdatabound="asigvarGridView_RowDataBound" 
                        onselectedindexchanging="gridView_SelectedIndexChanging" 
                        onselectedindexchanged="gridView_SelectedIndexChanged" 
                        Font-Size="11px" >
                        <Columns>
                            <asp:BoundField DataField="USUACOUS" HeaderText="Usuario" >
                            <ItemStyle CssClass="isavas" />
                            </asp:BoundField>
                            <asp:BoundField DataField="USUANOUS" HeaderText="Nombre" >
                            <ItemStyle CssClass="isavas" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderColor="#6B696B" BorderStyle="Solid"/>
                        <RowStyle BackColor="#F3F3F3" />
                        <SelectedRowStyle BackColor="#8D8B8D" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td style="height: 78px" colspan="3">
                     <table id="PosTable" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" cellpadding="5" cellspacing="0">
                        <tr>
                        <td style="padding-left: 5px; " align="left">
                            <asp:HiddenField ID="instanciaActualHiddenField" runat="server" />
                        </td>


                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="grabarButton" runat="server" CommandName="Save" Text="Guardar" CausesValidation="True"
                                    ValidationGroup="GuardarGroup" Enabled="False" style="width:72px;"
                                onclick="grabarButton_Click"  />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="cancelarButton" runat="server" CommandName="Cancel" Text="Limpiar" 
                                Enabled="False" style="width:72px;" onclick="cancelarButton_Click" CausesValidation="False"  />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                            <td style="padding-left: 5px; " align="left">
                                
                            </td>
                        </tr>
                        <tr style="height: 40px">
                            <td style="padding-left: 5px; " align="left">
                            </td>
                            <td style="padding-left: 5px; " align="left">
                            </td>
                            <td style="padding-left: 5px; " align="left">
                            </td>
                            <td style="padding-left: 5px; " align="left">
                            </td>
                            <td style="padding-left: 5px; " align="left">
                            </td>
                            <td style="padding-left: 5px; " align="left">
                            </td>
                        </tr>
                    </table>
                    
                    
                </td>
                <td style="width: auto; height: 16px;"></td>
            </tr>
            <tr>
                <td colspan="4" >
                                <asp:Label ID="ErrorLabel" CssClass="titulo2" runat="server" Font-Bold="False" 
                                Font-Names="Verdana" Font-Size="Small" EnableViewState="False" ></asp:Label>
                </td>
            </tr>
        </table>

            </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
