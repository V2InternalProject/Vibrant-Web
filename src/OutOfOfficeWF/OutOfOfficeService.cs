using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.OutOfOfficeWF
{
    [Serializable]
	public class OutOfOfficeService:IOutOfOffice
	{

        public event EventHandler<OutOfOfficeEventArgs> Approve;
        public event EventHandler<OutOfOfficeEventArgs> Reject;
        public event EventHandler<OutOfOfficeEventArgs> Cancel;

        public void RaiseApproveEvent(Guid instanceId)
        {
            if (Approve != null)
            {
                OutOfOfficeEventArgs e = new OutOfOfficeEventArgs(instanceId);
                Approve(this, e);
            }
        }
        public void RaiseRejectEvent(Guid instanceId)
        {
            if (Reject != null)
            {
                OutOfOfficeEventArgs e = new OutOfOfficeEventArgs(instanceId);
                Reject(this, e);
            }
        }
        public void RaiseCancelEvent(Guid instanceId)
        {
            if (Cancel != null)
            {
                OutOfOfficeEventArgs e = new OutOfOfficeEventArgs(instanceId);
                Cancel(this, e);
            }
        }
	}
}
