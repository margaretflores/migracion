<%@ Page Title="Rangos para Máquinas Fantasía" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="maquinasfantasia.aspx.cs" Inherits="appFew.mae.maquinasfantasia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery.blockUI.js" type="text/javascript" ></script>

  <link rel="stylesheet" href="../Content/ui/1.12.0/themes/base/jquery-ui.css" />
  <link rel="stylesheet" href="/resources/demos/style.css" />
  <script src="../Scripts/jquery-1.12.4.js"></script>
  
    <script src="../Scripts/ui/1.12.0/jquery-ui.js"></script>
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

//            function doSearch() {
//                $("#buscarButton").click();
//            }

//            function HideModalPopup() {
//                var modal = $find('MainContent_mpe2');
//                modal.hide();
            //            }



            $(function () {
                $("#tabs").tabs();
            });            


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
                //var popup = $find('<%= modalPopup.ClientID %>');
                //if (popup != null) {
                //    popup.show();
                //}
            }

            function EndRequestHandler(sender, args) {
                //Hide the modal popup - the update progress
                //var popup = $find('<%= modalPopup.ClientID %>');
                //if (popup != null) {
                //    popup.hide();
                //}
                $("#tabs").tabs();
            }
            //            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            //$("[id*=txtSearch]").live("keyup", function () {
            //    doSearch();
            //});

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

        <table style="width: 850px;" >

            <tr>
                <td style="width: auto; height: 24px;" colspan="2">
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1" Font-Names="Verdana" Text="Rangos para Máquinas Fantasía"></asp:Label></td>
                <td style="width: auto; height: 24px; " colspan="2" align="right" >
                    &nbsp;</td>
            </tr>
        </table>
        <table style="width: 850px" cellspacing="0">
                   

            <tr>
                <td class="toolbar" colspan="4" >
                     <table id="PosTable" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" cellpadding="5" cellspacing="0">
                        <tr>


                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="agregarButton" runat="server" CommandName="Save" Text="Agregar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="agregarButton_Click"
                                 />
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="modificarButton" runat="server" CommandName="Save" Text="Modificar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="modificarButton_Click" 
                                 />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:ImageButton ID="eliminarButton" runat="server" Text="Eliminar" Enabled="False"
                                ImageUrl="~/Images/remove-icon.png" style="width:20px;" 
                                onclick="eliminarButton_Click" ToolTip="Eliminar" />
                        </td>                            
                            
                        </tr>
                    </table>
                    
                </td>

            </tr>
            <tr>
                <td colspan="4">
        <div class="GridContainer">
            <asp:TextBox ID="txtSearch" runat="server" />
            <asp:Button ID="buscarButton" runat="server" 
                CommandName="Search" Text="Buscar" ClientIDMode="Static" 
                onclick="buscarButton_Click" />
            <hr />
