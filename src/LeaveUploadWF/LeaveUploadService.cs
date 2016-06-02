using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.LeaveUploadWF
{
	[Serializable]
	public class LeaveUploadService: ILeaveUpload
	{
        public event EventHandler<LeaveUploadEventArgs> Stop;
        public event EventHandler<LeaveUploadEventArgs> OnDemandLeaveUpload;
       
        public void RaiseStopEvent(Guid instanceId)
        {
            if (Stop != null)
            {
                LeaveUploadEventArgs e = new LeaveUploadEventArgs(instanceId);
                Stop(this, e);
            }
        }
        public void RaiseOnDemandAttendanceCheckEvent(Guid instanceId)
        {
            if (OnDemandLeaveUpload != null)
            {
                LeaveUploadEventArgs e = new LeaveUploadEventArgs(instanceId);
                OnDemandLeaveUpload(this, e);
            }
        }
	}
}

