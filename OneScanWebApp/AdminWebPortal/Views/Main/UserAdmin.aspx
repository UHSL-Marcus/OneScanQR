<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="AdminWebPortal.Views.Main.UserAdmin" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div class="content-padding">
        <div class="panel panel-default">
            <table class="table table-striped">
                <thead>
                    <tr>
					    <th>User Name</th>
					    <th>UserToken</th>
                        <th></th>
				    </tr>
                </thead>
                <tbody>
			        <tr>
                        <td>User 1</td>
                        <td>Token1</td>
                        <td><a>Delete</a></td>
                    </tr>
			    </tbody>
            </table>
        </div>
    </div>
</asp:Content>
