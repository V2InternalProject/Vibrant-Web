<%@ Page Language="C#" AutoEventWireup="true" Inherits="SLAForRRF"
    MasterPageFile="../Views/Shared/HRMS.Master" CodeBehind="SLAForRRF.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" type="text/css" />
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" runat="server" />
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody SLAStatusBody reInterview">
            <div class="InnerContainer rwrap">
                <div class="clearfix">
                    <asp:Button ID="btnBack" OnClick="btnBack_Click"
                        runat="server" Text="Back" CssClass="floatL BackLink" />
                </div>
                <h3 class="SmallHeadingBold clearL">SLA Status</h3>
                <asp:Label ID="lblMessage" Visible="false" runat="server" Text="No Record Found"
                    SkinID="lblError" CssClass="ErrorMsgOrbit"></asp:Label>

                <div class="statuswrapR">
                    <label class="proceed mrgnR45">Not Proceeded</label>
                    <label class="clear mrgnR45">Met SLA</label>
                    <label class="reject mrgnR45">Not Met SLA</label>
                </div>
                <div class="clearfix mrgnT10">
                    <div class="mrgnR50 SLALabels">
                        <div class="LabelDiv">
                            <asp:Label ID="Label1" runat="server" SkinID="lblSubheader" Text="RRF No :"></asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblRRFNO" runat="server" SkinID="lblSubheader"></asp:Label>
                        </div>
                    </div>
                    <div class="SLALabels mrgnR50">
                        <div class="LabelDiv">
                            <asp:Label ID="lblRRFApprove" runat="server" SkinID="lblSubheader" Text="RRF Approved Date:"></asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblRRFApproveDate" runat="server" SkinID="lblSubheader"></asp:Label>
                        </div>
                    </div>
                </div>

                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="TableJqgrid SLARRFTable">
                    <thead>
                        <tr>
                            <th class="tableHeaders" width="19%">SLA Stage</th>
                            <th class="tableHeaders" width="18%">Lapse Date For Stages</th>
                            <th class="tableHeaders" width="62%">Progress</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="tableRows">
                            <td class="style2">
                                <asp:Label ID="Label4" runat="server" Text="RRF Accepted"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStage1LapseDate" runat="server" />
                            </td>
                            <td class="style2">
                                <asp:Image ID="imgRRFAccepted" runat="server" ImageUrl="~/Images/NotProceeded.png" />
                            </td>
                        </tr>
                        <tr class="tableRows">
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Interview Scheduled"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStage2LapseDate" runat="server" />
                            </td>
                            <td>
                                <asp:Image ID="imgInterviewScheduled" runat="server" ImageUrl="~/Images/NotProceeded.png" />
                            </td>
                        </tr>
                        <tr class="tableRows">
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Candidate Selected"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStage3LapseDate" runat="server" />
                            </td>
                            <td>
                                <asp:Image ID="imgCandidateSelected" runat="server" ImageUrl="~/Images/NotProceeded.png" />
                            </td>
                        </tr>
                        <tr class="tableRows">
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Offer Generated"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStage4LapseDate" runat="server" />
                            </td>
                            <td>
                                <asp:Image ID="imgOfferGenerated" runat="server" ImageUrl="~/Images/NotProceeded.png" />
                            </td>
                        </tr>
                        <tr class="FooterRow">
                            <td colspan="3"></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </section>
</asp:Content>