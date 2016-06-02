
<%@ Page Language="c#" CodeBehind="ReportIssue.aspx.cs" AutoEventWireup="false" Inherits="Helpdesk.web.WebForm1"
    ValidateRequest="false" Title="ReportIssue"%>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="uc1" TagName="header" Src="~/HelpDesk/header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <!--[if lt IE 7]>
    <script src="dist/html5shiv.js"></script>
    <![endif]-->
    <title>Report Issue</title>
    <meta content="False" name="vs_snapToGrid">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script src="Script/common.js" type="text/javascript"></script>
    <script src="Script/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="Script/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Script/mm_menu.js" type="text/javascript"></script>
    <link href="css/allstyles.css" type="text/css" rel="stylesheet">
    <link href="css/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" type="text/css" />

  <%--  <link href="css/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="css/jquery.ui.dialog.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/jquery.ui.theme.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--	<script language="javascript">
//		function validateAndCheck() {
//		    
//                if (checkSubCategorySelection())
//                 {
//					return true;
//				}
//				else return false;
//
	    //		}
	    function validateAndCheck() {
	        if (validate()) {
	            if (checkSubCategorySelection()) {

	                return true;
	            }
	            else return false;
	        }
	        else return false;
	    }
		function validate() {
			var i = isRequire("txtName^txtEmailID^txtPhoneExtension^ddlCategories^txtDescription", "Name^Email ID^Phone Ext^Category^Description", this.enabled)
			if (i == true) {
				if (validateV2Email("txtEmailID", "Email ID") &&
				 validateV2Email("txtCCEmailID", "CC Email ID") &&
				 checkNumeric("txtPhoneExtension", "Phone Extension") )
				  {
					return true;
				}
				else {
					return false;
				}

				//return true;				
			}
			else {
				return false;
			}
		}
		function checkSubCategorySelection() {
			var ddlSubCategory = document.getElementById("ddlCategories");
			if (ddlSubCategory.options[ddlSubCategory.selectedIndex].value == "-1") {
				alert("Please select the Category");
				return false;
			}
			else if (ddlSubCategory.options[ddlSubCategory.selectedIndex].value == "0") {
				alert("Please select the Category and not the Department");
				return false;
			}
			else {
				return true;
			}
		}
		function setfocus() {
			document.getElementById("txtName").focus();

		}
		//function checkEmailID(txt, name)
		//{
		//	var validEmail = sub
		//}
	</script>--%>
    <script type="text/javascript">


        function validateAndCheck() {
            if (validate()) {
                if (checkSubCategorySelection()) {
                    try {
                        //setTimeout("$('#spinner').attr('src', 'Images/loading_animation.gif');", 100);
                        //$.blockUI({ message: $('div#divContent') });
                        $("#divContent").dialog({
                            closeOnEscape: false,
                            resizable: false,
                            height: 140,
                            width: 300,
                            modal: true,
                            dialogClass: "noclose"
                        });
                        
                    }
                    catch (e) {
                        $.unblockUI();
                    }
                    return true;


                }
                else return false;
            }
            else return false;
        }
        function validate() {
            var i = isRequire("txtName^txtEmailID^txtPhoneExtension^txtSeatingLocation^ddlCategories^ddltype^txtDescription", "Name^Email ID^Phone Ext^Seating Location^Category^Type^Description", this.enabled)
            if (i == true) {
                if (validateV2Email("txtEmailID", "Email ID") &&
				 validateV2Email("txtCCEmailID", "CC Email ID") &&
				 checkNumeric("txtPhoneExtension", "Phone Extension")) {
                    return true;
                }
                else {
                    return false;
                }

                //return true;				
            }
            else {
                return false;
            }
        }
        function checkSubCategorySelection() {
            var ddlSubCategory = document.getElementById("ddlCategories");
            if (ddlSubCategory.options[ddlSubCategory.selectedIndex].value == "-1") {
                alert("Please select the Category");
                return false;
            }
            else if (ddlSubCategory.options[ddlSubCategory.selectedIndex].value == "0") {
                alert("Please select the Category and not the Department");
                return false;
            }
            else {
                return true;
            }
        }
        function setfocus() {
            document.getElementById("txtName").focus();

        }

        function leaveChange() {
            var e = document.getElementById("ddlCategories");
            var strUser = e.options[e.selectedIndex].value;
            $.ajax({
                type: "post",
                url: 'ReportIssue.aspx/getDropdomdata',
                data: "{ 'stringParam': '" + strUser + "'}",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: true,
                success: function (data) {
                    document.getElementById("lblCategorySummary").innerHTML = data.d.toString();
                }
            });    // end $.ajax
        }

        //            function BlockUI() {
        //                try {
        //                    //setTimeout("$('#spinner').attr('src', 'Images/loading_animation.gif');", 100);
        //                    $.blockUI({ message: $('div#divContent') });
        //                }
        //                catch (e) {
        //                    $.unblockUI();
        //                }
        //            } 
 
    </script>
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        .style2
        {
            font-size: 11px;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        .LoadingWrap
        {
            margin: auto;
            margin-top: 26px;
        }

        .noclose .ui-dialog-titlebar-close
        {
            display: none;
        }
    </style>
</head>
<body onload="setfocus();" ms_positioning="GridLayout">
    <%--  <div class="headerbg">
         <img class="v2app" alt="People App" src="../../Images/vibrantweb.png">
         <img class="v2logoheader" alt="v2solutions" src="../../Images/v2logo.png">
    </div>--%>
    <div id="body1">
        <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="100%">
                    <uc1:header ID="header" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td class="">
                    &nbsp;<h3>Report Issue</h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" colspan="3">
                    <asp:Label ID="lblMessage" CssClass="success" runat="server"></asp:Label>
                    <asp:Label ID="lblMailError" runat="server" CssClass="error"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td class="blueBorderTop" height="500px;" width="715px" valign="top">
                    <table cellspacing="0" width="100%" cellpadding="5" border="0">
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Name:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:TextBox ID="txtName" CssClass="txtfield" runat="server" size="50" ValidationGroup="Add"
                                    CausesValidation="True" Enabled="False"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Email ID:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:TextBox ID="txtEmailID" CssClass="txtfield" runat="server" size="50" Enabled="False"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                                CC to (Manager/Lead Mail ID):
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:TextBox ID="txtCCEmailID" CssClass="txtfield" runat="server" size="50"></asp:TextBox>&nbsp;
                                <asp:Label ID="lblCcEmailError" CssClass="error" runat="server"></asp:Label>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="[A-Za-z\s]+"
								ErrorMessage="Enter only characters" ControlToValidate="txtName" ForeColor="Red"
								ValidationGroup="Add" CausesValidation="True" ></asp:RegularExpressionValidator>--%>
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Phone Extension:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:TextBox ID="txtPhoneExtension" CssClass="txtfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left" valign="top" style="padding-top: 9px;">
                               <font color="#ff3333">*</font> Seating Location:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:TextBox ID="txtSeatingLocation" CssClass="txtfield" MaxLength="25" runat="server"></asp:TextBox>&nbsp;
                                <span style="float: left; color: #999999; font-size: 11px; padding-bottom: 3px; padding-top: 3px;">
                                    (The code at the desk. Eg: V2MUM/GRFLR/0123)</span>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Categories:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <select id="ddlCategories" class="dropdown" runat="server" name="ddlCategories" onchange="leaveChange()">
                                </select>
                                <br />
                                <asp:Label ID="lblCategorySummary" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Type:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:DropDownList ID="ddltype" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Severity:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <!--<TR class="trcolor">
								<TD vAlign="top" align="right"><FONT color="#ff3333">*</FONT>Priority:</TD>
								<TD class="trcolor" vAlign="top"><asp:dropdownlist id="ddlPriority" runat="server" CssClass="dropdown"></asp:dropdownlist></TD>
							</TR>-->
                        <tr class="trcolor">
                            <td class="issuscol-left">
                                Upload File:
                            </td>
                            <td class="style2 issuscol-right" valign="top">
                                <input class="" id="uploadFiles" type="file" style="width: 200px;" size="40" name="uploadFiles"
                                    runat="server">
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td class="issuscol-left">
                               <font color="#ff3333">*</font> Description:
                            </td>
                            <td class="style2 issuscol-right" valign="bottom">
                                <textarea class="txtfield" id="txtDescription" onkeydown="textCounter(txtDescription,txtDescCount,1000)"
                                    onkeyup="textCounter(txtDescription,txtDescCount,1000)" name="txtDescription"
                                    runat="server"></textarea>
                            </td>
                            <td valign="bottom">
                                <input class="txtfieldlimit" id="txtDescCount" readonly type="text" maxlength="3"
                                    size="3" value="1000" name="txtDescCount">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right" colspan="2">
                            </td>
                        </tr>
                        <tr class="trcolor">
                            <td align="right" style="padding-top: 20px;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="Add" CausesValidation="True"
                                    class="btn1"></asp:Button>
                            </td>
                            <td align="left" colspan="3" style="padding-left: 5px; padding-top: 20px;">
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" class="btn1">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Add" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <!--Footer will come here-->
                </td>
            </tr>
        </table>
        </form>
    </div>
    <div id="divContent" title="Please Wait..." style="display: none">
        <p class="LoadingWrap" style="width: 25px;">
            <img src="Images/Loading.gif" style="width: 40px; height: 40px;"
                alt="Loading..." />
        </p>
    </div>
</body>
</html>
