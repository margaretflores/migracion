<%@ Page Title="Unidad de Medida de Variables" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="tipounidad.aspx.cs" Inherits="appFew.auxt.tipounidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }  
        .modalBackground {
            background-color:silver;
            opacity:0.7;
        }
        .hideGridColumn
        {
            display:none;
        }
        
        .style1
        {
            height: 16px;
        }
        
    </style>
        <script type="text/javascript">

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

            function doClickEnter(buttonName, e) {//the purpose of this function is to allow the enter key to point to the correct button to click.
                //var key;

//                if (window.event)
//                    key = window.event.keyCode;     //IE
//                else
//                    key = e.which;     //firefox

                //if (key == 13) {
                    //Get the button the user wants to have clicked
                    var btn = document.getElementById(buttonName);
                    if (btn != null) { //If we find the button click it
                        btn.click();
                        //event.keyCode = 0
                    }
                //}

            }

//            function HideModalPopup() {
//                var modal = $find('MainContent_mpe2');
//                modal.hide();
//            }
    </script>

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
        </script>

    <style type="text/css">
        .modalPopup
        {
        background-color: #696969;
        filter: alpha(opacity=40);
        opacity: 0.7;
        xindex:-1;
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
                        Font-Names="Verdana" Text="Unidades de Medida de Variables"></asp:Label>
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

        <tr align="left"  >
            <td style="text-align:left " valign="middle">
                <asp:Label ID="Label3" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" >Descripción/símbolo:</asp:Label>
            </td>
            <td style=" text-align:left " valign="middle" colspan="2">
                <asp:TextBox ID="descripcionBusqTextBox" runat="server" MaxLength="30" ></asp:TextBox>
                <asp:HiddenField ID="tablaHiddenField" runat="server" Value="TUMV"  />
            </td>
        </tr>
        <tr>
                <td></td>
                <td align="left">
                    <asp:Button ID="BuscarButton" runat="server" CommandName="Search" Text="Buscar" 
                            ValidationGroup="BuscarGroup" OnClick="BuscarButton_Click" style="display:inline;" />
                </td>
                <td></td>
            <td align="center">
                
                </td>
        </tr>
        <tr>
            <td colspan="3" >
                                <asp:Label ID="ErrorLabel" CssClass="titulo2" runat="server" Font-Bold="False" 
                                Font-Names="Verdana" Font-Size="Small" EnableViewState="False" ></asp:Label>
            </td>
        </tr>
    </table>
        <table style="width: 700px" cellspacing="0">

            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                    <asp:Label ID="accionLabel" runat="server" CssClass="encabezado1" Font-Bold="True" 
                        Font-Names="Verdana" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3">
                <td colspan="4" style="height: 16px;"></td>
            </tr>
            <tr style="background-color:#F3F3F3; display:none;">
                <td style="height: 16px;" colspan="">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" 
                        Font-Size="X-Small" >Código</asp:Label></td>
                <td >
                    <asp:TextBox ID="codigoTextBox" runat="server" Enabled="False" MaxLength="15"  
                        style="width:100px; text-transform:uppercase;" ></asp:TextBox><asp:HiddenField ID="tauxestaHiddenField"
                            runat="server" />
                </td>
                <td style="height: 16px;" colspan="2">
                    
                </td>
            </tr>  
            <tr style="background-color:#E3E3E3">
                <td style="height: 16px;" colspan="">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Verdana" 
                        Font-Size="X-Small" >Descripción</asp:Label></td>
                <td colspan="2">
                    <asp:TextBox ID="descripcionTextBox" runat="server" Enabled="False" MaxLength="50"  
                        style="width:250px; " ></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="desRequiredFieldValidator"  runat="server" 
                        ValidationGroup="GuardarGroup" ControlToValidate="descripcionTextBox" 
                        ErrorMessage="Ingrese la descripción de la unidad" EnableClientScript="true" 
                        SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>
                </td>
                <td style="height: 16px;">
                </td>
            </tr>  
            <tr style="background-color:#F3F3F3">
                <td style="height: 16px;" colspan="">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Verdana" 
                        Font-Size="X-Small" >Símbolo</asp:Label></td>
                <td colspan="2">
                    <asp:TextBox ID="descortaTextBox" runat="server" Enabled="False" MaxLength="20"
                        style="width:250px; " ></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator1"  runat="server" 
                        ValidationGroup="GuardarGroup" ControlToValidate="descortaTextBox" 
                        ErrorMessage="Ingrese el simbolo de la unidad" EnableClientScript="true" 
                        SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>
                </td>
                <td style="height: 16px;">
                </td>
            </tr>  
            <tr style="background-color:#F3F3F3">
                <td colspan="4" style="height: 16px;"></td>
            </tr>
            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                    </td>
            </tr>

            <tr>
                <td style="height: 58px" colspan="3">
                     <table id="PosTable" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" cellpadding="5" cellspacing="0">
                        <tr>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="nuevoButton" runat="server" CommandName="New" Text="Nuevo" 
                                    ValidationGroup="NuevoGroup" Enabled="True" style="width:72px;"
                                onclick="nuevoButton_Click"  /><asp:HiddenField ID="instanciaActualHiddenField" runat="server" />
                        </td>


                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="grabarButton" runat="server" CommandName="Save" Text="Guardar" 
                                    ValidationGroup="GuardarGroup" Enabled="False" style="width:72px;"
                                onclick="grabarButton_Click" OnClientClick="return confirmBox();" />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="cancelarButton" runat="server" CommandName="Cancel" Text="Limpiar" 
                                Enabled="False" style="width:72px;" onclick="cancelarButton_Click" CausesValidation="False"  />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="eliminarButton" runat="server" CommandName="Save" Text="Eliminar" 
                                    ValidationGroup="GuardarGroup" Enabled="False" style="width:72px;"
                                onclick="eliminarButton_Click"  />
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
        </table>

  <!--Panel to Show Lot Search-->         
    <asp:Button ID="dummy3BusqButton" runat="server" style="display:none" />  
    <cc1:ModalPopupExtender ID="busqLotesModalPopupExtender" runat="server" TargetControlID="dummy3BusqButton" CancelControlID="canelarBusqButton"  PopupControlID="contentBusqpanel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloBusqpanel" BackgroundCssClass="modalBackground" X="120"
                           Y="50" ></cc1:ModalPopupExtender>
    <asp:Panel ID="contentBusqpanel" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="600" Height="550">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#383F52" ForeColor="White" Height="25" >Búsqueda Unidades de Medida de Variables</asp:Panel>
            <table width="100%" style="padding:5px; height:85%">

            <tr style="background-color:#E3E3E3; ">
                <td style="height: 16px">
                    <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small">Descripción</asp:Label></td>
                <td >
                    <asp:TextBox ID="busqLote2TextBox" runat="server" Enabled="False" ReadOnly="true" Font-Size="Small" MaxLength="12"  style="width:100px; text-align:left;" ></asp:TextBox>
                </td>  
                <td style="height: 16px;" >
                    </td>

            </tr>
            <tr style="background-color:#F3F3F3">
                <td colspan="3" style="height: 16px;"></td>
            </tr>

            <tr>
                <td colspan="3" style="background-color:#383F52; height: 26px;
                    text-align: left">
                    <asp:Label ID="Label20" runat="server" CssClass="encabezado1" 
                        Font-Names="Verdana" Font-Bold="True"
                        Text="Detalle" ></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                <div style="height:400px; overflow:auto;">
                    <asp:GridView ID="busqLoteGridView" Width="100%" HorizontalAlign="Center" DataKeyNames="TAUXTABL,TAUXCODI"  
                        runat="server" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#6B696B" cellpadding="5" cellspacing="1" 
                        BorderStyle="Solid" BorderWidth="1px" ShowFooter="True"
                        ForeColor="Black" GridLines="Both" Font-Names="Verdana" 
                        Font-Size="14px"  
                        HeaderStyle-CssClass="hs" onrowcreated="busqLoteGridView_RowCreated" 
                        onselectedindexchanging="busqLoteGridView_SelectedIndexChanging" 
                        onrowdatabound="busqLoteGridView_RowDataBound" 
                        onrowcommand="busqLoteGridView_RowCommand" >
                        <AlternatingRowStyle BackColor="#E3E3E3" />
                        <Columns> 
                            <asp:BoundField DataField="TAUXTABL" HeaderText="Tabla" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" FooterStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TAUXCODI" HeaderText="Código" HeaderStyle-CssClass="hideGridColumn"  ItemStyle-CssClass="hideGridColumn" FooterStyle-CssClass="hideGridColumn"  HeaderStyle-BorderStyle="None"   />
                            <asp:BoundField DataField="TAUXDESC" HeaderText="Unidad de Medida" ItemStyle-CssClass="is" FooterStyle-CssClass="fs"  ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TAUXALF1" HeaderText="Símbolo" ItemStyle-CssClass="is" FooterStyle-CssClass="fs"  ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TAUXESTA" HeaderText="Estado" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" FooterStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" Font-Bold="True" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#F3F3F3" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                        <SortedAscendingHeaderStyle BackColor="#848384" />
                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                        <SortedDescendingHeaderStyle BackColor="#575357" />
                    </asp:GridView>
                </div>
                </td>

            </tr>

                <tr>
                    <td colspan="3"><asp:Label ID="errorPopupBusqLoteLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
            </table>
                <div align="right">
                <asp:Button ID="aceptarBusqButton" runat="server" Width="70" Text="Aceptar" 
                        ValidationGroup="busqLoteAceptar" onclick="aceptarBusqButton_Click"/>
                &nbsp;
                <asp:Button ID="canelarBusqButton" runat="server" Width="70" Text="Cancelar" CausesValidation="false" />&nbsp;&nbsp;
            </div>
    </asp:Panel>    
    <!--End of Panel to Show Items Detail-->   
                       </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
