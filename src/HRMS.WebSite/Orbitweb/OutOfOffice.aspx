<%@ Page Title="Out Of Office" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="OutOfOffice.aspx.cs" Inherits="HRMS.Orbitweb.OutOfOffice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <!-- SelectBOx -->
    <script src="../JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var totalRows = $("#<%=grdSignInSignOut.ClientID %> tr").length;
            if (totalRows > 5) {
                $('#MainContent_grdSignInSignOut').css('border-bottom', 'none');
            }
            else {
                $('#MainContent_grdSignInSignOut').css('border-bottom', 'solid 35px #C7CED4');
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

        $(function () {
            $('select').selectbox();
            $('.sbOptions a').hover(function () {
                $(this).parent().toggleClass("hoveroption");
            });
        });

        function UpdateValidation(txtDate1, ddlOutTimeHrs, ddlOutTimeMins, ddlInTimeHrs, ddlInTimeMins, txtComments) {
            var txtDate1 = txtDate1;
            var ddlOutTimeHrs = ddlOutTimeHrs;
            var ddlOutTimeMins = ddlOutTimeMins;
            var ddlInTimeHrs = ddlInTimeHrs;
            var ddlInTimeMins = ddlInTimeMins;
            var txtComments = txtComments;

            if (txtDate1.value == "") {
                alert("Please Select date.")
                return false;
            }

            if (parseInt(ddlOutTimeHrs.value) > parseInt(ddlInTimeHrs.value)) {
                alert("Out Time should be greater than In Time");
                return false;
            }
            else if (parseInt(ddlOutTimeHrs.value) == parseInt(ddlInTimeHrs.value)) {
                if (parseInt(ddlOutTimeMins.value) >= parseInt(ddlInTimeMins.value)) {
                    alert("Please select proper In-Time and Out-Time Minutes ");
                    return false;
                }
            }
            if (txtComments.value.trim() == "") {
                alert("Please enter comments.")
                return false;
            }
            //
        }
        function ButtonValidation() {
            var txtDate = document.getElementById("ctl00_ContentPlaceHolder1_txtDate");
            txtDate = document.getElementById("MainContent_txtDate");

            var txtComments = document.getElementById("ctl00_ContentPlaceHolder1_txtComments");
            txtComments = document.getElementById("MainContent_txtComments");
            var ddlHrsIn = document.getElementById("ctl00_ContentPlaceHolder1_ddlHrsIn");
            ddlHrsIn = document.getElementById("MainContent_ddlHrsOut");

            var ddlMinsIn = document.getElementById("ctl00_ContentPlaceHolder1_ddlMinsIn");
            ddlMinsIn = document.getElementById("MainContent_ddlMinsOut");

            var ddlHrsOut = document.getElementById("ctl00_ContentPlaceHolder1_ddlHrsOut");
            ddlHrsOut = document.getElementById("MainContent_ddlHrsIn");
            var ddlMinsOut = document.getElementById("ctl00_ContentPlaceHolder1_ddlMinsOut");
            ddlMinsOut = document.getElementById("MainContent_ddlMinsIn");

            if (txtDate.value == "") {
                alert("Please date")
                return false;
            }
            if (ddlHrsOut.value == 0 || ddlMinsOut.value == 0 || ddlHrsIn.value == 0 || ddlMinsIn.value == 0) {
                //alert ("Please Select Out Time Hours")
                alert("Please select Out-Time/In-Time hours and minutes")
                return false;
            }
            //        if (ddlMinsOut.value==0)
            //        {
            //            alert ("Please Select Out Time Minutes")
            //            return false;
            //        }
            //
            //        if (ddlHrsIn.value==0)
            //        {
            //            alert ("Please Select In Time Hours")
            //            return false;
            //        }
            //        if (ddlMinsIn.value==0)
            //        {
            //            alert ("Please Select In Time Minutes")
            //            return false;
            //        }

            if (parseInt(ddlHrsOut.value) > parseInt(ddlHrsIn.value)) {
                alert("Out Time should be greater than In Time");
                return false;
            }
            else if (parseInt(ddlHrsOut.value) == parseInt(ddlHrsIn.value)) {
                if (parseInt(ddlMinsOut.value) > parseInt(ddlMinsIn.value)) {
                    alert("Please select proper In-Time and Out-Time Minutes ");
                    return false;
                }
            }
            if (txtComments.value.trim() == "") {
                alert("Please enter the comment");
                return false;
            }

        }
    </script>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:HiddenField ID="selected_tab" runat="server" />
    <section class="LeaveMgmtContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Out Of Office</h2>
                <%--                <div class="EmpSearch clearfix">
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
        <div class="MainBody outofOffice">
            <div class="clearfix">
                <div class="SuccessMsgOrbit" align="center">
                    <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                </div>
                <div class="ErrorMsgOrbit" align="center">
                    <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>
                </div>
            </div>
            <div class="tabs comptabs">
                <ul class="leave-mgmt-tabs">
                    <li id="tab1">
                        <asp:LinkButton ID="lnkAdddetails" OnClick="lnkAdddetails_Click" runat="server" CausesValidation="false">Add Details</asp:LinkButton></li>
                    <li id="tab2">
                        <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server" CausesValidation="false"> Search Details</asp:LinkButton></li>
                </ul>
            </div>
            <section class="add-detailsdata out-of-ofc clearfix">
                <asp:Panel ID="pnlAddDetails" runat="Server">
                    <div class="fill-dtls clearfix">
                        <div class="colOneThird">
                            <label for="Select a Date">
                                Select a Date:</label>
                            <asp:TextBox ID="txtDate" runat="server" Width="143px"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnDate" runat="server" CausesValidation="false" ImageUrl="~/images/New Design/calender-icon.png"
                                ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtDate"
                                PopupButtonID="imgbtnDate" BehaviorID="ce1">
                            </ajaxToolkit:CalendarExtender>
                            <%--<img src="images/calender-icon.png" class="datepicker-image mrgnR12">--%>
                        </div>
                        <div class="colOneThird">
                            <label for="Select a Type">
                                Select a Type:</label>
                            <asp:RadioButtonList ID="rdbType" runat="server" RepeatDirection="Horizontal"
                                Width="165px" CssClass="RadioButtonList">
                            </asp:RadioButtonList>
                            <%--<label for="rdbType" class="LabelForRadio">Personal</label>--%>
                        </div>
                    </div>
                    <div class="fill-dtls0 clearfix">
                        <div class="colOneFouth">
                            <label for="Out Time">
                                Out Time:</label>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlHrsOut" runat="server" Width="82px">
                                    <asp:ListItem Selected="True" Value="0">Hrs</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlMinsOut" runat="server" Width="81px">
                                    <asp:ListItem Selected="True" Value="0">Mins</asp:ListItem>
                                    <asp:ListItem Value="1">00</asp:ListItem>
                                    <asp:ListItem Value="2">10</asp:ListItem>
                                    <asp:ListItem Value="3">20</asp:ListItem>
                                    <asp:ListItem Value="4">30</asp:ListItem>
                                    <asp:ListItem Value="5">40</asp:ListItem>
                                    <asp:ListItem Value="6">50</asp:ListItem>
                                    <asp:ListItem Value="7">59</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="colOneFouth">
                            <label for="In Time" class="Intime">
                                In Time:</label>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlHrsIn" runat="server" Width="82px">
                                    <asp:ListItem Text="Hrs"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlMinsIn" runat="server" Width="81px">
                                    <asp:ListItem Selected="True" Value="0"> Mins</asp:ListItem>
                                    <asp:ListItem Value="1">00</asp:ListItem>
                                    <asp:ListItem Value="2">10</asp:ListItem>
                                    <asp:ListItem Value="3">20</asp:ListItem>
                                    <asp:ListItem Value="4">30</asp:ListItem>
                                    <asp:ListItem Value="5">40</asp:ListItem>
                                    <asp:ListItem Value="6">50</asp:ListItem>
                                    <asp:ListItem Value="7">59</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="fill-dtls1 clearfix">
                        <label for="Reason" class="lb-reason">
                            Comments:</label>
                        <asp:TextBox ID="txtComments" runat="server" Width="165px" Height="55px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="fill-dtls2">
                        <%--                    <button type="button" class="ButtonGray">
                        Submit</button>
                    <button type="button" class="ButtonGray">
                        Reset</button>--%>
                        <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CausesValidation="false"
                            Text="Submit" CssClass="ButtonGray"></asp:Button>
                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="SERVER" CausesValidation="false"
                            Text="Reset" CssClass="ButtonGray"></asp:Button>
                    </div>
                </asp:Panel>
            </section>
            <section class="search-detailsdata">
                <asp:Panel ID="pnlSearch" runat="Server">
                    <div class="fill-dtls clearfix">
                        <label for="From Date" class="select-type">
                            Select Status:</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                        <div class="remain">
                            <label for="From Date">
                                From Date:</label>
                            <asp:TextBox ID="txtSearchFromDate" Width="100px" runat="server" placeholder="From Date"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"
                                runat="server" ID="imgbtnSearchFromDate" ImageAlign="Middle" CausesValidation="false" />
                            <ajaxToolkit:CalendarExtender ID="CEFDate" runat="server" TargetControlID="txtSearchFromDate"
                                PopupButtonID="imgbtnSearchFromDate" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Select From date"
                                ControlToValidate="txtSearchFromDate" runat="server" Display="None" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>
                            <%--<img src="images/calender-icon.png" class="datepicker-image mrgnR12">--%>
                            <label for="To Date">
                                To Date:</label>
                            <asp:TextBox ID="txtSearchToDate" Width="100px" runat="server" placeholder="To Date"></asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"
                                runat="server" ID="imgbtnSearchToDate" ImageAlign="Middle" CausesValidation="false" />
                            <ajaxToolkit:CalendarExtender ID="CEToDate" runat="server" TargetControlID="txtSearchToDate"
                                PopupButtonID="imgbtnSearchToDate" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Select To date"
                                ControlToValidate="txtSearchToDate" runat="server" Display="None" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>
                            <%--<img src="images/calender-icon.png" class="datepicker-image">--%>
                            <%--                        <button type="button" class="ButtonGray">
                            Search</button>
                        <button type="button" class="ButtonGray">
                            Reset</button>--%>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                ValidationGroup="vgSearch" CssClass="ButtonGray" />
                            <asp:CompareValidator ID="CmpDate" runat="server" Visible="True" ControlToValidate="txtSearchToDate"
                                ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                                Operator="GreaterThanEqual" Type="Date" Display="None" ValidationGroup="vgSearch"></asp:CompareValidator>
                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="ButtonGray" />
                        </div>
                    </div>
                </asp:Panel>
            </section>
            <div>
                <asp:GridView ID="grdSignInSignOut" runat="server" OnRowCancelingEdit="grdSignInSignOut_RowCancelingEdit"
                    OnRowUpdating="grdSignInSignOut_RowUpdating" OnSorting="grdSignInSignOut_Sorting"
                    AllowSorting="True" OnRowDataBound="grdSignInSignOut_RowDataBound" OnPageIndexChanging="grdSignInSignOut_PageIndexChanging"
                    OnRowDeleting="grdSignInSignOut_RowDeleting" OnRowEditing="grdSignInSignOut_RowEditing"
                    PageSize="10" AllowPaging="true" AutoGenerateColumns="False" Width="100%" CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <RowStyle CssClass="tableRows" />
                    <Columns>
                        <asp:TemplateField HeaderText="Out Of Office ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblOutOfOFficeId" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutOfOfficeID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblUserid" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"UserId")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="OutTimeDate">
                            <EditItemTemplate>
                                <asp:Label ID="lblOutDate" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeDate") %>'
                                    Width="1px" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtDate1" runat="Server" Text='<%# Bind ("OutTimeDate") %>' Width="65px"></asp:TextBox>&nbsp;<asp:ImageButton
                                    ID="imgbtnSearchToDate" runat="server" ImageUrl="~/images/New Design/calender-icon.png"
                                    CausesValidation="false" ImageAlign="Middle"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" PopupButtonID="imgbtnSearchToDate"
                                    TargetControlID="txtDate1">
                                </ajaxToolkit:CalendarExtender>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOutDate" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OutTime1" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblOuttime1" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeTime")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OutTime" SortExpression="OutTimeTime">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlOutTimeHrs" runat="Server">
                                </asp:DropDownList>
                                &nbsp;

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
                                <asp:Label ID="lblOuttime" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeTime")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InTime1" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblIntime1" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"InTimeTime")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InTime" SortExpression="InTimeTime">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInTimeHrs" runat="Server">
                                </asp:DropDownList>
                                &nbsp;

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
                                <asp:Label ID="lblIntime" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"InTimeTime")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtComments" runat="Server" TextMode="MultiLine" Text='<%# Bind("Comment") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblComment" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Comment")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblresonID" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"TypeId")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason" SortExpression="Reason">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlReason" runat="Server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblreson" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Reason")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Status")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver" SortExpression="Approver">
                            <ItemTemplate>
                                <asp:Label ID="lblApprover" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"ApproverName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approver Comments" SortExpression="ApproverComments">
                            <ItemTemplate>
                                <asp:Label ID="lblApproverComment" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"ApproverComments")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center" />
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="SERVER" Text="Update" CommandName="Update"
                                    CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="Server" Text="Cancel" CommandName="Cancel"
                                    CausesValidation="FALSE"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApproved" runat="Server"></asp:Label>
                                <asp:LinkButton ID="lnkbtnEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="False"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDelete" runat="Server" Text="Delete" CommandName="Delete"
                                    CausesValidation="false" OnClientClick="return confirm('Are you sure ? you want to delete this item');"
                                    Visible="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnCancel" runat="Server" Text="Cancel Out Of Office" CommandName="Delete"
                                    CausesValidation="false" OnClientClick="return confirm('Are you sure ? you want to cancel this item');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
                ShowMessageBox="True"></asp:ValidationSummary>
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="False"
                ShowMessageBox="True" ValidationGroup="vgSearch"></asp:ValidationSummary>
        </div>
    </section>
    <%--<asp:UpdatePanel runat="server" id="upnloutofoffice">--%>
    <%--<contenttemplate>--%>
    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0" style="display: none">
        <tbody>
            <tr>
                <td class="tableHeadBlueLight" align="center">Out Of Office
                </td>
            </tr>
            <tr>
                <td class="lineDotted"></td>
            </tr>
            <tr>
                <td class="h5"></td>
            </tr>
            <tr>
                <td align="center">
                    <%--                    <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
                    <asp:Label ID="lblError" runat="SERVER" SkinID="lblError"> </asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <table align="right">
                        <tbody>
                            <tr>
                                <td id="tdAddDetails" align="right" runat="server">
                                    <%--<asp:LinkButton ID="lnkAdddetails" OnClick="lnkAdddetails_Click" runat="server" CausesValidation="false">Add Details  </asp:LinkButton>--%>
                                </td>
                                <td id="td1" align="right" runat="Server">|
                                </td>
                                <td id="tdSearch" align="right" runat="Server">
                                    <%--<asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server" CausesValidation="false"> Search Details</asp:LinkButton>--%>
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
                    <%--<asp:Panel ID="pnlAddDetails" runat="Server">--%>
                    <table class="tableBorder" cellspacing="0" cellpadding="0" width="25%" align="center"
                        border="0">
                        <tbody>
                            <tr style="height: 15px;">
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblDate" runat="server" Text="Select a date"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                        <asp:TextBox ID="txtDate" runat="server" Width="143px"></asp:TextBox>
                                        <asp:ImageButton ID="imgbtnDate" runat="server" CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png"
                                            ImageAlign="AbsMiddle"></asp:ImageButton>
                                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtDate"
                                            PopupButtonID="imgbtnDate" BehaviorID="ce1">
                                        </ajaxToolkit:CalendarExtender>--%>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblSelectType" runat="server" Text="Select Type"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="lblSelectTypeDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                        <asp:RadioButtonList ID="rdbType" runat="server" RepeatDirection="Horizontal" Width="165px">
                                        </asp:RadioButtonList>--%>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblOutTime" runat="server" Text="Out time"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="lblOutTimeDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                        <asp:DropDownList ID="ddlHrsOut" runat="server" Width="82px">
                                            <asp:ListItem Selected="True" Value="0">Hrs</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMinsOut" runat="server" Width="81px">
                                            <asp:ListItem Selected="True" Value="0">Mins</asp:ListItem>
                                            <asp:ListItem Value="1">00</asp:ListItem>
                                            <asp:ListItem Value="2">10</asp:ListItem>
                                            <asp:ListItem Value="3">20</asp:ListItem>
                                            <asp:ListItem Value="4">30</asp:ListItem>
                                            <asp:ListItem Value="5">40</asp:ListItem>
                                            <asp:ListItem Value="6">50</asp:ListItem>
                                            <asp:ListItem Value="7">59</asp:ListItem>
                                        </asp:DropDownList>--%>
                                    <br />
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblInTime" runat="server" Text="In Time"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="lblInTimeDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                        <asp:DropDownList ID="ddlHrsIn" runat="server" Width="82px">
                                            <asp:ListItem Text="Hrs"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMinsIn" runat="server" Width="81px">
                                            <asp:ListItem Selected="True" Value="0"> Mins</asp:ListItem>
                                            <asp:ListItem Value="1">00</asp:ListItem>
                                            <asp:ListItem Value="2">10</asp:ListItem>
                                            <asp:ListItem Value="3">20</asp:ListItem>
                                            <asp:ListItem Value="4">30</asp:ListItem>
                                            <asp:ListItem Value="5">40</asp:ListItem>
                                            <asp:ListItem Value="6">50</asp:ListItem>
                                            <asp:ListItem Value="7">59</asp:ListItem>
                                        </asp:DropDownList>--%>
                                </td>
                            </tr>
                            <tr style="height: 70px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblComments" runat="server" Text="Comments"></asp:Label>
                                </td>
                                <td width="2%">
                                    <asp:Label ID="lblCommentsDot" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--<asp:TextBox ID="txtComments" runat="server" Width="165px" Height="55px" TextMode="MultiLine"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                            <tr id="tdbutton" runat="server">
                                <%--td width="35%">
                                    </td>--%>
                                <%--<td align="left" style="width: 15%">
                                    </td>--%>
                                <%--<td width="2%">
                                    </td>--%>
                                <td align="center" colspan="4">
                                    <%--                                        <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CausesValidation="false"
                                            Text="Submit"></asp:Button>
                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="SERVER" CausesValidation="false"
                                            Text="Reset"></asp:Button>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                        </tbody>
                    </table>
                    <%--</asp:Panel>--%>
                </td>
            </tr>
            <tr>
                <td class="h10"></td>
            </tr>
            <tr>
                <td>
                    <%--<asp:Panel ID="pnlSearch" runat="Server">--%>
                    <table width="100%" class="tableBorder">
                        <tr>
                            <td colspan="8" class="h10" style="height: 10px" width="100"></td>
                        </tr>
                        <tr>
                            <td style="width: 5%"></td>
                            <td width="10%" align="right">
                                <asp:Label ID="lblType" runat="server" Text="Select Status :"></asp:Label>
                            </td>
                            <td align="left" width="20%">
                                <%--                                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                            </td>
                            <td align="right" width="10%">
                                <asp:Label ID="lblSearchFromDate" runat="server" Text="From Date : "></asp:Label>
                            </td>
                            <td align="left" width="15%">
                                <%--                                    <asp:TextBox ID="txtSearchFromDate" Width="100px" runat="server"></asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchFromDate"
                                        ImageAlign="Middle" CausesValidation="false" />
                                    <ajaxToolkit:CalendarExtender ID="CEFDate" runat="server" TargetControlID="txtSearchFromDate"
                                        PopupButtonID="imgbtnSearchFromDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ErrorMessage="Select From date"
                                        ControlToValidate="txtSearchFromDate" runat="server" Display="None" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td align="right" width="10%">
                                <asp:Label ID="lblSearchToDate" runat="server" Text="To Date : "></asp:Label>
                            </td>
                            <td align="left" width="15%">
                                <%--                                 <asp:TextBox ID="txtSearchToDate" Width="100px" runat="server"></asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/Calendar_scheduleHS.png" runat="server" ID="imgbtnSearchToDate"
                                        ImageAlign="Middle" CausesValidation="false" />
                                    <ajaxToolkit:CalendarExtender ID="CEToDate" runat="server" TargetControlID="txtSearchToDate"
                                        PopupButtonID="imgbtnSearchToDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ErrorMessage="Select To date"
                                        ControlToValidate="txtSearchToDate" runat="server" Display="None" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td align="center" width="10%">
                                <%--                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                        ValidationGroup="vgSearch" />
                                    <asp:CompareValidator ID="CmpDate" runat="server" Visible="True" ControlToValidate="txtSearchToDate"
                                        ErrorMessage="From date should not be greater than To date" ControlToCompare="txtSearchFromDate"
                                        Operator="GreaterThanEqual" Type="Date" Display="None" ValidationGroup="vgSearch"></asp:CompareValidator>--%>
                            </td>
                            <td>
                                <%--<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />&nbsp;--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" class="h10"></td>
                        </tr>
                    </table>
                    <%--</asp:Panel>--%>
                </td>
            </tr>
            <tr style="height: 40px;">
            </tr>
            <tr>
                <td align="center">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td colspan="2" align="center">
                                    <%--                                    <asp:GridView ID="grdSignInSignOut" runat="server" OnRowCancelingEdit="grdSignInSignOut_RowCancelingEdit"
                                        OnRowUpdating="grdSignInSignOut_RowUpdating" OnSorting="grdSignInSignOut_Sorting"
                                        AllowSorting="True" OnRowDataBound="grdSignInSignOut_RowDataBound" OnPageIndexChanging="grdSignInSignOut_PageIndexChanging"
                                        OnRowDeleting="grdSignInSignOut_RowDeleting" OnRowEditing="grdSignInSignOut_RowEditing"
                                        PageSize="10" AllowPaging="true" AutoGenerateColumns="False" Width="100%" CssClass="grid">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Out Of Office ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutOfOFficeId" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutOfOfficeID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UserID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserid" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"UserId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" SortExpression="OutTimeDate">
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblOutDate" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeDate") %>'
                                                        Width="1px" Visible="False"></asp:Label>
                                                    <asp:TextBox ID="txtDate1" runat="Server" Text='<%# Bind ("OutTimeDate") %>' Width="65px"></asp:TextBox>&nbsp;<asp:ImageButton
                                                        ID="imgbtnSearchToDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                        CausesValidation="false" ImageAlign="Middle"></asp:ImageButton>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" PopupButtonID="imgbtnSearchToDate"
                                                        TargetControlID="txtDate1">
                                                    </ajaxToolkit:CalendarExtender>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutDate" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OutTime1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOuttime1" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeTime")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OutTime" SortExpression="OutTimeTime">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlOutTimeHrs" runat="Server">
                                                    </asp:DropDownList>
                                                    &nbsp;
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
                                                    <asp:Label ID="lblOuttime" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"OutTimeTime")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="InTime1" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIntime1" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"InTimeTime")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="InTime" SortExpression="InTimeTime">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlInTimeHrs" runat="Server">
                                                    </asp:DropDownList>
                                                    &nbsp;
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
                                                    <asp:Label ID="lblIntime" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"InTimeTime")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtComments" runat="Server" TextMode="MultiLine" Text='<%# Bind("Comment") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComment" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Comment")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblresonID" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"TypeId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reason" SortExpression="Reason">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlReason" runat="Server">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblreson" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Reason")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"Status")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver" SortExpression="Approver">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprover" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"ApproverName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approver Comments" SortExpression="ApproverComments">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproverComment" runat="Server" Text='<%# DataBinder.Eval (Container.DataItem,"ApproverComments")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" runat="SERVER" Text="Update" CommandName="Update"
                                                        CausesValidation="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" runat="Server" Text="Cancel" CommandName="Cancel"
                                                        CausesValidation="FALSE"></asp:LinkButton>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApproved" runat="Server"></asp:Label>
                                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="False"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnDelete" runat="Server" Text="Delete" CommandName="Delete"
                                                        CausesValidation="false" OnClientClick="return confirm('Are you sure ? you want to delete this item');"
                                                        Visible="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnCancel" runat="Server" Text="Cancel Out Of Office" CommandName="Delete"
                                                        CausesValidation="false" OnClientClick="return confirm('Are you sure ? you want to cancel this item');"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>--%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <%--                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
                        ShowMessageBox="True"></asp:ValidationSummary>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="False"
                        ShowMessageBox="True" ValidationGroup="vgSearch"></asp:ValidationSummary>--%>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>