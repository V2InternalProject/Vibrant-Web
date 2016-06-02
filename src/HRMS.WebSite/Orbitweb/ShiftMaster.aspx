<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="ShiftMaster.aspx.cs" Inherits="HRMS.Orbitweb.ShiftMaster" %>

<%@ Register Src="~/Orbitweb/OrbitMastersTabs.ascx" TagPrefix="RT" TagName="OrbitMastersTabs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <script language="javascript" type="text/javascript">

                function pageLoad() {
                    //To apply plugin UI to dropdowns
                    $('select').selectBox();
                    var name = "<%=ViewState["AdminMaster"]%>";
                    if (name == 'ShiftMaster')
                        $('#ShiftDetailsTab').addClass('colored-border');
                }

                function validation(txtShiftName) {
                    if (txtShiftName.value == "") {
                        alert("Please enter the Shift Name");
                        txtShiftName.focus();
                        return false;
                    }
                }
            </script>
            <section class="Container AttendanceContainer">
                <div class="FixedHeader">
                    <div class="clearfix">
                        <h2 class="MainHeading">Administration</h2>
                    </div>
                    <nav class="sub-menu-colored">
                        <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                            Transaction</a> <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a><%--<a href="StartWorkflows.aspx">Manage Processes</a>--%>
                        <a href="HolidayList.aspx" class="selected">Masters</a>
                    </nav>
                </div>
                <div class="MainBody Admin">
                    <RT:OrbitMastersTabs ID="OrbitMastersTabs" runat="server"></RT:OrbitMastersTabs>
                    <div class="clearfix">
                        <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                        <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
                    </div>
                    <p class="OrbitNote">
                        * Previous date entry is not Added or Edited
                    </p>
                    <div class="clearfix mrgnT25">
                        <asp:DataGrid ID="grdShiftMaster" runat="server" ShowFooter="True" PageSize="1000"
                            AutoGenerateColumns="False" AllowPaging="True" Width="96%" OnItemCommand="grdShiftMaster_ItemCommand"
                            OnItemDataBound="grdShiftMaster_ItemDataBound" OnEditCommand="grdShiftMaster_EditCommand"
                            CssClass="TableJqgrid">
                            <EditItemStyle VerticalAlign="Middle" HorizontalAlign="Left"></EditItemStyle>
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" CssClass="tableRows"></ItemStyle>
                            <HeaderStyle VerticalAlign="Middle" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <FooterStyle HorizontalAlign="left" CssClass="tableRows" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="ShiftID" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblShiftID" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:Label ID="lblShiftID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftID") %>'>
                                        </asp:Label>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:Label runat="server" ID="lblShiftID2" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftID") %>'>
                                        </asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Shift Name" ItemStyle-Width="15%">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblShiftName" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtShiftName" runat="server" MaxLength="20" Width="50"></asp:TextBox>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtShiftName1" runat="server" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftName") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Shift Description" ItemStyle-Width="15%" Visible="false">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblShiftDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtShiftDescription" runat="server" MaxLength="50"></asp:TextBox>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtShiftDescription1" runat="server" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Shift In Time" ItemStyle-Width="30%">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlReportingHourIN" runat="server" AutoPostBack="false" Enabled="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteIN" runat="server" AutoPostBack="false" Enabled="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlReportingHourIN1" runat="server" AutoPostBack="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteIN1" runat="server" AutoPostBack="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlReportingHourIN2" runat="server" AutoPostBack="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteIN2" runat="server" AutoPostBack="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </EditItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Shift Out Time" ItemStyle-Width="30%">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlReportingHourOUT" runat="server" Enabled="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteOUT" runat="server" Enabled="false">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlReportingHourOUT1" runat="server">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteOUT1" runat="server">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlReportingHourOUT2" runat="server">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9" Selected="True">09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                        </asp:DropDownList>
                                        Hrs.

                                        <asp:DropDownList ID="ddlReportingMinuteOUT2" runat="server">
                                            <asp:ListItem Value="0">00</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem Selected="True">30</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                        Min.
                                    </EditItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Is Active" ItemStyle-Width="10%">
                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsactive" runat="server" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "ISActive") )%>' />
                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="chkIsactive" class="LabelForCheckbox"></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkIsactive1" runat="server" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "ISActive") )%>' />
                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="chkIsactive1" class="LabelForCheckbox"></asp:Label>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkIsactive2" runat="server" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "ISActive") )%>' />
                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="chkIsactive2" class="LabelForCheckbox"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Action">
                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbutEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="btnAdd" runat="server" Text="ADD" CommandName="btnAdd"></asp:LinkButton>
                                        <asp:LinkButton ID="btn" runat="server" Text="Cancel" CommandName="btnCancel" CausesValidation="false"></asp:LinkButton>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkbutUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;

                                        <asp:LinkButton ID="lnkbutCancel" runat="server" Text="Cancel" CommandName="btnCancel"
                                            CausesValidation="false"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>