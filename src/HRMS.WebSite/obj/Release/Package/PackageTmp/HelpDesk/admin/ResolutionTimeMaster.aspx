<%@ Page Language="c#" CodeBehind="ResolutionTimeMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ResolutionTimeMaster" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Register TagPrefix="MT" TagName="MasterHeader" Src="~/HelpDesk/controls/HelpDeskMastersTabs.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <!-- Loading Calendar JavaScript files -->
    <script src="../utils/zapatec.js" type="text/javascript"></script>
    <script src="../src/calendar.js" type="text/javascript"></script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <script language="javascript">
        $(document).ready(function () {
            $("#ResolutionTimeMasterTab").removeClass('tabshover').addClass('colored-border');
        });
        function ParametersRequired() {
            if (isRequire("txtgreen^txtamber", "Green Resolution Hours^Amber Resolution Hours", this.enabled)) {
                return isInteger("txtgreen^txtamber", "Green Resolution Hours^Amber Resolution Hours")
            }
            else {
                return false;
            }
        }
        function validatetxtgreen() {
            if (isNaN(txtgreen1.value)) {
                alert("Total Leave Days should be numeric");
                return false;
            }
            else if (isNaN(txtamber1.value)) {
                alert("Total Leave Days should be numeric");
                return false;
            }
        }
		</script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <MT:MasterHeader ID="MasterHeader1" runat="server"></MT:MasterHeader>
                <section class="add-detailsdata clearfix ResolutionTimeMaster">
                    <asp:Label ID="lblRecordMsg" runat="server" CssClass="error" Visible="True"></asp:Label>
                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="true"></asp:Label>

                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lbtnAddResolutionTime" OnClick="lbtnAddResolutionTime_Click" CssClass="ButtonGray mrgnL20" runat="server">Add Resolution Time</asp:LinkButton>
                    </div>
                    <asp:ValidationSummary ID="valSummary" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                    <div class="InnerContainer scrollHContainer">
                        <asp:DataGrid ID="dgResolutionTime" CssClass="TableJqgrid" runat="server" OnCancelCommand="dgResolutionTime_cancel"
                            OnDeleteCommand="dgResolutionTime_Delete" HorizontalAlign="Center" OnEditCommand="dgResolutionTime_Edit" OnUpdateCommand="dgResolutionTime_Update" OnItemDataBound="dgResolutionTime_ItemDataBound" Width="100%" AutoGenerateColumns="False" OnPageIndexChanged="dgResolutionTime_PageIndexChanged"
                            PageSize="10" AllowPaging="True">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <Columns>
                                <asp:TemplateColumn Visible="False" HeaderText="Resolution ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblresolutionID" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"resolutionID")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn Visible="False" HeaderText="CategoryID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcategoryid" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"CategoryID")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcategory" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"Category")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlcategory1" CssClass="dropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcategory1_SelectedIndexChanged"
                                            OnPreRender="PreRendercategory">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn Visible="False" HeaderText="SubCategoryID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubcategoryid" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"SubCategoryID")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubcategory" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"SubCategory")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlsubCategorymaster" CssClass="dropdown" runat="server" OnPreRender="PreRenderSubCategory"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn Visible="False" HeaderText="ProblemseverityID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProblemseverityID" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"ProblemseverityID ")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Problem Severity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProblemseverity" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"severity")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlseveritymaster" CssClass="dropdown" runat="server" OnPreRender="PreRenderSeverity" SelectedIndex='<%#DataBinder.Eval (Container.DataItem,"ProblemseverityID ")%>'>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolution For Green(Hrs)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgreen" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"ResolutionForGreen")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtgreen1" runat="server" MaxLength="3" Text='<%#DataBinder.Eval (Container.DataItem,"ResolutionForGreen")%>'>
										</asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regExTxtGreen" runat="server" Display="None" ValidationExpression="^(\.[0-9]+|[0-9]+(\.[0-9]*)?)$"
                                            ErrorMessage="Please enter a valid number in Resolution For Green." ControlToValidate="txtgreen1"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolution For Amber(Hrs)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamber" runat="server" Text='<%#DataBinder.Eval (Container.DataItem,"ResolutionForAmber")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtamber1" runat="server" MaxLength="3" Text='<%#DataBinder.Eval (Container.DataItem,"ResolutionForAmber")%>'>
										</asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regExTxtAmber" runat="server" Display="None" ValidationExpression="^(\.[0-9]+|[0-9]+(\.[0-9]*)?)$"
                                            ErrorMessage="Please enter a valid number in Resolution For Amber." ControlToValidate="txtamber1"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" HeaderText="Edit" CancelText="Cancel"
                                    EditText="Edit"></asp:EditCommandColumn>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <asp:Panel ID="pnlAddResolutionMaster" Visible="False" runat="server" Width="100%" CssClass="mrgnT20">
                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Department:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlcategory" CssClass="dropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged" Width="150px"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <label>Category:</label>
                                </div>
                                <div class="InputDiv MonthYear">
                                    <asp:DropDownList ID="ddlsubcategory" CssClass="dropdown" runat="server"
                                        OnSelectedIndexChanged="ddlsubcategory_SelectedIndexChanged" Width="150px">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Problem Severity:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlseverity" CssClass="dropdown" runat="server" Width="150px"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <label>Resolution For Green:</label>
                                </div>
                                <div class="InputDiv MonthYear">
                                    <asp:TextBox ID="txtgreen" CssClass="txtfield" runat="server" MaxLength="3" Width="150px"></asp:TextBox>
                                    <label>(Hrs)</label>
                                </div>
                            </div>
                        </div>

                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Resolution For Amber:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtamber" CssClass="txtfield" runat="server" MaxLength="3" Width="150px"></asp:TextBox>
                                    <label>(Hrs)</label>
                                </div>
                            </div>
                        </div>

                        <asp:Label ID="lblMessage" runat="server" Visible="False" CssClass="error">Please Enter a Department</asp:Label>
                        <asp:Label ID="Label1" runat="server" Visible="False" CssClass="error">Please Enter a Category</asp:Label>
                        <asp:Label ID="Label2" runat="server" Visible="False" CssClass="error">Please Enter a Problem Severity</asp:Label>
                        <asp:Label ID="Label3" runat="server" Visible="False" CssClass="error">Please Enter a Resolution For Green</asp:Label>
                        <asp:Label ID="Label4" runat="server" Visible="False" CssClass="error">Please Enter a Resolution For Amber</asp:Label>

                        <div class="clearfix">
                            <asp:Button ID="btnsubmit" CssClass="ButtonGray mrgnT10" runat="server" Text="Submit" OnClick="btnsubmit_Click"></asp:Button>
                            <asp:Button ID="btncancel" CssClass="ButtonGray mrgnT10" runat="server" Text="Cancel" OnClick="btncancel_Click"></asp:Button>
                        </div>
                    </asp:Panel>
                </section>
            </div>
        </div>
    </section>
</asp:Content>