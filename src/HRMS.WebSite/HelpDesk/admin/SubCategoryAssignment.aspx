<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="SubCategoryAssignment.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.SubCategoryAssignment" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Category Assignment</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../css/allstyles.css" type="text/css" rel="stylesheet">
    <!--<script language="JavaScript">

function removeall(dest)
{
dest.options.length=0;
}

function selectall(source,dest)
{
dest.options.length=1;

	var l_iInd = 0;
	var lastIndex = 0;
	var l_iMax = source.length;
	for (l_iInd = 0; l_iInd < l_iMax; l_iInd++)
	{
		dest.options.length = lastIndex+1;
		dest.options[lastIndex].value= source.options[l_iInd].value;
		dest.options[lastIndex].text= source.options[l_iInd].text;
		lastIndex++;
	}
}

function additem(src,dest)
  {
    var textvalue;
    var optvalue;
	if (src.selectedIndex==-1 || src.selectedIndex<0)
       {
      alert("Please make a selection!");
      return false;
       }

    textvalue=src.options[src.selectedIndex].value;
    optvalue= src.options[src.selectedIndex].text;

	if (checkduplicate(dest,optvalue) == true)
		{
			alert("Selected Item is already available in Selected Item List");
			return false;
		}

     var l_iAt = dest.options.length++;
    // alert(l_iAt);
     dest.options[l_iAt].value = textvalue;
	dest.options[l_iAt].text = optvalue;
	sortlist(dest);
}

function removeselected(dest)
{
	var l_iInd = 0;
	var l_iMax = dest.length;

	textvalue = dest.options[l_iMax-1].value;
	optvalue = dest.options[l_iMax-1].text;

	for (l_iInd = 0; l_iInd < l_iMax; l_iInd++)
	{
		if (dest.options[l_iInd].selected == true)
		{
			dest.options[l_iInd].value = textvalue;
			dest.options[l_iInd].text = optvalue;
			dest.length--;
			l_iInd = l_iMax;
			sortlist(dest);
		}
	}
}

function addselection(dest)
{
	var l_iInd = 0;
	//var l_iMax = dest.options.length;
	var l_iMax = dest.length;
	var total = 0;
	for (l_iInd = 1; l_iInd < l_iMax; l_iInd++)
	{
		dest.options[l_iInd].selected = true;
		total=total + 1;
	}
return total;
}

function removeitem(dest)
{
	if (dest.selectedIndex==-1)
	  {
	    alert("Please select an item to remove!")
	    return true;
	  }

	p_strSymbol=dest.options[dest.selectedIndex].text;

	if (dest.selectedIndex==-1 || dest.selectedIndex<0)
       {
      alert("Please make a selection to remove!");
      return false;
       }
	removeselected(dest);
//	sortlist(dest);
	return true;
}

function checkduplicate(objectname,findstring)
{
	var l_iInd = 0;
	var l_iMax = objectname.length;
	var found = false
	for (l_iInd = 0; l_iInd < l_iMax; l_iInd++)
	{
		if(objectname.options[l_iInd].text == findstring)
		{
			found = true;
		}
	}
return found;
}

function sortlist(dest)
    {
	var l_iMax = dest.length;

	if (l_iMax < 3)
	{
		return true;
	}

	var l_iInd1 = 0, l_iInd2 = 0;
	for ( l_iInd1 = 1; l_iInd1 < l_iMax-1; l_iInd1++)
	{
		for ( l_iInd2 = l_iInd1+1; l_iInd2 < l_iMax; l_iInd2++)
		{
			if (  dest.options[l_iInd1].text >  dest.options[l_iInd2].text )
			{
             		swap(l_iInd1, l_iInd2,dest);
			}
		}

	}

	return true;
}

