<%@ Page Title="Solicitud Autorización Producto No Conforme" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="solprodnoconforme.aspx.cs" Inherits="appFew.ncon.solprodnoconforme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery.blockUI.js" type="text/javascript" ></script>

  <link rel="stylesheet" href="../Content/ui/1.12.0/themes/base/jquery-ui.css" />
  <link rel="stylesheet" href="/resources/demos/style.css" />
      <link href="../Content/MyStyles.css" type="text/css" rel="stylesheet" />

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
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Solicitud Autorización Producto No Conforme"></asp:Label></td>
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
                            <asp:Button ID="agregarButton" runat="server" CommandName="Save" Text="Agregar" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="agregarButton_Click" 
                                 />
                        </td>
                        <td class="toolbarsep" >
                        </td>
                        <td style="padding-left: 5px; " align="left">
                            <asp:Button ID="anularButton" runat="server" CommandName="Save" Text="Anular" 
                                    ValidationGroup="BuscarGroup" Enabled="False" style="width:72px;" onclick="anularButton_Click" 
                                 />
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
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Estado:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:DropDownList ID="estautDropDownList" runat="server" AutoPostBack="False" >
                            <asp:ListItem Value="S" Text="Solcitado"></asp:ListItem>
                            <asp:ListItem Value="A" Text="Autorizado"></asp:ListItem>
                            <asp:ListItem Value="B" Text="Anulado"></asp:ListItem>
                    </asp:DropDownList>
                   
                </td> 
            </tr>
            <tr>
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Del:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:TextBox ID="fechaTextBox" runat="server" MaxLength="10"  style="width:100px;" ></asp:TextBox>
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/img/cal.gif" ImageAlign="Bottom"
                        runat="server" />
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="fechaTextBox"
                        Format="dd/MM/yyyy">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidatorFecLiq"  runat="server" 
                        ValidationGroup="GuardarGroup" ControlToValidate="fechaTextBox" 
                        ErrorMessage="Ingrese la fecha inicio" EnableClientScript="true" 
                        SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>
                </td>              
                <td style="text-align:left; width:20%; " valign="middle">
                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Al:" ></asp:Label>
                </td>
                <td colspan="1" align="left">
                    <asp:TextBox ID="fecfinTextBox" runat="server" MaxLength="10"  style="width:100px;" ></asp:TextBox>
                    <asp:ImageButton ID="pupup2ImageButton" ImageUrl="~/img/cal.gif" ImageAlign="Bottom"
                        runat="server" />
                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="pupup2ImageButton" runat="server" TargetControlID="fecfinTextBox"
                        Format="dd/MM/yyyy">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator1"  runat="server" 
                        ValidationGroup="GuardarGroup" ControlToValidate="fecfinTextBox" 
                        ErrorMessage="Ingrese la fecha fin" EnableClientScript="true" 
                        SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>                   
                </td> 
            </tr>
            <tr>
                <td style="text-align:left; width:20%; " valign="middle">
                    
                </td>
                <td colspan="1" align="left">
                   
                </td>              
                <td style="text-align:left; width:20%; " valign="middle">
                    
                </td>
                <td colspan="1" align="left">

                   
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
                    <asp:GridView ID="cabeceraGridView" Width="100%" runat="server" DataKeyNames="PRNCIDNC"  
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
                            <asp:BoundField DataField="PRNCIDNC" HeaderText="Número" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="hsleft"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRNCLIPR" HeaderText="IDLP" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                                <HeaderStyle CssClass="hideGridColumn" />
                                <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LIPRDESC" HeaderText="Línea" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRNCPART" HeaderText="Partida" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRNCFEEM" HeaderText="Fecha" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  dataformatstring="{0:dd/MM/yyyy}" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRNCUSCR" HeaderText="Usuario" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
                                <ItemStyle CssClass="is" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRNCESTA" HeaderText="Estado" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                <HeaderStyle CssClass="hsleft" />
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
                    <asp:HiddenField ID="idspncHiddenField" runat="server" />
                    <asp:HiddenField ID="iddeHiddenField" runat="server" />
                </div>

                 </td>

            </tr>
            <tr class="fs">
                <td style="width:16%; padding:5px; text-align: left" >Acción:</td>
                <td colspan="1" style="padding:5px; width:45%; text-align: left" ><b><asp:Label ID="descripcion3Label" runat="server" ></asp:Label><b></td>
                <td style="width:14%; padding:5px; text-align: left" >Almacén:</td>
                <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="localizacionLabel" runat="server" ></asp:Label><b></td>
            </tr>
            <tr>
                <td colspan="4">
                      <div>
                        <table width="100%" style="padding:1px" class="iscb" >
                            <tr>
                                <td width="14%">Línea:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="lineashowLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td width="14%">Proceso:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="procesoshowLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td width="14%">Variable:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="variableshowLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td width="14%">Partida:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="partidashowLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td width="14%">Resultado:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="resultadoshowLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td width="14%">Estandar:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="estandarshowLabel" runat="server" ></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td width="14%">Fecha Límite:</td>
                                <td></td>
                                <td width="30%"><b><asp:Label ID="feclimLabel" runat="server" ></asp:Label></b></td>
                                <td style="padding:2px"></td>
                                <td width="14%"></td>
                                <td></td>
                                <td width="30%"></td>
                            </tr>
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
        <div>
            <!--Panel to Show Fuera de Estandar Search-->         
            <asp:Button ID="dummyfueraestandarButton" runat="server" style="display:none" />  
            <cc1:ModalPopupExtender ID="busqfueraestandarModalPopupExtender" runat="server" TargetControlID="dummyfueraestandarButton" CancelControlID="cancelfuesButton"  PopupControlID="contentBusqpanel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="busqfueraestandarPanel" BackgroundCssClass="modalBackground" X="30"
                                    Y="30" ></cc1:ModalPopupExtender>
            <asp:Panel ID="contentBusqpanel" runat="server" style="display:none; background-color:#F3F3F3;" ForeColor="Black" Width="730" Height="420">  
                <asp:Panel ID="busqfueraestandarPanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#383F52" ForeColor="White" Height="25" ><b>Generar Solicitud Autorización Producto No Conforme - Paso 1</b></asp:Panel>
                    <table width="100%" style="padding:0px; height:86%">
                    <tr>
                        <td colspan="4" valign="top">
                            <div class="GridContainer">
                                <table>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="Label1" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Seleccione la Variable Fuera de Estándar No Liberada"></asp:Label>
                                        </td>
                                    </tr>
                                        <tr>
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Línea de Producción:" ></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:DropDownList ID="linprofilfeDropDownList" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="linprofilDropDownList_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="[Filtrar por Línea de Producción]"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>              
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo de Proceso:" ></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:DropDownList ID="tipprofilDropDownList" runat="server" AutoPostBack="False" >
                                                    <asp:ListItem Value="0" Text="[Filtrar por Tipo de Proceso]"></asp:ListItem>
                                            </asp:DropDownList>
                   
                                        </td>              
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Variable:" ></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:DropDownList ID="varnomfilDropDownList" runat="server" AutoPostBack="False" >
                                                    <asp:ListItem Value="0" Text="[Filtrar por Variable]"></asp:ListItem>
                                            </asp:DropDownList>
                   
                                        </td>              
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            
                                        </td>
                                        <td colspan="1" align="left">
                  
                                        </td>                 
                                     </tr>
                                </table>
                                <hr />
                                <asp:TextBox ID="buscarfueraestandarTextBox" runat="server" />
                                <asp:Button ID="buscarfueraestandarButton" runat="server" CommandName="Search" Text="Buscar" ClientIDMode="Static" onclick="buscarfuesButton_Click" />
                                <hr />
                                <asp:GridView ID="fueraestandarGridView" Width="100%" runat="server" DataKeyNames="ATFEIDED"  
                                    AutoGenerateColumns="False" emptydatatext="No data available."
                                    BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                                    BorderStyle="Solid" BorderWidth="1px" 
                                    ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                                    Font-Size="14px" AllowPaging="true" PageSize="15"
                                    onrowcreated="fueraestandarGridView_RowCreated" 
                                    onrowdatabound="fueraestandarGridView_RowDataBound" 
                                    onselectedindexchanged="fueraestandarGridView_SelectedIndexChanged" 
                                    onselectedindexchanging="fueraestandarGridView_SelectedIndexChanging" 
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
                                        <asp:BoundField DataField="ENVAVALO" HeaderText="Resultado" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                            <HeaderStyle CssClass="hsleft" />
                                            <ItemStyle CssClass="is" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENVAVAE1" HeaderText="Estánd 1" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" >
                                            <HeaderStyle CssClass="hideGridColumn" />
                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENVAVAE2" HeaderText="Estánd 2" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" >
                                            <HeaderStyle CssClass="hideGridColumn" />
                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENSAFEEN" HeaderText="Fecha" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is"  dataformatstring="{0:dd/MM/yyyy}" >
                                            <HeaderStyle CssClass="hsleft" />
                                            <ItemStyle CssClass="is" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ATFEUSCR" HeaderText="Usuario" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                            <HeaderStyle CssClass="hsleft" />
                                            <ItemStyle CssClass="is" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ATFEESTA" HeaderText="Estado" HeaderStyle-CssClass="hsleft" ItemStyle-CssClass="is" >
                                            <HeaderStyle CssClass="hsleft" />
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
                                    <asp:Button ID="primerofueraestandarButton" runat="server" CommandName="PagerPrimero" 
                                            Text="Primero" ClientIDMode="Static" onclick="primerofueraestandarButton_Click" />
                                    <asp:Button ID="anteriorfueraestandarButton" runat="server" CommandName="PagerAnterior" 
                                            Text="Anterior" ClientIDMode="Static" onclick="anteriorfueraestandarButton_Click" />
                                    <asp:Button ID="siguientefueraestandarButton" runat="server" CommandName="PagerSiguiente" 
                                            Text="Siguiente" ClientIDMode="Static" onclick="siguientefueraestandarButton_Click" />
                                    <asp:Button ID="ultimofueraestandarButton" runat="server" CommandName="PagerUltimo" 
                                            Text="Ultimo" ClientIDMode="Static" onclick="ultimofueraestandarButton_Click" />
                                        <asp:Label ID="paginafueraestandarLabel" runat="server" Text="Pág: 1" CssClass="iscb" 
                                            Font-Bold="True"></asp:Label> 
                                        <asp:Label ID="totalfueraestandarLabel" runat="server" Text=" Total: 0" CssClass="iscb" 
                                            Font-Bold="True"></asp:Label>
                                        <asp:HiddenField ID="pageIndexfuesHiddenField" runat="server" Value="1" />
                                        <asp:HiddenField ID="totalfuesHiddenField" runat="server" Value="0" />
                                </div>
                                <asp:HiddenField ID="iddefuesHiddenField" runat="server" />
                                <asp:HiddenField ID="liprHiddenField" runat="server" />
                                <asp:HiddenField ID="tiprHiddenField" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:Button ID="aceptarfuesButton" runat="server" Width="70" Text="Siguiente" ValidationGroup="fueraestandarAceptar" onclick="siguientefuesButton_Click"/>
                    &nbsp;
                    <asp:Button ID="cancelfuesButton" runat="server" Width="70" Text="Cancelar" CausesValidation="false" />&nbsp;&nbsp;
                </div>
                <div align="left" style="padding-left:10px;">
                    <asp:Label ID="errorPopupfuesLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                </div>
            </asp:Panel>    
            <!--End of Panel to Show Items Detail-->  
        </div>
        <div>
            <!--Panel to Show Fuera de Estandar Search-->         
            <asp:Button ID="dummyfueraestandar2Button" runat="server" style="display:none" />  
            <cc1:ModalPopupExtender ID="fues2ModalPopupExtender" runat="server" TargetControlID="dummyfueraestandar2Button" CancelControlID="cancelarfues2Button"  PopupControlID="contentfues2Panel" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="busqfueraestandar2Panel" BackgroundCssClass="modalBackground" X="30"
                                    Y="30" ></cc1:ModalPopupExtender>
            <asp:Panel ID="contentfues2Panel" runat="server" style="display:none; background-color:#F3F3F3;" ForeColor="Black" Width="730" Height="420">  
                <asp:Panel ID="busqfueraestandar2Panel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#383F52" ForeColor="White" Height="25" ><b>Generar Solicitud Autorización Producto No Conforme - Paso 2</b></asp:Panel>
                    <table width="100%" style="padding:0px; ">
                    <tr>
                        <td colspan="4" valign="top">
                            <div class="GridContainer">
                                <table>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="Label2" runat="server" CssClass="titulo1ddl" Font-Names="Verdana" Text="Variable Fuera de Estándar No Liberada"></asp:Label>
                                        </td>
                                    </tr>
                                        <tr>
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Línea de Producción:" ></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:Label ID="lineaLabel" runat="server" Font-Names="Verdana" Font-Size="Small" ></asp:Label>
                                        </td>              
                                        <td style="text-align:left; width:20%; " valign="middle">
                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo de Proceso:" ></asp:Label>
                                        </td>
                                        <td colspan="1" align="left">
                                            <asp:Label ID="procesoLabel" runat="server" Font-Names="Verdana" Font-Size="Small" ></asp:Label>
                                        </td>              
                                    </tr>
                                </table>
                                <hr />
                            </div>
                        </td>
                    </tr>
                    <tr class="fs">
                        <td style="width:16%; padding:5px; text-align: left" >Variable:</td>
                        <td colspan="1" style="padding:5px; width:45%; text-align: left" ><b><asp:Label ID="desvarLabel" runat="server" ></asp:Label><b></td>
                        <td style="width:14%; padding:5px; text-align: left" >Partida:</td>
                        <td colspan="1" style="padding:5px; text-align: left" ><b><asp:Label ID="partida2Label" runat="server" ></asp:Label><b></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                                <div>
                                <table width="100%" style="padding:1px" class="iscb" >
                                    <tr>
                                        <td colspan="2">Ensayo: 
                                            <b><asp:Label ID="ensayoLabel" runat="server" ></asp:Label></b>
                                        </td>
                                        <td></td>
                                        <td style="padding:2px"></td>
                                        <td>Fecha:</td>
                                        <td><b><asp:Label ID="fechaLabel" runat="server" ></asp:Label></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left" style="padding:5px">Acción:
                                        
                                            <asp:RadioButtonList ID="accionRadioButtonList" runat="server" CssClass="RadioButtonList">
                                                <asp:ListItem Value="D" Text="Desechar" ></asp:ListItem>
                                                <asp:ListItem Value="R" Text="Reprocesar" Selected="True" ></asp:ListItem>
                                                <asp:ListItem Value="A" Text="Reasignación" ></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="padding:2px"></td>
                                        <td>Fecha Límite:</td>
                                        <td colspan="2" align="left">
                                            <asp:TextBox ID="feclimTextBox" runat="server" MaxLength="10"  style="width:100px;" ></asp:TextBox>
                                            <asp:ImageButton ID="feclimImageButton" ImageUrl="~/img/cal.gif" ImageAlign="Bottom"
                                                runat="server" />
                                            <cc1:calendarextender ID="feclimCalendarextender" PopupButtonID="feclimImageButton" runat="server" TargetControlID="feclimTextBox"
                                                Format="dd/MM/yyyy">
                                            </cc1:calendarextender>
                                            <asp:RequiredFieldValidator  ID="feclimRequiredFieldValidator"  runat="server" 
                                                ValidationGroup="fueraestandar2Aceptar" ControlToValidate="feclimTextBox" 
                                                ErrorMessage="Ingrese la fecha límite" EnableClientScript="true" 
                                                SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>                   
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left" style="padding:5px">Almacén:
                                            <asp:DropDownList ID="almdesDropDownList" runat="server" AutoPostBack="False" >
                                                    <asp:ListItem Value="0" Text="[Seleccione al almacén]"></asp:ListItem>
                                                    <asp:ListItem Value="101" Text="101 - Fuera de especificaciones"></asp:ListItem>
                                                    <asp:ListItem Value="102" Text="102 - Mercadería Fallada"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator  ID="almdesRequiredFieldValidator"  runat="server" 
                                                ValidationGroup="fueraestandar2Aceptar" ControlToValidate="almdesDropDownList" 
                                                ErrorMessage="Ingrese el almacén destino" EnableClientScript="true" InitialValue="0"
                                                SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>
                                        </td> 
                                        <td width="10%"></td>
                                        <td></td>
                                        <td width="30%"></td>
                                    </tr>
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
                                            <asp:TextBox ID="obspncTextBox"  width="96%" style="margin-left:2%;" runat="server" TextMode="MultiLine" />
                                            <asp:RequiredFieldValidator  ID="obspncRequiredFieldValidator"  runat="server" 
                                                ValidationGroup="fueraestandar2Aceptar" ControlToValidate="obspncTextBox" 
                                                ErrorMessage="Ingrese la fecha límite" EnableClientScript="true" 
                                                SetFocusOnError="true" CssClass="validmessage" Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                </table>
                                </div>
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:Button ID="aceptarfues2Button" runat="server" Width="70" Text="Aceptar" ValidationGroup="fueraestandar2Aceptar" onclick="aceptarfues2Button_Click"/>
                    &nbsp;
                    <asp:Button ID="cancelarfues2Button" runat="server" Width="70" Text="Cancelar" CausesValidation="false" />&nbsp;&nbsp;
                </div>
                <div align="left" style="padding-left:10px;">
                    <asp:Label ID="errorPopupfues2Label" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="Small" EnableViewState="False"></asp:Label>
                </div>
            </asp:Panel>    
            <!--End of Panel to Show Items Detail-->  
        </div>
    </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
