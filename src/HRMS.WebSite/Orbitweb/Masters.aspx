<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="Masters.aspx.cs" Inherits="HRMS.Orbitweb.Masters" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register Src="ConfigItems.ascx" TagName="ConfigItemsTag" TagPrefix="CI" %>--%>
<%--<%@ Register Src="Status.ascx" TagName="StatusTag" TagPrefix="ST" %>--%>
<%--<%@ Register Src="MonthlyLeaveUpload.ascx" TagName="MonthlyLeaveUploadTag" TagPrefix="MLU" %>--%>
<%--<%@ Register Src="ShiftMaster.ascx" TagName="ShiftMasterTag" TagPrefix="SM" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
   <link href="../../Content/New Design/jquery.selectbox.css" type="text/css" rel="stylesheet" />
  <%--  <script type="text/javascript" src="../../Scripts/New Design/jquery.selectbox-0.2.min.js"></script>--%>
    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/New%20Design/common.js" type="text/javascript"></script>

   
    <script language="javascript" type="text/javascript">
        //$(function () {
        //    $('select').selectbox();
        //});
        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });
        });
        function validation(HolidaysDescription, txtHolidayDate) {
            if (txtHolidayDate.value == "") {
                alert("Please enter the Holidays Date");
                HolidaysDescription.focus();
                return false;
            }
            else if (HolidaysDescription.value == "") {
                alert("Please enter the Holidays Description");
                HolidaysDescription.focus();
                return false;
            }

        }
        function spcharacter(input) {
            var txtbox = input.value;
            var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?_";
            for (var i = 0; i < txtbox.length; i++) {
                if (iChars.indexOf(txtbox.charAt(i)) != -1) {
                    alert("Please Dont enter Special Characters");
                    input.value = "";
                    return false;
                }
            }
            return true;
        }

        function validation(ConfigItemValue) {
            if (ConfigItemValue.value == "") {
                alert("Please enter the Config Item Value");
                return false;
            }
        }



    </script>
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <section class="AttendanceContainer Container">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">
                    Administration</h2>
                <%-- <div class="EmpSearch clearfix">
                    <a href="#"></a>
                    <input type="text" placeholder="Employee Search">
                </div>--%>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                    Transaction</a> <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%> <a href="BulkEntries.aspx">
                        Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a><%--<a
                            href="StartWorkflows.aspx">Manage Processes</a>--%> <a href="Masters.aspx" class="selected">
                                Masters</a>
            </nav>
        </div>
        <div class="MainBody MastersContainer">
            <div class="master-tabs clearfix">
                <div class="tabs">
                    <ul class="leave-mgmt-tabs">
                        <li id="tab1">Holiday List</li>
                        <li id="tab2">Configure Settings</li>
                        <li id="tab3">Status Master</li>
                        <li id="tab4">Monthly Leave Upload</li>
                        <li id="tab5">Shift Details</li>
                    </ul>
                </div>
                <section class="add-detailsdata clearfix">
                    <%--  <div class="clearfix OrbitAuto AdminApproval">--%>
                    <div class="clearfix">
                        <div>
                            <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                            <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
                        </div>
                        <p class="OrbitNote">
                            * Previous date entry is not Added or Edited
                        </p>
                        <div class="leftcol">
                            <div class="formrow clearfix selectYr">
                                <div class="LabelDiv">
                                    <asp:Label ID="lblSelectYear" Text="Select Year:" runat="server"></asp:Label></div>
                                <%--<asp:Label ID="Label1" Text=":" Font-Bold="true" runat="server"></asp:Label>--%>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="DDlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDlYear_SelectedIndexChanged"
                                        CausesValidation="True">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--</div>--%>
                    <!--Gridview here-->
                    <asp:DataGrid ID="grdHolidayList" runat="server" ShowFooter="True" PageSize="1000"
                        OnEditCommand="grdHolidayList_EditCommand" AutoGenerateColumns="False" AllowPaging="True"
                        Width="100%" OnItemCommand="grdHolidayList_ItemCommand" OnItemDataBound="grdHolidayList_ItemDataBound"
                        CssClass="TableJqgrid mrgnT20">
                        <EditItemStyle VerticalAlign="Middle" HorizontalAlign="Left"></EditItemStyle>
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" CssClass="tableRows"></ItemStyle>
                         <HeaderStyle CssClass="tableHeaders" />
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                        <FooterStyle CssClass="tableRows"></FooterStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Holidays ID" Visible="False">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblHolidayID" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                <FooterTemplate>
                                    <asp:Label ID="lblHolidayID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="lblHolidayID2" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Holidays Date" ItemStyle-Width="36%">
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblHolidayDate" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtHolidaydate" runat="server" Height="28px" MaxLength="50" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                                    </asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchToDate1"
                                        ImageAlign="Middle" CssClass="ui-datepicker-trigger"/>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate1" runat="server" TargetControlID="txtHolidayDate"
                                        PopupButtonID="imgbtnSearchToDate1" />
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtHolidayDate1" runat="server" MaxLength="50" Height="28px" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                                    </asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="imgbtnSearchToDate2"
                                        ImageAlign="Middle" CssClass="ui-datepicker-trigger"/>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate2" runat="server" TargetControlID="txtHolidayDate1"
                                        PopupButtonID="imgbtnSearchToDate2" />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Holidays Description">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblHolidayDescriptio" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDescription") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtHolidayDescription" runat="server" MaxLength="50"></asp:TextBox>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtHolidayDescription1" runat="server" Width="90%" MaxLength="50"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDescription") %>'>
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="IsHolidayForShift">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsForShift" runat="server" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsHolidayForShift") )%>' />
                                    <asp:label runat="server" AssociatedControlID="chkIsForShift" class="LabelForCheckbox"></asp:label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkIsForShiftFooter" runat="server" />
                                    <asp:label runat="server" AssociatedControlID="chkIsForShiftFooter" class="LabelForCheckbox"></asp:label>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkIsForShiftEdit" runat="server" Checked='<%#  Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsHolidayForShift")) %>' />
                                     <asp:label runat="server" AssociatedControlID="chkIsForShiftEdit" class="LabelForCheckbox"></asp:label>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Action">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbutEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAdd" runat="server" Text="ADD" CommandName="btnAdd"></asp:LinkButton>
                                    <asp:LinkButton ID="Linkbutton1" runat="server" Text="Cancel" CommandName="btnCancel"
                                        CausesValidation="false"></asp:LinkButton>
                                </FooterTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkbutUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkbutCancel" runat="server" Text="Cancel" CommandName="btnCancel"
                                        CausesValidation="false"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </section>
               <%-- <section class="search-detailsdata clearfix">
                    <!--	gridview here (configure settings)-->
                    <CI:ConfigItemsTag ID="ConfigItems1" runat="server" />
                </section>
                <section class="holiday-listdata clearfix">
                    <!--   gridview here (Status Master)-->
                    <ST:StatusTag ID="Status1" runat="server" />
                </section>
                <section class="tab-panel4 clearfix">
                    <!-- gridview here (monthly leave upload_)-->
                    <MLU:MonthlyLeaveUploadTag ID="MonthlyLeaveUpload1" runat="server" />
                </section>
                <section class="tab-panel5 clearfix">
                    <!--gridview here (shift master)-->
                    <SM:ShiftMasterTag ID="ShiftMaster1" runat="server" />
                </section>--%>
            </div>
        </div>
    </section>
</asp:Content>
