<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true" CodeBehind="SigninPage.aspx.cs" Inherits="HRMS.Orbitweb.SigninPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
       
        <script src="JavaScript/common.js" type="text/javascript"></script>
        <!-- SelectBOx -->
       
        <script src="JavaScript/jquery.selectbox-0.2.min.js" type="text/javascript"></script>
 <script type="text/javascript">

     //New addition

     $(document).ready(function () {

         if ($('#ctl00_ContentPlaceHolder1_ddlType').val() != "7") {
             $('.OrbitFilterExpand').show();
         }
         $('.OrbitFilterLink').click(function () {

             $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 1000);
         });
     });


     $(function () {
         $('select').selectbox();
         $('.sbOptions a').hover(function () {
             $(this).parent().toggleClass("hoveroption");
         });
     });
        </script>

        <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
        
            <div class="FixedHeader">
                <div class="clearfix">
                    <h2 class="MainHeading">
                        Attendance</h2>
                    <div class="EmpSearch clearfix">
                        <a href="#"></a>
                        <input type="text" placeholder="Employee Search">
                    </div>
                </div>
                <nav class="sub-menu-colored">
                    <a href="SignInSignOut.aspx" class="selected">Auto</a> <a href="ManualSignInSignOut.aspx">
                        Manual</a>
                </nav>
            </div>
            <div class="MainBody">
                <div align="center" class="tableHeadBlueLight">
                    Orbit Login
                </div>
                <div align="center">
                    <asp:Label ID="lblSuccess" runat="server" SkinID="lblSuccess"></asp:Label>
                </div>
                <div align="center">
                    <asp:Label ID="lblErrorMess" runat="server" SkinID="lblError"></asp:Label>
                </div>
                <div class="OrbitAuto">
                    <div class="clearfix">
                        <div class="floatL mrgnL20">
                            <%--                            <input type="button" value="Sign In" class="ButtonBlue mrgnR11">
                            <input type="button" value="Sign Out" class="ButtonSignOut">--%>
                            <asp:Button ID="btnSignIn" runat="server" Text="Sign In" OnClick="btnSignIn_Click"
                                CausesValidation="False" CssClass="ButtonBlue mrgnR11" />
                            &nbsp; &nbsp;
                            <asp:Button ID="btnSignOut" runat="server" OnClick="btnSignOut_Click" Text="Sign Out"
                                CausesValidation="False" CssClass="ButtonSignOut" />
                        </div>
                        <div class="OrbitFilter">
                            <a href="#" class="OrbitFilterLink floatR">Filters</a>
                        </div>
                        <div class="OrbitFilterExpand" style="display: none;">
                            <asp:DropDownList Width="150px" ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                AutoPostBack="True">
                                <asp:ListItem Value="7">All</asp:ListItem>
                                <asp:ListItem Value="0">Late </asp:ListItem>
                                <asp:ListItem Value="1">Early Leavers</asp:ListItem>
                                <asp:ListItem Value="2">Absent</asp:ListItem>
                                <asp:ListItem Value="3">Present </asp:ListItem>
                                <asp:ListItem Value="4">Pending</asp:ListItem>
                                <asp:ListItem Value="5">Rejected</asp:ListItem>
                                <asp:ListItem Value="6">Approved</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType"
                                ErrorMessage="Please Select The Type" InitialValue="-1" SetFocusOnError="True"
                                Display="None"></asp:RequiredFieldValidator>
                            <%--<input type="text" placeholder="From Date">--%>
                            <asp:TextBox ID="txtSearchFromDate" runat="server" MaxLength="256" placeholder="From Date"></asp:TextBox>
                            <asp:ImageButton ImageUrl="images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchFromDate"
                                ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchFromDate" runat="server"
                                TargetControlID="txtSearchFromDate" PopupButtonID="imgbtnSearchFromDate" />
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="Please Select the From Date"
                                ControlToValidate="txtSearchFromDate" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvSISODates" runat="server" ControlToCompare="txtSearchFromDate"
                                ControlToValidate="txtSearchToDate" Display="None" ErrorMessage="To Date cannot be samaller than the From Date"
                                Operator="GreaterThanEqual" Type="Date" Visible="true"></asp:CompareValidator>
                            <%--<img src="../../Images/New Design/calender-icon.png" class="ui-datepicker-trigger mrgnR12">--%>
                            <%--<input type="text" placeholder="To Date">--%>
                            <%--                            <asp:TextBox ID="TextBox1" Width="100px" runat="server" MaxLength="256" placeholder="To Date"></asp:TextBox>
                            <asp:ImageButton ImageUrl="../../Images/New Design/calender-icon.png" runat="server"
                                ID="ImageButton1" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSearchToDate"
                                PopupButtonID="imgbtnSearchToDate" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSearchToDate"
                                Display="None" ErrorMessage="Please Select the To Date" SetFocusOnError="True"></asp:RequiredFieldValidator>&nbsp;--%>
                            <asp:TextBox ID="txtSearchToDate" runat="server" MaxLength="256" placeholder="To Date"></asp:TextBox>
                            <asp:ImageButton ImageUrl="images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchToDate"
                                ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate" runat="server" TargetControlID="txtSearchToDate"
                                PopupButtonID="imgbtnSearchToDate" />
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtSearchToDate"
                                Display="None" ErrorMessage="Please Select the To Date" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <%--<img src="../../Images/New Design/calender-icon.png" class="ui-datepicker-trigger mrgnR12">--%>
                            <%--<asp:Button ID="Button2" runat="server" OnClick="btnSearch_Click" CssClass="OrbitFilterImage"/>--%>
                            <asp:Button type="Button2" class="OrbitFilterImage" ID="btnSearch" runat="server"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="Button1" runat="server" OnClick="btnReset_Click" CssClass="OrbitFilterImage" />
                        </div>
                    </div>
                </div>
                <div class="OrbitNote">
                    *Please note that records shown In BOLD are for either a Sunday/Saturday or a Holiday.</div>
                <div>
                    <asp:GridView ID="grdSignInSignOut" Width="100%" runat="server" OnSelectedIndexChanged="grdSignInSignOut_SelectedIndexChanged"
                        OnDataBound="grdSignInSignOut_DataBound" OnRowDataBound="grdSignInSignOut_RowDataBound"
                        OnRowEditing="grdSignInSignOut_RowEditing" AllowPaging="True" AllowSorting="True"
                        OnPageIndexChanging="gridView_PageIndexChanging" OnSorting="gridView_Sorting"
                        AutoGenerateColumns="False" OnRowCommand="grdSignInSignOut_RowCommand" DataKeyNames="SignInSignOutID"
                        CssClass="TableJqgrid">
                        <HeaderStyle CssClass="tableHeaders"/>
                        <RowStyle CssClass="tableRows"/>
                        <Columns>
                            <asp:TemplateField HeaderText="SignInSignOutID" SortExpression="SignInSignOutID"
                                Visible="False">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSignInSignOutID" runat="server" Text='<%# Bind("SignInSignOutID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" SortExpression="date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("date") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkDate" Font-Underline="true" Text='<%# Bind("date") %>' runat="server"
                                        CommandName="Date" CausesValidation="false" CommandArgument='<%#  Eval("SignInSignOutID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time" SortExpression="SignInTime">
                                <ItemTemplate>
                                    <asp:Label ID="lblInTime" runat="server" Text='<%# Bind("SignInTime") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkInTime" Font-Underline="true" Text='<%# Bind("SignInTime") %>'
                                        runat="server" CommandName="InTime" CausesValidation="false" CommandArgument='<%#  Eval("SignInSignOutID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time" SortExpression="SignOutTime">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutTime" runat="server" Text='<%# Bind("SignOutTime") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkOutTime" Font-Underline="true" Text='<%# Bind("SignOutTime") %>'
                                        runat="server" CommandName="OutTime" CausesValidation="false" CommandArgument='<%#  Eval("SignInSignOutID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalHoursWorked" HeaderText="Total Hours" SortExpression="TotalHoursWorked" />
                            <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                            <asp:BoundField DataField="SignInComment" HeaderText="Sign In Comments" SortExpression="SignInComment" />
                            <asp:BoundField DataField="SignOutComment" HeaderText="Sign Out Comments" SortExpression="SignOutComment" />
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus1" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EmployeeName" HeaderText="Approver" SortExpression="EmployeeName" />
                            <asp:BoundField DataField="ApproverComments" HeaderText="Approver Comments" SortExpression="ApproverComments" />
                            <asp:TemplateField HeaderText="Action" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="false" CommandName="EditSignInSignOut"
                                        CommandArgument='<%#  Eval("SignInSignOutID") %>'>Edit</asp:LinkButton>
                                    <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" />
                </div>
            </div>

</asp:Content>
