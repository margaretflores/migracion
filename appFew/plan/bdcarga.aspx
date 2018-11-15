<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="bdcarga.aspx.cs" Inherits="appFew.plan.bdcarga" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script src="<%=ResolveClientUrl("~/Scripts/jquery-1.7.1.min.js") %>" type="text/javascript"></script>
<script src="../js/jquery.blockUI.js" type="text/javascript" ></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler2);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler2);
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

        function BeginRequestHandler2(sender, args) {
            $.blockUI();
        }

        function EndRequestHandler2(sender, args) {
            $.unblockUI();
        }

        
    </script>
    <asp:ScriptManager ID="tsm" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" Visible="false">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground"  />

    <asp:UpdatePanel runat="server" ID="up" UpdateMode="Conditional" >

    <ContentTemplate>
    <div >
        <table style="width: 800px;" >
            <tr>
                <td style="width: auto; height: 24px;" colspan="2">
                    <asp:Label ID="lblTitulo" runat="server" CssClass="titulo1" 
                        Font-Names="Verdana" Text="Generar base de datos de carga"></asp:Label>
                    
                </td>
                <td style="width: auto; height: 24px; " colspan="3" align="right" >
                    </td>
            </tr>
        </table>
        <div style="height:10px;"></div>

        <style>
            .row-header-blue
            {
                background-color: #383F52;
                height: 26px;
                text-align: left;
                
            }
            .row-header-blue td:first-child
            {
                padding-left: 10px;
                }
            
            .formulario tbody tr
            {
                height: 25px;
                background-color:#F3F3F3;
            }
                                    
            .formulario tbody tr:nth-child(odd)
            {
                background-color: #E3E3E3;
            }
            .formulario tbody tr td:first-child
            {
                padding-left: 10px;
            }
        </style>
        <table style="width: 800px" cellspacing="0" class="formulario">
        <thead>
            <tr class="row-header-blue">
                <td colspan="2" >
                    <asp:Label ID="Label12" runat="server" CssClass="encabezado1" Font-Bold="True" 
                        Font-Names="Verdana" Text="Periodo">Plan de Producción</asp:Label>
                </td>
                <td colspan="2" >
                    
                </td>
            </tr>
        </thead>
        <tbody>                        
            <tr >
                <td style="width:300px;" >
                    <asp:Label ID="ultimacaptionLabel" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="X-Small">Fecha y hora de última generación de base de datos:</asp:Label></td>
                <td colspan="3" style="padding-left:5px;">
                    <asp:Label ID="ultimaLabel" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>                    
                </td>

            </tr>
            <tr >
                <td colspan="4">
                    
                </td>
            </tr>
            <tr >
                <td colspan="2" style="height: 16px;">
                    <asp:Button ID="generarButton" runat="server" CommandName="Carga" Text="Generar"
                        ValidationGroup="GuardarGroup" Style="width: 72px;" OnClick="generarButton_Click" ClientIDMode="Static" />
                </td>
                <td colspan="2" style="padding-left: 5px;" align="left">

                </td>
            </tr>
            <tr>
                <td colspan="4" style="background-color:#383F52; height: 26px;
                    text-align: left">
                    </td>
            </tr>
            </tbody>
        </table>
        <div style="width: 800px;">
            <asp:Label ID="ErrorLabel" CssClass="titulo2" runat="server" Font-Bold="False" Font-Names="Verdana"
                Font-Size="Small" EnableViewState="False"></asp:Label>
        </div>
    </div>

    </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