<%--            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="MQFAIDMF"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        Font-Size="14px" AllowPaging="true" PageSize="15"
                        onrowcreated="cabeceraGridView_RowCreated" 
                        onrowdatabound="cabeceraGridView_RowDataBound" 
                        onselectedindexchanged="cabeceraGridView_SelectedIndexChanged" 
                        onselectedindexchanging="cabeceraGridView_SelectedIndexChanging" 
                        >
                        <AlternatingRowStyle BackColor="#E3E3E3" />
                        <Columns>
                            <asp:BoundField DataField="MQFAIDMF" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MQFAITEM" HeaderText="Item" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MQFAIDMA" HeaderText="ID Maq" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>

                            <asp:BoundField DataField="MAQUCOMA" HeaderText="Código" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MAQUDES2" HeaderText="Descripción Máquina" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MAQUMARC" HeaderText="Marca" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MQFARNDE" HeaderText="Rango de" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MQFARANA" HeaderText="Rango a" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderWidth="1px" BorderStyle="Solid"  />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#F3F3F3" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                        <SortedAscendingHeaderStyle BackColor="#848384" />
                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                        <SortedDescendingHeaderStyle BackColor="#575357" />
                    </asp:GridView>
                    <div class="Pager">
                    <asp:Button ID="primeroButton" runat="server" CommandName="PagerPrimero" 
                            Text="Primero" ClientIDMode="Static" onclick="primeroButton_Click" />
                    <asp:Button ID="anteriorButton" runat="server" CommandName="PagerAnterior" 
                            Text="Anterior" ClientIDMode="Static" onclick="anteriorButton_Click" />
                    <asp:Button ID="siguienteButton" runat="server" CommandName="PagerSiguiente" 
                            Text="Siguiente" ClientIDMode="Static" onclick="siguienteButton_Click" />
                    <asp:Button ID="ultimoButton" runat="server" CommandName="PagerUltimo" 
                            Text="Ultimo" ClientIDMode="Static" onclick="ultimoButton_Click" />
                        <asp:Label ID="paginaLabel" runat="server" Text="Pág: 1" CssClass="iscb" 
                            Font-Bold="True"></asp:Label> 
                        <asp:Label ID="totalLabel" runat="server" Text=" Total: 0" CssClass="iscb" 
                            Font-Bold="True"></asp:Label>
                        <asp:HiddenField ID="pageIndexHiddenField" runat="server" Value="1" />
                        <asp:HiddenField ID="totalHiddenField" runat="server" Value="0" />
                        
                    </div>
        </div>

                 </td>

            </tr>
            <tr class="fs">
                <td style="width:6%; padding:5px; text-align: left" >Artículo:</td>
                <td colspan="1" style="padding:5px; width:45%; text-align: left" ><b><asp:Label ID="descripcion3Label" runat="server" ></asp:Label><b></td>
                <td style="width:6%; padding:5px; text-align: left" >Máquina:</td>
                <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="localizacionLabel" runat="server" ></asp:Label><b></td>
            </tr>
            <tr>
                <td colspan="4">
                      <div>
                        <table width="100%" style="padding:1px" class="iscb" >
                            <tr>
                                <td>Artículo</td>
                                <td>:</td>
                                <td><b><asp:Label ID="articuloLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Rango de</td>
                                <td>:</td>
                                <td><b><asp:Label ID="rangodeLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Código Máquina</td>
                                <td>:</td>
                                <td><b><asp:Label ID="codigoLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Rango a</td>
                                <td>:</td>
                                <td><b><asp:Label ID="rangoaLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Máquina</td>
                                <td>:</td>
                                <td><b><asp:Label ID="maquinaLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style="padding:2px"></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                       </table>
                      </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="background-color:#6B696B; height: 26px;
                    text-align: left">
                    </td>
            </tr>

        </table>

    
                <!--Panel to Edit record-->         
    <asp:Button ID="dummy3BusqButton" runat="server" style="display:none" />  
    <cc1:ModalPopupExtender ID="mpe2" runat="server" TargetControlID="dummy3BusqButton" CancelControlID="canelarBusqButton"  PopupControlID="panelEdit" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloBusqpanel" BackgroundCssClass="modalBackground" X="120"
                           Y="50" ></cc1:ModalPopupExtender>
    <asp:Panel ID="panelEdit" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="510" Height="210">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
        <asp:Literal ID="tituloeditLiteral" runat="server" Text="Modificar Máquinas Fantasía"></asp:Literal></b></asp:Panel>
            <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
            cellspacing="1" cellpadding="1" >

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label17" runat="server" >Item:</asp:Label></td>
                <td >
                    <asp:TextBox ID="articuloeditTextBox" runat="server" Font-Size="Small" MaxLength="9"  style="text-align:left; text-transform:uppercase;" ></asp:TextBox><asp:HiddenField ID="idmfanHiddenField" runat="server" />
                    <asp:RequiredFieldValidator ID="codmaqRequiredFieldValidator" runat="server" ControlToValidate="articuloeditTextBox" Display="None" ErrorMessage="Ingrese el código de artículo"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="codmaqRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label3" runat="server" >Código Máq.:</asp:Label></td>
                <td >
                    <asp:TextBox ID="codmaqeditTextBox" runat="server" Font-Size="Small" MaxLength="6"  style="text-align:left; text-transform:uppercase;" ></asp:TextBox><asp:HiddenField ID="idmaqHiddenField" runat="server" />
                    <asp:Button ID="buscaMaquinaButton" runat="server" Text="Buscar" ValidationGroup="buscaMaquina" onclick="buscaMaquinaButton_Click" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="codmaqeditTextBox" Display="None" ErrorMessage="Ingrese el código de máquina"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label4" runat="server" >Máquina:</asp:Label></td>
                <td >
                    <asp:TextBox ID="desmaqeditTextBox" runat="server" Font-Size="Small" MaxLength="50"  style="width:98%; text-align:left; " ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="desmaqeditTextBox" Display="None" ErrorMessage="Ingrese la máquina"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator2" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label2" runat="server" >Rango de:</asp:Label></td>
                <td >
                    <asp:TextBox ID="rangdeeditTextBox" runat="server" Font-Size="Small" MaxLength="10"  style="width:48%; text-align:right; " ></asp:TextBox> 
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="rangdeeditTextBox" />
                    <asp:RequiredFieldValidator ID="canmueRequiredFieldValidator" runat="server" ControlToValidate="rangdeeditTextBox" Display="None" ErrorMessage="Ingrese el rango inicial"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="vce3" runat="server" TargetControlID="canmueRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label5" runat="server" >Rango a:</asp:Label></td>
                <td >
                    <asp:TextBox ID="rangoaTextBox" runat="server" Font-Size="Small" MaxLength="10" style="width:48%; text-align:right; " ></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="rangoaTextBox" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rangoaTextBox" Display="None" ErrorMessage="Ingrese el rango final"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator3" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>

                <tr>
                    <td colspan="2"><asp:Label ID="ErrorLabelPopup" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
            </table>
                <div align="right">
                <asp:Button ID="aceptarBusqButton" runat="server" Width="70" Text="Aceptar" 
                        ValidationGroup="edit" onclick="aceptarBusqButton_Click"/>
                &nbsp;
                <asp:Button ID="canelarBusqButton" runat="server" Width="70" Text="Cancelar" CausesValidation="false" />&nbsp;&nbsp;
            </div>
    </asp:Panel>    

               <!--End of Panel to edit record-->

  <!--Panel to Show Maquina Search-->         
    <asp:Button ID="dummymaquinasButton" runat="server" style="display:none" />  
    <cc1:ModalPopupExtender ID="busqMaquinasModalPopupExtender" runat="server" TargetControlID="dummymaquinasButton" CancelControlID="cancelMaquinaButton"  PopupControlID="contentBusqpanel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="busqMaquinasPanel" BackgroundCssClass="modalBackground" X="100"
                           Y="50" ></cc1:ModalPopupExtender>
    <asp:Panel ID="contentBusqpanel" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="550" Height="400">  
        <asp:Panel ID="busqMaquinasPanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#383F52" ForeColor="White" Height="25" ><b>Búsqueda de Máquinas</b></asp:Panel>
            <table width="100%" style="padding:5px; height:84%">
            <tr>
                <td colspan="3" valign="top">
                    <div class="GridContainer">
                        <asp:TextBox ID="buscarMaquinasTextBox" runat="server" />
                        <asp:Button ID="buscarMaquinasButton" runat="server" CommandName="Search" Text="Buscar" ClientIDMode="Static" onclick="buscarMaquinasButton_Click" />
                    <hr />
                    <asp:GridView ID="maquinasGridView" Width="100%" runat="server" DataKeyNames="MAQUIDMA"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        Font-Size="14px" AllowPaging="false" 
                        OnRowCreated="maquinasGridView_RowCreated" 
                        OnRowDataBound="maquinasGridView_RowDataBound" 
                        OnSelectedIndexChanging="maquinasGridView_SelectedIndexChanging" 
                        OnSelectedIndexChanged="maquinasGridView_SelectedIndexChanged" 
                        >
                        <AlternatingRowStyle BackColor="#E3E3E3" />
                        <Columns>
                            <asp:BoundField DataField="MAQUIDMA" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MAQUCOMA" HeaderText="Código" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MAQUDES2" HeaderText="Descripción Máquina" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MAQUMARC" HeaderText="Marca" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" BorderWidth="1px" BorderStyle="Solid"  />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#F3F3F3" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                        <SortedAscendingHeaderStyle BackColor="#848384" />
                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                        <SortedDescendingHeaderStyle BackColor="#575357" />
                    </asp:GridView>
                    <div class="Pager">
                    <asp:Button ID="primeromaqButton" runat="server" CommandName="PagerPrimero" 
                            Text="Primero" ClientIDMode="Static" onclick="primeromaqButton_Click" />
                    <asp:Button ID="anteriormaqButton" runat="server" CommandName="PagerAnterior" 
                            Text="Anterior" ClientIDMode="Static" onclick="anteriormaqButton_Click" />
                    <asp:Button ID="siguientemaqButton" runat="server" CommandName="PagerSiguiente" 
                            Text="Siguiente" ClientIDMode="Static" onclick="siguientemaqButton_Click" />
                    <asp:Button ID="ultimomaqButton" runat="server" CommandName="PagerUltimo" 
                            Text="Ultimo" ClientIDMode="Static" onclick="ultimomaqButton_Click" />
                        <asp:Label ID="paginamaqLabel" runat="server" Text="Pág: 1" CssClass="iscb" 
                            Font-Bold="True"></asp:Label> 
                        <asp:Label ID="totalmaqLabel" runat="server" Text=" Total: 0" CssClass="iscb" 
                            Font-Bold="True"></asp:Label>
                        <asp:HiddenField ID="pageIndexmaqHiddenField" runat="server" Value="1" />
                        <asp:HiddenField ID="totalmaqHiddenField" runat="server" Value="0" />
                        
                    </div>

        </div>
                </td>

            </tr>
            </table>
                <div align="right">
                <asp:Button ID="aceptarMaquinaButton" runat="server" Width="70" Text="Aceptar" 
                        ValidationGroup="busqMaquinaAceptar" onclick="aceptarMaquinaButton_Click"/>
                &nbsp;
                <asp:Button ID="cancelMaquinaButton" runat="server" Width="70" Text="Cancelar" CausesValidation="false" />&nbsp;&nbsp;
            </div>
            <div align="left" style="padding-left:10px;">
            <asp:Label ID="errorPopupBusqMaquinaLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
            </div>
    </asp:Panel>    
    <!--End of Panel to Show Items Detail-->  
                  
    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
