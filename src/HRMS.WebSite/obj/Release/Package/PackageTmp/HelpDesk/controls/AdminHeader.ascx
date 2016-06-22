<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AdminHeader.ascx.cs"
    Inherits="V2.Helpdesk.web.controls.AdminHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/Global.min.js" type="text/javascript"></script>
<script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
<script type="text/javascript">    function mmLoadMenus() {

        if (window.mm_menu_0929210101_1) return;
        window.mm_menu_0929210101_1 = new Menu("root", 190, 18, "Verdana, Arial, Helvetica, sans-serif", 12, "#FFFFFF", "#FFFFFF", "#5ca0d9", "#3dd9a0", "center", "middle", 3, 0, 500, -5, 7, true, true, true, 0, true, true);
        mm_menu_0929210101_1.addMenuItem("Issue Memberwise", "location='MemberWiseSearchreport.aspx'");
        mm_menu_0929210101_1.addMenuItem("Issue Departmentwise", "location='CategorywisesearchReport.aspx'");
        mm_menu_0929210101_1.addMenuItem("Resolution Time", "location='ResolutionTimeReport.aspx'");
        mm_menu_0929210101_1.addMenuItem("Severity Wise", "location='IssueStatusReport.aspx'");
        mm_menu_0929210101_1.addMenuItem("Summary Report Department", "location='SummaryReport.aspx'");
        mm_menu_0929210101_1.addMenuItem("SLA Report", "location='SLAreport.aspx'");
        //mm_menu_0929210101_1.addMenuItem("Summary Report Employee","location='SummaryEmployeeWise.aspx'");
        mm_menu_0929210101_1.hideOnMouseOut = true;
        mm_menu_0929210101_1.bgColor = '#555555';
        mm_menu_0929210101_1.menuBorder = 1;
        mm_menu_0929210101_1.menuLiteBgColor = '#FFFFFF';
        mm_menu_0929210101_1.menuBorderBgColor = '#777777';

        window.mm_menu_0929210101_0 = new Menu("root", 180, 18, "Verdana, Arial, Helvetica, sans-serif", 12, "#FFFFFF", "#FFFFFF", "#5ca0d9", "#3dd9a0", "center", "middle", 3, 0, 500, -5, 7, true, true, true, 0, true, true);

        mm_menu_0929210101_0.addMenuItem("User Master", "location='../admin/ViewEmployeeDetails.aspx'");
        //mm_menu_0929210101_0.addMenuItem("Department Master","location='../admin/categoryMaster.aspx'");
        mm_menu_0929210101_0.addMenuItem("Category Master", "location='../admin/subCategoryMaster.aspx'");
        mm_menu_0929210101_0.addMenuItem("Severity Master", "location='../admin/ProblemSeverityMaster.aspx'");
        mm_menu_0929210101_0.addMenuItem("Resolution Time Master", "location='../admin/ResolutionTimeMaster.aspx'");

        //mm_menu_0929210101_0.addMenuItem("Priority Master","location='../admin/ProblemPriorityMaster.aspx'");
        //mm_menu_0929210101_0.addMenuItem("Status Master","location='../admin/StatusMaster.aspx'");
        mm_menu_0929210101_0.hideOnMouseOut = true;
        mm_menu_0929210101_0.bgColor = '#555555';
        mm_menu_0929210101_0.menuBorder = 1;
        mm_menu_0929210101_0.menuLiteBgColor = '#FFFFFF';
        mm_menu_0929210101_0.menuBorderBgColor = '#777777';

        mm_menu_0929210101_0.writeMenus();
    } // mmLoadMenus()
    function highlight_link(id1, id2) {
        if (id1 == 'td1') {
            document.getElementById(id1).innerHTML = '<span style="color:red; background:#57BDEE;">Column-1</span>';
            document.getElementById(id2).innerHTML = '<span>Column-2</span>';
        } else if (id1 == 'td2') {
            document.getElementById(id1).innerHTML = '<span style="color:red; background:#57BDEE;">Column-2</span>';
            document.getElementById(id2).innerHTML = '<span>Column-1</span>';
        } else {
            document.getElementById(id1).innerHTML = '<span>Column-1</span>';
            document.getElementById(id2).innerHTML = '<span>Column-2</span>';
        }

    }
    function show_hide_div(id, id2) {
        alert("hrere")
        var id_var = "" + id;
        alert('hello' + id_var)
        if (id_var != 'undefined') {
            el = document.getElementById(id_var);
            e2 = document.getElementById(id2);
            if (el.style.display == 'none') {
                e2.style.display = 'none';
                el.style.display = '';
            } /*else {
el.style.display = 'none';
}*/
        }
    }

    $(document).ready(function () {
        $("#MainContent_AdminHeader1_hdnTabClick").hide();
        if ($("#MainContent_AdminHeader1_hdnTabClick").val() == 'IssueHealth')
            $("#issueHealth").addClass('selected');
        else if ($("#MainContent_AdminHeader1_hdnTabClick").val() == 'ViewMyIssue')
            $("#ViewMyIssue").addClass('selected');
        else if ($("#MainContent_AdminHeader1_hdnTabClick").val() == 'SuperAdminIssue')
            $("#viewSuperAdminIssue").addClass('selected');
        else if ($("#MainContent_AdminHeader1_hdnTabClick").val() == 'EmpDetails')
            $("#viewEmpDetails").addClass('selected');
        else if ($("#MainContent_AdminHeader1_hdnTabClick").val() == 'SearchReport')
            $("#searchReport").addClass('selected');
        else
            $("#issueHealth").addClass('selected');

        //    $("#issueHealth").click(function () {

        //        $("#hdnTabClick").val('IssueHealth');
        //        document.getElementById("hdnTabClick").value = 'IssueHealth';
        //        var selItem = document.getElementById("hdnTabClick");
        //    });
        //    $("#ViewMyIssue").click(function () {

        //        $("#hdnTabClick").val('ViewMyIssue');
        //    });
        //    $("#viewSuperAdminIssue").click(function () {

        //        $("#hdnTabClick").val('SuperAdminIssue');
        //    });
        //    $("#viewEmpDetails").click(function () {

        //        $("#hdnTabClick").val('EmpDetails');
        //    });
        //    $("#searchReport").click(function () {

        //        $("#hdnTabClick").val('SearchReport');
        //    });

    });
