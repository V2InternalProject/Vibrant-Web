<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="AdminLeaveApplicationForm.aspx.cs" Inherits="HRMS.Orbitweb.AdminLeaveApplicationForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/New%20Design/common.js" type="text/javascript"></script>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnablePartialRendering="true">
    </ajaxToolkit:ToolkitScriptManager>

    <script language="javascript" type="text/javascript">
        //        $(document).ready(function () {
        //            $('.OrbitFilterLink').click(function () {
        //                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
        //            });
        //        });
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
    </script>
    <section class="LeaveMgmtContainer Container">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
                <%--<div class="EmpSearch clearfix">
                    <a href="#"></a>
                    <input type="text" placeholder="Employee Search">
                </div>--%>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                    Transaction</a> <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%> <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx" class="selected">Admin Leave
                            Application</a><a href="HolidayList.aspx">
                                Masters</a>
            </nav>
        </div>
        <div class="MainBody">
            <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="Server">--%>
            <%-- <ContentTemplate>--%>
            <div>
                <span id="spanAddLeave" runat="server"></span><span
                    id="spanSearch" runat="server"></span><span id="spanEdit"
                        runat="server"></span>
            </div>
            <div>
                <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
                <asp:Label ID="lblHidden" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="admin-leave-tabs clearfix">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">
                            <asp:LinkButton ID="lnkAddLeaves" OnClick="lnkAddLeaves_Click" runat="server"
                                Text="Add Leaves Details"></asp:LinkButton></li>
                        <li id="tab2">
                            <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server"
                                Text="Search Details"></asp:LinkButton></li>
                        <%-- <asp:LinkButton ID="lnkLeavePolicy" runat="server" CausesValidation="false" Text="Leave Policy"></asp:LinkButton>
                                <asp:LinkButton ID="lnkHolidayList" runat="server" CausesValidation="false" Text="Holiday List"
                                    OnClientClick="return false;" Visible="false"></asp:LinkButton>--%>
                        <!-- Info panel to be displayed as a flyout when the button is clicked -->
                        <div style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid; display: none; z-index: 2; overflow: hidden; border-left: #d0d0d0 1px solid; border-bottom: #d0d0d0 1px solid; background-color: #ffffff"
                            id="flyout" visible="true">
                        </div>
                    </ul>
                </div>
                <section class="add-detailsdata clearfix">
                    <div id="tdAddLeave" runat="server">
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblEmployee" runat="server" Text="Employee Name"></asp:Label>
                                    <asp:Label ID="lblName" runat="server" Text=":"></asp:Label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtempname" runat="server" AutoPostBack="true" Width="170px" OnTextChanged="txtempname_TextChanged"></asp:TextBox>
                                    <asp:TextBox ID="txtempid" runat="server" Visible="false" Width="75px"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender ID="aceEmpName1" runat="server" TargetControlID="txtempname"
                                        ServicePath="~/LeaveTransactionAutoComplete.asmx" ServiceMethod="GetEmployeeName"
                                        MinimumPrefixLength="1" CompletionListCssClass="list2" CompletionListItemCssClass="listitem2"
                                        CompletionListHighlightedItemCssClass="hoverlistitem2">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <%-- <asp:RequiredFieldValidator ID="rfvEmployee" runat="server" Display="None" ControlToValidate="txtempname"
                                                ErrorMessage="Please enter Employee Name"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div id="trLeaveBalance" runat="server">
                            <asp:Label ID="lblAvailable" runat="server" Visible="false" Text="Available leaves"></asp:Label><asp:Label
                                ID="lblAleave" runat="server" Visible="false" Text=":"></asp:Label><asp:Label ID="lblAvailableLeaves"
                                    runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label><asp:Label
                                        ID="lblFromDatedot" runat="server" Text=":"></asp:Label>
                                </div>
                                <div class="fill-dtls">
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" ReadOnly="false" onpaste="return false" autocomplete="off"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnFromDate" runat="server" class="ui-datepicker-trigger mrgnR12"
                                        ImageUrl="~/images/New Design/calender-icon.png" CausesValidation="false" ImageAlign="AbsMiddle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                        TargetControlID="txtFromDate" PopupButtonID="imgbtnFromDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <%--  <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" Display="None" ControlToValidate="txtFromDate"
                                                ErrorMessage="Select From Date "></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label><asp:Label ID="lblToDatDOt"
                                        runat="server" Text=":"></asp:Label>
                                </div>
                                <div class="fill-dtls fLeft">
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" ReadOnly="false" onpaste="return false" autocomplete="off"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnToDate" class="ui-datepicker-trigger mrgnR12" runat="server"
                                        ImageUrl="~/images/New Design/calender-icon.png" CausesValidation="false" ImageAlign="Middle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtToDate"
                                        PopupButtonID="imgbtnToDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <%-- <asp:RequiredFieldValidator ID="rfvToDate" runat="server" Display="None" ControlToValidate="txtToDate"
                                                ErrorMessage="Select To Date "></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpDates" runat="server" Display="None" ControlToValidate="txtToDate"
                                                ErrorMessage="From date should not be greater than To date" ControlToCompare="txtFromDate"
                                                Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblReason" runat="server" Text="Approval Comments"></asp:Label><asp:Label
                                        ID="lblReasonDot" runat="server" Text=":"></asp:Label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtReason" runat="server" Width="170px" TextMode="MultiLine"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="rfvResason" runat="server" Display="None" ControlToValidate="txtReason"
                                                ErrorMessage="Enter Comments "></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" class="ButtonGray" runat="server"
                                Text="Submit"></asp:Button>
                        </div>
                    </div>
                </section>
                <section class="search-detailsdata clearfix">
                    <%-- <div id="tdSearch" runat="server">--%>
                    <div class="OrbitFilter">
                        <a href="#" class="OrbitFilterLink floatR">Filters</a>
                    </div>
                    <div class="OrbitFilterExpand" style="display: none;">
                        <asp:DropDownList ID="ddlEmployeeName" runat="server" OnSelectedIndexChanged="ddlEmployeeName_SelectedIndexChanged"
                            AutoPostBack="true" Width="150px">
                            <asp:ListItem Value="0" Text="All"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" runat="server" Width="100px"
                            ReadOnly="false" onpaste="return false" autocomplete="off"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnSearchFromDate" class="ui-datepicker-trigger mrgnR12"
                            runat="server" ImageUrl="~/images/New Design/calender-icon.png" CausesValidation="false"
                            ImageAlign="Middle"></asp:ImageButton>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                            TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate">
                        </ajaxToolkit:CalendarExtender>
                        <%-- <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" Display="None"
                                    ControlToValidate="txtSearchFromDate" ErrorMessage="Select From Date "></asp:RequiredFieldValidator>--%>
                        <asp:TextBox ID="txtSearchToDate" placeholder="To Date" runat="server" Width="100px"
                            ReadOnly="false" onpaste="return false" autocomplete="off"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnSearchToDate" class="ui-datepicker-trigger mrgnR12" runat="server"
                            ImageUrl="~/images/New Design/calender-icon.png" CausesValidation="false" ImageAlign="Middle"></asp:ImageButton>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                            PopupButtonID="imgbtnSearchToDate">
                        </ajaxToolkit:CalendarExtender>
                        <%-- <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                    ErrorMessage="Select To Date "></asp:RequiredFieldValidator>--%>
                        <%-- <asp:CompareValidator ID="cmpTask" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                    ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                        <asp:Button ID="btnSearch" OnClick="btnSearch_Click" class="OrbitFilterImage" runat="server"></asp:Button>
                    </div>
                    <%-- </div>--%>
                </section>
            </div>
            <div class="InnerContainer mrgnT20">
                <asp:GridView ID="gvLeaveApplication" runat="server" Width="100%" AllowSorting="True"
                    OnSorting="gvLeaveApplication_Sorting" OnRowUpdating="gvLeaveApplication_RowUpdating"
                    OnRowCommand="gvLeaveApplication_RowCommand" OnRowDataBound="gvLeaveApplication_RowDataBound"
                    OnRowEditing="gvLeaveApplication_RowEditing" AutoGenerateColumns="false" PageSize="5"
                    AllowPaging="true" OnPageIndexChanging="gvLeaveApplication_PageIndexChanging"
                    CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                        LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                        PreviousPageText="Prev" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <RowStyle CssClass="tableRows" />
                    <Columns>
                        <asp:TemplateField HeaderText="LeaveDetailsID" Visible="False">
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lblLeaveDetailID1" Text='<%# Eval("LeaveDetailID") %>'>
                                    </asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblLeaveDetailID" Text='<%# Eval("LeaveDetailID") %>'>
                                    </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <EditItemTemplate>
                                <%--<asp:TextBox ID="txtgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:TextBox>--%>
                                <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Date" SortExpression="LeaveDateFrom">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Text='<%# Bind("LeaveDateFrom") %>' onpaste="return false" autocomplete="off"></asp:TextBox><asp:ImageButton
                                    ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnFromDate"
                                    ImageAlign="Middle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                    TargetControlID="txtgrvFormDate" PopupButtonID="imgbtnFromDate" />
                                <asp:RequiredFieldValidator ID="rfvFromDate" ErrorMessage="Select From date" ControlToValidate="txtgrvFormDate"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvFromDate" runat="server" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date" SortExpression="LeaveDateTo">
                            <EditItemTemplate>
                                <asp:Label ID="lblgrvToDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                <asp:TextBox ID="txtgrvToDate" Width="100px" runat="server" Text='<%# Bind("LeaveDateTo") %>' onpaste="return false" autocomplete="off"></asp:TextBox><asp:ImageButton
                                    ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnToDate"
                                    ImageAlign="Middle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtgrvToDate"
                                    PopupButtonID="imgbtnToDate" />
                                <asp:RequiredFieldValidator ID="rfvTodate" ErrorMessage="Select To date" ControlToValidate="txtgrvToDate"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cmpgvdate" runat="server" ErrorMessage="From date should not be greater than To date"
                                    ControlToValidate="txtgrvToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                    ControlToCompare="txtgrvFormDate"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvToDate" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Leaves" SortExpression="TotalLeaveDays">
                            <EditItemTemplate>
                                <%--<asp:TextBox ID="txtgrvTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:TextBox>--%>
                                <asp:Label ID="lblTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Absent">
                            <EditItemTemplate>
                                <asp:Label ID="lblTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status Id" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                            <EditItemTemplate>
                                <asp:Label ID="lblgrvStatusName" Text='<%# Bind("StatusName") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ApproverID" Visible="False">
                            <EditItemTemplate>
                                <%-- <asp:TextBox ID="txtgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:TextBox>--%>
                                <asp:Label ID="lblApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserID" Visible="False">
                            <EditItemTemplate>
                                <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver Comments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'
                                    TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAppComments" ErrorMessage="Please Enter Approver Comments"
                                    ControlToValidate="txtgrvApproverComments" runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                &nbsp;

                                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="lnkCancel"
                                    CausesValidation="false"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <itemstyle horizontalalign="Center" width="16%"></itemstyle>
                                <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                    CommandArgument='<%#  Eval("LeaveDetailID") %>'>Edit</asp:LinkButton>&nbsp;

                                <asp:LinkButton ID="lnkButCancel" runat="server" CommandName="LeaveCancel" CausesValidation="False"
                                    CommandArgument='<%#  Eval("LeaveDetailID") %>' OnClientClick="return confirm('Are you sure you want to Cancel this Leave?');">Cancel Leave</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
                ShowMessageBox="True"></asp:ValidationSummary>
            <%--  <ajaxToolkit:AnimationExtender ID="OpenAnimation" runat="server" TargetControlID="lnkHolidayList">--%>
            <animations>
                <OnClick>
                    <Sequence>
                        <%-- Disable the button so it can't be clicked again --%>
                        <EnableAction Enabled="false" />

                        <%-- Position the wire frame on top of the button and show it --%>
                        <ScriptAction Script="Cover($get('ctl00_SampleContent_lnkHolidayList'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block" />

                        <%-- Move the wire frame from the button's bounds to the info panel's bounds --%>
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="150" Vertical="-50" />
                            <Resize Width="260" Height="280" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>

                        <%-- Move the info panel on top of the wire frame, fade it in, and hide the frame --%>
                        <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block" />
                        <FadeIn AnimationTarget="info" Duration=".2" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none" />

                        <%-- Flash the text/border red and fade in the "close" button --%>
                        <Parallel AnimationTarget="info" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>
                        <Parallel AnimationTarget="info" Duration=".5">
                            <Color PropertyKey="color" StartValue="#FF0000" EndValue="#666666" />
                            <Color PropertyKey="borderColor" StartValue="#FF0000" EndValue="#666666" />
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />
                        </Parallel>
                    </Sequence>
                </OnClick>
                        </animations>
            </ajaxToolkit:AnimationExtender>

            <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid; display: none; padding-left: 5px; font-size: 12px; z-index: 2; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0); padding-bottom: 5px; border-left: #cccccc 1px solid; width: 250px; padding-top: 5px; border-bottom: #cccccc 1px solid; background-color: #ffffff; opacity: 0"
                id="info">
                <div style="filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0); float: right; opacity: 0"
                    id="btnCloseParent">
                    <asp:LinkButton Style="border-right: #ffffff thin outset; padding-right: 5px; border-top: #ffffff thin outset; padding-left: 5px; font-weight: bold; padding-bottom: 5px; border-left: #ffffff thin outset; color: #ffffff; padding-top: 5px; border-bottom: #ffffff thin outset; background-color: #666666; text-align: center; text-decoration: none"
                        ID="btnClose" runat="server" Text="X"
                        OnClientClick="return false;" ToolTip="Close"></asp:LinkButton>
                </div>
                <div>
                    <table align="left">
                        <tbody>
                            <tr>
                                <td align="left">1. New Year - 1st Jan - Mon
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">2. Ugadi / Gudi Padwa 7-Apr Monday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">3. May Day / Maharastra Day 1-May Thursday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">4. Independence Day 15-Aug Friday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">5. Ganesh Chaturthi 3-Sep Wednesday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">6. Mahatma Gandhi Jayanti - 2nd Oct - Tue
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">7. Dussehra 10-Oct Friday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">8. Diwali (Deepawali) 28-Oct Tuesday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">9. Diwali (Balipratipada) 29-Oct Wednesday
                                        </td>
                            </tr>
                            <tr>
                                <td align="left">10. Christmas 25-Dec Thursday
                                        </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <script language="javascript" type="text/javascript">
                // Move an element directly on top of another element (and optionally
                // make it the same size)
                function Cover(bottom, top, ignoreSize) {
                    var location = Sys.UI.DomElement.getLocation(bottom);
                    top.style.position = 'absolute';
                    top.style.top = location.y + 'px';
                    top.style.left = location.x + 'px';
                    if (!ignoreSize) {
                        top.style.height = bottom.offsetHeight + 'px';
                        top.style.width = bottom.offsetWidth + 'px';
                    }
                }
                    </script>
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </section>
</asp:Content>