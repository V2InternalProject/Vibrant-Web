using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Workflow.AttendanceCheckerWF
{
	[Serializable]
	public class AttendanceCheckerService: IAttendanceChecker
	{
        public event EventHandler<AttendanceCheckEventArgs> Stop;
        public event EventHandler<AttendanceCheckEventArgs> OnDemandAttendanceCheck;
       
        public void RaiseStopEvent(Guid instanceId)
        {
            if (Stop != null)
            {
                AttendanceCheckEventArgs e = new AttendanceCheckEventArgs(instanceId);
                Stop(this, e);
            }
        }
        public void RaiseOnDemandAttendanceCheckEvent(Guid instanceId)
        {
            if (OnDemandAttendanceCheck != null)
            {
                AttendanceCheckEventArgs e = new AttendanceCheckEventArgs(instanceId);
                OnDemandAttendanceCheck(this, e);
            }
        }
	}
}




