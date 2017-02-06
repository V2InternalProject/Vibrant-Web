<%@ Page Language="C#" AutoEventWireup="true" Inherits="RRFRequestor"
    MasterPageFile="../Views/Shared/HRMS.master" CodeBehind="RRFRequestor.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>--%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script src="../Scripts/Recruitment/HRMS.js" type="text/javascript"></script>
    <script language="VBScript" type="text/vbscript">
        Function myAlert(title, content)
        MsgBox content, 0,title
        End Function
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/demo.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/common.css" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <%--    <script type='text/javascript'
    src='http://jqueryjs.googlecode.com/files/jquery-1.3.2.min.js'>
    </script>--%>
    <script type="text/javascript">
        //$(function () {
        //    $('input[id$=txtExpectedClosureDate]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtPositionsRequired]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtExperience]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtIndicativePanel1]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtIndicativePanel2]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtIndicativePanel3]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //    $('input[id$=txtReplacementFor]').bind('cut copy paste', function (e) {
        //        e.preventDefault();
        //    });
        //});

        $(document).ready(function () {
            var MaxLength = 100;
            $('#txtKeySkills').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });
        $(function () {
            $('select').selectBox();
            $('.sbOptions a').hover(function () {
            $(this).parent().toggleClass("hoveroption");
            });

            //$('select').selectBox({
            //    hideOnWindowScroll: true,
            //    keepInViewport: false
            //});
        });
    </script>
    <%--    <script type="text/javascript">
        $(function () {
            $("message").dialog({ buttons: { "Ok": function () { $(this).dialog("close"); } } }, "open");
        });
    </script>--%>
    <%--    <script type="text/javascript" src="C:\Users\suraj.podval\Documents\Visual Studio 2010\Projects\RRFApprover\RRFApprover\Scripts\jquery.alerts.js"></script>--%>
    <script language="javascript" type="text/javascript">

        var title = "";
        function NumberOnly() {
            var AsciiValue = event.keyCode
            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
                event.returnValue = true;
            else
                event.returnValue = false;
        }

        function Validate() {
            var flag = 1;
            var message = '';
            var num1 = document.getElementById('<%=txtPositionsRequired.ClientID %>');
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
            var approver = document.getElementById('<%=ddlApproverName.ClientID %>').value;
            var SLA = document.getElementById('<%=ddlSLAForTechnology.ClientID %>').value;

            if (num1.value > 30 || num1.value <= 0 || num1.value == "" || !num1.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(num1.value.toString()))) {
                message = '<p>' + 'Please Enter Positions Required Value between 1 to 30' + '</p>';
                document.getElementById('<%=txtPositionsRequired.ClientID %>').value = "";
                document.getElementById('<%=txtPositionsRequired.ClientID %>').focus();
                flag = 0;
            }
            if (num2.value > 50 || num2.value < 0 || num2.value == "" || !num2.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(num2.value.toString()))) {
                message = message + '<p>' + 'Please Enter Experience Value between 0 to 50' + '</p>';
                document.getElementById('<%=txtExperience.ClientID %>').value = "";
                document.getElementById('<%=txtExperience.ClientID %>').focus();
                flag = 0;
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

            if (approver == "Select") {
                message = message + '<p>' + 'Please Select an Approver' + '</p>';
                document.getElementById('<%=ddlApproverName.ClientID %>').focus();
                flag = 0;
            }

            if (SLA == "Select") {
                message = message + '<p>' + 'Please Select an SLA For' + '</p>';
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
            var txtKeySkills = document.getElementById('<%=txtKeySkills.ClientID %>');
            if (txtKeySkills.value.length > 100) {
                message = message + '<p>' + "KeySkills character limit is 100" + '</p>';
                flag = 0;
            }

            if (flag == 1) {

                document.getElementById('<%=pnlRRFRequestor.ClientID %>').style.display = 'none';
                document.getElementById('<%=btnBack.ClientID %>').style.display = 'none';

                DisplayLoadingDialog();
                // document.getElementById('<%=loadImage.ClientID %>').style.visibility = 'visible';
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }
            else {
                V2hrmsAlert(message, title);
                //alert(message);
                //    myAlert("Recruitment Module", message);
                return false;
            }
        }
        //        function textboxMultilineMaxNumber(txt)
        //        {
        //            if (txt.value.length > 100)
        //                txt.value = txt.substr(0, 100);
        //                V2hrmsAlert("character Limit is 100", title);
        //                return false;

        //          }

        //function MessageTextResult() {

        //    alert("RRF Created Successfully.");
        //}
        function checkTextAreaMaxLength(textBox, e, length) {
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                    {
                        e.returnValue = false;
                        V2hrmsAlert("character Limit is 100", title);
                        return false;
                    }
                    else//Firefox
                    {
                        e.preventDefault();
                        V2hrmsAlert("character Limit is 100", title);
                    }
                }
            }
        }

        function checkSpecialKeys(e) {

            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
    </script>
    <body class="AttendancePage">
        <div id="page">
            <%-- <section class="clearfix">
                <header id="header">
                    <div class="SideMenuConBorderR">
                        <a href="#menu" id="SlideMenuBtn"></a>
                    </div>
                    <h1>Vibrant Web</h1>
                    <div class="UserLogout">
                        <div class="ImgConBorderL">
                            <img src="" alt="logout" />
                        </div>
                        <div class="ImgConBorderL">
                            <img src="" alt="user" />
                        </div>
                        <p class="floatR mrgnR15"></p>
                    </div>
                </header>
            </section>--%>
            <section class="ConfirmationContainer Container">
                <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
                <div class="MainBody hrmF RequestorADDNEW forerrorRRF">
                    <div class="InnerContainer">
                        <div class="clearfix">
                            <asp:Button ID="btnBack" OnClientClick="javascript:history.go(-1);return false;"
                                runat="server" Text="Back" CssClass="ButtonGray BackLink" />
                        </div>
                        <div class="clearfix">

                            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" runat="server" />
                            <%--  <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">--%>
                            <div>
                                <div class="clearfix">
                                    <asp:Button ID="btnRedirect" Visible="false" Width="100px" runat="server" Text="Back"
                                        OnClick="btnRedirect_Click" CssClass="ButtonGray BackLink" />
                                </div>
                                <h3 class="smartrackH">
                                    <asp:Label SkinID="lblWelcome" runat="server" Visible="true" ID="pageTitle">RRF Requestor Screen</asp:Label>
                                </h3>
                            </div>

                            <div id="">
                                <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>
                            </div>
                            <div id="" class="rffnote">
                                <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Red" Text="RRF Created Successfully"
                                    Visible="false" SkinID="lblSuccess"></asp:Label>
                            </div>
                            <%--   </table>--%>
                            <asp:Panel ID="pnlRRFRequestor" runat="server">
                                <%-- <table class="tableBorder" width="98%" align="center" border="0" cellpadding="0"
                                cellspacing="10">

                                <table width="98%" align="center" border="0" cellpadding="0" cellspacing="10">--%>

                                <div class="sec3C FormContainerBox">
                                    <%-- <div class="CandidateLeftcol clearfix mrgnB30">--%>
                                    <div class="clearfix">
                                        <div class="colOneThird">
                                            <div class="clearfix formcol">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblRequestDate" runat="server" Text="Request Date"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtRequestDate" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblExpectedClosureDate" runat="server" Text="Expected Closure Date"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtExpectedClosureDate" runat="server" onkeydown="return false" Width="200px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgbtnExpectedClosureDate" ImageAlign="AbsMiddle" CssClass="ui-datepicker-trigger mrgnR12" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                    <ajaxToolkit:CalendarExtender ID="calEExpectedClosureDate" runat="server" TargetControlID="txtExpectedClosureDate"
                                                        PopupButtonID="imgbtnExpectedClosureDate" Format="MM/dd/yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblProjectName" runat="server" Text="Project Name"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtProjectName" runat="server" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--</div>--%>

                                        <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label1" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblForDU" runat="server" Text="For DU"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlForDU" runat="server" OnSelectedIndexChanged="ddlForDU_SelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblForDT" runat="server" Text="For DT"></asp:Label>
                                                </div>

                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlForDT" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label2" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblDesignation" runat="server" Text="Designation"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlDesignation" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%--</div>--%>

                                        <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblResourcePool" runat="server" Text="Resource Pool"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlResourcePool" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label3" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblPositionsRequired" runat="server" Text="Positions Required"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtPositionsRequired" MaxLength="2" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">

                                                    <span class="hiddenstar">*</span><asp:Label ID="lblIndicativePanel11" runat="server" Text="Indicative Panel"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel1" runat="server" autocomplete="off">
                                                    </asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtIndicativePanel1"
                                                        MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                        ServiceMethod="GetEmployeeName1" OnClientShowing="aceResetPosition" />

                                                    <p>
                                                        <asp:Label ID="lblIndicativePanel1" runat="server" ForeColor="Red" SkinID="lblError" CssClass="exampleNumber" Text="Enter Correct Value"
                                                            Visible="false"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <%--</div>--%>

                                        <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblIndicativePanel22" runat="server" Text="Indicative Panel"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel2" runat="server" autocomplete="off">
                                                    </asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtIndicativePanel2"
                                                        MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                        ServiceMethod="GetEmployeeName2" OnClientShowing="aceResetPosition" />

                                                    <p>
                                                        <asp:Label ID="lblIndicativePanel2" runat="server" ForeColor="Red" CssClass="exampleNumber" SkinID="lblError" Text="Enter Correct Value"
                                                            Visible="false"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblIndicativePanel33" runat="server" Text="Indicative Panel"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel3" runat="server" autocomplete="off">
                                                    </asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="txtIndicativePanel3"
                                                        MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                        ServiceMethod="GetEmployeeName3" OnClientShowing="aceResetPosition" />

                                                    <p>
                                                        <asp:Label ID="lblIndicativePanel3" runat="server" ForeColor="Red" CssClass="exampleNumber" SkinID="lblError" Text="Enter Correct Value"
                                                            Visible="false"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblEmploymentType" runat="server" Text="Employment Type"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlEmploymentType" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- </div>--%>

                                        <%-- <div class="CandidateLeftcol clearfix mrgnB30">--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblIsReplacement" runat="server" Text="Is Replacement"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:RadioButtonList ID="rdobtnIsReplacement" runat="server" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="rdobtnIsReplacement_SelectedIndexChanged" AutoPostBack="True"
                                                        CssClass="RadioButtonList">
                                                        <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">

                                                    <asp:Label ID="lblReplacementFor" runat="server" Text="*Replacement For" Visible="false"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtReplacementFor" runat="server" Visible="false"> </asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="txtReplacementFor"
                                                        MinimumPrefixLength="3" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                                        ServiceMethod="GetEmployeeName4" />
                                                    <asp:Label ID="lblReplacementMandatory" runat="server" Text="" Visible="false" SkinID="lblError"></asp:Label>
                                                    <asp:Label ID="lblReplacement" runat="server" CssClass="exampleNumber" SkinID="lblError" Text="Enter Correct Value"
                                                        Visible="false"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
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

                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label5" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblExperience" runat="server" Text="Experience"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtExperience" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--  </div>--%>

                                        <%-- <div class="clearfix">--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label8" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblSLA" runat="server" Text="SLA For"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlSLAForTechnology" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- <div class="colOneThird">
                                                    <div class="formcol clearfix">
                                                        <div class="LabelDiv">
                                                               <asp:Label ID="lblTotalSLADays" runat="server"  Visible="false">Total SLA Days</asp:Label>
                                                        </div>
                                                        <div class="InputDiv">
                                                            <asp:TextBox ID="lblSLADays" runat="server" Text="" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label7" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblApproverName" runat="server" Text="Approver"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:DropDownList ID="ddlApproverName" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%--</div>--%>
                                    </div>

                                    <div class="TeaxtareaContainer clearfix">
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label4" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblKeySkills" runat="server" Text="Key Skills"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtKeySkills" runat="server" TextMode="MultiLine" MaxLength="100"
                                                        onkeyDown="return checkTextAreaMaxLength(this,event,'100')" Height="100px"></asp:TextBox>

                                                    <div class="ClassTextareaDiv"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="Label6" runat="server" SkinID="lblError">*</asp:Label>
                                                    <asp:Label ID="lblBuisnessJustification" runat="server" Text="Business Justification"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtBuisnessJustification" runat="server" TextMode="MultiLine" MaxLength="200"
                                                        onkeyDown="return checkTextAreaMaxLength(this,event,'100')" Height="100px"></asp:TextBox>

                                                    <div class="ClassTextareaDiv"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="colOneThird">
                                            <div class="formcol clearfix">
                                                <div class="LabelDiv">
                                                    <span class="hiddenstar">*</span><asp:Label ID="lblAdditionalInformation" runat="server" Text="Additional Information"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtAdditionalInformation" runat="server" TextMode="MultiLine" Rows="3"
                                                        onkeyDown="return checkTextAreaMaxLength(this,event,'100')" Height="100px" cols="57"></asp:TextBox>
                                                    <div class="ClassTextareaDiv"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%-- </table>
                            </table>--%>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="ButtonContainer1 clearfix">
                        <asp:Button ID="btnSendForApproval" runat="server" Text="Send For Approval" OnClick="btnSendForApproval_Click"
                            Width="180px" OnClientClick="javascript:return Validate()" CssClass="ButtonGray" />

                        <asp:Button ID="btnCancel" Visible="false" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="ButtonGray" />
                    </div>
                </div>
            </section>
        </div>
    </body>
</asp:Content>