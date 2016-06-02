<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true"
    Inherits="CandidateSearch" CodeBehind="CandidateSearch.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>--%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Recruitment/pop_up_window.js" type="text/javascript"></script>
    <script src="../Scripts/Recruitment/jquery.tmpl.js" type="text/javascript"></script>
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
        $(document).ready(function () {

            $("#MainContent_grdCandidateSearch_ddlQualification").bind("change", function () {
                $("#MainContent_grdCandidateSearch_ddlQualification").next().attr('title', $("#MainContent_grdCandidateSearch_ddlQualification option:selected").text());
            });
        });
        var title = "";
        function NumberOnly() {
            var AsciiValue = event.keyCode
            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
                event.returnValue = true;
            else
                event.returnValue = false;
        }

        //        function AlphabetsOnly() {
        //            var AsciiValue = event.keyCode
        //            if ((AsciiValue >= 65 && AsciiValue <= 90) || (AsciiValue >= 97 && AsciiValue <= 122))
        //                event.returnValue = true;
        //            else
        //                event.returnValue = false;
        //        }

        function CheckAlphaNumericName(_char, _mozChar) {
            if (_mozChar != null) { // Look for a Mozilla-compatible browser
                if ((_mozChar >= 65 && _mozChar <= 90) || (_mozChar >= 97 && _mozChar <= 122) || (_mozChar == 13) || (_mozChar == 32) || (_mozChar == 39) || (_mozChar == 8) || (_mozChar == 0)) _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter character only.' + '</p>', title);
                }
            }
            else { // Must be an IE-compatible Browser

                if ((_char >= 65 && _char <= 90) || (_char >= 97 && _char <= 122) || (_char == 13) || (_char == 32) || (_char == 39) || (_char == 8) || (_char == 0)) _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter character only.' + '</p>', title);
                }
            }
            return _RetVal;
        }
        function CheckNumericKeyInfo(_char, _mozChar) {
            if (_mozChar != null) { // Look for a Mozilla-compatible browser
                if ((_mozChar >= 48 && _mozChar <= 57) || _mozChar == 0 || _char == 8 || _mozChar == 13 || _mozChar == 46) _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter a numeric value.' + '</p>', title);
                }
            }
            else { // Must be an IE-compatible Browser
                if ((_char >= 48 && _char <= 57) || _char == 13 || _char == 46) _RetVal = true;
                else {
                    _RetVal = false;
                    V2hrmsAlert('<p>' + 'Please enter a numeric value.' + '</p>', title);
                }
            }
            return _RetVal;
        }
        function ValidateYears() {
            var fromyears = document.getElementById("MainContent_grdCandidateSearch_txtFromYears").value;
            var uptoyears = document.getElementById("MainContent_grdCandidateSearch_txtUptoYears").value;

            if ((fromyears == "" && uptoyears == "") || (fromyears != "" && uptoyears != "")) {
                if (fromyears != "" && uptoyears != "") {
                    if (eval(fromyears) > eval(uptoyears)) {
                        V2hrmsAlert('<p>' + "From Years cannot be greater than Upto years." + '</p>', title);
                        return false;
                    }
                }
                return true;
            }
            else {
                V2hrmsAlert('<p>' + "Please enter the Years Value correctly." + '</p>', title);
                if (fromyears == "") {
                    document.getElementById("MainContent_grdCandidateSearch_txtFromYears").focus();
                    return false;
                }

                if (uptoyears == "") {
                    document.getElementById("MainContent_grdCandidateSearch_txtUptoYears").focus();
                    return false;

                }
            }
        }

        function SingleSelectCheckbox(current) {
            var flag = true;
            if (current.checked == false)
                flag = false;

            var gv = document.getElementById('<%= this.grdSearchResults.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];

                var inputs = rowElement.getElementsByTagName("input");
                //  V2hrmsAlert('<p>' + inputs.length);
                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        inputs[0].checked = false;
                    }
                }

            }
            if (flag == false)
            { current.checked = false; }
            else {
                current.checked = true;

            }
        }

        function Validate() {

            var flag = true;
            var gv = document.getElementById('<%= this.grdSearchResults.ClientID %>');
            var rowCount = gv.rows.length;

            for (var i = 1; i < rowCount; i++) {
                var rowElement = gv.rows[i];
                //  V2hrmsAlert('<p>' + rowElement.getElementById('hdID').value);
                var inputs = rowElement.getElementsByTagName("input");

                if (typeof (inputs[0]) != 'undefined') {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked == true) {
                            flag = false;
                        }
                    }
                }
            }
            if (flag == true) {
                V2hrmsAlert('<p>' + 'Kindly select record' + '</p>', title);
                return false;
            }
            else
                return true;
        }
    </script>
    <script language="javascript" type="text/javascript">

        function show_confirm() {
            alert("No Resume for this candidate was found.");
            //V2hrmsAlert('<p>' + "No Resume for this candidate was found." + '</p>', title);
            return false;
        }
    </script>
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <%--<div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">
                    SmarTrack</h2>
                <!--<div class="EmpSearch clearfix">
									<a href="#"></a>
									<input type="text" placeholder="Employee Search">
								</div>-->
            </div>
            <nav class="sub-menu-colored">
                <a href="MastersTable.aspx">MasterTable</a> <a href="Recruiter.aspx" class="selected">
                    Recruiter</a> <a href="RRFList.aspx">RRF List</a> <a href="Candidate.aspx">Candidate</a>
                <a href="RRFList.aspx">HRM List</a> <a href="Interviewer.aspx">Interviewer</a>
            </nav>
        </div>--%>
        <div class="MainBody RecruiterBody">
            <div class="InnerContainer">
                <div class="clearfix">
                    <asp:Button ID="btnBack" OnClientClick="javascript:history.go(-1);return false;"
                        runat="server" Text="Back" Width="100px" CssClass="floatL BackLink" />
                    <asp:Button ID="btnRedirect" Visible="false" runat="server" Text="Back" Width="100px"
                        OnClick="btnRedirect_Click" CssClass="floatL BackLink" />
                </div>
                <h3 class="SmallHeadingBold">Candidate Search

                    <%--<a class="floatR BackLink" href="#"
                   id="BackPrevious">Back</a>--%>
                </h3>
                <div class="clearfix LabelST">
                    <div class="floatL mrgnR50">
                        <%-- <label class="prefix">RRF No:</label>
                                  <label class="suffix">4567278</label>--%>
                        <asp:Label ID="Label1" runat="server" CssClass="prefix" Text="RRF No :"></asp:Label>
                        <asp:Label ID="lblRRFNO" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                    <div class="floatL mrgnR50">
                        <%--<label class="prefix">
                            Position:</label>
                        <label class="suffix">
                            Associate Executive - HR</label>--%>
                        <asp:Label ID="Label2" runat="server" CssClass="prefix" Text="Position :"></asp:Label>
                        <asp:Label ID="lblPosition" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                    <div class="floatL mrgnR50">
                        <%--<label class="prefix">
                            Resource Pool:</label>
                        <label class="suffix">
                            -</label>--%>
                        <asp:Label ID="Label6" runat="server" CssClass="prefix" Text="Resource Pool :"></asp:Label>
                        <asp:Label ID="lblResourcePoolName" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                    <div class="floatL mrgnR50">
                        <%--<label class="prefix">
                            Posted Date:</label>
                        <label class="suffix">
                            11/22/2013</label>--%>
                        <asp:Label ID="Label3" runat="server" CssClass="prefix" Text="Posted Date :"></asp:Label>
                        <asp:Label ID="lblPostedDate" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                    <div class="floatL mrgnR50">
                        <%--<label class="prefix">
                            Requestor:</label>
                        <label class="suffix">
                            Tanmay Subhash Sawant</label>--%>
                        <asp:Label ID="Label4" runat="server" CssClass="prefix" Text="Requestor :"></asp:Label>
                        <asp:Label ID="lblRequestor" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                </div>
                <!-- gridview here (search button to be designed)-->
                <div class="">
                    <asp:GridView ID="grdCandidateSearch" runat="server" AutoGenerateColumns="False"
                        ShowFooter="True" OnRowCommand="grdCandidateSearch_RowCommand"
                        Width="100%" CellPadding="3"
                        OnSelectedIndexChanged="grdCandidateSearch_SelectedIndexChanged"
                        CssClass="TableJqgrid">
                        <FooterStyle CssClass="tableRows" />
                        <HeaderStyle CssClass="tableHeaders" />
                        <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                            LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                            PreviousPageText="Prev" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                        <RowStyle CssClass="tableRows" />
                        <Columns>
                            <asp:TemplateField HeaderText="Name" Visible="true">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtName" runat="server" onkeypress="return CheckAlphaNumericName(event.keyCode, event.which);"
                                        Width="100"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Work Experience" Visible="true">
                                <FooterTemplate>
                                    <asp:Label ID="lblBetween" runat="server" Text="Between"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Years" Visible="true">
                                <FooterTemplate>
                                    <table width="100px" style="text-align: center;">
                                        <tr>
                                            <td height="32px">From :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromYears" runat="server" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"
                                                    Width="22" MaxLength="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Upto :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUptoYears" runat="server" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"
                                                    Width="22" Visible="true" MaxLength="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qualification" Visible="true">
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlQualification" runat="server" Width="100">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Keyword" Visible="true">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtKeyword" runat="server" Width="100"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Notice Period" Visible="true">
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNoticePeriod" MaxLength="2" runat="server" onkeypress="return CheckNumericKeyInfo(event.keyCode, event.which);"
                                        Width="22"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" Visible="true">
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="150">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" Visible="true">
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkSearch" runat="server" class="ButtonGraySearch" OnClientClick="javascript:return ValidateYears()">Search</asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <!------------------->
                </div>
                <asp:Label ID="lblError" runat="server" SkinID="lblError" CssClass="ErrorMsgOrbit"></asp:Label>
                <asp:Panel ID="pnlAction" runat="server" class="ButtonContainer2 clearfix">
                    <asp:Button ID="btnInitiateRecruitment" runat="server" Text="Initiate Recruitment"
                        OnClick="btnInitiateRecruitment_Click" OnClientClick="javascript:return Validate()"
                        CssClass="ButtonGray"></asp:Button>
                    <asp:Button ID="btnOpenResume" runat="server" Text="View Resume" OnClick="btnOpenResume_Click"
                        OnClientClick="javascript:return Validate()" CssClass="ButtonGray"></asp:Button>
                </asp:Panel>
                <asp:GridView ID="grdSearchResults" runat="server" Width="100%" AutoGenerateColumns="False"
                    Style="text-align: center" CellPadding="3" BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" OnRowCommand="grdSearchResults_RowCommand" OnPageIndexChanging="grdSearchResults_PageIndexChanging"
                    AllowSorting="true" OnSorting="grdSearchResults_Sorting" CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                        LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                        PreviousPageText="Prev" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <RowStyle CssClass="tableRows" />
                    <Columns>
                        <asp:TemplateField HeaderText="CandidateID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" Visible="true" SortExpression="FirstName">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                                <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Work Experience" Visible="true" SortExpression="TotalWorkExperienceInYear">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalWorkExperienceInYears" runat="server" Text='<%# Eval("TotalWorkExperienceInYear")%>'></asp:Label>
                                <asp:Label ID="Label3" runat="server" Text="Years"></asp:Label>
                                <asp:Label ID="lblTotalWorkExperienceInMonths" runat="server" Text='<%# Eval("TotalWorkExperienceInMonths")%>'></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="Months"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qualification" Visible="true" SortExpression="QualificationName">
                            <ItemTemplate>
                                <asp:Label ID="lblQualification" runat="server" Text='<%# Bind("QualificationName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notice Period" Visible="true" SortExpression="NoticePeriod">
                            <ItemTemplate>
                                <asp:Label ID="lblNoticePeriod" runat="server" Text='<%# Bind("NoticePeriod") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" Visible="true" SortExpression="CandidateStatus">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("CandidateStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Resume Uploaded" SortExpression="IsFileUploaded">
                            <ItemTemplate>
                                <asp:Label ID="lblIsFileUploaded" runat="server" Text='<%# (Boolean.Parse(Eval("IsFileUploaded").ToString())) ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Modified Date" SortExpression="CreatedDate">
                            <ItemTemplate>
                                <asp:Label ID="lblLastModifiedDate" runat="server" Text='<%# Eval("LastModifiedDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--       <asp:TemplateField HeaderText="Initiate" Visible="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkInitiateRecruitment" CommandName="InitiateRecruitment" runat="server">Initiate Recruitment</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Resume" Visible="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkOpenResume" CommandName="OpenResume" runat="server">Open Resume</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Select Record">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                                <%-- <label for="chkSelect" class="LabelForCheckbox"></label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>