<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoorControl.aspx.cs" Inherits="OneScanWebApp.DoorControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/DoorControl.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="getQrbtn" runat="server" OnClick="getQrbtn_Click" Text="Get QR" />
        <span id="scanStatusLbl"></span>
        Door Status:
        <span id="doorStatusLbl">Locked</span>
        <br /><asp:Image ID="qrImg" runat="server" />
    </form>
</body>
</html>