function swap(p_iInd1, p_iInd2,dest)
{
	var l_strTempVal = dest.options[p_iInd1].value;
	var l_strTempText = dest.options[p_iInd1].text;

	dest.options[p_iInd1].value = dest.options[p_iInd2].value;
	dest.options[p_iInd1].text = dest.options[p_iInd2].text;

	dest.options[p_iInd2].value = l_strTempVal;
	dest.options[p_iInd2].text = l_strTempText;
}
		</script>-->
    <script language="JavaScript">

		function ShowPanel()
		{
			//alert("start");
			if((document.getElementById("dgCategories__ctl2_chkBoxCategory")).checked)
				{
					document.getElementById("pnlAdmin").style.display = "";
				}
				else
				{
					//Form1.elements["lbSelectedAdminSubCategories"].reset;
					document.getElementById("lbSelectedAdminSubCategories").options.length=0;
					document.getElementById("pnlAdmin").style.display = "none";
					(document.getElementById("dgCategories__ctl2_chkBoxSysAdmin")).checked = false;
				}
			if((document.getElementById("dgCategories__ctl4_chkBoxCategory")).checked)
				{
					document.getElementById("pnlIT").style.display = "";
				}
				else
				{
					document.getElementById("lbSelectedITSubCategories").options.length=0;
					document.getElementById("pnlIT").style.display = "none";
					(document.getElementById("dgCategories__ctl4_chkBoxSysAdmin")).checked = false;
				}
			if((document.getElementById("dgCategories__ctl3_chkBoxCategory")).checked)
				{
					document.getElementById("pnlHR").style.display = "";
				}
				else
				{
					document.getElementById("lbSelectedHRSubCategories").options.length=0;
					document.getElementById("pnlHR").style.display = "none";
					(document.getElementById("dgCategories__ctl3_chkBoxSysAdmin")).checked = false;
				}
		}

		function onBodyLoad()
		{	if(document.getElementById("dgCategories__ctl2_chkBoxCategory") != null)
			{
				if(!(document.getElementById("dgCategories__ctl2_chkBoxCategory")).checked)
				{
				document.getElementById("pnlAdmin").style.display = "none";
				}
			}
			else
			{
				document.getElementById("pnlAdmin").style.display = "none";
			}
			if(document.getElementById("dgCategories__ctl4_chkBoxCategory") != null)
			{
				if(!(document.getElementById("dgCategories__ctl4_chkBoxCategory")).checked)
				{
				document.getElementById("pnlIT").style.display = "none";
				}
			}
			else
			{
				document.getElementById("pnlIT").style.display = "none";
			}
			if(document.getElementById("dgCategories__ctl3_chkBoxCategory") != null)
			{
				if(!(document.getElementById("dgCategories__ctl3_chkBoxCategory")).checked)
				{
				document.getElementById("pnlHR").style.display = "none";
				}
			}
			else
			{
				document.getElementById("pnlHR").style.display = "none";
			}
		}

		function IsCategoryChecked()
		{
			if(!(document.getElementById("dgCategories__ctl2_chkBoxCategory")).checked)
			{
				if((document.getElementById("dgCategories__ctl2_chkBoxSysAdmin")).checked)
				{
					alert("Please Select a Department First!!!");
					document.getElementById("dgCategories__ctl2_chkBoxSysAdmin").checked = false;
				}
			}
			if(!(document.getElementById("dgCategories__ctl3_chkBoxCategory")).checked)
			{
				if((document.getElementById("dgCategories__ctl3_chkBoxSysAdmin")).checked)
				{
					alert("Please Select a Department First!!!");
					document.getElementById("dgCategories__ctl3_chkBoxSysAdmin").checked = false;
				}
			}
			if(!(document.getElementById("dgCategories__ctl4_chkBoxCategory")).checked)
			{
				if((document.getElementById("dgCategories__ctl4_chkBoxSysAdmin")).checked)
				{
					alert("Please Select a Department First!!!");
					document.getElementById("dgCategories__ctl4_chkBoxSysAdmin").checked = false;
				}
			}
		}

		function isSubCategorySelected()
		{
			if((document.getElementById("dgCategories__ctl2_chkBoxCategory")).checked && document.getElementById("lbSelectedAdminSubCategories").options.length == 0)
			{
				alert("Please Select SubCategories for Admin; or uncheck the Admin Department in the table");
				return false;
			}
			if((document.getElementById("dgCategories__ctl3_chkBoxCategory")).checked && document.getElementById("lbSelectedHRSubCategories").options.length == 0)
			{
				alert("Please Select SubCategories for HR; or uncheck the HR Department in the table");
				return false;
			}
			if((document.getElementById("dgCategories__ctl4_chkBoxCategory")).checked && document.getElementById("lbSelectedITSubCategories").options.length == 0)
			{
				alert("Please Select SubCategories for IT; or uncheck the IT Department in the table");
				return false;
			}

			if(!(document.getElementById("dgCategories__ctl2_chkBoxCategory")).checked && !(document.getElementById("dgCategories__ctl3_chkBoxCategory")).checked && !(document.getElementById("dgCategories__ctl4_chkBoxCategory")).checked)
			{
				alert("Please Select atleast one Department.");
				return false;
			}
		}
		</script>
