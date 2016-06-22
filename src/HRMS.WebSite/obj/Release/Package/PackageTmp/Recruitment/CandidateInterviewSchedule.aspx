<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true"
    Inherits="CandidateInterviewSchedule" CodeBehind="CandidateInterviewSchedule.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <%--   <script type='text/javascript' src='http://jqueryjs.googlecode.com/files/jquery-1.3.2.min.js'>
    </script>--%>
    <%--    <script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <%-- <script src="../JavaScript/jquery.selectbox-0.2.min.js"></script>--%>
    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <script src="../Scripts/Recruitment/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../Scripts/Recruitment/pop_up_window.js" type="text/javascript"></script>
    <%--    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>--%>
    <%-- <script src="../Scripts/Recruitment/jquery-1.7.min.js"></script>--%>
    <script src="../Scripts/Recruitment/jquery-ui.min.js"></script>
    <script src="../Scripts/Recruitment/HRMS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
           //To apply plugin UI to dropdowns
            $('select').selectBox();
        }
        $(function () {
            $('input[id$=MainContent_grdCandidateSchedule_txtDate]').bind('cut copy paste', function (e) {
                e.preventDefault();
            });

        });

        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });
        });

        function CandidatescheduleLoader() {
            document.getElementById('<%=tblCandidateSchedule.ClientID %>').style.display = 'inline';
            document.getElementById('<%=loadImage.ClientID %>').style.display = 'none';

        }

        function HideImageloader() {

            document.getElementById('<%=tblCandidateSchedule.ClientID %>').style.display = 'inline';
            document.getElementById('<%=loadImage.ClientID %>').style.display = 'none';
            return false;
        }

        function show_confirm() {

            V2hrmsAlert('<p>' + 'You can not reschedule interview more than 5 times.' + '</p>', '');
            return false;
        }

        function CssApplication() {
            $("select").selectBox();
            $('*[id*=MainContent_grdCandidateSchedule_lnkbtnInsert]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });
        }

        function pageLoad() {
            CssApplication();
        }
        $(document).ready(function () {
            CssApplication();
        });

        function validateDatetime() {
            CssApplication();

            var title = "";
            var message = '';
            var flag = 1;
            var CandidateScheduleDate = document.getElementById("MainContent_grdCandidateSchedule_txtDate");
            var Hrs = document.getElementById("MainContent_grdCandidateSchedule_ddlTimeHours");
            var Min = document.getElementById("MainContent_grdCandidateSchedule_ddlTimeMinutes");
            var e = document.getElementById("MainContent_grdCandidateSchedule_ddlStage");
            var ddlStage = e.options[e.selectedIndex].text;

            if (ddlStage != "Final Stage") {
                if (document.getElementById("MainContent_grdCandidateSchedule_ddlStage").value == "Select") {
                    message = '<p>' + 'Please select stage name ' + '</p>';
                    //document.getElementById("MainContent_grdCandidateSchedule_ddlStage").focus();

                    flag = 0;
                }
                if (document.getElementById("MainContent_grdCandidateSchedule_txtDate").value == "") {
                    message = message + '<p>' + 'Please select date' + '</p>';

                    flag = 0;
                }

                else if (CandidateScheduleDate) {
                    var expireOnDate = CandidateScheduleDate.value;
                    var pos1 = expireOnDate.indexOf("/");
                    var pos2 = expireOnDate.indexOf("/", pos1 + 1);
                    var strMonth = eval(expireOnDate.substring(0, pos1) - 1);
                    var strDay = expireOnDate.substring(pos1 + 1, pos2);
                    var strYear = expireOnDate.substring(pos2 + 1);
                    var strDate = new Date();

                    strDate.setDate(strDay);
                    strDate.setMonth(strMonth);
                    strDate.setFullYear(strYear);
                    strDate.setHours(Hrs.value);
                    strDate.setMinutes(Min.value);
                    var today = new Date();
                    if (CandidateScheduleDate.value != "") {
                        if (CandidateScheduleDate) {

                            if (strDate < today) {

                                message = message + '<p>' + 'Date and Time should be greater than equal to current date' + '</p>';
                                flag = 0;
                            }
                        }
                        else {

                            message = message + '<p>' + 'Date and Time should be greater than equal to current date' + '</p>';
                            flag = 0;
                        }
                    }
                    else {

                        message = message + '<p>' + 'Date and Time should be greater than equal to current date' + '</p>';
                        flag = 0;
                    }
                }
                else {
                    message = message + '<p>' + 'Date and Time should be greater than equal to current date' + '</p>';
                    flag = 0;
                }

                if (document.getElementById("MainContent_grdCandidateSchedule_txtInterviewerName").value == "") {
                    //document.getElementById("MainContent_grdCandidateSchedule_txtInterviewerName").focus();

                    message = message + '<p>' + 'Please select interviewer name ' + '</p>';

                    flag = 0;
                }

            }
            else {

                if (document.getElementById("MainContent_grdCandidateSchedule_ddlStage").value == "Select") {
                    document.getElementById("MainContent_grdCandidateSchedule_ddlStage").focus();
                    message = '<p>' + 'Please select stage name ' + '</p>';
                    flag = 0;
                }
                if (document.getElementById("MainContent_grdCandidateSchedule_txtInterviewerName").value == "") {
                    document.getElementById("MainContent_grdCandidateSchedule_txtInterviewerName").focus();
                    message = message + '<p>' + 'Please select interviewer name' + '</p>';
                    flag = 0;
                }

            }

            if (flag == 1) {

                document.getElementById('<%=tblCandidateSchedule.ClientID %>').style.display = 'none';
                //$("#loading").dialog({
                //    closeOnEscape: false,
                //    resizable: false,
                //    height: 140,
                //    width: 300,
                //    modal: true,
                //    dialogClass: "noclose",
                //    open: function () {
                //        $('#loading').parent().css('background-color', 'transparent');
                //        $(this).parent().next('.ui-widget-overlay').css('z-index', '32');
                //        $(this).parent().css('z-index', '33');
                //    }
                //});
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';

            }
            else {

                V2hrmsAlert(message, title);
                return false;
            }

        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <%-- <body class="AttendancePage">--%>
    <div id="page">
        <section class="ConfirmationContainer Container">
            <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
            <%-- <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">--%>
            <div class="MainBody RecH feedBack">
                <div class="rwrap">
                    <div class="clearfix">
                        <span id="guide">
                            <asp:Button ID="btnback" runat="server" Text="Back" CssClass="ButtonGray BackLink" OnClick="btnback_Click"
                                Width="100px" />
                        </span>
                    </div>
                    <h3 class="smartrackH">Candidate Interview Schedule Form
                    </h3>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" SkinID="lblSuccess" Visible="false"></asp:Label>
                        <asp:Label ID="lblErrorMsg" runat="server" SkinID="lblError" Visible="false"></asp:Label>
                    </div>
                    <div>
                        <asp:Image ID="loadImage" ImageUrl="../Images/New%20Design/loader.GIF" Style="display: none" runat="server"></asp:Image>
                    </div>
                    <%--     <tr>
            <td class="lineDotted" colspan="4">
            </td>
        </tr>
        <tr>
            <td id="tblCandidateSchedule" runat="server" colspan="6">
                <table class="tableBorder" width="100%" align="center" border="0" cellpadding="10"
                    cellspacing="10">--%>
                    <div id="tblCandidateSchedule" runat="server">
                        <%--    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">--%>
                        <div class="clearfix mrgnT20">
                            <div class="floatL">
                                <asp:Label ID="Label1" runat="server" CssClass=" prefix" Text="RRF No :"></asp:Label>
                                <asp:Label ID="lblRRFNo" runat="server" CssClass="suffix"></asp:Label>
                            </div>

                            <div class="floatL">
                                <asp:Label ID="Label2" runat="server" CssClass=" prefix" Text="Position :"></asp:Label>
                                <asp:Label ID="lblPosition" runat="server" CssClass="suffix"></asp:Label>
                            </div>
                            <div class="floatL">
                                <asp:Label ID="Label3" runat="server" CssClass="prefix" Text="Posted Date :"></asp:Label>
                                <asp:Label ID="lblPostedDate" runat="server" CssClass="suffix"></asp:Label>
                            </div>
                            <div class="floatL">
                                <asp:Label ID="Label4" runat="server" CssClass="prefix" Text="Requestor :"></asp:Label>
                                <asp:Label ID="lblRequestor" runat="server" CssClass="suffix"></asp:Label>
                            </div>
                            <div class="floatL">
                                <asp:Label ID="Label5" runat="server" CssClass="prefix" Text="Candidate Name :"></asp:Label>
                                <asp:Label ID="lblCandidateName" runat="server" CssClass="suffix"></asp:Label>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="updatePanel" runat="server">

                            <ContentTemplate>
                                <asp:GridView ID="grdCandidateSchedule" runat="server" AutoGenerateColumns="False"
                                    Width="100%" CellSpacing="0" CellPadding="0" ShowHeaderWhenEmpty="True" ShowFooter="true"
                                    OnRowCommand="grdCandidateSchedule_RowCommand" OnPageIndexChanging="grdCandidateSchedule_PageIndexChanging"
                                    AllowPaging="true" AllowSorting="true" PageSize="20" OnRowDataBound="grdCandidateSchedule_RowDataBound"
                                    CssClass="grid TableJqgrid">
                                    <HeaderStyle CssClass="tableHeaders" />
                                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                    <RowStyle CssClass="tableRows" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Stage Name">
                                            <ItemTemplate>
                                                &nbsp;

                                                <asp:Label ID="lblStageName" runat="server" Text='<%# Bind("StageName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                &nbsp;

                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlStage" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged"
                                                        onblur="javascript:validateDatetime();" CssClass="margin_left_interview">
                                                    </asp:DropDownList>
                                                </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stage ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStageID" runat="server" Text='<%# Bind("Stage") %>'></asp:Label>
                                                <asp:Label ID="lblRoundId" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesignationName" runat="server" Text='<%# Bind("DesignationName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Candidate ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCandidateID" runat="server" Text='<%# Bind("CandidateID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Schedule ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblScheduleID" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RRF ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRRFID" runat="server" Text='<%# Bind("RRFID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RRF No" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRRFNumber" runat="server" Text='<%# Bind("RRFNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Round Number" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblroundNumber" runat="server" Text='<%# Bind("roundNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Date">
                                            <HeaderStyle Width="180px" />
                                            <ItemTemplate>
                                                &nbsp;

                                                <asp:Label ID="lblScheduleDate" runat="server" Text='<%# Bind("ScheduledDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                &nbsp;

                                                <asp:TextBox ID="txtDate" runat="server" onkeypress="return false" CssClass="interview_schedule margin_left_interview" ImageAlign="AbsMiddle" Height="28px" Width="100px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnDate" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" CssClass="ui-datepicker-trigger" />
                                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                                    TargetControlID="txtDate" PopupButtonID="imgbtnDate" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time">
                                            <HeaderStyle Width="222px" />
                                            <ItemTemplate>
                                                &nbsp;

                                                <asp:Label ID="lblTime" runat="server" Text='<%# Bind("Time") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                &nbsp;

                                                <asp:DropDownList ID="ddlTimeHours" Width="50" runat="server" OnLoad="ddlTimeHours_Load"
                                                    OnSelectedIndexChanged="ddlTimeHours_SelectedIndexChanged" AutoPostBack="false"
                                                    CssClass="margin_left_interview">
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;&nbsp;

                                                <asp:DropDownList ID="ddlTimeMinutes" Width="50" runat="server" OnLoad="ddlTimeMinutes_Load"
                                                    OnSelectedIndexChanged="ddlTimeMinutes_SelectedIndexChanged" AutoPostBack="false"
                                                    CssClass="margin_left_interview">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblTime" runat="server" Visible="false"></asp:Label>
                                            </FooterTemplate>

                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Interviewer">
                                            <ItemTemplate>
                                                &nbsp;

                                                <asp:Label ID="lblInterviewer" runat="server" Text='<%# Bind("Interviewer") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                                <asp:TextBox ID="txtInterviewerName" runat="server" oncopy="return false" oncut="return false"
                                                    onpaste="return false" CssClass="margin_left_interview" autocomplete="off">
                                                </asp:TextBox>
                                                <asp:Label ID="lblInterviewerName" runat="server" Text="Enter correct interviewer name"
                                                    Visible="false" SkinID="lblError"></asp:Label>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtInterviewerName"
                                                    MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetEmployeeName" OnClientShowing="aceResetPosition" UseContextKey="true" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                &nbsp;

                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%# Bind("EntryDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <FooterTemplate>
                                                &nbsp;

                                                <asp:LinkButton ID="lnkbtnInsert" runat="server" Text="Insert" CommandName="Insert"
                                                    OnClientClick="javascript:return validateDatetime();">Add Stage</asp:LinkButton>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkReschedule" runat="server" Text="Reschedule" CommandName="Reschedule"
                                                    CommandArgument='<%#((GridViewRow)Container).RowIndex %>'>Reschedule</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="lnkViewStageDetails" runat="server" Text="ViewFeedBack" CommandName="ViewFeedBack"
                                                    CommandArgument='<%#((GridViewRow)Container).RowIndex %>'>View FeedBack</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdCandidateSchedule" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <%--  </body>--%>
</asp:Content>