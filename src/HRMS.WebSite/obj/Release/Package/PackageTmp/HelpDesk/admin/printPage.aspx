<%@ Page Language="c#" CodeBehind="printPage.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.printPage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Print Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../css/allstyles.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblSubHeading" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="dgPrint" runat="server" Width="80%"></asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>