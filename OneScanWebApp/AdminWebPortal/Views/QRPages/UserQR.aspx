<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ViewStateEncryptionMode="Always" ValidateRequest="false" CodeBehind="UserQR.aspx.cs" Inherits="AdminWebPortal.Views.Main.QRPages.UserQR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Views/UserQR.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <span ID="resultlabel"></span>
    <div>
        <asp:UpdatePanel ID="hiddenQRCompleteUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenQRCompleteBtn" runat="server" Text="" CssClass="hidden" ClientIDMode="Static" OnClick="hiddenQRCompleteBtn_Click" />
                <asp:Image ID="qrImg" Visible="false" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenStatusCheckBtn" runat="server" CssClass="hidden" Text="" ClientIDMode="Static" OnClick="hiddenStatusCheckBtn_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
