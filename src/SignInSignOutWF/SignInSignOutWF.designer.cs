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

namespace SignInSignOutWF
{
    partial class SignInSignOutWF
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
            this.ssaInitial = new System.Workflow.Activities.SetStateActivity();
            this.eaToLead = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caGetReportingToInfo = new System.Workflow.Activities.CodeActivity();
            this.edaCancel = new System.Workflow.Activities.EventDrivenActivity();
            this.edaRejected = new System.Workflow.Activities.EventDrivenActivity();
            this.edaApproval = new System.Workflow.Activities.EventDrivenActivity();
            this.siaInitialState = new System.Workflow.Activities.StateInitializationActivity();
            this.saCompleted = new System.Workflow.Activities.StateActivity();
            this.saApproval = new System.Workflow.Activities.StateActivity();
            this.Workflow1InitialState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaCancel
            // 
            this.ssaCancel.Name = "ssaCancel";
            this.ssaCancel.TargetStateName = "saCompleted";
            // 
            // eaCancel
            // 
            activitybind1.Name = "SignInSignOutWF";
            activitybind1.Path = "Body";
            activitybind2.Name = "SignInSignOutWF";
            activitybind2.Path = "From";
            this.eaCancel.Name = "eaCancel";
            activitybind3.Name = "SignInSignOutWF";
            activitybind3.Path = "SMTPServer";
            activitybind4.Name = "SignInSignOutWF";
            activitybind4.Path = "Subject";
            activitybind5.Name = "SignInSignOutWF";
            activitybind5.Path = "To";
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.eaCancel.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // caCancel
            // 
            this.caCancel.Name = "caCancel";
            this.caCancel.ExecuteCode += new System.EventHandler(this.caCanecl_ExecuteCode);
            // 
            // heaCancel
            // 
            this.heaCancel.EventName = "Cancel";
            this.heaCancel.InterfaceType = typeof(V2.Orbit.Workflow.SignInSignOutWF.ISignInSignOut);
            this.heaCancel.Name = "heaCancel";
            // 
            // ssaRejected
            // 
            this.ssaRejected.Name = "ssaRejected";
            this.ssaRejected.TargetStateName = "saCompleted";
            // 
            // eaRejected
            // 
            activitybind6.Name = "SignInSignOutWF";
            activitybind6.Path = "Body";
            activitybind7.Name = "SignInSignOutWF";
            activitybind7.Path = "From";
            this.eaRejected.Name = "eaRejected";
            activitybind8.Name = "SignInSignOutWF";
            activitybind8.Path = "SMTPServer";
            activitybind9.Name = "SignInSignOutWF";
            activitybind9.Path = "Subject";
            activitybind10.Name = "SignInSignOutWF";
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
            this.heaRejected.InterfaceType = typeof(V2.Orbit.Workflow.SignInSignOutWF.ISignInSignOut);
            this.heaRejected.Name = "heaRejected";
            // 
            // ssaApproval
            // 
            this.ssaApproval.Name = "ssaApproval";
            this.ssaApproval.TargetStateName = "saCompleted";
            // 
            // eaApproval
            // 
            activitybind11.Name = "SignInSignOutWF";
            activitybind11.Path = "Body";
            activitybind12.Name = "SignInSignOutWF";
            activitybind12.Path = "From";
            this.eaApproval.Name = "eaApproval";
            activitybind13.Name = "SignInSignOutWF";
            activitybind13.Path = "SMTPServer";
            activitybind14.Name = "SignInSignOutWF";
            activitybind14.Path = "Subject";
            activitybind15.Name = "SignInSignOutWF";
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
            this.heaApproval.InterfaceType = typeof(V2.Orbit.Workflow.SignInSignOutWF.ISignInSignOut);
            this.heaApproval.Name = "heaApproval";
            // 
            // ssaInitial
            // 
            this.ssaInitial.Name = "ssaInitial";
            this.ssaInitial.TargetStateName = "saApproval";
            // 
            // eaToLead
            // 
            activitybind16.Name = "SignInSignOutWF";
            activitybind16.Path = "Body";
            activitybind17.Name = "SignInSignOutWF";
            activitybind17.Path = "From";
            this.eaToLead.Name = "eaToLead";
            activitybind18.Name = "SignInSignOutWF";
            activitybind18.Path = "SMTPServer";
            activitybind19.Name = "SignInSignOutWF";
            activitybind19.Path = "Subject";
            activitybind20.Name = "SignInSignOutWF";
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
            // siaInitialState
            // 
            this.siaInitialState.Activities.Add(this.caGetReportingToInfo);
            this.siaInitialState.Activities.Add(this.eaToLead);
            this.siaInitialState.Activities.Add(this.ssaInitial);
            this.siaInitialState.Name = "siaInitialState";
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
            // Workflow1InitialState
            // 
            this.Workflow1InitialState.Activities.Add(this.siaInitialState);
            this.Workflow1InitialState.Name = "Workflow1InitialState";
            // 
            // SignInSignOutWF
            // 
            this.Activities.Add(this.Workflow1InitialState);
            this.Activities.Add(this.saApproval);
            this.Activities.Add(this.saCompleted);
            this.CompletedStateName = "saCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "Workflow1InitialState";
            this.Name = "SignInSignOutWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private StateInitializationActivity siaInitialState;
        private CodeActivity caGetReportingToInfo;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaToLead;
        private SetStateActivity ssaInitial;
        private HandleExternalEventActivity heaApproval;
        private EventDrivenActivity edaCancel;
        private EventDrivenActivity edaRejected;
        private EventDrivenActivity edaApproval;
        private StateActivity saCompleted;
        private StateActivity saApproval;
        private SetStateActivity ssaApproval;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaApproval;
        private CodeActivity caApproval;
        private SetStateActivity ssaRejected;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaRejected;
        private CodeActivity caRejected;
        private HandleExternalEventActivity heaRejected;
        private SetStateActivity ssaCancel;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaCancel;
        private CodeActivity caCancel;
        private HandleExternalEventActivity heaCancel;
        private StateActivity Workflow1InitialState;
























































    }
}
