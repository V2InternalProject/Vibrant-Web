using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.LeaveAbsentCheckWF
{
    [Serializable]
	public class LeaveAbsentCheckService: ILeaveAbsentCheck
	{
        public event EventHandler<LeaveAbsentCheckEventArgs> OnDemand;
        public event EventHandler<LeaveAbsentCheckEventArgs> Stop;
        

        public void RaiseOnDemandEvent(Guid instanceId)
        {
            if (OnDemand != null)
            {
                LeaveAbsentCheckEventArgs e = new LeaveAbsentCheckEventArgs(instanceId);
                OnDemand(this, e);
            }
        }
        public void RaiseStopEvent(Guid instanceId)
        {
            if (Stop != null)
            {
                LeaveAbsentCheckEventArgs e = new LeaveAbsentCheckEventArgs(instanceId);
                Stop(this, e);
            }
        }
        
	}
}



