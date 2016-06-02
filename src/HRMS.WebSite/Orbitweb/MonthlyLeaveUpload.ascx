<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonthlyLeaveUpload.ascx.cs"
    Inherits="HRMS.Orbitweb.MonthlyLeaveUpload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <script language="javascript" type="text/javascript">

            $(document).ready(function () {
                $('#MainContent_MonthlyLeaveUpload1_grdMonthlyLeaveUpload_lnkAdd').click(function () {
                    var ua = window.navigator.userAgent;
                    var msie = ua.indexOf("MSIE");
                    //condition for IE 10 and IE 11
                    if ($.browser.msie || navigator.appName == 'Microsoft Internet Explorer' || !!navigator.userAgent.match(/Trident\/7\./) || msie > 0) {
                        if ($('#MainContent_MonthlyLeaveUpload1_grdMonthlyLeaveUpload_ddlMonth1').val() == 0) {
                            alert("Please select the Month");
                            return false;
                        }
                        if ($('#MainContent_MonthlyLeaveUpload1_grdMonthlyLeaveUpload_txtdays1').val().trim() == "") {
                            alert("Please Enter Leave Days");
                            return false;
                        }
                    }
                });
            });
            function Validate(txtdays1) {
                var txtDays = txtdays1;
                if (txtDays.value.trim() == "") {
                    alert("Please Enter Leave Days");
                    return false;
                }
                else if (!isNaN(txtDays.value)) {
                    if (txtDays.value % .5 != 0) {
                        alert("Please Enter Leave Days Multilpe of .5 ");
                        return false;
                    }
                }
                else {
                    alert("Please Dont enter Characters or Special Characters ");
                    return false;
                }
                //        else if(!spcharacter(txtDays))
                //        {
                //            alert("special characters!!!");
                //            return false;
                //        }
            }

            function ValidateAdd(txtdays, ddlYear, ddlMonth) {
                var txtDays = txtdays;

                if (ddlMonth.selectedIndex == 0) {
                    alert("Please select the Month");
                    return false;
                }
                if (ddlYear.selectedIndex == 0) {
                    alert("Please select the Year");
                    return false;
                }
                if (txtDays.value.trim() == "") {
                    alert("Please Enter Leave Days");
                    return false;
                }
                else if (!isNaN(txtDays.value)) {
                    if (txtDays.value % .5 != 0) {
                        alert("Please Enter Leave Days Multilpe of .5 ");
                        return false;
                    }
                }
                else {
                    alert("Please Dont enter Characters or Special Characters ");
                    return false;
                }
                //        else if(!spcharacter(txtDays))
                //        {
                //            alert("special characters!!!");
                //            return false;
                //        }
            }
            function spcharacter(input) {
                var txtbox = input.value;
                var iChars = "!@#$%^&*()+=-[]\\\';,/{}|\":<>?_ ";
                for (var i = 0; i < txtbox.length; i++) {
                    if (iChars.indexOf(txtbox.charAt(i)) != -1) {
                        alert("Please Dont enter Characters or Special Characters ");
                        input.value = "";
                        return false;
                    }
                    else {
                        alert("Please Dont enter Characters  ");
                        input.value = "";
                        return false;
                    }
                }
                return true;
            }
        </script>
        <div>
            <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
            <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
        </div>
        <div>
        </div>
        <p class="OrbitNote">
            * Previous date entry is not Added or Edited
        </p>
        <div class="clearfix">
            <div class="leftcol">
                <div class="formrow clearfix selectYr">
                    <div class="LabelDiv">
                        <asp:Label ID="lblSelectYear" Text="Select Year:" runat="server"></asp:Label></div>
                    <div class="InputDiv">
                        <asp:DropDownList ID="DDlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDlYear_SelectedIndexChanged"
                            CausesValidation="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <asp:GridView ID="grdMonthlyLeaveUpload" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                PageSize="12" ShowFooter="True" OnPageIndexChanging="grdMonthlyLeaveUpload_PageIndexChanging"
                OnRowCommand="grdMonthlyLeaveUpload_RowCommand" OnRowDataBound="grdMonthlyLeaveUpload_RowDataBound"
                OnRowUpdating="grdMonthlyLeaveUpload_RowUpdating" OnRowEditing="grdMonthlyLeaveUpload_RowEditing"
                OnRowCancelingEdit="grdMonthlyLeaveUpload_RowCancelingEdit" CssClass="TableJqgrid mrgnT20"
                Width="100%">
                <FooterStyle HorizontalAlign="left" CssClass="tableRows" />
                <HeaderStyle CssClass="tableHeaders" />
                <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                    LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                    PreviousPageText="Prev" />
                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                <RowStyle CssClass="tableRows" />
                <Columns>
                    <asp:TemplateField HeaderText="MonthlyLeaveUploadId" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblUploadYearID" runat="server" Text='<%# Eval("UploadYearID")%>'> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Month" ItemStyle-Width="20%">
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlMonth1" runat="server">
                                <asp:ListItem Value="0">Select Month</asp:ListItem>
                                <asp:ListItem Value="1">01-January</asp:ListItem>
                                <asp:ListItem Value="2">02-February</asp:ListItem>
                                <asp:ListItem Value="3">03-March</asp:ListItem>
                                <asp:ListItem Value="4">04-April</asp:ListItem>
                                <asp:ListItem Value="5">05-May</asp:ListItem>
                                <asp:ListItem Value="6">06-June</asp:ListItem>
                                <asp:ListItem Value="7">07-July</asp:ListItem>
                                <asp:ListItem Value="8">08-August</asp:ListItem>
                                <asp:ListItem Value="9">09-September</asp:ListItem>
                                <asp:ListItem Value="10">10-October</asp:ListItem>
                                <asp:ListItem Value="11">11-November</asp:ListItem>
                                <asp:ListItem Value="12">12-December</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLeaveMonth" runat="server" Text='<%# Eval("Month")%>'> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Year" ItemStyle-Width="30%">
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlYear1" runat="server">
                            </asp:DropDownList>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLeaveYear" runat="server" Text='<%# Eval("Year")%>'> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Days" ItemStyle-Width="30%">
                        <ItemTemplate>
                            <asp:Label ID="lblLeaveDays" runat="server" Text='<%# Eval("Days")%>'> </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtdays" runat="server" MaxLength="3" Width="20%" Text='<%# Eval("Days")%>'>  </asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtdays1" runat="server" MaxLength="3" Width="50%"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%">
                        <ItemStyle HorizontalAlign="Center" />
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                Text="Update"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="Edit"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton ID="lnkAdd" runat="server" Text="ADD" CommandName="lnkAdd"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel1" runat="server" Text="Cancel" CommandName="lnkCancel"></asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
