<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JoinEmployeePopup.aspx.cs"
    Inherits="HRMS.Recruitment.JoinEmployeePopup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8; IE=7; IE=EDGE" />
    <title></title>
    <style type="text/css">
        .style1 {
            width: 140px;
        }

        .table-left-joinpopup {
            padding: 15px 30px 15px 110px;
            text-align: right;
        }

        .table-right-joinpopup {
            padding: 4px 16px 4px 30px;
        }

        .table-mrgntp-joinpopup {
            margin-top: 35px;
        }
    </style>
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/demo.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/jquery.mmenu.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/common.css" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" />
    <link href="../Content/themes/base/jquery.ui.dialog.css" rel="stylesheet" />

    <script src="../Scripts/New%20Design/jquery.selectBox.js"></script>
    <link href="../Content/New%20Design/jquery.selectBox%20(2).css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>

    <script language="javascript" type="text/javascript">
        function checkEmail() {
            var txtEmailID = document.getElementById('txtEmailID');
            if (txtEmailID.value == "") {
                txtEmailID.focus();
                alert('Please enter EmailID');
                txtEmailID.focus();
                return false;
            }
            if (txtEmailID.value != "") {

                var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
                var emailid = txtEmailID.value;
                var matchArray = emailid.match(emailPat);
                if (matchArray == null) {
                    alert('Please enter valid emailID');

                    txtEmailID.focus();
                    return false;
                }
            }

        }

        $(function () {
            $('select').selectBox({
                hideOnWindowScroll: true,
                keepInViewport: false
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            <div id="page">
                <section class="ConfirmationContainer Container">
                <div id="tblMain" runat="server" class="MainBody RecH sendmailpop">
                    <div class="rwrap">
                        <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                        <div class="clearfix">
                            <div class="clearfix sec3C FormContainerBox">
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label1" runat="server" Text="Candidate Status : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:DropDownList Enabled="false" ID="CandidateStatus" runat="server" Width="190px">
                                                    <asp:ListItem>Joined</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label2" runat="server" Text="Joining Date : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtJoiningDate" runat="server" ReadOnly="true" Width="160px"></asp:TextBox>
                                                <asp:ImageButton Visible="true" ID="imgJoiningDate" TabIndex="31" runat="server"
                                                    ImageUrl="../Images/New%20Design/calender-icon.png" />

                                                <ajaxToolkit:CalendarExtender ID="calextJoiningDate" runat="server" TargetControlID="txtJoiningDate"
                                                    PopupButtonID="imgJoiningDate" Format="MM/dd/yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label3" runat="server" Text="Contract Employee : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:CheckBox ID="chkIscontract" runat="server" AutoPostBack="true" OnCheckedChanged="chkIscontract_CheckedChanged" />
                                                <label for="chkIscontract" class="LabelForCheckbox">
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label4" runat="server" Text="Employee Code : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtEmployeeCode" Enabled="false" runat="server" Width="190px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label5" runat="server" Text="UserName : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="TextBox1" runat="server" Width="190px"></asp:TextBox>
                                                <div>
                                                    <asp:Label ID="Label7" Text="Username already exists" Visible="false" ForeColor="Red"
                                                        runat="server"></asp:Label></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="rightcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label6" runat="server" Text="EmailID : "></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="TextBox2" Enabled="false" Visible="false" runat="server" Width="190px"></asp:TextBox>
                                                <asp:TextBox ID="txtUserName" runat="server" Visible="false"></asp:TextBox>
                                                <div>
                                                    <asp:Label ID="lblUserNameError" Text="Username already exists" Visible="false" ForeColor="Red"
                                                        runat="server"></asp:Label></div>
                                                <asp:TextBox ID="txtEmailID" runat="server" Width="190px"></asp:TextBox>
                                                <div>
                                                    <asp:Label ID="lblEmailError" Text="EmailID already exists" Visible="false" ForeColor="Red"
                                                        runat="server"></asp:Label></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ButtonContainer1 clearfix">
                            <asp:Button ID="btnSubmit" OnClientClick="return checkEmail()" runat="server" Text="Submit"
                                OnClick="btnSubmit_Click" CssClass="ButtonGray" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="ButtonGray" />
                        </div>
                    </div>
                </div>
            </section>
            </div>
        </div>
    </form>
</body>
</html>