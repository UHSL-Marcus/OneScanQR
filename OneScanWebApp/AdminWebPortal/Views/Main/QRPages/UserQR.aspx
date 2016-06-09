<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserQR.aspx.cs" Inherits="AdminWebPortal.Views.Main.QRPages.UserQR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Views/UserQR.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div runat="server" id="RegisterQRDiv" visible="false" ClientIDMode="Static">
        <asp:Button ID="hiddenQRCompleteBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenQRCompleteBtn_Click" />
        <asp:Image ID="qrImg" runat="server" />
        <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenStatusCheckBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenStatusCheckBtn_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
