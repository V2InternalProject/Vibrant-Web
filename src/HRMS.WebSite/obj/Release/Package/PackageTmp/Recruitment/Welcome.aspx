<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.Master" AutoEventWireup="true" Inherits="Welcome" CodeBehind="Welcome.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>

        <div class="MainBody LandingPage">
            <p>Welcome to</p>
            <div>Talent Acquisition System</div>
        </div>
        <%--<asp:Image ID="imgwelcome" ImageUrl="Images/Welcome.jpg" runat="server"
        Height="339px" Width="779px" />
           </section>--%>
</asp:Content>