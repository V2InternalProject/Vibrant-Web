<%@ Page Title="ManualSigninSignOut" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="True" CodeBehind="ManualSignin.aspx.cs" Inherits="HRMS.Orbitweb.ManualSignin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../JavaScript/common.js" type="text/javascript"></script>
    <!-- SelectBOx -->
    <%--<script src="../JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <%--<%@ Page Language="C#" AutoEventWireup="true" Inherits="ManualSignInSignOut" CodeBehind="ManualSignInSignOut.aspx.cs"
    Title="Manual SignInSignOut" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
    <script type="text/javascript">

        //New addition

        $(document).ready(function () {
            if ($('#MainContent_PanelSignIn').is(':visible') == true && $('#MainContent_PanelSignOut').is(':visible') == false) {

                $('.ManualFormDiv').css('width', '423px');
            }
            if ($('#MainContent_PanelSignOut').is(':visible') == true && $('#MainContent_PanelSignIn').is(':visible') == false) {

                $('.ManualFormDiv').css('width', '423px');
            }

            if ($("[id$=MainTabSelected]").val() == "Manual") {
                $('#ManualSignIn').addClass('selected');
                $('#AutoSignIn').removeClass('selected');
            }
            else {
                $('#ManualSignIn').removeClass('selected');
                $('#AutoSignIn').addClass('selected');
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
    <%--<body class="AttendancePage">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>--%>
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <asp:HiddenField ID="MainTabSelected" runat="server" />
    <section class="AttendanceContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Attendance</h2>
                <%--                <div class="EmpSearch clearfix">
                    <a href="#"></a>
                    <input type="text" placeholder="Employee Search">
                </div>--%>
            </div>
            <nav class="sub-menu-colored">
                <a href="SignInSignOut.aspx" class="selected" id="AutoSignIn">Auto</a> <a href="ManualSignin.aspx"
                    id="ManualSignIn">Manual</a>
            </nav>
        </div>
        <div class="MainBody ManualContainer">
            <%--<h3>
                Manual</h3>--%>
            <div align="center" class="SuccessMsgOrbit">
                <asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>
            </div>
            <div align="center" class="ErrorMsgOrbit">
                <asp:Label ID="lblErrorMess" runat="server" SkinID="lblError"></asp:Label>
            </div>
            <div class="clearfix ManualFormDiv">
                <asp:Panel ID="PanelSignIn" Width="100%" runat="server">
                    <div class="ManualInTimeDiv">
                        <div class="clearfix">
                            <h3>In Time</h3>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Sign-In Date :</label>
                                </div>
                                <div class="InputDiv">
                                    <%--<input type="text">--%>
                                    <asp:TextBox ID="txtSignInDate" runat="server" Width="150px" OnTextChanged="txtSignInDate_TextChanged"
                                        AutoPostBack="True" onpaste="return false" autocomplete="off"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSignInDate" runat="server" ImageAlign="AbsMiddle" CausesValidation="false"
                                        ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                    <asp:RequiredFieldValidator ID="rfvSignInDate" runat="server" SetFocusOnError="True"
                                        ErrorMessage="Please select In Time Date" Display="None" ControlToValidate="txtSignInDate"></asp:RequiredFieldValidator>&nbsp;

                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSignInDate"
                                        PopupButtonID="imgbtnSignInDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RangeValidator ID="rvInDate" runat="server" Display="None" ErrorMessage="Please dont enter future date"
                                        Type="Date" ControlToValidate="txtSignInDate"></asp:RangeValidator>
                                    <%--<img src="images/calender-icon.png"  class="datepicker-image">--%>
                                </div>
                            </div>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Time :</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlInHrs" runat="server" Width="83px">
                                        <asp:ListItem Text="Hrs" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvInHours" runat="server" ErrorMessage="Please select In Time Hours"
                                        Display="None" ControlToValidate="ddlInHrs" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddlInMins" runat="server" Width="83px">
                                        <asp:ListItem Text="Mins" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvInMinutes" runat="server" ErrorMessage="Please select In Time Minutes"
                                        Display="None" ControlToValidate="ddlInMins" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Comments :</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtInComments" runat="server" TextMode="MultiLine">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvInComments" runat="server" SetFocusOnError="True"
                                        ErrorMessage="Please enter In Time comments!" Display="None" ControlToValidate="txtInComments">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelSignOut" runat="server" Width="100%">
                    <div class="ManualOutTimeDiv">
                        <div class="clearfix">
                            <h3>Out Time</h3>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Sign-Out Date :</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtSignOutDate" runat="server" Width="150px" onpaste="return false" autocomplete="off"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSignOutDate" runat="server" ImageAlign="AbsMiddle" CausesValidation="false"
                                        ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                    <asp:RequiredFieldValidator ID="rfvSignOutDate" runat="server" SetFocusOnError="True"
                                        ErrorMessage="Please select Out Time Date" Display="None" ControlToValidate="txtSignOutDate"></asp:RequiredFieldValidator>&nbsp;

                                    <ajaxToolkit:CalendarExtender ID="ceOut" runat="server" TargetControlID="txtSignOutDate"
                                        PopupButtonID="imgbtnSignOutDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:CompareValidator ID="cvDate" runat="server" ControlToCompare="txtSignInDate"
                                        ControlToValidate="txtSignOutDate" Display="None" ErrorMessage="Sign Out date cannot be smaller than the Sign In Date"
                                        Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                    <asp:RangeValidator ID="rvOutDate" runat="server" Display="None" ErrorMessage="Please dont enter future date"
                                        Type="Date" ControlToValidate="txtSignOutDate"></asp:RangeValidator>
                                    <%--<img src="images/calender-icon.png" class="datepicker-image">--%>
                                </div>
                            </div>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Time :</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlOutHrs" runat="server" Width="83px">
                                        <asp:ListItem Text="Hrs" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvOutHours" runat="server" ErrorMessage="Please select Out Time Hours"
                                        Display="None" ControlToValidate="ddlOutHrs" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddlOutMins" runat="server" Width="83px">
                                        <asp:ListItem Text="Mins" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvOutMinutes" runat="server" ErrorMessage="Please select Out Time Minutes"
                                        Display="None" ControlToValidate="ddlOutMins" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="clearfix formcol">
                                <div class="LabelDiv">
                                    <label>
                                        Comments :</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtOutComments" runat="server" TextMode="MultiLine">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvOutComments" runat="server" SetFocusOnError="True"
                                        ErrorMessage="Please enter Out Time comments!" Display="None" ControlToValidate="txtOutComments">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="ManualButtonDiv clearfix">
                <%--                    <input type="button" class="ButtonGray mrgnR11" value="Save">
                    <input type="button" class="ButtonGray" value="Cancel">--%>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    CausesValidation="true" Style="margin-right: 10px;" CssClass="ButtonGray mrgnR11" Enabled="false"></asp:Button>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    CausesValidation="false" CssClass="ButtonGray"></asp:Button>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
            ShowMessageBox="True"></asp:ValidationSummary>
    </section>
    <%--    </form>
</body>
</html>--%>
    <%--    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0" style="display: none">
        <tbody>
            <tr>
                <td class="tableHeadBlueLight" align="center">
                    Manual Sign-In Sign-Out
                </td>
            </tr>
            <tr>
                <td class="lineDotted" valign="baseline">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:Label ID="lblSuccess" runat="SERVER" SkinID="lblSuccess"> </asp:Label>--%>
    <tr>
        <td align="center">
            <asp:Table ID="tblSignInSignOut" runat="server" Width="100%" Style="display: none">
                <asp:TableRow>
                    <asp:TableCell>
                        <%--<asp:Panel ID="PanelSignIn" Width="100%" runat="server">--%>
                        <table class="tableBorder" width="45%">
                            <tr style="height: 40px;">
                                <td class="tableHeadBlueLight" align="center" colspan="3">In Time
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblSignInTime" runat="server" Text=" Sign In Date"></asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblSignInDateC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                                <asp:TextBox ID="txtSignInDate" runat="server" Width="150px" OnTextChanged="txtSignInDate_TextChanged"
                                                    AutoPostBack="True"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnSignInDate" runat="server" ImageAlign="AbsMiddle" CausesValidation="false"
                                                    ImageUrl="~/images/Calendar_scheduleHS.png"></asp:ImageButton>
                                                <asp:RequiredFieldValidator ID="rfvSignInDate" runat="server" SetFocusOnError="True"
                                                    ErrorMessage="Please select a Date" Display="None" ControlToValidate="txtSignInDate"></asp:RequiredFieldValidator>&nbsp;
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSignInDate"
                                                    PopupButtonID="imgbtnSignInDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RangeValidator ID="rvInDate" runat="server" Display="None" ErrorMessage="Please dont enter future date"
                                                    Type="Date" ControlToValidate="txtSignInDate"></asp:RangeValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblInTime" runat="server" Text="Time"></asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblInTimeC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                                <asp:DropDownList ID="ddlInHrs" runat="server" Width="83px">
                                                    <asp:ListItem Text="Hrs" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvInHours" runat="server" ErrorMessage="Please select the Hours"
                                                    Display="None" ControlToValidate="ddlInHrs" InitialValue="-1"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlInMins" runat="server" Width="83px">
                                                    <asp:ListItem Text="Mins" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvInMinutes" runat="server" ErrorMessage="Please select the Minutes"
                                                    Display="None" ControlToValidate="ddlInMins" InitialValue="-1">
                                                </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 85px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblInComments" runat="server" Text="Comments">
                                    </asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblInCommentsC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%" style="display: none">
                                    <%--                                                <asp:TextBox ID="txtInComments" runat="server" Width="170px" Height="55px" TextMode="MultiLine">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvInComments" runat="server" SetFocusOnError="True"
                                                    ErrorMessage="Please enter the comments!" Display="None" ControlToValidate="txtInComments">
                                                </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                        </table>
                        <%--</asp:Panel>--%>
                    </asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell>
                        <%--<asp:Panel ID="PanelSignOut" runat="server" Width="100%">--%>
                        <table class="tableBorder" width="45%">
                            <tr style="height: 40px;">
                                <td class="tableHeadBlueLight" align="center" colspan="3">Out Time
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblSignOutTime" runat="server" Text=" Sign Out Date"></asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblSignOutDateC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%"></td>
                            </tr>
                            <tr style="height: 30px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblOutTime" runat="server" Text="Time"></asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblOutTimeC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                                <asp:DropDownList ID="ddlOutHrs" runat="server" Width="83px">
                                                    <asp:ListItem Text="Hrs" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvOutHours" runat="server" ErrorMessage="Please select the Hours"
                                                    Display="None" ControlToValidate="ddlOutHrs" InitialValue="-1"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlOutMins" runat="server" Width="83px">
                                                    <asp:ListItem Text="Mins" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvOutMinutes" runat="server" ErrorMessage="Please select the Minutes"
                                                    Display="None" ControlToValidate="ddlOutMins" InitialValue="-1">
                                                </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 85px;">
                                <td align="right" width="40%">
                                    <asp:Label ID="lblOutComments" runat="server" Text="Comments">
                                    </asp:Label>
                                </td>
                                <td align="center" width="2%">
                                    <asp:Label ID="lblOutCommentsC" runat="server" Text=":"></asp:Label>
                                </td>
                                <td align="left" width="58%">
                                    <%--                                              <asp:TextBox ID="txtOutComments" runat="server" Width="170px" Height="55px" TextMode="MultiLine">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvOutComments" runat="server" SetFocusOnError="True"
                                                    ErrorMessage="Please enter the comments!" Display="None" ControlToValidate="txtOutComments">
                                                </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr style="height: 15px;">
                            </tr>
                        </table>
                        <%--</asp:Panel>--%>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
</asp:Content>