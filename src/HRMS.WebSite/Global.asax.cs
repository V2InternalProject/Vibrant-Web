using HRMS.Controllers;
using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Workflow.Activities;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using V2.CommonServices.FileLogger;
using V2.Orbit.Workflow.AttendanceCheckerWF;
using V2.Orbit.Workflow.CompensationWF;
using V2.Orbit.Workflow.LeaveDetailsWF;
using V2.Orbit.Workflow.LeaveSchedulerWO;
using V2.Orbit.Workflow.LeaveUploadWF;
using V2.Orbit.Workflow.OutOfOfficeWF;
using V2.Orbit.Workflow.SignInSignOutWF;

namespace HRMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly FileLog objFileLog = FileLog.GetLogger();

        protected void Application_Start()
        {
            objFileLog.WriteLine(LogType.Info, "App start", "Global.asax", "App_Start", string.Empty);
            XmlConfigurator.Configure();
            log.Info("Vibrant Web Application started.");
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //timer.AutoReset = true;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //timer.Enabled = true;

            var SEM = new SEMController();
            SEM.SetTimerValue();

            //Resource Allocation Auto mail
            var resource = new ResourceController();
            // resource.SetTimerValue();
            var travel = new TravelController();
            travel.SetTimerValue();

            //Confirmation Process Auto mail
            var Confirmation = new ConfirmationProcessController();
            Confirmation.SetTimerValue();

            Application["LeaveBalanceWF"] = "";
            Application["AttendanceWF"] = "";
            Application["LeaveUpLoadWF"] = "";
            Application["WokflowRuntime"] = "";

            var workflowRuntime = Application["WokflowRuntime"] as WorkflowRuntime;

            if (workflowRuntime == null)
            {
                workflowRuntime = new WorkflowRuntime();

                workflowRuntime.WorkflowIdled += workflowRuntime_WorkflowIdled;
                workflowRuntime.WorkflowLoaded += workflowRuntime_WorkflowLoaded;
                workflowRuntime.WorkflowUnloaded += workflowRuntime_WorkflowUnloaded;
                workflowRuntime.WorkflowPersisted += workflowRuntime_WorkflowPersisted;
                workflowRuntime.WorkflowCompleted += delegate { };
                workflowRuntime.WorkflowTerminated +=
                    delegate(object sender1, WorkflowTerminatedEventArgs e1) { Debug.WriteLine(e1.Exception.Message); };

                // Add the Persistance Service

                //WorkflowPersistenceService workflowPersistanceService = new SqlWorkflowPersistenceService(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                //workflowRuntime.AddService(workflowPersistanceService);

                WorkflowSchedulerService workflowSchedulerService = new DefaultWorkflowSchedulerService();
                workflowRuntime.AddService(workflowSchedulerService);

                //System.Workflow.Runtime.Hosting. SqlTrackingService trackingService = new SqlTrackingService(connectionString2);

                //System.Workflow.Runtime.Tracking.SqlTrackingService workflowTrackingService = new System.Workflow.Runtime.Tracking.SqlTrackingService(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                //workflowRuntime.AddService(workflowTrackingService);

                // Add the External Data Exchange Service
                var dataExchangeService = new ExternalDataExchangeService();
                workflowRuntime.AddService(dataExchangeService);

                LeaveBalanceService objLeaveBalanceService;
                LeaveDetailsService objLeaveDetailsService;
                OutOfOfficeService objOutOfOfficeService;
                CompensationService objCompensationService;
                SignInSignOutService objSignInSignOutService;
                AttendanceCheckerService objAttendanceCheckerService;
                LeaveUploadService objLeaveUploadService;
                // Add a new instance of the LeaveBalanceService to the External Data Exchange Service
                objLeaveBalanceService = new LeaveBalanceService();
                objLeaveDetailsService = new LeaveDetailsService();
                objOutOfOfficeService = new OutOfOfficeService();
                objCompensationService = new CompensationService();
                objSignInSignOutService = new SignInSignOutService();
                objAttendanceCheckerService = new AttendanceCheckerService();
                objLeaveUploadService = new LeaveUploadService();

                //Adding our services to  ExternalDataExchangeService

                dataExchangeService.AddService(objLeaveBalanceService);
                dataExchangeService.AddService(objLeaveDetailsService);
                dataExchangeService.AddService(objOutOfOfficeService);
                dataExchangeService.AddService(objCompensationService);
                dataExchangeService.AddService(objSignInSignOutService);
                dataExchangeService.AddService(objAttendanceCheckerService);
                dataExchangeService.AddService(objLeaveUploadService);

                // Start the Workflow services
                //workflowRuntime.UnloadOnIdle = true;
                workflowRuntime.StartRuntime();
                Application["WokflowRuntime"] = workflowRuntime;
                //System.Web.HttpContext.Current.Cache["WokflowRuntime"]=workflowRuntime;
                //System.Web.HttpContext.Current.Cache["WokflowRuntime"]=workflowRuntime;
                objFileLog.WriteLine(LogType.Info, "App start finished", "Global.asax", "App_Start", string.Empty);
            }
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e)
        {
            objFileLog.WriteLine(LogType.Error, "App Error", "Global.asax", "App_Error",
                Server.GetLastError().ToString());
            objFileLog.WriteLine(LogType.Error, "App Error", "Global.asax", "App_Error",
                "---------Error Block End---------");
            log.Error(Server.GetLastError().ToString(), Server.GetLastError());
        }

        protected void Session_Start()
        {
            var globalID = Guid.NewGuid();
            Session["SecurityKey"] = globalID.ToString();
        }

        protected void Session_End()
        {
            Session.Abandon();
        }

        private void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            var cookie = FormsAuthentication.GetAuthCookie(User.Identity.Name, false, "/tools");
            Session["UserCookie"] = cookie;
            //Session["UName"] = User.Identity.Name;
            Session["LoginUserName"] = "0";
            Session["UserID"] = "0"; // added by viveka
            Session["CommandName"] = "";

            log.Info("User : " + User.Identity.Name + " logged in to Vibrant Web " + DateTime.Now);
        }

        #region Workflow events

        private static void workflowRuntime_WorkflowPersisted(object sender, WorkflowEventArgs e)
        {
            // Debug.WriteLine("Persisted " +e.WorkflowInstance.InstanceId + " on "+DateTime.Now.ToUniversalTime() );
        }

        private static void workflowRuntime_WorkflowUnloaded(object sender, WorkflowEventArgs e)
        {
            Debug.WriteLine("Unloaded " + e.WorkflowInstance.InstanceId + " on " + DateTime.Now.ToUniversalTime());
        }

        private static void workflowRuntime_WorkflowLoaded(object sender, WorkflowEventArgs e)
        {
            Debug.WriteLine("Loaded " + e.WorkflowInstance.InstanceId + " on " + DateTime.Now.ToUniversalTime());
        }

        private static void workflowRuntime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            Debug.WriteLine("Idled " + e.WorkflowInstance.InstanceId + " on " + DateTime.Now.ToUniversalTime());
            try
            {
                e.WorkflowInstance.Unload();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Workflow events
    }
}