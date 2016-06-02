<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true" Inherits="SelectedCandidate" CodeBehind="SelectedCandidate.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>

    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>

    <script src="~/JavaScript/common.js"></script>
    <%--  <script src="~/JavaScript/jquery.selectbox-0.2.min.js"></script>    --%>

    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('input[id$=txtProbationPeriod]').bind('cut copy paste', function (e) {
                e.preventDefault();
            });

            $('#btnPrint').click(function () {
                var docprint = window.open("about:blank", "_blank");
                var oTable = document.getElementById("MainContent_tblSelectedCandidate");
                docprint.document.open();
                docprint.document.write('<html><head><link type="text/css" rel="stylesheet" href="../Content/New%20Design/demo.css" />');
                docprint.document.write('<link type="text/css" rel="stylesheet" href="../Content/New%20Design/common.css" />');
                docprint.document.write('<link href="../../Content/New Design/jquery.selectbox.css" type="text/css" rel="stylesheet" />');
                docprint.document.write('</head><body>');
                //docprint.document.write('Appraisee Name :' + employeeName + '</br>');
                docprint.document.write(oTable.innerHTML);
                docprint.document.write('</body></html>');
                docprint.document.close();
                docprint.print();
                docprint.close();
                return true;
            });
        });
    </script>
    <script language="javascript" type="text/javascript">

        function ShowofferGenerated() {

            if (document.getElementById("MainContent_ddlAction").value == "7") {
                document.getElementById("MainContent_ShowHideRow").style.visibility = "visible";
                document.getElementById("MainContent_HideDate").style.visibility = "visible";

            } else {
                document.getElementById("MainContent_ShowHideRow").style.visibility = "hidden";
                document.getElementById("MainContent_HideDate").style.visibility = "hidden";
                document.getElementById("MainContent_txtCTC").value = "";
            }
        }

        function checkvalidData() {

            var title = "";
            var message = '';
            var flag = 1;
            var Budget = document.getElementById("MainContent_txtCTC").value;
            var filter1 = /^\d{1,3}(\.\d{1,2})?$/;
            var CandidateScheduleDate = document.getElementById("MainContent_txtJoiningDate");

            if (document.getElementById("MainContent_ddlAction").value == "7") {
                {

                    if (document.getElementById("MainContent_ddlOfferedPosition").value == "") {
                        //                        alert("Please select offered position");
                        //
                        //                        return false;
                        message = '<p>' + 'Please select offered position' + '</p>';
                        document.getElementById("MainContent_ddlOfferedPosition").focus();
                        flag = 0;
                    }
                    if (document.getElementById("MainContent_ddlGrade").value == "") {
                        //                        alert("Please select grade");
                        //                        document.getElementById("MainContent_ddlGrade").focus();
                        //                        return false;

                        message = message + '<p>' + 'Please select grade' + '</p>';
                        document.getElementById("MainContent_ddlGrade").focus();
                        flag = 0;
                    }
                    if (document.getElementById("MainContent_txtCTC").value == "") {
                        //                        alert("Please enter CTC ");
                        //                        document.getElementById("MainContent_txtCTC").focus();
                        //                        return false;
                        message = message + '<p>' + 'Please enter CTC' + '</p>';
                        document.getElementById("MainContent_txtCTC").focus();
                        flag = 0;
                    }

                    else if (!filter1.test(Budget)) {
                        //                        alert(' Invalid CTC ! Please re-enter.');
                        //                        document.getElementById("MainContent_txtCTC").focus();
                        //                        return false;
                        message = message + '<p>' + 'Invalid CTC ! Please re-enter.' + '</p>';
                        document.getElementById("MainContent_txtCTC").focus();
                        flag = 0;

                    }

                    //\d+(\.\d{1,2})?
                    if (document.getElementById("MainContent_txtJoiningDate").value == "") {
                        //                            alert("Please enter joining date ");
                        //                            document.getElementById("MainContent_txtJoiningDate").focus();
                        //                            return false;

                        message = message + '<p>' + 'Please enter joining date .' + '</p>';
                        document.getElementById("MainContent_txtJoiningDate").focus();
                        flag = 0;
                    }
                    else if (CandidateScheduleDate) {

                        var expireOnDate = CandidateScheduleDate.value;
                        var pos1 = expireOnDate.indexOf("/");
                        var pos2 = expireOnDate.indexOf("/", pos1 + 1);
                        var strMonth = eval(expireOnDate.substring(0, pos1) - 1);
                        var strDay = expireOnDate.substring(pos1 + 1, pos2);
                        var strYear = expireOnDate.substring(pos2 + 1);
                        var strDate = new Date();

                        strDate.setDate(strDay);
                        strDate.setMonth(strMonth);
                        strDate.setFullYear(strYear);

                        var today = new Date();
                        if (CandidateScheduleDate.value != "") {
                            if (CandidateScheduleDate) {
                                if (strDate < today) {
                                    //alert("Joing  date should be greater than equal to current date");
                                    //return false;
                                    message = message + '<p>' + 'Joining  date should be greater than equal to current date .' + '</p>';
                                    flag = 0;
                                }
                                else {

                                    //                                        document.getElementById('<%=tblSelectedCandidate.ClientID %>').style.display = 'none';
                                    //                                        document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                                    //                                        return true;
                                }
                            }
                            else {

                                message = message + '<p>' + 'Joining  date should be greater than equal to current date .' + '</p>';
                                flag = 0;
                            }
                        }
                        else {

                            message = message + '<p>' + 'Joining  date should be greater than equal to current date .' + '</p>';
                            flag = 0;
                        }
                    }
                    else {

                        message = message + '<p>' + 'Joining  date should be greater than equal to current date .' + '</p>';
                        flag = 0;
                    }
            }
        } else {

            if (document.getElementById("MainContent_txtJoiningDate").value == "") {

                //                    message = message + '<p>' + 'Please enter joining date .' + '</p>';
                //                    document.getElementById("MainContent_txtJoiningDate").focus();
                //                    flag = 0;
            }
        }

        if (flag == 1) {
            document.getElementById('<%=tblSelectedCandidate.ClientID %>').style.display = 'none';
            DisplayLoadingDialog();
            //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
            return true;
        }
        else {

            V2hrmsAlert(message, title);
            return false;
        }

    }

    function CheckNumericKeyInfo(_char, _mozChar) {

        if (_mozChar != null) { // Look for a Mozilla-compatible browser
            if ((_mozChar >= 48 && _mozChar <= 57) || _mozChar == 0 || _mozChar == 8 || _mozChar == 13 || _mozChar == 46) _RetVal = true;
            else {
                _RetVal = false;

                V2hrmsAlert('<p>' + 'Please enter a numeric value.' + '</p>', '');
                document.getElementById("MainContent_txtCTC").focus();
            }
        }
        else { // Must be an IE-compatible Browser
            if ((_char >= 48 && _char <= 57) || _char == 0 || _char == 8 || _char == 13 || _char == 46) _RetVal = true;
            else {
                _RetVal = false;
                V2hrmsAlert('<p>' + 'Please enter a numeric value.' + '</p>', '');
                document.getElementById("MainContent_txtCTC").focus();
            }
        }
        return _RetVal;
    }
    </script>
    <script language="javascript" type="text/javascript">

        function SingleSelectCheckbox(current) {
            var flag = true;
            if (current.checked == false)
                flag = false;

            var gv = document.getElementById('<%= this.grdSelectedcandidate.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                if (typeof (inputs[0]) != 'undefined') {

                    if (inputs[0].type == "checkbox") {
                        inputs[0].checked = false;
                    }
                }
            }

            if (flag == false)
            { current.checked = false; }
            else
            { current.checked = true; }
        }

        function selectedValidate() {
            var flag = true;
            var gv = document.getElementById('<%= this.grdSelectedcandidate.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];
                // alert(rowElement.getElementById('hdID').value);
                var inputs = rowElement.getElementsByTagName("input");

                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked == true) {
                            flag = false;
                            //                            var CandidateID = inputs[1].value;
                            //                            var RRFID = inputs[2].value;
                            //                            var ScheduleID = inputs[3].value;
                            //                            var StageID = inputs[4].value;
                        }
                    }
                }
            }

            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record.' + '</p>', '');
                return false;
            }

        }
    </script>
    <script language="javascript" type="text/javascript">
        function SelectedCandidateLoader() {
            document.getElementById('<%=tblSelectedCandidate.ClientID %>').style.display = 'inline';
            document.getElementById('<%=loadImage.ClientID %>').style.display = 'none';

        }

        $(document).ready(function () {

            $('.aspNetDisabled').each(function (index, element) {
                var $element = $(element);
                if ($element.is('select')) {
                    $(this).replaceWith(function () { return $(this) });
                } else if ($element.is('checkbox')) {
                    // code here
                    $(this).replaceWith(function () { return $(this) });
                } else if ($element.is('span')) {
                    $(this).replaceWith(function () { return $('input', this) });
                }

            });

            if ($('#MainContent_txtJoiningDate').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtJoiningDate').text($('#MainContent_txtJoiningDate').val());
                $('#MainContent_txtJoiningDate').hide();
                $('#MainContent_imgbtnDate').hide();
                //Lets Hide DropDown
                $('*[id*=sbSelector_]').each(function () {
                    var DropVal = $(this).text();
                    var obj = $(this);
                    var vali = obj[0].id;
                    var Parentobj = $('#' + vali).parents('.InputDiv').children(".ClassDisplayLabel")[0].id;
                    $('#' + Parentobj).text(DropVal);
                });
                //Hide All Drop Down
                $('*[id*=sbHolder_]').each(function () {
                    var obj = $(this);
                    var vali = obj[0].id;
                    if ($('#' + vali)) {
                        $('#' + vali).attr("style", "display:none");
                    }
                });
                // Start Hiding
                $('.hiddenstar').hide();
            }
            if ($('#MainContent_txtProbationPeriod').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtProbationPeriod').text($('#MainContent_txtProbationPeriod').val());
                $('#MainContent_txtProbationPeriod').hide();

            }
            if ($('#MainContent_txtComment').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtComment').text($('#MainContent_txtComment').val());
                $('#MainContent_txtComment').hide();
            }
            //MainContent_txtCTC
            if ($('#MainContent_txtCTC').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtCTC').text($('#MainContent_txtCTC').val());
                $('#MainContent_txtCTC').hide();
            }

        });
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <div align="right">
        <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="confirmPrint" CssClass="ButtonGray" />--%>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="ButtonGray" />
    </div>
    </br>

    <asp:Panel ID="pnl1" runat="server">
        <%--table border="0" cellpadding="0" cellspacing="10" width="98%" align="center">
            <tr>
                <td colspan="4" align="center">
                    <asp:Label ID="lblWelcome" runat="server" Text="Selected Candidate Form" SkinID="lblWelcome"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblErrorMsg" runat="server" SkinID="lblError" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>--%>

        <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>
        <section class="ConfirmationContainer Container">
            <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
            <div class="MainBody RecH feedBack">
                <div class="rwrap" id="tblSelectedCandidate" runat="server">
                    <h3 class="smartrackH">Selected Candidate Form</h3>
                    <asp:Label ID="Label10" runat="server" SkinID="lblError" Visible="false"></asp:Label>

                    <div class="clearfix mrgnT20">

                        <div class="floatL">
                            <asp:Label ID="Label1" runat="server" Text="RRF No :" CssClass="prefix" AssociatedControlID="Label1"></asp:Label>
                            <asp:Label ID="lblRRFNo" CssClass="suffix" AssociatedControlID="lblRRFNo" runat="server"></asp:Label>
                        </div>

                        <div class="floatL">
                            <asp:Label ID="Label2" runat="server" Text="Position :" CssClass="prefix" AssociatedControlID="Label2"></asp:Label>
                            <asp:Label ID="lblPosition" CssClass="suffix" runat="server" AssociatedControlID="lblPosition"></asp:Label>
                        </div>

                        <div class="floatL">
                            <asp:Label ID="Label17" runat="server" Text="Final Score# :" CssClass="prefix" AssociatedControlID="Label17"></asp:Label>
                            <asp:Label ID="lblFinalScore" runat="server" CssClass="suffix" AssociatedControlID="lblFinalScore"></asp:Label>
                        </div>

                        <div class="floatL">
                            <asp:Label ID="Label3" runat="server" Text="Candidate :" CssClass="prefix expenselocation" AssociatedControlID="Label3"></asp:Label>
                            <asp:Label ID="lblCandidate" CssClass="suffix expenseLoc" runat="server" Width="30%" AssociatedControlID="lblCandidate"></asp:Label>
                        </div>

                        <div class="floatL">
                            <asp:Label ID="Label4" runat="server" Text="Stage :" CssClass="prefix" AssociatedControlID="Label4"></asp:Label>
                            <asp:Label ID="lblStage" CssClass="suffix datealign" runat="server" Width="30%" AssociatedControlID="lblStage"></asp:Label>
                        </div>
                    </div>

                    <div class="ButtonContainer1">
                        <asp:Button ID="btnShowFeedBack" runat="server" Text="Show Feedback" CssClass="ButtonGray"
                            OnClick="btnShowFeedBack_Click" OnClientClick="javascript:return selectedValidate()" />
                    </div>

                    <br />
                    <br />
                    <br />
                    <asp:GridView ID="grdSelectedcandidate" runat="server" Width="100%" AutoGenerateColumns="False"
                        CellSpacing="0" CellPadding="0" ShowHeaderWhenEmpty="True" ShowFooter="false"
                        AllowPaging="true" AllowSorting="true" PageSize="20" EmptyDataText="No Data Found"
                        OnRowDataBound="grdSelectedcandidate_RowDataBound" CssClass="grid TableJqgrid" OnPageIndexChanging="grdSelectedcandidate_PageIndexChanging">
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField HeaderText=" Candidate ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCandidateID" runat="server" Text='<%# Bind("CandidateID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" RRF ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRRFNO" runat="server" Text='<%# Bind("RRFNO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleID" runat="server" Text='<%# Bind("ScheduleID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Stage ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStageID" runat="server" Text='<%# Bind("StageID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage Name">
                                <ItemTemplate>
                                    <%--<a id="hpyHRComment" style="text-decoration: none" href="InterviewfeedBack.aspx?RRFID=<%#DataBinder.Eval(Container.DataItem,"RRFNo")%>&CandidateID=<%#DataBinder.Eval(Container.DataItem,"CandidateID")%>&StageID=<%#DataBinder.Eval(Container.DataItem,"StageID")%>&Mode=Read"
                                                    target="_blank">
                                                    <%#DataBinder.Eval(Container.DataItem, "StageName")%></a>--%>
                                            &nbsp;<asp:Label ID="lblStageName" runat="server" Text='<%# Bind("StageName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" HR Comments">
                                <ItemTemplate>
                                    <%--        <a id="hpyHRComment" style="text-decoration: none" href="InterviewfeedBack.aspx?RRFID=<%#DataBinder.Eval(Container.DataItem,"RRFNo")%>&CandidateID=<%#DataBinder.Eval(Container.DataItem,"CandidateID")%>&StageID=<%#DataBinder.Eval(Container.DataItem,"StageID")%>"
                                                    target="_blank">
                                                    <%#DataBinder.Eval(Container.DataItem, "HRComments")%></a>--%>
                                            &nbsp;<asp:Label ID="lblHRComment" runat="server" Text='<%# Bind("HRComments") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Interviewers Comments">
                                <ItemTemplate>
                                    &nbsp;<asp:Label ID="lblInterviewersComments" runat="server" Text='<%# Bind("OverallComments") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" HRM Comments">
                                <ItemTemplate>
                                    &nbsp;<asp:Label ID="lblComments" runat="server" Text='<%# Bind("SelectedComment") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                    <asp:Label runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br />

                    <asp:Label ID="lblMessage" runat="server" Visible="false" SkinID="lblError"></asp:Label>

                    <div id="HRMView" runat="server">
                        <div class="clearfix">
                            <div class="clearfix sec3C">
                                <div id="" class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="Label5" runat="server" Text="Action"></asp:Label>
                                            </div>
                                            <div class="InputDiv" id="div_ddlAction">
                                                <asp:DropDownList ID="ddlAction" runat="server" TabIndex="0" onblur="javascript:return ShowofferGenerated();"
                                                    Style="margin-left: 1px">
                                                    <asp:ListItem Value="7" Text="Generate offer" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="On Hold"></asp:ListItem>
                                                    <asp:ListItem Value="15" Text="Rejected"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlAction" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="Label6" runat="server" Text="Employment Type"></asp:Label>
                                            </div>
                                            <div class="InputDiv" id="div_ddlEmploymentType">
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlEmploymentType" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div id="ShowHideRow" runat="server">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="Label7" runat="server" Text="Offered Position"></asp:Label>
                                                </div>
                                                <div class="InputDiv" id="div_ddlOfferedPosition">
                                                    <asp:DropDownList ID="ddlOfferedPosition" runat="server" TabIndex="2">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_ddlOfferedPosition" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label8" runat="server" Text="*Grade"></asp:Label>
                                                    <asp:Label ID="lblFirstNameMandatory" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                </div>
                                                <div class="InputDiv" id="div_ddlGrade">
                                                    <asp:DropDownList ID="ddlGrade" runat="server" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_ddlGrade" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label9" runat="server" Text="*CTC (lacs p. a.)(eg. 3.40)"></asp:Label>
                                                <asp:Label ID="Label11" runat="server" Text="" SkinID="lblError"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtCTC" runat="server" MaxLength="6" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"></asp:TextBox>
                                                <asp:Label ID="lbl_txtCTC" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="test" runat="server" class="CandidateLeftcol clearfix mrgnB30">
                                    <div id="HideDate" runat="server" class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label12" runat="server" Text="*Joining Date"></asp:Label>
                                                <asp:Label ID="Label16" runat="server" Text="" SkinID="lblError"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtJoiningDate" runat="server" ReadOnly="false" onkeypress="return false"
                                                    TabIndex="4" Width="203px"></asp:TextBox>
                                                <asp:Label ID="lbl_txtJoiningDate" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:ImageButton ID="imgbtnDate" runat="server" CausesValidation="false" ImageUrl="../Images/New%20Design/calender-icon.png" ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger mrgnR12" />&nbsp;&nbsp;

                                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtenderFromDate" runat="server"
                                                    TargetControlID="txtJoiningDate" PopupButtonID="imgbtnDate" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="Label13" runat="server" Text="Probation Period"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtProbationPeriod" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"
                                                    runat="server" MaxLength="2" TabIndex="5"></asp:TextBox>
                                                <asp:Label ID="lbl_txtProbationPeriod" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:Label ID="Label14" runat="server" Text=" Months" CssClass="exampleNumber"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="Label15" runat="server" Text="Comment"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine"
                                                    TabIndex="6"></asp:TextBox>
                                                <asp:Label ID="lbl_txtComment" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ButtonContainer1 clearfix">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                            OnClientClick="javascript:return checkvalidData();" TabIndex="7" CssClass="ButtonGray" />
                    </div>
                </div>
            </div>
        </section>
    </asp:Panel>
</asp:Content>