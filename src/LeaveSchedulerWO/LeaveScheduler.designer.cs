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

namespace V2.Orbit.Workflow.LeaveSchedulerWO
{
    partial class LeaveScheduler
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
            this.ssaToSleepFromUpdateLeaves = new System.Workflow.Activities.SetStateActivity();
            this.caCalculateLeaveBalance = new System.Workflow.Activities.CodeActivity();
            this.ssaToSleep = new System.Workflow.Activities.SetStateActivity();
            this.ssaToCompleteState = new System.Workflow.Activities.SetStateActivity();
            this.heeaStopScheduler = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaItSelfAfterOnRequest = new System.Workflow.Activities.SetStateActivity();
            this.heeaLeaveBalanceOnRequest = new System.Workflow.Activities.HandleExternalEventActivity();
            this.ssaItSelfAfterDelay = new System.Workflow.Activities.SetStateActivity();
            this.daSleepTime = new System.Workflow.Activities.DelayActivity();
            this.stateInitializationActivity2 = new System.Workflow.Activities.StateInitializationActivity();
            this.stateInitializationActivity1 = new System.Workflow.Activities.StateInitializationActivity();
            this.edaStopScheduler = new System.Workflow.Activities.EventDrivenActivity();
            this.edaHandleEvent = new System.Workflow.Activities.EventDrivenActivity();
            this.edaSleepTimer = new System.Workflow.Activities.EventDrivenActivity();
            this.saUpdateLeaves = new System.Workflow.Activities.StateActivity();
            this.CompletedState = new System.Workflow.Activities.StateActivity();
            this.InitialState = new System.Workflow.Activities.StateActivity();
            this.SleepState = new System.Workflow.Activities.StateActivity();
            // 
            // ssaToSleepFromUpdateLeaves
            // 
            this.ssaToSleepFromUpdateLeaves.Name = "ssaToSleepFromUpdateLeaves";
            this.ssaToSleepFromUpdateLeaves.TargetStateName = "SleepState";
            // 
            // caCalculateLeaveBalance
            // 
            this.caCalculateLeaveBalance.Name = "caCalculateLeaveBalance";
            this.caCalculateLeaveBalance.ExecuteCode += new System.EventHandler(this.caCalculateLeaveBalance_ExecuteCode);
            // 
            // ssaToSleep
            // 
            this.ssaToSleep.Name = "ssaToSleep";
            this.ssaToSleep.TargetStateName = "saUpdateLeaves";
            // 
            // ssaToCompleteState
            // 
            this.ssaToCompleteState.Name = "ssaToCompleteState";
            this.ssaToCompleteState.TargetStateName = "CompletedState";
            // 
            // heeaStopScheduler
            // 
            this.heeaStopScheduler.EventName = "Stop";
            this.heeaStopScheduler.InterfaceType = typeof(V2.Orbit.Workflow.LeaveSchedulerWO.ILeaveScheduler);
            this.heeaStopScheduler.Name = "heeaStopScheduler";
            // 
            // ssaItSelfAfterOnRequest
            // 
            this.ssaItSelfAfterOnRequest.Name = "ssaItSelfAfterOnRequest";
            this.ssaItSelfAfterOnRequest.TargetStateName = "saUpdateLeaves";
            // 
            // heeaLeaveBalanceOnRequest
            // 
            this.heeaLeaveBalanceOnRequest.EventName = "OnDemandLeaveBalance";
            this.heeaLeaveBalanceOnRequest.InterfaceType = typeof(V2.Orbit.Workflow.LeaveSchedulerWO.ILeaveScheduler);
            this.heeaLeaveBalanceOnRequest.Name = "heeaLeaveBalanceOnRequest";
            // 
            // ssaItSelfAfterDelay
            // 
            this.ssaItSelfAfterDelay.Name = "ssaItSelfAfterDelay";
            this.ssaItSelfAfterDelay.TargetStateName = "saUpdateLeaves";
            // 
            // daSleepTime
            // 
            this.daSleepTime.Name = "daSleepTime";
            this.daSleepTime.TimeoutDuration = System.TimeSpan.Parse("00:05:00");
            // 
            // stateInitializationActivity2
            // 
            this.stateInitializationActivity2.Activities.Add(this.caCalculateLeaveBalance);
            this.stateInitializationActivity2.Activities.Add(this.ssaToSleepFromUpdateLeaves);
            this.stateInitializationActivity2.Name = "stateInitializationActivity2";
            // 
            // stateInitializationActivity1
            // 
            this.stateInitializationActivity1.Activities.Add(this.ssaToSleep);
            this.stateInitializationActivity1.Name = "stateInitializationActivity1";
            // 
            // edaStopScheduler
            // 
            this.edaStopScheduler.Activities.Add(this.heeaStopScheduler);
            this.edaStopScheduler.Activities.Add(this.ssaToCompleteState);
            this.edaStopScheduler.Name = "edaStopScheduler";
            // 
            // edaHandleEvent
            // 
            this.edaHandleEvent.Activities.Add(this.heeaLeaveBalanceOnRequest);
            this.edaHandleEvent.Activities.Add(this.ssaItSelfAfterOnRequest);
            this.edaHandleEvent.Name = "edaHandleEvent";
            // 
            // edaSleepTimer
            // 
            this.edaSleepTimer.Activities.Add(this.daSleepTime);
            this.edaSleepTimer.Activities.Add(this.ssaItSelfAfterDelay);
            this.edaSleepTimer.Name = "edaSleepTimer";
            // 
            // saUpdateLeaves
            // 
            this.saUpdateLeaves.Activities.Add(this.stateInitializationActivity2);
            this.saUpdateLeaves.Name = "saUpdateLeaves";
            // 
            // CompletedState
            // 
            this.CompletedState.Name = "CompletedState";
            // 
            // InitialState
            // 
            this.InitialState.Activities.Add(this.stateInitializationActivity1);
            this.InitialState.Name = "InitialState";
            // 
            // SleepState
            // 
            this.SleepState.Activities.Add(this.edaSleepTimer);
            this.SleepState.Activities.Add(this.edaHandleEvent);
            this.SleepState.Activities.Add(this.edaStopScheduler);
            this.SleepState.Name = "SleepState";
            // 
            // LeaveScheduler
            // 
            this.Activities.Add(this.SleepState);
            this.Activities.Add(this.InitialState);
            this.Activities.Add(this.CompletedState);
            this.Activities.Add(this.saUpdateLeaves);
            this.CompletedStateName = "CompletedState";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "InitialState";
            this.Name = "LeaveScheduler";
            this.CanModifyActivities = false;

        }

        #endregion

        private StateInitializationActivity stateInitializationActivity1;
        private StateActivity InitialState;
        private SetStateActivity ssaToSleep;
        private DelayActivity daSleepTime;
        private EventDrivenActivity edaHandleEvent;
        private EventDrivenActivity edaSleepTimer;
        private SetStateActivity ssaItSelfAfterOnRequest;
        private HandleExternalEventActivity heeaLeaveBalanceOnRequest;
        private SetStateActivity ssaItSelfAfterDelay;
        private SetStateActivity ssaToCompleteState;
        private HandleExternalEventActivity heeaStopScheduler;
        private EventDrivenActivity edaStopScheduler;
        private StateActivity CompletedState;
        private SetStateActivity ssaToSleepFromUpdateLeaves;
        private CodeActivity caCalculateLeaveBalance;
        private StateInitializationActivity stateInitializationActivity2;
        private StateActivity saUpdateLeaves;
        private StateActivity SleepState;




































    }
}
