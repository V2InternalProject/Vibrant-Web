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

namespace V2.Orbit.Workflow.LeaveUploadWF
{
    partial class LeaveUpLoadWF
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
            this.ssaComplete = new System.Workflow.Activities.SetStateActivity();
            this.heaStopUpload = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaOnDemandLeaveUpload = new System.Workflow.Activities.SetStateActivity();
            this.heaOnDemandLeaveUpload = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaTimeOut = new System.Workflow.Activities.SetStateActivity();
            this.daTimeOut = new System.Workflow.Activities.DelayActivity();
            this.ssaUploadandSendingMail = new System.Workflow.Activities.SetStateActivity();
            this.caUploadandSendingMail = new System.Workflow.Activities.CodeActivity();
            this.ssaLeaveUpload = new System.Workflow.Activities.SetStateActivity();
            this.edaStopUpload = new System.Workflow.Activities.EventDrivenActivity();
            this.edaOndemand = new System.Workflow.Activities.EventDrivenActivity();
            this.edaWaitForTimeOut = new System.Workflow.Activities.EventDrivenActivity();
            this.siaUploadandSendingMail = new System.Workflow.Activities.StateInitializationActivity();
            this.siaLeaveUpload = new System.Workflow.Activities.StateInitializationActivity();
            this.saWaitForAction = new System.Workflow.Activities.StateActivity();
            this.saUploadAndSendMail = new System.Workflow.Activities.StateActivity();
            this.saCompleted = new System.Workflow.Activities.StateActivity();
            this.LeaveUpLoadWFInitialState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaComplete
            // 
            this.ssaComplete.Name = "ssaComplete";
            this.ssaComplete.TargetStateName = "saCompleted";
            // 
            // heaStopUpload
            // 
            this.heaStopUpload.EventName = "Stop";
            this.heaStopUpload.InterfaceType = typeof(V2.Orbit.Workflow.LeaveUploadWF.ILeaveUpload);
            this.heaStopUpload.Name = "heaStopUpload";
            // 
            // ssaOnDemandLeaveUpload
            // 
            this.ssaOnDemandLeaveUpload.Name = "ssaOnDemandLeaveUpload";
            this.ssaOnDemandLeaveUpload.TargetStateName = "saUploadAndSendMail";
            // 
            // heaOnDemandLeaveUpload
            // 
            this.heaOnDemandLeaveUpload.EventName = "OnDemandLeaveUpload";
            this.heaOnDemandLeaveUpload.InterfaceType = typeof(V2.Orbit.Workflow.LeaveUploadWF.ILeaveUpload);
            this.heaOnDemandLeaveUpload.Name = "heaOnDemandLeaveUpload";
            // 
            // ssaTimeOut
            // 
            this.ssaTimeOut.Name = "ssaTimeOut";
            this.ssaTimeOut.TargetStateName = "saUploadAndSendMail";
            // 
            // daTimeOut
            // 
            this.daTimeOut.Name = "daTimeOut";
            this.daTimeOut.TimeoutDuration = System.TimeSpan.Parse("05:00:00");
            // 
            // ssaUploadandSendingMail
            // 
            this.ssaUploadandSendingMail.Name = "ssaUploadandSendingMail";
            this.ssaUploadandSendingMail.TargetStateName = "saWaitForAction";
            // 
            // caUploadandSendingMail
            // 
            this.caUploadandSendingMail.Name = "caUploadandSendingMail";
            this.caUploadandSendingMail.ExecuteCode += new System.EventHandler(this.caUploadandSendingMail_ExecuteCode);
            // 
            // ssaLeaveUpload
            // 
            this.ssaLeaveUpload.Name = "ssaLeaveUpload";
            this.ssaLeaveUpload.TargetStateName = "saUploadAndSendMail";
            // 
            // edaStopUpload
            // 
            this.edaStopUpload.Activities.Add(this.heaStopUpload);
            this.edaStopUpload.Activities.Add(this.ssaComplete);
            this.edaStopUpload.Name = "edaStopUpload";
            // 
            // edaOndemand
            // 
            this.edaOndemand.Activities.Add(this.heaOnDemandLeaveUpload);
            this.edaOndemand.Activities.Add(this.ssaOnDemandLeaveUpload);
            this.edaOndemand.Name = "edaOndemand";
            // 
            // edaWaitForTimeOut
            // 
            this.edaWaitForTimeOut.Activities.Add(this.daTimeOut);
            this.edaWaitForTimeOut.Activities.Add(this.ssaTimeOut);
            this.edaWaitForTimeOut.Name = "edaWaitForTimeOut";
            // 
            // siaUploadandSendingMail
            // 
            this.siaUploadandSendingMail.Activities.Add(this.caUploadandSendingMail);
            this.siaUploadandSendingMail.Activities.Add(this.ssaUploadandSendingMail);
            this.siaUploadandSendingMail.Name = "siaUploadandSendingMail";
            // 
            // siaLeaveUpload
            // 
            this.siaLeaveUpload.Activities.Add(this.ssaLeaveUpload);
            this.siaLeaveUpload.Name = "siaLeaveUpload";
            // 
            // saWaitForAction
            // 
            this.saWaitForAction.Activities.Add(this.edaWaitForTimeOut);
            this.saWaitForAction.Activities.Add(this.edaOndemand);
            this.saWaitForAction.Activities.Add(this.edaStopUpload);
            this.saWaitForAction.Name = "saWaitForAction";
            // 
            // saUploadAndSendMail
            // 
            this.saUploadAndSendMail.Activities.Add(this.siaUploadandSendingMail);
            this.saUploadAndSendMail.Name = "saUploadAndSendMail";
            // 
            // saCompleted
            // 
            this.saCompleted.Name = "saCompleted";
            // 
            // LeaveUpLoadWFInitialState
            // 
            this.LeaveUpLoadWFInitialState.Activities.Add(this.siaLeaveUpload);
            this.LeaveUpLoadWFInitialState.Name = "LeaveUpLoadWFInitialState";
            // 
            // LeaveUpLoadWF
            // 
            this.Activities.Add(this.LeaveUpLoadWFInitialState);
            this.Activities.Add(this.saCompleted);
            this.Activities.Add(this.saUploadAndSendMail);
            this.Activities.Add(this.saWaitForAction);
            this.CompletedStateName = "saCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "LeaveUpLoadWFInitialState";
            this.Name = "LeaveUpLoadWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private StateActivity saWaitForAction;
        private StateActivity saUploadAndSendMail;
        private StateActivity saCompleted;
        private SetStateActivity ssaLeaveUpload;
        private StateInitializationActivity siaLeaveUpload;
        private SetStateActivity ssaUploadandSendingMail;
        private CodeActivity caUploadandSendingMail;
        private StateInitializationActivity siaUploadandSendingMail;
        private EventDrivenActivity edaWaitForTimeOut;
        private SetStateActivity ssaTimeOut;
        private DelayActivity daTimeOut;
        private EventDrivenActivity edaOndemand;
        private SetStateActivity ssaOnDemandLeaveUpload;
        private HandleExternalEventActivity heaOnDemandLeaveUpload;
        private SetStateActivity ssaComplete;
        private HandleExternalEventActivity heaStopUpload;
        private EventDrivenActivity edaStopUpload;
        private StateActivity LeaveUpLoadWFInitialState;











    }
}
