<%@ Page Title="Doors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DoorAdmin.aspx.cs" Inherits="AdminWebPortal.Views.Main.DoorAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div class="content-padding">
        <div class="panel panel-default">
            <asp:Table runat="server" ID="DoorsTbl" CssClass="table table-striped">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>ID</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Secret</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table> 
        </div>
        <asp:TextBox runat="server" ID="newDoorIdTxtBx" placeholder="Door ID" />
        <asp:TextBox runat="server" ID="newDoorSecretTxtBx" placeholder="Door Secret" />
        <asp:Button ID="addDoorBtn" runat="server" Text="Add New Door" OnClick="addDoorBtn_Click"/>
    </div>
</asp:Content>
