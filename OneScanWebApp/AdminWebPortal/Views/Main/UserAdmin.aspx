<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableViewState="true" CodeBehind="UserAdmin.aspx.cs" Inherits="AdminWebPortal.Views.Main.UserAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <input type="hidden" runat="server" id="doorViewState" />
    <input type="hidden" runat="server" id="RegisterDoorCtlsViewState" />
    <div class="content-padding">
        <div class="panel panel-default">
            <asp:Table runat="server" ID="usersTbl" CssClass="table table-striped">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Username</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Token</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table> 
        </div>
        <asp:Button ID="registerNewUserBtn" runat="server" Text="Register New User" OnClick="registerNewUserBtn_Click"/> 
    </div>
</asp:Content>
