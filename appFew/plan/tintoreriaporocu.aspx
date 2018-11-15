<%@ Page Title="Información Plan de Producción Tintorería" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tintoreriaporocu.aspx.cs" Inherits="appFew.plan.tintoreriaporocu" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%=ResolveClientUrl("~/Scripts/bootstrap.min.js")%>" type="text/javascript" ></script>     
    <script src="<%=ResolveClientUrl("~/Scripts/bootbox.min.js")%>" type="text/javascript" ></script>     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="tsm" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="up" UpdateMode="Conditional" >

    <ContentTemplate>
    <div>
        <asp:Label ID="Label7" runat="server" Text="Unidad de Proceso:"></asp:Label>
        <asp:DropDownList ID="unidadDropDownList" runat="server" CssClass="labedit" >
                                        <asp:ListItem Value="0" Text="[Seleccione la Unidad de Proceso]"></asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="unidadDropDownList" CssClass="validmessage" EnableClientScript="true" ErrorMessage="Seleccione la Unidad de Proceso" InitialValue="0" 
                                        SetFocusOnError="true" ValidationGroup="buscarGroup"></asp:RequiredFieldValidator>
        <asp:Button ID="buscarButton" runat="server" CommandName="Search" Text="Buscar" ValidationGroup="buscarGroup" ClientIDMode="Static" OnClick="buscarButton_Click" />                                   
    </div>
    <div style="overflow:scroll; height:0px; min-height: calc(100vh - 130px);">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" 
            ShowBackButton="False" ShowFindControls="False" 
            ShowPageNavigationControls="False" ShowZoomControl="False" 
        Width="100%" Height="100%" SizeToReportContent="True"  >
            <LocalReport EnableHyperlinks="True">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <div>
        <asp:Label ID="errorLabel" runat="server" Text=""></asp:Label>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
