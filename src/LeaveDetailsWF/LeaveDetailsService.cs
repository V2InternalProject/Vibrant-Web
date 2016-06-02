using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.LeaveDetailsWF
{
    [Serializable]
	public class LeaveDetailsService:ILeaveDetails
	{
        public event EventHandler<LeaveDetailsEventArgs> Approve;
        public event EventHandler<LeaveDetailsEventArgs> Reject;
        public event EventHandler<LeaveDetailsEventArgs> Cancel;

        public void RaiseApproveEvent(Guid instanceId)
        {
            if (Approve != null)
            {
                LeaveDetailsEventArgs e = new LeaveDetailsEventArgs(instanceId);
                Approve(this, e);
            }
        }
        public void RaiseRejectEvent(Guid instanceId)
        {
            if (Reject != null)
            {
                LeaveDetailsEventArgs e = new LeaveDetailsEventArgs(instanceId);
                Reject(this, e);
            }
        }
        public void RaiseCancelEvent(Guid instanceId)
        {
            if (Cancel != null)
            {
                LeaveDetailsEventArgs e = new LeaveDetailsEventArgs(instanceId);
                Cancel(this, e);
            }
        }
       

	}
}

