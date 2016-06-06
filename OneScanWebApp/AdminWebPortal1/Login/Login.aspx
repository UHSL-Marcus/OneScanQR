<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AdminWebpPortal.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="qrImg" runat="server" />
        <asp:Button ID="hiddenPostBackBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenPostBackBtn_Click" />
        <asp:Button ID="hiddenNewQRBtn" runat="server" Text="" Visible="true" ClientIDMode="Static" OnClick="hiddenNewQRBtn_Click" />
    </div>
    </form>
</body>
</html>
