using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.SignInSignOutWF
{
    [Serializable]
	public class SignInSignOutService:ISignInSignOut
	{
        public event EventHandler<SignInSignOutEventArgs> Approve;
        public event EventHandler<SignInSignOutEventArgs> Reject;
        public event EventHandler<SignInSignOutEventArgs> Cancel;

        public void RaiseApproveEvent(Guid instanceId)
        {
            if (Approve != null)
            {
                SignInSignOutEventArgs e = new SignInSignOutEventArgs(instanceId);
                Approve(this, e);
            }
        }
        public void RaiseRejectEvent(Guid instanceId)
        {
            if (Reject != null)
            {
                SignInSignOutEventArgs e = new SignInSignOutEventArgs(instanceId);
                Reject(this, e);
            }
        }
        public void RaiseCancelEvent(Guid instanceId)
        {
            if (Cancel != null)
            {
                SignInSignOutEventArgs e = new SignInSignOutEventArgs(instanceId);
                Cancel(this, e);
            }
        }

	}


}




