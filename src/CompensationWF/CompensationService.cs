using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.CompensationWF
{
    [Serializable]
	public class CompensationService:ICompensation
	{
        public event EventHandler<CompensationEventArgs> Approve;
        public event EventHandler<CompensationEventArgs> Reject;
        public event EventHandler<CompensationEventArgs> Cancel;

        public void RaiseApproveEvent(Guid instanceId)
        {
            if (Approve != null)
            {
                CompensationEventArgs e = new CompensationEventArgs(instanceId);
                Approve(this, e);
            }
        }
        public void RaiseRejectEvent(Guid instanceId)
        {
            if (Reject != null)
            {
                CompensationEventArgs e = new CompensationEventArgs(instanceId);
                Reject(this, e);
            }
        }
        public void RaiseCancelEvent(Guid instanceId)
        {
            if (Cancel != null)
            {
                CompensationEventArgs e = new CompensationEventArgs(instanceId);
                Cancel(this, e);
            }
        }


	}
}


