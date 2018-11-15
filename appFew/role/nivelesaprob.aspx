<%@ Page Title="Gestión de Niveles de Autorización para Liberar Variable" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="nivelesaprob.aspx.cs" Inherits="appFew.role.nivelesaprob" %>
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

        <table style="width: 750px;" >

            <tr>
                <td style="width: auto; height: 24px;" colspan="2">
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Gestión de Niveles de Autorización para Liberar Variable"></asp:Label></td>
                <td style="width: auto; height: 24px; " colspan="2" align="right" >
                    &nbsp;</td>
            </tr>
        </table>
        <table style="width: 750px" cellspacing="0" cellpadding="0">
                   
            <tr >
                <td style="text-align:left " valign="middle">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo de Control:" ></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="tipconDropDownList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="tipconDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Control de Calidad"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Control de Proceso"></asp:ListItem>
                    </asp:DropDownList><asp:HiddenField ID="tipconHiddenField" runat="server" />
                </td>  
            </tr>
            <tr>
                <td class="toolbar" colspan="4" >
                     <table id="PosTable" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" >
                        <tr >
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="agregarButton" runat="server" CommandName="Save" Text="Agregar"  Enabled="False"
                                    ValidationGroup="BuscarGroup" style="width:72px;" onclick="agregarButton_Click"
                                 />
                        </td>
                        <td  align="left">
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
            <asp:TreeView ID="TreeView1" runat="server" NodeIndent="20"
                 NodeStyle-CssClass="treeNode"
                    RootNodeStyle-CssClass="rootNode"
                    LeafNodeStyle-CssClass="leafNode" onselectednodechanged="TreeView1_SelectedNodeChanged" 
                         >
                <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />

                <LeafNodeStyle CssClass="leafNode" ImageUrl="~/Images/edificio3a.png"></LeafNodeStyle>
                <ParentNodeStyle CssClass="parentNode" ImageUrl="~/Images/edificio3a.png"></ParentNodeStyle>

                <RootNodeStyle CssClass="rootNode" ImageUrl="~/Images/edificio3a.png"></RootNodeStyle>

                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                    VerticalPadding="0px" />
            </asp:TreeView>                    
            <hr />

