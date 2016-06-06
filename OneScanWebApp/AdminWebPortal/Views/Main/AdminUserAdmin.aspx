<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminUserAdmin.aspx.cs" Inherits="AdminWebPortal.Views.Main.AdminUserAdmin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="/Scripts/Views/AdminUserAdmin.js"></script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div class="content-padding">
        <div class="panel panel-default">
            <asp:Table runat="server" ID="AdminUsersTbl" CssClass="table table-striped">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Token</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table> 
        </div>
        <asp:Button ID="registerNewAdminBtn" runat="server" Text="Register New Admin" OnClick="registerNewAdminBtn_Click"/>
        <div runat="server" id="RegisterQRDiv" visible="false" ClientIDMode="Static">
            <asp:Button ID="hiddenQRCompleteBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenQRCompleteBtn_Click" />
            <asp:Image ID="qrImg" runat="server" />
            <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
                <ContentTemplate>
                    <asp:Button ID="hiddenStatusCheckBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenStatusCheckBtn_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div> 
    </div>
</asp:Content>
