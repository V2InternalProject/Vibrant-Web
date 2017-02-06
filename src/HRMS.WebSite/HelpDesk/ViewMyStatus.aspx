<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="True"
    CodeBehind="ViewMyStatus.aspx.cs" Inherits="HRMS.HelpDesk.ViewMyStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <link href="../Content/New%20Design/helpdesk.css" rel="stylesheet" />
    <script src="../Scripts/Global.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#MainContent_txtComments').is(':visible')) {
                $('.hideOnAdd').hide();
            }
            $('#MainContent_btnAddComments').click(function () {
                $('.hideOnAdd').hide();
            });
            $('#MainContent_btnSaveComments').click(function () {
                DisplayLoadingDialog();
            });
            $(".FileUploadBtn").bind("change", function (event) {
                var path = $(this).val().replace("C:\\fakepath\\", "");
                $("#FileUploadFilesField").val(path);
            });

            $('#MainContent_txtUpdateCurrentAllocation').hide();
            $('#MainContent_txtSingleOrBulkExtension').hide();
            $('#MainContent_txtNewResource').hide();
            // var e = document.getElementById("ddlCategories");
            // var strUser = e.options[e.selectedIndex].value;
            var strUser = $('#MainContent_lblSubCategory_Category').text();
            if (strUser == $('#MainContent_txtNewResource').val() || strUser == $('#MainContent_txtUpdateCurrentAllocation').val()) {
                $('.OtherCategoryShow').hide();
                $('.PMSCategoriesHide').hide();
                $('.PMSCategories').show();
                $('.PMSShow').show();
            }
            else if (strUser == $('#MainContent_txtSingleOrBulkExtension').val()) {
                $('.OtherCategoryShow').hide();
                $('.PMSCategoriesHide').hide();
                $('.PMSCategories').hide();
                $('.BulkCategories').show();
                $('.PMSShow').show();
            }
            else {
                $('.OtherCategoryShow').show();
                $('.PMSCategories').hide();
                $('.PMSCategoriesHide').show();
            }
        });
        function validate() {
            var txtLoginID = document.getElementById("txtLoginID").value;
            var txtPassword = document.getElementById("txtPassword").value;
            txtLoginID = Trim(txtLoginID);
            txtPassword = Trim(txtPassword);

            if (txtLoginID == "" && txtPassword == "") {
                alert("Please enter Login ID and Password");
            }
            else if (txtLoginID == "" && txtPassword != "") {

                alert("Please enter Login ID");
                return false;

            }
            else if (txtLoginID != "" && txtPassword == "") {

                alert("Please enter Password");
                return false;
            }
            else if (txtLoginID != "" && txtPassword != "") {
                //alert("No Records Found");
                //return false;
            }

        }
    </script>
    <section class="HelpdeskContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">HelpDesk</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="ReportIssue.aspx">Report Issue</a> <a href="ViewMyStatus.aspx" class="selected">Issue Status</a>
            </nav>
        </div>
        <div class="MainBody IssueStatusMainbody">
            <div>
                <asp:Label ID="lblMessage1" runat="server" CssClass="error"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
            </div>
            <asp:Panel ID="pnlDataGrid" runat="server" Width="100%">
                <asp:DataGrid ID="dgMyIssueList" CssClass="TableJqgrid IssueStatusTable" Width="96%"
                    runat="server" AllowPaging="True" PageSize="5" OnPageIndexChanged="dgMyIssueList_PageChange"
                    DataKeyField="ReportIssueID" AutoGenerateColumns="False" OnItemCommand="dgMyIssueList_ItemCommand"
                    OnItemDataBound="dgMyIssueList_ItemBound" CellPadding="10" CellSpacing="10">
                    <HeaderStyle CssClass="tableHeaders"></HeaderStyle>
                    <ItemStyle CssClass="bluebordertable1 tableRows"></ItemStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:TemplateColumn HeaderStyle-Width="10%" HeaderText="Report Issue ID">
                            <ItemTemplate>
                                <asp:LinkButton ID="lBtnReportIssueID" runat="server" CommandName="viewDetails" Text='<%#DataBinder.Eval(Container.DataItem, "ReportIssueID")%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <%--<ItemTemplate>
                                <asp:label ID="lBtnReportIssueID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ReportIssueID")%>'>
                                </asp:label>
                            </ItemTemplate>--%>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="15%" HeaderText="Report Issue Date">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "ReportIssueDate")%>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="15%" HeaderText="Assigned To">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "EmployeeName")%>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrentStatusID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="30%" HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblIssueDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="30%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-Width="10%" HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrentStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusDesc") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateColumn>
                        <%-- <asp:TemplateColumn HeaderStyle-Width="5%" HeaderText="SLA (HH)">
                            <ItemTemplate>
                                <asp:Label ID="SLA" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ResolutionForAmber") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateColumn>--%>
                        <asp:TemplateColumn HeaderStyle-Width="15%" HeaderText="Remaining Time To Go To Amber Or Red (HH:MM)">
                            <ItemTemplate>
                                <asp:Label ID="RemainingTimeToGoTOAmberOrRed" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemainingTimeToGoTOAmberOrRed") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <asp:Panel ID="pnlIssueDetails" runat="server" Width="100%">
                <div class="FormContainerBox  IssueStatus clearfix">
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Issue ID:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblIssueID" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Issue Reported by:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportedBy" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Issue Reported on:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportedOn" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Department:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblDepartment" runat="server" CssClass="trcolor"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Category:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblSubCategory_Category" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Type:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblTypeID" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Problem Severity:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblSeverity" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Problem Description:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblDescription" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Report Status:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportStatus" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories BulkCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Project Name:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProjectName" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Project Role:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProjectRole" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Work Hours:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblWorkHours" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    From Date:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblFromDate" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories BulkCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    To Date:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblToDate" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories BulkCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Number Of Resources:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblNoOfResources" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Resource Pool:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblResourcePool" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix PMSCategories">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Will be Reporting To:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportingTo" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix" style="display: none">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Comment:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblComment" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Comment And Description:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblCommentDesc" CssClass="trcolor" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="rightcol">
                        <div class="LabelDiv">
                            <label>
                                File Name:
                            </label>
                        </div>
                        <div class="InputDiv">
                            <asp:Panel ID="pnlFileName" runat="server">
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Upload File:
                                </label>
                            </div>
                            <div class="InputDiv positionR BrowseSpacingFix">
                                <input type="file" id="uploadFiles" class="FileUploadBtn" name="uploadFiles"
                                    runat="server" style="width: 100px" />
                                <div class="BrowserVisible">
                                    <input type="button" class="BtnForCustomUpload" value="Browse.." /><input type="text" id="FileUploadFilesField" class="FileField" value="No files selected" />
                                </div>
                                <%--  <input class="uploadBtn" id="" type="file" size="20" name="uploadFiles"
                                    runat="server" />--%>
                            </div>
                        </div>
                    </div>
                    <asp:TextBox ID="txtUpdateCurrentAllocation" hidden="true" runat="server" Text="<%$appSettings:UpdateCurrentAllocationText %>" />
                    <asp:TextBox ID="txtNewResource" hidden="true" runat="server" Text="<%$appSettings:NewResourceText %>" />
                    <asp:TextBox ID="txtSingleOrBulkExtension" hidden="true" runat="server" Text="<%$appSettings:SingleOrBulkExtensionText %>" />
                </div>
                <asp:DataGrid ID="dgIssueDetails" CssClass="TableJqgrid IssueFixTable" runat="server"
                    Width="96%" AllowPaging="True" PageSize="5" OnPageIndexChanged="dgIssueDetails_pageChange"
                    AutoGenerateColumns="False" OnItemDataBound="dgIssuDetails_ItemBound">
                    <HeaderStyle CssClass="tableHeaders"></HeaderStyle>
                    <ItemStyle CssClass="bluebordertable1 alignC tableRows"></ItemStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="User Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "EmployeeName")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Cause">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Cause")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Fix">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Fix")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Date">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Date")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <div class="ButtonContainer1 clearfix">
                    <asp:Button ID="btnAddComments" OnClick="btnAddComments_Click" runat="server" CssClass="ButtonGray mrgnR11 hideOnAdd"
                        Text="Add Comments" ></asp:Button>
                    <asp:Button ID="btnUpload" runat="server" CssClass="ButtonGray" Text="Upload File"
                        OnClick="btnUpload_Click"  />
                </div>
            </asp:Panel>
            <div>
                <asp:Label ID="lblMessage" CssClass="success" runat="server" Visible="True"></asp:Label>
            </div>
            <asp:Panel ID="pnlAddComments" runat="server" Width="80%">
                <div class="FormContainerBox">
                    <p class="IssueStatusNote">
                        Please write your comments in the box below. This will be appended to your problem
                        and may help you get solved soon.
                    </p>
                    <textarea class="newText" id="txtComments" onkeydown="textCounter(txtComments,txtDescCount,txtMaxLimit.value)"
                        onkeyup="textCounter(txtComments,txtDescCount,txtMaxLimit.value)" style="width: 720px; height: 82px"
                        runat="server"></textarea>
                    <input class="txtfieldlimit" id="txtDescCount" readonly type="text" maxlength="3"
                        size="3" value="1000" name="txtDescCount" runat="server" />
                </div>
                <div class="ButtonContainer1 clearfix">
                    <asp:Button ID="btnSaveComments" runat="server" CssClass="ButtonGray mrgnR11" Text="Save Comments"
                        OnClick="btnSaveComments_Click"></asp:Button>
                    <asp:Button ID="btnCloseIssue" runat="server" CssClass="ButtonGray mrgnR11" Text="Close Issue"
                        OnClick="btnCloseIssue_Click" ></asp:Button>
                    <asp:Button ID="btnReOpenIssue" OnClick="btnReOpenIssue_Click" runat="server" CssClass="ButtonGray"
                        Text="Reopen Issue"></asp:Button>
                </div>
                <div>
                    <input id="txtMaxLimit" type="hidden" runat="server">
                </div>
            </asp:Panel>
        </div>
    </section>
    <%--   <footer>&#169; 2008 V2Solutions, Inc.</footer>
        </div>--%>
    <script type="text/javascript" language="javascript" src="footer.js"></script>
    <script type="text/javascript" language="javascript" src="js/common.js"></script>
    <%-- </body>
    </html>--%>
</asp:Content>
