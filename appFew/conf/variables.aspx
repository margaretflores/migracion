<%@ Page Title="Maestro de Variables" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="variables.aspx.cs" Inherits="appFew.conf.variables" %>
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
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1" Font-Names="Verdana" Text="Maestro de Variables"></asp:Label></td>
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
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="MVARIDVA"  
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
                            <asp:BoundField DataField="MVARIDVA" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARDECO" HeaderText="Desc corta" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARNOMB" HeaderText="Nombre Variable" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTBUM" HeaderText="TBUM" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTCUM" HeaderText="COUM" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UNMEDESC" HeaderText="Unidad" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARVACC" HeaderText="VACC" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARVACC" HeaderText="Control Calidad" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARVACP" HeaderText="VACP" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARVACP" HeaderText="Control de Proceso" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTBTV" HeaderText="TBTV" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTCTV" HeaderText="COTV" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TPVADESC" HeaderText="Tipo" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTBMC" HeaderText="TBMC" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARTCMC" HeaderText="COMC" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MTCADESC" HeaderText="Método Cálculo" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
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
                <td style="width:16%; padding:5px; text-align: left" >Nombre Variable:</td>
                <td colspan="1" style="padding:5px; width:45%; text-align: left" ><b><asp:Label ID="descripcion3Label" runat="server" ></asp:Label><b></td>
                <td style="width:14%; padding:5px; text-align: left" >Desc corta:</td>
                <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="localizacionLabel" runat="server" ></asp:Label><b></td>
            </tr>
            <tr>
                <td colspan="4">
                      <div>
                        <table width="100%" style="padding:1px" class="iscb" >
                            <tr>
                                <td width="14%">Desc corta</td>
                                <td>:</td>
                                <td width="30%"><b><asp:Label ID="codigoLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td width="14%"></td>
                                <td></td>
                                <td width="30%"></td>
                            </tr>
                            <tr>
                                <td>Estado Activo</td>
                                <td>:</td>
                                <td><b><asp:Label ID="estadoLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td></td>
                                <td></td>
                                <td</td>
                            </tr>
                            <tr>
                                <td>Requiere validación</td>
                                <td>:</td>
                                <td><b><asp:Label ID="noreqvalLabel" runat="server" ></asp:Label></b></td>
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
    <asp:Panel ID="panelEdit" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="510" Height="300">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
        <asp:Literal ID="tituloeditLiteral" runat="server" Text="Modificar Variable"></asp:Literal></b></asp:Panel>
            <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
            cellspacing="1" cellpadding="1" >

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label17" runat="server" >Desc corta:</asp:Label></td>
                <td >
                    <asp:TextBox ID="descoreditTextBox" runat="server" MaxLength="10"  style="width:100px;" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="descortaRequiredFieldValidator" runat="server" ControlToValidate="descoreditTextBox" Display="None" ErrorMessage="Ingrese un código o una descripción corta"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="descortaValidatorCalloutExtender" runat="server" TargetControlID="descortaRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label3" runat="server" >Nombre Variable:</asp:Label></td>
                <td >
                    <asp:TextBox ID="nomvareditTextBox" runat="server" Font-Size="Small" MaxLength="50"  style="text-align:left; " ></asp:TextBox><asp:HiddenField ID="iddeHiddenField" runat="server" />
                    <asp:RequiredFieldValidator ID="nomvarRequiredFieldValidator" runat="server" ControlToValidate="nomvareditTextBox" Display="None" ErrorMessage="Ingrese el nombre de la variable"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="nomvarValidatorCalloutExtender" runat="server" TargetControlID="nomvarRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label1" runat="server" >Unidad:</asp:Label></td>
                <td >
                    <asp:DropDownList ID="unimededitDropDownList" runat="server" AutoPostBack="False">
                            <asp:ListItem Value="0" Text="[Seleccione Unidad]"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="unimedRequiredFieldValidator" runat="server" ControlToValidate="unimededitDropDownList" Display="None" ErrorMessage="Seleccione unidad" InitialValue="0" ValidationGroup="edit"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="unimedValidatorCalloutExtender" runat="server" TargetControlID="unimedRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label4" runat="server" >Var Control de Calidad:</asp:Label></td>
                <td >
                    <asp:CheckBox ID="varccaeditCheckBox" runat="server" />
                </td>  
            </tr>

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label2" runat="server" >Var Control de Proceso:</asp:Label></td>
                <td >
                    <asp:CheckBox ID="varcpreditCheckBox" runat="server" />
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    </td>
                <td >
                    <asp:CheckBox ID="norqvaeditCheckBox" Text="No requiere validación" Font-Size="Small" runat="server" />
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label5" runat="server" >Tipo Valor:</asp:Label></td>
                <td >
                    <asp:DropDownList ID="tipvaleditDropDownList" runat="server" AutoPostBack="False">
                            <asp:ListItem Value="0" Text="[Seleccione tipo valor]"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="tipvaleditRequiredFieldValidator" runat="server" ControlToValidate="tipvaleditDropDownList" Display="None" ErrorMessage="Seleccione tipo valor" InitialValue="0" ValidationGroup="edit"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="tipvaleditValidatorCalloutExtender" runat="server" TargetControlID="tipvaleditRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label6" runat="server" >Método Cálculo:</asp:Label></td>
                <td >
                    <asp:DropDownList ID="metcaleditDropDownList" runat="server" AutoPostBack="False">
                            <asp:ListItem Value="0" Text="[Seleccione método cálculo]"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="metcalRequiredFieldValidator" runat="server" ControlToValidate="metcaleditDropDownList" Display="None" ErrorMessage="Seleccione método cálculo" InitialValue="0" ValidationGroup="edit"></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="metcalValidatorCalloutExtender" runat="server" TargetControlID="metcalRequiredFieldValidator" ></cc1:ValidatorCalloutExtender>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label7" runat="server" >Estado activo:</asp:Label></td>
                <td >
                    <asp:CheckBox ID="estacteditCheckBox" runat="server" />
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
    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
