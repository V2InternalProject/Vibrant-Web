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

namespace V2.Orbit.Workflow.LeaveAbsentCheckWF
{
    partial class LeaveAbsentCheckWF
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
            this.ssaStopAbsentLeave = new System.Workflow.Activities.SetStateActivity();
            this.heaStopAbsentLeave = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaOnDemandAbsentLeave = new System.Workflow.Activities.SetStateActivity();
            this.heaOnDemandAbsentLeave = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaOnTimeAbsentLeave = new System.Workflow.Activities.SetStateActivity();
            this.daOnTimeAbsentLeave = new System.Workflow.Activities.DelayActivity();
            this.ssaCheckandSendMailToEvents = new System.Workflow.Activities.SetStateActivity();
            this.caCheckandSendMail = new System.Workflow.Activities.CodeActivity();
            this.ssaInitial = new System.Workflow.Activities.SetStateActivity();
            this.edaStopAbsentLeave = new System.Workflow.Activities.EventDrivenActivity();
            this.edaOnDemandAbsentLeave = new System.Workflow.Activities.EventDrivenActivity();
            this.edaOnTimeAbsentLeave = new System.Workflow.Activities.EventDrivenActivity();
            this.siaCheckAbsentLeaveSendMail = new System.Workflow.Activities.StateInitializationActivity();
            this.siaALCheck = new System.Workflow.Activities.StateInitializationActivity();
            this.saCheckAttendanceLeaveEvents = new System.Workflow.Activities.StateActivity();
            this.saCompleted = new System.Workflow.Activities.StateActivity();
            this.saCheckAbsentLeaveSendMail = new System.Workflow.Activities.StateActivity();
            this.LeaveAbsentCheckWFInitialState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaStopAbsentLeave
            // 
            this.ssaStopAbsentLeave.Name = "ssaStopAbsentLeave";
            this.ssaStopAbsentLeave.TargetStateName = "saCompleted";
            // 
            // heaStopAbsentLeave
            // 
            this.heaStopAbsentLeave.EventName = "Stop";
            this.heaStopAbsentLeave.InterfaceType = typeof(V2.Orbit.Workflow.LeaveAbsentCheckWF.ILeaveAbsentCheck);
            this.heaStopAbsentLeave.Name = "heaStopAbsentLeave";
            // 
            // ssaOnDemandAbsentLeave
            // 
            this.ssaOnDemandAbsentLeave.Name = "ssaOnDemandAbsentLeave";
            this.ssaOnDemandAbsentLeave.TargetStateName = "saCheckAbsentLeaveSendMail";
            // 
            // heaOnDemandAbsentLeave
            // 
            this.heaOnDemandAbsentLeave.EventName = "OnDemand";
            this.heaOnDemandAbsentLeave.InterfaceType = typeof(V2.Orbit.Workflow.LeaveAbsentCheckWF.ILeaveAbsentCheck);
            this.heaOnDemandAbsentLeave.Name = "heaOnDemandAbsentLeave";
            // 
            // ssaOnTimeAbsentLeave
            // 
            this.ssaOnTimeAbsentLeave.Name = "ssaOnTimeAbsentLeave";
            this.ssaOnTimeAbsentLeave.TargetStateName = "saCheckAbsentLeaveSendMail";
            // 
            // daOnTimeAbsentLeave
            // 
            this.daOnTimeAbsentLeave.Name = "daOnTimeAbsentLeave";
            this.daOnTimeAbsentLeave.TimeoutDuration = System.TimeSpan.Parse("00:00:00");
            // 
            // ssaCheckandSendMailToEvents
            // 
            this.ssaCheckandSendMailToEvents.Name = "ssaCheckandSendMailToEvents";
            this.ssaCheckandSendMailToEvents.TargetStateName = "saCheckAttendanceLeaveEvents";
            // 
            // caCheckandSendMail
            // 
            this.caCheckandSendMail.Name = "caCheckandSendMail";
            this.caCheckandSendMail.ExecuteCode += new System.EventHandler(this.caCheckandSendMail_ExecuteCode);
            // 
            // ssaInitial
            // 
            this.ssaInitial.Name = "ssaInitial";
            this.ssaInitial.TargetStateName = "saCheckAbsentLeaveSendMail";
            // 
            // edaStopAbsentLeave
            // 
            this.edaStopAbsentLeave.Activities.Add(this.heaStopAbsentLeave);
            this.edaStopAbsentLeave.Activities.Add(this.ssaStopAbsentLeave);
            this.edaStopAbsentLeave.Name = "edaStopAbsentLeave";
            // 
            // edaOnDemandAbsentLeave
            // 
            this.edaOnDemandAbsentLeave.Activities.Add(this.heaOnDemandAbsentLeave);
            this.edaOnDemandAbsentLeave.Activities.Add(this.ssaOnDemandAbsentLeave);
            this.edaOnDemandAbsentLeave.Name = "edaOnDemandAbsentLeave";
            // 
            // edaOnTimeAbsentLeave
            // 
            this.edaOnTimeAbsentLeave.Activities.Add(this.daOnTimeAbsentLeave);
            this.edaOnTimeAbsentLeave.Activities.Add(this.ssaOnTimeAbsentLeave);
            this.edaOnTimeAbsentLeave.Name = "edaOnTimeAbsentLeave";
            // 
            // siaCheckAbsentLeaveSendMail
            // 
            this.siaCheckAbsentLeaveSendMail.Activities.Add(this.caCheckandSendMail);
            this.siaCheckAbsentLeaveSendMail.Activities.Add(this.ssaCheckandSendMailToEvents);
            this.siaCheckAbsentLeaveSendMail.Name = "siaCheckAbsentLeaveSendMail";
            // 
            // siaALCheck
            // 
            this.siaALCheck.Activities.Add(this.ssaInitial);
            this.siaALCheck.Name = "siaALCheck";
            // 
            // saCheckAttendanceLeaveEvents
            // 
            this.saCheckAttendanceLeaveEvents.Activities.Add(this.edaOnTimeAbsentLeave);
            this.saCheckAttendanceLeaveEvents.Activities.Add(this.edaOnDemandAbsentLeave);
            this.saCheckAttendanceLeaveEvents.Activities.Add(this.edaStopAbsentLeave);
            this.saCheckAttendanceLeaveEvents.Name = "saCheckAttendanceLeaveEvents";
            // 
            // saCompleted
            // 
            this.saCompleted.Name = "saCompleted";
            // 
            // saCheckAbsentLeaveSendMail
            // 
            this.saCheckAbsentLeaveSendMail.Activities.Add(this.siaCheckAbsentLeaveSendMail);
            this.saCheckAbsentLeaveSendMail.Name = "saCheckAbsentLeaveSendMail";
            // 
            // LeaveAbsentCheckWFInitialState
            // 
            this.LeaveAbsentCheckWFInitialState.Activities.Add(this.siaALCheck);
            this.LeaveAbsentCheckWFInitialState.Name = "LeaveAbsentCheckWFInitialState";
            // 
            // LeaveAbsentCheckWF
            // 
            this.Activities.Add(this.LeaveAbsentCheckWFInitialState);
            this.Activities.Add(this.saCheckAbsentLeaveSendMail);
            this.Activities.Add(this.saCompleted);
            this.Activities.Add(this.saCheckAttendanceLeaveEvents);
            this.CompletedStateName = "saCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "LeaveAbsentCheckWFInitialState";
            this.Name = "LeaveAbsentCheckWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private StateInitializationActivity siaALCheck;
        private SetStateActivity ssaInitial;
        private StateActivity saCheckAttendanceLeaveEvents;
        private StateActivity saCompleted;
        private StateActivity saCheckAbsentLeaveSendMail;
        private SetStateActivity ssaCheckandSendMailToEvents;
        private CodeActivity caCheckandSendMail;
        private StateInitializationActivity siaCheckAbsentLeaveSendMail;
        private SetStateActivity ssaStopAbsentLeave;
        private HandleExternalEventActivity heaStopAbsentLeave;
        private SetStateActivity ssaOnDemandAbsentLeave;
        private HandleExternalEventActivity heaOnDemandAbsentLeave;
        private SetStateActivity ssaOnTimeAbsentLeave;
        private DelayActivity daOnTimeAbsentLeave;
        private EventDrivenActivity edaStopAbsentLeave;
        private EventDrivenActivity edaOnDemandAbsentLeave;
        private EventDrivenActivity edaOnTimeAbsentLeave;
        private StateActivity LeaveAbsentCheckWFInitialState;











    }
}
