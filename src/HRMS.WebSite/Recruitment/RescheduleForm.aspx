<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.master" AutoEventWireup="true" Inherits="CandidateInterviewScheduleForm.RescheduleForm" CodeBehind="RescheduleForm.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 190px;
        }
        .style2
        {
            width: 157px;
        }
    </style>
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/New%20Design/demo.css" rel="stylesheet" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" />
    <script src="../Scripts/Recruitment/HRMS.js"></script>
    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <script language="javascript" type="text/javascript">        function validateDatetime() {
            var title = "";
            var message = '';
            var flag = 1;

            var CandidateScheduleDate = document.getElementById("MainContent_txtNewDate");
            var Hrs = document.getElementById("MainContent_ddlNewTimeHours");
            var Min = document.getElementById("MainContent_ddlNewTimeMinutes");
            var RescheduleBy = document.getElementById("MainContent_ddlRescheduledby");
            var NewDate = document.getElementById("MainContent_txtNewDate");
            var Reason = document.getElementById("MainContent_txtReason")
            if (RescheduleBy.value == "") {

                message = '<p>' + 'Please select rescheduled by ' + '</p>';
                RescheduleBy.focus();
                flag = 0;
            }
            if (NewDate.value == "") {

                message = message + '<p>' + 'Please select new date ' + '</p>';
                NewDate.focus();
                flag = 0;

            } else
                if (CandidateScheduleDate) {
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

                                message = message + '<p>' + 'Re-scheduled  date should be greater than equal to current date ' + '</p>';
                                flag = 0;
                            }

                        }
                        else {

                            message = message + '<p>' + 'Re-scheduled  date should be greater than equal to current date ' + '</p>';
                            flag = 0;
                        }
                    }
                    else {

                        message = message + '<p>' + 'Re-scheduled  date should be greater than equal to current date ' + '</p>';
                        flag = 0;
                    }
                }
                else {

                    message = message + '<p>' + 'Re-scheduled  date should be greater than equal to current date ' + '</p>';
                    flag = 0;
                }

            if (document.getElementById("MainContent_txtReason").value == "") {

                message = message + '<p>' + 'Enter reason for re-schedule interview ' + '</p>';
                Reason.focus();
                flag = 0;
            }
            else {
                //                document.getElementById('<%=tblCandidateReschedule.ClientID %>').style.display = 'none';
                //                document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';

            }

            if (flag == 1) {
                document.getElementById('<%=tblCandidateReschedule.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }
            else {

                V2hrmsAlert(message, title);
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('input[id$=MainContent_txtNewDate]').bind('cut copy paste', function (e) {
                e.preventDefault();
            });

        });
        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function CandidateRescheduleLoader() {
            document.getElementById('<%=tblCandidateReschedule.ClientID %>').style.display = 'inline';
            document.getElementById('<%=loadImage.ClientID %>').style.display = 'none';

        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <div id="page">
        <section class="ConfirmationContainer Container">
            <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
            <div class="MainBody hrmF RequestorADDNEW newC">
                <div class="rwrap clearfix">
                    <h3 class="smartrackH">Candidate Interview Re-Schedule Form </h3>

                    <div id="">
                        <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>
                    </div>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" SkinID="lblSuccess" Visible="false"></asp:Label>
                    </div>

                    <div id="tblCandidateReschedule" runat="server">
                        <div class="clearfix">
                            <div class="clearfix sec3C FormContainerBox">
                                <div class="formrow clearfix">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label1" runat="server" Text="RRF No."></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblRRFNO" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label2" runat="server" Text="Position"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblPosition" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="formrow clearfix">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label3" runat="server" Text="Candidate Name"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblCandidateName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label4" runat="server" Text="Stage #"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblStage" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="formrow clearfix">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label5" runat="server" Text="Interviewer"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblInterviewerold" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label6" runat="server" Text="New Interviewer"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtNewInterviewer" runat="server" ReadOnly="false" TabIndex="0" autocomplete="off" ImageAlign="AbsMiddle"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtNewInterviewer"
                                                MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                ServiceMethod="GetEmployeeName" OnClientShowing="aceResetPosition" />

                                            <asp:Label ID="lblInterviewer" runat="server" Visible="false" ForeColor="Red" Text="Please enter correct Name"
                                                SkinID="lblError"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="formrow clearfix">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label7" runat="server" Text="Scheduled Date"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblScheduledDate" runat="server" CssClass="ClassDisplayLabel" ToolTip="dd/MM/yyyy"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label13" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="Label8" runat="server" Text="*New Date"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtNewDate" runat="server" onkeypress="return false" TabIndex="1"></asp:TextBox>

                                                <asp:ImageButton ID="imgbtnNewDate" runat="server" CssClass="ui-datepicker-trigger" ImageUrl="../Images/New%20Design/calender-icon.png" ImageAlign="AbsMiddle" />

                                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                                    TargetControlID="txtNewDate" PopupButtonID="imgbtnNewDate" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="formrow clearfix Creschedule">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="Label9" runat="server" Text="ScheduledTime"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblScheduledTime" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="LabelDiv">
                                            <asp:Label ID="Label10" runat="server" Text="*New Time"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:DropDownList ID="ddlNewTimeHours" runat="server" OnLoad="ddlNewTimeHours_Load"
                                                OnSelectedIndexChanged="ddlNewTimeHours_SelectedIndexChanged" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:DropDownList ID="ddlNewTimeMinutes" runat="server" OnLoad="ddlNewTimeMinutes_Load"
                                                OnSelectedIndexChanged="ddlNewTimeMinutes_SelectedIndexChanged" AutoPostBack="false">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label14" runat="server" Text="" SkinID="lblError"></asp:Label>

                                            <asp:Label ID="lblNewTime" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="formrow clearfix">
                                    <div class="leftcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="Label11" runat="server" Text="*Rescheduled By"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:DropDownList ID="ddlRescheduledby" runat="server" TabIndex="2">
                                                <asp:ListItem Selected="True" Text="Select" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Candidate" Value="Candidate"></asp:ListItem>
                                                <asp:ListItem Text="HR" Value="HR"></asp:ListItem>
                                                <asp:ListItem Text="Panel" Value="Panel"></asp:ListItem>
                                                <asp:ListItem Text="IT" Value="IT"></asp:ListItem>
                                                <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblFirstNameMandatory" runat="server" Text="" SkinID="lblError"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="LabelDiv">
                                            <asp:Label ID="Label12" runat="server" Text="*Reason"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="3"
                                                MaxLength="200" TabIndex="3"></asp:TextBox>
                                            <asp:Label ID="Label15" runat="server" Text="" SkinID="lblError"></asp:Label>
                                            <div class="ClassTextareaDiv"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ButtonContainer1">

                                <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" OnClick="btnReschedule_Click"
                                    CssClass="ButtonGray" OnClientClick="javascript:return validateDatetime();" TabIndex="4" />

                                <asp:Button ID="btnBack" runat="server" Text="Cancel" TabIndex="5" OnClientClick="JavaScript:window.history.back(1); return false;" CssClass="ButtonGray"></asp:Button>

                                <%--OnClientClick="javascript:return validateDatetime();changeClass(0)" TabIndex="4" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>