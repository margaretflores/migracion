<%@ Page Title="Tipos de Proceso que Intervienen para la Carga" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="tiposprocesocarga.aspx.cs" Inherits="appFew.mae.tiposprocesocarga" %>
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
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1" Font-Names="Verdana" Text="Tipos de Proceso que Intervienen para la Carga"></asp:Label></td>
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
                            
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="modificarButton" runat="server" CommandName="Save" Text="Agregar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="modificarButton_Click" 
                                 />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:ImageButton ID="eliminarButton" runat="server" Text="Remover" Enabled="False"
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
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="ARBOSEPE,ARBOSEPI,ARBONUPE,RUTASETP"  
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
                            <asp:BoundField DataField="RUTASETP" HeaderText="SETP" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARBOSEPE" HeaderText="SEPE" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARBOSEPI" HeaderText="SEPI" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARBONUPE" HeaderText="Contrato" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARBOTIPE" HeaderText="L" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARBOARTI" HeaderText="Código" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PENDFERP" HeaderText="F Compr." HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" dataformatstring="{0:dd/MM/yyyy}" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MCSICL" HeaderText="Cliente" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PENDCAPP" HeaderText="Pend Prog" HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n2}" >
                                <HeaderStyle CssClass="hs" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PENDSTAT" HeaderText="E" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LIPRDESC" HeaderText="Línea Producción" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TUPRDESC" HeaderText="Unidad Proceso" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RUTASETP" HeaderText="Sec" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" ItemStyle-HorizontalAlign="Right"  >
                                <HeaderStyle CssClass="hs" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                             <asp:BoundField DataField="TTPRDESC" HeaderText="Tipo Proceso" HeaderStyle-CssClass="hs" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RUPRSETP" HeaderText="SecCar" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RUPRSETC" HeaderText="Carga Prod." HeaderStyle-CssClass="hsright" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsright" />
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
                <td style="width:15%; padding:5px; text-align: left" >Tipo Proceso:</td>
                <td colspan="1" style="padding:5px; width:40%; text-align: left" ><b><asp:Label ID="descripcion3Label" runat="server" ></asp:Label><b></td>
                <td style="width:20%; padding:5px; text-align: left" >Interviene para la Carga:</td>
                <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="localizacionLabel" runat="server" ></asp:Label><b></td>
            </tr>
            <tr>
                <td colspan="4">
                      <div>
                        <table width="100%" style="padding:1px" class="iscb" >
                            <tr>
                                <td>Contrato</td>
                                <td>:</td>
                                <td><b><asp:Label ID="nupeLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Estado</td>
                                <td>:</td>
                                <td><b><asp:Label ID="statLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Tipo</td>
                                <td>:</td>
                                <td><b><asp:Label ID="tipeLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Línea Producción</td>
                                <td>:</td>
                                <td><b><asp:Label ID="lindesLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Artículo</td>
                                <td>:</td>
                                <td><b><asp:Label ID="artiLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Unidad Proceso</td>
                                <td>:</td>
                                <td><b><asp:Label ID="unidesLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Fecha Compromiso</td>
                                <td>:</td>
                                <td><b><asp:Label ID="ferpLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Secuencia</td>
                                <td>:</td>
                                <td><b><asp:Label ID="setpLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Cliente</td>
                                <td>:</td>
                                <td><b><asp:Label ID="siclLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Tipo Proceso</td>
                                <td>:</td>
                                <td><b><asp:Label ID="prodesLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td>Pendiente Programar</td>
                                <td>:</td>
                                <td><b><asp:Label ID="cappLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td>Interviene para la Carga</td>
                                <td>:</td>
                                <td><b><asp:Label ID="setcLabel" runat="server" ></asp:Label></b></td>
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
    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
