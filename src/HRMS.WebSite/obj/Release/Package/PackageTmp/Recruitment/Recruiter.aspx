<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true" Inherits="Recruiter" CodeBehind="Recruiter.aspx.cs" %>

<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>--%>
<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Recruitment/pop_up_window.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var title = "";
        function SingleSelectCheckbox(current) {
            var flag = true;
            if (current.checked == false)
                flag = false;

            var gv = document.getElementById('<%= this.grdRecruiter.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[1].type == "checkbox") {
                        inputs[1].checked = false;

                    }
                }

            }
            if (flag == false)
            { current.checked = false; }
            else
            { current.checked = true; }
        }

        function validate(obj1, obj2) {
            var flag = true;
            var gv = document.getElementById('<%= this.grdRecruiter.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];
                // alert(rowElement.getElementById('hdID').value);
                var inputs = rowElement.getElementsByTagName("input");

                if (typeof (inputs[1]) != 'undefined') {
                    if (inputs[1].type == "checkbox") {
                        if (inputs[1].checked == true) {
                            flag = false;
                            var Status = inputs[0].value;
                        }
                    }
                }
            }
            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record.' + '</p>', title);
                return false;
            }
            else {
                if (obj1 == "1") {
                    if (Status != "Yet To Begin") {
                        if (Status == "Closed") {
                            V2hrmsAlert('<p>' + 'This RRF has already been closed.' + '</p>', title);

                            return false;
                        } else if (Status == "Cancelled") {
                            V2hrmsAlert('<p>' + 'This RRF has already been cancelled.' + '</p>', title);

                            return false;

                        } else {
                            V2hrmsAlert('<p>' + 'You have already accepted this RRF.' + '</p>', title);
                            return false;
                        }

                    } else {
                        var confirmation = confirm("Do you want to accept this RRF?")
                        if (confirmation) {

                          document.getElementById('<%=tblMain.ClientID %>').style.display = 'none';
                            DisplayLoadingDialog();
                            //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';

                            return true;
                        }
                        else
                            return false;
                        //                        return confirm("Do you want to accept this RRF?")
                    }
                } else if (obj1 == "2") {
                    if (Status == "Yet To Begin") {
                        V2hrmsAlert('<p>' + 'Kindly accept the RRF first.' + '</p>', title);
                        return false;
                    }
                    else if (Status == "Closed") {
                        if (obj2 == "1") {
                            V2hrmsAlert('<p>' + 'This RRF has already been closed.' + '</p>', title);
                            return false;
                        }
                    }
                    else if (Status == "Cancelled") {
                        if (obj2 == "1") {
                            V2hrmsAlert('<p>' + 'This RRF has already been cancelled.' + '</p>', title);
                            return false;
                        }
                    }
                }

            }
        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <%--    <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">
       <tr>
            <td align="center" class="tableHeadBlueLight">
                <asp:Label ID="lblWelcome" runat="server" Text="Recruiter" Visible="true"
                    SkinID="lblWelcome"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="false" SkinID="lblError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server">
                </asp:Image>
            </td>
        </tr>
    </table>--%>
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody RecH SmartT">
            <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="false" SkinID="lblError"></asp:Label>
            <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>
            <div id="tblMain" runat="server" class="rwrap clearfix">

                <div class="clearfix">
                    <h3 class="smartrackH floatL">
                        <asp:Label ID="lblWelcome" runat="server" Visible="true"
                            Text="Recruiter" SkinID="lblWelcome"></asp:Label></h3>
                    <div class="HRMrightsec clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <asp:Label ID="lblRRFCodeSearch" runat="server" Text="RRF Code:"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtRRFCodeSearch" runat="server" class="srchinput mrgnR3">
                           </asp:TextBox>
                            </div>
                        </div>
                        <div class="rightcol">
                            <asp:Button ID="btnSearch" runat="server" Text="Search RRF Code" CssClass="ButtonGray"
                                OnClick="btnSearch_Click" />
                            <%--<asp:Button ID="btnReset" runat="server" Text="Clear Filters" CssClass="ButtonGray"
                            OnClick="btnReset_Click" />--%>
                        </div>
                    </div>
                </div>

                <div class="ButtonContainer2 clearfix">
                    <asp:Panel ID="pnlAction" runat="server">
                        <asp:Button ID="btnAcceptRRF" runat="server" Text="Accept RRF" OnClientClick="return validate(1,0);"
                            OnClick="btnAcceptRRF_Click" CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnSearchCandidate" runat="server" Text="Search Candidate" OnClientClick="return validate(2,1);"
                            OnClick="btnSearchCandidate_Click" CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnScheduleInterview" runat="server" Text="Schedule Interview" OnClientClick="return validate(2,1);"
                            OnClick="btnScheduleInterview_Click" CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnViewRRF" runat="server" Text="View RRF" OnClientClick="return validate(2,0);"
                            OnClick="btnViewRRF_Click" CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnViewProgress" runat="server" Text="View Progress" OnClick="btnViewProgress_Click"
                            OnClientClick="return validate(2,0);" CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnViewSLA" runat="server" Text="View SLA" OnClientClick="javascript:return validate();"
                            OnClick="btnViewSLA_Click" CssClass="ButtonGray mrgnB5" />
                    </asp:Panel>
                </div>

                <div class="scrollHContainer">
                    <asp:GridView ID="grdRecruiter" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="3"
                        OnRowDataBound="grdRecruiter_RowDataBound" OnRowCommand="grdRecruiter_RowCommand" AllowPaging="True" OnPageIndexChanging="grdRecruiter_PageIndexChanging"
                        OnSorting="grdRecruiter_Sorting" AllowSorting="true" CssClass="grid TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField HeaderText="RRF Code" Visible="true" SortExpression="RRFNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblRRFcode" runat="server" Width="100" Text='<%# Eval("RRFNo") %>'
                                        Visible="true"></asp:Label>
                                    <asp:Label ID="lblRRFID" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requestor" Visible="true" SortExpression="RequestedBy">
                                <ItemTemplate>
                                    <asp:Label ID="lblrequestor" runat="server" Width="200" Text='<%# Bind("RequestedBy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requested Date" Visible="true" SortExpression="RequestDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblrequestdate" runat="server" Width="100" Text='<%# Eval("RequestDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expected Closure Date" Visible="true" SortExpression="ExpectedClosureDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblexpectedclosuredate" runat="server" Width="100" Text='<%# Eval("ExpectedClosureDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RRF Status" Visible="true" SortExpression="RRFStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblRRFstatus" runat="server" Text='<%# Bind("RRFStatus") %>' Width="100"></asp:Label>
                                    <asp:HiddenField ID="hdnRRFStatus" runat="server" Value='<%# Bind("RRFStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DU " Visible="true" SortExpression="Du">
                                <ItemTemplate>
                                    <asp:Label ID="lblDU" runat="server" Width="100" Text='<%# Eval("Du") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DT" Visible="true" SortExpression="Dt">
                                <ItemTemplate>
                                    <asp:Label ID="lblDt" runat="server" Width="100" Text='<%# Eval("Dt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Resource Pool Name" Visible="true" SortExpression="ResourcepoolName">
                                <ItemTemplate>
                                    <asp:Label ID="lblResourcepoolName" runat="server" Width="100" Text='<%# Eval("ResourcepoolName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" onClick="javascript:SingleSelectCheckbox(this);" />

                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>
</asp:Content>