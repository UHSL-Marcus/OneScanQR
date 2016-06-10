<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AdminWebPortal.Views.Login.Login1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Views/Login.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <asp:Image ID="qrImg" runat="server" />
    <asp:Button ID="hiddenNewQRBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenNewQRBtn_Click" />
     <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
        <ContentTemplate>
            <asp:Button ID="hiddenStatusCheckBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenStatusCheckBtn_Click" />
            <asp:Label ID="tempTest" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