</script>
<script type="text/javascript" language="JavaScript" src="../Script/mm_menu.js"></script>
<div class="FixedHeader">
    <div class="clearfix">
        <h2 class="MainHeading">HelpDesk Admin</h2>
    </div>
    <nav class="sub-menu-colored">
        <a id="issueHealth" href="IssueHealth.aspx?PageSource=IssueHealth">Issues Health
        </a><a id="ViewMyIssue" href="ViewMyIssues.aspx?PageSource=ViewMyIssue">My Issue</a>
        <a id="viewSuperAdminIssue" href="viewSuperAdminIssues.aspx?PageSource=SuperAdminIssue">Assign Issues </a><a id="viewEmpDetails" href="ViewEmployeeDetails.aspx?PageSource=EmpDetails">Masters</a> <a id="searchReport" href="MemberWiseSearchreport.aspx?PageSource=SearchReport">Reports</a>
    </nav>
    <asp:TextBox ID="hdnTabClick" hidden="true" runat="server" />
</div>
<%--
<div class="AdminHeaderContainer clearfix">
<h1 style="color:#3F93B8;margin-left:50px;">Helpdesk System</h1>
<ul>
    <li></li>
    <li></li>
    <li></li>
    <li>
        <ul>
            <li><a href="../admin/ViewEmployeeDetails.aspx">User Master</a></li>
            <li><a href="../admin/subCategoryMaster.aspx">Category Master</a></li>
            <li><a href="../admin/ProblemSeverityMaster.aspx">Severity Master</a></li>
            <li><a href="../admin/ResolutionTimeMaster.aspx">Resolution Time Master</a></li>
        </ul>
    </li>
    <li>
        <ul>
            <li><a href="MemberWiseSearchreport.aspx">Issue Memberwise</a></li>
            <li><a href="CategorywisesearchReport.aspx">Issue Departmentwise</a></li>
            <li><a href="ResolutionTimeReport.aspx">Resolution Time</a></li>
            <li><a href="IssueStatusReport.aspx">Severity Wise</a></li>
            <li><a href="SummaryReport.aspx">Summary Report Department</a></li>
            <li><a href="SLAreport.aspx">SLA Report</a></li>
        </ul>
    </li>
    <li style="float:right;">
    </li>
</ul>
</div>--%>
<%--<asp:LinkButton ID="lnkBtnLogin" Visible="False" runat="server" OnClick="lnkBtnLogin_Click"></asp:LinkButton>--%>
<script type="text/javascript" language="JavaScript">    mmLoadMenus();</script>