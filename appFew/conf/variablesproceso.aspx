﻿<%@ Page Title="Variables asignadas a Procesos" Language="C#" MasterPageFile="~/Site.Master" enableEventValidation="false" AutoEventWireup="true" CodeBehind="variablesproceso.aspx.cs" Inherits="appFew.conf.variablesproceso" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
        .column-left{ float: left; width: 38%; padding-left: 3%; }
        .column-center{ display: inline-block; width: 18%; }
        .column-right{ float: right; width: 41%; }
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
                        Font-Names="Verdana" Text="Variables asignadas a Procesos"></asp:Label>
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
        <table style="width: 700px" cellspacing="0">
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
                <td style="text-align:left " valign="middle">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Línea de Producción:" ></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="linproDropDownList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="linproDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="[Seleccione la Línea de Producción]"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="linproDropDownList" CssClass="validmessage" 
                    EnableClientScript="false" ErrorMessage="Seleccione la Línea de Producción" InitialValue="0" 
                    SetFocusOnError="true" ValidationGroup="BuscarGroup"></asp:RequiredFieldValidator>
                </td>  
            </tr>
            <tr style="background-color:#F3F3F3">
                <td style="text-align:left " valign="middle">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Tipo de Proceso:" ></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="tipproDropDownList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="tipproDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="[Seleccione el Tipo de Proceso]"></asp:ListItem>
                    </asp:DropDownList>
                        
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tipproDropDownList" CssClass="validmessage" 
                    EnableClientScript="false" ErrorMessage="Seleccione el Tipo de Proceso" InitialValue="0" 
                    SetFocusOnError="true" ValidationGroup="GuardarGroup"></asp:RequiredFieldValidator>
                </td>  
            </tr>
            <tr style="background-color:#E3E3E3">
                <td style="text-align:left " valign="middle">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" Text="Máquina:" ></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="maquinDropDownList" runat="server" AutoPostBack="True" onselectedindexchanged="maquinDropDownList_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="[Todas]"></asp:ListItem>
                    </asp:DropDownList>
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
                    <asp:Button ID="resumenButton" runat="server" CommandName="Search" Text="Ver Resumen" ValidationGroup="ListadoVarsGroup" OnClick="resumenButton_Click" style="display:inline;" />
                </td>
            </tr>  
            <tr style="background-color:#E3E3E3">
                <td colspan="4" style="height: 16px;"></td>
            </tr>
            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                </td>
            </tr>
            <tr >
                <td colspan="4" >
                    <div>
                        <div runat="server" id="div2" style="padding-left: 3%;">
                            <asp:TextBox ID="txtSearch" runat="server" />
                            <asp:Button ID="filtraVarsButton" runat="server" CommandName="Search" 
                                Text="Buscar" ClientIDMode="Static" onclick="filtraVarsButton_Click" />
                        </div>
                        <hr />
                        <div id="the-whole-thing" >
                            <div id="leftThing" class="column-left"  >

                    <asp:GridView ID="dispGridView" runat="server" style="height: 90%; width:90%" DataKeyNames="MVARIDVA"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#6B696B" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" AllowPaging="false" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        onrowdatabound="dispGridView_RowDataBound" 
                        onselectedindexchanging="gridView_SelectedIndexChanging" 
                        onselectedindexchanged="gridView_SelectedIndexChanged" 
                        Font-Size="11px" >
                        <Columns>
                            <asp:BoundField DataField="MVARIDVA" HeaderText="ID" 
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARNOMB" HeaderText="Nombre Variable" >
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
                                    <div runat="server" id="div1" style="padding-left: 3%;">
                                            <asp:Button ID="agregarButton" runat="server" CommandName="Search" style="width:70%;"
                                                Text="Agregar --&gt;" ClientIDMode="Static" onclick="agregarButton_Click" />
                                            <asp:Button ID="removerButton" runat="server" CommandName="Search" style="width:70%;"
                                                Text="&lt;-- Remover" ClientIDMode="Static" onclick="removerButton_Click" />        
		                            </div>
                            </div>
                            <div id="rightThing" class="column-right">
                    <asp:GridView ID="asigvarGridView" runat="server" style="height: 90%; width:90%" DataKeyNames="MVARIDVA"  
                        AutoGenerateColumns="False" emptydatatext="No data available."
                        BackColor="White" BorderColor="#AAAAAA" cellpadding="10" cellspacing="5" 
                        BorderStyle="Solid" BorderWidth="1px" AllowPaging="false" 
                        ForeColor="Black" GridLines="Vertical" Font-Names="Verdana" 
                        onrowdatabound="asigvarGridView_RowDataBound" 
                        onselectedindexchanging="gridView_SelectedIndexChanging" 
                        onselectedindexchanged="gridView_SelectedIndexChanged" 
                        Font-Size="11px" >
                        <Columns>
                            <asp:BoundField DataField="MVARIDVA" HeaderText="ID" 
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"  >
                            <HeaderStyle CssClass="hideGridColumn" />
                            <ItemStyle CssClass="hideGridColumn" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MVARNOMB" HeaderText="Nombre Variable" >
                            <ItemStyle CssClass="isavas" />
                            </asp:BoundField>
                            <asp:TemplateField  HeaderText="Obligatorio" >
                                <ItemTemplate><asp:CheckBox runat="server" Text="" ID="checkBoxProv" checked='<%# Eval("VRPROBLI2") %>' ></asp:CheckBox>
                                </ItemTemplate>
                                <ItemStyle CssClass="isavas" HorizontalAlign="Center" />
                            </asp:TemplateField>

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
                <td style="height: 58px" colspan="3">
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


                <!--Panel to Edit record-->         
    <asp:Button ID="dummy3BusqButton" runat="server" style="display:none" />  
    <cc1:ModalPopupExtender ID="mpe2" runat="server" TargetControlID="dummy3BusqButton" CancelControlID="canelarBusqButton"  PopupControlID="panelEdit" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" PopupDragHandleControlID="tituloBusqpanel" BackgroundCssClass="modalBackground" X="50"
                           Y="50" ></cc1:ModalPopupExtender>
    <asp:Panel ID="panelEdit" runat="server" style="display:none; background-color:#E3E3E3;" ForeColor="Black" Width="649" Height="450">  
        <asp:Panel ID="tituloBusqpanel" runat="server" style="cursor:move;font-family:Tahoma;padding:2px;" HorizontalAlign="Center" BackColor="#660000" ForeColor="White" Height="25"  ><b>
        </asp:Panel>
            <table width="99%" style="border: thin solid #6B696B; height:85%; margin-top: 0.2%; margin-left: 0.3%; margin-right: 0.2%;" 
            cellspacing="1" cellpadding="1" >

            <tr style="background-color:#F3F3F3; ">
                <td valign="top" >
                    <div >
                        <rsweb:ReportViewer ID="listadoReportViewer" runat="server" BackColor="White" ShowBackButton="False" ShowFindControls="False" ShowPageNavigationControls="True" ShowZoomControl="True" AsyncRendering="false" 
                        Width="100%" Height="420" SizeToReportContent="True"  >
                            <LocalReport EnableHyperlinks="True">
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </div>
                </td>  
            </tr>

            </table>
            <div align="right">
                <asp:Button ID="canelarBusqButton" runat="server" Width="70" Text="Cerrar" CausesValidation="false" />&nbsp;&nbsp;
            </div>
    </asp:Panel>    

               <!--End of Panel to edit record-->   


            </div>

        </ContentTemplate>        
    </asp:UpdatePanel>

</asp:Content>
