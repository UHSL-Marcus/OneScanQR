<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="argumentTest.aspx.cs" Inherits="AdminWebPortal.argumentTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container" runat="server">
        <asp:Label runat="server" ID="showArgument" />
        <asp:Button runat="server" ID="postbackBtn" OnClick="postbackBtn_Click" Text="Postback"/>
    </div>
    </form>
</body>
</html>
