<%@ Page Title="Variables Pendientes de Liberar" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="pendienteliberar.aspx.cs" Inherits="appFew.cali.pendienteliberar" %>
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
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Variables Pendientes de Liberar"></asp:Label></td>
                <td style="width: auto; height: 24px; " colspan="2" align="right" >
                    &nbsp;</td>
            </tr>
        </table>
        <table style="width: 750px" cellspacing="0">
                   

            <tr>
                <td class="toolbar" colspan="4" >
                     <table id="PosTable" runat="server" style="width: auto; font-size: 11px; font-family: Verdana;" cellpadding="5" cellspacing="0">
                        <tr>


                        <td style="padding-left: 5px; " align="left">

                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="modificarButton" runat="server" CommandName="Save" Text="Ver Detalle" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="modificarButton_Click" 
                                 />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">

                        </td>                            
                            
                        </tr>
                    </table>
                    
                </td>

            </tr>
            <tr>
                <td colspan="4">
        <div class="GridContainer">
        <table>
            <tr>
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Línea de Producción:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:DropDownList ID="linprofilDropDownList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="linprofilDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="[Filtrar por Línea de Producción]"></asp:ListItem>
                    </asp:DropDownList>
                </td>              
                <td style="width: auto; " colspan="2" align="right" ></td>
            </tr>
            <tr>
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo de Proceso:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:DropDownList ID="tipprofilDropDownList" runat="server" AutoPostBack="False" >
                            <asp:ListItem Value="0" Text="[Filtrar por Tipo de Proceso]"></asp:ListItem>
                    </asp:DropDownList>
                   
                </td>              
                <td style="width: auto; " colspan="2" align="right" ></td>
            </tr>
            <tr>
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Variable:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:DropDownList ID="varnomfilDropDownList" runat="server" AutoPostBack="False" >
                            <asp:ListItem Value="0" Text="[Filtrar por Variable]"></asp:ListItem>
                    </asp:DropDownList>
                   
                </td>              
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Estado:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:DropDownList ID="estautDropDownList" runat="server" AutoPostBack="False" >
                            <asp:ListItem Value="G" Text="Pendientes"></asp:ListItem>
                            <asp:ListItem Value="L" Text="Liberadas"></asp:ListItem>
                    </asp:DropDownList>
                   
                </td>                 
              </tr>
            </table>
            <hr />

            <asp:TextBox ID="txtSearch" runat="server" />
            <asp:Button ID="buscarButton" runat="server" 
                CommandName="Search" Text="Buscar" ClientIDMode="Static" 
                onclick="buscarButton_Click" />
            <hr />
<%--            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="ATFEIDED"  
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
                            <asp:BoundField DataField="ATFEIDED" HeaderText="IDDETVAR" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENSACLPR" HeaderText="IDLN" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LIPRDESC" HeaderText="Línea" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENSACTPR" HeaderText="IDPR" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TTPRDESC" HeaderText="Proceso" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARDECO" HeaderText="Variable" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>

                            <asp:BoundField DataField="ENSAPART" HeaderText="Partida" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENVAVALO" HeaderText="Resultado" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENVAVAE1" HeaderText="Estándar 1" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENVAVAE2" HeaderText="Estándar 2" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ENSAFEEN" HeaderText="Fecha" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is"  dataformatstring="{0:dd/MM/yyyy}" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ATFEUSCR" HeaderText="Usuario" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ATFEESTA" HeaderText="Estado" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
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
                    <asp:HiddenField ID="iddeHiddenField" runat="server" />
                </div>

                 </td>

            </tr>
            <tr class="fs">
                <td style="width:16%; padding:5px; text-align: left" >Variable:</td>
                <td colspan="1" style="padding:5px; width:45%; text-align: left" ><b><asp:Label ID="descripcion3Label" runat="server" ></asp:Label><b></td>
                <td style="width:14%; padding:5px; text-align: left" ></td>
                <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="localizacionLabel" runat="server" ></asp:Label><b></td>
            </tr>
            <tr>
                <td colspan="4">
                      <div>
                        <table width="100%" style="padding:1px" class="iscb" >
                            <tr>
                                <td width="30%">Observación Solicitud</td>
                                <td></td>
                                <td width="30%"></td>
                                <td style="padding:2px"></td>
                                <td width="10%"></td>
                                <td></td>
                                <td width="30%"></td>
                            </tr>
                            <tr>
                                <td colspan="7" >
                                    <asp:TextBox ID="observacionTextBox"  width="96%" style="margin-left:2%;" 
                                        runat="server" TextMode="MultiLine" ReadOnly="True" />
                                </td>
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
    <asp:Panel ID="panelEdit" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="580" Height="390">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
        <asp:Literal ID="tituloeditLiteral" runat="server" Text="Liberar Variable"></asp:Literal></b></asp:Panel>
            <table width="99%" style="border: thin solid #6B696B; margin-top: 0.5%; margin-left: 0.5%; margin-right: 0.5%;" 
            cellspacing="1" cellpadding="1" >
            <tr style="background-color:#F3F3F3; ">
                <td class="hs" width="30%" >
                    <asp:Label ID="Label1" runat="server" >Línea:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="lineditLabel" runat="server" ></asp:Label>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label2" runat="server" >Proceso:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="proeditLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label9" runat="server" >Máquina:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="maqeditLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label12" runat="server" >Partida:</asp:Label></td>
                <td class="hsb">
                    <b><asp:Label ID="pareditLabel" runat="server" ></asp:Label><b>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label4" runat="server" >Variable:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="vareditLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label5" runat="server" >Resultado:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="reseditLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label14" runat="server" >Valor Estándar 1:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="es1editLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs">
                    <asp:Label ID="Label16" runat="server" >Valor Estándar 2:</asp:Label></td>
                <td class="hsb">
                    <asp:Label ID="es2editLabel" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs" colspan="2">
                    <asp:Label ID="Label6" runat="server" >Observación Solicitud:</asp:Label>
                    <asp:TextBox ID="osoeditTextBox"  width="96%" style="margin-left:2%;" runat="server" TextMode="MultiLine" ReadOnly="True" />
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3; ">
                <td class="hs" colspan="2">
                    <asp:Label ID="Label3" runat="server" >Observación:</asp:Label>
                
                    <asp:TextBox ID="obseditTextBox" MaxLength="500" width="96%" style="margin-left:2%;" runat="server" TextMode="MultiLine" ReadOnly="True" />
                </td>  
            </tr>

                <tr>
                    <td colspan="2"><asp:Label ID="ErrorLabelPopup" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
            </table>
                <div align="center">
                <asp:Button ID="canelarBusqButton" runat="server" Width="70" Text="Cerrar" CausesValidation="false" />&nbsp;&nbsp;
            </div>
    </asp:Panel>    

               <!--End of Panel to edit record-->   
    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
