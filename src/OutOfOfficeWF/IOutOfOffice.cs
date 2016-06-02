using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;

namespace V2.Orbit.Workflow.OutOfOfficeWF
{
     [ExternalDataExchange]
	public interface IOutOfOffice
	{
         event EventHandler<OutOfOfficeEventArgs> Approve;
        event EventHandler<OutOfOfficeEventArgs> Reject;
        event EventHandler<OutOfOfficeEventArgs> Cancel;
	}

    [Serializable]
    public class OutOfOfficeEventArgs : ExternalDataEventArgs
    {
       

        public OutOfOfficeEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}
