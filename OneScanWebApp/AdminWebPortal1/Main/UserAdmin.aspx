<%@ Page Title="" Language="C#" MasterPageFile="~/Main/AdminWebPortal.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="AdminWebPortal.Main.UserAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
</asp:Content>
