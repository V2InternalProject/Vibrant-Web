using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;

namespace V2.Orbit.Workflow.CompensationWF
{
     [ExternalDataExchange]
	public interface ICompensation
	{
        event EventHandler<CompensationEventArgs> Approve;
        event EventHandler<CompensationEventArgs> Reject;
        event EventHandler<CompensationEventArgs> Cancel;
	}
    [Serializable]
    public class CompensationEventArgs : ExternalDataEventArgs
    {
       

        public CompensationEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}