</head>
<body leftmargin="0" topmargin="0" onload="return onBodyLoad();" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td>
                    <uc1:header ID="Header1" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td class="header" height="10"></td>
            </tr>
            <tr class="trcolor">
                <td class="header">&nbsp;SubCategory Assignment
					</td>
            </tr>
            <tr>
                <td align="center" height="10"></td>
            </tr>
            <tr>
                <td style="height: 12px" align="center">
                    <asp:DropDownList ID="ddlEmployeeName" runat="server" CssClass="dropdown"></asp:DropDownList><asp:Label ID="lblEmployee" runat="server" CssClass="error"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" height="10"></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="dgCategories" runat="server" CssClass="trcolor" CellPadding="5" AutoGenerateColumns="False"
                        DataKeyField="CategoryID" OnItemDataBound="dgCategories_ItemDataBound" OnSelectedIndexChanged="dgCategories_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Department">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBoxCategory" onclick="ShowPanel();" runat="server" OnCheckedChanged="dgCategories_IsCateogrySelected" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>' AutoPostBack="True" CssClass="trcolor"></asp:CheckBox><!--<input id="chkBoxCategory" value = '<%# DataBinder.Eval(Container.DataItem,"Category") %>' type=checkbox onclick=ShowPanel();>-->
                                    <!--<asp:Label ID="lblCategory" Runat=server Visible=True>
										<%# DataBinder.Eval(Container.DataItem, "Category") %>
										</asp:Label>-->
                                    <asp:Label ID="lblCategoryID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryID") %>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Is System Admin">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBoxSysAdmin" onclick="IsCategoryChecked();" runat="server" OnCheckedChanged="dgCategories_isSysAdmin"
                                        Text="Yes" AutoPostBack="True" CssClass="trcolor"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="isActive" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsActive" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "isActive")%>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlAdmin" runat="server">
                        <table cellspacing="0" cellpadding="0" align="center" border="0">
                            <tr>
                                <td class="trcolor" align="center">Categories of Admin</td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="5" align="center" border="0">
                                        <tr>
                                            <td style="height: 160px" valign="top">
                                                <asp:ListBox ID="lbAvailableAdminSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                    Width="150" Height="160"></asp:ListBox></td>
                                            <td style="height: 160px" valign="middle">
                                                <asp:Button ID="btnAdminCopy" runat="server" Text=">>" OnClick="btnAdminCopy_Click"></asp:Button><!--<input id="btnAdminCopy" onclick="javascript:additem(this.form.lbAvailableAdminSubCategories, this.form.lbSelectedAdminSubCategories)"
													type="button" value=">>">--><br>
                                                <br>
                                                <asp:Button ID="btnAdminRemove" runat="server" Text="<<" OnClick="btnAdminRemove_Click"></asp:Button><!--<input id="btnAdminRemove" onclick="javascript:javascript:removeitem(this.form.lbSelectedAdminSubCategories);"
													type="button" value="<<">--></td>
                                            <td style="height: 160px" valign="top">
                                                <asp:ListBox ID="lbSelectedAdminSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                    Width="150px" Height="160px"></asp:ListBox></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnAdminCopyAll" CssClass="btn" runat="server" Text="Copy All" OnClick="btnAdminCopyAll_Click"></asp:Button>
                                                <!--<input id="btnAdminCopyAll" onclick="javascript:selectall(this.form.lbAvailableAdminSubCategories, this.form.lbSelectedAdminSubCategories)"
													type="button" value="Copy All"></td>-->
                                                <td></td>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnAdminRemoveAll" CssClass="btn" runat="server" Text="Remove All" OnClick="btnAdminRemoveAll_Click"></asp:Button><!--<input id="btnAdminRemoveAll" onclick="javascript:removeall(this.form.lbSelectedAdminSubCategories)"
													type="button" value="Remove All">--></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlHR" runat="server">
                        <table cellspacing="0" cellpadding="0" align="center" border="0">
                            <tr>
                                <td class="trcolor" align="center">Categories of HR</td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="5" align="center" border="0">
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lbAvailableHRSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                    Width="150" Height="160"></asp:ListBox></td>
                                            <td valign="middle">
                                                <asp:Button ID="btnHRCopy" runat="server" Text=">>" OnClick="btnHRCopy_Click"></asp:Button><!--<input id="btnHRCopy" onclick="javascript:additem(this.form.lbAvailableHRSubCategories, this.form.lbSelectedHRSubCategories)"
													type="button" value=">>">--><br>
                                                <br>
                                                <asp:Button ID="btnHRRemove" runat="server" Text="<<" OnClick="btnHRRemove_Click"></asp:Button>
                                                <!--<input id="btnHRRemove" onclick="javascript:removeitem(this.form.lbSelectedHRSubCategories)"
													type="button" value="<<">-->
                                                <td valign="top">
                                                    <asp:ListBox ID="lbSelectedHRSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                        Width="150" Height="160"></asp:ListBox></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnHRCopyAll" CssClass="btn" runat="server" Text="Copy All" OnClick="btnHRCopyAll_Click"></asp:Button><!--<input id="btnHRCopyAll" onclick="javascript:selectall(this.form.lbAvailableHRSubCategories, this.form.lbSelectedHRSubCategories)"
													type="button" value="Copy All">--></td>
                                            <td></td>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnHRRemoveAll" CssClass="btn" runat="server" Text="Remove All" OnClick="btnHRRemoveAll_Click"></asp:Button><!--<input id="btnHRRemoveAll" onclick="javascript:removeall(this.form.lbSelectedHRSubCategories)"
													type="button" value="Remove All">--></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlIT" runat="server">
                        <table cellspacing="0" cellpadding="0" align="center" border="0">
                            <tr>
                                <td class="trcolor" align="center">Categories of IT</td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="5" align="center" border="0">
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lbAvailableITSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                    Width="150" Height="160"></asp:ListBox></td>
                                            <td valign="middle">
                                                <asp:Button ID="btnITCopy" runat="server" Text=">>" OnClick="btnITCopy_Click"></asp:Button><!--<input id="btnITCopy" onclick="javascript:additem(this.form.lbAvailableITSubCategories, this.form.lbSelectedITSubCategories)"
													type="button" value=">>">--><br>
                                                <br>
                                                <asp:Button ID="btnITRemove" runat="server" Text="<<" OnClick="btnITRemove_Click"></asp:Button><!--<input id="btnITRemove" onclick="javascript:javascript:removeitem(this.form.lbSelectedITSubCategories);"
													type="button" value="<<">--></td>
                                            <td valign="top">
                                                <asp:ListBox ID="lbSelectedITSubCategories" CssClass="dropdown" runat="server" SelectionMode="Multiple"
                                                    Width="150" Height="160"></asp:ListBox></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnITCopyAll" CssClass="btn" runat="server" Text="Copy All" OnClick="btnITCopyAll_Click"></asp:Button><!--<input id="btnITCopyAll" onclick="javascript:selectall(this.form.lbAvailableITSubCategories, this.form.lbSelectedITSubCategories)"
													type="button" value="Copy All">--></td>
                                            <td></td>
                                            <td valign="top" align="right">
                                                <asp:Button ID="btnITRemoveAll" CssClass="btn" runat="server" Text="Remove All" OnClick="btnITRemoveAll_Click"></asp:Button><!--<input id="btnITRemoveAll" onclick="javascript:removeall(this.form.lbSelectedITSubCategories)"
													type="button" value="Remove All">--></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="4" align="center" border="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="btnSubmit_Click"></asp:Button></td>
                            <td>
                                <asp:Button ID="btnReset" runat="server" CssClass="btn" Text="Reset" OnClick="btnReset_Click"></asp:Button></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>