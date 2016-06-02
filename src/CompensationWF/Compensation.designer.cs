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


namespace V2.Orbit.Workflow.CompensationWF
{
    partial class Compensation
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
            this.ssaCancel = new System.Workflow.Activities.SetStateActivity();
            this.eaCancel = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caCancel = new System.Workflow.Activities.CodeActivity();
            this.heaCancel = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaRejected = new System.Workflow.Activities.SetStateActivity();
            this.eaRejected = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caRejected = new System.Workflow.Activities.CodeActivity();
            this.heaRejected = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaApproval = new System.Workflow.Activities.SetStateActivity();
            this.eaApproval = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caApproval = new System.Workflow.Activities.CodeActivity();
            this.heaApproval = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaInitialState = new System.Workflow.Activities.SetStateActivity();
            this.eaReportingToDetails = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caRepotingToDetails = new System.Workflow.Activities.CodeActivity();
            this.edaCancel = new System.Workflow.Activities.EventDrivenActivity();
            this.edaRejected = new System.Workflow.Activities.EventDrivenActivity();
            this.edaApproval = new System.Workflow.Activities.EventDrivenActivity();
            this.stateInitializationActivity1 = new System.Workflow.Activities.StateInitializationActivity();
            this.saCompleted = new System.Workflow.Activities.StateActivity();
            this.saApproval = new System.Workflow.Activities.StateActivity();
            this.CompensationInitialState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaCancel
            // 
            this.ssaCancel.Name = "ssaCancel";
            this.ssaCancel.TargetStateName = "saCompleted";
            // 
            // eaCancel
            // 
            activitybind1.Name = "Compensation";
            activitybind1.Path = "Body";
            activitybind2.Name = "Compensation";
            activitybind2.Path = "From";
            this.eaCancel.Name = "eaCancel";
            activitybind3.Name = "Compensation";
            activitybind3.Path = "Subject";
            activitybind4.Name = "Compensation";
            activitybind4.Path = "To";
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // caCancel
            // 
            this.caCancel.Name = "caCancel";
            this.caCancel.ExecuteCode += new System.EventHandler(this.caCancel_ExecuteCode);
            // 
            // heaCancel
            // 
            this.heaCancel.EventName = "Cancel";
            this.heaCancel.InterfaceType = typeof(V2.Orbit.Workflow.CompensationWF.ICompensation);
            this.heaCancel.Name = "heaCancel";
            // 
            // ssaRejected
            // 
            this.ssaRejected.Name = "ssaRejected";
            this.ssaRejected.TargetStateName = "saCompleted";
            // 
            // eaRejected
            // 
            activitybind5.Name = "Compensation";
            activitybind5.Path = "Body";
            activitybind6.Name = "Compensation";
            activitybind6.Path = "From";
            this.eaRejected.Name = "eaRejected";
            activitybind7.Name = "Compensation";
            activitybind7.Path = "Subject";
            activitybind8.Name = "Compensation";
            activitybind8.Path = "To";
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.eaRejected.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // caRejected
            // 
            this.caRejected.Name = "caRejected";
            this.caRejected.ExecuteCode += new System.EventHandler(this.caRejected_ExecuteCode);
            // 
            // heaRejected
            // 
            this.heaRejected.EventName = "Reject";
            this.heaRejected.InterfaceType = typeof(V2.Orbit.Workflow.CompensationWF.ICompensation);
            this.heaRejected.Name = "heaRejected";
            // 
            // ssaApproval
            // 
            this.ssaApproval.Name = "ssaApproval";
            this.ssaApproval.TargetStateName = "saCompleted";
            // 
            // eaApproval
            // 
            activitybind9.Name = "Compensation";
            activitybind9.Path = "Body";
            activitybind10.Name = "Compensation";
            activitybind10.Path = "From";
            this.eaApproval.Name = "eaApproval";
            activitybind11.Name = "Compensation";
            activitybind11.Path = "Subject";
            activitybind12.Name = "Compensation";
            activitybind12.Path = "To";
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.eaApproval.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // caApproval
            // 
            this.caApproval.Name = "caApproval";
            this.caApproval.ExecuteCode += new System.EventHandler(this.caApproval_ExecuteCode);
            // 
            // heaApproval
            // 
            this.heaApproval.EventName = "Approve";
            this.heaApproval.InterfaceType = typeof(V2.Orbit.Workflow.CompensationWF.ICompensation);
            this.heaApproval.Name = "heaApproval";
            // 
            // ssaInitialState
            // 
            this.ssaInitialState.Name = "ssaInitialState";
            this.ssaInitialState.TargetStateName = "saApproval";
            // 
            // eaReportingToDetails
            // 
            activitybind13.Name = "Compensation";
            activitybind13.Path = "Body";
            activitybind14.Name = "Compensation";
            activitybind14.Path = "From";
            this.eaReportingToDetails.Name = "eaReportingToDetails";
            activitybind15.Name = "Compensation";
            activitybind15.Path = "Subject";
            activitybind16.Name = "Compensation";
            activitybind16.Path = "To";
            this.eaReportingToDetails.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.eaReportingToDetails.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.eaReportingToDetails.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.eaReportingToDetails.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            // 
            // caRepotingToDetails
            // 
            this.caRepotingToDetails.Name = "caRepotingToDetails";
            this.caRepotingToDetails.ExecuteCode += new System.EventHandler(this.caRepotingToDetails_ExecuteCode);
            // 
            // edaCancel
            // 
            this.edaCancel.Activities.Add(this.heaCancel);
            this.edaCancel.Activities.Add(this.caCancel);
            this.edaCancel.Activities.Add(this.eaCancel);
            this.edaCancel.Activities.Add(this.ssaCancel);
            this.edaCancel.Name = "edaCancel";
            // 
            // edaRejected
            // 
            this.edaRejected.Activities.Add(this.heaRejected);
            this.edaRejected.Activities.Add(this.caRejected);
            this.edaRejected.Activities.Add(this.eaRejected);
            this.edaRejected.Activities.Add(this.ssaRejected);
            this.edaRejected.Name = "edaRejected";
            // 
            // edaApproval
            // 
            this.edaApproval.Activities.Add(this.heaApproval);
            this.edaApproval.Activities.Add(this.caApproval);
            this.edaApproval.Activities.Add(this.eaApproval);
            this.edaApproval.Activities.Add(this.ssaApproval);
            this.edaApproval.Name = "edaApproval";
            // 
            // stateInitializationActivity1
            // 
            this.stateInitializationActivity1.Activities.Add(this.caRepotingToDetails);
            this.stateInitializationActivity1.Activities.Add(this.eaReportingToDetails);
            this.stateInitializationActivity1.Activities.Add(this.ssaInitialState);
            this.stateInitializationActivity1.Name = "stateInitializationActivity1";
            // 
            // saCompleted
            // 
            this.saCompleted.Name = "saCompleted";
            // 
            // saApproval
            // 
            this.saApproval.Activities.Add(this.edaApproval);
            this.saApproval.Activities.Add(this.edaRejected);
            this.saApproval.Activities.Add(this.edaCancel);
            this.saApproval.Name = "saApproval";
            // 
            // CompensationInitialState
            // 
            this.CompensationInitialState.Activities.Add(this.stateInitializationActivity1);
            this.CompensationInitialState.Name = "CompensationInitialState";
            // 
            // Compensation
            // 
            this.Activities.Add(this.CompensationInitialState);
            this.Activities.Add(this.saApproval);
            this.Activities.Add(this.saCompleted);
            this.CompletedStateName = "saCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "CompensationInitialState";
            this.Name = "Compensation";
            this.CanModifyActivities = false;

        }

        #endregion

        private SetStateActivity ssaInitialState;
        private EventDrivenActivity edaCancel;
        private EventDrivenActivity edaRejected;
        private EventDrivenActivity edaApproval;
        private StateActivity saApproval;
        private SetStateActivity ssaApproval;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaApproval;
        private CodeActivity caApproval;
        private HandleExternalEventActivity heaApproval;
        private StateActivity saCompleted;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaRejected;
        private CodeActivity caRejected;
        private HandleExternalEventActivity heaRejected;
        private SetStateActivity ssaCancel;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaCancel;
        private CodeActivity caCancel;
        private HandleExternalEventActivity heaCancel;
        private SetStateActivity ssaRejected;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaReportingToDetails;
        private CodeActivity caRepotingToDetails;
        private StateInitializationActivity stateInitializationActivity1;
        private StateActivity CompensationInitialState;








































    }
}
