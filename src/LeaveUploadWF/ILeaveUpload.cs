using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
namespace V2.Orbit.Workflow.LeaveUploadWF
{
	[ExternalDataExchange]
	public interface ILeaveUpload
	{
         event EventHandler<LeaveUploadEventArgs> Stop;
         event EventHandler<LeaveUploadEventArgs> OnDemandLeaveUpload;
	}
    [Serializable]
    public class LeaveUploadEventArgs : ExternalDataEventArgs
    {
       

        public LeaveUploadEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}

