using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;

namespace V2.Orbit.Workflow.LeaveDetailsWF
{
    [ExternalDataExchange]
	public interface  ILeaveDetails
	{
         event EventHandler<LeaveDetailsEventArgs> Approve;
        event EventHandler<LeaveDetailsEventArgs> Reject;
        event EventHandler<LeaveDetailsEventArgs> Cancel;

	}

    [Serializable]
    public class LeaveDetailsEventArgs : ExternalDataEventArgs
    {
       

        public LeaveDetailsEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}
