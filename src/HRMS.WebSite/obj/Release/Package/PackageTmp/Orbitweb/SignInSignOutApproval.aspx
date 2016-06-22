<%@ Page Title="SignInSignOutApproval" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="True" CodeBehind="SignInSignOutApproval.aspx.cs" Inherits="HRMS.Orbitweb.SignInSignOutApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <!-- SelectBOx -->
    <%--  <script src="../JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <script type="text/javascript">

        //New addition

        $(document).ready(function () {

            //             if ($("[id$=mainTab_Selected]").val() == "CompOff") {
            //                 $('#CompOffDetails').addClass('selected');
            //                 $('#LeaveDetails').removeClass('selected');
            //             }
            //             else {
            //                 $('#CompOffDetails').removeClass('selected');
            //                 $('#LeaveDetails').addClass('selected');
            //             }

            if ($('#MainContent_ddlStatus :selected').val() != "0" && $("#<%=grvSISOApproval.ClientID %> tr").length > 0) {
                 $('.OrbitFilterExpand').show();
             }

             $('.OrbitFilterLink').click(function () {
                 $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
             });
         });

         //$(function () {
         //    $('select').selectbox();
         //    $('.sbOptions a').hover(function () {
         //        $(this).parent().toggleClass("hoveroption");
         //    });
         //});
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

            <%--					<header id="header">
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
					</header>--%>

            <section class="AttendanceContainer Container">
                <div class="FixedHeader">
                    <div class="clearfix">
                        <h2 class="MainHeading">Approvals</h2>
                        <%--							<div class="EmpSearch clearfix">
								<a href="#"></a>
								<input type="text" placeholder="Employee Search">
							</div>--%>
                    </div>
                    <nav class="sub-menu-colored">
                        <a href="SignInSignOutApproval.aspx" class="selected">SignIn SignOut</a>
                        <a href="LeaveApproval.aspx">Leave</a>
                        <a href="CompensationApproval.aspx">Compensatory Leave</a>
                    </nav>
                </div>
                <div class="MainBody">
                    <div class="SuccessMsgOrbit" align="center">
                        <asp:Label ID="lblSuccess" runat="server" SkinID="lblSuccess"></asp:Label>
                    </div>
                    <div class="ErrorMsgOrbit" align="center">
                        <asp:Label ID="lblErrorMess" runat="server" SkinID="lblError"></asp:Label>
                    </div>
                    <div class="OrbitAuto">
                        <div class="clearfix">
                            <div class="OrbitFilter">
                                <a href="#" class="OrbitFilterLink floatR">Filters</a>
                            </div>
                            <div class="OrbitFilterExpand" style="display: none;">

                                <asp:DropDownList Width="150px" ID="ddlStatus" runat="server" EnableViewState="true"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                </asp:DropDownList>

                                <asp:TextBox ID="txtSearchFromDate" runat="server" MaxLength="256" placeholder="From Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchFromDate"
                                    ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                    TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="Please Select the From Date"
                                    ControlToValidate="txtSearchFromDate" Display="None" SetFocusOnError="True" ValidationGroup="Search"></asp:RequiredFieldValidator>

                                <asp:TextBox ID="txtSearchToDate" runat="server" MaxLength="256" placeholder="To Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchToDate"
                                    ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                    PopupButtonID="imgbtnSearchToDate" />
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtSearchToDate"
                                    Display="None" ErrorMessage="Please Select the To Date" SetFocusOnError="True"
                                    ValidationGroup="Search"></asp:RequiredFieldValidator>
                                <%--<a class="OrbitFilterImage" href="#"></a>--%>

                                <asp:Button ID="btnSearch" runat="server" class="OrbitFilterImage" OnClick="btnSearch_Click"
                                    ValidationGroup="Search" />
                            </div>
                        </div>
                    </div>
                    <div class="InnerContainer scrollHContainer">
                        <asp:GridView ID="grvSISOApproval" Width="100%" runat="server" AutoGenerateColumns="False"
                            OnRowEditing="grvSISOApproval_RowEditing" OnRowUpdating="grvSISOApproval_RowUpdating"
                            OnRowDataBound="grvSISOApproval_RowDataBound" OnRowCancelingEdit="grvSISOApproval_RowCancelingEdit"
                            OnPageIndexChanging="grvSISOApproval_PageIndexChanging" OnSorting="grvSISOApproval_Sorting"
                            AllowPaging="True" AllowSorting="True" CssClass="TableJqgrid">
                            <HeaderStyle CssClass="tableHeaders" />
                            <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png"
                                FirstPageText="" LastPageImageUrl="~/Images/New Design/next.png"
                                LastPageText="" NextPageText="Next" PreviousPageText="Prev" />
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
                                <asp:TemplateField HeaderText="User Name" SortExpression="EmployeeName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblUserName1" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" SortExpression="date">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("date") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate1" runat="server" Text='<%# Bind("date") %>'></asp:Label>
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
                                        </asp:DropDownList>&nbsp; &nbsp;<asp:DropDownList ID="ddlOutTimeMins" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSignOutTime" runat="server" Text='<%# Eval("SignOutTime") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblOutDate" runat="server" Text='<%# Eval("OutDate") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSignOutTime1" runat="server" Text='<%# Eval("SignOutTime") %>'></asp:Label>
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
                                    <EditItemTemplate>
                                        <asp:Label ID="lblSignInComment1" runat="server" Text='<%# Eval("SignInComment") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sign Out Comment" SortExpression="SignOutComment">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSignOutComment" runat="server" Text='<%# Eval("SignOutComment") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblSignOutComment1" runat="server" Text='<%# Eval("SignOutComment") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved" SortExpression="Status">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                        &nbsp;<asp:DropDownList ID="ddlStatusdEdit" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus1" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approver's Comments" SortExpression="ApproverComments">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtApproversComments" runat="server" TextMode="MultiLine" Text='<%# Eval("ApproverComments") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvApproversComments" runat="server" ControlToValidate="txtApproversComments"
                                            Display="None" ErrorMessage="Please enter the comments" SetFocusOnError="True"
                                            ValidationGroup="Edit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproversComments" runat="server" Text='<%# Eval("ApproverComments") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Modified Date" SortExpression="LastModified">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastModified" runat="server" Text='<%# Eval("LastModified", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" ValidationGroup="Edit"
                                            CommandName="Update" Text="Update"></asp:LinkButton>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                            Text="Edit"></asp:LinkButton>
                                        <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                            ShowMessageBox="true" ShowSummary="false" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Edit"
                            ShowMessageBox="true" ShowSummary="false" />
                    </div>
                </div>
            </section>
            <footer>&#169; 2008 V2Solutions, Inc.</footer>
        </div>
        <script type="text/javascript" language="javascript" src="footer.js"></script>
    </div>

    <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0" style="display: none">
        <tr>
            <td align="center" class="tableHeadBlueLight">SignIn SignOut Approval
            </td>
        </tr>
        <tr>
            <td valign="baseline" class="lineDotted" style="height: 20px"></td>
        </tr>
        <%--        <tr align="center">
            <td>
                <asp:Label ID="lblSuccess" runat="server" SkinID="lblSuccess"></asp:Label></td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;<asp:Label ID="lblErrorMess" runat="server" SkinID="lblError"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td class="h5"></td>
        </tr>
        <tr>
            <td>
                <table width="100%" class="tableBorder">
                    <tr>
                        <td colspan="8" class="h10"></td>
                    </tr>
                    <tr>
                        <td style="width: 5%"></td>
                        <td align="right">
                            <asp:Label ID="lblType" runat="server" Text="Select Type :"></asp:Label>
                        </td>
                        <td align="left" width="20%">
                            <%--<asp:DropDownList Width="150px" ID="ddlStatus" runat="server" EnableViewState="true"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                        </td>
                        <td align="left">
                            <%--                            <asp:TextBox ID="txtSearchFromDate" Width="100px" runat="server" MaxLength="256"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchFromDate"
                                ImageAlign="AbsMiddle" CausesValidation="False" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="Please Select the From Date"
                                ControlToValidate="txtSearchFromDate" Display="None" SetFocusOnError="True" ValidationGroup="Search"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                        </td>
                        <td align="left">
                            <%--<asp:TextBox ID="txtSearchToDate" Width="100px" runat="server" MaxLength="256"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchToDate"
                                ImageAlign="AbsMiddle" CausesValidation="False" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                PopupButtonID="imgbtnSearchToDate" />
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtSearchToDate"
                                Display="None" ErrorMessage="Please Select the To Date" SetFocusOnError="True"
                                ValidationGroup="Search"></asp:RequiredFieldValidator>--%>&nbsp;
                        </td>
                        <td align="center">
                            <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                ValidationGroup="Search" />--%>
                        </td>
                        <td align="center" style="display: none">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="h10"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="h10"></td>
        </tr>
        <tr>
            <td>
                <%--                <asp:GridView ID="grvSISOApproval" Width="100%" runat="server" AutoGenerateColumns="False"
                    OnRowEditing="grvSISOApproval_RowEditing" OnRowUpdating="grvSISOApproval_RowUpdating"
                    OnRowDataBound="grvSISOApproval_RowDataBound" OnRowCancelingEdit="grvSISOApproval_RowCancelingEdit"
                    OnPageIndexChanging="grvSISOApproval_PageIndexChanging" OnSorting="grvSISOApproval_Sorting"
                    AllowPaging="True" AllowSorting="True" CssClass="grid">
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
                        <asp:TemplateField HeaderText="User Name" SortExpression="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblUserName1" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="date">
                            <EditItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("date") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDate1" runat="server" Text='<%# Bind("date") %>'></asp:Label>
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
                                </asp:DropDownList>&nbsp; &nbsp;<asp:DropDownList ID="ddlOutTimeMins" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lblSignOutTime" runat="server" Text='<%# Eval("SignOutTime") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblOutDate" runat="server" Text='<%# Eval("OutDate") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSignOutTime1" runat="server" Text='<%# Eval("SignOutTime") %>'></asp:Label>
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
                            <EditItemTemplate>
                                <asp:Label ID="lblSignInComment1" runat="server" Text='<%# Eval("SignInComment") %>'></asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sign Out Comment" SortExpression="SignOutComment">
                            <ItemTemplate>
                                <asp:Label ID="lblSignOutComment" runat="server" Text='<%# Eval("SignOutComment") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblSignOutComment1" runat="server" Text='<%# Eval("SignOutComment") %>'></asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approved" SortExpression="Status">
                            <EditItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                &nbsp;<asp:DropDownList ID="ddlStatusdEdit" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus1" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver's Comments" SortExpression="ApproverComments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtApproversComments" runat="server" TextMode="MultiLine" Text='<%# Eval("ApproverComments") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvApproversComments" runat="server" ControlToValidate="txtApproversComments"
                                    Display="None" ErrorMessage="Please enter the comments" SetFocusOnError="True"
                                    ValidationGroup="Edit"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApproversComments" runat="server" Text='<%# Eval("ApproverComments") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Modified Date" SortExpression="LastModified">
                            <ItemTemplate>
                                <asp:Label ID="lblLastModified" runat="server" Text='<%# Eval("LastModified") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" ValidationGroup="Edit"
                                    CommandName="Update" Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit"></asp:LinkButton>
                                <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Search"
                    ShowMessageBox="true" ShowSummary="false" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Edit"
                    ShowMessageBox="true" ShowSummary="false" />--%>
            </td>
        </tr>
    </table>
</asp:Content>