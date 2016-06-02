<%@ Page Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true"
    Inherits="Interviewer" Title="Interviewer Header" CodeBehind="Interviewer.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var title = '';

        function SingleSelectCheckbox(current) {
            var flag = true;
            if (current.checked == false)
                flag = false;

            var gv = document.getElementById('<%= this.grdInterviwer.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");

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

        function validate(obj) {

            var flag = true;
            var gv = document.getElementById('<%= this.grdInterviwer.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");

                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked == true) {
                            flag = false;
                            var CandidateID = inputs[1].value;
                            var RRFID = inputs[2].value;
                            var ScheduleID = inputs[3].value;
                            var StageID = inputs[4].value;
                            var RoundNumber = inputs[5].value;
                            var SrNo = inputs[6].value;
                            var conducted = inputs[7].value;
                        }
                    }
                }
            }
            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record.' + '</p>', title);
                return false;
            }
            else {
                if (obj == "1") {
                    if (conducted != "") {
                        V2hrmsAlert('<p>' + 'You have already conducted Interview for this candidate.' + '</p>', title);
                        return false;
                    }

                    else {
                        //                        var left = (screen.width / 2) - (w / 2);
                        //                        var top = (screen.height / 2) - (h / 2);

                        if (StageID != "6" && StageID != "17" && StageID != "18")
                        { window.open('InterviewFeedback.aspx', null, 'height=1000, width=1200,status= yes, resizable= no, scrollbars=yes,titlebar=yes,toolbar=no,location=center,menubar=no '); }

                        if (StageID == "6" || StageID == "18")
                        { window.open('HRInterviewAssessment.aspx', null, 'height=1000, width=1200,status= yes, resizable= no, scrollbars=yes,titlebar=yes,toolbar=no,location=no,menubar=no '); }
                    }
                }
                else if (obj == "3") {
                    if (conducted == "") {
                        V2hrmsAlert('<p>' + 'You have not filled feedback form for this candidate.' + '</p>', title);
                        return false;
                    }
                }

            }

        }

        function show_confirm() {
            alert("No Resume for this candidate was found.");
            // V2hrmsAlert('<p>' + 'No Resume for this candidate was found.' + '</p>', title);
            return false;
        }
    </script>
    <script type="text/javascript">
        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                validate(4);
                return false;
            }

        });
    </script>
    <section class="ConfirmationContainer Container">
        <%--<asp:Label ID="lblWelcome" runat="server" Text="Interviewer Form" SkinID="lblWelcome" />  --%>

        <%--<div class="FixedHeader">
						<div class="clearfix">
							<h2 class="MainHeading">Smart Track</h2>
							<div class="EmpSearch clearfix">
								<a href="#"></a>
								<input type="text" placeholder="Employee Search">
							</div>
						</div>
						<nav class="sub-menu-colored">
							<a href="MastersTable.aspx">MasterTable</a>
							<a href="Recruiter.aspx">Recruiter</a>
							<a href="Candidate.aspx">Candidate</a>
							<a href="RRFList.aspx">RRF List</a>
							<a href="HRM.aspx">HRM List</a>
							<a href="#" class="selected">Interviewer</a>
						</nav>
					</div>--%>
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody hrmF SmartT">
            <div class="ErrorMaster">
                <asp:Label ID="lblMsg" runat="server" SkinID="lblError" />
            </div>
            <div class="rwrap InterveiwL clearfix">
                <div class="clearfix">
                    <h3 class="smartrackH floatL">RRF List</h3>
                    <div class="HRMrightsec clearfix">
                        <asp:Label ID="Label1" runat="server" class="prefix" Text="Candidate First Name : "></asp:Label>
                        <asp:TextBox ID="txtFirstName" runat="server" class="mrgnR10"></asp:TextBox>
                        <asp:Label ID="lblLastName" runat="server" class="prefix" Text="Candidate Last Name : "></asp:Label>
                        <asp:TextBox ID="txtLastName" runat="server" class="mrgnR10"></asp:TextBox>
                        <div class="floatR mrgnR3">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" value="Search RRF No"
                                CssClass="ButtonGray" OnClick="btnSearch_Click" />
                            <%--  <asp:Button ID="btnReset" runat="server" Text="Clear Filters" value="Clear Filters"
                                            CssClass="ButtonGray" OnClick="btnReset_Click" />--%>
                        </div>
                    </div>
                </div>
                <div class="ButtonContainer2 clearfix">
                    <asp:Button ID="btnUpdateFeedback" runat="server" Text="Update Feedback"
                        CssClass="ButtonGray mrgnB5" OnClick="btnUpdateFeedback_Click" />
                    <asp:Button ID="btnViewRRF" runat="server" Text="View RRF" CssClass="ButtonGray mrgnB5"
                        OnClientClick=" javascript:return validate(2);" OnClick="btnViewRRF_Click" />
                    <asp:Button ID="btnViewFeedback" runat="server" Text="View FeedBack"
                        OnClientClick=" javascript:return validate(3);" CssClass="ButtonGray mrgnB5"
                        OnClick="btnViewFeedback_Click" />
                    <asp:Button ID="btnOpenResume" OnClientClick=" javascript:return validate(4);"
                        CssClass="ButtonGray mrgnB5" runat="server" Text="View Resume"
                        OnClick="btnOpenResume_Click"></asp:Button>
                </div>

                <div class="scrollHContainer">
                    <asp:GridView ID="grdInterviwer" runat="server" AutoGenerateColumns="false" Width="100%" AllowPaging="True"
                        OnPageIndexChanging="grdInterviwer_PageIndexChanging" AllowSorting="true" OnSorting="grdInterviwer_Sorting" CssClass="grid TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField HeaderText="CandidateID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCandidateID" runat="server" Text='<%# Bind("CandidateID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRRFID" runat="server" Text='<%# Bind("RRFID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ScheduleID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleID" runat="server" Text='<%# Bind("ScheduleID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StageID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStageID" runat="server" Text='<%# Bind("StageID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RoundNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRoundNO" Text='<%# Bind("RoundNumber") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SrNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSrNo" Text='<%# Bind("SrNo") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RRF Code" SortExpression="RRFNo">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRRFCode" Text='<%# Bind("RRFNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RRF ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRRFNo" Text='<%# Bind("RRFID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Candidate Name" SortExpression="CandidateName">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCandidateName" Text='<%# Bind("CandidateName") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage Name" SortExpression="StageName">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStage" Text='<%# Bind("StageName") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Position" SortExpression="Position">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPosition" Text='<%#Bind("Position") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DU" SortExpression="DU">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDU" Text='<%#Bind("DU") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DT" SortExpression="DT">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDT" Text='<%# Bind("DT") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interview Date" SortExpression="InterviewDate">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblInterviewDate" Text='<%# Eval("InterviewDate","{0:MM/dd/yyyy}") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interview Time" SortExpression="InterviewTime">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblInterviewTime" Text='<%# Bind("InterviewTime") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interview Status" SortExpression="CandidateStatus">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblstagestatus" Text='<%# Bind("CandidateStatus") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interview Conducted" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInterviewConducted" runat="server" Text='<%# Bind("Conducted") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                    <%-- <label for ="chkSelect" class="LabelForCheckbox"></label>--%>
                                    <asp:Label runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                                    <asp:HiddenField ID="HdCandidateID" runat="server" Value='<%# Bind("CandidateID") %>' />
                                    <asp:HiddenField ID="HdID" runat="server" Value='<%# Bind("RRFID") %>' />
                                    <asp:HiddenField ID="HdScheduleID" runat="server" Value='<%# Bind("ScheduleID") %>' />
                                    <asp:HiddenField ID="HDStageID" runat="server" Value='<%# Bind("StageID") %>' />
                                    <asp:HiddenField ID="HDRoundNo" runat="server" Value='<%# Bind("RoundNumber") %>' />
                                    <asp:HiddenField ID="HDSrNo" runat="server" Value='<%# Bind("SrNo") %>' />
                                    <asp:HiddenField ID="HDInterviewConducted" runat="server" Value='<%# Bind("Conducted") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stagename" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStagename" Text='<%# Bind("StageName") %>'>
                                                        </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>
</asp:Content>