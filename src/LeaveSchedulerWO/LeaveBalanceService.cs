using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;

namespace  V2.Orbit.Workflow.LeaveSchedulerWO
{
    [Serializable]
    public class LeaveBalanceService:ILeaveScheduler
    {
       public event EventHandler<LeaveBalanceEventArgs> Stop;
      
       public event EventHandler<LeaveBalanceEventArgs> OnDemandLeaveBalance;

        public void RaiseStopEvent(Guid instanceId)
        {
            if (Stop != null)
            {
                LeaveBalanceEventArgs e = new LeaveBalanceEventArgs(instanceId);
                Stop(this, e);
            }
        }
       
        public void RaiseOnDemandLeaveBalanceEvent(Guid instanceId)
        {
            if (OnDemandLeaveBalance != null)
            {
                LeaveBalanceEventArgs e = new LeaveBalanceEventArgs(instanceId);
                OnDemandLeaveBalance(this, e);
            }
        }
        
    }
}
