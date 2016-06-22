<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true" Inherits="RRFList" CodeBehind="RRFList.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <%--   <script src="../JavaScript/jquery.selectbox-0.2.min.js"></script>--%>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var title = "";
        function SingleSelectCheckbox(current) {
            var flag = true;
            if (current.checked == false)
                flag = false;

            var gv = document.getElementById('<%= this.grdRRF.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                // alert(inputs.length);
                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        inputs[0].checked = false;
                    }
                }

            }
            if (flag == false)
            { current.checked = false; }
            else
            { current.checked = true; }
        }

        function validate() {
            var flag = true;
            var gv = document.getElementById('<%= this.grdRRF.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                if (typeof (inputs) != 'undefined') {
                    if (typeof (inputs[0]) != 'undefined') {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked == true)
                                flag = false;
                        }
                    }
                }
            }

            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record' + '</p>', title);
                return false;
            }
            else {
                document.getElementById('<%=tblMain.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }

        }
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" runat="server" />

    <section class="ConfirmationContainer Container">
        <div>
            <asp:Label ID="lblWelcome" runat="server" Text="" CssClass="smartrackH" SkinID="lblWelcome"></asp:Label>
        </div>

        <div>
            <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none; position: fixed; left: -1000px; top: -1000px;" runat="server"></asp:Image>
        </div>
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody hrmF SmartT">
            <div id="tblMain" runat="server">
                <asp:Label ID="lblMessage" runat="server" Visible="true" Text="Label" SkinID="lblError"></asp:Label>
                <asp:Label ID="lblSuccessMessage" runat="server" Visible="false" Text="Label" SkinID="lblSuccess"></asp:Label>
                <div class="rwrap clearfix">
                    <div class="clearfix">
                        <h3 class="smartrackH floatL">RRF List</h3>
                        <div class="HRMrightsec clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblRRFNoSearch" runat="server" Text="RRF No :  "></asp:Label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtRRFNOSearch" runat="server" CssClass="srchinput mrgnR10">
                                            </asp:TextBox>
                                </div>
                            </div>
                            <div class="rightcol">
                                <asp:Button ID="btnSearch" runat="server" Text="Search RRF No" CssClass="ButtonGray" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Clear Filters" CssClass="ButtonGray" OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="ButtonContainer2 clearfix">
                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click"
                            CssClass="ButtonGray mrgnB5" />
                        <asp:Button ID="btnViewRRF" runat="server" Text="View RRF" OnClientClick="javascript:return validate();"
                            CssClass="ButtonGray mrgnB5" OnClick="btnViewRRF_Click" />
                        <asp:Button ID="btnCandidateStatus" runat="server" Text="View Candidate Status" OnClientClick="javascript:return validate();"
                            CssClass="ButtonGray mrgnB5" OnClick="btnCandidateStatus_Click" />
                        <asp:Button ID="btnCancelRRF" runat="server" Text="Cancel RRF" OnClientClick="javascript:return validate();"
                            CssClass="ButtonGray mrgnB5" OnClick="btnCancelRRF_Click" />
                        <asp:Button ID="btnRejectRRF" runat="server" Text="Reject RRF" OnClientClick="javascript:return validate();"
                            CssClass="ButtonGray mrgnB5" OnClick="btnRejectRRF_Click" />
                        <asp:Button ID="btnCloseRRRF" runat="server" Text="Close RRF" OnClientClick="javascript:return validate();"
                            CssClass="ButtonGray mrgnB5" OnClick="btnCloseRRRF_Click" />
                        <asp:Button ID="btnViewSLA" runat="server" Text="View SLA" CssClass="ButtonGray mrgnB5" OnClientClick="javascript:return validate();"
                            OnClick="btnViewSLA_Click" />
                    </div>

                    <div class="scrollHContainer">
                        <asp:GridView ID="grdRRF" runat="server" ShowHeaderWhenEmpty="false" AutoGenerateColumns="false"
                            AllowSorting="true" OnRowCommand="grdRRF_RowCommand" AllowPaging="true"
                            OnPageIndexChanging="grdRRF_PageIndexChanging" OnSorting="grdRRF_Sorting" Width="100%" CssClass="grid TableJqgrid">

                            <HeaderStyle CssClass="tableHeaders" />
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                            <RowStyle CssClass="tableRows" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRFID" runat="server" Text='<%# Bind("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RRFNo" HeaderText="RRF No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRFNo" runat="server" Width="120" Text='<%# Bind("RRFNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DU" HeaderText="DU">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltDU" runat="server" Text='<%# Bind("DU") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DT" HeaderText="DT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDT" runat="server" Text='<%# Bind("DT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RequestedBy" HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedBy" runat="server" Text='<%# Bind("RequestedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ApprovedBy" HeaderText="Approved By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedBy" runat="server" Text='<%# Bind("ApprovedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ApprovalStatus" HeaderText="Approval Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Position" HeaderText="Position">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPosition" runat="server" Text='<%# Bind("Position") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RecruiterName" HeaderText="Recruiter Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecruiterName" runat="server" Text='<%# Bind("RecruiterName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RequestDate" HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Eval("RequestDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                        <%-- <asp:Label ID="lblRequestDate" runat="server"  Text='<%# Format(Container.DataItem("RequestDate"),"MM/dd/yy")%>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ExpectedClosureDate" HeaderText="Expected Closure Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpectedClosureDate" runat="server" Text='<%# Eval("ExpectedClosureDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ShortListedCandidate" HeaderText="ShortListed Candidate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShortListedCandidate" runat="server" Text='<%# Bind("ShortListedCandidate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RRFStatus" HeaderText="RRF Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRFStatus" runat="server" Text='<%# Bind("RRFStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>

                                        <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                        <asp:Label runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox" Width="22"></asp:Label>

                                        <%--<asp:CheckBox runat="server"  ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                                        <label for="chkSelect"  class="LabelForCheckbox" ></label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>