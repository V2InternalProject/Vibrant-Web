<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="AdminApproval.aspx.cs" Inherits="HRMS.Orbitweb.AdminApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> --%>
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $('.OrbitFilterLink').click(function () {

                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
            });
        });

        function Validation(txtFromDate, txtToDate, txtApproversComment) {
            var txtFromDate = txtFromDate;
            var txtToDate = txtToDate;
            var txtApproversComment = txtApproversComment;

            if (txtFromDate.value == "") {
                alert("Please select From Date");
                return false;
            }
            else if (txtToDate.value == "") {
                alert("Please select To Date");
                return false;
            }
            else if (Date.parse(txtFromDate.value) <= Date.parse(txtToDate.value)) {
                //alert("The dates are valid.");
            }
            else {
                if (txtFromDate.value == "" || txtToDate.value == "") {
                    alert("Both dates must be entered.");
                }
                else {
                    alert("From Date should not be greater than To Date");
                    return false;
                }
            }

            if (txtApproversComment.value.trim() == "") {
                alert("Please enter Approver Comments");
                return false;
            }
        }

        function ValidationForCompOff(txtApproversComment) {
            var txtApproversComment = txtApproversComment;
            if (txtApproversComment.value.trim() == "") {
                alert("Please enter Approver Comments");
                return false;
            }
        }
    </script>
    <section class="Container AttendanceContainer">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx" class="selected">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave Transaction</a> <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%>
                <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a><%--<a href="StartWorkflows.aspx">Manage Processes</a>--%>
                <a href="HolidayList.aspx">Masters</a>
            </nav>
        </div>
        <div class="MainBody Admin">
            <div>
                <asp:Label ID="lblSuccess" runat="Server" SkinID="lblSuccess"></asp:Label>
                <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
            </div>
            <div class="clearfix OrbitAuto AdminApproval">
                <div class="leftcol">
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <label>
                                Select Module Type:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlModuleType" runat="server" Width="130px" AutoPostBack="true" Enabled="false"
                                OnSelectedIndexChanged="ddlModuleType_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select --" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Sign-In Sign-Out" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Leave" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Compensatory" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Out Of Office" Value="3" Enabled="false"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlSearch" runat="server" Visible="false">
                    <div class="OrbitFilter">
                        <a href="#" class="OrbitFilterLink floatR">Filters</a>
                    </div>
                    <div class="OrbitFilterExpand">
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" class="ui-datepicker-trigger mrgnR12"
                            ImageUrl="~/images/New Design/calender-icon.png" CausesValidation="false" ImageAlign="Middle"></asp:ImageButton>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                            PopupButtonID="imgbtnSearchFromDate" TargetControlID="txtSearchFromDate">
                        </ajaxToolkit:CalendarExtender>
                        <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" Display="None"
                            SetFocusOnError="True" ValidationGroup="Search" ControlToValidate="txtSearchFromDate"
                            ErrorMessage="Select From Date "></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtSearchToDate" placeholder="To Date" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnSearchToDate" runat="server" ImageUrl="~/images/New Design/calender-icon.png"
                            CausesValidation="false" class="ui-datepicker-trigger mrgnR12" ImageAlign="Middle"></asp:ImageButton>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" PopupButtonID="imgbtnSearchToDate"
                            TargetControlID="txtSearchToDate">
                        </ajaxToolkit:CalendarExtender>
                        <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                            SetFocusOnError="True" ValidationGroup="Search" ErrorMessage="Select To Date "></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpTask" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                            ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                            SetFocusOnError="True" ValidationGroup="Search" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                        <asp:Button ID="btnSearch" runat="server" class="OrbitFilterImage"
                            CausesValidation="true" OnClick="btnSearch_Click" ValidationGroup="Search"></asp:Button>
                    </div>
                </asp:Panel>

                <div>
                    <div class="leftcol">
                        <div class="formrow clearfix">
                            <div class="LabelDiv">
                                <asp:Label ID="lblSelectUser" runat="server" Text="Select User :" Visible="False"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlEmployeeName" runat="server" Width="150px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                    Visible="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--   <div class="ButtonContainer2 clearfix">
                <input type="button" class="ButtonGray mrgnR8" value="Select All" />
                <input type="button" class="ButtonGray mrgnR8" value="Deselect All" />
                <input type="button" class="ButtonGray mrgnR8" value="Update" />
            </div>--%>
            <!--Gridview here-->
            <div class="InnerContainer scrollHContainer">
                <asp:Panel ID="pnlSISO" runat="server" Visible="false" Width="100%">
                    <asp:GridView ID="gvSISO" runat="server" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="gvSISO_PageIndexChanging"
                        OnRowEditing="gvSISO_RowEditing" OnRowCancelingEdit="gvSISO_RowCancelingEdit"
                        OnRowDataBound="gvSISO_RowDataBound" OnRowUpdating="gvSISO_RowUpdating" CssClass="TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                            LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                            PreviousPageText="Prev" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField HeaderText="SignInSignOutID" SortExpression="SignInSignOutID"
                                Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("SignInSignOutID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblID1" runat="server" Text='<%# Bind("SignInSignOutID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="SignInSignOutWFID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSignInSignOutWFID" Text='<%# Eval("WorkflowID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UserID" Visible="False" SortExpression="UserID">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditUserID" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEditUserID1" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Name" SortExpression="EmployeeName">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" SortExpression="date">
                                <EditItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%#Bind("date","{0: MM/dd/yyyy}") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate1" runat="server" Text='<%#Bind("date","{0: MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time" SortExpression="SignInTime">
                                <EditItemTemplate>
                                    &nbsp;<asp:DropDownList ID="ddlInTimeHours" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;

                                    <asp:DropDownList ID="ddlInTimeMins" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSignInTime" runat="server" Visible="false" Text='<%# Eval("SignInTime")  %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSignInTime1" runat="server" Text='<%# Eval("SignInTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time" SortExpression="SignOutTime">
                                <EditItemTemplate>
                                    &nbsp;<asp:DropDownList ID="ddlOutTimeHours" runat="server">
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;<asp:DropDownList ID="ddlOutTimeMins" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSignOutTime" runat="server" Text='<%# Eval("SignOutTime") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblOutDate" runat="server" Text='<%# Eval("OutDate") %>' Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSignOutTime1" runat="server" Text='<%# Eval("SignOutTime") %>'></asp:Label>
                                    <asp:Label ID="lblOutDate1" runat="server" Text='<%# Eval("OutDate") %>' Visible="False"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Hours" SortExpression="TotalHoursWorked">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalHours" runat="server" Text='<%# Bind("TotalHoursWorked") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mode" SortExpression="Mode">
                                <ItemTemplate>
                                    <asp:Label ID="lblMode" runat="server" Text='<%# Bind("Mode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sign In Comment" SortExpression="SignInComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblSignInComment" runat="server" Text='<%# Eval("SignInComment") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sign Out Comment" SortExpression="SignOutComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblSignOutComment" runat="server" Text='<%# Eval("SignOutComment") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Width="110px">
                                <EditItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                    &nbsp;<asp:DropDownList ID="ddlStatusdEdit" runat="server">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus1" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver's Name" SortExpression="Approver's Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproversName" runat="server" Text='<%# Eval("ApproverName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver's Comments" SortExpression="ApproverComments" ItemStyle-Width="180px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtApproversComments" runat="server" TextMode="MultiLine" Text='<%# Eval("ApproverComments") %>' Width="160px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvApproversComments" runat="server" ControlToValidate="txtApproversComments"
                                        Display="None" ErrorMessage="Please enter the comments" SetFocusOnError="True"
                                        ValidationGroup="Edit"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApproversComments" runat="server" Text='<%# Eval("ApproverComments") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Modified Date" SortExpression="LastModified" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblLastModified" runat="server" Text='<%# Eval("LastModified") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkBtnUpdate" runat="server" CausesValidation="True" ValidationGroup="Edit" Enabled="false"
                                        CommandName="Update" Text="Update"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Enabled="false"
                                        Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnEdit" runat="server" CausesValidation="False" CommandName="Edit" Enabled="false"
                                        Text="Edit"></asp:LinkButton>
                                    <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                    <%--  <label for="chkSelectAll" class="LabelForCheckbox"></label> --%><asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelectAll" class="LabelForCheckbox"></asp:Label>
                                    <asp:Button ID="btnupdateselect" CssClass="ButtonAsLink" runat="server" Text="Update" OnClick="btnupdateselect_click" />
                                </HeaderTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelect" runat="server" OnCheckedChanged="ChkSelect_CheckedChanged" />
                                    <%--<label for="ChkSelect" class="LabelForCheckbox"></label>--%>
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ChkSelect" class="LabelForCheckbox"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Edit"
                        ShowMessageBox="true" ShowSummary="false" />
                </asp:Panel>
            </div>
            <div class="InnerContainer scrollHContainer">
                <asp:Panel ID="pnlLeave" runat="server" Visible="false" Width="100%">
                    <asp:GridView ID="gvLeave" runat="server" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="gvLeave_PageIndexChanging"
                        OnRowDataBound="gvLeave_RowDataBound" OnRowCancelingEdit="gvLeave_RowCancelingEdit"
                        OnRowEditing="gvLeave_RowEditing" OnRowUpdating="gvLeave_RowUpdating" CssClass="TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                            LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                            PreviousPageText="Prev" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="LeaveDetailsID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLeaveDetailID" Text='<%# Eval("LeaveDetailID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblLeaveDetailID1" Text='<%# Eval("LeaveDetailID") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="LeaveDetailsWFID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLeaveDetailWFID" Text='<%# Eval("WorkflowID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblLeaveDetailWFID1" Text='<%# Eval("WorkflowID") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UserID" Visible="False" SortExpression="UserID">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditUserID" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEditUserID1" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Name" SortExpression="EmployeeName">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvEditUserName" Text='<%# Bind("EmployeeName") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Date" SortExpression="LeaveDateFrom">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvFromDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                                    <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Height="28px" Text='<%# Bind("LeaveDateFrom") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnFromDate"
                                        ImageAlign="Middle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                        TargetControlID="txtgrvFormDate" PopupButtonID="imgbtnFromDate" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvFromDate" runat="server" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date" SortExpression="LeaveDateTo">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvToDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                    <asp:TextBox ID="txtgrvToDate" Width="100px" runat="server" Height="28px" Text='<%# Bind("LeaveDateTo") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnToDate"
                                        ImageAlign="Middle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtgrvToDate"
                                        PopupButtonID="imgbtnToDate" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvToDate" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leaves">
                                <EditItemTemplate>
                                    <asp:Label ID="lblTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Absent ">
                                <EditItemTemplate>
                                    <asp:Label ID="lblTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvStatus" Visible="False" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                                    <asp:DropDownList ID="ddlgrvStatusName" runat="server" Width="100px">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvStatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver" Visible="False">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvApproverEdit" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver's Name" SortExpression="Approver's Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproversName" runat="server" Text='<%# Eval("ApproverName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver Comments" ItemStyle-Width="180px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'
                                        TextMode="MultiLine" Width="160px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                <ItemStyle HorizontalAlign="Center" Width="14%"></ItemStyle>
                                <ItemStyle Wrap="False" />
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update" Enabled="false"
                                        CausesValidation="True" ValidationGroup="Edit"></asp:LinkButton>
                                    &nbsp;

                                    <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="Cancel"  Enabled="false"
                                        CausesValidation="false"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApproved" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False" Enabled="false"
                                        CommandArgument='<%#  Eval("LeaveDetailID") %>'>Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAllLeave" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAllLeave_CheckedChanged" />
                                    <%-- <label for="chkSelectAllLeave" class="LabelForCheckbox"></label>--%><asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelectAllLeave" class="LabelForCheckbox"></asp:Label>
                                    <asp:Button ID="btnupdateselectLeave" CssClass="ButtonAsLink" runat="server" Text="Update" OnClick="btnupdateselectLeave_click" />
                                </HeaderTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelectLeave" runat="server" OnCheckedChanged="ChkSelectLeave_CheckedChanged" />
                                    <%-- <label for="ChkSelectLeave" class="LabelForCheckbox"></label>--%>
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ChkSelectLeave" class="LabelForCheckbox"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div class="InnerContainer scrollHContainer">
                <asp:Panel ID="pnlCompensdatory" runat="server" Visible="false" Width="100%">
                    <asp:GridView ID="gvCompensatory" runat="server" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="gvCompensatory_PageIndexChanging"
                        OnRowCancelingEdit="gvCompensatory_RowCancelingEdit" OnRowDataBound="gvCompensatory_RowDataBound"
                        OnRowEditing="gvCompensatory_RowEditing" OnRowUpdating="gvCompensatory_RowUpdating" CssClass="TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                            LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                            PreviousPageText="Prev" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="CompensationID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompensationID" Text='<%# Eval("CompensationID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblEditCompensationID" Text='<%# Eval("CompensationID") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="CompensationWFID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompensationWFID" Text='<%# Eval("WorkflowID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User" Visible="False" SortExpression="UserID">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditUserID" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEditUserIDTemp" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Name" SortExpression="EmployeeName">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvEditUserName" Text='<%# Bind("EmployeeName") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Applied For" SortExpression="AppliedFor">
                                <EditItemTemplate>
                                    <asp:Label ID="lblgrvAppliedFor" runat="server" Visible="false" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                                    <asp:TextBox ID="txtgrvAppliedFor" runat="server" Width="100px" Height="28px" Text='<%# Bind("AppliedFor") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnFromDate"
                                        ImageAlign="Middle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                        TargetControlID="txtgrvAppliedFor" PopupButtonID="imgbtnFromDate" />
                                    <asp:RequiredFieldValidator ID="rfvFromDate" ErrorMessage="Select From date" ControlToValidate="txtgrvAppliedFor"
                                        runat="server" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvAppliedFor" runat="server" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="130px" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditgrvReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                                <EditItemTemplate>
                                    <asp:Label ID="lblStatus" Visible="False" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                                    <asp:DropDownList ID="ddlgrvStatusName" runat="server" Width="100px">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvStatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver" Visible="False">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver's Name" SortExpression="Approver's Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproversName" runat="server" Text='<%# Eval("ApproverName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver Comments" ItemStyle-Width="180px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'
                                        TextMode="MultiLine" Width="160px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                <ItemStyle HorizontalAlign="Center" Width="16%"></ItemStyle>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"
                                        CausesValidation="false"></asp:LinkButton>
                                    &nbsp;

                                    <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                        CausesValidation="false"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                        CommandArgument='<%#  Eval("CompensationID") %>'>Edit</asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAllCompensatory" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAllCompensatory_CheckedChanged" />
                                    <%-- <label for="chkSelectAllCompensatory" class="LabelForCheckbox"></label>--%>
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelectAllCompensatory" class="LabelForCheckbox"></asp:Label>
                                    <asp:Button ID="btnupdateselectCompensatory" runat="server" CssClass="ButtonAsLink" Text="Update" OnClick="btnupdateselectCompensatory_click" />
                                </HeaderTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelectCompensatory" runat="server" OnCheckedChanged="ChkSelectCompensatory_CheckedChanged" />
                                    <%--<label for="ChkSelectCompensatory" class="LabelForCheckbox"></label>--%>
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ChkSelectCompensatory" class="LabelForCheckbox"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlOutOfOffice" runat="server" Visible="false" Width="100%">
                    <asp:GridView ID="gvOutOfOffice" runat="server" Width="100%" AutoGenerateColumns="false"
                        AllowPaging="true" PageSize="10" CssClass="TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                            LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                            PreviousPageText="Prev" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                    </asp:GridView>
                </asp:Panel>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                ShowMessageBox="true" ShowSummary="false" />
        </div>
    </section>
</asp:Content>