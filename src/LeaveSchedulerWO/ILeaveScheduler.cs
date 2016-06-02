using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;

namespace V2.Orbit.Workflow.LeaveSchedulerWO
{
    [ExternalDataExchange]
	public interface ILeaveScheduler
	{
       event EventHandler<LeaveBalanceEventArgs> Stop;
     
       event EventHandler<LeaveBalanceEventArgs> OnDemandLeaveBalance;

	}
    [Serializable]
    public class LeaveBalanceEventArgs : ExternalDataEventArgs
    {
       

        public LeaveBalanceEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }

}
