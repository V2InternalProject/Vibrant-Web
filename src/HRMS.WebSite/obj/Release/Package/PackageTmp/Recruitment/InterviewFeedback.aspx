<%@ Page Language="C#" AutoEventWireup="true" Inherits="InterviewFeedback" Title="InterviewFeedback"
    CodeBehind="InterviewFeedback.aspx.cs" %>

<link href="../CSS/Orbit.css" rel="stylesheet" type="text/css" />
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8; IE=7; IE=EDGE" />
    <title></title>

    <script type="text/javascript" src="../Scripts/Recruitment/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/pop_up_window.js"></script>
    <script type="text/javascript" src="../Scripts/New Design/jquery.selectBox.js"></script>
    <script type="text/javascript" src="../Scripts/New Design/jquery.selectbox-0.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/HRMS.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/jquery.tmpl.js"></script>

    <link href="../Content/New Design/demo.css" rel="stylesheet" />
    <link href="../Content/New Design/common.css" rel="stylesheet" />
    <link href="../Content/New Design/hr.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/jquery.mmenu.css" />
    <link href="../Content/themes/base/jquery.ui.dialog.css" rel="stylesheet" />

    <script language="VBScript" type="text/vbscript">

Function myAlert(title, content)
MsgBox content, 0,title
End Function
    </script>
    <script language="javascript" type="text/javascript">
        var title = "";

        function validateGrid() {

            var gv = document.getElementById('<%= this.grdSkills.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");

                if (typeof (inputs[0]) != 'undefined') {

                    if (inputs[0].type == "text") {
                        if (inputs[0].value == '') {
                            V2hrmsAlert('<p>' + 'Enter Skills' + '</p>', title);
                            return false;
                        }
                        if (inputs[1].value == '') {
                            V2hrmsAlert('<p>' + 'Enter Rating' + '</p>', title);
                            return false;
                        }
                        else if (inputs[1].value < 0 || inputs[1].value > 4) {
                            //V2hrmsAlert('<p>' + 'Rating should be one of the values given in the legend.' + '</p>', title);
                            V2hrmsAlert('<p>' + 'Please enter valid ratings.' + '</p>', title);
                            return false;
                        }
                    }
                }
            }
        }

        function DisplayLoadingDialog() {
            $("#loading").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $('#loading').parent().css('background-color', 'transparent');
                    $(this).parent().next('.ui-widget-overlay').css('z-index', '32');
                    $(this).parent().css('z-index', '33');
                }
            });
        }

        function Validate(skills, rating) {

            var flag = 1;
            var message = "";

            if (skills.value == "") {
                skills.focus();
                message = message + '<p>' + 'Please enter skills' + '</p>';
                flag = 0;
            }

            if (rating.value == "") {
                rating.focus();
                message = message + '<p>' + 'Please enter rating' + '</p>';
                flag = 0;

            }

            if (rating.value != "" && rating.value > 4) {
                rating.focus();
                //message = message + '<p>' + 'Please enter rating between 0 to 4' + '</p>';
                message = message + '<p>' + 'Please enter valid ratings.' + '</p>';
                flag = 0;
            }

            if (flag == 1) {
                return true;
            }
            else {
                V2hrmsAlert(message, title);
                return false;
            }

        }

        function ValidateBlankComments() {
            var flag1 = 1;
            if (document.getElementById('<%=txtLanguage.ClientID %>').value == '') {
                document.getElementById('<%= txtLanguage.ClientID %>').value = "";
                document.getElementById('<%= txtLanguage.ClientID %>').focus();
                V2hrmsAlert('<p>' + 'Enter rating in Strong English Communication skills/Team Player/some business customer facing field' + '</p>', title);
                flag1 = 0;
                //                return false;
            } else if (document.getElementById('<%= txtLanguage.ClientID %>').value < 0 || document.getElementById('<%= txtLanguage.ClientID %>').value > 4) {
                V2hrmsAlert('<p>' + 'Enter valid Legend values in Strong English Communication skills/Team Player/some business customer facing field (Between 0 and 4)' + '</p>', title);
                //                return false;
                flag1 = 0;
            }

            if (document.getElementById('<%= txtCompliance.ClientID %>').value == '') {
                document.getElementById('<%= txtCompliance.ClientID %>').value = "";
                document.getElementById('<%= txtCompliance.ClientID %>').focus();
                V2hrmsAlert('<p>' + 'Enter rating for disciplined field' + '</p>', title);
                //                return false;
                flag1 = 0;
            } else if (document.getElementById('<%= txtCompliance.ClientID %>').value < 0 || document.getElementById('<%= txtCompliance.ClientID %>').value > 4) {
                //V2hrmsAlert('<p>' + 'Enter valid Legend values for disciplined field (Between 0 and 4)' + '</p>', title);
                V2hrmsAlert('<p>' + 'Please enter valid ratings.' + '</p>', title);
                //                return false;
                flag1 = 0;
            }

            if (document.getElementById('<%= txtProjectKnowledge.ClientID %>').value == '') {
                document.getElementById('<%= txtProjectKnowledge.ClientID %>').value = "";
                document.getElementById('<%= txtProjectKnowledge.ClientID %>').focus();
                V2hrmsAlert('<p>' + 'Enter rating for current project knowledge field' + '</p>', title);
                //                return false;
                flag1 = 0;
            }

            if (document.getElementById('<%= txtOverallComments.ClientID %>').value == '') {
                document.getElementById('<%= txtOverallComments.ClientID %>').value = "";
                document.getElementById('<%= txtOverallComments.ClientID %>').focus();
                V2hrmsAlert('<p>' + 'Enter Overall Comments' + '</p>', title);
                //                return false;
                flag1 = 0;
            }
            if (flag1 == 1) {
                document.getElementById('<%=tblMain.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }
            else {
                return false;
            }

        }

        //        function NumberOnly() {
        //            var AsciiValue = event.keyCode
        //            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
        //                event.returnValue = true;
        //            else
        //                event.returnValue = false;
        //        }

        function CheckNumericKeyInfo(_char, _mozChar) {

            if (_char == 13) {

                var textvalue;
                var gv = document.getElementById('<%= this.grdSkills.ClientID %>');
                var rowCount = gv.rows.length;

                if (rowCount == 3) {
                    for (var i = 1; i < rowCount; i++) {
                        var rowElement = gv.rows[i];
                        var inputs = rowElement.getElementsByTagName("input");

                        if (typeof (inputs[2]) != 'undefined') {
                            textvalue = inputs[1].value;

                            if (inputs[2].type == "submit") {

                                if (inputs[1] != "") {

                                    inputs[2].focus();
                                }
                                else {
                                    inputs[2].focus();
                                }
                            }
                        }
                    }
                }
            }
            if (_mozChar != null) { // Look for a Mozilla-compatible browser
                if ((_mozChar >= 48 && _mozChar <= 52) || (_mozChar == 8) || (_mozChar == 13)) _RetVal = true;
                else {
                    _RetVal = false;
                    //V2hrmsAlert('<p>' + 'Please enter numeric values between 0 to 4' + '</p>', title);
                    V2hrmsAlert('<p>' + 'Please enter valid ratings.' + '</p>', title);
                    focus();
                }
            }
            else { // Must be an IE-compatible Browser
                if ((_char >= 48 && _char <= 52) || (_char == 8) || (_char == 13)) _RetVal = true;
                else {
                    _RetVal = false;
                    //V2hrmsAlert('<p>' + 'Please enter numeric values between 0 to 4' + '</p>', title);
                    V2hrmsAlert('<p>' + 'Please enter valid ratings.' + '</p>', title);
                    focus();
                }
            }

            return _RetVal;

        }

        function Focus() {
            var textvalue;
            var gv = document.getElementById('<%= this.grdSkills.ClientID %>');
            var rowCount = gv.rows.length;
            if (rowCount == 3) {
                for (var i = 1; i < rowCount; i++) {
                    var rowElement = gv.rows[i];
                    var inputs = rowElement.getElementsByTagName("input");

                    if (typeof (inputs[2]) != 'undefined') {
                        textvalue = inputs[1].value;
                        alert(textvalue);
                        if (inputs[2].type == "submit") {

                            if (inputs[1] != "") {

                                inputs[2].focus();
                            }
                            else {
                                inputs[2].focus();
                            }
                        }
                    }
                }
            }

        }

        function doClick() {
            //the purpose of this function is to allow the enter key to
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#btnPrint').click(function () {
                $('#btnAddMore1').hide();
                //var docprint = window.open("about:blank", "_blank");
                var docprint = window.open('', '', 'height=200,width=400');
                var oTable = document.getElementById("tblMain");
                docprint.document.open();
                docprint.document.write('<html><head>');
                docprint.document.write('<link type="text/css" rel="stylesheet"  media="print"  href="../Content/New%20Design/demo.css" />');
                docprint.document.write('<link type="text/css" rel="stylesheet"  media="print" href="../Content/New%20Design/common.css" />');
                docprint.document.write('<link type="text/css" rel="stylesheet"  media="print" href="../Content/New%20Design/hr.css" />');
                docprint.document.write('<link  media="print" href="../Content/New Design/jquery.selectBox (2).css" type="text/css" rel="stylesheet" />');
                docprint.document.write('</head><body>');
                //docprint.document.write('Appraisee Name :' + employeeName + '</br>');
                var content = oTable.innerHTML;
                var find = "type=\"submit\"";
                var re = new RegExp(find, 'g');
                content = content.replace(re, "style=\"display:none;\"");
                docprint.document.write(content);
                docprint.document.write('</body></html>');
                docprint.document.close();
                docprint.print();

                return true;
            });

            //grdSkills_btnAddMore1

            $('*[id*=grdSkills_btnAddMore1]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });

            //$('#txt_PMSOrganizationUnit').text($('#PMSOrganizationUnit').find(":selected").text());
            if ($('#txtLanguage').prop('disabled')) {
                //textbox is disabled
                $('#txt_txtLanguage').text($('#txtLanguage').val());
                $('#txtLanguage').hide();
                $('#Label18').hide();

            }
            if ($('#txtCompliance').prop('disabled')) {
                //textbox is disabled
                $('#txt_txtCompliance').text($('#txtCompliance').val());
                $('#txtCompliance').hide();
                $('#Label17').hide();

            }

            if ($('#txtProjectKnowledge').prop('disabled')) {
                //textbox is disabled
                $('#txt_txtProjectKnowledge').text($('#txtProjectKnowledge').val());
                $('#txtProjectKnowledge').hide();
                $('#Label14').hide();

            }

            if ($('#txtOverallComments').prop('disabled')) {
                //textbox is disabled
                $('#txt_txtOverallComments').text($('#txtOverallComments').val());
                $('#txtOverallComments').hide();
                $('#Label15').hide();

            }

            //$('#txt_txtLanguage').text($('#txtLanguage').val());
            // $('#txt_txtCompliance').text($('#txtCompliance').val());
            //$('#txt_txtProjectKnowledge').text($('#txtProjectKnowledge').val());
            //$('#txt_txtOverallComments').text($('#txtOverallComments').val());
            // $('#txtLanguage').hide();

        });
    </script>
    <%-- <link href="Styles/HRMS.css" rel="stylesheet" type="text/css" />
       <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />--%>
