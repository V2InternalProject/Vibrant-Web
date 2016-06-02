<%@ Page Language="c#" CodeBehind="EditEmployeeDetail.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.EditEmployeeDetail" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CheckCategory() {
            //var controlName="RepSubCategory__ctl";

            for (var i = 0; i <= document.Form1.elements.length - 1; i++) {

                if (document.forms[0].elements[i].type == "checkbox" && document.forms[0].elements[i].checked == true) {
                    //	document.forms[0].elements[i+5].name;
                    if ((document.forms[0].elements[i + 2].checked == true) && (document.forms[0].elements[i + 5].length == 0)) {
                        alert("Please Assign Category for selected Department");
                        return false;
                    }
                    /*	if(document.forms[0].elements[i+5].length == 0)
                    {
                    alert("Please Assign Category for selected Department");
                    return false;
                    }*/
                    //	return false;
                }
            }
            //RepObjectiveCategoryDetails:_ctl1:txtObjectiveWeightage
            /*var valueInTextBox = document.getElementById("RepSubCategory__ctl"+i+"_chkSuperAdmin").checked;
            if(valueInTextBox == "")
            {
            alert('Is Blank');
            document.getElementById("rbtnlAssessmentMatrix_"+i).checked = true;
            break;
            }*/
            //}
            //	for()
            /*var lbSelectedSubCategory=document.getElementById("lblSelectedAdminSubCategories");
            if(lbSelectedSubCategory.==0)
            {
            alert("Please Select SubCategory to Assign");
            return;
            }*/

        }
        function Update() {
            alert("Employee's Record Updated Successfully");
            return true;
        }

        $(document).ready(function () {
            $('.EditUserMaster').find('select').selectBox("detach");
        });
        //$('.EditUserMaster').find('select').selectbox("detach");
    </script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody EditUserMaster">
            <asp:Label ID="lblMessage" runat="server" Visible="False" CssClass="error"></asp:Label>
            <div class="FormContainerBox">
                <div class="formrow clearfix">
                    <div class="leftcol">
                        <div class="LabelDiv">
                            <asp:Label ID="lblEmployeeName" runat="server" CssClass="txt">Employee Name</asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="txtfield" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="InnerContainer clearfix mrgnT15">
                <asp:Repeater ID="RepSubCategory" runat="server" OnItemCommand="RepSubCategory_ItemCommand">
                    <ItemTemplate>
                        <div class="UserMasterEditBox">
                            <div class="clearfix">
                                <h3 class="SmallHead">Department:</h3>
                                <asp:CheckBox ID="chkSuperAdmin" Checked="False" runat="server" AutoPostBack="True" />
                                <%--<asp:label ID="Label1"  runat="server" AssociatedControlID="chkSuperAdmin"  class="LabelForCheckbox" ></asp:label>--%>
                                <asp:Label ID="lblforchkSuperAdmin" runat="server" class="LabelForCheckbox" AssociatedControlID="chkSuperAdmin" />
                            </div>
                            <div class="FormContainerBox clearfix">
                                <div class="formrow clearfix">
                                    <div class="LabelDiv">
                                        <label>Is Administrator:</label></div>
                                    <div class="InputDiv">
                                        <asp:RadioButtonList ID="IsAdmin" CssClass="RadioButtonList" RepeatLayout="Table" AutoPostBack="true" runat="server"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="true">Yes</asp:ListItem>
                                            <asp:ListItem Value="false" Selected="False">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="ListBoxContainer">
                                    <asp:ListBox ID="lbAvailableAdminSubCategories" runat="server" SelectionMode="Multiple" Width="220"></asp:ListBox>
                                    <div class="clearfix">
                                        <asp:Button ID="btCopyAll" CssClass="ButtonGray mrgnT10" runat="server" Text="Copy All" CommandName="CopyAll"></asp:Button>
                                    </div>
                                </div>

                                <div class="ShiftBtnContainer">
                                    <asp:Button ID="btnRightShift" CssClass="next mrgnT10" runat="server" Text="" CommandName="rightshift"></asp:Button><br />
                                    <asp:Button ID="btnLeftShift" CssClass="prev mrgnT5" runat="server" CommandName="leftshift" Text=""></asp:Button>
                                </div>

                                <div class="ListBoxContainer">
                                    <asp:ListBox ID="lblSelectedAdminSubCategories" runat="server" SelectionMode="Multiple" Width="220"></asp:ListBox>
                                    <div class="clearfix">
                                        <asp:Button ID="btnRemoveAll" CssClass="ButtonGray mrgnT10" runat="server" Text="Remove All" CommandName="RemoveAll"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="clearfix InnerContainer mrgnT15">
                <asp:Button ID="btnUpdate" runat="server" CssClass="mrgnR11 ButtonGray" CommandName="UpdateSubCategory" Text="Submit" OnClientClick="return Update();" OnClick="btnUpdate_Click"></asp:Button>
                <asp:Button ID="btnCancle" runat="server" CssClass="mrgnR11 ButtonGray" Text="Cancel" OnClick="btnCancle_Click"></asp:Button>
            </div>
        </div>
    </section>
</asp:Content>