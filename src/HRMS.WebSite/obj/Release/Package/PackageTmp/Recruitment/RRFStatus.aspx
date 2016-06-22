<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true"
    Inherits="RRFStatus" CodeBehind="RRFStatus.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" type="text/css" />

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

            var gv = document.getElementById('<%= this.grdCandidateProgress.ClientID %>');
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
            var gv = document.getElementById('<%= this.grdCandidateProgress.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked == true)
                            flag = false;
                    }
                }
            }

            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record.' + '</p>', title);
                //  alert('Kindly select record');
                return false;
            }

        }
    </script>
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody reInterview SmartT">
            <div class="InnerContainer">
                <div class="clearfix">
                    <asp:Button ID="btnBack" OnClientClick="javascript:history.go(-1);return false;"
                        runat="server" Text="Back" CssClass="floatL BackLink" />
                </div>
            </div>
            <div class="InnerContainer clearfix">

                <h4 class="SmallHeadingBold">Candidate Status</h4>

                <div class="statuswrapR">
                    <label class="conduct mrgnR45">
                        Not Yet Conducted</label>
                    <label class="hold mrgnR45">
                        On Hold</label>
                    <label class="reject mrgnR45">
                        Rejected</label>
                    <label class="clear mrgnR45">
                        Cleared</label>
                    <label class="offer mrgnR45">
                        Offer Issued</label>
                </div>
                <div class="clearfix wrapLabel">
                    <div class="LabelDiv">
                        <%--<label>RFF No:</label>--%>
                        <asp:Label ID="Label1" runat="server" SkinID="lblSubheader" Text="RRF No :"></asp:Label>
                    </div>
                    <div class="InputDiv">
                        <%--<label>DI20130001</label>--%>
                        <asp:Label ID="lblRRFNO" runat="server" SkinID="lblSubheader"></asp:Label>
                    </div>
                </div>
                <asp:Label ID="lblMessage" Visible="false" runat="server" Text="No Record Found"
                    SkinID="lblError" CssClass="ErrorMsgOrbit"></asp:Label>
                <div class="FormContainerBox clearfix">

                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <%--<label>Select RFF to Reassign:</label>--%>
                                <asp:Label Visible="false" ID="lblReassign" Text="Select RRF to Reassign : " runat="server"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <%-- <select>
                                            <option>abc</option>
                                            <option>abc</option>
                                            <option>abc</option>
                                        </select>--%>
                                <asp:DropDownList ID="ddlRRFReassign" Visible="false" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="inlineBtns">
                            <%--<input type="button" class="ButtonGray mrgnR11" value="Reassign Candidate">
								<input type="button" class="ButtonGray" value="Schedule">--%>
                            <asp:Button Visible="false" ID="btnReassign" runat="server" Text="Reassign Candidate"
                                OnClientClick="javascript:return validate();" OnClick="btnReassign_Click" CssClass="ButtonGray mrgnR11" />
                            <asp:Button Visible="false" ID="btnSchedule" runat="server" Text="Schedule" OnClientClick="javascript:return validate();"
                                OnClick="btnSchedule_Click" CssClass="ButtonGray" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:GridView ID="grdCandidateProgress" runat="server" Width="96%" AutoGenerateColumns="False"
                OnRowDataBound="grdCandidateProgress_RowDataBound" AllowSorting="true" OnPageIndexChanging="grdCandidateProgress_PageIndexChanging"
                OnSorting="grdCandidateProgress_Sorting" CssClass="TableJqgrid" AllowPaging="true" PageSize="10">
                <HeaderStyle CssClass="tableHeaders" />
                <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                    LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                    PreviousPageText="Prev" />
                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                <RowStyle CssClass="tableRows" />
                <Columns>
                    <asp:TemplateField SortExpression="CandidateName" HeaderText="CandidateName" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCandidateName" runat="server" Text='<%# Eval("CandidateName") %>'
                                Visible="true"></asp:Label>
                            <asp:Label ID="lblCandidateID" runat="server" Text='<%# Eval("CandidateID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Progress" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblStage" Text='<%# Eval("CurrentStageStatus") %>' runat="server"
                                Visible="false"></asp:Label>
                            <asp:Table ID="Table1" runat="server">
                            </asp:Table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </section>
</asp:Content>