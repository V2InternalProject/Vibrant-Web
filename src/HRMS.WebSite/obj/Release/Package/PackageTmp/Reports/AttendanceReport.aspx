<%--<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />
		<meta name="viewport" content="width=device-width initial-scale=1.0 maximum-scale=1.0 user-scalable=yes" />
		<meta http-equiv="x-ua-compatible" content="IE=edge">
		<link type="text/css" rel="stylesheet" href="css/demo.css" />
		<link type="text/css" rel="stylesheet" href="css/jquery.mmenu.css" />
		<link type="text/css" rel="stylesheet" href="css/common.css" />
		<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
		<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>
		<!-- for menu -->
		<script type="text/javascript" src="scripts/jquery.mmenu.js"></script>

		<!-- SelectBOx -->
    	<link href="css/jquery.selectbox.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript" src="scripts/jquery.selectbox-0.2.min.js"></script>
		<script type="text/javascript">
		    $(document).ready(function () {
		        $('.OrbitFilterLink').click(function () {
		            $('.OrbitFilterExpand').toggle('slide', { direction: 'right' }, 600);
		        });

		    });
		    $(function () {
		        $('nav#menu').mmenu({
		            slidingSubmenus: false
		        });
		    });

		    $(function () {
		        $('select').selectbox();
		    });
		</script>
	</head>

		<body class="LeaveManagement">
			<div id="page">
				<section class="clearfix">
					<header id="header">
						<div class="SideMenuConBorderR">
							<a href="#menu" id="SlideMenuBtn"></a>
						</div>
						<h1>Vibrant Web</h1>
						<div class="UserLogout">
							<div class="ImgConBorderL">
								<img src="images/logout.png" alt="logout" />
							</div>
							<div class="ImgConBorderL">
								<img src="images/user.png" alt="user" />
							</div>
							<p class="floatR mrgnR15">Namrata</p>
						</div>
					</header>

					<nav id="menu" class="slide-menu">
						<ul>
							<li class="Selected head mm-subopen"><img src="images/orbit-icon.png" class="menu-logo" alt="vibrantweb"><a href="#">MY VIBRANT WEB</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="myvb">Attendance</a></li>
									<li class="align"><a href="#" class="myvb">Leave Management</a></li>
									<li class="align"><a href="#" class="myvb">Out of Office</a></li>
									<li class="align"><a href="#" class="myvb">Approvals</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/HR-icon.png" class="menu-logo" alt="processes"><a href="#">HR PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="hr">Confirmation</a></li>
									<li class="align"><a href="#" class="hr">Appraisal</a></li>
									<li class="align"><a href="#" class="hr">Separation</a></li>
									<li class="align"><a href="#" class="hr">Smart Track</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/logout-icon.png" class="menu-logo" alt="reports"><a href="#">REPORTS</a>
							</li>
							<li class="head"><img src="images/finance-icon.png" class="menu-logo" alt"finance"><a href="#">FINANCE PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="finance">Expense Reimbursement</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/admin-icon.png" class="menu-logo" alt="admin"><a href="#">ADMIN PROCESSES</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="admin">Travel</a></li>
									<li class="align"><a href="#" class="admin">Helpdesk</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/settings-icon.png" class="menu-logo" alt="settings"><a href="#">SETTINGS</a>
								<ul class="slide-submenu">
									<li class="submenu1"><a href="#" class="settings">Confirmation</a></li>
									<li class="align"><a href="#" class="settings">Appraisal</a></li>
									<li class="align"><a href="#" class="settings">Separation</a></li>
								</ul>
							</li>
							<li class="head"><img src="images/logout-icon.png" class="menu-logo" alt="logout"><a href="#">LOG OUT</a></li>
						<!-- 	</ul> -->
					</nav>
				</section>
				<section class="ReportContainer Container">
					    <div class="FixedHeader">
							<div class="clearfix">
								<h2 class="MainHeading">Reports</h2>
								<div class="EmpSearch clearfix">
									<a href="#"></a>
									<input type="text" placeholder="Employee Search">
								</div>
							</div>
							<nav class="sub-menu-colored">
								<a href="#" class="selected">Attendance</a>
								<a href="#">SignIn SignOut</a>
								<a href="#">Absenteeism</a>
								<a href="#">Leave</a>
								<a href="#">Leave Transaction</a>
								<a href="#">Compensatory Leave</a>
								<a href="#">Out Of Office</a>
							</nav>
						</div>
					<div class="MainBody ReportMainbody">
						<p class="attendancenote">*Legend : P = Present, A = Absent, F = Future Date, L = Leave, C = Compensatory Leave, W = Weekly Off, H = Holiday</p>
						<div class="FormContainerBox  Oreport clearfix">
                            <div class="formrow clearfix">
                                <div class="leftcol clearfix">
                                    <div class="LabelDiv">
                                        <label>Shift Name:</label>
                                    </div>
                                    <div class="InputDiv">
                                         <select>
                                            <option>abc</option>
                                            <option>abc</option>
                                            <option>abc</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="rightcol">
                                    <div class="LabelDiv">
                                        <label>Employee Name:</label>
                                    </div>
                                    <div class="InputDiv">
                                         <select>
                                            <option>abc</option>
                                            <option>abc</option>
                                            <option>abc</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="formrow clearfix">
                                <div class="leftcol clearfix">
                                    <div class="LabelDiv">
                                        <label>Select Month:</label>
                                    </div>
                                    <div class="InputDiv">
                                         <select>
                                            <option>abc</option>
                                            <option>abc</option>
                                            <option>abc</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="rightcol">
                                    <div class="LabelDiv">
                                        <label>Select Year:</label>
                                    </div>
                                    <div class="InputDiv">
                                        <select>
                                            <option>abc</option>
                                            <option>abc</option>
                                            <option>abc</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ButtonContainer1 clearfix">
                            <input type="button" value="Submit" class="ButtonGray" />
                        </div>
   				 </div>
</section>
				<footer>&#169; 2008 V2Solutions, Inc.</footer>
			</div>
			<script type="text/javascript" language="javascript" src="footer.js"></script>
			<script type="text/javascript" language="javascript" src="js/common.js"></script>
		</body>
	</html>--%>