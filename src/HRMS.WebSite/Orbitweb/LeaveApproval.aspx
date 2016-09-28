<%@ Page Title="LeaveApproval" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="LeaveApproval.aspx.cs" Inherits="HRMS.Orbitweb.LeaveApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <!-- SelectBOx -->
    <script src="../JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidationFor() {
            var txtFromdate = document.getElementById("ctl00_ContentPlaceHolder1_txtSearchFromDate");

            if (txtFromdate.value == "") {
                alert("Please select From Date");
                return false;
            }
            var txtTodate = document.getElementById("ctl00_ContentPlaceHolder1_txtSearchToDate");

            if (txtTodate.value == "") {
                alert("Please select To Date");
                return false;
            }
        }

        function Validation(txtFromDate, txtToDate, txtApproversComment) {
            var txtFromDate = txtFromDate;
            var txtToDate = txtToDate;
            var txtApproversComment = txtApproversComment;

            if (txtFromDate.value == "") {
                alert("Please select From date");
                return false;
            }
            else if (txtToDate.value == "") {
                alert("Please select To date");
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
                    alert("From date should not be greater than To date");
                    return false;
                }
            }
            if (txtApproversComment.value.trim() == "") {
                alert("Please Enter Approver Comments");
                return false;
            }

        }

        $(document).ready(function () {
            $('.OrbitFilterLink').click(function () {
                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
            });

            if ($("#<%=gvLeaveApprovals.ClientID %> tr").length > 0 && $('#MainContent_ddlLeaveStatus').val() != "0") {
                $('.OrbitFilterExpand').show();
            }

        });

        $(function () {
            $('nav#menu').menu({
                slidingSubmenus: false
            });
        });

        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });
        });
    </script>
    <%-- <asp:ScriptManager runat="server" ID="ScriptManager2">
    </asp:ScriptManager>--%>
      <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <asp:HiddenField ID="selected_tab" runat="server" />
    <asp:HiddenField ID="mainTab_Selected" runat="server" />
    <asp:HiddenField ID="GridEditModel" runat="server" />
    <div class="AttendancePage">
        <div id="page">
            <%--				<section class="clearfix">
					<header id="header">
						<div class="SideMenuConBorderR">
							<a href="#menu" id="SlideMenuBtn"></a>
						</div>
						<h1>Vibrant Web</h1>
						<div class="UserLogout">
							<div class="ImgConBorderL">
								<img src="images/logout.png" alt="logout" />
							</div>
							<div class="ImgConBorderL">
								<img src="images/user.png" alt="user" />
							</div>
							<p class="floatR mrgnR15">Namrata</p>
						</div>
					</header>

					<nav id="menu" class="slide-menu">
						<ul>
							<li class="Selected head mm-subopen"><img src="images/orbit-icon.png" class="menu-logo" alt="vibrantweb"><a href="#">MY VIBRANT WEB</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="myvb">Attendance</a></li>
									<li class="align"><a href="#" class="myvb">Leave Management</a></li>
									<li class="align"><a href="#" class="myvb">Out of Office</a></li>
									<li class="align"><a href="#" class="myvb">Approvals</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/HR-icon.png" class="menu-logo" alt="processes"><a href="#">HR PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="hr">Confirmation</a></li>
									<li class="align"><a href="#" class="hr">Appraisal</a></li>
									<li class="align"><a href="#" class="hr">Separation</a></li>
									<li class="align"><a href="#" class="hr">Smart Track</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/logout-icon.png" class="menu-logo" alt="reports"><a href="#">REPORTS</a>
							</li>
							<li class="head"><img src="images/finance-icon.png" class="menu-logo" alt"finance"><a href="#">FINANCE PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="ExpenseIndex.html" class="finance">Expense Reimbursement</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/admin-icon.png" class="menu-logo" alt="admin"><a href="#">ADMIN PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="admin">Travel</a></li>
									<li class="align"><a href="#" class="admin">Helpdesk</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/settings-icon.png" class="menu-logo" alt="settings"><a href="#">SETTINGS</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="settings">Confirmation</a></li>
									<li class="align"><a href="#" class="settings">Appraisal</a></li>
									<li class="align"><a href="#" class="settings">Separation</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/logout-icon.png" class="menu-logo" alt="logout"><a href="#">LOG OUT</a></li>
						<!-- 	</ul> -->
					</nav>
				</section>--%>
            <section class="AttendanceContainer Container">
                <div class="FixedHeader">
                    <div class="clearfix">
                        <h2 class="MainHeading">
                            Approvals</h2>
                        <%--							<div class="EmpSearch clearfix">
								<a href="#"></a>
								<input type="text" placeholder="Employee Search">
							</div>--%>
                    </div>
                    <nav class="sub-menu-colored">
                        <a href="SignInSignOutApproval.aspx">SignIn SignOut</a> <a href="LeaveApproval.aspx" class="selected">
                            Leave</a> <a href="CompensationApproval.aspx">Compensatory Leave</a>
                    </nav>
                </div>
                <div class="MainBody">
                    <div align="center" class="SuccessMsgOrbit">
                        <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                    </div>
                    <div align="center" class="ErrorMsgOrbit">
                        <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
                    </div>
                    <div class="OrbitAuto">
                        <div class="clearfix">
                            <div class="OrbitFilter">
                                <a href="#" class="OrbitFilterLink floatR">Filters</a>
                            </div>
                            <div class="OrbitFilterExpand" style="display: none;">
                                <asp:DropDownList Width="150px" ID="ddlLeaveStatus" runat="server" OnSelectedIndexChanged="ddlLeaveStatus_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchFromDate" ReadOnly="false" runat="server"
                                    placeholder="From Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="imgbtnSearchFromDate" ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                    TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                                <asp:RequiredFieldValidator ID="rfvSearchFromDate" ErrorMessage="Select From Date "
                                    ControlToValidate="txtSearchFromDate" runat="server" Display="None"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtSearchToDate" ReadOnly="false" runat="server" placeholder="To Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="imgbtnSearchToDate" ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                    PopupButtonID="imgbtnSearchToDate" />
                                <asp:RequiredFieldValidator ID="rfvsearchToDate" ErrorMessage="Select To Date " ControlToValidate="txtSearchToDate"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cmpTask" runat="server" ErrorMessage="From date should not be greater than To date"
                                    ControlToValidate="txtSearchToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                    ControlToCompare="txtSearchFromDate"></asp:CompareValidator>
                                <asp:Button type="Button2" class="OrbitFilterImage" ID="btnSearch" runat="server"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="InnerContainer scrollHContainer">
                        <asp:GridView Width="100%" ID="gvLeaveApprovals" runat="server" AutoGenerateColumns="False"
                            AllowPaging="True" OnRowCommand="gvLeaveApprovals_RowCommand" PageSize="10" OnRowCancelingEdit="gvLeaveApprovals_RowCancelingEdit"
                            OnRowEditing="gvLeaveApprovals_RowEditing" OnRowUpdating="gvLeaveApprovals_RowUpdating"
                            OnRowDataBound="gvLeaveApprovals_RowDataBound" OnPageIndexChanging="gvLeaveApprovals_PageIndexChanging"
                            AllowSorting="True" OnSorting="gvLeaveApprovals_Sorting" CssClass="TableJqgrid">
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
                                <asp:TemplateField HeaderText="User" Visible="False" SortExpression="UserID">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditUserID" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User" SortExpression="EmployeeName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblgrvEditUserName" Text='<%# Bind("EmployeeName") %>' runat="server"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrvUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Date" SortExpression="LeaveDateFrom">
                                    <EditItemTemplate>
                                    <span>
                                        <asp:Label ID="lblgrvFromDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                                        <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Height="28px" Text='<%# Bind("LeaveDateFrom") %>' onpaste="return false" autocomplete="off"></asp:TextBox><asp:ImageButton
                                            ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnFromDate"
                                            ImageAlign="Middle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                            TargetControlID="txtgrvFormDate" PopupButtonID="imgbtnFromDate" />
                                            </span>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrvFromDate" runat="server" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date" SortExpression="LeaveDateTo">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblgrvToDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                        <asp:TextBox ID="txtgrvToDate" Width="100px" Height="28px" runat="server" Text='<%# Bind("LeaveDateTo") %>' onpaste="return false" autocomplete="off"></asp:TextBox><asp:ImageButton
                                            ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnToDate"
                                            ImageAlign="Middle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtgrvToDate"
                                            PopupButtonID="imgbtnToDate" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrvToDate" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                    </ItemTemplate>
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
                                        <asp:TextBox ID="txtgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approver Comments">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'
                                            TextMode="MultiLine"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                    <ItemStyle HorizontalAlign="Center" Width="14%"></ItemStyle>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update" 
                                            CausesValidation="false" Enabled="false"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="lbnCancel"
                                            CausesValidation="false" Enabled="false"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproved" runat="server"></asp:Label>
                                        <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                            CommandArgument='<%#  Eval("LeaveDetailID") %>' Enabled="false">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False"></asp:ValidationSummary>
            </section>
            <%--<footer>&#169; 2008 V2Solutions, Inc.</footer>--%>
        </div>
        <script type="text/javascript" language="javascript" src="footer.js"></script>
    </div>
    <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">
        <<%--tr>
            <td align="center" class="tableHeadBlueLight">
                Leave Approval
            </td>
        </tr>--%>
        <%--<tr>
            <td class="lineDotted">
            </td>
        </tr>--%>
        <%-- <tr>
            <td class="h10">
            </td>
        </tr>--%>
        <%--<tr>
            <td align="center">--%>
        <%-- <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>--%>
        <%-- </td>
        </tr>--%>
        <%-- <tr>
            <td align="center">--%>
        <%-- <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>--%>
        <%-- </td>
        </tr>
        <tr>
            <td class="h10">
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" class="tableBorder">
                    <tr>
                        <td colspan="8" class="h10">
                        </td>
                    </tr>
                    <tr>
                        <td width="5%">
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="lblType" runat="server" Text="Select Type :"></asp:Label>
                        </td>
                        <td align="left" width="20%">--%>
        <%--                            <asp:DropDownList Width="150px" ID="ddlLeaveStatus" runat="server" OnSelectedIndexChanged="ddlLeaveStatus_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>--%>
        <%-- </td>
                        <td align="right" width="10%">
                            <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                        </td>
                        <td align="left" width="15%">--%>
        <%--                            <asp:TextBox ID="txtSearchFromDate" Width="100px" ReadOnly="false" runat="server"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchFromDate"
                                ImageAlign="AbsMiddle" CausesValidation="false" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                            <asp:RequiredFieldValidator ID="rfvSearchFromDate" ErrorMessage="Select From Date "
                                ControlToValidate="txtSearchFromDate" runat="server" Display="None"></asp:RequiredFieldValidator>--%>
        <%-- </td>
                        <td align="right" width="10%">
                            <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                        </td>
                        <td align="left" width="15%">--%>
        <%--<asp:TextBox ID="txtSearchToDate" Width="100px" ReadOnly="false" runat="server"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchToDate"
                                ImageAlign="AbsMiddle" CausesValidation="false" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                PopupButtonID="imgbtnSearchToDate" />
                            <asp:RequiredFieldValidator ID="rfvsearchToDate" ErrorMessage="Select To Date " ControlToValidate="txtSearchToDate"
                                runat="server" Display="None"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmpTask" runat="server" ErrorMessage="From date should not be greater than To date"
                                ControlToValidate="txtSearchToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                ControlToCompare="txtSearchFromDate"></asp:CompareValidator>--%>
        <%--  </td>--%>
        <%--<td align="center" width="10%">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                CausesValidation="true" />&nbsp;--%>
        <%--<asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click"
                                CausesValidation="false" /></td>--%>
        <%--  </tr>
                    <tr>
                        <td colspan="8" class="h10">
                        </td>
                    </tr>--%>
        <%-- </table>--%>
        <%-- </td>
        </tr>
        <tr>
            <td class="h10">
            </td>
        </tr>
        <tr>
            <td>--%>
        <%-- <table width="100%">
                    <tr>
                        <td>--%>
        <%--   <asp:GridView Width="100%" ID="gvLeaveApprovals" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowCommand="gvLeaveApprovals_RowCommand"
                                PageSize="10" OnRowCancelingEdit="gvLeaveApprovals_RowCancelingEdit"
                                OnRowEditing="gvLeaveApprovals_RowEditing" OnRowUpdating="gvLeaveApprovals_RowUpdating"
                                OnRowDataBound="gvLeaveApprovals_RowDataBound" OnPageIndexChanging="gvLeaveApprovals_PageIndexChanging"
                                AllowSorting="True" OnSorting="gvLeaveApprovals_Sorting" CssClass="grid">
                                <PagerStyle HorizontalAlign="Right" />
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
                                    <asp:TemplateField HeaderText="User" Visible="False" SortExpression="UserID">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditUserID" Text='<%# Bind("UserID") %>' runat="server"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User" SortExpression="EmployeeName">
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
                                            <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Text='<%# Bind("LeaveDateFrom") %>'></asp:TextBox><asp:ImageButton
                                                ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnFromDate"
                                                ImageAlign="Middle" CausesValidation="false" />
                                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                                TargetControlID="txtgrvFormDate" PopupButtonID="imgbtnFromDate" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrvFromDate" runat="server" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Date" SortExpression="LeaveDateTo">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblgrvToDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                            <asp:TextBox ID="txtgrvToDate" Width="100px" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:TextBox><asp:ImageButton
                                                ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnToDate"
                                                ImageAlign="Middle" CausesValidation="false" />
                                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtgrvToDate"
                                                PopupButtonID="imgbtnToDate" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrvToDate" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                        </ItemTemplate>
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
                                            <asp:TextBox ID="txtgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approver Comments">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'
                                                TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                        <ItemStyle HorizontalAlign="Center" Width="14%"></ItemStyle>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"
                                                CausesValidation="false"></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton ID="lbnCancel" runat="server" Text="Cancel" CommandName="lbnCancel"
                                                CausesValidation="false"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblApproved" runat="server"></asp:Label>
                                            <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                                CommandArgument='<%#  Eval("LeaveDetailID") %>'>Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>--%>
        <%--</td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False"></asp:ValidationSummary>
            </td>
        </tr>--%>
    </table>
</asp:Content>