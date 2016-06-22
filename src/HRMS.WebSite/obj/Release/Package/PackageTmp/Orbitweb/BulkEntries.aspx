<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="BulkEntries.aspx.cs" Inherits="HRMS.Orbitweb.BulkEntries" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/New%20Design/common.js" type="text/javascript"></script>

    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <script language="javascript">
        $(document).ready(function () {

            $('.OrbitFilterLink').click(function () {

                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
            });
            var name = "<%=ViewState["Search"]%>";
            if (name == 1) {
                $('#tab2').addClass('colored-border');
                $('#tab1').removeClass('colored-border');
                //$('#tab3').removeClass('colored-border');
                $('#tab2').removeClass('.tabshover');
                $('#tab1').addClass('tabshover');
                //$('#tab3').addClass('tabshover');
                $('.add-detailsdata').hide();
                $('.search-detailsdata').show();
                // $('.holiday-listdata').hide();
            }
            else if (name == 0) {
                $('#tab1').addClass('colored-border');
                $('#tab2').removeClass('colored-border');
                //$('#tab3').removeClass('colored-border');
                $('#tab1').removeClass('tabshover');
                $('#tab2').addClass('tabshover');
                //$('#tab3').addClass('tabshover');
                $('.add-detailsdata').show();
                $('.search-detailsdata').hide();
                //$('.holiday-listdata').hide();

            }
        });
        function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal) {
            re = new RegExp('$' + aspCheckBoxID + '$');  //generated control name starts with a dollar
            //for (i = 0; i < document.aspnetForm.elements.length; i++) {document.forms.form1.elements[21].type
            for (i = 0; i < document.forms.form1.elements.length; i++) {
                //                elm = document.aspnetForm.elements[i];
                elm = document.forms.form1.elements[i];
                //alert(elm.type)
                if (elm.type == 'checkbox') {
                    elm.checked = checkVal;
                }
            }
        }

        function Validation() {
            //            var txtFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate");
            var txtFromDate = document.getElementById("MainContent_txtFromDate");
            //            var txtToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate");
            var txtToDate = document.getElementById("MainContent_txtToDate");

            //            var txtEmployee = document.getElementById("ctl00_ContentPlaceHolder1_txtempname");
            var txtEmployee = document.getElementById("MainContent_txtempname");
            //var valueEmployee = ddlEmployee.options[ddlEmployee.selectedIndex].value;

            if (txtEmployee.value == "") {
                alert("Please enter Employee Name");
                return false;
            }
            else if (txtFromDate.value == "") {
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
                if (txtFromDate.value == "" || txtToDate.value == "")
                    alert("Both dates must be entered.");
                else
                    // alert("To date must occur after the from date.");
                    alert("From date should not be greater than To date");
                return false;
            }

        }

        function SearchValidation() {
            DisplayLoadingDialog();

            //            var txtSearchFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txtSearchFromDate");
            //            var txtSearchToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtSearchToDate");
            var txtSearchFromDate = document.getElementById("MainContent_txtSearchFromDate");
            var txtSearchToDate = document.getElementById("MainContent_txtSearchToDate");

            if (txtSearchFromDate.value == "") {
                alert("Please select From date");
                return false;
            }
            else if (txtSearchToDate.value == "") {
                alert("Please select To date");
                return false;
            }
            else if (Date.parse(txtSearchFromDate.value) <= Date.parse(txtSearchToDate.value)) {
                //alert("The dates are valid.");
            }
            else {
                if (txtSearchFromDate.value == "" || txtSearchToDate.value == "")
                    alert("Both dates must be entered.");
                else
                    // alert("To date must occur after the from date.");
                    alert("From date should not be greater than To date");
                return false;
            }

        }

        function ValidationButton() {

            //            var txtReason = document.getElementById("ctl00_ContentPlaceHolder1_txtReason");
            //            var cblGenerateDates = document.getElementById("ctl00_ContentPlaceHolder1_cblGenerateDates");

            var txtReason = document.getElementById("MainContent_txtReason");
            var cblGenerateDates = document.getElementById("MainContent_cblGenerateDates");

            //            var txtFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate");
            //            var txtToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate");

            var txtFromDate = document.getElementById("MainContent_txtFromDate");
            var txtToDate = document.getElementById("MainContent_txtToDate");

            //var ddlEmployee = document.getElementById("ctl00_ContentPlaceHolder1_ddlEmployee");
            //var valueEmployee = ddlEmployee.options[ddlEmployee.selectedIndex].value;
            //            var txtEmployee = document.getElementById("ctl00_ContentPlaceHolder1_txtempname");
            var txtEmployee = document.getElementById("MainContent_txtempname");

            if (txtEmployee.value == "") {
                alert("Please enter Employee Name");
                return false;
            }
            else if (txtFromDate.value == "") {
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
                if (txtFromDate.value == "" || txtToDate.value == "")
                    alert("Both dates must be entered.");
                else
                    // alert("To date must occur after the from date.");
                    alert("From date should not be greater than To date");
                return false;
            }

            var checked = 0;
            //            var chkObjlst = document.getElementById('ctl00_ContentPlaceHolder1_cblGenerateDates').getElementsByTagName("INPUT");
            var chkObjlst = document.getElementById('MainContent_cblGenerateDates').getElementsByTagName("INPUT");

            for (var i = 0; i < chkObjlst.length; i++) {

                if (chkObjlst[i].type == "checkbox") {

                    if (chkObjlst[i].checked == true) {
                        var checked = 1;
                    }
                }
            }

            if (checked == 0) {
                alert("Please Select atleast one checkbox");
                return false;
            }

            if (txtReason.value.trim() == "") {
                alert("Please enter Comments");
                return false;
            }
        }
    </script>
    <section class="AttendanceContainer Container">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                    Transaction</a>
                <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%>
                <a href="BulkEntries.aspx" class="selected">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a>
                <a href="HolidayList.aspx">Masters</a>
            </nav>
        </div>
        <div class="MainBody">
            <div class="tableHeadBlueLight">
                <span id="spanAddBulk" runat="server"></span><span id="spanSearch" runat="server"></span><span id="spanEdit" runat="server"></span>
            </div>
            <div>
                <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
            </div>
            <div class="Bulk-tabs clearfix">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">
                            <asp:LinkButton runat="server" ID="lnkAdd" Text="Add Bulk Details" OnClick="lnkAdd_Click"></asp:LinkButton></li>
                        <li id="tab2">
                            <asp:LinkButton runat="server" ID="lnkSearch" Text="Search Details" OnClick="lnkSearch_Click"></asp:LinkButton></li>
                    </ul>
                </div>
                <section class="add-detailsdata">
                    <div id="tdAddBulk" runat="server">
                        <div class="mainContentPad clearfix">
                            <div class="formrow clearfix">
                                <div class="leftcol clearfix">
                                    <div class="LabelDiv">
                                        <label>
                                            Employee Name:</label>
                                    </div>
                                    <div class="InputDiv">
                                        <asp:TextBox ID="txtempname" runat="server" AutoPostBack="true" Width="180px" OnTextChanged="txtempname_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtempid" runat="server" Visible="false" Width="75px"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="aceEmpName1" runat="server" TargetControlID="txtempname"
                                            ServicePath="~/LeaveTransactionAutoComplete.asmx" ServiceMethod="GetEmployeeName"
                                            MinimumPrefixLength="1" CompletionListCssClass="list2" CompletionListItemCssClass="listitem2"
                                            CompletionListHighlightedItemCssClass="hoverlistitem2">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="formrow clearfix">
                                <div class="leftcol clearfix">
                                    <div class="LabelDiv">
                                        <label>
                                            From Date:</label>
                                    </div>
                                    <div class="InputDiv">
                                        <asp:TextBox ID="txtFromDate" runat="server" OnTextChanged="txtFromDate_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                        <asp:ImageButton ID="imgFromDate" runat="server" class="ui-datepicker-trigger mrgnR12"
                                            ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="Middle" />
                                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" PopupButtonID="imgFromDate"
                                            runat="server" TargetControlID="txtFromDate">
                                        </ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>
                                <div class="rightcol GenerateD">
                                    <div class="LabelDiv">
                                        <label>
                                            To Date:</label>
                                    </div>
                                    <div class="InputDiv fLeft">
                                        <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                        <asp:ImageButton ID="imgToDate" runat="server" class="ui-datepicker-trigger"
                                            ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="AbsMiddle" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarButtonExtenderToDate" PopupButtonID="imgToDate"
                                            runat="server" TargetControlID="txtToDate">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:LinkButton ID="lnkbtnGenerateDates" runat="server" Text="Generate dates" OnClick="lnkbtnGenerateDates_Click" />

                                        <asp:Label ID="lblGenarate" runat="server" SkinID="lblError" ForeColor="Red" CssClass="gendates" Text="Please click 'Generate Dates'"
                                            Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="generatedDates clearfix" id="tdGenerate" runat="server">
                                    <%--<div class="datePad">
                                    <input type="checkbox" />
                                    <div class="date">
                                        06/06/2014</div>
                                </div>
                                <div class="datePad">
                                    <input type="checkbox" />
                                    <div class="date">
                                        06/06/2014</div>
                                </div>
                                <div class="datePad">
                                    <input type="checkbox" />
                                    <div class="date">
                                        06/06/2014</div>
                                </div>--%>
                                    <asp:CheckBoxList ID="cblGenerateDates" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                        CellSpacing="5" RepeatLayout="Table" CssClass="CheckBoxList">
                                    </asp:CheckBoxList>
                                    <input id="chkIsuueIdAll" name="chkIsuueIdAll" type="checkbox" onclick="JavaScript: CheckAllDataGridCheckBoxes('ctl00_ContentPlaceHolder1_cblGenerateDates', this.checked);" />
                                    <label for="chkIsuueIdAll" class="LabelForCheckbox" id="lblAll">
                                        Select All</label>
                                    <%--<asp:Label ID="lblAll" runat="server" Text="Select All"></asp:Label>--%>
                                </div>
                            </div>
                            <div class="formrow clearfix">
                                <div class="leftcol clearfix">
                                    <div class="LabelDiv">
                                        <asp:Label runat="server" ID="lblReason" Text="Approver Comments"></asp:Label><asp:Label
                                            runat="server" ID="lblReasonDot" Text=":"></asp:Label>
                                    </div>
                                    <div class="InputDiv">
                                        <asp:TextBox ID="txtReason" Width="290px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- mainContentPad -->
                        <div class="clearfix">
                            <%--<input type="button" class="ButtonGray mrgnR11" value="Add Comments">
                        <input type="button" class="ButtonGray" value="Upload File">--%>
                            <asp:Button ID="btnSubmit" class="ButtonGray mrgnR11" runat="server" Text="Submit"
                                OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </section>
                <section class="search-detailsdata clearfix">
                    <%--<div id="tdSearch" runat="server">--%>
                    <div class="OrbitFilter">
                        <a href="#" class="OrbitFilterLink floatR">Filters</a>
                    </div>
                    <div class="OrbitFilterExpand" style="display: none;">
                        <asp:DropDownList Width="150px" ID="ddlEmployeeNames" runat="server" OnSelectedIndexChanged="ddlEmployeeNames_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="0" Text="All"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" Width="100px" runat="server"></asp:TextBox>
                        <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" class="ui-datepicker-trigger mrgnR12"
                            runat="server" ID="imgbtnSearchFromDate" ImageAlign="Middle" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                            TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                        <asp:TextBox ID="txtSearchToDate" Width="100px" placeholder="To Date" runat="server"></asp:TextBox>
                        <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" class="ui-datepicker-trigger mrgnR12"
                            runat="server" ID="imgbtnSearchToDate" ImageAlign="Middle" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                            PopupButtonID="imgbtnSearchToDate" />
                        <asp:Button ID="btnSearch" runat="server" class="OrbitFilterImage" Text="" OnClick="btnSearch_Click" />
                    </div>
                    <%-- </div>--%>
                </section>
            </div>
            <div class="InnerContainer mrgnT20">
                <asp:GridView ID="gvBulkEntries" Width="100%" runat="server" AutoGenerateColumns="False"
                    AllowSorting="true" OnRowDeleting="gvBulkEntries_RowDeleting" OnPageIndexChanging="gvBulkEntries_PageIndexChanging"
                    OnRowDataBound="gvBulkEntries_RowDataBound" OnSorting="gvBulkEntries_Sorting"
                    CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                        LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                        PreviousPageText="Prev" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <RowStyle CssClass="tableRows" />
                    <Columns>
                        <asp:TemplateField HeaderText="SignInSignOutID" Visible="False">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSignInSignOutID" Text='<%# Eval("SignInSignOutID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SignInTime" SortExpression="SignInTime">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvInTime" runat="server" Text='<%# Bind("SignInTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SignOutTime" SortExpression="SignOutTime">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvOutTime" runat="server" Text='<%# Bind("SignOutTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Hours Worked" SortExpression="TotalHoursWorked">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvHours" runat="server" Text='<%# Bind("TotalHoursWorked") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status Id" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ApproverID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <itemstyle horizontalalign="Center" width="16%"></itemstyle>
                                <asp:Label ID="lblApproved" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkButDelete" runat="server" CommandName="Delete" CausesValidation="False"
                                    CommandArgument='<%#  Eval("SignInSignOutID") %>' OnClientClick="return confirm('Are you sure you want to Delete this Record?');">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>