</head>
<body class="AttendancePage">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" runat="server">
        </asp:ScriptManager>
        <!---------- Model Code ------------->
        <div class="dialog" style="color: blue;" id="modal_alertModalWindow_container">
        </div>
        <script type="text/html" id="modal_alertModalWindow" style="color: Red;">
            <div class="modal quickFund" id="alertmodaltitle" title="${headerMessage}">
                <div class="midLeft">
                    <div class="midRight">
                        <form class="thin" action="">
                            <div class="innerContent">
                                <div class="padMe">
                                    <div class="info">
                                        <div class="messagePara">
                                            <p>${message}</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="modalButtons">
                                    <input type="button" value="OK" class="submitBtn btn orange_sm" onclick="$('#modal_alertModalWindow_container').dialog('close'); return false;">
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="botLeft">
                    <div class="botRight">
                    </div>
                </div>
            </div>
        </script>
        <!---------- Model Code ------------->
        <div id="page">
            <section class="ConfirmationContainer Container">
                <div class="MainBody RecH assessment">
                    <div class="rwrap">
                        <div class="clearfix" align="right">
                            <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="confirmPrint" CssClass="ButtonGray" />--%>
                            <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="ButtonGray" Visible="true" />
                        </div>
                        <asp:Panel ID="pnlInterviewFeedBack" runat="server" Width="100%">
                            <h3 class="smartrackH">Technical Interview Assessment Form</h3>
                            <div id="">
                                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                            </div>
                            <div id="">
                                <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>
                            </div>
                            <div class="clearfix mrgnT20 smartT" id="tblMain" runat="server">
                                <div class="clearfix">
                                    <div class="floatL">
                                        <asp:Label ID="Label2" runat="server" class="prefix" for="Employee_Name:" Text="Name of candidate:" />
                                        <asp:Label ID="lblCandidateName" class="suffix" for="ReimbursementEmployeeName" runat="server" />
                                    </div>
                                    <div class="floatL">
                                        <asp:Label class="prefix" for="Employee_Code:" ID="Label3" runat="server" Text="Date of interview:" />
                                        <asp:Label class="suffix" for="ReimbursementEmployeeCode" ID="lblInterviewDate" runat="server" />
                                    </div>
                                    <div class="floatL">
                                        <asp:Label class="prefix" for="Reimbursement_Form_Code:" ID="Label1" runat="server"
                                            Text="Level & Competency:" />
                                        <asp:Label class="suffix" for="FormCode" ID="lblCompetency" runat="server" />
                                    </div>
                                    <div class="floatL">
                                        <asp:Label class="prefix" for="Reimbursement_Form_Code:" ID="Label4" runat="server"
                                            Text=" Name of interviewer:" />
                                        <asp:Label class="suffix" for="FormCode" ID="lblInterviewer" runat="server" />
                                    </div>
                                </div>
                                <div>
                                    <p class="NoteHr mrgnT20">
                                        Legend &nbsp;&nbsp;&nbsp;0-No Basis&nbsp;&nbsp;&nbsp;1-Low&nbsp;&nbsp;&nbsp;2-Marginal&nbsp;&nbsp;&nbsp;3-Strong&nbsp;&nbsp;&nbsp;4-Outstanding
                                    </p>
                                </div>
                                <p class="NoteHr mrgnT20">
                                    <asp:Label ID="Label5" runat="server" Text="Technical Skill Domains:" CssClass="SubHeading"></asp:Label>
                                </p>
                                <p class="NoteHr">
                                    <asp:Label ID="Label6" runat="server" Text="Core Skills:"></asp:Label>
                                </p>
                                <asp:GridView ID="grdSkills" Width="100%" runat="server" CellPadding="3" AutoGenerateColumns="false"
                                    ShowFooter="true" OnRowCommand="grdSkills_RowCommand" OnRowDeleting="grdSkills_RowDeleting"
                                    OnRowEditing="grdSkills_RowEditing" OnRowCancelingEdit="grdSkills_RowCancelingEdit"
                                    OnRowDataBound="grdSkills_RowDataBound" OnRowUpdating="grdSkills_RowUpdating"
                                    OnRowCreated="grdSkills_RowCreated" OnRowDeleted="grdSkills_RowDeleted" AllowPaging="false"
                                    PageSize="100" CssClass="TableJqgrid">
                                    <HeaderStyle CssClass="tableHeaders" />
                                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                    <RowStyle CssClass="tableRows" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Sr No" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpSrNo" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center" Visible="false" HeaderText="ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" Text='<%#Eval("ID") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center" HeaderText="Skills">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSkills" Text='<%#Eval("Skills") %>' runat="server"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtSkills"
                                                    MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetSkills" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSkills" Text='<%#Eval("Skills") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtSkills1" runat="server" align="Right"></asp:TextBox><br />
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtSkills1"
                                                    MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetSkills" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center" HeaderText="Rating">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRating" Text='<%#Eval("Rating") %>' runat="server" MaxLength="1"
                                                    onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRating" Text='<%#Eval("Rating") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRating1" runat="server" align="Right" MaxLength="1" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"></asp:TextBox><br />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            FooterStyle-HorizontalAlign="Center" HeaderText="Actions">
                                            <EditItemTemplate>
                                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="ButtonGray"></asp:Button>
                                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="ButtonGray"></asp:Button>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnRemove1" runat="server" Text="Remove" CommandName="Delete" CssClass="ButtonGray"
                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CssClass="ButtonGray" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddMore1" Text="Save and Add More" CommandName="Add" runat="server"
                                                    CssClass="ButtonGray" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--<FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />--%>
                                </asp:GridView>
                                <br />
                                <p class="NoteHr">
                                    <asp:Label ID="Label7" runat="server" Text="Language Proficiency - Other Skills:"></asp:Label>
                                </p>
                                <div class="clearfix">
                                    <div class="clearfix sec3C FormContainerBox PixelLabelFix">
                                        <div class="CandidateLeftcol clearfix">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label8" runat="server" Text="*Strong English Communication skills/Team Player/some business customer facing"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtLanguage" runat="server" MaxLength="1" TabIndex="0" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);" />&nbsp;

                                                    <asp:Label ID="Label18" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                    <asp:Label ID="txt_txtLanguage" runat="server" CssClass="ClassDisplayLabel"></asp:Label><br />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="CandidateLeftcol clearfix">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label9" runat="server" Text="*Disciplined(e.g follow maintenance schedules; today is a Tuesday so i must do x,y & z.)"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtCompliance" runat="server" MaxLength="1" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);" />&nbsp;

                                                    <asp:Label ID="txt_txtCompliance" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                    <asp:Label ID="Label17" runat="server" SkinID="lblError" Text="" TabIndex="1"></asp:Label><br />
                                                    <%--   <asp:RegularExpressionValidator ID="regexCompliance" runat="server" ErrorMessage="RegularExpressionValidator"
                                                    Text="Enter Rating as per legend" ControlToValidate="txtCompliance" ValidationExpression="^[01234]$"></asp:RegularExpressionValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="CandidateLeftcol clearfix">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label10" runat="server" class="bold" Text="*Current Project Knowledge:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtProjectKnowledge" runat="server" TabIndex="2" TextMode="MultiLine" />&nbsp;

                                                    <asp:Label ID="txt_txtProjectKnowledge" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                    <asp:Label ID="Label14" runat="server" SkinID="lblError" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="CandidateLeftcol clearfix">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label11" runat="server" class="bold" Text="*Overall Comments:"></asp:Label>
                                                    <asp:Label ID="Label12" runat="server" class="bold" Text="(Must include risks and areas to probe at a further interview)"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtOverallComments" runat="server" TextMode="MultiLine"
                                                        TabIndex="3" />&nbsp;

                                                    <asp:Label ID="txt_txtOverallComments" runat="server" CssClass="ClassDisplayLabel"></asp:Label>&nbsp;

                                                    <asp:Label ID="Label15" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Label ID="Label13" runat="server" class="label" Text="*Select:"></asp:Label>
                                        <div class="CandidateLeftcol clearfix">
                                            <div class="formrow clearfix InterviewRadio">
                                                <div class="radioLong">
                                                    <asp:RadioButton ID="rbtnMarginal" runat="server" GroupName="test" TabIndex="4" /><label
                                                        class="LabelForRadio" for="rbtnMarginal">Marginal Candidate - relatively high risk</label>
                                                    <asp:RadioButton ID="rbtnAverage" runat="server" GroupName="test" Checked="true"
                                                        TabIndex="5" /><label class="LabelForRadio" for="rbtnAverage">Average Candidate - normal
                                                        risk</label>
                                                    <asp:RadioButton ID="rbtnStrong" runat="server" TabIndex="6" GroupName="test" /><label
                                                        class="LabelForRadio" for="rbtnStrong">Strong Candidate - minimal risk</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ButtonContainer1 clearfix">
                                        <asp:Button ID="btnNextStage" runat="server" Text="Approve and Next Stage" OnClientClick="javascript:return ValidateBlankComments();"
                                            CssClass="ButtonGray mrgnR11" OnClick="btnNextStage_Click" TabIndex="7" />
                                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="ButtonGray mrgnR11" OnClick="btnReject_Click"
                                            OnClientClick="javascript:return ValidateBlankComments();" TabIndex="8" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </section>
        </div>
    </form>
    <div id="loading" style="display: none" title="Please Wait....">
        <center class="LoadingWrap">
                <img class="loadImg" src="../../Images/New%20Design/loader.gif" style="width: 120px;
                height: 128px;" alt="Loading..." />
            </center>
    </div>
</body>
</html>