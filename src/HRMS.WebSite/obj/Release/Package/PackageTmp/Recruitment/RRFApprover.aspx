<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.master" AutoEventWireup="true" Inherits="RRFApprover" CodeBehind="RRFApprover.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/demo.css" rel="stylesheet" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <script language="VBScript" type="text/vbscript">
        Function myAlert(title, content)
            MsgBox content, 0,title
        End Function
    </script>

    <script language="javascript" type="text/javascript">
        var title = "";
        function NumberOnly() {
            var AsciiValue = event.keyCode
            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 46) || (AsciiValue == 8 || AsciiValue == 127))
                event.returnValue = true;
            else
                event.returnValue = false;
        }

        function Validate(obj) {
            var flag = 1;
            var message = '';
            var num1 = document.getElementById('<%=txtPositionsRequired.ClientID %>');

            var filter1 = /^\d{1,3}(\.\d{1,2})?$/;

            //            var Budget = document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').value;
            var num2 = document.getElementById('<%=txtExperience.ClientID %>');
            var keyskills = document.getElementById('<%=txtKeySkills.ClientID %>').value;
            var businessjustification = document.getElementById('<%=txtBuisnessJustification.ClientID %>').value;
            var name;
            var optn = document.getElementById("<%=rdobtnIsReplacement.ClientID %>");
            var radioButtons = optn.getElementsByTagName('input');

            if (radioButtons[0].checked)
                name = document.getElementById('<%=txtReplacementFor.ClientID %>').value;
            else
                name = "hello";

            var DU = document.getElementById('<%=ddlForDU.ClientID %>').value;
            var desig = document.getElementById('<%=ddlDesignation.ClientID %>').value;
            var SLA = document.getElementById('<%=ddlSLAForTechnology.ClientID %>').value;

            if (num1.value > 30 || num1.value <= 0 || num1.value == "" || !num1.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(num1.value.toString()))) {
                message = '<p>' + 'Please Enter Positions Required Value between 1 to 30' + '</p>';
                document.getElementById('<%=txtPositionsRequired.ClientID %>').value = "";
                document.getElementById('<%=txtPositionsRequired.ClientID %>').focus();
                flag = 0;
            }
            if (num2.value > 50 || num2.value <= 0 || num2.value == "" || !num2.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(num2.value.toString()))) {
                message = message + '<p>' + 'Please Enter Experience Value between 0 to 50';
                document.getElementById('<%=txtExperience.ClientID %>').value = "";
                document.getElementById('<%=txtExperience.ClientID %>').focus();
                flag = 0;
            }

            if (obj == "1") {
                var num3 = document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').value;

                if (num3 <= 0 || num3 == "") {
                    message = '<p>' + 'Please Enter Correct Budget Value' + '</p>';
                    document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').value = "";
                    document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').focus();
                    flag = 0;
                }

                else if (!filter1.test(num3)) {
                    message = message + '<p>' + 'Invalid budget format' + '</p>';
                    document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').value = "";
                    document.getElementById('<%=txtBudgetPerVacancy.ClientID %>').focus();
                    flag = 0;
                }
        }
        if (name == "") {
            message = message + '<p>' + 'Please Select a Name for Replacement' + '</p>';
            document.getElementById('<%=txtReplacementFor.ClientID %>').value = "";
            document.getElementById('<%=txtReplacementFor.ClientID %>').focus();
            flag = 0;
        }

        if (keyskills == "") {
            message = message + '<p>' + 'Please Enter Key Skills required' + '</p>';
            document.getElementById('<%=txtKeySkills.ClientID %>').value = "";
                document.getElementById('<%=txtKeySkills.ClientID %>').focus();
                flag = 0;
            }

            if (businessjustification == "") {
                message = message + '<p>' + 'Please Enter Business Justification' + '</p>';
                document.getElementById('<%=txtBuisnessJustification.ClientID %>').value = "";
                document.getElementById('<%=txtBuisnessJustification.ClientID %>').focus();
                flag = 0;
            }

            if (DU == "Select") {
                message = message + '<p>' + 'Please Select a Delivery Unit' + '</p>';
                document.getElementById('<%=ddlForDU.ClientID %>').focus();
                flag = 0;
            }

            if (desig == "Select") {
                message = message + '<p>' + 'Please Select a Designation' + '</p>';
                document.getElementById('<%=ddlDesignation.ClientID %>').focus();
                flag = 0;
            }

            if (SLA == "Select") {
                message = message + '<p>' + 'Please Select a SLA For' + '</p>';
                document.getElementById('<%=ddlSLAForTechnology.ClientID %>').focus();
                flag = 0;
            }

            var ExpectedClosureDate = document.getElementById('<%=txtExpectedClosureDate.ClientID %>');
            if (ExpectedClosureDate) {
                var expireOnDate = ExpectedClosureDate.value;
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
                if (ExpectedClosureDate.value != "") {
                    if (ExpectedClosureDate) {
                        if (strDate < today) {
                            message = message + '<p>' + "ExpectedClosureDate should be greater than equal to current date" + '</p>';
                            flag = 0;
                        }
                    }

                }
            }

            if (flag == 1) {

                document.getElementById('<%=pnlRRFApprover.ClientID %>').style.display = 'none';
                document.getElementById('<%=btnBack.ClientID %>').style.display = 'none';
                document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;

            }
            else {

                V2hrmsAlert(message, title);

                return false;
            }
        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function chkBudget() {

        }
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" runat="server" />
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody Candidate ViewRRFMultiple clearfix HRM">
            <div id="Div1" class="clearfix">
                <asp:Button ID="btnBack" OnClientClick="javascript:history.go(-1);return false;"
                    Width="100px" runat="server" Text="Back" CssClass="BackLink ButtonGray" />
                <asp:Button ID="btnRedirect" Width="100px" runat="server" Text="Back" Visible="false"
                    OnClick="btnRedirect_Click" CssClass="BackLink ButtonGray" />
            </div>
            <div class="InnerContainer">
                <h3 class="smartrackH">
                    <asp:Label ID="lblTitle" runat="server" Visible="true" Text="RRF Approver Screen"
                        SkinID="lblWelcome"></asp:Label></h3>
                <div id="">
                    <asp:Image ID="loadImage" ImageUrl="../Images/New%20Design/loader.GIF" Style="display: none" runat="server"></asp:Image>
                </div>

                <div class="ErrorMaster">
                    <asp:Label ID="lblSuccessMessage" runat="server" Text="RRF Approved Successfully"
                        SkinID="lblSuccess" Visible="false"></asp:Label>
                </div>
            </div>

            <section class="">
                <asp:Panel ID="pnlRRFApprover" runat="server" Width="100%">
                    <asp:Panel ID="pnlHRM" runat="server">
                        <div class="FormContainerBox">
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblRequestor" runat="server" Text="Requestor"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtRequestor" Enabled="false" runat="server" ReadOnly="true" TabIndex="0"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblRRFNo" runat="server" Text="RRF No"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtRRFNo" Enabled="false" runat="server" ReadOnly="true" TabIndex="1"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblRequestDate" runat="server" Text="Request Date" ReadOnly="true"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtRequestDate" Enabled="false" runat="server" Text="" ReadOnly="true"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblExpectedClosureDate" runat="server" Text="Expected Closure Date"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtExpectedClosureDate" runat="server" TabIndex="3" Width="153px"
                                                    onkeydown="return false"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnExpectedClosureDate" runat="server" CssClass="ui-datepicker-trigger" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                <ajaxToolkit:CalendarExtender ID="calEExpectedClosureDate" runat="server" TargetControlID="txtExpectedClosureDate"
                                                    PopupButtonID="imgbtnExpectedClosureDate" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblProjectName" runat="server" Text="Project Name"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtProjectName" runat="server" MaxLength="30" TabIndex="4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblForDU" runat="server" Text="*For DU"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlForDU" runat="server" OnSelectedIndexChanged="ddlForDU_SelectedIndexChanged"
                                                    AutoPostBack="True" TabIndex="5">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label1" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblForDT" runat="server" Text="For DT"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlForDT" runat="server" TabIndex="6">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span>
                                                <asp:Label ID="lblIndicativePanel11" runat="server" Text="Indicative Panel"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtIndicativePanel1" runat="server" TabIndex="10"
                                                    autocomplete="off"> </asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtIndicativePanel1"
                                                    MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetEmployeeName1" OnClientShowing="aceResetPosition" />

                                                <asp:Label ID="lblIndicativePanel1" CssClass="exampleNumber" runat="server" Text="Enter Correct Value" SkinID="lblError"
                                                    Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblDesignation" runat="server" Text="*Designation"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlDesignation" runat="server" TabIndex="7">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label2" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblResourcePool" runat="server" Text="Resource Pool"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlResourcePool" runat="server" TabIndex="8">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblIndicativePanel22" runat="server" Text="Indicative Panel"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtIndicativePanel2" runat="server" TabIndex="11"
                                                    autocomplete="off"> </asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtIndicativePanel2"
                                                    MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetEmployeeName2" OnClientShowing="aceResetPosition" />

                                                <asp:Label ID="lblIndicativePanel2" CssClass="exampleNumber" runat="server" Text="Enter Correct Value" SkinID="lblError"
                                                    Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblPositionsRequired" runat="server" Text="*Positions Required"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtPositionsRequired" runat="server" MaxLength="2" TabIndex="9"></asp:TextBox>
                                                <asp:Label ID="Label3" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblExperience" runat="server" Text="*Experience"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtExperience" runat="server" MaxLength="2" TabIndex="17"></asp:TextBox>
                                                <asp:Label ID="Label5" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblIndicativePanel33" runat="server" Text="Indicative Panel"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtIndicativePanel3" runat="server" TabIndex="12"
                                                    autocomplete="off"> </asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="txtIndicativePanel3"
                                                    MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetEmployeeName3" OnClientShowing="aceResetPosition" />

                                                <asp:Label ID="lblIndicativePanel3" runat="server" CssClass="exampleNumber" Text="Enter Correct Value" SkinID="lblError"
                                                    Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblEmploymentType" runat="server" Text="Employment Type"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="13">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblIsReplacement" runat="server" Text="Is Replacement"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:RadioButtonList ID="rdobtnIsReplacement" runat="server" RepeatDirection="Horizontal"
                                                    OnSelectedIndexChanged="rdobtnIsReplacement_SelectedIndexChanged" AutoPostBack="True"
                                                    CssClass="RadioButtonList" TabIndex="14">
                                                    <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblReplacementFor" runat="server" Text="Replacement For" Visible="false"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtReplacementFor" runat="server" Visible="false" TabIndex="15"> </asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="txtReplacementFor"
                                                    MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                    ServiceMethod="GetEmployeeName4" />
                                                <asp:Label ID="lblReplacementMandatory" runat="server" Text="*" Visible="false" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblReplacement" runat="server" CssClass="exampleNumber" Text="Enter Correct Value" Visible="false"
                                                    SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C ViewRRF clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblKeySkills" runat="server" Text="*
                                                        Key Skills"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtKeySkills" runat="server" TextMode="MultiLine"
                                                    MaxLength="100" TabIndex="16"></asp:TextBox>
                                                <asp:Label ID="Label4" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <div class="sec1C ViewRRF clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblBuisnessJustification" runat="server" Text="*Business Justification"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtBuisnessJustification" runat="server" TextMode="MultiLine"
                                                    MaxLength="200" TabIndex="18"></asp:TextBox>
                                                <asp:Label ID="Label6" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblAdditionalInformation" runat="server" Text="Additional Information"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtAdditionalInformation" runat="server" TextMode="MultiLine" Rows="3"
                                                    cols="57" TabIndex="19"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearL sec1C ViewRRF clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblBillable" runat="server" Text="Is Billable"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:RadioButtonList ID="rdobtnIsBillable" runat="server" RepeatDirection="Horizontal"
                                                    CssClass="RadioButtonList">
                                                    <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tdComments" class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblBudgetPerVacancy" runat="server" Text="*Budget Per Vacancy (lacs p.a)(e.g. 2.30)"
                                                    onkeypress="return NumberOnly()"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtBudgetPerVacancy" runat="server" onkeypress="return NumberOnly()"
                                                    MaxLength="6" TabIndex="20"></asp:TextBox>
                                                <asp:Label ID="Label8" runat="server"></asp:Label>
                                                <asp:Label ID="Label7" runat="server" SkinID="lblError"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearL sec1C ViewRRF clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblComments" runat="server" Text="Comments" Visible="false"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="100"
                                                    Visible="false" TabIndex="22"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="trBudget" runat="server" class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblSLA" runat="server" Text="*SLA For"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlSLAForTechnology" runat="server"
                                                    TabIndex="21">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label9" runat="server" SkinID="lblError"></asp:Label><br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ButtonContainer1 clearfix">
                            <asp:Button ID="btnApproveandSendtoHR" runat="server" Text="Approve and Send to HR"
                                OnClick="btnApproveandSendtoHR_Click" OnClientClick="javascript:return Validate(1)"
                                TabIndex="22" CssClass="ButtonGray" />
                            <asp:Button ID="btnPushbackRRF" runat="server" Text="Pushback RRF" OnClick="btnPushbackRRF_Click"
                                OnClientClick="javascript:return Validate(2)" TabIndex="23" CssClass="ButtonGray" />
                            <asp:Button ID="btnRejectRRF" runat="server" Text="Reject RRF" OnClick="btnRejectRRF_Click"
                                TabIndex="24" CssClass=" ButtonGray" />
                            <asp:Button ID="btnResendForApproval" runat="server" Text="Resend For Approval" OnClick="btnResendForApproval_Click"
                                Visible="false" CssClass="ButtonGray" OnClientClick="javascript:return Validate(2)" TabIndex="25" />
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>
        </div>
    </section>
</asp:Content>