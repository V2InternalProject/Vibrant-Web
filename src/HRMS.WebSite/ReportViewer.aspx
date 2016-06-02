<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="HRMS.Views.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <div>
            <table width="100%" align="center">
                <tr>
                    <td>
                        <rsweb:ReportViewer ID="rptLeaveReport" runat="server" Width="100%" Font-Names="Verdana"
                            Font-Size="8pt" Height="400px" ShowRefreshButton="true" ShowPrintButton="false" ShowFindControls="true" ShowPageNavigationControls="true" PageCountMode="Actual">
                            <LocalReport>
                                <DataSources>
                                    <rsweb:ReportDataSource />
                                </DataSources>
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>