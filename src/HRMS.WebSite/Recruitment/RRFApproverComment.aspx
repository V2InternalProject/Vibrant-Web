<%@ Page Title="" Language="C#" MasterPageFile="../Views/Shared/HRMS.master" AutoEventWireup="true" Inherits="RRFApproverComment" CodeBehind="RRFApproverComment.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="RecruiterHeader" Src="~/Recruitment/UserControl/RecruitementHeader.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../Content/New%20Design/demo.css" rel="stylesheet" />
    <link href="../Content/New%20Design/common.css" rel="stylesheet" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" />
    <script language="VBScript" type="text/vbscript">
        Function myAlert(title, content)
            MsgBox content, 0,title
        End Function
    </script>
    <script language="javascript" type="text/javascript">
        var title = "";
        function Validate() {
            var flag = 1;
            var message = '';
            var comments = document.getElementById('<%=txtReasonFor.ClientID %>').value;
            if (comments == "") {
                message = message + '<p>' + 'Please enter comments.' + '</p>';
                document.getElementById('<%=txtReasonFor.ClientID %>').value = "";
                document.getElementById('<%=txtReasonFor.ClientID %>').focus();
                flag = 0;
            }
            if (flag == 1) {
                // alert(document.getElementById('<%=pnlRRFComments.ClientID %>'));
                //                document.getElementById('<%=btnSendToAddComment.ClientID %>').style.display = 'none';
                //                document.getElementById('<%=txtReasonFor.ClientID %>').style.display = 'none';
                document.getElementById('<%=pnlRRFComments.ClientID %>').style.display = 'none';
                document.getElementById('<%=btnBack.ClientID %>').style.display = 'none';
                DisplayLoadingDialog();
                //document.getElementById('<%=loadImage.ClientID %>').style.display = 'inline';
                return true;
            }
            else {
                V2hrmsAlert(message, title);
                // alert(message);
                // myAlert("Recruitment Module", message);
                return false;
            }
        }
    </script>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <%--    <body class="AttendancePage">--%>
    <div id="page">
        <section class="ConfirmationContainer Container">
            <uc1:RecruiterHeader ID="RecruiterHeader1" runat="server"></uc1:RecruiterHeader>
            <div class="MainBody hrmF RequestorADDNEW app">
                <div class="rwrap clearfix">
                    <div class="clearfix">
                        <span id="guide">
                            <asp:Button ID="btnBack" OnClientClick="javascript:history.go(-1);return false;"
                                Width="100px" runat="server" Text="Back" CssClass="ButtonGray BackLink" />
                            <asp:Button ID="btnRedirect" Width="100px" runat="server" Text="Back" OnClick="btnRedirect_Click"
                                Visible="false" CssClass="ButtonGray BackLink" />
                        </span>
                    </div>
                    <%--                       <h3 class="smartrackH">Cancel RRF</h3>--%>
                    <div id="">
                        <h3 class="smartrackH mrgnB20">
                            <asp:Label ID="lblTitle" runat="server" SkinID="lblWelcome"></asp:Label></h3>
                    </div>
                    <asp:Label ID="lblSuccess" runat="server" Visible="false" SkinID="lblSuccess" Text="*"></asp:Label>

                    <asp:Image ID="loadImage" ImageUrl="~/Images/loading.gif" Style="display: none" runat="server"></asp:Image>

                    <asp:Panel ID="pnlRRFComments" runat="server">

                        <div class="clearfix">
                            <div class="clearfix sec3C FormContainerBox">
                                <div class="CandidateLeftcol clearfix mrgnB30">
                                    <div class="leftcol">
                                        <div class="formrow clearfix">
                                            <div class="LabelDiv">
                                                <asp:Label ID="Label1" runat="server" Text="" SkinID="lblError">*</asp:Label>
                                                <asp:Label ID="lblReasonFor" runat="server"></asp:Label>
                                            </div>
                                            <div class="InputDiv">
                                                <asp:TextBox ID="txtReasonFor" runat="server" MaxLength="100" TextMode="MultiLine"
                                                    TabIndex="0"></asp:TextBox>
                                                <div class="ClassTextareaDiv"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix mrgnT20">
                                <asp:Button ID="btnSendToAddComment" runat="server" Text="Send" OnClick="btnSendToAddComment_Click"
                                    OnClientClick="javascript:return Validate()" TabIndex="1" CssClass="ButtonGray" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </section>
    </div>
    <%--  </body>--%>
</asp:Content>