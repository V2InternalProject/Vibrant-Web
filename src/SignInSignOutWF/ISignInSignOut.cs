using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;

namespace V2.Orbit.Workflow.SignInSignOutWF
{
    [ExternalDataExchange]
	public interface ISignInSignOut
	{
        event EventHandler<SignInSignOutEventArgs> Approve;
        event EventHandler<SignInSignOutEventArgs> Reject;
        event EventHandler<SignInSignOutEventArgs> Cancel;

	}
    [Serializable]
    public class SignInSignOutEventArgs : ExternalDataEventArgs
    {
       

        public SignInSignOutEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }

}
