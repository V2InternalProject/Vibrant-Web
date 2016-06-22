<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CandidateDataBank.ascx.cs" Inherits="HRMS.Recruitment.UserControl.CandidateDataBank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
        <link href="../Content/New%20Design/common.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-dialog-titlebar-close {
                display: none;
            }
        </style>

        <script language="javascript" type="text/javascript">

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
                ApplyClass();
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                ApplyClass();
            });

            var title = "";

            function show_confirm(msg) {
                V2hrmsAlert('<p>' + msg + '</p>', title);
                return false;
            }

            function NumberOnly() {
                var AsciiValue = event.keyCode
                if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
                    event.returnValue = true;
                else
                    event.returnValue = false;
            }

            function AlphabetsOnly() {
                var AsciiValue = event.keyCode
                if ((AsciiValue >= 65 && AsciiValue <= 90) || (AsciiValue >= 97 && AsciiValue <= 122))
                    event.returnValue = true;
                else
                    event.returnValue = false;
            }

            function ValidateYears() {

                var fromyears = document.getElementById("MainContent_grdCandidateSearch_txtFromYears").value;
                var uptoyears = document.getElementById("MainContent_grdCandidateSearch_txtUptoYears").value;

                if ((fromyears == "" && uptoyears == "") || (fromyears != "" && uptoyears != "")) {
                    if (fromyears != "" && uptoyears != "") {
                        if (eval(fromyears) > eval(uptoyears)) {
                            V2hrmsAlert('<p>' + 'From Years cannot be greater than Upto years.' + '</p>', title);
                            // ("From Years cannot be greater than Upto years.",)
                            return false;
                        }
                    }
                    return true;
                }
                else {
                    V2hrmsAlert('<p>' + 'Please enter the Years Value correctly.' + '</p>', title);
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

            function Delete_confirm() {
                return confirm("Are you sure you want to delete this Candidate?");
            }
        </script>
        <script language="javascript" type="text/javascript">
            function SingleSelectCheckbox(current) {
                var flag = true;
                if (current.checked == false)
                    flag = false;

                var gv = document.getElementById('<%= this.grdCandidates.ClientID %>');
                var rowCount = gv.rows.length;

                for (var i = 1; i < rowCount; i++) {
                    var rowElement = gv.rows[i];

                    var inputs = rowElement.getElementsByTagName("input");
                    // alert(inputs.length);

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

            function Validate() {
                var flag = true;
                var gv = document.getElementById('<%= this.grdCandidates.ClientID %>');
                var rowCount = gv.rows.length;

                for (var i = 1; i < rowCount; i++) {
                    var rowElement = gv.rows[i];
                    // alert(rowElement.getElementById('hdID').value);
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
            function checkBeforeDeleting() {
                if (!Validate()) return false;
                if (!Delete_confirm()) return false;
            }
        </script>

        <%-- <section class="ConfirmationContainer Container">--%>
        <%--<div class="MainBody SmartT DatabankB">--%>
        <div id="tblMain" runat="server" class="DatabankB SmartT">
            <asp:Label ID="lblErrorMessage" SkinID="lblError" runat="server"></asp:Label>
            <asp:Label ID="lblSuccessMessage" SkinID="lblSuccess" runat="server"></asp:Label>
            <div class="clearfix">
                <%--<h3 class="smartrackH floatL">Candidate Data Bank</h3>--%>

                <div class="">
                    <asp:UpdatePanel ID="pnlCandidateSearch" runat="server">

                        <ContentTemplate>
                            <asp:GridView ID="grdCandidateSearch" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" ShowHeaderWhenEmpty="True" OnRowCommand="grdCandidateSearch_RowCommand"
                                Width="100%" CssClass="margin_top TableJqgrid">
                                <HeaderStyle CssClass="tableHeaders" />
                                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                <RowStyle CssClass="tableRows" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" Visible="true">
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtName" runat="server" onkeypress="javascript:return AlphabetsOnly()"
                                                Width="100"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Total Work Experience" Visible="true">
                                            <FooterTemplate>
                                                <asp:Label ID="lblBetween" runat="server" Text="Between" CssClass="label"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Total Work Experience (Years)" Visible="true">
                                        <FooterTemplate>
                                            <table width="100px">
                                                <tr>
                                                    <td class="databank_width">
                                                        <asp:Label ID="lblFrom" runat="server" Text="From:" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromYears" runat="server" onkeypress="javascript:return NumberOnly()"
                                                            MaxLength="2" Width="20px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="databank_width">
                                                        <asp:Label ID="lblUpto" runat="server" Text="Upto:" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUptoYears" runat="server" onkeypress="javascript:return NumberOnly()"
                                                            Visible="true" MaxLength="2" Width="20px"></asp:TextBox>
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
                                            <asp:TextBox ID="txtNoticePeriod" runat="server" MaxLength="2" onkeypress="javascript:return NumberOnly()"
                                                Width="20px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" Visible="true">
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" Visible="true">
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkSearch" runat="server" OnClientClick="javascript:return ValidateYears()" CssClass="ButtonGraySearch">Search</asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="scrollHContainer">
                    <asp:UpdatePanel ID="pnlCandidates" runat="server">

                        <ContentTemplate>
                            <asp:Panel ID="pnlAction1" Visible="false" runat="server">
                                <div class="ButtonContainer2 clearfix">

                                    <asp:Button ID="btnAdd" runat="server" Text="Add Candidate" OnClick="btnAdd_Click"
                                        CssClass="ButtonGray"></asp:Button>

                                    <asp:Button ID="btnEdit" OnClientClick="javascript:return Validate()"
                                        runat="server" Text="Edit" OnClick="btnEdit_Click" CssClass="ButtonGray"></asp:Button>

                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="javascript:return checkBeforeDeleting()"
                                        OnClick="btnDelete_Click" CssClass="ButtonGray"></asp:Button>

                                    <asp:Button ID="btnViewProfile" OnClientClick="javascript:return Validate()"
                                        runat="server" Text="View Profile" OnClick="btnViewProfile_Click" CssClass="ButtonGray"></asp:Button>

                                    <asp:Button ID="btnOpenResume" OnClientClick="javascript:return Validate()"
                                        runat="server" Text="View Resume" OnClick="btnOpenResume_Click" CssClass="ButtonGray"></asp:Button>
                                </div>
                            </asp:Panel>

                            <asp:GridView ID="grdCandidates" runat="server" Width="100%" AutoGenerateColumns="False"
                                OnPageIssndexChanging="grdCandidates_PageIndexChanging" OnRowCommand="grdCandidates_RowCommand"
                                AllowPaging="true" PageSize="10" OnSorting="grdCandidates_Sorting" AllowSorting="true" CssClass="margin_top TableJqgrid">
                                <HeaderStyle CssClass="tableHeaders" />
                                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                                <RowStyle CssClass="tableRows" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Candidate ID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCandidateID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Candidate Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCandidateFirstName" runat="server" Text='<%# Eval("FirstName")%>'></asp:Label>
                                            <asp:Label ID="lblCandidateLastName" runat="server" Text='<%# Eval("LastName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Work Experience" SortExpression="TotalWorkExperienceInYear">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalWorkExperienceInYears" runat="server" Text='<%# Eval("TotalWorkExperienceInYear")%>'></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="Years"></asp:Label>
                                            <asp:Label ID="lblTotalWorkExperienceInMonths" runat="server" Text='<%# Eval("TotalWorkExperienceInMonths")%>'></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Text="Months"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relevant Work Experience" SortExpression="RelevantWorkExperienceInYear">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRelevantWorkExperienceInYears" runat="server" Text='<%# Eval("RelevantWorkExperienceInYear")%>'></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Text="Years"></asp:Label>
                                            <asp:Label ID="lblRelevantWorkExperienceInmonths" runat="server" Text='<%# Eval("RelevantWorkExperienceInmonths")%>'></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Text="Months"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Highest Qualification" SortExpression="QualificationName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHighestQualification" runat="server" Text='<%# Eval("QualificationName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Skills" SortExpression="OtherSkills">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSkills" runat="server" Text='<%# Eval("OtherSkills")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Candidate Status" SortExpression="CandidateStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCandidateStatus" runat="server" Text='<%# Eval("CandidateStatus")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RRF Code" SortExpression="RRFNo">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRRFCode" Text='<%# Eval("RRFNo") %>' />
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
                                    <asp:TemplateField HeaderText="Select Record">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkSelect" onclick="javascript:SingleSelectCheckbox(this);" />
                                            <asp:Label ID="Label5" runat="server" AssociatedControlID="chkSelect" class="LabelForCheckbox"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <%-- </div>--%>
        <%--        </section>--%>
    </ContentTemplate>
</asp:UpdatePanel>