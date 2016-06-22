<%@ Page Language="C#" AutoEventWireup="true" Inherits="SendMailPopUP" CodeBehind="SendMailPopUP.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8; IE=7; IE=EDGE" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/demo.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/jquery.mmenu.css" />
    <link type="text/css" rel="stylesheet" href="../Content/New%20Design/common.css" />
    <link href="../Content/New%20Design/hr.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>
    <link href="../Content/themes/base/jquery.ui.dialog.css" rel="stylesheet" />
    <%-- <link href="../../Content/New Design/jquery.selectbox.css" type="text/css" rel="stylesheet" />--%>
    <%--<script type="text/javascript" src="../../Scripts/New Design/jquery.selectbox-0.2.min.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function checkEmail() {
            var txtToEmailID = document.getElementById('txtTo');

            if (txtToEmailID.value == "") {
                txtToEmailID.focus();
                alert('Please enter To EmailID');
                txtToEmailID.focus();
                return false;
            }

            //             if (txtToEmailID.value != "") {
            //                 var splitEmail1 = 'txtToEmailID'.split(';');
            //                 var splitEmail2 = 'txtToEmailID'.split(' ');
            //                 var splitEmail3 = 'txtToEmailID'.split(',');

            //                 if (splitEmail1.length != 0) {

            //                     for (var i = 0; i < splitEmail1.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail1[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtToEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }
            //                 else if (splitEmail2.length != 0) {

            //                     for (var i = 0; i < splitEmail2.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail2[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtToEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }

            //                 else if (splitEmail3.length != 0) {

            //                     for (var i = 0; i < splitEmail3.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail3[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtToEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }

            //             }

            //             if (txtCCEmailID.value != "") {
            //                 var splitEmail1 = 'txtCCEmailID'.split(';');
            //                 var splitEmail2 = 'txtCCEmailID'.split(' ');
            //                 var splitEmail3 = 'txtCCEmailID'.split(',');

            //                 if (splitEmail1.length != 0) {

            //                     for (var i = 0; i < splitEmail1.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail1[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtCCEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }
            //                 else if (splitEmail2.length != 0) {

            //                     for (var i = 0; i < splitEmail2.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail2[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtCCEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }

            //                 else if (splitEmail3.length != 0) {

            //                     for (var i = 0; i < splitEmail3.length; i++) {
            //                         var emailPat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //                         var emailid = splitEmail3[i].value;
            //                         var matchArray = emailid.match(emailPat);

            //                         if (matchArray == null) {
            //                             alert('Please enter valid emailID');

            //                             txtCCEmailID.focus();
            //                             return false;
            //                         }
            //                     }
            //                 }

            //             }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div>

            <div id="page">
                <section class="ConfirmationContainer Container">
         <div id="tblMain" runat="server" class="MainBody RecH sendmailpop lblwidthreset">
              <div class="rwrap">
   <%-- <table>
    <tr><td colspan="2">
     </td></tr>
    <tr>
     <td>  </td> <td></td>
    </tr>
     <tr>
     <td>  </td> <td></td>
    </tr>
     <tr>
     <td> </td> <td></td>
    </tr>

     <tr>
     <td> </td> </td>
    </tr>
     <tr>
     <td colspan="2"> </td>
    </tr>
     <tr>
     <td valign="top"></td>  <td>
        </td>
    </tr>
    <tr><td>
         </td><td>
        <%--<asp:Button ID="btnClose" runat="server" Text="Close" onclick="btnClose_Click" /> --%><%--</td></tr>
    </table--%>

                    <div class="clearfix">
                             <div class="clearfix sec3C FormContainerBox">
                                  <asp:Label ID="lblError" Visible="false" ForeColor="Red" runat="server" Text=""></asp:Label>
                                 <div class="CandidateLeftcol clearfix mrgnB30">
                                     <div class="leftcol">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv">
                                               <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                                             </div>
                                             <div class="InputDiv">
                                               <asp:TextBox Enabled="false" ID="txtFrom" runat="server" Width="300px"></asp:TextBox>
                                             </div>
                                         </div>
                                     </div>
                                   </div>
                                     <div class="CandidateLeftcol clearfix mrgnB30">
                                     <div class="leftcol">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv">
                                               <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                                             </div>
                                             <div class="InputDiv">
                                               <asp:TextBox ID="txtTo" runat="server" Width="300px"></asp:TextBox>
                                             </div>
                                         </div>
                                     </div>
                                   </div>

                                  <div class="CandidateLeftcol clearfix mrgnB30">
                                     <div class="leftcol">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv">
                                              <asp:Label ID="lblCC" runat="server" Text="CC"></asp:Label>
                                             </div>
                                             <div class="InputDiv">
                                             <asp:TextBox ID="txtCC" runat="server" Width="300px"></asp:TextBox>
                                             </div>
                                         </div>
                                     </div>
                                    </div>

                                      <div class="CandidateLeftcol clearfix mrgnB30">
                                         <div class="leftcol">
                                            <div class="formrow clearfix">
                                                 <div class="LabelDiv">
                                                <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                                                 </div>
                                                 <div class="InputDiv">
                                                     <asp:TextBox ID="txtSubject" runat="server" Width="300px"></asp:TextBox>
                                                 </div>
                                             </div>
                                         </div>
                                    </div>

                                 <div class="CandidateLeftcol clearfix mrgnB30">
                                      <div class="leftcol">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv Width90">
                                             <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                                             </div>
                                         </div>
                                      </div>
                                  </div>
                                     <div class="CandidateLeftcol clearfix mrgnB30">
                                     <div class="leftcol mrgnL66">
                                         <div class="formrow clearfix">
                                             <div class="LabelDiv">
                                             <asp:Label ID="lblMessage"  runat="server" Text=""></asp:Label>
                                             </div>
                                             <div class="InputDiv">
                                            <asp:TextBox TextMode="MultiLine" ID="txtMessage" runat="server" Width="455px"  Height="322px"></asp:TextBox>
                                             </div>
                                         </div>
                                     </div>
                                   </div>
                               </div>
                                 </div>
                        </div>
                       <div class="ButtonContainer1 clearfix">
                         <asp:Button ID="btnSubmit" OnClientClick="return checkEmail()" runat="server" Text="Send"
                          onclick="btnSubmit_Click" CssClass="ButtonGray" />
                      </div>
            </div>
            </div>
            </section>
        </div>
        </div>
    </form>
</body>
</html>