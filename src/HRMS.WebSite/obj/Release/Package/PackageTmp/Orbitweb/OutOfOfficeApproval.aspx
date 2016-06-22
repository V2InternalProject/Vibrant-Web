<%@ Page Title="OutOfOfficeApproval" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="OutOfOfficeApproval.aspx.cs" Inherits="HRMS.Orbitweb.OutOfOfficeApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/jquery.selectbox-0.2.min.js"></script>
    <script type="text/javascript">
        function Validation(ddlOutTimeHrs, ddlOutTimeMins, ddlInTimeHrs, ddlInTimeMins, txtApproversComment) {
            var txtApproversComment = txtApproversComment;

            var ddlOutTimeHrs = ddlOutTimeHrs;
            var ddlOutTimeMins = ddlOutTimeMins;

            var ddlInTimeHrs = ddlInTimeHrs;
            var ddlInTimeMins = ddlInTimeMins;

            if (parseInt(ddlOutTimeHrs.value) > parseInt(ddlInTimeHrs.value)) {
                alert("please select proper Out-Time and In-Time Hours");
                return false;
            }
            else if (parseInt(ddlOutTimeHrs.value) == parseInt(ddlInTimeHrs.value)) {
                if (parseInt(ddlOutTimeMins.value) >= parseInt(ddlInTimeMins.value)) {
                    alert("Please select proper In-Time and Out-Time Minutes ");
                    return false;
                }
            }
            if (txtApproversComment.value.trim() == "") {
                alert("Please enter the comment");
                return false;
            }

        }
        $(document).ready(function () {
            $('.OrbitFilterLink').click(function () {
                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
            });

            if($("#<%=grvOutOfOfficeApproval.ClientID %> tr").length > 0 && $('#MainContent_ddlStatus').val() != "0"){
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
    <asp:ScriptManager runat="server" ID="ScriptManager2">
    </asp:ScriptManager>
    <asp:HiddenField ID="selected_tab" runat="server" />
    <asp:HiddenField ID="mainTab_Selected" runat="server" />
    <asp:HiddenField ID="GridEditModel" runat="server" />
    <div class="AttendancePage">
        <div id="page">
            <%--<section class="clearfix">
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
                        <h2 class="MainHeading">Approvals</h2>
                        <%--<div class="EmpSearch clearfix">
								<a href="#"></a>
								<input type="text" placeholder="Employee Search">
							</div>--%>
                    </div>
                    <nav class="sub-menu-colored">
                        <a href="SignInSignOutApproval.aspx">SignIn SignOut</a>
                        <a href="LeaveApproval.aspx">Leave</a>
                        <a href="CompensationApproval.aspx">Compensatory Leave</a>
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
                                <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchFromDate" runat="server" placeholder="From Date"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" ImageUrl="~/images/New Design/calender-icon.png"
                                    ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger mrgnR12"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                    TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvsearchFromDate" runat="server" ErrorMessage="Select From Date "
                                    ControlToValidate="txtSearchFromDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtSearchToDate" runat="server" placeholder="To Date"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnSearchToDate" runat="server" ImageUrl="~/images/New Design/calender-icon.png"
                                    ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                    PopupButtonID="imgbtnSearchToDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" ErrorMessage="Select To Date "
                                    ControlToValidate="txtSearchToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:Button type="Button2" class="OrbitFilterImage" ID="btnSearch" runat="server"
                                    OnClick="btnSearch_Click" />
                                <asp:CompareValidator ID="CmpDate" runat="server" ErrorMessage="From date should not be greater than To date"
                                    ControlToValidate="txtSearchToDate" Display="None" Visible="True" ControlToCompare="txtSearchFromDate"
                                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                <%--  <a class="OrbitFilterImage" href="#"></a>--%>
                            </div>
                        </div>
                    </div>
                    <div class="InnerContainer scrollHContainer">
                        <asp:GridView ID="grvOutOfOfficeApproval" runat="server" Width="100%" AllowSorting="True"
                            OnSorting="grvOutOfOfficeApproval_Sorting" AutoGenerateColumns="False" OnRowDataBound="grvOutOfOfficeApproval_RowDataBound"
                            OnRowEditing="grvOutOfOfficeApproval_RowEditing" OnRowCancelingEdit="grvOutOfOfficeApproval_RowCancelingEdit"
                            OnRowUpdating="grvOutOfOfficeApproval_RowUpdating" AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="grvOutOfOfficeApproval_PageIndexChanging" CssClass="TableJqgrid">
                            <HeaderStyle CssClass="tableHeaders" />
                            <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                                LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                                PreviousPageText="Prev" />
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                            <RowStyle CssClass="tableRows" />
                            <Columns>
                                <asp:TemplateField HeaderText="Out Of Office ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutofOFficeid" runat="server" Text='<%# Eval("OutOfOfficeID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OutofOfficeWFID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOutofOfficeWFID" Text='<%# Eval("WorkflowID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" SortExpression="OutTimeDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="Server" Text='<%# Eval("OutTimeDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Time1" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutTime1" runat="server" Text='<%# Eval("OutTimeTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Time" SortExpression="OutTimeTime">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlOutTimeHrs" runat="Server">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlOutTimeMins" runat="Server">
                                            <asp:ListItem Value="1">00</asp:ListItem>
                                            <asp:ListItem Value="2">10</asp:ListItem>
                                            <asp:ListItem Value="3">20</asp:ListItem>
                                            <asp:ListItem Value="4">30</asp:ListItem>
                                            <asp:ListItem Value="5">40</asp:ListItem>
                                            <asp:ListItem Value="6">50</asp:ListItem>
                                            <asp:ListItem Value="7">59</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutTime" runat="server" Text='<%# Eval("OutTimeTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Time Time " Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInTime1" runat="server" Text='<%# Eval("InTimeTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Time" SortExpression="InTimeTime">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInTimeHrs" runat="Server">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlInTimeMins" runat="Server">
                                            <asp:ListItem Value="1">00</asp:ListItem>
                                            <asp:ListItem Value="2">10</asp:ListItem>
                                            <asp:ListItem Value="3">20</asp:ListItem>
                                            <asp:ListItem Value="4">30</asp:ListItem>
                                            <asp:ListItem Value="5">40</asp:ListItem>
                                            <asp:ListItem Value="6">50</asp:ListItem>
                                            <asp:ListItem Value="7">59</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInTime" runat="server" Text='<%# Eval("InTimeTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTypeid" runat="server" Text='<%# Eval("TypeID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason" SortExpression="Reason">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comments" SortExpression="Comment">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusid" runat="server" Text='<%# Eval("StatusID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlApproved" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblstatusApproved" runat="server" Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approver ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproverID" runat="Server" Text='<%# Eval ("ApproverID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approver's Comments" SortExpression="ApproverComments">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtApproversComment" runat="server" TextMode="MultiLine" Text='<%#Bind("ApproverComments") %>'>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproversComment" runat="server" Text='<%#Eval("ApproverComments") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnUpdate" runat="Server" Text="Update" CommandName="Update"
                                            CausesValidation="true" ValidationGroup="vgOutInTime"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="Server" Text="cancel" CommandName="Cancel"
                                            CausesValidation="FALSE"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproved" runat="Server"></asp:Label>
                                        <asp:LinkButton ID="lnkEdit" runat="Server" Text="Edit" CommandName="Edit" CausesValidation="FALSE">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False"></asp:ValidationSummary>
                <asp:ValidationSummary ID="vsOutInTime" runat="server" ShowMessageBox="True" ShowSummary="False"
                    ValidationGroup="vgOutInTime"></asp:ValidationSummary>
            </section>
            <%-- <footer>&#169; 2008 V2Solutions, Inc.</footer>--%>
        </div>
        <script type="text/javascript" language="javascript" src="footer.js"></script>
    </div>
    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
        <tbody>
            <tr>
                <td class="tableHeadBlueLight" align="center">Out Of Office Approval
                </td>
            </tr>
            <tr>
                <td class="lineDotted" valign="baseline"></td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr>
                <td align="center">
                    <%-- <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                    <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr>
                <td>
                    <table class="tableBorder" width="100%">
                        <tbody>
                            <tr>
                                <td class="h10" colspan="8"></td>
                            </tr>
                            <tr>
                                <td width="5%"></td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblType" runat="server" Text="Select Type :"></asp:Label>
                                </td>
                                <td align="left" width="20%">
                                    <%--                                    <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%--  <asp:TextBox ID="txtSearchFromDate" runat="server" Width="100px"></asp:TextBox>--%>
                                    <%--<asp:ImageButton ID="imgbtnSearchFromDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                        ImageAlign="AbsMiddle" CausesValidation="false"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                        TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvsearchFromDate" runat="server" ErrorMessage="Select From Date "
                                        ControlToValidate="txtSearchFromDate" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%-- <asp:TextBox ID="txtSearchToDate" runat="server" Width="100px"></asp:TextBox>--%>
                                    <%--<asp:ImageButton ID="imgbtnSearchToDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                        ImageAlign="AbsMiddle" CausesValidation="false"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                        PopupButtonID="imgbtnSearchToDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" ErrorMessage="Select To Date "
                                        ControlToValidate="txtSearchToDate" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td align="center" width="10%">
                                    <%--  <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search">
                                    </asp:Button>--%>
                                    <%-- <asp:CompareValidator ID="CmpDate" runat="server" ErrorMessage="From date should not be greater than To date"
                                        ControlToValidate="txtSearchToDate" Display="None" Visible="True" ControlToCompare="txtSearchFromDate"
                                        Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                                </td>
                                <td>
                                    <%-- <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />&nbsp;--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="h10" colspan="8"></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr>
                <td>
                    <%--   <asp:GridView ID="grvOutOfOfficeApproval" runat="server" Width="100%" AllowSorting="True"
                        OnSorting="grvOutOfOfficeApproval_Sorting" AutoGenerateColumns="False" OnRowDataBound="grvOutOfOfficeApproval_RowDataBound"
                        OnRowEditing="grvOutOfOfficeApproval_RowEditing" OnRowCancelingEdit="grvOutOfOfficeApproval_RowCancelingEdit"
                        OnRowUpdating="grvOutOfOfficeApproval_RowUpdating" AllowPaging="true" PageSize="10"
                        OnPageIndexChanging="grvOutOfOfficeApproval_PageIndexChanging" CssClass="grid">
                        <Columns>
                            <asp:TemplateField HeaderText="Out Of Office ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutofOFficeid" runat="server" Text='<%# Eval("OutOfOfficeID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OutofOfficeWFID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOutofOfficeWFID" Text='<%# Eval("WorkflowID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" SortExpression="OutTimeDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="Server" Text='<%# Eval("OutTimeDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time1" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutTime1" runat="server" Text='<%# Eval("OutTimeTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time" SortExpression="OutTimeTime">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlOutTimeHrs" runat="Server">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlOutTimeMins" runat="Server">
                                        <asp:ListItem Value="1">00</asp:ListItem>
                                        <asp:ListItem Value="2">10</asp:ListItem>
                                        <asp:ListItem Value="3">20</asp:ListItem>
                                        <asp:ListItem Value="4">30</asp:ListItem>
                                        <asp:ListItem Value="5">40</asp:ListItem>
                                        <asp:ListItem Value="6">50</asp:ListItem>
                                        <asp:ListItem Value="7">59</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOutTime" runat="server" Text='<%# Eval("OutTimeTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time Time " Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblInTime1" runat="server" Text='<%# Eval("InTimeTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time" SortExpression="InTimeTime">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlInTimeHrs" runat="Server">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlInTimeMins" runat="Server">
                                        <asp:ListItem Value="1">00</asp:ListItem>
                                        <asp:ListItem Value="2">10</asp:ListItem>
                                        <asp:ListItem Value="3">20</asp:ListItem>
                                        <asp:ListItem Value="4">30</asp:ListItem>
                                        <asp:ListItem Value="5">40</asp:ListItem>
                                        <asp:ListItem Value="6">50</asp:ListItem>
                                        <asp:ListItem Value="7">59</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInTime" runat="server" Text='<%# Eval("InTimeTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblTypeid" runat="server" Text='<%# Eval("TypeID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason" SortExpression="Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments" SortExpression="Comment">

                                <ItemTemplate>
                                    <asp:Label ID="lblComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusid" runat="server" Text='<%# Eval("StatusID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlApproved" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblstatusApproved" runat="server" Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproverID" runat="Server" Text='<%# Eval ("ApproverID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver's Comments" SortExpression="ApproverComments">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtApproversComment" runat="server" TextMode="MultiLine" Text='<%#Bind("ApproverComments") %>'>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApproversComment" runat="server" Text='<%#Eval("ApproverComments") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemStyle HorizontalAlign="Center" />
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="Server" Text="Update" CommandName="Update"
                                        CausesValidation="true" ValidationGroup="vgOutInTime"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancel" runat="Server" Text="cancel" CommandName="Cancel"
                                        CausesValidation="FALSE"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApproved" runat="Server"></asp:Label>
                                    <asp:LinkButton ID="lnkEdit" runat="Server" Text="Edit" CommandName="Edit" CausesValidation="FALSE">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                </td>
                <td></td>
            </tr>
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False"></asp:ValidationSummary>
            <asp:ValidationSummary ID="vsOutInTime" runat="server" ShowMessageBox="True" ShowSummary="False"
                ValidationGroup="vgOutInTime"></asp:ValidationSummary>--%>
        </tbody>
    </table>
</asp:Content>