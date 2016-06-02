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

namespace OutOfOfficeWF
{
    partial class OutOfOfficeWF
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
            this.ssaCancelled = new System.Workflow.Activities.SetStateActivity();
            this.eaCancelled = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caCancelled = new System.Workflow.Activities.CodeActivity();
            this.heaCancelled = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaToCompletedFromRejected = new System.Workflow.Activities.SetStateActivity();
            this.eaRejected = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caRejected = new System.Workflow.Activities.CodeActivity();
            this.heaRejected = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaApprovalToCompleted = new System.Workflow.Activities.SetStateActivity();
            this.eaApproval = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caApproval = new System.Workflow.Activities.CodeActivity();
            this.heaApproval = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaToApproval = new System.Workflow.Activities.SetStateActivity();
            this.eaToLead = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caGetReportingToInfo = new System.Workflow.Activities.CodeActivity();
            this.edaCancelled = new System.Workflow.Activities.EventDrivenActivity();
            this.edaRejected = new System.Workflow.Activities.EventDrivenActivity();
            this.edaApproval = new System.Workflow.Activities.EventDrivenActivity();
            this.stateInitializationActivity1 = new System.Workflow.Activities.StateInitializationActivity();
            this.saOutOfOfficeApproval = new System.Workflow.Activities.StateActivity();
            this.saCompleted = new System.Workflow.Activities.StateActivity();
            this.OutOfOfficeWFInitialState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaCancelled
            // 
            this.ssaCancelled.Name = "ssaCancelled";
            this.ssaCancelled.TargetStateName = "saCompleted";
            // 
            // eaCancelled
            // 
            activitybind1.Name = "OutOfOfficeWF";
            activitybind1.Path = "Body";
            activitybind2.Name = "OutOfOfficeWF";
            activitybind2.Path = "From";
            this.eaCancelled.Name = "eaCancelled";
            activitybind3.Name = "OutOfOfficeWF";
            activitybind3.Path = "SMTPServer";
            activitybind4.Name = "OutOfOfficeWF";
            activitybind4.Path = "Subject";
            activitybind5.Name = "OutOfOfficeWF";
            activitybind5.Path = "To";
            this.eaCancelled.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.eaCancelled.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.eaCancelled.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.eaCancelled.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.eaCancelled.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // caCancelled
            // 
            this.caCancelled.Name = "caCancelled";
            this.caCancelled.ExecuteCode += new System.EventHandler(this.caCancelled_ExecuteCode);
            // 
            // heaCancelled
            // 
            this.heaCancelled.EventName = "Cancel";
            this.heaCancelled.InterfaceType = typeof(V2.Orbit.Workflow.OutOfOfficeWF.IOutOfOffice);
            this.heaCancelled.Name = "heaCancelled";
            // 
            // ssaToCompletedFromRejected
            // 
            this.ssaToCompletedFromRejected.Name = "ssaToCompletedFromRejected";
            this.ssaToCompletedFromRejected.TargetStateName = "saCompleted";
            // 
            // eaRejected
            // 
            activitybind6.Name = "OutOfOfficeWF";
            activitybind6.Path = "Body";
            activitybind7.Name = "OutOfOfficeWF";
            activitybind7.Path = "From";
            this.eaRejected.Name = "eaRejected";
            activitybind8.Name = "OutOfOfficeWF";
            activitybind8.Path = "SMTPServer";
            activitybind9.Name = "OutOfOfficeWF";
            activitybind9.Path = "Subject";
            activitybind10.Name = "OutOfOfficeWF";
            activitybind10.Path = "To";
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
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
            this.heaRejected.InterfaceType = typeof(V2.Orbit.Workflow.OutOfOfficeWF.IOutOfOffice);
            this.heaRejected.Name = "heaRejected";
            // 
            // ssaApprovalToCompleted
            // 
            this.ssaApprovalToCompleted.Name = "ssaApprovalToCompleted";
            this.ssaApprovalToCompleted.TargetStateName = "saCompleted";
            // 
            // eaApproval
            // 
            activitybind11.Name = "OutOfOfficeWF";
            activitybind11.Path = "Body";
            activitybind12.Name = "OutOfOfficeWF";
            activitybind12.Path = "From";
            this.eaApproval.Name = "eaApproval";
            activitybind13.Name = "OutOfOfficeWF";
            activitybind13.Path = "SMTPServer";
            activitybind14.Name = "OutOfOfficeWF";
            activitybind14.Path = "Subject";
            activitybind15.Name = "OutOfOfficeWF";
            activitybind15.Path = "To";
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            // 
            // caApproval
            // 
            this.caApproval.Name = "caApproval";
            this.caApproval.ExecuteCode += new System.EventHandler(this.caApproval_ExecuteCode);
            // 
            // heaApproval
            // 
            this.heaApproval.EventName = "Approve";
            this.heaApproval.InterfaceType = typeof(V2.Orbit.Workflow.OutOfOfficeWF.IOutOfOffice);
            this.heaApproval.Name = "heaApproval";
            // 
            // ssaToApproval
            // 
            this.ssaToApproval.Name = "ssaToApproval";
            this.ssaToApproval.TargetStateName = "saOutOfOfficeApproval";
            // 
            // eaToLead
            // 
            activitybind16.Name = "OutOfOfficeWF";
            activitybind16.Path = "Body";
            activitybind17.Name = "OutOfOfficeWF";
            activitybind17.Path = "From";
            this.eaToLead.Name = "eaToLead";
            activitybind18.Name = "OutOfOfficeWF";
            activitybind18.Path = "SMTPServer";
            activitybind19.Name = "OutOfOfficeWF";
            activitybind19.Path = "Subject";
            activitybind20.Name = "OutOfOfficeWF";
            activitybind20.Path = "To";
            this.eaToLead.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.eaToLead.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.eaToLead.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.eaToLead.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            this.eaToLead.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            // 
            // caGetReportingToInfo
            // 
            this.caGetReportingToInfo.Name = "caGetReportingToInfo";
            this.caGetReportingToInfo.ExecuteCode += new System.EventHandler(this.caGetReportingToInfo_ExecuteCode);
            // 
            // edaCancelled
            // 
            this.edaCancelled.Activities.Add(this.heaCancelled);
            this.edaCancelled.Activities.Add(this.caCancelled);
            this.edaCancelled.Activities.Add(this.eaCancelled);
            this.edaCancelled.Activities.Add(this.ssaCancelled);
            this.edaCancelled.Name = "edaCancelled";
            // 
            // edaRejected
            // 
            this.edaRejected.Activities.Add(this.heaRejected);
            this.edaRejected.Activities.Add(this.caRejected);
            this.edaRejected.Activities.Add(this.eaRejected);
            this.edaRejected.Activities.Add(this.ssaToCompletedFromRejected);
            this.edaRejected.Name = "edaRejected";
            // 
            // edaApproval
            // 
            this.edaApproval.Activities.Add(this.heaApproval);
            this.edaApproval.Activities.Add(this.caApproval);
            this.edaApproval.Activities.Add(this.eaApproval);
            this.edaApproval.Activities.Add(this.ssaApprovalToCompleted);
            this.edaApproval.Name = "edaApproval";
            // 
            // stateInitializationActivity1
            // 
            this.stateInitializationActivity1.Activities.Add(this.caGetReportingToInfo);
            this.stateInitializationActivity1.Activities.Add(this.eaToLead);
            this.stateInitializationActivity1.Activities.Add(this.ssaToApproval);
            this.stateInitializationActivity1.Name = "stateInitializationActivity1";
            // 
            // saOutOfOfficeApproval
            // 
            this.saOutOfOfficeApproval.Activities.Add(this.edaApproval);
            this.saOutOfOfficeApproval.Activities.Add(this.edaRejected);
            this.saOutOfOfficeApproval.Activities.Add(this.edaCancelled);
            this.saOutOfOfficeApproval.Name = "saOutOfOfficeApproval";
            // 
            // saCompleted
            // 
            this.saCompleted.Name = "saCompleted";
            // 
            // OutOfOfficeWFInitialState
            // 
            this.OutOfOfficeWFInitialState.Activities.Add(this.stateInitializationActivity1);
            this.OutOfOfficeWFInitialState.Name = "OutOfOfficeWFInitialState";
            // 
            // OutOfOfficeWF
            // 
            this.Activities.Add(this.OutOfOfficeWFInitialState);
            this.Activities.Add(this.saCompleted);
            this.Activities.Add(this.saOutOfOfficeApproval);
            this.CompletedStateName = "saCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "OutOfOfficeWFInitialState";
            this.Name = "OutOfOfficeWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private SetStateActivity ssaApprovalToCompleted;
        private CodeActivity caApproval;
        private HandleExternalEventActivity heaApproval;
        private SetStateActivity ssaToApproval;
        private EventDrivenActivity edaApproval;
        private EventDrivenActivity edaRejected;
        private CodeActivity caRejected;
        private HandleExternalEventActivity heaRejected;
        private SetStateActivity ssaCancelled;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaCancelled;
        private CodeActivity caCancelled;
        private HandleExternalEventActivity heaCancelled;
        private SetStateActivity ssaToCompletedFromRejected;
        private EventDrivenActivity edaCancelled;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaRejected;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaApproval;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaToLead;
        private CodeActivity caGetReportingToInfo;
        private StateInitializationActivity stateInitializationActivity1;
        private StateActivity saOutOfOfficeApproval;
        private StateActivity saCompleted;
        private StateActivity OutOfOfficeWFInitialState;




















































    }
}
