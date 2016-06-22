<%@ Page Title="LeaveApplicationForm" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="LeaveApplicationForm.aspx.cs" Inherits="HRMS.Orbitweb.LeaveApplicationForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <!-- SelectBOx -->
    <%--   <script src="../JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>--%>
    <%--  <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
           <link href="../Content/New%20Design/orbit.css" rel="stylesheet" type="text/css" />
    --%>

    <script src="../Scripts/New Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />

    <script type="text/javascript">

        function Validation() {
            //            var txtFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate");
            var txtFromDate = document.getElementById("MainContent_txtFromDate");
            //            var txtToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate");
            var txtToDate = document.getElementById("MainContent_txtToDate");

            //            var txtEmployee = document.getElementById("ctl00_ContentPlaceHolder1_txtempname");
            var txtreason = document.getElementById("MainContent_txtReason");
            //var valueEmployee = ddlEmployee.options[ddlEmployee.selectedIndex].value;

            if (txtFromDate.value == "") {
                alert("Please select From date");
                return false;
            }
            else if (txtToDate.value == "") {
                alert("Please select To date");
                return false;
            }

            else if (Date.parse(txtFromDate.value) > Date.parse(txtToDate.value)) {
                alert("From date should not be greater than To date");
                return false;
            }
            if (txtreason.value == "") {
                alert("Please enter Reason");
                return false;
            }

        }
        //New addition

        $(document).ready(function () {

            //            $('#<%= txtFromDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');
            //            $('#<%= txtToDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');
            //            $('#<%= txtSearchFromDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');
            //            $('#<%= txtSearchToDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("d")%>');

            $('#MainContent_lnkHolidayLists, #tab3').on('click', function () {
                $('#MainContent_gvLeaveApplication').hide();
            });
            $('#tab1', '#tab2').on('click', function () {
                $('#MainContent_gvLeaveApplication').show();
            });

            var totalRows = $("#<%=gvLeaveApplication.ClientID %> tr").length;
            if (totalRows > 5) {
                $('#MainContent_gvLeaveApplication').css('border-bottom', 'none');
            }
            else {
                $('#MainContent_gvLeaveApplication').css('border-bottom', 'solid 35px #C7CED4');
            }

            if ($("[id$=mainTab_Selected]").val() == "CompOff") {
                $('#CompOffDetails').addClass('selected');
                $('#LeaveDetails').removeClass('selected');
            }
            else {
                $('#CompOffDetails').removeClass('selected');
                $('#LeaveDetails').addClass('selected');
            }

            if ($("[id$=selected_tab]").val() == "Search") {
                //debugger;
                $('#tab2').addClass('colored-border');
                $('#tab1').removeClass('colored-border');
                $('#tab3').removeClass('colored-border');
                $('#tab2').removeClass('tabshover');
                $('#tab1').addClass('tabshover');
                $('#tab3').addClass('tabshover');
                $('.search-detailsdata').show();
                $('.add-detailsdata').hide();

                $('.holiday-listdata').hide();
            }
            else if ($("[id$=selected_tab]").val() == "Add" || $("[id$=selected_tab]").val() == "") {
                //debugger;
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
            if ($("[id$=selected_tab]").val() == "") {
                document.getElementById('<%= lnkAddLeavess.ClientID %>').click();
            }

            $('.OrbitFilterLink').click(function () {
                $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
            });
        });

        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });
        });

        //$(function () {
        //    $('select').selectbox();
        //    $('.sbOptions a').hover(function () {
        //        $(this).parent().toggleClass("hoveroption");
        //    });
        //});

        //                    var selected_tab = 1;
        //                    $(function () {

        //                        var tabs = $("#form1").tabs({
        //                            select: function (e, i) {

        //                                selected_tab = i.index;
        //                            }
        //                        });

        //                        var currTab = $("#" + selected_tab.ClientID).val();
        //                        selected_tab = $("[id$=selected_tab]").val() != "" ? parseInt($("[id$=selected_tab]").val()) : 0;
        //                        tabs.tabs({ selected: selected_tab });
        //                        $("form1").submit(function () {

        //                            $("[id$=selected_tab]").val(selected_tab);
        //                        });
        //                    });
    </script>

    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <asp:HiddenField ID="selected_tab" runat="server" />
    <asp:HiddenField ID="mainTab_Selected" runat="server" />
    <asp:HiddenField ID="GridEditModel" runat="server" />
    <section class="LeaveMgmtContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Leave Management</h2>
                <%--                    <div class="EmpSearch clearfix">
                        <a href="#"></a>
                        <input type="text" placeholder="Employee Search">
                    </div>--%>
            </div>
            <nav class="sub-menu-colored">
                <a href="LeaveApplicationForm.aspx" class="selected" id="LeaveDetails">Leave Application</a>
                <a href="CompensationApplicationForm.aspx" id="CompOffDetails">Compensatory Leave Application</a>
            </nav>
        </div>
        <div width="35%">
        </div>
        <div>
            <asp:Label ID="lblHidden" runat="server" Visible="false"></asp:Label>
        </div>
        <div id="LeaveApplicationMainBodyID" runat="server" class="MainBody LeaveApplicationBodyClass">
            <%--<asp:HiddenField--%>
            <div class="clearfix">
                <div class="SuccessMsgOrbit" align="center">
                    <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                </div>
                <div class="ErrorMsgOrbit" align="center">
                    <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
                </div>
                <%--  <h3 class="clearfix">
                    Leave Application</h3>--%>
                <div class="leave-note">
                    Available leaves :

                    <asp:Label ID="lblAvailableLeaves" runat="server"></asp:Label>
                    <div class="SpanNote">
                        *Only pending entries will be editable.
                    </div>
                </div>
                <div style="display: none">
                    <font size="1">
                             <asp:LinkButton ID="lnkAddLeaves" OnClick="lnkAddLeaves_Click" runat="server" Text="Add Details  |"
                                            CausesValidation="false"></asp:LinkButton>
                                       <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server" Text="Search Details  |"
                                            CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkLeavePolicy" runat="server" Text="Leave Policy" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkHolidayList" runat="server" Text="Holiday List" CausesValidation="false"
                                            OnClientClick="return false;"></asp:LinkButton>
                                        <!-- Info panel to be displayed as a flyout when the button is clicked -->
                                        <div style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid; display: none;
                                            z-index: 2; overflow: hidden; border-left: #d0d0d0 1px solid; border-bottom: #d0d0d0 1px solid;
                                            background-color: #ffffff" id="flyout" visible="true">
                                        </div>

                                        <ajaxToolkit:AnimationExtender ID="CloseAnimation" runat="server" TargetControlID="btnClose">
                                            <Animations>
                <OnClick>
                    <Sequence AnimationTarget="info">

                        <StyleAction Attribute="overflow" Value="hidden" />
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>

                        <StyleAction Attribute="display" Value="none" />
                        <StyleAction Attribute="width" Value="250px" />
                        <StyleAction Attribute="height" Value="" />
                        <StyleAction Attribute="fontSize" Value="12px" />
                        <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />

                        <EnableAction AnimationTarget="lnkHolidayList" Enabled="true" />
                    </Sequence>
                </OnClick>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FFFFFF" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                </OnMouseOut>
                                            </Animations>
                                        </ajaxToolkit:AnimationExtender>
                                    </font>
                </div>
            </div>
            <div class="WrapTabs floatL">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">
                            <asp:LinkButton ID="lnkAddLeavess" OnClick="lnkAddLeaves_Click" runat="server" Text="Apply For Leave"
                                CausesValidation="false" OnClientClick="return false;"></asp:LinkButton></li>

                        <li id="tab2">
                            <asp:LinkButton ID="lnkSearchs" OnClick="lnkSearch_Click" runat="server" Text="Search Details"
                                CausesValidation="false"></asp:LinkButton></li>

                        <li id="tab3">
                            <asp:LinkButton ID="lnkHolidayLists" runat="server" Text="Holiday List" CausesValidation="false" OnClientClick="return false;"></asp:LinkButton></li>
                    </ul>
                    <%--<form id="form1" runat="server">
                    <table width="80%" align="center">
                        <tr>
                            <td>
                                <asp:Button Text="Tab 1" BorderStyle="None" ID="Button1" CssClass="Initial" runat="server"
                                    OnClick="lnkAddLeaves_Click" />
                                <asp:Button Text="Tab 2" BorderStyle="None" ID="Button2" CssClass="Initial" runat="server"
                                    OnClick="lnkSearch_Click" />
                                <asp:Button Text="Tab 3" BorderStyle="None" ID="Button3" CssClass="Initial" runat="server"
                                    OnClick="return false;" />
                                <asp:MultiView ID="MainView" runat="server">
                                    <asp:View ID="View1" runat="server">
                                        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                            <tr>
                                                <td>
                                                    <h3>
                                                        <span>View 1 </span>
                                                    </h3>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="View2" runat="server">
                                        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                            <tr>
                                                <td>
                                                    <h3>
                                                        View 2
                                                    </h3>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="View3" runat="server">
                                        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                            <tr>
                                                <td>
                                                    <h3>
                                                        View 3
                                                    </h3>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                        </tr>
                    </table>
                    </form>--%>
                </div>
                <section class="add-detailsdata" id="addDetails">
                    <asp:Panel ID="PanelAddDetails" runat="server">
                        <div class="fill-dtls">
                            <label for="From Date">
                                From Date:</label>
                            <asp:TextBox ID="txtFromDate" runat="server" ReadOnly="false" placeholder="From Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnFromDate" runat="server" CausesValidation="false" ImageUrl="~/images/New Design/calender-icon.png"
                                ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                TargetControlID="txtFromDate" PopupButtonID="imgbtnFromDate">
                            </ajaxToolkit:CalendarExtender>
                            <%--<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="Select From Date "
                            ControlToValidate="txtFromDate" Display="None" ></asp:RequiredFieldValidator>--%>
                            <label for="To Date">
                                To Date:</label>
                            <asp:TextBox ID="txtToDate" TabIndex="1" runat="server" ReadOnly="false" placeholder="To Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnToDate" TabIndex="1" runat="server" CausesValidation="false"
                                ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="imgbtnToDate">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ErrorMessage="Select To Date "
                            ControlToValidate="txtToDate" Display="None" ></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpDates" runat="server" ErrorMessage="From date should not be greater than To date"
                            ControlToValidate="txtToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                            ControlToCompare="txtFromDate" ></asp:CompareValidator>--%>
                        </div>
                        <div class="fill-dtls1">
                            <label for="Reason" class="lb-reason">
                                Reason:</label>
                            <asp:TextBox ID="txtReason" TabIndex="2" runat="server" TextMode="MultiLine" CssClass="reason"
                                placeholder="Reason"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="rfvResason" runat="server" ErrorMessage="Enter Reason "
                            ControlToValidate="txtReason" Display="None" ></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="fill-dtls2">
                            <%--                        <button type="button" class="ButtonGray">
                            Submit</button>
                        <button type="button" class="ButtonGray">
                            Reset</button>--%>
                            <asp:Button ID="btnSubmit" TabIndex="3" OnClick="btnSubmit_Click" runat="server" CausesValidation="true" OnClientClick="return Validation();"
                                Text="Submit" CssClass="ButtonGray"></asp:Button>&nbsp;

                            <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset" CausesValidation="false"
                                CssClass="ButtonGray"></asp:Button>
                        </div>
                    </asp:Panel>
                </section>
                <section class="search-detailsdata" id="searchDetails">
                    <asp:Panel ID="PanelSearchDetails" runat="server">
                        <div class="fill-dtls clearfix">
                            <label for="From Date" class="select-type">
                                Select Type:</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                AutoPostBack="false">
                                <asp:ListItem Value="0" Text="All"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="remain">
                                <label for="From Date">
                                    From Date:</label>
                                <asp:TextBox ID="txtSearchFromDate" runat="server" ReadOnly="false" Visible="true"
                                    placeholder="From Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdnFromdate" runat="server" />
                                <asp:HiddenField ID="hdnTodate" runat="server" />
                                <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" CausesValidation="false"
                                    ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="Middle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                    TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" ErrorMessage="Select From Date "
                                    ControlToValidate="txtSearchFromDate" Display="None"></asp:RequiredFieldValidator>
                                <label for="To Date">
                                    To Date:</label>
                                <asp:TextBox ID="txtSearchToDate" runat="server" ReadOnly="false" placeholder="To Date" onpaste="return false" autocomplete="off"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnSearchToDate" runat="server" CausesValidation="false"
                                    ImageUrl="~/images/New Design/calender-icon.png" ImageAlign="Middle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                    PopupButtonID="imgbtnSearchToDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" ErrorMessage="Select To Date "
                                    ControlToValidate="txtSearchToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cmpTask" runat="server" ErrorMessage="From date should not be greater than To date"
                                    ControlToValidate="txtSearchToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                    ControlToCompare="txtSearchFromDate"></asp:CompareValidator>
                                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search"
                                    CssClass="ButtonGray"></asp:Button>&nbsp;

                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Reset"
                                    CausesValidation="false" CssClass="ButtonGray" Style="display: none;"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                </section>
                <section class="holiday-listdata">
                    <%--<div class="fill-dtls clearfix">
                    <p>
                        New Year---------------------------<span>Jan 01,2014</span></p>
                    <p>
                        Holi----------------------------------<span>Mar 17, 2014</span></p>
                    <p>
                        Gudi Padwa-----------------------<span>Mar 31, 2014</span></p>
                    <p>
                        Labour Day------------------------<span>May 01, 2013</span></p>
                    <p>
                        Independence Day---------------<span>Aug 15,2014</span></p>
                    <p>
                        GaneshChaturthi-----------------<span>Aug 29,2014</span></p>
                    <p>
                        Gandhi Jayanti--------------------<span>Oct 02,2014</span></p>
                    <p>
                        Dassehara-------------------------<span>Oct 03,2014</span></p>
                    <p>
                        Diwali-------------------------------<span>Oct 23,2014</span></p>
                    <p>
                        Cristmas---------------------------<span>Dec 25,2014</span></p>
                </div>--%>
                    <asp:Table ID="HolidayTable" runat="server" class="textGray:active">
                    </asp:Table>
                </section>
            </div>
            <div>
                <asp:GridView ID="gvLeaveApplication" runat="server" OnPageIndexChanging="gvLeaveApplication_PageIndexChanging"
                    AllowPaging="true" PageSize="5" AutoGenerateColumns="false" OnRowEditing="gvLeaveApplication_RowEditing"
                    OnRowCommand="gvLeaveApplication_RowCommand" OnRowUpdating="gvLeaveApplication_RowUpdating" OnRowDataBound="gvLeaveApplication_RowDataBound" OnSorting="gvLeaveApplication_Sorting"
                    AllowSorting="True" OnRowCancelingEdit="gvLeaveApplication_RowCancelingEdit"
                    CssClass="TableJqgrid clearB">
                    <HeaderStyle CssClass="tableHeaders" />
                    <RowStyle CssClass="tableRows" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <Columns>
                        <asp:TemplateField HeaderText="LeaveDetailsID" Visible="False">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblLeaveDetailID" Text='<%# Eval("LeaveDetailID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lblLeaveDetailID1" Text='<%# Eval("LeaveDetailID") %>'>
                                </asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Date" SortExpression="LeaveDateFrom">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvFromDate" runat="server" Text='<%# Bind("LeaveDateFrom") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Height="28px" Text='<%# Bind("LeaveDateFrom") %>'></asp:TextBox><asp:ImageButton
                                    ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"
                                    runat="server" ID="imgbtnFromDate" ImageAlign="Middle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtgrvFormDate"
                                    PopupButtonID="imgbtnFromDate" />
                                <asp:RequiredFieldValidator ID="rfvFromDate" ErrorMessage="Select From date" ControlToValidate="txtgrvFormDate"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemStyle Width="220px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date" SortExpression="LeaveDateTo">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvToDate" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblgrvToDate" runat="server" Visible="false" Text='<%# Bind("LeaveDateTo") %>'></asp:Label>
                                <asp:TextBox ID="txtgrvToDate" Width="100px" Height="28px" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:TextBox><asp:ImageButton
                                    ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"
                                    runat="server" ID="imgbtnToDate" ImageAlign="Middle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtgrvToDate"
                                    PopupButtonID="imgbtnToDate" />
                                <asp:RequiredFieldValidator ID="rfvTodate" ErrorMessage="Select To date" ControlToValidate="txtgrvToDate"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cmpgvdate" runat="server" ErrorMessage="From date should not be greater than To date"
                                    ControlToValidate="txtgrvToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                    ControlToCompare="txtgrvFormDate"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemStyle Width="220px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Leaves" SortExpression="TotalLeaveDays">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTotalLeaves" runat="server" Text='<%# Bind("TotalLeaveDays") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Absent">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblTotalAbsentLeaves" runat="server" Text='<%# Bind("LeaveCorrectionDays") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'
                                    TextMode="MultiLine" Width="170px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvreason" ErrorMessage="Please Enter Reason" ControlToValidate="txtgrvLeaveReason"
                                    runat="server" Display="None"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status Id" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>' CommandArgument='<%#  Eval("StatusID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvStatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblgrvStatusName" Text='<%# Bind("StatusName") %>' runat="server"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ApproverID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblApproverID" runat="server" Text='<%# Bind("ApproverID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblgrvApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblApproverComments" runat="server" Text='<%# Bind("ApproverComments") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False">
                            <ItemStyle HorizontalAlign="Center" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                    CommandArgument='<%#  Eval("LeaveDetailID") %>'>Edit</asp:LinkButton>&nbsp;

                                <asp:LinkButton ID="lnkButCancel" runat="server" CommandName="LeaveCancel" CausesValidation="False"
                                    CommandArgument='<%#  Eval("LeaveDetailID") %>' OnClientClick="return confirm('Are you sure you want to Cancel this Leave?');">Cancel&nbsp;Leave</asp:LinkButton>
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False"></asp:ValidationSummary>
            <ajaxToolkit:AnimationExtender ID="OpenAnimation" runat="server" TargetControlID="lnkHolidayList">
                <Animations>
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
                </Animations>
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
                    <%--                    <asp:Table ID="HolidayTable" runat="server" class="textGray:active">
                    </asp:Table>--%>
                </div>
            </div>
        </div>
    </section>
    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0" style="display: none">
        <tbody>
            <tr>
                <td class="tableHeadBlueLight" align="center">
                    <span id="spanAddLeave" runat="server">Leave Application Form</span> <span id="spanSearch"
                        runat="server">Leave Application Form</span> <span id="spanEdit" runat="server">Leave
                            Application Form</span>
                </td>
            </tr>
            <tr>
                <td class="lineDotted" valign="baseline"></td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td width="35%"></td>
                <td align="left" width="15%">
                    <%--<asp:Label ID="lblHidden" runat="server" Visible="false"></asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tbody>
                            <tr>
                                <td class="tableHeadBlueLight" align="left">
                                    <%--                                    <font size="1">Available leaves :
                                        <asp:Label ID="lblAvailableLeaves" runat="server"></asp:Label>
                                    </font>--%>
                                </td>
                                <td class="tableHeadBlueLight" align="right">
                                    <%--                                    <font size="1">
                                        <asp:LinkButton ID="lnkAddLeaves" OnClick="lnkAddLeaves_Click" runat="server" Text="Add Details  |"
                                            CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server" Text="Search Details  |"
                                            CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkLeavePolicy" runat="server" Text="Leave Policy" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkHolidayList" runat="server" Text="Holiday List" CausesValidation="false"
                                            OnClientClick="return false;"></asp:LinkButton>
                                        <!-- Info panel to be displayed as a flyout when the button is clicked -->
                                        <div style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid; display: none;
                                            z-index: 2; overflow: hidden; border-left: #d0d0d0 1px solid; border-bottom: #d0d0d0 1px solid;
                                            background-color: #ffffff" id="flyout" visible="true">
                                        </div>

                                        <ajaxToolkit:AnimationExtender ID="CloseAnimation" runat="server" TargetControlID="btnClose">
                                            <Animations>
                <OnClick>
                    <Sequence AnimationTarget="info">

                        <StyleAction Attribute="overflow" Value="hidden" />
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>

                        <StyleAction Attribute="display" Value="none" />
                        <StyleAction Attribute="width" Value="250px" />
                        <StyleAction Attribute="height" Value="" />
                        <StyleAction Attribute="fontSize" Value="12px" />
                        <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />

                        <EnableAction AnimationTarget="lnkHolidayList" Enabled="true" />
                    </Sequence>
                </OnClick>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FFFFFF" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                </OnMouseOut>
                                            </Animations>
                                        </ajaxToolkit:AnimationExtender>
                                    </font>--%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="textNote" valign="top">* Only pending entries will be editable.
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
                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td width="2%" align="center">
                                    <asp:Label ID="lblFromDatedot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" ReadOnly="false"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnFromDate" runat="server" CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png"
                                        ImageAlign="AbsMiddle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                        TargetControlID="txtFromDate" PopupButtonID="imgbtnFromDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="Select From Date "
                                        ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td width="2%" align="center">
                                    <asp:Label ID="lblToDatDOt" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                    <asp:TextBox ID="txtToDate" TabIndex="1" runat="server" Width="150px" ReadOnly="false"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnToDate" TabIndex="1" runat="server" CausesValidation="false"
                                        ImageUrl="~/images/Calendar_scheduleHS.png" ImageAlign="AbsMiddle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderToDate" runat="server" TargetControlID="txtToDate"
                                        PopupButtonID="imgbtnToDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ErrorMessage="Select To Date "
                                        ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpDates" runat="server" ErrorMessage="From date should not be greater than To date"
                                        ControlToValidate="txtToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                        ControlToCompare="txtFromDate"></asp:CompareValidator>--%>
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
                                    <%--                                    <asp:TextBox ID="txtReason" TabIndex="2" runat="server" Width="170px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvResason" runat="server" ErrorMessage="Enter Reason "
                                        ControlToValidate="txtReason" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <%--                                    <asp:Button ID="btnSubmit" TabIndex="3" OnClick="btnSubmit_Click" runat="server"
                                        Text="Submit"></asp:Button>&nbsp;
                                    <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset" CausesValidation="false">
                                    </asp:Button>--%>
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
                                        <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%--                                    <asp:TextBox ID="txtSearchFromDate" TabIndex="1" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSearchFromDate" runat="server" CausesValidation="false"
                                        ImageUrl="~/images/Calendar_scheduleHS.png" ImageAlign="Middle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                        TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvSearchFromDate" runat="server" ErrorMessage="Select From Date "
                                        ControlToValidate="txtSearchFromDate" Display="None"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td align="right" width="10%">
                                    <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                                </td>
                                <td align="left" width="15%">
                                    <%--                                    <asp:TextBox ID="txtSearchToDate" TabIndex="2" runat="server" Width="100px" ReadOnly="false"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSearchToDate" runat="server" CausesValidation="false"
                                        ImageUrl="~/images/Calendar_scheduleHS.png" ImageAlign="Middle"></asp:ImageButton>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                        PopupButtonID="imgbtnSearchToDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvsearchToDate" runat="server" ErrorMessage="Select To Date "
                                        ControlToValidate="txtSearchToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpTask" runat="server" ErrorMessage="From date should not be greater than To date"
                                        ControlToValidate="txtSearchToDate" Display="None" Type="Date" Operator="GreaterThanEqual"
                                        ControlToCompare="txtSearchFromDate"></asp:CompareValidator>--%>
                                </td>
                                <td align="center" width="10%">
                                    <%--                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search">
                                    </asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Reset"
                                        CausesValidation="false"></asp:Button>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px" class="h10" colspan="8"></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr style="height: 40px;">
            </tr>
            <tr>
                <td align="center" width="100%">
                    <%-- <asp:GridView ID="gvLeaveApplication" runat="server" Width="100%" OnPageIndexChanging="gvLeaveApplication_PageIndexChanging"
                        AllowPaging="true" PageSize="5" AutoGenerateColumns="false" OnRowEditing="gvLeaveApplication_RowEditing"
                        OnRowDataBound="gvLeaveApplication_RowDataBound" OnRowCommand="gvLeaveApplication_RowCommand"
                        OnRowUpdating="gvLeaveApplication_RowUpdating" OnSorting="gvLeaveApplication_Sorting"
                        AllowSorting="True" OnRowCancelingEdit="gvLeaveApplication_RowCancelingEdit"
                        CssClass="grid">
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
                            <asp:TemplateField HeaderText="From Date" SortExpression="LeaveDateFrom">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvFormDate" runat="server" Width="100px" Text='<%# Bind("LeaveDateFrom") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnFromDate"
                                        ImageAlign="Middle" CausesValidation="false" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtgrvFormDate"
                                        PopupButtonID="imgbtnFromDate" />
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
                                    <asp:TextBox ID="txtgrvToDate" Width="100px" runat="server" Text='<%# Bind("LeaveDateTo") %>'></asp:TextBox><asp:ImageButton
                                        ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnToDate"
                                        ImageAlign="Middle" CausesValidation="false" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtgrvToDate"
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
                            <asp:TemplateField HeaderText="Reason">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'
                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvreason" ErrorMessage="Please Enter Reason" ControlToValidate="txtgrvLeaveReason"
                                        runat="server" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvLeaveReason" runat="server" Text='<%# Bind("LeaveReason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblgrvStatusID" runat="server" Text='<%# Bind("StatusID") %>' CommandArgument='<%#  Eval("StatusID") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Approver">
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
                                <ItemStyle HorizontalAlign="Center" Width="14%"></ItemStyle>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="lnkCancel"
                                        CausesValidation="false"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprove" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkbutEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                        CommandArgument='<%#  Eval("LeaveDetailID") %>'>Edit</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkButCancel" runat="server" CommandName="LeaveCancel" CausesValidation="False"
                                        CommandArgument='<%#  Eval("LeaveDetailID") %>' OnClientClick="return confirm('Are you sure you want to Cancel this Leave?');">Cancel&nbsp;Leave</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>
                </td>
            </tr>
        </tbody>
    </table>
    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False"></asp:ValidationSummary>
    <ajaxToolkit:AnimationExtender ID="OpenAnimation" runat="server" TargetControlID="lnkHolidayList">
        <Animations>
                <OnClick>
                    <Sequence>

                        <EnableAction Enabled="false" />

                        <ScriptAction Script="Cover($get('ctl00_SampleContent_lnkHolidayList'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block" />

                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="150" Vertical="-50" />
                            <Resize Width="260" Height="280" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>

                        <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block" />
                        <FadeIn AnimationTarget="info" Duration=".2" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none" />

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
        </Animations>
    </ajaxToolkit:AnimationExtender>
    <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
        display: none; padding-left: 5px; font-size: 12px; z-index: 2; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);
        padding-bottom: 5px; border-left: #cccccc 1px solid; width: 250px; padding-top: 5px;
        border-bottom: #cccccc 1px solid; background-color: #ffffff; opacity: 0" id="info">
        <div style="filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0); float: right;
            opacity: 0" id="btnCloseParent">
            <asp:LinkButton Style="border-right: #ffffff thin outset; padding-right: 5px; border-top: #ffffff thin outset;
                padding-left: 5px; font-weight: bold; padding-bottom: 5px; border-left: #ffffff thin outset;
                color: #ffffff; padding-top: 5px; border-bottom: #ffffff thin outset; background-color: #666666;
                text-align: center; text-decoration: none" ID="btnClose" runat="server" Text="X"
                OnClientClick="return false;" ToolTip="Close"></asp:LinkButton>
        </div>
        <div>
            <asp:Table ID="HolidayTable" runat="server" class="textGray:active">
            </asp:Table>
        </div>
    </div>--%>
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
    <%-- </contenttemplate>
</asp:UpdatePanel>--%>
</asp:Content>