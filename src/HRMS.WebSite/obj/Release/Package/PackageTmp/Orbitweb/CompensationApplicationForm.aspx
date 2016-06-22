<%@ Page Title="CompensationApplicationForm" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="CompensationApplicationForm.aspx.cs" Inherits="HRMS.Orbitweb.CompensationApplicationForm" %>

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

            //            $('#<%= txtAppliedFor.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');
            //            $('#<%= txtSearchFromDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');
            //            $('#<%= txtSearchToDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');

            var totalRows = $("#<%=gvCampensation.ClientID %> tr").length;
            if (totalRows > 5) {
                $('#MainContent_gvCampensation').css('border-bottom', 'none');
            }
            else {
                $('#MainContent_gvCampensation').css('border-bottom', 'solid 35px #C7CED4');
            }

            if ($("[id$=MainTab_Selected]").val() == "CompOff") {
                $('#CompOffDetails').addClass('selected');
                $('#LeaveDetails').removeClass('selected');
            }
            else {
                $('#CompOffDetails').removeClass('selected');
                $('#LeaveDetails').addClass('selected');
            }

            if ($("[id$=selected_tab]").val() == "Search") {
                $('#tab2').addClass('colored-border');
                $('#tab1').removeClass('colored-border');
                $('#tab3').removeClass('colored-border');
                $('#tab2').removeClass('.tabshover');
                $('#tab1').addClass('tabshover');
                $('#tab3').addClass('tabshover');
                $('.add-detailsdata').hide();
                $('.search-detailsdata').show();
                $('.holiday-listdata').hide();
            }
            else if ($("[id$=selected_tab]").val() == "Add" || $("[id$=selected_tab]").val() == "") {
                $('#tab1').addClass('colored-border');
                $('#tab2').removeClass('colored-border');
                $('#tab3').removeClass('colored-border');
                $('#tab1').removeClass('tabshover');
                $('#tab2').addClass('tabshover');
                $('#tab3').addClass('tabshover');
                $('.add-detailsdata').show();
                $('.search-detailsdata').hide();
                $('.holiday-listdata').hide();
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
    <%--  <asp:ScriptManager runat="server" ID="ScriptManager2">
    </asp:ScriptManager>--%>
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <asp:HiddenField ID="selected_tab" runat="server" />
    <asp:HiddenField ID="MainTab_Selected" runat="server" />
    <asp:HiddenField ID="GridEditModel" runat="server" />
    <section class="LeaveMgmtContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Leave Management</h2>
                <%--                <div class="EmpSearch clearfix">
                    <a href="#"></a>
                    <input type="text" placeholder="Employee Search">
                </div>--%>
            </div>
            <nav class="sub-menu-colored">
                <a href="LeaveApplicationForm.aspx" class="selected" id="LeaveDetails">Leave Application</a>
                <a href="CompensationApplicationForm.aspx" id="CompOffDetails">Compensatory Leave Application</a>
            </nav>
        </div>
        <div class="MainBody CompOff">
            <div class="clearfix">
                <div class="SuccessMsgOrbit" align="center">
                    <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                </div>
                <div class="ErrorMsgOrbit" align="center">
                    <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblHidden" runat="server" Visible="false"></asp:Label>
                </div>
                <%-- <h3 class="clearfix">
                    Compensatory Leave Application</h3>--%>
                <!-- <p class="leave-note">Available Leaves 21.0 <span>*Only pending entries will be editable.</span></p> -->
            </div>
            <div class="TabsContainer">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">
                            <asp:LinkButton ID="lnkAddLeave" OnClick="lnkAddLeaves_Click" runat="server" CausesValidation="false"
                                Text="Apply For Leave"></asp:LinkButton></li>
                        <li id="tab2">
                            <asp:LinkButton ID="lnkSearchs" OnClick="lnkSearch_Click" runat="server" CausesValidation="false"
                                Text="Search Details"></asp:LinkButton></li>
                    </ul>
                </div>
                <section class="add-detailsdata clearfix">
                    <asp:Panel ID="AddCompOffDetails" runat="server">
                        <div class="leftcol">
                            <div class="formrow clearfix">
                                <div class="LabelDiv">
                                    <label for="From Date">
                                        Applied For:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtAppliedFor" runat="server" ReadOnly="false" placeholder="AppliedFor" onpaste="return false" autocomplete="off">
                                </asp:TextBox>
                                    <asp:ImageButton ID="imgbtnFromDate" runat="server" CausesValidation="false" ImageUrl="~/images/New Design/calender-icon.png"
                                        ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgbtnFromDate"
                                        TargetControlID="txtAppliedFor">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="None"
                                        ControlToValidate="txtAppliedFor" ErrorMessage="Select Date ">
                                </asp:RequiredFieldValidator>
                                    <%--<img src="images/calender-icon.png" class="datepicker-image mrgnR12">--%>
                                    <!-- <label for ="To Date">To Date:</label>
								<input type="text">
								<img src="images/calender-icon.png"  class="datepicker-image"> -->
                                </div>
                            </div>
                            <div class="formrow clearfix">
                                <div class="LabelDiv">
                                    <label for="Reason" class="lb-reason">
                                        Reason:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" placeholder="Reason"
                                        CssClass="TextareaBig">
                                </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvResason" runat="server" Display="None" ControlToValidate="txtReason"
                                        ErrorMessage="Enter Reason ">
                                </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="fill-dtls2">
                                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"
                                    CssClass="ButtonGray"></asp:Button>
                                &nbsp;

                                <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" CausesValidation="false"
                                    Text="Reset" CssClass="ButtonGray"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                </section>
                <section class="search-detailsdata">
                    <asp:Panel ID="SearchCompOff" runat="server">
                        <div class="fill-dtls clearfix">
                            <label for="From Date" class="select-type">
                                Select Type:</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <div class="remain">
                                <label for="From Date">
                                    From Date:</label>
                                <asp:TextBox ID="txtSearchFromDate" runat="server" ReadOnly="false" placeholder="From Date" onpaste="return false" autocomplete="off">
                            </asp:TextBox>
                                <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" CausesValidation="false"
                                    ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                    PopupButtonID="imgbtnSearchFromDate" TargetControlID="txtSearchFromDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" Display="None"
                                    ControlToValidate="txtSearchFromDate" ErrorMessage="Select From Date ">
                            </asp:RequiredFieldValidator>
                                <%--<img src="images/calender-icon.png" class="datepicker-image mrgnR12">--%>
                                <label for="To Date">
                                    To Date:</label>
                                <asp:TextBox ID="txtSearchToDate" runat="server" ReadOnly="false" placeholder="Date Date" onpaste="return false" autocomplete="off">
                            </asp:TextBox>
                                <asp:ImageButton ID="imgbtnSearchToDate" runat="server" CausesValidation="false"
                                    ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" PopupButtonID="imgbtnSearchToDate"
                                    TargetControlID="txtSearchToDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                    ErrorMessage="Select To Date ">
                            </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cmpTask" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                    ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                                    Operator="GreaterThanEqual" Type="Date">
                            </asp:CompareValidator>
                                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search"
                                    CssClass="ButtonGray"></asp:Button>
                                &nbsp;

                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" CausesValidation="false"
                                    Text="Reset" CssClass="ButtonGray" Style="display: none;"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                </section>
            </div>
            <div>
                <asp:GridView ID="gvCampensation" runat="server" OnSorting="gvCampensation_Sorting"
                    AllowSorting="true" OnRowCancelingEdit="gvCampensation_RowCancelingEdit" OnRowDataBound="gvCampensation_RowDataBound"
                    OnRowUpdating="gvCampensation_RowUpdating" OnRowEditing="gvCampensation_RowEditing"
                    OnRowCommand="gvCampensation_RowCommand" AutoGenerateColumns="false" PageSize="5"
                    AllowPaging="true" OnPageIndexChanging="gvCampensation_PageIndexChanging" CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <RowStyle CssClass="tableRows" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <Columns>
                        <asp:TemplateField Visible="False" HeaderText="LeaveDetailsID">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompensationID" Text='<%# Eval("CompensationID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lblEditCompensationID" Text='<%# Eval("CompensationID") %>'>
                                </asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Applied For" SortExpression="AppliedFor">
                            <EditItemTemplate>
                                <asp:Label ID="lblAppliedFor" runat="server" Visible="false" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                                <asp:TextBox ID="txtEditAppliedFor" runat="server" Width="100px" Text='<%# Bind("AppliedFor") %>' onpaste="return false" autocomplete="off"></asp:TextBox><asp:ImageButton
                                    ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnFromDate"
                                    ImageAlign="Middle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                    TargetControlID="txtEditAppliedFor" PopupButtonID="imgbtnFromDate" />
                                <asp:RequiredFieldValidator ID="rfvFromDate" ErrorMessage="Select From date" ControlToValidate="txtEditAppliedFor"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAppliedFor" runat="server" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgrvLeaveReason" runat="server" Text='<%# Bind("Reason") %>' TextMode="MultiLine"
                                    Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvreason" ErrorMessage="Please Enter Reason" ControlToValidate="txtgrvLeaveReason"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
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
                                <asp:Label ID="lblApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver" SortExpression="EmployeeName">
                            <EditItemTemplate>
                                <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver Comments">
                            <EditItemTemplate>
                                <asp:Label ID="lblApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False">
                            <ItemStyle HorizontalAlign="Center" Width="16%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                    CommandArgument='<%#  Eval("CompensationID") %>'>Edit</asp:LinkButton>&nbsp;

                                <asp:LinkButton ID="lnkButCancel" runat="server" CommandName="CampensationCancel"
                                    CausesValidation="False" CommandArgument='<%#  Eval("CompensationID") %>' OnClientClick="return confirm('Do you want to cancel Comp. Off?');">Cancel&nbsp;CompOff</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                &nbsp;

                                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="lnkCancel"
                                    CausesValidation="false"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0" style="display: none">
        <tbody>
            <tr>
                <td class="tableHeadBlueLight" align="center">
                    <span id="spanAddLeave" runat="server">Compensatory Leave Application Form</span>
                    <span id="spanSearch" runat="server">Compensatory Leave Application Form</span><span
                        id="spanEdit" runat="server">Compensatory Leave Application Form</span>
                </td>
            </tr>
            <tr>
                <td class="lineDotted" valign="baseline"></td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:label id="lblSuccess" runat="server" skinid="lblSuccess"></asp:label>--%>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:label id="lblError" runat="server" skinid="lblError"></asp:label>--%>
                </td>
            </tr>
            <tr>
                <td align="left" width="15%">
                    <%--<asp:label id="lblHidden" runat="server" visible="false"></asp:label>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tbody>
                            <tr id="travailableCompOff" runat="server">
                                <td class="tableHeadBlueLight" align="left">
                                    <%-- <font size="1">Available Compensatory leave balance :
                                        <asp:Label ID="lblAvailableLeaves" runat="server"></asp:Label>
                                    </font>--%>
                                </td>
                                <td class="tableHeadBlueLight" align="right">
                                    <font size="1">
                                        <asp:LinkButton ID="lnkAddLeaves" OnClick="lnkAddLeaves_Click" runat="server" CausesValidation="false"
                                            Text="Add Details"></asp:LinkButton>
                                    </font>
                                </td>
                                <td id="tddiff" runat="server" visible="false" align="right" style="width: 1%">|
                                </td>
                                <td class="tableHeadBlueLight" align="right" style="width: 11%" id="tdSearchLink"
                                    runat="server">
                                    <font size="1">
                                        <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server" CausesValidation="false"
                                            Text="Search Details"></asp:LinkButton></font>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tdAddLeave" class="tableBorder" cellspacing="0" cellpadding="0" width="25%"
                        align="center" border="0" runat="server">
                        <tbody>
                            <tr style="height: 15px;">
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblAppliedFor" runat="server" Text="Applied For"></asp:Label>
                                </td>
                                <td width="2%" align="center">
                                    <asp:Label ID="lblAppliedDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                <asp:textbox id="txtAppliedFor" runat="server" readonly="false" width="150px">
                                    </asp:textbox>
                                <asp:imagebutton id="imgbtnFromDate" runat="server" causesvalidation="false" imagealign="AbsMiddle"
                                    imageurl="~/images/Calendar_scheduleHS.png">
                                    </asp:imagebutton>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                    PopupButtonID="imgbtnFromDate" TargetControlID="txtAppliedFor">
                                </ajaxToolkit:CalendarExtender>
                                <asp:requiredfieldvalidator id="rfvFromDate" runat="server" display="None" controltovalidate="txtAppliedFor"
                                    errormessage="Select Date ">
                                    </asp:requiredfieldvalidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblReason" runat="server" Text="Reason"></asp:Label>
                                </td>
                                <td width="2%" align="center">
                                    <asp:Label ID="lblReasonDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                    <asp:TextBox ID="txtReason" runat="server" Width="170px" TextMode="MultiLine">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvResason" runat="server" Display="None" ControlToValidate="txtReason"
                                        ErrorMessage="Enter Reason ">
                                    </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <%--                                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit">
                                    </asp:Button>
                                    &nbsp;
                                    <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" CausesValidation="false"
                                        Text="Reset"></asp:Button>--%>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tdSearch" class="tableBorder" width="100%" runat="server">
                        <tbody>
                            <tr>
                                <td class="h10" colspan="8"></td>
                            </tr>
                            <tr>
                                <td style="width: 5%"></td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblType" runat="server" Text="Select Type :"></asp:Label>
                                </td>
                                <td align="left" width="20%">
                                    <%--                                    <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                        AutoPostBack="true" Width="150px">
                                    </asp:DropDownList>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%--                                    <asp:TextBox ID="txtSearchFromDate" runat="server" ReadOnly="false" Width="100px">
                                    </asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" CausesValidation="false"
                                        ImageAlign="Middle" ImageUrl="~/images/Calendar_scheduleHS.png"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                        PopupButtonID="imgbtnSearchFromDate" TargetControlID="txtSearchFromDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" Display="None"
                                        ControlToValidate="txtSearchFromDate" ErrorMessage="Select From Date ">
                                    </asp:RequiredFieldValidator>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%--                                    <asp:TextBox ID="txtSearchToDate" runat="server" ReadOnly="false" Width="100px">
                                    </asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSearchToDate" runat="server" CausesValidation="false"
                                        ImageAlign="Middle" ImageUrl="~/images/Calendar_scheduleHS.png"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" PopupButtonID="imgbtnSearchToDate"
                                        TargetControlID="txtSearchToDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                        ErrorMessage="Select To Date ">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpTask" runat="server" Display="None" ControlToValidate="txtSearchToDate"
                                        ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                                        Operator="GreaterThanEqual" Type="Date">
                                    </asp:CompareValidator>--%>
                                </td>
                                <td align="center" width="10%">
                                    <%--                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search">
                                    </asp:Button>
                                    &nbsp;
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" CausesValidation="false"
                                        Text="Reset"></asp:Button>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="h10" colspan="8"></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr style="height: 40px;">
            </tr>
            <tr>
                <td align="center">
                    <%--                    <asp:GridView ID="gvCampensation" runat="server" Width="100%" OnSorting="gvCampensation_Sorting"
                        AllowSorting="true" OnRowCancelingEdit="gvCampensation_RowCancelingEdit" OnRowDataBound="gvCampensation_RowDataBound"
                        OnRowUpdating="gvCampensation_RowUpdating" OnRowEditing="gvCampensation_RowEditing"
                        OnRowCommand="gvCampensation_RowCommand" AutoGenerateColumns="false" PageSize="5"
                        AllowPaging="true" OnPageIndexChanging="gvCampensation_PageIndexChanging" CssClass="grid">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="LeaveDetailsID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompensationID" Text='<%# Eval("CompensationID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblEditCompensationID" Text='<%# Eval("CompensationID") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Applied For" SortExpression="AppliedFor">
                                <EditItemTemplate>
                                    <asp:Label ID="lblAppliedFor" runat="server" Visible="false" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                                    <asp:TextBox ID="txtEditAppliedFor" runat="server" Width="100px" Text='<%# Bind("AppliedFor") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnFromDate"
                                        ImageAlign="Middle" CausesValidation="false" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                        TargetControlID="txtEditAppliedFor" PopupButtonID="imgbtnFromDate" />
                                    <asp:RequiredFieldValidator ID="rfvFromDate" ErrorMessage="Select From date" ControlToValidate="txtEditAppliedFor"
                                        runat="server" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAppliedFor" runat="server" Text='<%# Bind("AppliedFor") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvLeaveReason" runat="server" Text='<%# Bind("Reason") %>' TextMode="MultiLine"
                                        Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvreason" ErrorMessage="Please Enter Reason" ControlToValidate="txtgrvLeaveReason"
                                        runat="server" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
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
                                    <asp:Label ID="lblApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver" SortExpression="EmployeeName">
                                <EditItemTemplate>
                                    <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approver Comments">
                                <EditItemTemplate>
                                    <asp:Label ID="lblApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                <ItemStyle HorizontalAlign="Center" Width="16%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                        CommandArgument='<%#  Eval("CompensationID") %>'>Edit</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkButCancel" runat="server" CommandName="CampensationCancel"
                                        CausesValidation="False" CommandArgument='<%#  Eval("CompensationID") %>' OnClientClick="return confirm('Do you want to cancel Comp. Off?');">Cancel&nbsp;CompOff</asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="lnkCancel"
                                        CausesValidation="false"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
        ShowMessageBox="True"></asp:ValidationSummary>
</asp:Content>