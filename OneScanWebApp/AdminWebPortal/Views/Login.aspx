<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AdminWebPortal.Views.Login.Login1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Views/Login.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <span ID="resultlabel"></span>

    <div runat="server" id="RegisterQRDiv" ClientIDMode="Static">
        <asp:UpdatePanel ID="hiddenQRCompleteUptPnl" runat="server">
            <ContentTemplate>
                <asp:Image ID="qrImg" runat="server" />
                <asp:Button ID="hiddenNewQRBtn" runat="server" Text="" OnClick="hiddenNewQRBtn_Click" /> 
            </ContentTemplate>
        </asp:UpdatePanel>
    
         <asp:UpdatePanel ID="hiddenPostBackUptPnl" runat="server">
            <ContentTemplate>
                <asp:Button ID="hiddenStatusCheckBtn" runat="server" Text="" CssClass="hidden" OnClick="hiddenStatusCheckBtn_Click" />
                <asp:Label ID="tempTest" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
