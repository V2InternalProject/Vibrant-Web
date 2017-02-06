<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true"
    Inherits="HRM" CodeBehind="HRM.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>--%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txt_PMSOrganizationUnit').text($('#PMSOrganizationUnit').find(":selected").text());
            if ($('#MainContent_txtRequestor').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtRequestor').text($('#MainContent_txtRequestor').val());
                $('#MainContent_txtRequestor').hide();
                //$('#Label18').hide();
            }

            if ($('#MainContent_txtRequestDate').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtRequestDate').text($('#MainContent_txtRequestDate').val());
                $('#MainContent_txtRequestDate').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtExpectedClosureDate').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtExpectedClosureDate').text($('#MainContent_txtExpectedClosureDate').val());
                $('#MainContent_txtExpectedClosureDate').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtRRFNo').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtRRFNo').text($('#MainContent_txtRRFNo').val());
                $('#MainContent_txtRRFNo').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtForDU').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtForDU').text($('#MainContent_txtForDU').val());
                $('#MainContent_txtForDU').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtForDT').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtForDT').text($('#MainContent_txtForDT').val());
                $('#MainContent_txtForDT').hide();
                //$('#Label18').hide();
            }

            //

            if ($('#MainContent_txtProjectName').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtProjectName').text($('#MainContent_txtProjectName').val());
                $('#MainContent_txtProjectName').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtResourcePool').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtResourcePool').text($('#MainContent_txtResourcePool').val());
                $('#MainContent_txtResourcePool').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtPositionsRequired').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtPositionsRequired').text($('#MainContent_txtPositionsRequired').val());
                $('#MainContent_txtPositionsRequired').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtIndicativePanel1').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtIndicativePanel1').text($('#MainContent_txtIndicativePanel1').val());
                $('#MainContent_txtIndicativePanel1').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtEmployementType').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtEmployementType').text($('#MainContent_txtEmployementType').val());
                $('#MainContent_txtEmployementType').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtExperience').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtExperience').text($('#MainContent_txtExperience').val());
                $('#MainContent_txtExperience').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtIndicativePanel2').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtIndicativePanel2').text($('#MainContent_txtIndicativePanel2').val());
                $('#MainContent_txtIndicativePanel2').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtIndicativePanel3').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtIndicativePanel3').text($('#MainContent_txtIndicativePanel3').val());
                $('#MainContent_txtIndicativePanel3').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtReplacementFor').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtReplacementFor').text($('#MainContent_txtReplacementFor').val());
                $('#MainContent_txtReplacementFor').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtKeySkills').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtKeySkills').text($('#MainContent_txtKeySkills').val());
                $('#MainContent_txtKeySkills').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtBuisnessJustification').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtBuisnessJustification').text($('#MainContent_txtBuisnessJustification').val());
                $('#MainContent_txtBuisnessJustification').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtAdditionalInformation').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtAdditionalInformation').text($('#MainContent_txtAdditionalInformation').val());
                $('#MainContent_txtAdditionalInformation').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtComments').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtComments').text($('#MainContent_txtComments').val());
                $('#MainContent_txtComments').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtApproverName').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtApproverName').text($('#MainContent_txtApproverName').val());
                $('#MainContent_txtApproverName').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtBudget').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtBudget').text($('#MainContent_txtBudget').val());
                $('#MainContent_txtBudget').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtSLAForTechnology').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtSLAForTechnology').text($('#MainContent_txtSLAForTechnology').val());
                $('#MainContent_txtSLAForTechnology').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtTotalSLADays').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtTotalSLADays').text($('#MainContent_txtTotalSLADays').val());
                $('#MainContent_txtTotalSLADays').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_txtDesignation').prop('disabled')) {
                //textbox is disabled
                $('#MainContent_lbl_txtDesignation').text($('#MainContent_txtDesignation').val());
                $('#MainContent_txtDesignation').hide();
                //$('#Label18').hide();
            }
            if ($('#MainContent_ddlSLATypeName').prop('disabled')) {

                //textbox is disabled
                $('#MainContent_lbl_ddlSLATypeName').text($("#MainContent_ddlSLATypeName option:selected").text());
                $('#MainContent_ddlSLATypeName').hide().next().hide();
                //$('#Label18').hide();
            }

        });
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var title = "";
        function ValidateRecruiterSelection() {

            var selectedRecruiter = document.getElementById('<%=lstRecruiterName.ClientID %>').value;
            var selectedSLA = document.getElementById('<%= ddlSLATypeName.ClientID%>').value;
            if (selectedRecruiter == "") {
                V2hrmsAlert('<p>' + 'Please select a recruiter first.' + '</p>', title);
                return false;
            }
            else if (selectedSLA == "Select") {
                V2hrmsAlert('<p>' + 'Please select a SLA Type first.' + '</p>', title);
                return false;
            }
            else {
                document.getElementById('<%=pnlHRM.ClientID %>').style.display = 'none';
                document.getElementById('<%=btnBack.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }

    }

    function ValidateCancel() {

        var confirmation = confirm("Are you sure you want to Cancel this RRF?");
        if (confirmation) {
            document.getElementById('<%=pnlHRM.ClientID %>').style.display = 'none';
            document.getElementById('<%=btnBack.ClientID %>').style.display = 'none';
            DisplayLoadingDialog();
            //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';

            return true;
        }
        else
            return false;
    }
    </script>
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody Candidate ViewRRFMultiple clearfix HRM">
            <div class="clearfix">
                <div id="" class="">
                    <asp:Button ID="btnBack"
                        runat="server" Text="Back" Width="100px" CssClass="ButtonGray BackLink" OnClick="btnBack_Click" />
                    <asp:Button ID="btnRedirect" runat="server" Text="Back" Width="100px" OnClick="btnRedirect_Click"
                        Visible="false" CssClass="ButtonGray BackLink" />
                </div>
            </div>
            <div class="InnerContainer">
                <div class="">

                    <h3 class="smartrackH">
                        <asp:Label ID="lblWelcome" runat="server" Visible="true" SkinID="lblWelcome"></asp:Label></h3>
                    <asp:Label ID="lblError" runat="server" Width="100%" SkinID="lblError" Visible="false"></asp:Label>
                    <asp:Label ID="lblSuccess" runat="server" Width="100%" SkinID="lblSuccess" Visible="false"></asp:Label>
                    <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none; position: fixed; left: -1000px; top: -1000px;" runat="server"></asp:Image>
                    <section class="FormContainerBox">
                        <asp:Panel ID="pnlHRM" runat="server">
                            <div class="clearfix">
                                <div class="clearfix">
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>Requestor:</label>--%>
                                                    <asp:Label ID="lblRequestor" runat="server" Text="Requestor:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>Jayaraj Nadar</label>--%>
                                                    <asp:TextBox ID="txtRequestor" runat="server" ReadOnly="True" Enabled="False" BorderWidth="0"
                                                        Text="" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtRequestor" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>Request Date:</label>--%>
                                                    <asp:Label ID="lblRequestDate" runat="server" Text="Request Date:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>25/06/2014</label>--%>
                                                    <asp:TextBox ID="txtRequestDate" runat="server" ReadOnly="True" Enabled="False" Text=""
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtRequestDate" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>Expected Closure Date:</label>--%>
                                                    <asp:Label ID="lblExpectedClosureDate" runat="server" Text="Expected Closure Date:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>30/06/2014</label>--%>
                                                    <asp:TextBox ID="txtExpectedClosureDate" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtExpectedClosureDate" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>RRF No:</label>--%>
                                                    <asp:Label ID="lblRRFNo" runat="server" Text="RRF No:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>Te20134322</label>--%>
                                                    <asp:TextBox ID="txtRRFNo" runat="server" ReadOnly="True" Enabled="False" Text=""
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtRRFNo" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
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
                                                    <%--<label>For DU:</label>--%>
                                                    <asp:Label ID="lblForDU" runat="server" Text="For DU:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>Technology</label>--%>
                                                    <asp:TextBox ID="txtForDU" runat="server" ReadOnly="True" Enabled="False" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtForDU" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>for DT:</label>--%>
                                                    <asp:Label ID="lblForDT" runat="server" Text="For DT:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>QA-DT</label>--%>
                                                    <asp:TextBox ID="txtForDT" runat="server" ReadOnly="True" Enabled="False" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtForDT" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>Project Name:</label>--%>
                                                    <asp:Label ID="lblProjectName" runat="server" Text="Project Name:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>Vibrant Web</label>--%>
                                                    <asp:TextBox ID="txtProjectName" runat="server" ReadOnly="True" Enabled="False" Width="180px" Style="word-wrap: normal; word-break: break-all;"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtProjectName" runat="server" CssClass="ClassDisplayLabel" Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>Designation:</label>--%>
                                                    <asp:Label ID="lblDesignation" runat="server" Text="Designation:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>Sr. QA Engineer</label>--%>
                                                    <asp:TextBox ID="txtDesignation" runat="server" ReadOnly="True" Enabled="False" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtDesignation" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
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
                                                    <%--<label>Resource Pool:</label>--%>
                                                    <asp:Label ID="lblResourcePool" runat="server" Text="Resource Pool:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>QA</label>--%>
                                                    <asp:TextBox ID="txtResourcePool" runat="server" ReadOnly="True" Enabled="False"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtResourcePool" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>Positions Required:</label>--%>
                                                    <asp:Label ID="lblPositionsRequired" runat="server" Text="Positions Required:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>1</label>--%>
                                                    <asp:TextBox ID="txtPositionsRequired" runat="server" ReadOnly="True" Enabled="False"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtPositionsRequired" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="lblIndicativePanel11" runat="server" Text="Indicative Panel:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel1" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtIndicativePanel1" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
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
                                                    <%--<label>
                                                    Employment Type:</label>--%>
                                                    <asp:Label ID="lblEmploymentType" runat="server" Text="Employment Type:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>
                                                    Regular</label>--%>
                                                    <asp:TextBox ID="txtEmployementType" runat="server" ReadOnly="True" Enabled="False"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtEmployementType" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>
                                                    Experience:</label>--%>
                                                    <asp:Label ID="lblExperience" runat="server" Text="Experience:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>
                                                    6</label>--%>
                                                    <asp:TextBox ID="txtExperience" ReadOnly="True" Enabled="False" runat="server" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtExperience" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>
                                                    Is Replacement:</label>--%>
                                                    <asp:Label ID="lblIsReplacement" runat="server" Text="Is Replacement:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>
                                                    No</label>--%>
                                                    <asp:RadioButtonList ID="rdobtnIsReplacement" runat="server" Enabled="false" RepeatDirection="Horizontal"
                                                        AutoPostBack="True" CssClass="RadioButtonList">
                                                        <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>
                                                    Is Billable:</label>--%>
                                                    <asp:Label ID="lblBillable" runat="server" Text="Is Billable:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>
                                                    Yes</label>--%>
                                                    <asp:RadioButtonList ID="rdobtnIsBillable" runat="server" RepeatDirection="Horizontal"
                                                        CssClass="RadioButtonList" Enabled="false">
                                                        <asp:ListItem Text="Yes" Value="Yes">Yes</asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No" Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
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
                                                    <asp:Label ID="lblIndicativePanel22" runat="server" Text="Indicative Panel:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel2" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtIndicativePanel2" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="lblIndicativePanel33" runat="server" Text="Indicative Panel:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtIndicativePanel3" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtIndicativePanel3" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="lblReplacementFor" runat="server" Text="Replacement For:" Visible="false"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtReplacementFor" runat="server" ReadOnly="True" Enabled="False"
                                                        Visible="false" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtReplacementFor" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
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
                                                    <%-- <label>
                                                    Key Skills:</label>--%>
                                                    <asp:Label ID="lblKeySkills" runat="server" Text="Key Skills:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<div class="ClassTextareaDiv">
                                                </div>--%>
                                                    <asp:TextBox ID="txtKeySkills" runat="server" ReadOnly="True" Enabled="False" TextMode="MultiLine"
                                                        Height="100px" MaxLength="100" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtKeySkills" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>
                                                    Business Justification:</label>--%>
                                                    <asp:Label ID="lblBuisnessJustification" runat="server" Text="Business Justification:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<div class="ClassTextareaDiv">
                                                </div>--%>
                                                    <asp:TextBox ID="txtBuisnessJustification" ReadOnly="True" Enabled="False" runat="server"
                                                        TextMode="MultiLine" Height="100px" MaxLength="200" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtBuisnessJustification" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearL sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>
                                                    Additional Information:</label>--%>
                                                    <asp:Label ID="lblAdditionalInformation" runat="server" Text="Additional Information:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<div class="ClassTextareaDiv">
                                                </div>--%>
                                                    <asp:TextBox ID="txtAdditionalInformation" ReadOnly="True" Enabled="False" runat="server"
                                                        TextMode="MultiLine" Height="100px" Rows="3" cols="57" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtAdditionalInformation" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="tdComments" class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>
                                                    Comments:</label>--%>
                                                    <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<div class="ClassTextareaDiv">
                                                </div>--%>
                                                    <asp:TextBox ID="txtComments" ReadOnly="True" Enabled="False" runat="server" TextMode="MultiLine"
                                                        Height="100px" Rows="3" cols="57" Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtComments" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearL sec1C ViewRRF clearfix">
                                        <div class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>
                                                    Approver:</label>--%>
                                                    <asp:Label ID="lblApproverName" runat="server" Text="Approver:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<label>
                                                    Lakshmi Raghvendra Murthy:</label>--%>
                                                    <asp:TextBox ID="txtApproverName" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtApproverName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="trBudget" runat="server" class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%--<label>
                                                    Budget:</label>--%>
                                                    <asp:Label ID="lblBudget" runat="server" Text="Budget:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%-- <label>
                                                    10 Lacs P.a</label>--%>
                                                    <asp:TextBox ID="txtBudget" ReadOnly="True" Enabled="False" runat="server" Width="180px"
                                                        TabIndex="0"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtBudget" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                    <asp:Label ID="Label1" runat="server" CssClass="exampleNumber" Text="lacs p.a."></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                        <div class="clearL sec1C ViewRRF clearfix">
                                            <div class="leftcol">
                                                <div class="formrow clearfix">
                                                    <div class="LabelDiv biglabel">
                                                        <asp:Label ID="lblSLAForTechnology" runat="server" Text="Technology:" />
                                                    </div>
                                                    <div class="InputDiv">
                                                        <asp:TextBox ID="txtSLAForTechnology" ReadOnly="True" Enabled="False" runat="server"
                                                            Width="180px"></asp:TextBox>
                                                        <asp:Label ID="lbl_txtSLAForTechnology" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--     <div class="rightcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <asp:Label ID="lblTotalSLADays" runat="server" Text="Total SLA Days:" />
                                                </div>
                                                <div class="InputDiv">
                                                    <asp:TextBox ID="txtTotalSLADays" ReadOnly="True" Enabled="False" runat="server"
                                                        Width="180px"></asp:TextBox>
                                                    <asp:Label ID="lbl_txtTotalSLADays" runat="server"  CssClass="ClassDisplayLabel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>--%>
                                            <div class="rightcol" id="trSLAType" runat="server">
                                                <div class="formrow clearfix">
                                                    <div class="LabelDiv">
                                                        <asp:Label ID="lblSLAType" runat="server" Text="*SLA Type:" />
                                                    </div>
                                                    <div class="InputDiv">
                                                        <asp:DropDownList ID="ddlSLATypeName" runat="server" CssClass="ClassDisabledFields"></asp:DropDownList>
                                                        <asp:Label ID="lbl_ddlSLATypeName" runat="server" CssClass="ClassDisplayLabel"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearL sec1C ViewRRF clearfix">
                                        <div id="trRecruiter" runat="server" class="leftcol">
                                            <div class="formrow clearfix">
                                                <div class="LabelDiv">
                                                    <%-- <label>
                                                    Recruiter Name:</label>--%>
                                                    <asp:Label ID="lblRecruiter" runat="server" Text="*Recruiter Name:"></asp:Label>
                                                </div>
                                                <div class="InputDiv">
                                                    <%--<div class="ClassTextareaDiv">
                                                </div>--%>
                                                    <asp:ListBox ID="lstRecruiterName" runat="server" Height="100px" Width="180px" TabIndex="1"></asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </section>
                </div>
            </div>

            <div id="trSendForApproval" runat="server" class="ButtonContainer1 clearfix">
                <%--<input type="button" class="ButtonGray" value="Confirm and Assign">
                    <input type="button" class="ButtonGray" value="Cancel RRF">--%>
                <asp:Button ID="btnConfirmAndAssign" runat="server" Text="Confirm and Assign"
                    OnClick="btnConfirmAndAssign_Click" OnClientClick="javascript:return ValidateRecruiterSelection()"
                    TabIndex="2" CssClass="ButtonGray" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel RRF" OnClientClick="javascript:return ValidateCancel()"
                    OnClick="btnCancel_Click" TabIndex="3" CssClass="ButtonGray" />
            </div>
        </div>
    </section>
</asp:Content>