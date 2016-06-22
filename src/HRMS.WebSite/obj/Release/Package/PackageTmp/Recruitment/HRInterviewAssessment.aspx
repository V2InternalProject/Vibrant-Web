<%@ Page Language="C#" AutoEventWireup="true" Inherits="HRInterviewAssessment" Title="Recruitment System"
    CodeBehind="HRInterviewAssessment.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8; IE=7; IE=EDGE" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="Styles/HRMS.css" />
    <style type="text/css">
        .style1 {
            width: 318px;
        }

        .style2 {
            width: 245px;
        }
    </style>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script type="text/javascript" src="../Scripts/Recruitment/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="../Scripts/Recruitment/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/pop_up_window.js"></script>
    <script type="text/javascript" src="../Scripts/New Design/jquery.selectBox.js"></script>
    <script type="text/javascript" src="../Scripts/Recruitment/HRMS.js"></script>

    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/demo.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New Design/jquery.mmenu.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/common.css" />
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <link href="../Content/New Design/hr.css" rel="stylesheet" />
    <link href="../Content/themes/base/jquery.ui.dialog.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnPrint').click(function () {

                var docprint = window.open("about:blank", "_blank");
                var oTable = document.getElementById("tblMain");
                docprint.document.open();
                docprint.document.write('<html><head><link type="text/css" rel="stylesheet" media="print"   href="../Content/New%20Design/demo.css" />');
                docprint.document.write('<link type="text/css" rel="stylesheet" media="print"   href="../Content/New%20Design/common.css" />');
                docprint.document.write('<link media="print" href="../Content/New%20Design/jquery.selectBox%20(2).css" type="text/css" rel="stylesheet" />');
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
                docprint.close();
                return true;
            });

            if ($('#txtComments').prop('disabled')) {
                //textbox is disabled
                $('#txt_txtComments').text($('#txtComments').val());
                $('#txtComments').hide();
                $('#lblMandatory').hide();

            }
            $('.aspNetDisabled').replaceWith(function () {
                return $('input', this);
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script language="VBScript" type="text/vbscript">
        Function myAlert(title, content)
            MsgBox content, 0,title
        End Function
    </script>
        <script language="javascript" type="text/javascript">
            var title = "";

            function getSelectedRadioValue(buttonGroup) {
                // returns the value of the selected radio button or "" if no button is selected
                var i = getSelectedRadio(buttonGroup);
                if (i == -1) {
                    return "";
                } else {
                    if (buttonGroup[i]) { // Make sure the button group is an array (not just one button)
                        return buttonGroup[i].value;
                    } else { // The button group is just the one button, and it is checked
                        return buttonGroup.value;
                    }
                }
            } // Ends the "getSelectedRadioValue" function

            function PrintDocument() {
            }

            function Validate() {
                var flag = 1;
                var message = '';
                var comments = document.getElementById('<%=txtComments.ClientID %>').value;
            if (comments == "") {
                message = message + '<p>' + 'Please enter comments.' + '</p>';
                document.getElementById('<%=txtComments.ClientID %>').value = "";
                document.getElementById('<%=txtComments.ClientID %>').focus();
                flag = 0;
            }
            if (flag == 1) {
                document.getElementById('<%=tblMain.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }
            else {
                V2hrmsAlert(message, title);
                return false;
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
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '32');
                    $(this).parent().css('z-index', '33');
                }
            });
        }
    </script>
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
            <div align="right" class="InnerContainer">
                <%--<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="confirmPrint" CssClass="ButtonGray"
                    Visible="true" />--%>
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="ButtonGray"
                    Visible="true" />
                    </div>
        <div id="">
            <asp:Label ID="lblSuccess" runat="server" SkinID="lblSuccess" Visible="false"></asp:Label>
        </div>

        <div id="">
            <asp:Image ID="loadImage" ImageUrl="~/Images/loading_animation.gif" Style="display: none"
                runat="server"></asp:Image>
        </div>
                 <div id="tblMain" runat="server" class="MainBody RecH assessment">
                     <div class="rwrap">
                         <h3 class="smartrackH">
                             <asp:Label ID="lblWelcome" runat="server" SkinID="lblWelcome" Visible="true" Text="HR Interview Assessment Form"></asp:Label></h3>
                         <div class="clearfix mrgnT20">
                             <div class="floatL">
                                 <asp:Label ID="Label1" runat="server" class="prefix" for="Employee_Name:" Text="Candidate Name : "></asp:Label>
                                 <asp:Label ID="lblCandidateName" class="suffix" for="ReimbursementEmployeeName" runat="server"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label2" runat="server" class="prefix" for="Employee_Code:" Text="Recruiter Name : "></asp:Label>
                                 <asp:Label ID="lblRecruiterName" class="suffix" for="ReimbursementEmployeeCode" runat="server"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label3" runat="server" Text="Project Department: " class="prefix"
                                     for="Reimbursement_Form_Code:"></asp:Label>
                                 <asp:Label ID="lblDepartment" runat="server" class="suffix" for="FormCode"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label4" runat="server" class="prefix expenselocation" for="Location:"
                                     Text="Position :"></asp:Label>
                                 <asp:Label ID="lblPosition" runat="server" class="suffix expenseLoc" for="Location"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label5" runat="server" Text="Years Of Experience: " class="prefix"
                                     for="Date_Of_Submission:"></asp:Label>
                                 <asp:Label ID="lblTotalExp" runat="server" class="suffix datealign" for="DateOfSubmission"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label6" runat="server" class="prefix" for="Date_Of_Submission:" Text="Relevant Experience : "></asp:Label>
                                 <asp:Label ID="lblRelevantExp" class="suffix datealign" for="DateOfSubmission" runat="server"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label7" runat="server" Text="Notice Period: " class="prefix" for="Date_Of_Submission:"></asp:Label>
                                 <asp:Label ID="lblNoticePeriod" runat="server" class="suffix" for="DateOfSubmission"></asp:Label>
                                 <asp:Label ID="Label9" runat="server" Text="(Days)" class="suffix datealign" for="DateOfSubmission"></asp:Label>
                             </div>
                             <div class="floatL">
                                 <asp:Label ID="Label8" runat="server" Text="Interviewed By: " class="prefix" for="Date_Of_Submission:"></asp:Label>
                                 <asp:Label ID="lblInterviewedBy" runat="server" class="suffix datealign" for="DateOfSubmission"></asp:Label>
                             </div>
                         </div>
                         <table cellpadding="0" cellspacing="0" border="0" width="100%" class="TableJqgrid ExperienceTable">
                             <thead>
                                 <tr>
                                     <th class="tableHeaders" width="20%">
                                     </th>
                                     <th class="tableHeaders" width="20%">
                                         <asp:Label ID="lblNotSuitable" runat="server" Text="Not Suitable"></asp:Label>
                                     </th>
                                     <th class="tableHeaders" width="10%">
                                         <asp:Label ID="lblLow" runat="server" Text="Low"></asp:Label>
                                     </th>
                                     <th class="tableHeaders" width="20%">
                                         <asp:Label ID="lblMarginal" runat="server" Text="Marginal"></asp:Label>
                                     </th>
                                     <th class="tableHeaders" width="10%">
                                         <asp:Label ID="lblStrong" runat="server" Text="Strong"></asp:Label>
                                     </th>
                                     <th class="tableHeaders" width="20%">
                                         <asp:Label ID="lblOutstanding" runat="server" Text="Outstanding"></asp:Label>
                                     </th>
                                 </tr>
                             </thead>
                             <tbody>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblPersonality" runat="server" Text="Personality / Appearance"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton11" runat="server" GroupName="rdoPersonalityGroup" />
                                         <%-- <asp:label ID="Accept"  runat="server" AssociatedControlID="RadioButton11"></asp:label>--%>
                                         <%--<asp:label ID="Accept" runat="server" for="Accept" class="LabelForRadio" AssociatedControlID="RadioButton11"></asp:label>--%>
                                         <label class="LabelForRadio" for="RadioButton11">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton12" runat="server" GroupName="rdoPersonalityGroup" />
                                         <label class="LabelForRadio" for="RadioButton12">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton13" Checked="True" runat="server" GroupName="rdoPersonalityGroup" />
                                         <label class="LabelForRadio" for="RadioButton13">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton14" runat="server" GroupName="rdoPersonalityGroup" />
                                         <label class="LabelForRadio" for="RadioButton14">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton15" runat="server" GroupName="rdoPersonalityGroup" />
                                         <label class="LabelForRadio" for="RadioButton15">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblClarityOfThought" runat="server" Text="Clarity Of Thought"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton21" runat="server" GroupName="rdoClarityGroup" />
                                         <label class="LabelForRadio" for="RadioButton21">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton22" runat="server" GroupName="rdoClarityGroup" />
                                         <label class="LabelForRadio" for="RadioButton22">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton23" Checked="True" runat="server" GroupName="rdoClarityGroup" />
                                         <label class="LabelForRadio" for="RadioButton23">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton24" runat="server" GroupName="rdoClarityGroup" />
                                         <label class="LabelForRadio" for="RadioButton24">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton25" runat="server" GroupName="rdoClarityGroup" />
                                         <label class="LabelForRadio" for="RadioButton25">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblLeadershipCapabilities" runat="server" Text="Leadership Capabilities"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton31" runat="server" GroupName="rdoLeadershipGroup" />
                                         <label class="LabelForRadio" for="RadioButton31">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton32" runat="server" GroupName="rdoLeadershipGroup" />
                                         <label class="LabelForRadio" for="RadioButton32">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton33" Checked="True" runat="server" GroupName="rdoLeadershipGroup" />
                                         <label class="LabelForRadio" for="RadioButton33">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton34" runat="server" GroupName="rdoLeadershipGroup" />
                                         <label class="LabelForRadio" for="RadioButton34">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton35" runat="server" GroupName="rdoLeadershipGroup" />
                                         <label class="LabelForRadio" for="RadioButton35">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblInterpersonalSkills" runat="server" Text="Interpersonal Skills"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton41" runat="server" GroupName="rdoInterPersonalGroup" />
                                         <label class="LabelForRadio" for="RadioButton41">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton42" runat="server" GroupName="rdoInterPersonalGroup" />
                                         <label class="LabelForRadio" for="RadioButton42">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton43" Checked="True" runat="server" GroupName="rdoInterPersonalGroup" />
                                         <label class="LabelForRadio" for="RadioButton43">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton44" runat="server" GroupName="rdoInterPersonalGroup" />
                                         <label class="LabelForRadio" for="RadioButton44">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton45" runat="server" GroupName="rdoInterPersonalGroup" />
                                         <label class="LabelForRadio" for="RadioButton45">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblCommunication" runat="server" Text="Communication"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton51" runat="server" GroupName="rdoCommunicationGroup" />
                                         <label class="LabelForRadio" for="RadioButton51">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton52" runat="server" GroupName="rdoCommunicationGroup" />
                                         <label class="LabelForRadio" for="RadioButton52">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton53" Checked="True" runat="server" GroupName="rdoCommunicationGroup" />
                                         <label class="LabelForRadio" for="RadioButton53">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton54" runat="server" GroupName="rdoCommunicationGroup" />
                                         <label class="LabelForRadio" for="RadioButton54">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton55" runat="server" GroupName="rdoCommunicationGroup" />
                                         <label class="LabelForRadio" for="RadioButton55">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblInitiative" runat="server" Text="Initiative / Motivation"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton61" runat="server" GroupName="rdoInitiativeGroup" />
                                         <label class="LabelForRadio" for="RadioButton61">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton62" runat="server" GroupName="rdoInitiativeGroup" />
                                         <label class="LabelForRadio" for="RadioButton62">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton63" Checked="True" runat="server" GroupName="rdoInitiativeGroup" />
                                         <label class="LabelForRadio" for="RadioButton63">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton64" runat="server" GroupName="rdoInitiativeGroup" />
                                         <label class="LabelForRadio" for="RadioButton64">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton65" runat="server" GroupName="rdoInitiativeGroup" />
                                         <label class="LabelForRadio" for="RadioButton65">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="tableRows">
                                     <td>
                                         <asp:Label ID="lblCareerProgression" runat="server" Text="Career Progression"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton71" runat="server" GroupName="rdoCareerGroup" />
                                         <label class="LabelForRadio" for="RadioButton71">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton72" runat="server" GroupName="rdoCareerGroup" />
                                         <label class="LabelForRadio" for="RadioButton72">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton73" Checked="True" runat="server" GroupName="rdoCareerGroup" />
                                         <label class="LabelForRadio" for="RadioButton73">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton74" runat="server" GroupName="rdoCareerGroup" />
                                         <label class="LabelForRadio" for="RadioButton74">
                                         </label>
                                     </td>
                                     <td>
                                         <asp:RadioButton ID="RadioButton75" runat="server" GroupName="rdoCareerGroup" />
                                         <label class="LabelForRadio" for="RadioButton75">
                                         </label>
                                     </td>
                                 </tr>
                                 <tr class="FooterRow">
                                     <td colspan="3">
                                     </td>
                                 </tr>
                             </tbody>
                         </table>
                         <div class="clearfix">
                             <div class="clearfix sec3C FormContainerBox">
                                 <div class="CandidateLeftcol clearfix mrgnB30">
                                     <div class="leftcol">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv">
                                                 <asp:Label ID="lblComments" runat="server" Text="*HR Manager Comments :"></asp:Label>
                                             </div>
                                             <div class="InputDiv">
                                                 <b>
                                                     <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="200"
                                                         TabIndex="0"></asp:TextBox></b>
                                                 <asp:Label ID="txt_txtComments" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                 <asp:Label ID="lblMandatory" runat="server" SkinID="lblError" Text=""></asp:Label>
                                             </div>
                                         </div>
                                     </div>
                                 </div>
                             </div>
                         </div>
                     </div>
                     <div class="ButtonContainer1 clearfix">
                         <asp:Button ID="btnApprove" runat="server" Text="Approve and Next Stage" OnClick="btnSubmitAssessment_Click"
                             OnClientClick="javascript:return Validate()" TabIndex="1" CssClass="ButtonGray" />
                         <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click"
                             OnClientClick="javascript:return Validate()" TabIndex="2" CssClass="ButtonGray" />
                     </div>
                 </div>
        </section>
        </div>
    </form>
    <div id="loading" style="display: none" title="Please Wait....">
        <center class="LoadingWrap">
            <img class="loadImg" src="../../Images/New%20Design/loader.gif" style="width: 120px; height: 128px;"
                alt="Loading..." />
        </center>
    </div>
</body>
</html>