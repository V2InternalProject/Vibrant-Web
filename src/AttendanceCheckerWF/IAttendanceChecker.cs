using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
namespace V2.Orbit.Workflow.AttendanceCheckerWF
{
	[ExternalDataExchange]
	public interface IAttendanceChecker
	{
         event EventHandler<AttendanceCheckEventArgs> Stop;
         event EventHandler<AttendanceCheckEventArgs> OnDemandAttendanceCheck;
	}
    [Serializable]
    public class AttendanceCheckEventArgs : ExternalDataEventArgs
    {
       

        public AttendanceCheckEventArgs(Guid instanceId) : base(instanceId)
        {
          
        }

        
    }
}



