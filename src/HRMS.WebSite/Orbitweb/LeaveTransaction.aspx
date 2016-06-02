<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="LeaveTransaction.aspx.cs" Inherits="HRMS.Orbitweb.LeaveTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />

    <%--<link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />--%>
    <script language="javascript">
        function spcharacter(input) {
            // var txtQuantityAdd =txtQuantityAdd

            var txtbox = input.value;
            var iChars = "!#@$%^&*()+=[]\\\';,/{}|\":<>?_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // var iChars ="(-)?\d+(\.\d\d)?$"
            for (var i = 0; i < txtbox.length; i++) {

                if (iChars.indexOf(txtbox.charAt(i)) != -1) {
                    alert("Not allowed to enter characters and special characters ");
                    input.value = "";
                    return false;
                }
            }
        }
        function doValue(txtTransDateAdd, txtDescriptionAdd, txtQuantityAdd) {
            var txtTransDateAdd = document.getElementById(txtTransDateAdd).value;
            var txtDescriptionAdd = document.getElementById(txtDescriptionAdd).value;
            var txtQuantityAdd = document.getElementById(txtQuantityAdd).value;
            if (txtTransDateAdd == "") {
                alert("Please select date")
                return false;
            }
            if (txtDescriptionAdd == "") {
                alert("Please enter description details")
                return false;
            }

            //            if (txtQuantityAdd.value==0)
            //            {
            //                alert ("Please enter leave quantity")
            //                return false;
            //            }
            if (txtQuantityAdd == "" || txtQuantityAdd == 0) {
                alert("Please enter leave quantity")
                return false;
            }
            if (txtQuantityAdd % .5 != 0) {
                alert("Enter Correct value.")
                return false;
            }
        }

        function doValidation(input) {
            var txtbox = input.value;

            var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?0123456789";

            for (var i = 0; i < txtbox.length; i++) {

                if (iChars.indexOf(txtbox.charAt(i)) != -1) {
                    if (txtbox.length > 1) {

                        return true;
                    }
                    {

                        alert("Please do not enter numeric value & speacial character");
                        input.value = "";
                        return false;
                    }
                }

            }
        }
    </script>
    <section class="Container AttendanceContainer">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx" class="selected">Leave Transaction</a> <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%>
                <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a>
                <a href="HolidayList.aspx">Masters</a>
            </nav>
        </div>
        <div class="MainBody Admin">
            <div class="">
                <asp:Label ID="lblSuccess" runat="Server" SkinID="lblSuccess"></asp:Label>
                <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
            </div>
            <div class="FormContainerBox clearfix OrbitAuto LeaveTrans">
                <div class="leftcol">
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <asp:Label ID="lblEmpName" runat="server" Text=" Enter Employee Name :"></asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:TextBox ID="txtempname" runat="server" AutoPostBack="true" OnTextChanged="txtempname_TextChanged"
                                Width="182px"></asp:TextBox><asp:TextBox ID="txtempid" runat="server" Visible="false"
                                    Width="75px"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="aceEmpName1" runat="server" TargetControlID="txtempname"
                                ServicePath="~/LeaveTransactionAutoComplete.asmx" ServiceMethod="GetEmployeeName"
                                MinimumPrefixLength="1" CompletionListCssClass="list2" CompletionListItemCssClass="listitem2"
                                CompletionListHighlightedItemCssClass="hoverlistitem2">
                            </ajaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                </div>
                <p class="leave-note" id="trTotalLeave" runat="Server">
                    <asp:Label ID="lblTotalBalance" runat="server" Text=" Total Leave Balance :"></asp:Label>
                    <asp:Label ID="lblTotalDisplay" runat="Server"></asp:Label>
                </p>
            </div>
            <!--Gridview here-->

            <div class="InnerContainer">

                <asp:GridView ID="gvLeaveTransaction" runat="server" OnRowCancelingEdit="gvLeaveTransaction_RowCancelingEdit"
                    OnSorting="gvLeaveTransaction_Sorting" OnRowUpdating="gvLeaveTransaction_RowUpdating"
                    OnRowEditing="gvLeaveTransaction_RowEditing" OnRowDeleting="gvLeaveTransaction_RowDeleting"
                    OnPageIndexChanging="gvLeaveTransaction_PageIndexChanging" OnRowCommand="gvLeaveTransaction_RowCommand"
                    OnRowDataBound="gvLeaveTransaction_RowDataBound" ShowFooter="true" AllowSorting="true"
                    AllowPaging="true" PageSize="10" AutoGenerateColumns="false" OnSelectedIndexChanged="gvLeaveTransaction_SelectedIndexChanged"
                    Width="100%" CssClass="TableJqgrid">
                    <HeaderStyle CssClass="tableHeaders" />
                    <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                        LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                        PreviousPageText="Prev" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <RowStyle CssClass="tableRows" />
                    <FooterStyle HorizontalAlign="Left" CssClass="tableRows" />
                    <Columns>
                        <asp:TemplateField HeaderText="LeaveTransactionID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblleavetransactionID" runat="Server" Text='<%# Eval("LeaveTransactionID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="Date" HeaderStyle-Width="13%">
                            <EditItemTemplate>
                                <asp:Label ID="lblTransDate" runat="Server" Visible="False" Text='<%# Eval("Date") %>'
                                    Width="37px"></asp:Label>
                                <asp:TextBox ID="txtTransDate" runat="server" Text='<%# Eval("Date") %>' Width="100px" Height="28px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnDate" runat="server" CausesValidation="false" ImageAlign="Middle"
                                    ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgbtnDate"
                                    TargetControlID="txtTransDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None"
                                    ErrorMessage="Select Date" ValidationGroup="Update" ControlToValidate="txtTransDate"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTransDateAdd" runat="Server" Width="100px" Height="28px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnDate" runat="server" CausesValidation="false" ImageAlign="AbsMiddle"
                                    ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger"></asp:ImageButton>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" PopupButtonID="imgbtnDate"
                                    TargetControlID="txtTransDateAdd">
                                </ajaxToolkit:CalendarExtender>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTransDate" runat="Server" Text='<%# Eval("Date") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="SERVER" Text='<%# Bind("Description") %>'
                                    Width="195px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="None"
                                    ControlToValidate="txtDescription" ErrorMessage="Please enter description" ValidationGroup="Update"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDescriptionAdd" TextMode="MultiLine" runat="SERVER" Width="195px"></asp:TextBox>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbldescription" runat="Server" Text='<%# Eval("Description") %>'>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-Width="7%">
                            <EditItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' Width="1px"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtQuantityAdd" runat="Server" MaxLength="5" onkeyup="spcharacter(this)"
                                    onchange="spcharacter(this)" Width="60px"></asp:TextBox>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Quantity") %>'>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserID" SortExpression="UserID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lbluserid" runat="SERVER" Text='<%# Eval("UserID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason" SortExpression="Reason">
                            <ItemTemplate>
                                <asp:Label ID="lblRason" runat="SERVER" Text='<%# Eval("Reason") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="18%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Leave Type" SortExpression="LeaveType" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveType" runat="Server" Text='<%# Eval("LeaveType")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlLeaveTypeAdd" runat="server">
                                    <asp:ListItem Text="Leave" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Compensatory" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mode" SortExpression="TransactionMode" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <asp:Label ID="lblMode" runat="server" Text='<%#Eval ("TransactionMode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="center" />
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                    ValidationGroup="Update"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CausesValidation="false"
                                    CommandName="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" CommandName="Add" ValidationGroup="Add"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancelF" runat="Server" Text="Cancel" CausesValidation="false"
                                    CommandName="Cancel"></asp:LinkButton>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAuto" runat="Server"></asp:Label>
                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CausesValidation="false"
                                    CommandName="Edit"></asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="Server" Text="Delete" CausesValidation="false"
                                    CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="Update" ShowSummary="False"
                    ShowMessageBox="True" />
            </div>
        </div>
    </section>
</asp:Content>