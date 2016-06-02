using System;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.Workflow.AttendanceCheckerWF;
using V2.Orbit.Workflow.LeaveSchedulerWO;
using V2.Orbit.Workflow.LeaveUploadWF;

namespace HRMS.Orbitweb
{
    public partial class StartWorkflows : System.Web.UI.Page
    {
        private LeaveBalanceService objLeaveBalanceService;
        private AttendanceCheckerService objAttendanceCheckerService;
        private LeaveUploadService objLeaveUploadService;
        private WorkflowRuntime workflowRuntime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetWorkFlowStatus();
            }
        }

        public void GetWorkFlowStatus()
        {
            workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
            if (Application["LeaveBalanceWF"].ToString() != "")
            {
                try
                {
                    WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveBalanceWF"].ToString()));
                    if (instance == null)
                    {
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;
                        btnOnDemandLeaveBalance.Enabled = false;
                    }
                    else
                    {
                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                        btnOnDemandLeaveBalance.Enabled = true;
                    }
                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    Application["LeaveBalanceWF"] = "";
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                    //throw new V2Exceptions(ex.ToString(),ex);
                }
            }

            if (Application["AttendanceWF"].ToString() != "")
            {
                try
                {
                    WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["AttendanceWF"].ToString()));
                    if (instance == null)
                    {
                        btnACStart.Enabled = true;
                        btnACStop.Enabled = false;
                        btnOnDemandAttendencCheck.Enabled = false;
                    }
                    else
                    {
                        btnACStart.Enabled = false;
                        btnACStop.Enabled = true;
                        btnOnDemandAttendencCheck.Enabled = true;
                    }
                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    Application["AttendanceWF"] = "";
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
            }
            if (Application["LeaveUpLoadWF"].ToString() != "")
            {
                try
                {
                    WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveUpLoadWF"].ToString()));
                    if (instance == null)
                    {
                        btnLUStart.Enabled = true;
                        btnLUStop.Enabled = false;
                        btnLURefreshLeaveCredit.Enabled = false;
                    }
                    else
                    {
                        btnLUStart.Enabled = false;
                        btnLUStop.Enabled = true;
                        btnLURefreshLeaveCredit.Enabled = true;
                    }
                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    Application["LeaveUpLoadWF"] = "";
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
            }
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];  //System.Web.HttpContext.Current.Items["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof(LeaveScheduler));
                Application["LeaveBalanceWF"] = instance.InstanceId.ToString();
                instance.Start();
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveBalanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnOnDemandLeaveBalance_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveBalanceWF"].ToString()));
                instance.Resume();
                objLeaveBalanceService = (LeaveBalanceService)workflowRuntime.GetService(typeof(LeaveBalanceService));
                objLeaveBalanceService.RaiseOnDemandLeaveBalanceEvent(instance.InstanceId);
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveBalanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnACStart_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];  //System.Web.HttpContext.Current.Items["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof(AttendanceCheckerWF.AttendanceWF));
                Application["AttendanceWF"] = instance.InstanceId.ToString();
                instance.Start();
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["AttendanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnACStop_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["AttendanceWF"].ToString()));
                instance.Resume();
                objAttendanceCheckerService = (AttendanceCheckerService)workflowRuntime.GetService(typeof(AttendanceCheckerService));
                objAttendanceCheckerService.RaiseStopEvent(instance.InstanceId);
                Application["AttendanceWF"] = "";
                btnACStart.Enabled = true;
                btnACStop.Enabled = false;
                btnOnDemandAttendencCheck.Enabled = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["AttendanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnOnDemandAttendencCheck_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["AttendanceWF"].ToString()));
                instance.Resume();
                objAttendanceCheckerService = (AttendanceCheckerService)workflowRuntime.GetService(typeof(AttendanceCheckerService));
                objAttendanceCheckerService.RaiseOnDemandAttendanceCheckEvent(instance.InstanceId);
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["AttendanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveBalanceWF"].ToString()));
                instance.Resume();
                objLeaveBalanceService = (LeaveBalanceService)workflowRuntime.GetService(typeof(LeaveBalanceService));
                objLeaveBalanceService.RaiseStopEvent(instance.InstanceId);
                Application["LeaveBalanceWF"] = "";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnOnDemandLeaveBalance.Enabled = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveBalanceWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnLUStart_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];  //System.Web.HttpContext.Current.Items["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof(LeaveUpLoadWF));
                Application["LeaveUpLoadWF"] = instance.InstanceId.ToString();
                instance.Start();
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveUpLoadWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnLUStop_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveUpLoadWF"].ToString()));
                instance.Resume();
                objLeaveUploadService = (LeaveUploadService)workflowRuntime.GetService(typeof(LeaveUploadService));
                objLeaveUploadService.RaiseStopEvent(instance.InstanceId);
                Application["LeaveUpLoadWF"] = "";
                btnLUStart.Enabled = true;
                btnLUStop.Enabled = false;
                btnLURefreshLeaveCredit.Enabled = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveUpLoadWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnLURefreshLeaveCredit_Click(object sender, EventArgs e)
        {
            try
            {
                WorkflowRuntime workflowRuntime = (WorkflowRuntime)Application["WokflowRuntime"];
                WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid(Application["LeaveUpLoadWF"].ToString()));
                instance.Resume();
                objLeaveUploadService = (LeaveUploadService)workflowRuntime.GetService(typeof(LeaveUploadService));
                objLeaveUploadService.RaiseOnDemandAttendanceCheckEvent(instance.InstanceId);
                GetWorkFlowStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                Application["LeaveUpLoadWF"] = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}