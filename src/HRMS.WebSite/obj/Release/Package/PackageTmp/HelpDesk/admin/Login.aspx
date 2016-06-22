<%@ Page Language="c#" CodeBehind="Login.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.Login" %>

<%@ Register TagPrefix="uc1" TagName="Adminheader" Src="../controls/Adminheader.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script type="text/javascript" src="../Script/common.js"></script>
    <link type="text/css" rel="stylesheet" href="../css/allstyles.css">
    <script language="javascript">
        function LoginRequired() {
            if (isRequire("txtUserID^txtPassword", "User ID^Password", this.enabled)) {
                return isInteger("txtUserID", "User ID")
            }
            else {
                return false;
            }
        }

        function setfocus() {
            document.getElementById("txtUserID").focus();
        }
    </script>
</head>
<body ms_positioning="GridLayout" onload="setfocus();">
    <form id="Form1" method="post" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <uc1:Adminheader ID="Adminheader" runat="server"></uc1:Adminheader>
                </td>
            </tr>
            <tr>
                <td height="10"></td>
            </tr>
            <tr>
                <td height="10"></td>
            </tr>
            <tr>
                <td align="center">
                    <table cellspacing="0" cellpadding="2" width="30%" border="0">
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr class="header">
                            <td colspan="3" align="center">Login
                        </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" CssClass="error"></asp:Label>
                            </td>
                        </tr>
                        <tr class="txt">
                            <td width="48%" align="right">User ID
                        </td>
                            <td align="center" width="2%">:
                        </td>
                            <td width="50%">
                                <asp:TextBox ID="txtUserID" MaxLength="8" runat="server" CssClass="txtfield" Width="85px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="txt">
                            <td width="48%" align="right">Password
                        </td>
                            <td width="2%" align="center">:
                        </td>
                            <td width="50%">
                                <asp:TextBox ID="txtPassword" MaxLength="10" runat="server" TextMode="Password" CssClass="txtfield" Width="85px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="txt">
                            <td colspan="3">&nbsp;
                        </td>
                        </tr>
                        <tr class="txt">

                            <td colspan="3" align="center">
                                <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn" onclick="btnSubmit_Click"></asp:Button>--%>&nbsp;<asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr class="txt">
                            <td colspan="3">&nbsp;
                        </td>
                        </tr>
                        <tr class="error">
                            <td colspan="3" align="center">**Login Id and Password are same as PMS
                        </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>