<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="AdminWebPortal.Main.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="sidebar" class="left-side">
            <ul>

            </ul>
        </div>
        <asp:UpdatePanel ID="content" runat="server" class="right-side" ClientIDMode="Static">

        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
