<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" Inherits="_Default" CodeBehind="Default.aspx.cs" %>

<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>

<body>
    <form id="form2" runat="server">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td align="left" valign="top" style="width: 100%; height: 88px">
                    <img src="Images/Topbanner.jpg" style="background-repeat: repeat-x; width: 100%"
                        alt="" />
                </td>
            </tr>
        </table>
        <table border="0" width="98%" cellpadding="0" cellspacing="0">

            <tr>
                <td height="10px"></td>
            </tr>
            <tr height="450px" valign="top">
                <td style="font-family: Verdana; font-weight: bold; font-size: 11px; color: #515151; font-style: normal;">&nbsp;Login Failed! Please

                    <asp:LinkButton ID="LinkButton1" runat="server" Text="click here"
                        OnClick="LinkButton1_Click"></asp:LinkButton>
                    to login again.</td>
            </tr>
            <tr>
                <td style="background-color: #f2f2f2; height: 5px;"></td>
            </tr>
        </table>
    </form>
</body>
</html>