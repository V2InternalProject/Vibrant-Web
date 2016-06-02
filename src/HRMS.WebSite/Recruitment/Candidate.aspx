<%@ Page Language="C#" AutoEventWireup="true" Inherits="Candidate" MasterPageFile="../Views/Shared/HRMS.Master"
    CodeBehind="Candidate.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Recruitment/UserControl/CandidateDataBank.ascx" TagName="CandidateDataBankTag"
    TagPrefix="CD" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <script src="Scripts/encoder.js" type="text/javascript"></script>--%>
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            //To apply plugin UI to dropdowns
            $('select').selectBox();
        }

        function ApplyClass() {

            $('*[id*=MainContent_grdExperienceDetails_btnAddMoreExperience]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });
            //MainContent_grdEducationDetails_btnAddMoreEducation
            $('*[id*=MainContent_grdEducationDetails_btnAddMoreEducation]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });
            //MainContent_grdCertificationDetails_btnAddMoreCertification
            $('*[id*=MainContent_grdCertificationDetails_btnAddMoreCertification]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });
            $('*[id*=MainContent_CandidateDataBank1_grdCandidateSearch_lnkSearch]').each(function () {

                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });

        }
        $(document).ready(function () {
            //For ToolTip on Dropdowns

            $("#MainContent_ddlCandidateStatus").bind("change", function () {
                $("#MainContent_ddlCandidateStatus").next().attr('title', $("#MainContent_ddlCandidateStatus option:selected").text());
            });

            $("#MainContent_ddlHighestQualification").bind("change", function () {
                $("#MainContent_ddlHighestQualification").next().attr('title', $("#MainContent_ddlHighestQualification option:selected").text());
            });

            $("#MainContent_ddlSource").bind("change", function () {
                $("#MainContent_ddlSource").next().attr('title', $("#MainContent_ddlSource option:selected").text());
            });

            $("#MainContent_ddlCountry").bind("change", function () {
                $("#MainContent_ddlCountry").next().attr('title', $("#MainContent_ddlCountry option:selected").text());
            });

            $("#MainContent_grdExperienceDetails_ddlFooterExpType").bind("change", function () {
                $("#MainContent_grdExperienceDetails_ddlFooterExpType").next().attr('title', $("#MainContent_grdExperienceDetails_ddlFooterExpType option:selected").text());
            });

            $("#MainContent_grdEducationDetails_ddlFooterDegree").bind("change", function () {
                $("#MainContent_grdEducationDetails_ddlFooterDegree").next().attr('title', $("#MainContent_grdEducationDetails_ddlFooterDegree option:selected").text());
            });

            $("#MainContent_grdEducationDetails_ddlFooterCourse").bind("change", function () {
                $("#MainContent_grdEducationDetails_ddlFooterCourse").next().attr('title', $("#MainContent_grdEducationDetails_ddlFooterCourse option:selected").text());
            });

            $("#MainContent_grdEducationDetails_ddlFooterType").bind("change", function () {
                $("#MainContent_grdEducationDetails_ddlFooterType").next().attr('title', $("#MainContent_grdEducationDetails_ddlFooterType option:selected").text());
            });

            $("#MainContent_grdCertificationDetails_ddlFooterCertificationName").bind("change", function () {
                $("#MainContent_grdCertificationDetails_ddlFooterCertificationName").next().attr('title', $("#MainContent_grdCertificationDetails_ddlFooterCertificationName option:selected").text());
            });

            //Dropdowns in Candidate Data Bank tab

            $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlQualification").bind("change", function () {
                $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlQualification").next().attr('title', $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlQualification option:selected").text());
            });

            $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlStatus").bind("change", function () {
                $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlStatus").next().attr('title', $("#MainContent_CandidateDataBank1_grdCandidateSearch_ddlStatus option:selected").text());
            });

            // MainContent_grdExperienceDetails_btnAddMoreExperience
            ApplyClass();

            if ($('#MainContent_txtFirstName').prop('disabled')) {

                //To Hide the File Upload
                $('#btnForCustomUpload').hide();
                $('#FileUploadResumeField').hide();
                $('#MainContent_lblUploadResume').hide();

                //Hide the hidden star
                $('.hiddenstar').hide();

                if ($("#MainContent_pnlCandidate").length != 0) {

                    // Hides all the dropdowns on Candidate page
                    $("#MainContent_ddlCandidateStatus").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlSalutation").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlMaritalStatus").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlGender").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlTotalWorkExpYears").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlTotalWorkExpMonths").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlRelevantWorkExpYears").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlRelevantWorkExpMonths").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlHighestQualification").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlCountry").next().removeClass('sbHolder').addClass('forddlHide');
                    $("#MainContent_ddlSource").next().removeClass('sbHolder').addClass('forddlHide');

                    $('forddlHide').hide();

                    // Disables all the dropdowns inside the grid on Candidate page
                    $("#MainContent_grdExperienceDetails_ddlFooterExpType").next().attr("disabled", "disabled");
                    $("#MainContent_grdEducationDetails_ddlFooterDegree").next().attr("disabled", "disabled");
                    $("#MainContent_grdEducationDetails_ddlFooterCourse").next().attr("disabled", "disabled");
                    $("# MainContent_grdEducationDetails_ddlFooterYear").next().attr("disabled", "disabled");
                    $("#MainContent_grdEducationDetails_ddlFooterType").next().attr("disabled", "disabled");
                    $("#MainContent_grdCertificationDetails_ddlFooterCertificationName").next().attr("disabled", "disabled");
                }

                //textbox is disabled
                $('#MainContent_lbl_txtFirstName').text($('#MainContent_txtFirstName').val());
                $('#MainContent_txtFirstName').hide();
                $('#MainContent_lblFirstNameMandatory').hide();

                $('#MainContent_lbl_txtMiddleName').text($('#MainContent_txtMiddleName').val());
                $('#MainContent_txtMiddleName').hide();

                $('#MainContent_lbl_txtLastName').text($('#MainContent_txtLastName').val());
                $('#MainContent_txtLastName').hide();
                $('#MainContent_lblLastNameMandatory').hide();

                $('#MainContent_lbl_txtDateOfBirth').text($('#MainContent_txtDateOfBirth').val());
                $('#MainContent_txtDateOfBirth').hide();
                $('#MainContent_lblDateOfBirthMandatory').hide();
                $('#MainContent_imgbtnDateOfBirth').hide();

                $('#MainContent_lbl_txtAlternateContactNumber').text($('#MainContent_txtAlternateContactNumber').val());
                $('#MainContent_txtAlternateContactNumber').hide();
                $('#MainContent_lblAlternateContactNumberMandatory').hide();

                $('#MainContent_lbl_txtMobileNumber').text($('#MainContent_txtMobileNumber').val());
                $('#MainContent_txtMobileNumber').hide();
                $('#MainContent_lblMobileNumberMandatory').hide();

                $('#MainContent_lbl_txtEmailID').text($('#MainContent_txtEmailID').val());
                $('#MainContent_txtEmailID').hide();
                $('#MainContent_lblEmailIDMandatory').hide();

                $('#MainContent_lbl_txtAlternateEmailID').text($('#MainContent_txtAlternateEmailID').val());
                $('#MainContent_txtAlternateEmailID').hide();

                $('#MainContent_lbl_txtOtherSkills').text($('#MainContent_txtOtherSkills').val());
                $('#MainContent_txtOtherSkills').hide();
                $('#MainContent_lblMobileNumberMandatory').hide();

                $('#MainContent_lbl_txtPresentAddress').text($('#MainContent_txtPresentAddress').val());
                $('#MainContent_txtPresentAddress').hide();
                $('#MainContent_lblPresentAddressMandatory').hide();

                $('#MainContent_lbl_txtCity').text($('#MainContent_txtCity').val());
                $('#MainContent_txtCity').hide();
                $('#MainContent_lblCityMandatory').hide();

                $('#MainContent_lbl_txtState').text($('#MainContent_txtState').val());
                $('#MainContent_txtState').hide();
                $('#MainContent_lblStateMandatory').hide();

                $('#MainContent_lbl_txtPinCode').text($('#MainContent_txtPinCode').val());
                $('#MainContent_txtPinCode').hide();
                $('#MainContent_lblPinCodeMandatory').hide();

                $('#MainContent_lbl_txtCurrentNoticePeriod').text($('#MainContent_txtCurrentNoticePeriod').val());
                $('#MainContent_txtCurrentNoticePeriod').hide();

                $('#MainContent_lbl_txtCurrentCTC').text($('#MainContent_txtCurrentCTC').val());
                $('#MainContent_txtCurrentCTC').hide();

                $('#MainContent_lbl_txtSourceName').text($('#MainContent_txtSourceName').val());
                $('#MainContent_txtSourceName').hide();
                $('#MainContent_lblSourceNameMandatory').hide();

                $('#MainContent_lbl_txtAreasOfInterest').text($('#MainContent_txtAreasOfInterest').val());
                $('#MainContent_txtAreasOfInterest').hide();

                $('#MainContent_lbl_txtCurrentJobSummary').text($('#MainContent_txtCurrentJobSummary').val());
                $('#MainContent_txtCurrentJobSummary').hide();

                $('#MainContent_lbl_txtRewardsAndRecognition').text($('#MainContent_txtRewardsAndRecognition').val());
                $('#MainContent_txtRewardsAndRecognition').hide();

                $('#MainContent_lbl_txtSpecialAchievements').text($('#MainContent_txtSpecialAchievements').val());
                $('#MainContent_txtSpecialAchievements').hide();

                //assign selected values of dropdown to its respective label
                $('#MainContent_lbl_ddlCandidateStatus').text($('#MainContent_ddlCandidateStatus option:selected').text());

                $('#MainContent_lblCandidateStatusMandatory').hide();

                $('#MainContent_lbl_ddlSalutation').text($('#MainContent_ddlSalutation option:selected').text());
                $('#MainContent_lblSalutationMandatory').hide();

                $('#MainContent_lbl_ddlMaritalStatus').text($('#MainContent_ddlSalutation option:selected').text());
                $('#MainContent_lblMaritalStatusMandatory').hide();

                $('#MainContent_lbl_ddlGender').text($('#MainContent_ddlGender option:selected').text());
                $('#MainContent_lblGenderMandatory').hide();

                $('#MainContent_lbl_ddlTotalWorkExpYears').text($('#MainContent_ddlTotalWorkExpYears option:selected').text());
                $('#MainContent_lbl_ddlTotalWorkExpMonths').text($('#MainContent_ddlTotalWorkExpMonths option:selected').text());
                $('#MainContent_lblTotalWorkExp_Months').show();
                $('#MainContent_lblTotalWorkExp_Years').show();
                $('#MainContent_lblTotalWorkExpYearsMandatory').hide();

                $('#MainContent_lbl_ddlRelevantWorkExpYears').text($('#MainContent_ddlRelevantWorkExpYears option:selected').text());
                $('#MainContent_lbl_ddlRelevantWorkExpMonths').text($('#MainContent_ddlRelevantWorkExpMonths option:selected').text());
                $('#MainContent_lblRelevantWorkExp_Months').show();
                $('#MainContent_lblRelevantWorkExp_Years').show();
                $('#MainContent_lblRelevantWorkExpYearsMandatory').hide();

                $('#MainContent_lbl_ddlHighestQualification').text($('#MainContent_ddlHighestQualification option:selected').text());
                $('#MainContent_lblHighestQualificationMandatory').hide();

                $('#MainContent_lbl_ddlSource').text($('#MainContent_ddlSource option:selected').text());
                $('#MainContent_ddlSource').hide();
                $('#MainContent_lblSourceMandatory').hide();

                $('#MainContent_lbl_ddlCountry').text($('#MainContent_ddlCountry option:selected').text());
                $('#MainContent_ddlCountry').hide();
                $('#MainContent_lblCountryMandatory').hide();

                //assign checked radio buttons' value to respective Label
                if ($('input:radio[id=MainContent_rdobtnUSVisa_0]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnUSVisa').text($('input:radio[id=MainContent_rdobtnUSVisa_0]:checked').val());
                }
                if ($('input:radio[id=MainContent_rdobtnUSVisa_1]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnUSVisa').text($('input:radio[id=MainContent_rdobtnUSVisa_1]:checked').val());
                }

                if ($('input:radio[id=MainContent_rdobtnWillingToRelocate_0]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnWillingToRelocate').text($('input:radio[id=MainContent_rdobtnWillingToRelocate_0]:checked').val());
                }
                if ($('input:radio[id=MainContent_rdobtnWillingToRelocate_1]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnWillingToRelocate').text($('input:radio[id=MainContent_rdobtnWillingToRelocate_1]:checked').val());
                }

                if ($('input:radio[id=MainContent_rdobtnValidPassport_0]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnValidPassport').text($('input:radio[id=MainContent_rdobtnValidPassport_0]:checked').val());
                }
                if ($('input:radio[id=MainContent_rdobtnValidPassport_1]:checked').val() != "undefined") {
                    $('#MainContent_lbl_rdobtnValidPassport').text($('input:radio[id=MainContent_rdobtnValidPassport_1]:checked').val());
                }

                $('.RadioButtonList').hide();

                $('#MainContent_btnSave').hide();

                $('#MainContent_btnCancel').hide();

            }

        });
    </script>
    <script language="VBScript" type="text/vbscript">
        Function myAlert(title, content)
        MsgBox content, 0,title
        End Function
    </script>
    <script language="javascript" type="text/javascript">

        //Browse Button for Uploading Resume
        //            $("#fileUpload").bind("change", function (event) {
        //                $("#FileUploadResumeField").val($(this).val());
        //            });

        function Count(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                V2hrmsAlert('<p>' + " Only " + long + " characters are allowed " + '</p>', 'Recruitment Module');
            }
        }

        function textboxMultilineMaxNumber(txt, maxLen) {
            try {
                if (txt.value.length > (maxLen - 1)) return false;
            }
            catch (e) {
            }
        }

        function NumberOnly(_char, _mozChar) {
            if (_mozChar != null) {
                // if ((_char == 8) || (_mozChar >= 48 && _mozChar <= 57) || (_mozChar == 8 || _mozChar == 127) || (_mozChar == 11))
                if ((_mozChar >= 48 && _mozChar <= 57) || _mozChar == 0 || _char == 8 || _mozChar == 13)
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                    // event.returnValue = true;
                }
            }
            else { // Must be an IE-compatible Browser

                if ((_char >= 48 && _char <= 57) || (_char == 8 || _char == 127))
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                }
            }
            return _RetVal;
        }
        function AlphabetsAndSpecialCharacters(_char, _mozChar) {
            if (_mozChar != null) {
                if (_mozChar < 48 || _mozChar > 57)
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter character only.' + '</p>', 'Recruitment Module');
                    // event.returnValue = true;
                }
            }
            else { // Must be an IE-compatible Browser

                if (_char < 48 || _char > 57)
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter character only.' + '</p>', 'Recruitment Module');
                }
            }
            return _RetVal;
        }

        function AllCharactersWithoutNegative(_char, _mozChar) {
            if (_mozChar != null) {
                if (_mozChar != 45)
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid Percentage.' + '</p>', 'Recruitment Module');

                }
            }
            else { // Must be an IE-compatible Browser

                if (_char != 45)
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid Percentage.' + '</p>', 'Recruitment Module');
                }
            }
            return _RetVal;
        }

        function NumberOnlyWithDot(_char, _mozChar) {

            if (_mozChar != null) {
                if ((_mozChar >= 48 && _mozChar <= 57) || (_mozChar == 8 || _mozChar == 127) || (_mozChar == 46) || (_mozChar == 0)) {

                    return _RetVal = true;
                }
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                    // event.returnValue = true;
                }
            }
            else { // Must be an IE-compatible Browser

                if ((_char >= 48 && _char <= 57) || (_char == 8 || _char == 127) || (_char == 46))
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                }
            }
            return _RetVal;
        }

        //not in use
        function AlphabetsOnlyForNameFields(_char, _mozChar) {
            if (_mozChar != null) {
                if ((_mozChar >= 65 && _mozChar <= 90) || (_mozChar >= 97 && _mozChar <= 122) || (_mozChar == 39) || (_mozChar == 8) || (_mozChar == 13) || (_mozChar == 0))
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                    // event.returnValue = true;
                }
            }
            else { // Must be an IE-compatible Browser

                if ((_char >= 65 && _char <= 90) || (_char >= 97 && _char <= 122) || (_char == 39) || (_char == 8) || (_char == 13) || (_char == 0))
                    return _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter valid data.' + '</p>', 'Recruitment Module');
                }
            }
            return _RetVal;
        }

        function show_confirm() {

            if (confirm("Duplicate entry found. Do you want to edit this record ?") == true) {

                var btnclick = document.getElementById('<%= this.btnAlert.ClientID %>').click();
            }
            else {
                var btnCancelclick = document.getElementById('<%= this.btnAlertCancel.ClientID %>').click();
            }

        }

        function UpdateAlert(msg) {

            alert(msg);
            //var btnRedirectClick = document.getElementById('<%= this.btnRedirect.ClientID %>').click();
            return false;
        }

        function SaveAlert(msg) {

            var btnRedirectClick = document.getElementById('<%= this.btnRedirect.ClientID %>').click();
            return false;
        }

        function SaveAndAddMoreAlert(msg) {

            var btnSaveAndAddMoreRedirectClick = document.getElementById('<%= this.btnSaveAndAddMoreRedirect.ClientID %>').click();
            return false;
        }

        function PopUpWindow() {
            window.open('Candidate.aspx', null, 'height=800, width=800,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ');
        }

        //        var c = 0;
        function DisableClick() {
            document.getElementById('<%=this.btnSave.ClientID %>').disabled = true;

        }

        function validateForm(firstName, lastName, dateOfBirth, candidatestatus, salutation, maritalStatus, gender, alternateContactNo, mobileNo, EmailID, highestQualification, presentAddress, city, pincode, state, Country, source, sourcename, totalworkExpYears, totalworkExpMonths, relevantworkExpYears, relevantworkExpMonths, chkList) {
            //alert(firstName, middleName, lastName, dateOfBirth, candidatestatus, salutation, maritalStatus, gender, alternateContactNo, mobileNo, EmailID, highestQualification, presentAddress, city, pincode, state, currentNoticePeriod, source, sourcename, totalworkExpYears, totalworkExpMonths, relevantworkExpYears, relevantworkExpMonths);
            var title = "";
            var message = '';
            var totalExperience = Number(Number(totalworkExpYears.value) * Number(12)) + Number(totalworkExpMonths.value);
            var relevantExperience = Number(Number(relevantworkExpYears.value) * Number(12)) + Number(relevantworkExpMonths.value);
            var flag = 1;

            if (firstName.value == "") {
                firstName.focus();
                message = '<p>' + 'Please enter first name' + '</p>';
                flag = 0;
            }

            if (lastName.value == "") {
                lastName.focus();
                message = message + '<p>' + 'Please enter last name' + '</p>';
                flag = 0;
            }

            if (dateOfBirth.value == "") {
                dateOfBirth.focus();
                message = message + '<p>' + 'Please enter Birth Date' + '</p>';
                flag = 0;
            }

            if (dateOfBirth.value != "") {
                var expireOnDate = dateOfBirth.value;
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
                if (dateOfBirth.value != "") {
                    if (dateOfBirth) {
                        if (strDate >= today) {
                            message = message + '<p>' + "Birth Date should be less than current date" + '</p>';
                            flag = 0;
                        }
                    }

                }
            }

            if (candidatestatus.value == "" || candidatestatus.value == "none") {
                candidatestatus.focus();
                message = message + '<p>' + 'Please select candidate status' + '</p>';
                flag = 0;
            }

            if (salutation.value == "" || salutation.value == "0") {
                salutation.focus();
                message = message + '<p>' + 'Please select salutation' + '</p>';
                flag = 0;
            }

            if (maritalStatus.value == "" || maritalStatus.value == "0") {
                maritalStatus.focus();
                message = message + '<p>' + 'Please select marital status' + '</p>';
                flag = 0;
            }

            if (gender.value == "" || gender.value == "0") {
                gender.focus();
                message = message + '<p>' + 'Please select gender' + '</p>';
                flag = 0;
            }

            if (alternateContactNo.value == "") {
                alternateContactNo.focus();
                message = message + '<p>' + 'Please enter alternate contact number' + '</p>';
                flag = 0;
            }

            if (mobileNo.value == "") {
                mobileNo.focus();
                message = message + '<p>' + 'Please enter mobile number' + '</p>';
                flag = 0;
            }

            if ((alternateContactNo.value != "") && (alternateContactNo.value.length != 11)) {
                alternateContactNo.focus();
                message = message + '<p>' + 'Please enter 11 digit alternate contact number ' + '</p>';
                flag = 0;
            }

            if (!alternateContactNo.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(alternateContactNo.value.toString()))) {
                alternateContactNo.focus();
                message = message + '<p>' + 'Please enter Numeric Alternate ContactNo' + '</p>';
                flag = 0;
            }

            var CurrentCTC = document.getElementById('<%= this.txtCurrentCTC.ClientID %>');

            if (CurrentCTC != null) {
                var regexpression = new RegExp('^([1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|\.[0-9]{1,2})$');
                if (CurrentCTC.value.trim() != "") {
                    if (!CurrentCTC.value.match(regexpression)) {
                        message = message + '<p>' + 'Please enter correct CTC in following format 123.25' + '</p>';
                        flag = 0;
                    }
                }

            }

            if (CurrentCTC != null) {
                if ((CurrentCTC.value != "") && (CurrentCTC.value > 999)) {
                    CurrentCTC.focus();
                    message = message + '<p>' + 'Please enter Current CTC less than 999' + '</p>';
                    flag = 0;
                }
            }

            if ((mobileNo.value != "" && alternateContactNo.value != "") && (mobileNo.value.length == 11) && (alternateContactNo.value.length == 11) && (alternateContactNo.value == mobileNo.value)) {
                message = message + '<p>' + 'Alternate contact number and mobile number must be different ' + '</p>';
                flag = 0;
            }

            if ((mobileNo.value != "") && (mobileNo.value.length != 11)) {
                mobileNo.focus();
                message = message + '<p>' + 'Please enter 11 digit mobile number ' + '</p>';
                flag = 0;
            }

            if (!mobileNo.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(mobileNo.value.toString()))) {
                mobileNo.focus();
                message = message + '<p>' + 'Please enter Numeric Mobile No' + '</p>';
                flag = 0;
            }

            if (EmailID.value == "") {
                EmailID.focus();
                message = message + '<p>' + 'Please enter EmailID' + '</p>';
                flag = 0;
            }
            if (EmailID.value != "") {
                //                var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
                var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
                var emailid = EmailID.value;
                var matchArray = emailid.match(emailPat);
                if (matchArray == null) {
                    message = message + '<p>' + 'Please enter valid emailID' + '</p>';
                    flag = 0;
                    EmailID.focus();
                }
            }

            if (highestQualification.value == "" || highestQualification.value == "none") {
                highestQualification.focus();
                message = message + '<p>' + 'Please select highest qualification' + '</p>';
                flag = 0;
            }

            if (Country.value == "" || Country.value == "none") {
                Country.focus();
                message = message + '<p>' + 'Please select Country Name' + '</p>';
                flag = 0;
            }

            if (presentAddress.value.trim() == "") {
                presentAddress.focus();
                message = message + '<p>' + 'Please enter present address' + '</p>';
                flag = 0;
            }

            if (city.value.trim() == "") {
                city.focus();
                message = message + '<p>' + 'Please enter city' + '</p>';
                flag = 0;
            }

            if (state.value.trim() == "") {
                state.focus();
                message = message + '<p>' + 'Please enter state' + '</p>';
                flag = 0;
            }

            if (pincode.value == "") {
                pincode.focus();
                message = message + '<p>' + 'Please enter pincode' + '</p>';
                flag = 0;
            }

            var CurrentNoticePeriod = document.getElementById('<%=this.txtCurrentNoticePeriod.ClientID %>');
            if (!CurrentNoticePeriod.value.toString().match(/^[-]?\d*\.?\d*$/) || (/^\d+\.\d+$/.test(CurrentNoticePeriod.value.toString()))) {
                CurrentNoticePeriod.focus();
                message = message + '<p>' + 'Please enter Numeric Current Notice Period' + '</p>';
                flag = 0;
            }

            if (source.value == "" || source.value == "0") {
                source.focus();
                message = message + '<p>' + 'Please select source' + '</p>';
                flag = 0;
            }
            if (sourcename.value.trim() == "") {
                sourcename.focus();
                message = message + '<p>' + 'Please enter source name' + '</p>';
                flag = 0;
            }

            if (totalworkExpYears.value == "" || totalworkExpYears.value == "none") {
                totalworkExpYears.focus();
                message = message + '<p>' + 'Please select total work Experience in Years' + '</p>';
                flag = 0;
            }

            if (totalworkExpMonths.value == "" || totalworkExpMonths.value == "none") {
                totalworkExpMonths.focus();
                message = message + '<p>' + 'Please select total work Experience in Months' + '</p>';
                flag = 0;
            }

            if (relevantworkExpYears.value == "" || relevantworkExpYears.value == "none") {
                relevantworkExpYears.focus();
                message = message + '<p>' + 'Please select relevant work Experience in Years' + '</p>';
                flag = 0;
            }

            if (relevantworkExpMonths.value == "" || relevantworkExpMonths.value == "none") {
                relevantworkExpMonths.focus();
                message = message + '<p>' + 'Please select relevant work Experience in Months' + '</p>';
                flag = 0;
            }
            if ((totalworkExpYears.value != "" && totalworkExpYears.value != "none") && (totalworkExpMonths.value != "" || totalworkExpMonths.value != "none") && (relevantworkExpMonths.value != "" || relevantworkExpMonths.value != "none") && (relevantworkExpYears.value != "" || relevantworkExpYears.value != "none") && ((totalExperience) < (relevantExperience))) {
                message = message + '<p>' + 'Total work Experience must be greater than or equal to Relevant work Experience' + '</p>';
                flag = 0;
            }

            var atLeast = 1;
            var id = 0;
            var flag1 = 0;
            var counter1 = 0;

            while (flag1 == '0') {
                var tempcheckboxid = 'MainContent_chkList_' + id;
                var checkbox = document.getElementById(tempcheckboxid);

                if (checkbox != null) {
                    if (checkbox.checked == true) {
                        counter1 = counter1 + 1;
                    }
                    id++;
                }
                else {
                    flag1 = 1;
                }

            }
            if (atLeast > counter1) {
                message = message + '<p>' + 'Please select atleast  ' + atLeast + ' skill(s)' + '</p>';
                flag = 0;
            }

            if (flag == 1) {
                //  V2hrmsAlert('<p>' + " Candidate Details have been saved successfully. " + '</p>', 'Recruitment Module');
                return true;
            }
            else {

                V2hrmsAlert(message, title);
                return false;
            }
        }

        function validateExperience(Organisation, FromDate, ToDate, ExpType, Designation, ReportingManager, LastDrawnCTCInLacs) {

            var title = "";
            var message = '';
            var flag = 1;
            if (Organisation.value.trim() == "" || Organisation.value == "none") {
                message = '<p>' + 'Please enter the name of Organisation' + '</p>';
                Organisation.focus();
                flag = 0;
            }

            //            if (OfficeLocation.value.trim() == "") {
            //                OfficeLocation.focus();
            //                message = message + '<p>' + 'Please enter the Office Location' + '</p>';
            //                flag = 0;
            //            }

            if (FromDate.value == "") {
                FromDate.focus();
                message = message + '<p>' + 'Please enter From Date' + '</p>';
                flag = 0;
            }

            if (FromDate) {
                var expireOnDate = FromDate.value;
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
                if (FromDate.value != "") {
                    if (FromDate) {
                        if (strDate >= today) {
                            message = message + '<p>' + "From Date should be less than current date" + '</p>';
                            flag = 0;
                        }
                    }

                }
            }

            if (ToDate) {
                var endDate = ToDate.value;
                var pos1 = endDate.indexOf("/");
                var pos2 = endDate.indexOf("/", pos1 + 1);

                var strMonth = eval(endDate.substring(0, pos1) - 1);
                var strDay = endDate.substring(pos1 + 1, pos2);
                var strYear = endDate.substring(pos2 + 1);
                var strDate = new Date();

                strDate.setDate(strDay);
                strDate.setMonth(strMonth);
                strDate.setFullYear(strYear);

                var startDate = FromDate.value;
                var pos1 = startDate.indexOf("/");
                var pos2 = startDate.indexOf("/", pos1 + 1);

                var strMonth = eval(startDate.substring(0, pos1) - 1);
                var strDay = startDate.substring(pos1 + 1, pos2);
                var strYear = startDate.substring(pos2 + 1);
                var FromDate = new Date();

                FromDate.setDate(strDay);
                FromDate.setMonth(strMonth);
                FromDate.setFullYear(strYear);

                if (ToDate.value != "") {
                    if (ToDate) {
                        if (strDate <= FromDate) {
                            message = message + '<p>' + 'To Date should be greater than From Date' + '</p>';
                            flag = 0;
                        }
                    }

                }
            }
            if (ExpType.value == "" || ExpType.value == "none") {
                ExpType.focus();
                message = message + '<p>' + 'Please select Type' + '</p>';
                flag = 0;
            }

            if (Designation.value.trim() == "") {
                Designation.focus();
                message = message + '<p>' + 'Please enter the Designation' + '</p>';
                flag = 0;
            }
            if (ReportingManager.value.trim() == "") {
                ReportingManager.focus();
                message = message + '<p>' + 'Please enter the Reporting Manager' + '</p>';
                flag = 0;
            }

            //            var filter1 = /^\d{1,3}(\.\d{1,2})?$/;
            if (LastDrawnCTCInLacs.value == "") {
                LastDrawnCTCInLacs.focus();
                message = message + '<p>' + 'Please enter the last Drawn CTC in Lacs' + '</p>';
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

        function validateCertification(CertificationName, CertificationNo, Institution, CertifiedOnDate, CertificationScore, CertificationGrade) {

            var title = "";
            var message = '';
            var flag = 1;

            if (CertificationName.value == "" || CertificationName.value == "none") {
                CertificationName.focus();
                message = '<p>' + 'Please select Certification Name' + '</p>';
                flag = 0;
            }

            if (CertificationNo.value.trim() == "") {
                CertificationNo.focus();
                message = message + '<p>' + 'Please enter Certification No' + '</p>';
                flag = 0;
            }

            if (Institution.value.trim() == "") {
                Institution.focus();
                message = message + '<p>' + 'Please enter Institution' + '</p>';
                flag = 0;
            }

            if (CertifiedOnDate.value == "") {
                CertifiedOnDate.focus();
                message = message + '<p>' + 'Please enter Certified On Date' + '</p>';
                flag = 0;
            }

            if (CertifiedOnDate) {
                var certifiedDate = CertifiedOnDate.value;
                var pos1 = certifiedDate.indexOf("/");
                var pos2 = certifiedDate.indexOf("/", pos1 + 1);

                var strMonth = eval(certifiedDate.substring(0, pos1) - 1);
                var strDay = certifiedDate.substring(pos1 + 1, pos2);
                var strYear = certifiedDate.substring(pos2 + 1);
                var strDate = new Date();

                strDate.setDate(strDay);
                strDate.setMonth(strMonth);
                strDate.setFullYear(strYear);

                var today = new Date();
                if (CertifiedOnDate.value != "") {
                    if (CertifiedOnDate) {
                        if (strDate >= today) {
                            message = message + '<p>' + 'Certified On Date should be less than current date' + '</p>';
                            flag = 0;
                        }
                    }

                }
            }

            if (CertificationScore.value == "") {
                CertificationScore.focus();
                message = message + '<p>' + 'Please enter Certification Score' + '</p>';
                flag = 0;
            }

            if (CertificationGrade.value.trim() == "") {
                CertificationGrade.focus();
                message = message + '<p>' + 'Please enter Certification Grade' + '</p>';
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

        function validateEducation(Degree, Course, Specialization, University, Year, Type, Percentage) {

            var title = "";
            var message = '';
            var flag = 1;

            if (Degree.value == "" || Degree.value == "none") {
                Degree.focus();
                message = '<p>' + 'Please select Degree' + '</p>';
                flag = 0;
            }

            if (Course.value == "" || Course.value == "none") {
                Course.focus();
                message = message + '<p>' + 'Please select Course' + '</p>';
                flag = 0;
            }

            if (Specialization.value.trim() == "") {
                Specialization.focus();
                message = message + '<p>' + 'Please enter Specialization' + '</p>';
                flag = 0;
            }

            //if (Institute.value.trim() == "") {
            //    Institute.focus();
            //    message = message + '<p>' + 'Please enter Institute' + '</p>';
            //    flag = 0;
            //}

            if (University.value.trim() == "") {
                University.focus();
                message = message + '<p>' + 'Please enter University' + '</p>';
                flag = 0;
            }

            if (Year.value == "" || Year.value == "none") {
                Year.focus();
                message = message + '<p>' + 'Please select Year' + '</p>';
                flag = 0;
            }
            if (Type.value == "" || Type.value == "none") {
                Type.focus();
                message = message + '<p>' + 'Please select Type' + '</p>';
                flag = 0;
            }
            if (Percentage.value == "") {
                Percentage.focus();
                message = message + '<p>' + 'Please enter Percentage' + '</p>';
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
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody Candidate clearfix">
            <asp:Label ID="lblMsg" SkinID="lblSuccess"
                runat="server"></asp:Label>
            <div class="rwrap">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">Candidate Information</li>
                        <li id="tab2">Candidate Data Bank</li>
                    </ul>
                </div>

                <%--==================================--%>
                <asp:Panel ID="pnlCandidate" runat="server" Width="100%" ScrollBar="None">

                    <section class="add-detailsdata clearfix">

                        <div class="fourColumns">
                            <div class="clearfix">
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblCandidateIDDisplay" Text="Candidate ID:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:Label ID="lblCandidateID" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblFirstNameMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblFirstName" Text="First Name:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtFirstName" OnTextChanged="txtFirstName_TextChanged" AutoPostBack="true"
                                                    onkeypress="return AlphabetsOnlyForNameFields(event.keyCode, event.which)"
                                                    onKeyUp="Count(this,30)" onChange="Count(this,30)"
                                                    MaxLength="30" runat="server" TabIndex="1"></asp:TextBox>
                                                <asp:Label ID="lbl_txtFirstName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblMiddleName" Text="Middle Name:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtMiddleName"
                                                    onkeypress="return AlphabetsOnlyForNameFields(event.keyCode, event.which)"
                                                    onKeyUp="Count(this,30)" onChange="Count(this,30)"
                                                    TabIndex="2" MaxLength="30" runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtMiddleName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblLastNameMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblLastName" Text="Last Name:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtLastName" TabIndex="3" OnTextChanged="txtLastName_TextChanged"
                                                    AutoPostBack="true"
                                                    onkeypress="return AlphabetsOnlyForNameFields(event.keyCode, event.which)"
                                                    onKeyUp="Count(this,30)" onChange="Count(this,30)"
                                                    MaxLength="30" runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtLastName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblCandidateStatusMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblCandidateStatus" Text="Candidate Status:" runat="server"></asp:Label>
                                                <%--The below 2 lables need to be placed with proper CSS--%>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlCandidateStatus" TabIndex="4" runat="server"
                                                    ValidationGroup="Save">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlCandidateStatus" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblDateOfBirthMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblDateOfBirth" Text="Date of Birth:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv CDate">
                                                <%--<input type="text" placeholder="datepicker">--%>
                                                <asp:TextBox ID="txtDateOfBirth" OnTextChanged="txtDateOfBirth_TextChanged"
                                                    AutoPostBack="true" runat="server"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnDateOfBirth" TabIndex="5"
                                                    class="ui-datepicker-trigger" runat="server"
                                                    ImageUrl="../Images/New%20Design/calender-icon.png" ImageAlign="AbsMiddle" />
                                                <ajaxToolkit:CalendarExtender ID="calDateOfBirth" runat="server" TargetControlID="txtDateOfBirth" PopupButtonID="imgbtnDateOfBirth" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:Label ID="lbl_txtDateOfBirth" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblSalutationMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblSalutation" Text="Salutation:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlSalutation" TabIndex="6" runat="server"
                                                    ValidationGroup="Save" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlSalutation_SelectedIndexChanged">
                                                    <asp:ListItem Text="select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Mr." Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Ms." Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Mrs." Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlSalutation" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblMaritalStatusMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblMaritalStatus" Text="Marital Status:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlMaritalStatus" TabIndex="7" runat="server"
                                                    ValidationGroup="Save">
                                                    <asp:ListItem Text="select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Single" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Married" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlMaritalStatus" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblGenderMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblGender" Text="Gender:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlGender" TabIndex="8" runat="server"
                                                    ValidationGroup="Save" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlGender_SelectedIndexChanged">
                                                    <asp:ListItem Text="select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlGender" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblAlternateContactNumberMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblAlternateContactNumber" Text="Alternate Contact Number:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtAlternateContactNumber" onkeypress="return NumberOnly(event.keyCode, event.which);"
                                                    TabIndex="9" ValidationGroup="Save" MaxLength="11" runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtAlternateContactNumber" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblMobileNumberMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblMobileNumber" Text="Mobile Number:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtMobileNumber" TabIndex="10" ValidationGroup="Save"
                                                    onkeypress="return NumberOnly(event.keyCode, event.which);" MaxLength="11"
                                                    runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtMobileNumber" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="sec1C clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblEmailIDMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblEmailID" Text="Email ID:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtEmailID" TabIndex="11" runat="server"
                                                    ValidationGroup="Save"></asp:TextBox>
                                                <asp:Label ID="lbl_txtEmailID" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblAlternateEmailID" Text="Alternate EmailID:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtAlternateEmailID" TabIndex="12" runat="server"
                                                    ValidationGroup="Save"></asp:TextBox>
                                                <asp:Label ID="lbl_txtAlternateEmailID" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                                <div class="sec1C clearfix smallDDowns">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblTotalWorkExpYearsMandatory" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                <span class="hiddenstar">*</span><asp:Label ID="lblTotalWorkExp" Text="Total Work Experience:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlTotalWorkExpYears" TabIndex="13" runat="server" ValidationGroup="Save">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlTotalWorkExpYears" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:Label ID="lblTotalWorkExp_Years" runat="server" CssClass="ClassDisplayLabel">years</asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlTotalWorkExpMonths" TabIndex="14" runat="server"
                                                    ValidationGroup="Save">
                                                    <%--<asp:ListItem Text="select" Value="-1">months</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlTotalWorkExpMonths" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:Label ID="lblTotalWorkExp_Months" runat="server" CssClass="ClassDisplayLabel">months</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblRelevantWorkExpYearsMandatory" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                <span class="hiddenstar">*</span><asp:Label ID="lblRelevantWorkExp" Text="Relevant Work Experience:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlRelevantWorkExpYears" TabIndex="15" runat="server" ValidationGroup="Save">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlRelevantWorkExpYears" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:Label ID="lblRelevantWorkExp_Years" runat="server" CssClass="ClassDisplayLabel">years</asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlRelevantWorkExpMonths" TabIndex="16" runat="server"
                                                    ValidationGroup="Save">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlRelevantWorkExpMonths" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                <asp:Label ID="lblRelevantWorkExp_Months" runat="server" CssClass="ClassDisplayLabel">months</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- <div class="rightcol">
									            <div class="formrow clearfix">
									                <div class="LabelDiv">
									                     <asp:Label ID="Label1" runat="server" Text="" SkinID="lblError"></asp:Label>
                                                         <span class="hiddenstar">*</span><asp:Label ID="Label2" Text="Candidate History:" runat="server"></asp:Label>
									                </div>
									            </div>
	                                        		<div class="InputDiv">
                                                        <asp:TextBox ID="txtCandidateHistory" runat="server" TextMode="MultiLine" Width="750px"></asp:TextBox>
                                                    </div>
									        </div>--%>
                                </div>
                            </div>
                        </div>

                        <div class="checkboxContainer clearfix">
                            <h3 class="smartrackH">
                                <asp:Label ID="lblSkillsMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>Skills</h3>
                            <asp:Panel ID="pnlChkList" runat="server" ScrollBars="Auto" Wrap="true">
                                <asp:CheckBoxList TabIndex="17" RepeatDirection="Horizontal" RepeatColumns="5"
                                    ID="chkList" runat="server" ValidationGroup="Save" CellPadding="3"
                                    CellSpacing="3" CssClass="CheckBoxList">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>

                        <div class="clearfix mrgnT30 sec3C">
                            <div class="twoColumns">
                                <div class="CandidateLeftcol clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblHighestQualificationMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblHighestQualification" Text="Highest Qualification:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList ID="ddlHighestQualification" TabIndex="37" Width="192px"
                                                    runat="server" ValidationGroup="Save">
                                                </asp:DropDownList>
                                                <asp:Label ID="lbl_ddlHighestQualification" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblOtherSkills" Text="Other Skills:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtOtherSkills" onKeyUp="Count(this,100)"
                                                    onChange="Count(this,100)" TabIndex="38" MaxLength="100"
                                                    TextMode="MultiLine" runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtOtherSkills" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="CandidateLeftcol clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblValidPassport" Text="Valid Passport:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:RadioButtonList ID="rdobtnValidPassport" runat="server"
                                                    RepeatDirection="Horizontal" AutoPostBack="True"
                                                    CssClass="RadioButtonList" TabIndex="39">
                                                    <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:Label ID="lbl_rdobtnValidPassport" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblUSVisa" Text="US Visa:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:RadioButtonList ID="rdobtnUSVisa" runat="server"
                                                    RepeatDirection="Horizontal" AutoPostBack="True"
                                                    CssClass="RadioButtonList" TabIndex="40">
                                                    <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:Label ID="lbl_rdobtnUSVisa" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="CandidateLeftcol clearfix">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblPresentAddressMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                                <asp:Label ID="lblPresentAddress" Text="Present Address:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtPresentAddress" onKeyUp="Count(this,255)" onChange="Count(this,255)"
                                                    TabIndex="41" MaxLength="255" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                <asp:Label ID="lbl_txtPresentAddress" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <span class="hiddenstar">*</span><asp:Label ID="lblWillingToRelocate" Text="Willing To Relocate:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:RadioButtonList ID="rdobtnWillingToRelocate" runat="server"
                                                    RepeatDirection="Horizontal" AutoPostBack="True"
                                                    CssClass="RadioButtonList" TabIndex="42">
                                                    <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:Label ID="lbl_rdobtnWillingToRelocate" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix threeColumns">
                                <%--<div class="clearfix">--%>
                                <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblCityMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblCity" Text="City:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtCity" TabIndex="43" runat="server" MaxLength="30"
                                                onKeyUp="Count(this,30)" onChange="Count(this,30)"></asp:TextBox>
                                            <asp:Label ID="lbl_txtCity" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblStateMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblState" Text="State:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtState" TabIndex="44" Width="190px" runat="server"
                                                MaxLength="30" onKeyUp="Count(this,30)" onChange="Count(this,30)"></asp:TextBox>
                                            <asp:Label ID="lbl_txtState" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblPinCodeMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblPinCode" Text="Pin Code:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtPinCode" TabIndex="45" runat="server" MaxLength="30"
                                                onKeyUp="Count(this,30)" onChange="Count(this,30)" AutoPostBack="True"></asp:TextBox>
                                            <asp:Label ID="lbl_txtPinCode" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <%--</div>--%>
                                <%--</div>--%>

                                <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblCountryMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblCountry" Text="Country:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:DropDownList ID="ddlCountry" TabIndex="46" runat="server"
                                                ValidationGroup="Save">
                                            </asp:DropDownList>
                                            <asp:Label ID="lbl_ddlCountry" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="lblCurrentNoticePeriod" Text="Current Notice Period:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtCurrentNoticePeriod" placeholder="Days"
                                                onkeypress="return NumberOnly(event.keyCode, event.which);" TabIndex="47"
                                                MaxLength="3" runat="server"></asp:TextBox>
                                            <asp:Label ID="lbl_txtCurrentNoticePeriod" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="lblCurrentCTC" Text="Current CTC:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:Label ID="lblRupeeSymbolCurrentCTC" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtCurrentCTC"
                                                onkeypress="return NumberOnlyWithDot(event.keyCode, event.which);"
                                                MaxLength="6" runat="server" TabIndex="48" Width="70px"></asp:TextBox>
                                            <asp:Label ID="lbl_txtCurrentCTC" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            <asp:Label ID="lblCurrentCTCPA" runat="server" CssClass="exampleNumber" Text="(lacs p.a.) (eg. 123.25)"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <%-- </div>--%>

                                <%--<div class="CandidateLeftcol clearfix mrgnB30">--%>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblSourceMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblSource" Text="Source:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:DropDownList ID="ddlSource" TabIndex="49" runat="server"
                                                ValidationGroup="Save">
                                                <asp:ListItem Text="select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Website" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Agency" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Walk-in" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Advertisement" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Referral" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="V2 Online career section" Value="6"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lbl_ddlSource" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblSourceNameMandatory" runat="server" Text="*" SkinID="lblError"></asp:Label>
                                            <asp:Label ID="lblSourceName" Text="Source Name:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtSourceName" TabIndex="50" MaxLength="50" runat="server"></asp:TextBox>
                                            <asp:Label ID="lbl_txtSourceName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="colOneThird">
                                    <div class="formcol clearfix">
                                        <div class="LabelDiv">
                                            <span class="hiddenstar">*</span><asp:Label ID="lblAreasOfInterest" Text="Areas Of Interest:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <asp:TextBox ID="txtAreasOfInterest" onKeyUp="Count(this,100)"
                                                onChange="Count(this,100)" TabIndex="51" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            <asp:Label ID="lbl_txtAreasOfInterest" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <%--</div>--%>
                            </div>
                            <h4 class="smallHead">Experience Details</h4>
                            <div class="">
                                <asp:UpdatePanel ID="pnlExperienceDetails" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdExperienceDetails" runat="server" TabIndex="52" Width="100%"
                                            GridLines="None" OnRowCommand="grdExperienceDetails_RowCommand" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowDeleting="grdExperienceDetails_RowDeleting" OnRowEditing="grdExperienceDetails_RowEditing"
                                            OnRowDataBound="grdExperienceDetails_RowDataBound" OnRowCancelingEdit="grdExperienceDetails_RowCancelingEdit"
                                            OnRowUpdating="grdExperienceDetails_RowUpdating" CssClass="margin_top TableJqgrid">
                                            <HeaderStyle CssClass="tableHeaders" />
                                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                            <RowStyle CssClass="tableRows" />

                                            <Columns>
                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Center" HeaderText="Sr.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpSrNo" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Exp ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpID" Text='<%#Eval("ExpID") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Organisation">
                                                    <HeaderStyle Width="125px" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditOrganisation" Text='<%#Eval("OrganisationName")%>' MaxLength="100"
                                                            runat="server" Width="110px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>

                                                        <asp:Label ID="lblOrganisation" Text='<%#Eval("OrganisationName") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="False" />
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterOrganisation" Width="110px" MaxLength="100" runat="server"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Center" HeaderText="Office Location">
                                                            <HeaderStyle Width="125px" />
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtEditOfficeLocation" Text='<%#Eval("Location") %>' onkeypress="return AlphabetsAndSpecialCharacters(event.keyCode, event.which);"
                                                                   runat="server" MaxLength="100"></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOfficeLocation" Text='<%#Eval("Location") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFooterOfficeLocation" onkeypress="return AlphabetsAndSpecialCharacters(event.keyCode, event.which)"
                                                                    runat="server"  width="120px" MaxLength="100"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="From Date">
                                                    <HeaderStyle Width="225px" />
                                                    <EditItemTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEditFromDate" Text='<%#Eval("WorkedFrom", "{0:MM/dd/yyyy}") %>'
                                                                        runat="server" Width="82px"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnEditFromDate" CssClass="ui-datepicker-trigger" Visible="true" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calEditFromDate" runat="server" TargetControlID="txtEditFromDate"
                                                                        PopupButtonID="imgbtnEditFromDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" Text='<%#Eval("WorkedFrom", "{0:MM/dd/yyyy}") %>' runat="server"></asp:Label>
                                                                    <asp:TextBox ID="txtFromDate" Visible="false" Width="82px" runat="server"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnFromDate" CssClass="ui-datepicker-trigger" Visible="false" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>

                                                                    <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtFromDate"
                                                                        PopupButtonID="imgbtnFromDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFooterFromDate" Width="82px" runat="server"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnFooterFromDate" CssClass="ui-datepicker-trigger" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calFooterFromDate" runat="server" TargetControlID="txtFooterFromDate"
                                                                        PopupButtonID="imgbtnFooterFromDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="To Date">
                                                    <HeaderStyle Width="225px" />
                                                    <EditItemTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEditToDate" Text='<%#Eval("WorkedTill","{0:MM/dd/yyyy}") %>'
                                                                        Width="82px" runat="server"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnEditToDate" CssClass="ui-datepicker-trigger" Visible="true" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calEditToDate" runat="server" TargetControlID="txtEditToDate"
                                                                        PopupButtonID="imgbtnEditToDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" Text='<%#Eval("WorkedTill","{0:MM/dd/yyyy}") %>' runat="server"></asp:Label>
                                                                    <asp:TextBox ID="txtToDate" Visible="false" Width="82px" runat="server"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnToDate" CssClass="ui-datepicker-trigger" Visible="false" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                <td>

                                                                    <ajaxToolkit:CalendarExtender ID="calToDate" runat="server" TargetControlID="txtToDate"
                                                                        PopupButtonID="imgbtnToDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFooterToDate" Width="82px" Text="" runat="server"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnFooterToDate" CssClass="ui-datepicker-trigger" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calFooterToDate" runat="server"
                                                                        TargetControlID="txtFooterToDate" PopupButtonID="imgbtnFooterToDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Type">
                                                    <HeaderStyle Width="125px" />
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblExpType2" Visible="false" Text='<%#Eval("ExpType") %>' runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlExpType" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpType" Visible="false" Text='<%#Eval("ExpType") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblExpType3" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterExpType" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Designation">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditDesignation" Text='<%#Eval("PositionHeld") %>' Width="100px" MaxLength="100"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesignation" Text='<%#Eval("PositionHeld") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterDesignation" Width="100px" runat="server" MaxLength="100"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Reporting Manager">
                                                    <HeaderStyle Width="150px" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditReportingManager" Text='<%#Eval("ReportingManager") %>' Width="110px" MaxLength="100"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReportingManager" Text='<%#Eval("ReportingManager") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterReportingManager" Width="110px" runat="server" MaxLength="100"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Last Salary Drawn">
                                                    <HeaderStyle Width="190px" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditLastDrawnCTCInLacs" Text='<%#Eval("CTC") %>' MaxLength="50"
                                                            runat="server" Width="45px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLastDrawnCTCInLacs" Text='<%#Eval("CTC") %>' runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterLastDrawnCTCInLacs" runat="server" MaxLength="50" Width="50px" CssClass="mrgnR3"></asp:TextBox><asp:Label ID="lblLastDrawnCTCInLacsPA" runat="server" CssClass="exampleNumber" Text="(lacs p.a.)"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                                                    <EditItemTemplate>
                                                        <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="" CssClass="mrgnB5 BtnUpdate"></asp:Button>
                                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="" CssClass="BtnCancel"></asp:Button>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnRemoveExperience" runat="server" Text="" CommandName="Delete"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="BtnDelete mrgnB5" />
                                                                    <asp:Button ID="btnEditExperience" runat="server" Text="" CommandName="Edit" CssClass="BtnEdit" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddMoreExperience" Text="" CommandName="Add"
                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" runat="server" CssClass="BtnAdd" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdExperienceDetails" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <h4 class="smallHead">Education Details</h4>
                            <div class="">
                                <asp:UpdatePanel ID="pnlEducationDetails" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdEducationDetails" runat="server" Width="100%" TabIndex="53"
                                            AutoGenerateColumns="false" ShowFooter="true" GridLines="None" OnRowCommand="grdEducationDetails_RowCommand"
                                            OnRowDataBound="grdEducationDetails_RowDataBound" OnRowDeleting="grdEducationDetails_RowDeleting"
                                            OnRowEditing="grdEducationDetails_RowEditing" OnRowCancelingEdit="grdEducationDetails_RowCancelingEdit"
                                            OnRowUpdating="grdEducationDetails_RowUpdating" CssClass="margin_top TableJqgrid">

                                            <HeaderStyle CssClass="tableHeaders" />
                                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                            <RowStyle CssClass="tableRows" />
                                            <Columns>
                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Center" HeaderText="Sr.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEducationSrNo" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Education ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEducationID" Text='<%#Eval("EducationID") %>' runat="server">
                                                                </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Degree">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblDegree2" Visible="false" Text='<%#Eval("Degree") %>' runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlDegree" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDegree" Visible="false" Text='<%#Eval("Degree") %>' Width="100px"
                                                            runat="server"></asp:Label>
                                                        <asp:Label ID="lblDegreeName" Width="100px" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterDegree" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Qualification">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblCourse2" Visible="false" Text='<%#Eval("Course") %>' runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlCourse" Width="300px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCourse" Visible="false" Text='<%#Eval("Course") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblCourseName" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterCourse" Width="300px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Specialization">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditSpecialization" Text='<%#Eval("Specialization") %>' Width="100px" MaxLength="200"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSpecialization" Text='<%#Eval("Specialization") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterSpecialization" runat="server" Width="100px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Center" HeaderText="Institute">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtEditInstitute" Text='<%#Eval("Institute") %>' Width="100px" runat="server"></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInstitute" Text='<%#Eval("Institute") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFooterInstitute" runat="server" Width="100px" MaxLength="100"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Institute / University">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditUniversity" Text='<%#Eval("University") %>' Width="100px" MaxLength="200"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUniversity" Text='<%#Eval("University") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterUniversity" runat="server" Width="100px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Year">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblYear2" Visible="false" Text='<%#Eval("Year") %>' runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlYear" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" Text='<%#Eval("Year") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterYear" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Type">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblType2" Visible="false" Text='<%#Eval("Type") %>' runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlType" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" Visible="false" Text='<%#Eval("Type") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblTypeCourse" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterType" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Percentage">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditPercentage" Text='<%#Eval("Percentage") %>' Width="55px" MaxLength="50" onkeypress="return AllCharactersWithoutNegative(event.keyCode, event.which)"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPercentage" Text='<%#Eval("Percentage") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterPercentage" runat="server" Width="55px" onkeypress="return AllCharactersWithoutNegative(event.keyCode, event.which)"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                                                    <EditItemTemplate>
                                                        <asp:Button ID="btnUpdateEducation" runat="server" CommandName="Update"
                                                            Text="" CssClass="BtnUpdate mrgnB5"></asp:Button>
                                                        <asp:Button ID="btnCancelEducation" runat="server" CommandName="Cancel"
                                                            Text="" CssClass="BtnCancel"></asp:Button>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnRemoveEducation" runat="server" Text="" CommandName="Delete"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="BtnDelete mrgnB5" />
                                                                    <asp:Button ID="btnEditEducation" runat="server" Text="" CommandName="Edit" CssClass="BtnEdit" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddMoreEducation" ValidationGroup="Education" Text=""
                                                            CommandName="Add" runat="server" CssClass="BtnAdd" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdEducationDetails" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <h4 class="smallHead">Certification Details</h4>
                            <div class="">
                                <asp:UpdatePanel ID="pnlCertificationDetails" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdCertificationDetails" runat="server" Width="100%" TabIndex="54"
                                            AutoGenerateColumns="false" ShowFooter="true" GridLines="None" OnRowCommand="grdCertificationDetails_RowCommand"
                                            OnRowDeleting="grdCertificationDetails_RowDeleting" OnRowEditing="grdCertificationDetails_RowEditing"
                                            OnRowDataBound="grdCertificationDetails_RowDataBound" OnRowCancelingEdit="grdCertificationDetails_RowCancelingEdit"
                                            OnRowUpdating="grdCertificationDetails_RowUpdating" CssClass="margin_top TableJqgrid mrgnB30">
                                            <HeaderStyle CssClass="tableHeaders" />
                                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                            <RowStyle CssClass="tableRows" />
                                            <Columns>
                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Center" HeaderText="Sr.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCertificationSrNo" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certification ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificationID" Text='<%#Eval("CertificationID") %>' runat="server">
                                                                </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certification Name">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblCertificationName2" Visible="false" Text='<%#Eval("CertificationName") %>'
                                                            runat="server"></asp:Label>
                                                        <asp:DropDownList ID="ddlCertificationName" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificationName" Visible="false" Text='<%#Eval("CertificationName") %>'
                                                            Width="100px" runat="server"></asp:Label>
                                                        <asp:Label ID="lblCertificationNameName" Width="100px" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterCertificationName" Width="100px" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certification Number">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditCertificationNo" Text='<%#Eval("CertificationNo") %>' Width="100px" MaxLength="50"
                                                            runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificationNo" Text='<%#Eval("CertificationNo") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterCertificationNo" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Institution">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditInstitution" Text='<%#Eval("Institution") %>' runat="server" MaxLength="500"
                                                            Width="100px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstitution" Text='<%#Eval("Institution") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterInstitution" runat="server" Width="100px" MaxLength="500"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certified On Date">
                                                    <EditItemTemplate>
                                                        <table width="175px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEditCertifiedOnDate" Text='<%#Eval("CertificationDate","{0:MM/dd/yyyy}") %>'
                                                                        runat="server" Width="100px"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnEditCertifiedOnDate" CssClass="ui-datepicker-trigger" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calEditCertifiedOnDate" runat="server" TargetControlID="txtEditCertifiedOnDate"
                                                                        PopupButtonID="imgbtnEditCertifiedOnDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="175px">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCertifiedOnDate" Text='<%#Eval("CertificationDate","{0:MM/dd/yyyy}") %>'
                                                                        runat="server"></asp:Label>
                                                                    <asp:TextBox ID="txtCertifiedOnDate" Visible="false" runat="server" Width="100px"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnCertifiedOnDate" CssClass="ui-datepicker-trigger" Visible="false" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calCertifiedOnDate" runat="server" TargetControlID="txtCertifiedOnDate"
                                                                        PopupButtonID="imgbtnCertifiedOnDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table width="150px">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFooterCertifiedOnDate" runat="server" Width="82px"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnFooterCertifiedOnDate" CssClass="ui-datepicker-trigger" runat="server" ImageUrl="../Images/New%20Design/calender-icon.png" />
                                                                </td>
                                                                <td>
                                                                    <ajaxToolkit:CalendarExtender ID="calFooterCertifiedOnDate" runat="server" TargetControlID="txtFooterCertifiedOnDate"
                                                                        PopupButtonID="imgbtnFooterCertifiedOnDate" Format="MM/dd/yyyy">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certification Score %">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditCertificationScore" Text='<%#Eval("CertificationScore") %>'
                                                            MaxLength="30" Width="60px" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificationScore" Text='<%#Eval("CertificationScore") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterCertificationScore" MaxLength="30" runat="server" Width="60px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Certification Grade">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditCertificationGrade" Text='<%#Eval("CertificationGrade") %>' MaxLength="30"
                                                            Width="60px" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificationGrade" Text='<%#Eval("CertificationGrade") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterCertificationGrade" runat="server" Width="60px" MaxLength="30"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Center" HeaderText="Action">
                                                    <EditItemTemplate>
                                                        <asp:Button ID="btnUpdateCertification" runat="server" CommandName="Update"
                                                            Text="" CssClass="BtnUpdate mrgnB5"></asp:Button>
                                                        <asp:Button ID="btnCancelCertification" runat="server" CommandName="Cancel"
                                                            Text="" CssClass="BtnCancel"></asp:Button>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnRemoveCertification" runat="server" Text=""
                                                                        CommandName="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="BtnDelete mrgnB5" />
                                                                    <asp:Button ID="btnEditCertification" runat="server" Text="" CommandName="Edit" CssClass="BtnEdit" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddMoreCertification" CssClass="BtnAdd" Text="" CommandName="Add"
                                                            runat="server" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdCertificationDetails" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="clearfix">
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="lblCurrentJobSummary" Text="Current Job Summary:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtCurrentJobSummary" onKeyUp="Count(this,400)" onChange="Count(this,400)"
                                                    TabIndex="55" MaxLength="400" TextMode="MultiLine" runat="server">
                                                            </asp:TextBox>
                                                <asp:Label ID="lbl_txtCurrentJobSummary" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv biglabel">
                                                <asp:Label ID="lblRewardsAndRecognition" Text="Rewards and Recognition:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtRewardsAndRecognition" onKeyUp="Count(this,400)" onChange="Count(this,400)"
                                                    TabIndex="56" MaxLength="400" TextMode="MultiLine" runat="server">
                                                            </asp:TextBox>
                                                <asp:Label ID="lbl_txtRewardsAndRecognition" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv achieve">
                                                <asp:Label ID="lblSpecialAchievements" Text="Special Achievements:" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtSpecialAchievements" onKeyUp="Count(this,500)" onChange="Count(this,500)"
                                                    TabIndex="57" MaxLength="500" TextMode="MultiLine" runat="server">
                                                          </asp:TextBox>
                                                <asp:Label ID="lbl_txtSpecialAchievements" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="CandidateLeftcol clearfix mrgnB30 twoColumns">
                                <div class="leftcol">
                                    <div class="formrow clearfix">
                                        <div class="LabelDiv">
                                            <asp:Label ID="lblUploadResume" Text="Upload Resume:" runat="server"></asp:Label>
                                        </div>
                                        <div class="InputDiv">
                                            <div class="InputDiv positionR BrowseSpacingFix">

                                                <input type="file" name="uploadFiles" id="uploadFiles" size="24" data-val="true" class="FileUploadBtn" onchange="document.getElementById('FileUploadResumeField').value = this.value.split('\\')[this.value.split('\\').length-1];" runat="server" />
                                                <div class="BrowserVisible">
                                                    <input type="button" id="btnForCustomUpload" class="BtnForCustomUpload" value="Browse.." />
                                                    <input type="text" id="FileUploadResumeField" class="FileField" value="No files selected" />
                                                </div>

                                                <%--<asp:FileUpload ID="fileUpload" TabIndex="58" runat="server" />--%>
                                                <asp:Label runat="server" ID="lblSuccessMsg" SkinID="lblSuccess" />
                                                <asp:Label runat="server" ID="lblErrorMsg" SkinID="lblError" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="ButtonContainer1">
                            <asp:Button ID="btnSave" TabIndex="59" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnSaveAndAddMore" TabIndex="60" runat="server" Text="Save And Add More" OnClick="btnSaveAndAddMore_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnCancel" TabIndex="61" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnAlert" runat="server" OnClick="btnAlert_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnAlertCancel" runat="server" OnClick="btnAlertCancel_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnRedirect" runat="server" OnClick="btnRedirect_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnSaveAndAddMoreRedirect" runat="server" OnClick="btnSaveAndAddMoreRedirect_Click" CssClass="ButtonGray" />
                        </div>
                    </section>
                </asp:Panel>
                <%--==================================--%>

                <section class="search-detailsdata clearfix">
                    <%--grid view here (Candidate Data Bank)--%>
                    <CD:CandidateDataBankTag ID="CandidateDataBank1" runat="server" />
                </section>
            </div>
        </div>
    </section>
</asp:Content>