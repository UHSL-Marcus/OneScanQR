<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" EnableViewStateMac="true" CodeBehind="AdminUserQR.aspx.cs" Inherits="AdminWebPortal.Views.Main.QRPages.AdminUserQR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Views/AdminUserQR.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <span ID="resultlabel"></span>
    <div runat="server" id="RegisterQRDiv" visible="false" ClientIDMode="Static">
        <asp:UpdatePanel ID="hiddenQRCompleteUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenQRCompleteBtn" runat="server" Text="" CssClass="hidden" ClientIDMode="Static" OnClick="hiddenQRCompleteBtn_Click" />
                <asp:Image ID="qrImg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenStatusCheckBtn" runat="server" Text="" CssClass="hidden" ClientIDMode="Static" OnClick="hiddenStatusCheckBtn_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
