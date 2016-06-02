<%@ Page Language="C#" AutoEventWireup="true" Inherits="Masters" MasterPageFile="../Views/Shared/HRMS.Master"
    CodeBehind="MastersTable.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            var i = 0;
            for (var i = 0; i <= 19; i++) {
                if ($('#MainContent_grdMaster_Status_' + i).length > 0) {
                    $('#MainContent_grdMaster_Status_' + i).after("<label id='MainContent_grdMaster_Status_" + i + "' class='LabelForCheckbox' for='MainContent_grdMaster_Status_" + i + "' ></label>");
                }
            }
            $('.aspNetDisabled').each(function () {
                if ($(this).is("select")) {
                    $(this).replaceWith(function () { return $(this) });
                }

            });

            $('.tableHeaders').each(function () {
                var temp = $(this)[0];
                var tempT = $(temp).context;
                $(tempT).children().replaceWith(function () {
                    return ("<th scope='col'>" + this.textContent + "</th>");
                });
            });
            $('*[id*=MainContent_grdMaster_ct]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                if ($('#' + vali)) {
                    $('#' + vali).after("<label id='MainContent_Main' for='" + vali + "' class='LabelForCheckbox'></label>");
                }
            });

            $('*[id*=MainContent_grdMaster_insert_button]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $('#' + vali).parents('tr').addClass("tableRows");
            });

            //MainContent_grdMaster_update_button_3
            $('*[id*=MainContent_grdMaster_update_button_]').each(function () {
                var obj = $(this);
                var vali = obj[0].id;
                $ElementD = $('#' + vali).parents('tr').find('input:text');
                $ElementD.focus();
            });

            //For SlaType Css
            if ($('#MainContent_ddlMasterTableName option:selected').text() == "tbl_SLA_Type") {
                $('*[id*=MainContent_grdMaster_insert_button]').each(function () {
                    var obj = $(this);
                    var $row = $(this).closest("tr"),
                    $tds = $row.find("td");
                    var Temp = 0;
                    $tds.each(function () {
                        if (Temp >= 2) {
                            $(this).find('*').width('30px');
                        }
                        Temp++;
                    });
                });
            }

        });
    </script>
    <section class="ConfirmationContainer Container">
        <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
        <div class="MainBody SmM SmartT">
            <div class="ErrorMaster">
                <asp:Label ID="lblErrorMessage" runat="server" SkinID="lblError"></asp:Label>
                <asp:Label ID="lblSuccessMessage" runat="server" SkinID="lblSuccess"></asp:Label>
            </div>
            <div class="FormContainerBox MasterI clearfix">
                <div class="formrow clearfix MasterTab">
                    <div class="leftcol clearfix">
                        <div class="LabelDiv">
                            <asp:Label Text="Select Table Name :" runat="server"></asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlMasterTableName" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTableName_SelectedIndexChanged"
                                CssClass="DropDownBox">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <div class="InnerContainer scrollHContainer">
                <asp:GridView ID="grdMaster" runat="server" AllowPaging="true" AllowSorting="true"
                    PageSize="20" OnRowCancelingEdit="grdMaster_RowCancelingEdit" OnRowCreated="grdMaster_RowCreated"
                    OnRowDeleting="grdMaster_RowDeleting" OnRowEditing="grdMaster_RowEditing" OnRowUpdating="grdMaster_RowUpdating"
                    AutoGenerateColumns="False" ShowFooter="true" OnRowCommand="grdMaster_RowCommand" CssClass="grid TableJqgrid"
                    OnPageIndexChanging="grdMaster_PageIndexChanging" GridLines="None"
                    OnSorting="grdMaster_Sorting" Width="100%" RowStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="tableHeaders" />
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                    <RowStyle CssClass="tableRows" />
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>