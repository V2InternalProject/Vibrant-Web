
<%@ Page Language="C#" AutoEventWireup="true" Inherits="Logon" Codebehind="Logon.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>


<body >
    <form id="form2" runat="server" >
        <table border="0" width="98%" cellpadding="0" cellspacing="0">
            <tr >
                <td colspan="3" valign="top" >
                   <%-- <table cellpadding="0" cellspacing="0" border="0" width="100%"  style="background-repeat:repeat-x">
                        <tr>
                            <td align="left" valign="top" style="width: 580px">
                                <img id="Header1_Image1" runat="server" src="Images/OrbitHeader.JPG" alt="" border="0" />
                            </td>
                            
                        </tr>
                    </table>--%>
                     <table cellpadding="0" cellspacing="0" border="0" width="100%"  background="/OrbitWeb/images/topbackgroundbg.jpg"  style="background-repeat:repeat-x" >
                            <tr>
                            <td align="left"  valign="top" style="width: 580px; height: 88px">
                                <asp:Image ID="imgOrbitHeader" runat="server"  ImageUrl="~/images/orbit_header_left.jpg"  AlternateText="" BorderWidth="0" />
                                </td>
                                 <td  valign="top" align="left" style="height: 88px"></td>
                               
                                <td valign="top" align="right" style="height: 88px" >
                                <asp:Image ID="imgV2Logo" runat="server"  ImageUrl="~/images/Orbit_headerright.jpg"  AlternateText="" BorderWidth="0" />
                                </td>
                              
                            </tr>
                        </table>
                </td>
            </tr> 
            <tr>
            <td Height="10px">            
            </td>
            </tr>          
            <tr height="450px" valign="top">
                <td  style="font-family: Verdana; font-weight:bold; font-size: 11px; 
                    color: #515151; font-style: normal;">
                    &nbsp;Login Failed! Please  
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="click here" OnClick="lnkHomePage_Click"></asp:LinkButton>
                    to login again.</td>
            </tr>
            
            <tr>
                <td  style="background-color: #f2f2f2; height: 5px;">
                </td>
            </tr>
            <tr>
                <td  class="copyright" align="center" style="background-color: #40a3cc;
                    height: 20px; font-family: Verdana; font-size: 9px; color: #FFFFFF; font-weight: bold;
                    text-decoration: none;">
                    © 2007 V2Solutions, Inc.
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
