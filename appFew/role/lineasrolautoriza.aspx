<%@ Page Title="Líneas de Producción por Rol" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="lineasrolautoriza.aspx.cs" Inherits="appFew.role.lineasrolautoriza" %>
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
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Líneas de Producción por Rol"></asp:Label></td>
                <td style="width: auto; height: 24px; " colspan="2" align="right" >
                    &nbsp;</td>
            </tr>
        </table>
        <table style="width: 750px; border: thin solid #6B696B; " >
            <tr>
                <td colspan="4">
            <asp:TextBox ID="txtSearch" runat="server" />
            <asp:Button ID="buscarButton" runat="server" 
                CommandName="Search" Text="Buscar" ClientIDMode="Static" 
                onclick="buscarButton_Click" />
            <hr />
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="GRUSCOGR"  
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
                            <asp:BoundField DataField="GRUSCOGR" HeaderText="Código" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GRUSDEGR" HeaderText="Descripción Rol" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ROAPCOAP" HeaderText="ROAPCOAP" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
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
                    <asp:HiddenField ID="codgruHiddenField" runat="server" />
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

               <!--Fin Panel agregar y modificar líneas-->   
        </div>                

    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
