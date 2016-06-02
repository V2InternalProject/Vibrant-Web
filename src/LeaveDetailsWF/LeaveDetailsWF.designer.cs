using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace LeaveDetailsWF
{
    partial class LeaveDetailsWF
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind21 = new System.Workflow.ComponentModel.ActivityBind();
            this.ssaCancelToCompleted = new System.Workflow.Activities.SetStateActivity();
            this.eaLeavecancel = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caLeaveCancel = new System.Workflow.Activities.CodeActivity();
            this.heaLeaveCancel = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaRejected = new System.Workflow.Activities.SetStateActivity();
            this.eaRejected = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caRejected = new System.Workflow.Activities.CodeActivity();
            this.heaRejected = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaApproval = new System.Workflow.Activities.SetStateActivity();
            this.eaApproval = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caGetApprovalRequiredData = new System.Workflow.Activities.CodeActivity();
            this.heaApproved = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaTOLevelOneApproval = new System.Workflow.Activities.SetStateActivity();
            this.eaAlertToApprover = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caGetLeaveDetailsInfo = new System.Workflow.Activities.CodeActivity();
            this.edaCancelled = new System.Workflow.Activities.EventDrivenActivity();
            this.edaRejected = new System.Workflow.Activities.EventDrivenActivity();
            this.edaWaitForApproval = new System.Workflow.Activities.EventDrivenActivity();
            this.siaLeaveDetials = new System.Workflow.Activities.StateInitializationActivity();
            this.saStatusActivity = new System.Workflow.Activities.StateActivity();
            this.csCompleted = new System.Workflow.Activities.StateActivity();
            this.isLeaveDetails = new System.Workflow.Activities.StateActivity();
            // 
            // ssaCancelToCompleted
            // 
            this.ssaCancelToCompleted.Name = "ssaCancelToCompleted";
            this.ssaCancelToCompleted.TargetStateName = "csCompleted";
            // 
            // eaLeavecancel
            // 
            activitybind1.Name = "LeaveDetailsWF";
            activitybind1.Path = "Body";
            this.eaLeavecancel.Cc = null;
            activitybind2.Name = "LeaveDetailsWF";
            activitybind2.Path = "From";
            this.eaLeavecancel.Name = "eaLeavecancel";
            activitybind3.Name = "LeaveDetailsWF";
            activitybind3.Path = "SMTPServer";
            activitybind4.Name = "LeaveDetailsWF";
            activitybind4.Path = "Subject";
            activitybind5.Name = "LeaveDetailsWF";
            activitybind5.Path = "To";
            this.eaLeavecancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.eaLeavecancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.eaLeavecancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.eaLeavecancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.eaLeavecancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // caLeaveCancel
            // 
            this.caLeaveCancel.Name = "caLeaveCancel";
            this.caLeaveCancel.ExecuteCode += new System.EventHandler(this.caLeaveCancel_ExecuteCode);
            // 
            // heaLeaveCancel
            // 
            this.heaLeaveCancel.EventName = "Cancel";
            this.heaLeaveCancel.InterfaceType = typeof(V2.Orbit.Workflow.LeaveDetailsWF.ILeaveDetails);
            this.heaLeaveCancel.Name = "heaLeaveCancel";
            // 
            // ssaRejected
            // 
            this.ssaRejected.Name = "ssaRejected";
            this.ssaRejected.TargetStateName = "csCompleted";
            // 
            // eaRejected
            // 
            activitybind6.Name = "LeaveDetailsWF";
            activitybind6.Path = "Body";
            this.eaRejected.Cc = null;
            activitybind7.Name = "LeaveDetailsWF";
            activitybind7.Path = "From";
            this.eaRejected.Name = "eaRejected";
            activitybind8.Name = "LeaveDetailsWF";
            activitybind8.Path = "SMTPServer";
            activitybind9.Name = "LeaveDetailsWF";
            activitybind9.Path = "Subject";
            activitybind10.Name = "LeaveDetailsWF";
            activitybind10.Path = "To";
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // caRejected
            // 
            this.caRejected.Name = "caRejected";
            this.caRejected.ExecuteCode += new System.EventHandler(this.caRejected_ExecuteCode);
            // 
            // heaRejected
            // 
            this.heaRejected.EventName = "Reject";
            this.heaRejected.InterfaceType = typeof(V2.Orbit.Workflow.LeaveDetailsWF.ILeaveDetails);
            this.heaRejected.Name = "heaRejected";
            // 
            // ssaApproval
            // 
            this.ssaApproval.Name = "ssaApproval";
            this.ssaApproval.TargetStateName = "csCompleted";
            // 
            // eaApproval
            // 
            activitybind11.Name = "LeaveDetailsWF";
            activitybind11.Path = "Body";
            this.eaApproval.Cc = null;
            activitybind12.Name = "LeaveDetailsWF";
            activitybind12.Path = "From";
            this.eaApproval.Name = "eaApproval";
            activitybind13.Name = "LeaveDetailsWF";
            activitybind13.Path = "SMTPServer";
            activitybind14.Name = "LeaveDetailsWF";
            activitybind14.Path = "Subject";
            activitybind15.Name = "LeaveDetailsWF";
            activitybind15.Path = "To";
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            // 
            // caGetApprovalRequiredData
            // 
            this.caGetApprovalRequiredData.Name = "caGetApprovalRequiredData";
            this.caGetApprovalRequiredData.ExecuteCode += new System.EventHandler(this.caGetApprovalRequiredData_ExecuteCode);
            // 
            // heaApproved
            // 
            this.heaApproved.EventName = "Approve";
            this.heaApproved.InterfaceType = typeof(V2.Orbit.Workflow.LeaveDetailsWF.ILeaveDetails);
            this.heaApproved.Name = "heaApproved";
            // 
            // ssaTOLevelOneApproval
            // 
            this.ssaTOLevelOneApproval.Name = "ssaTOLevelOneApproval";
            this.ssaTOLevelOneApproval.TargetStateName = "saStatusActivity";
            // 
            // eaAlertToApprover
            // 
            activitybind16.Name = "LeaveDetailsWF";
            activitybind16.Path = "Body";
            activitybind17.Name = "LeaveDetailsWF";
            activitybind17.Path = "Cc";
            activitybind18.Name = "LeaveDetailsWF";
            activitybind18.Path = "From";
            this.eaAlertToApprover.Name = "eaAlertToApprover";
            activitybind19.Name = "LeaveDetailsWF";
            activitybind19.Path = "SMTPServer";
            activitybind20.Name = "LeaveDetailsWF";
            activitybind20.Path = "Subject";
            activitybind21.Name = "LeaveDetailsWF";
            activitybind21.Path = "To";
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.CcProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.eaAlertToApprover.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind21)));
            // 
            // caGetLeaveDetailsInfo
            // 
            this.caGetLeaveDetailsInfo.Name = "caGetLeaveDetailsInfo";
            this.caGetLeaveDetailsInfo.ExecuteCode += new System.EventHandler(this.caGetLeaveDetailsInfo_ExecuteCode);
            // 
            // edaCancelled
            // 
            this.edaCancelled.Activities.Add(this.heaLeaveCancel);
            this.edaCancelled.Activities.Add(this.caLeaveCancel);
            this.edaCancelled.Activities.Add(this.eaLeavecancel);
            this.edaCancelled.Activities.Add(this.ssaCancelToCompleted);
            this.edaCancelled.Name = "edaCancelled";
            // 
            // edaRejected
            // 
            this.edaRejected.Activities.Add(this.heaRejected);
            this.edaRejected.Activities.Add(this.caRejected);
            this.edaRejected.Activities.Add(this.eaRejected);
            this.edaRejected.Activities.Add(this.ssaRejected);
            this.edaRejected.Name = "edaRejected";
            // 
            // edaWaitForApproval
            // 
            this.edaWaitForApproval.Activities.Add(this.heaApproved);
            this.edaWaitForApproval.Activities.Add(this.caGetApprovalRequiredData);
            this.edaWaitForApproval.Activities.Add(this.eaApproval);
            this.edaWaitForApproval.Activities.Add(this.ssaApproval);
            this.edaWaitForApproval.Name = "edaWaitForApproval";
            // 
            // siaLeaveDetials
            // 
            this.siaLeaveDetials.Activities.Add(this.caGetLeaveDetailsInfo);
            this.siaLeaveDetials.Activities.Add(this.eaAlertToApprover);
            this.siaLeaveDetials.Activities.Add(this.ssaTOLevelOneApproval);
            this.siaLeaveDetials.Name = "siaLeaveDetials";
            // 
            // saStatusActivity
            // 
            this.saStatusActivity.Activities.Add(this.edaWaitForApproval);
            this.saStatusActivity.Activities.Add(this.edaRejected);
            this.saStatusActivity.Activities.Add(this.edaCancelled);
            this.saStatusActivity.Name = "saStatusActivity";
            // 
            // csCompleted
            // 
            this.csCompleted.Name = "csCompleted";
            // 
            // isLeaveDetails
            // 
            this.isLeaveDetails.Activities.Add(this.siaLeaveDetials);
            this.isLeaveDetails.Name = "isLeaveDetails";
            // 
            // LeaveDetailsWF
            // 
            this.Activities.Add(this.isLeaveDetails);
            this.Activities.Add(this.csCompleted);
            this.Activities.Add(this.saStatusActivity);
            this.CompletedStateName = "csCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "isLeaveDetails";
            this.Name = "LeaveDetailsWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaLeavecancel;
        private CodeActivity caLeaveCancel;
        private HandleExternalEventActivity heaLeaveCancel;
        private EventDrivenActivity edaCancelled;
        private SetStateActivity ssaCancelToCompleted;
        private HandleExternalEventActivity heaRejected;
        private HandleExternalEventActivity heaApproved;
        private EventDrivenActivity edaRejected;
        private SetStateActivity ssaRejected;
        private CodeActivity caRejected;
        private SetStateActivity ssaApproval;
        private SetStateActivity ssaTOLevelOneApproval;
        private EventDrivenActivity edaWaitForApproval;
        private StateActivity saStatusActivity;
        private CodeActivity caGetApprovalRequiredData;
        private StateActivity csCompleted;
        private StateInitializationActivity siaLeaveDetials;
        private CodeActivity caGetLeaveDetailsInfo;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaApproval;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaRejected;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaAlertToApprover;
        private StateActivity isLeaveDetails;







































































    }
}
