using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;


namespace V2.Orbit.Workflow.LeaveAbsentCheckWF
{
    [ExternalDataExchange]
	public interface ILeaveAbsentCheck
	{
        event EventHandler<LeaveAbsentCheckEventArgs> OnDemand;
        event EventHandler<LeaveAbsentCheckEventArgs> Stop;
        
	}
    [Serializable]
    public class LeaveAbsentCheckEventArgs : ExternalDataEventArgs
    {
       

        public LeaveAbsentCheckEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}
