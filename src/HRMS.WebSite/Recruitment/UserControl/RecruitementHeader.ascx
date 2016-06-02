<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="RecruitementHeader.ascx.cs" Inherits="HRMS.Recruitment.UserControl.RecruitementHeader" %>
<div class="FixedHeader">
    <div class="clearfix">
        <h2 class="MainHeading">Talent Acquisition</h2>
    </div>
    <nav class="sub-menu-colored">
        <%if (Session["HeaderCheck"] == "master")
          {%>
        <asp:LinkButton ID="masterTable" Text="MasterTable" CssClass="selected" Visible="false" runat="server" OnClick="masterTable_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="masterTableTemp" Text="MasterTable" Visible="false" runat="server" OnClick="masterTable_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "Recruiter")
          {%>
        <asp:LinkButton ID="Recruiter" Text="Recruiter" CssClass="selected" runat="server" Visible="false" OnClick="Recruiter_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="RecruiterTemp" Text="Recruiter" runat="server" Visible="false" OnClick="Recruiter_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "RRF List")
          {%>
        <asp:LinkButton ID="RRFRequestor" Text="Requestor" runat="server" CssClass="selected" Visible="false" OnClick="RRFRequestor_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="RRFRequestorTemp" Text="Requestor" runat="server" Visible="false" OnClick="RRFRequestor_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "RRF Approver List")
          {%>
        <asp:LinkButton ID="RRFApprover" Text="RRF Approver" runat="server" CssClass="selected" Visible="false" OnClick="RRFApprover_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="RRFApproverTemp" Text="RRF Approver" runat="server" Visible="false" OnClick="RRFApprover_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "Candidate")
          {%>
        <asp:LinkButton ID="Candidate" Text="Candidate" runat="server" CssClass="selected" Visible="false" OnClick="Candidate_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="CandidateTemp" Text="Candidate" runat="server" Visible="false" OnClick="Candidate_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "HRM RRFList")
          {%>
        <asp:LinkButton ID="HRM" Text="HRM List" runat="server" CssClass="selected" Visible="false" OnClick="HRM_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="HRMTemp" Text="HRM List" runat="server" Visible="false" OnClick="HRM_Click"></asp:LinkButton>
        <%} %>

        <%if (Session["HeaderCheck"] == "Interview")
          {%>
        <asp:LinkButton ID="Interviewer" Text="Interviewer" CssClass="selected" runat="server" Visible="false" OnClick="Interviewer_Click"></asp:LinkButton>
        <%}
          else
          { %>
        <asp:LinkButton ID="InterviewerTemp" Text="Interviewer" runat="server" Visible="false" OnClick="Interviewer_Click"></asp:LinkButton>
        <%} %>
    </nav>
</div>

<%-- <script type="text/javascript">
       $(document).ready(function () {

           $('#masterTable').click(function () {

               (this).addClass('selected');

               $('a').not(this).removeClass("selected");
           });
           $('#Recruiter').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });
           $('#RRFRequestor').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });
           $('#RRFApprover').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });
           $('#Candidate').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });
           $('#HRM').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });
           $('#Interviewer').click(function () {
               (this).addClass('selected');
               $('a').not(this).removeClass("selected");
           });

       });
       </script>--%>