<%--                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="calcularButton" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>

                 </td>

            </tr>


        </table>
        <table style="width: 750px; border: thin solid #6B696B; " >
            <tr class="fs">
                <td colspan="4" style="padding:5px; width:50%; text-align: center" ><b>LÍNEAS DE PRODUCCIÓN ASIGNADAS<b></td>
            </tr>
            <tr>
                <td class="toolbar" colspan="4" >
                     <table id="Table2" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" >
                        <tr>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="agregarLineaButton" runat="server" Text="Agregar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="agregarLineaButton_Click"
                                 />
                        </td>
                        <td align="left">
                            <asp:Button ID="removerLineaButton" runat="server" Text="Remover" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="removerLineaButton_Click" />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            
                        </td>                            
                        <td  >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                        </tr>
                    </table>
                    
                </td>

            </tr>
            <tr>
                <td colspan="4">
        <div class="GridContainer" style="overflow: auto; height: 150px;">
                    <asp:GridView ID="lineasGridView" Width="100%" runat="server" DataKeyNames="LIPRCODI"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        Font-Size="14px" AllowPaging="false" PageSize="15"
                        onrowcreated="lineasGridView_RowCreated" 
                        onrowdatabound="lineasGridView_RowDataBound" 
                        onselectedindexchanged="lineasGridView_SelectedIndexChanged" 
                        onselectedindexchanging="lineasGridView_SelectedIndexChanging" 
                        >
                        <AlternatingRowStyle BackColor="#E3E3E3" />
                        <Columns>
                            <asp:BoundField DataField="LIPRCODI" HeaderText="Código" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LIPRDESC" HeaderText="Nombre Línea de Producción" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataRowStyle BackColor="White" BorderColor="#6B696B" BorderStyle="Solid" 
                            BorderWidth="1px" />
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
            </div>
                </td>
            </tr>
        </table>
        <table style="width: 750px; border: thin solid #6B696B; " >
            <tr class="fs">
                <td colspan="4" style="padding:5px; width:50%; text-align: center" ><b>ROLES ASIGNADOS<b></td>
            </tr>
            <tr>
                <td class="toolbar" colspan="4" >
                     <table id="Table1" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" >
                        <tr>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="agregarRolButton" runat="server" Text="Agregar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="agregarRolButton_Click"
                                 />
                        </td>
                        <td align="left">
                            <asp:Button ID="removerRolButton" runat="server" Text="Remover" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="removerRolButton_Click" />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            
                        </td>                            
                        <td >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                        </td>
                        </tr>
                    </table>
                    
                </td>

            </tr>
            <tr>
                <td colspan="4">
                    <div class="GridContainer" style="overflow: auto; height: 100px;">
                    <asp:GridView ID="rolesGridView" Width="100%" runat="server" DataKeyNames="GRUSCOGR"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        Font-Size="14px" AllowPaging="false" PageSize="15"
                        onrowcreated="rolesGridView_RowCreated" 
                        onrowdatabound="rolesGridView_RowDataBound" 
                        onselectedindexchanged="rolesGridView_SelectedIndexChanged" 
                        onselectedindexchanging="rolesGridView_SelectedIndexChanging" 
                        >
                        <AlternatingRowStyle BackColor="#E3E3E3" />
                        <Columns>
                            <asp:BoundField DataField="GRUSCOGR" HeaderText="Código" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GRUSDEGR" HeaderText="Descripción Rol" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataRowStyle BackColor="White" BorderColor="#6B696B" BorderStyle="Solid" 
                            BorderWidth="1px" />
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
                    </div>
                </td>
            </tr>
        </table>

        <div>    
                <!--Panel to Edit record-->         
    <asp:Button ID="dummy3BusqButton" runat="server" style="display:none" />  
    <cc1:ModalPopupExtender ID="mpe2" runat="server" TargetControlID="dummy3BusqButton" CancelControlID="canelarBusqButton"  PopupControlID="panelEdit" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloBusqpanel" BackgroundCssClass="modalBackground" X="120"
                           Y="50" ></cc1:ModalPopupExtender>
    <asp:Panel ID="panelEdit" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="600" Height="250">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
            <asp:Literal ID="tituloeditLiteral" runat="server" Text="Modificar Localización"></asp:Literal></b></asp:Panel>
            <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
            cellspacing="1" cellpadding="1" >

            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label3" runat="server" >Nivel superior:</asp:Label></td>
                <td >
                    <asp:TextBox ID="nivpadeditTextBox" ReadOnly="true" runat="server" Font-Size="Small" MaxLength="40"  style="width:98%; text-align:left; text-transform:uppercase;" ></asp:TextBox>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hsedit">
                    <asp:Label ID="Label1" runat="server" >Descripción:</asp:Label></td>
                <td >
                    <asp:TextBox ID="desniveditTextBox" runat="server" Font-Size="Small" MaxLength="40"  style="width:98%; text-align:left; text-transform:uppercase;" ></asp:TextBox><asp:HiddenField ID="idnivHiddenField" runat="server" /><asp:HiddenField ID="idparHiddenField" runat="server" />
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="desniveditTextBox" Display="None" ErrorMessage="Ingrese la descripción del Nivel de Aprobación"  ValidationGroup="edit" ></asp:RequiredFieldValidator>
                    <cc1:ValidatorCalloutExtender ID="vce1" runat="server" TargetControlID="rfv1" ></cc1:ValidatorCalloutExtender>
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

        <div>
            <!--Inicio Panel agregar lineas-->         
            <asp:Button ID="dummyLineaBusqButton" runat="server" style="display:none" />  
            <cc1:ModalPopupExtender ID="lineaeditModalPopupExtender" runat="server" TargetControlID="dummyLineaBusqButton" CancelControlID="cancelareditLineaButton"  PopupControlID="editLineaPanel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloLineaPanel" BackgroundCssClass="modalBackground" X="170"
                                   Y="150" ></cc1:ModalPopupExtender>
            <asp:Panel ID="editLineaPanel" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="600" Height="150">  
                <asp:Panel ID="tituloLineaPanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
                    <asp:Literal ID="tituloLineaEditLiteral" runat="server" Text="Asignar Línea de Producción"></asp:Literal></b></asp:Panel>
                    <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
                    cellspacing="1" cellpadding="1" >

                    <tr style="background-color:#F3F3F3; ">
                        <td class="hsedittit" style="text-align:center;" colspan="3">
                            <asp:Label ID="desnivapreditTextBox" runat="server" ></asp:Label>
                        </td>  
                    </tr>
                        <tr style="background-color:#F3F3F3; ">
                            <td class="hsedit">
                                <asp:Label ID="Label9" runat="server" >Línea:</asp:Label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="limproeditDropDownList" runat="server" AutoPostBack="False">
                                     <asp:ListItem Value="0" Text="[Seleccione la Línea de Producción]"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                                ControlToValidate="limproeditDropDownList" CssClass="validmessage" 
                                EnableClientScript="true" ErrorMessage="Seleccione la clasificación" InitialValue="0" 
                                SetFocusOnError="true" ValidationGroup="editLinea"></asp:RequiredFieldValidator>
                            </td>  
                        </tr>

                        <tr>
                            <td colspan="3"><asp:Label ID="errortarLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                        <div align="right">
                        <asp:Button ID="aceptareditLineaButton" runat="server" Width="70" Text="Aceptar" 
                                ValidationGroup="editLinea" onclick="aceptareditLineaButton_Click"/>
                        &nbsp;
                        <asp:Button ID="cancelareditLineaButton" runat="server" Width="70" Text="Cancelar" 
                                CausesValidation="false"  />
                    </div>
            </asp:Panel>    

               <!--Fin Panel agregar y modificar tareas-->   
        </div>                

        <div>
            <!--Inicio Panel agregar roles-->         
            <asp:Button ID="dummyrolButton" runat="server" style="display:none" />  
            <cc1:ModalPopupExtender ID="roleditModalPopupExtender" runat="server" TargetControlID="dummyrolButton" CancelControlID="cancelareditRolButton"  PopupControlID="editRolPanel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloRolPanel" BackgroundCssClass="modalBackground" X="170"
                                   Y="150" ></cc1:ModalPopupExtender>
            <asp:Panel ID="editRolPanel" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="600" Height="150">  
                <asp:Panel ID="tituloRolPanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
                    <asp:Literal ID="tituloRolEditLiteral" runat="server" Text="Agregar Rol"></asp:Literal></b></asp:Panel>
                    <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
                    cellspacing="1" cellpadding="1" >

                    <tr style="background-color:#F3F3F3; ">
                        <td class="hsedittit" style="text-align:center;" colspan="3">
                            <asp:Label ID="desnivaprReditLabel" runat="server" ></asp:Label>
                        </td>  
                    </tr>
                        <tr style="background-color:#F3F3F3; ">
                            <td class="hsedit">
                                <asp:Label ID="Label5" runat="server" >Línea:</asp:Label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="roleditDropDownList" runat="server" AutoPostBack="False">
                                     <asp:ListItem Value="0" Text="[Seleccione el Rol a agregar]"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="roleditDropDownList" CssClass="validmessage" 
                                EnableClientScript="true" ErrorMessage="Seleccione el Rol a agregar" InitialValue="0" 
                                SetFocusOnError="true" ValidationGroup="editRol"></asp:RequiredFieldValidator>
                            </td>  
                        </tr>

                        <tr>
                            <td colspan="3"><asp:Label ID="errorgrupoLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                        <div align="right">
                        <asp:Button ID="aceptareditRolButton" runat="server" Width="70" Text="Aceptar" 
                                ValidationGroup="editRol" onclick="aceptareditRolButton_Click"/>
                        &nbsp;
                        <asp:Button ID="cancelareditRolButton" runat="server" Width="70" Text="Cancelar" 
                                CausesValidation="false"  />
                    </div>
            </asp:Panel>    

               <!--Fin Panel agregar y modificar tareas-->   
        </div>                

    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
