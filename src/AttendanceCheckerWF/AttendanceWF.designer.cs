using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace AttendanceCheckerWF
{
    partial class AttendanceWF
    {
        #region Designer generated code
        
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            this.eaFHCheckAttendence = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caFHCheckAttendence = new System.Workflow.Activities.CodeActivity();
            this.eafhOnStop = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.cafhOnStop = new System.Workflow.Activities.CodeActivity();
            this.eafhOnDemand = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.cafhOnDemand = new System.Workflow.Activities.CodeActivity();
            this.eafhDelay = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.cafhDelay = new System.Workflow.Activities.CodeActivity();
            this.eaFHAttendance = new V2.Orbit.Workflow.Activities.MailActivity.EmailActivity();
            this.caSetValuestoSendMailtoPMO = new System.Workflow.Activities.CodeActivity();
            this.fhCheckAttendence = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.fhOnStop = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.fhOnDemand = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.fhDelay = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.fhException = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity2 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ssaCheckAttendenceToWait = new System.Workflow.Activities.SetStateActivity();
            this.caCheckAttendance = new System.Workflow.Activities.CodeActivity();
            this.faultHandlersActivity5 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ssaToCompleted = new System.Workflow.Activities.SetStateActivity();
            this.heaStopAttendanceChecker = new System.Workflow.Activities.HandleExternalEventActivity();
            this.faultHandlersActivity4 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.ssaGoToSleepfromhea = new System.Workflow.Activities.SetStateActivity();
            this.heaOnDemandAttendanceCheck = new System.Workflow.Activities.HandleExternalEventActivity();
            this.faultHandlersActivity3 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ssaGotoSleepFromChecker = new System.Workflow.Activities.SetStateActivity();
            this.daCheckAttendance = new System.Workflow.Activities.DelayActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ssaGotoCheckAttendence = new System.Workflow.Activities.SetStateActivity();
            this.siaAttendenceSendMail = new System.Workflow.Activities.StateInitializationActivity();
            this.edaOnStop = new System.Workflow.Activities.EventDrivenActivity();
            this.edaOnDemand = new System.Workflow.Activities.EventDrivenActivity();
            this.edaDelay = new System.Workflow.Activities.EventDrivenActivity();
            this.siaAttendance = new System.Workflow.Activities.StateInitializationActivity();
            this.saCheckAttendenceSendMail = new System.Workflow.Activities.StateActivity();
            this.saCheckAttendance = new System.Workflow.Activities.StateActivity();
            this.csaCompleted = new System.Workflow.Activities.StateActivity();
            this.isaAttendance = new System.Workflow.Activities.StateActivity();
            // 
            // eaFHCheckAttendence
            // 
            activitybind1.Name = "AttendanceWF";
            activitybind1.Path = "Body";
            activitybind2.Name = "AttendanceWF";
            activitybind2.Path = "From";
            this.eaFHCheckAttendence.Name = "eaFHCheckAttendence";
            activitybind3.Name = "AttendanceWF";
            activitybind3.Path = "SMTPServer";
            activitybind4.Name = "AttendanceWF";
            activitybind4.Path = "Subject";
            activitybind5.Name = "AttendanceWF";
            activitybind5.Path = "To";
            this.eaFHCheckAttendence.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.eaFHCheckAttendence.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.eaFHCheckAttendence.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.eaFHCheckAttendence.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.eaFHCheckAttendence.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // caFHCheckAttendence
            // 
            this.caFHCheckAttendence.Name = "caFHCheckAttendence";
            this.caFHCheckAttendence.ExecuteCode += new System.EventHandler(this.caFHCheckAttendence_ExecuteCode);
            // 
            // eafhOnStop
            // 
            activitybind6.Name = "AttendanceWF";
            activitybind6.Path = "Body";
            activitybind7.Name = "AttendanceWF";
            activitybind7.Path = "From";
            this.eafhOnStop.Name = "eafhOnStop";
            activitybind8.Name = "AttendanceWF";
            activitybind8.Path = "SMTPServer";
            activitybind9.Name = "AttendanceWF";
            activitybind9.Path = "Subject";
            activitybind10.Name = "AttendanceWF";
            activitybind10.Path = "To";
            this.eafhOnStop.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.eafhOnStop.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.eafhOnStop.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.eafhOnStop.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.eafhOnStop.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // cafhOnStop
            // 
            this.cafhOnStop.Name = "cafhOnStop";
            this.cafhOnStop.ExecuteCode += new System.EventHandler(this.cafhOnStop_ExecuteCode);
            // 
            // eafhOnDemand
            // 
            activitybind11.Name = "AttendanceWF";
            activitybind11.Path = "Body";
            activitybind12.Name = "AttendanceWF";
            activitybind12.Path = "From";
            this.eafhOnDemand.Name = "eafhOnDemand";
            activitybind13.Name = "AttendanceWF";
            activitybind13.Path = "SMTPServer";
            activitybind14.Name = "AttendanceWF";
            activitybind14.Path = "Subject";
            activitybind15.Name = "AttendanceWF";
            activitybind15.Path = "To";
            this.eafhOnDemand.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.eafhOnDemand.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.eafhOnDemand.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.eafhOnDemand.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.eafhOnDemand.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // cafhOnDemand
            // 
            this.cafhOnDemand.Name = "cafhOnDemand";
            this.cafhOnDemand.ExecuteCode += new System.EventHandler(this.cafhOnDemand_ExecuteCode);
            // 
            // eafhDelay
            // 
            this.eafhDelay.Body = null;
            this.eafhDelay.From = null;
            this.eafhDelay.Name = "eafhDelay";
            this.eafhDelay.SMTPServer = null;
            this.eafhDelay.Subject = null;
            this.eafhDelay.To = null;
            // 
            // cafhDelay
            // 
            this.cafhDelay.Name = "cafhDelay";
            this.cafhDelay.ExecuteCode += new System.EventHandler(this.cafhDelay_ExecuteCode);
            // 
            // eaFHAttendance
            // 
            activitybind16.Name = "AttendanceWF";
            activitybind16.Path = "Body";
            activitybind17.Name = "AttendanceWF";
            activitybind17.Path = "From";
            this.eaFHAttendance.Name = "eaFHAttendance";
            activitybind18.Name = "AttendanceWF";
            activitybind18.Path = "SMTPServer";
            activitybind19.Name = "AttendanceWF";
            activitybind19.Path = "Subject";
            activitybind20.Name = "AttendanceWF";
            activitybind20.Path = "To";
            this.eaFHAttendance.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.eaFHAttendance.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.eaFHAttendance.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SMTPServerProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            this.eaFHAttendance.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.eaFHAttendance.SetBinding(V2.Orbit.Workflow.Activities.MailActivity.EmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            // 
            // caSetValuestoSendMailtoPMO
            // 
            this.caSetValuestoSendMailtoPMO.Name = "caSetValuestoSendMailtoPMO";
            this.caSetValuestoSendMailtoPMO.ExecuteCode += new System.EventHandler(this.caFHSendMailFromIsaAttendance);
            // 
            // fhCheckAttendence
            // 
            this.fhCheckAttendence.Activities.Add(this.caFHCheckAttendence);
            this.fhCheckAttendence.Activities.Add(this.eaFHCheckAttendence);
            this.fhCheckAttendence.FaultType = typeof(System.Exception);
            this.fhCheckAttendence.Name = "fhCheckAttendence";
            // 
            // fhOnStop
            // 
            this.fhOnStop.Activities.Add(this.cafhOnStop);
            this.fhOnStop.Activities.Add(this.eafhOnStop);
            this.fhOnStop.FaultType = typeof(System.Exception);
            this.fhOnStop.Name = "fhOnStop";
            // 
            // fhOnDemand
            // 
            this.fhOnDemand.Activities.Add(this.cafhOnDemand);
            this.fhOnDemand.Activities.Add(this.eafhOnDemand);
            this.fhOnDemand.FaultType = typeof(System.Exception);
            this.fhOnDemand.Name = "fhOnDemand";
            // 
            // fhDelay
            // 
            this.fhDelay.Activities.Add(this.cafhDelay);
            this.fhDelay.Activities.Add(this.eafhDelay);
            this.fhDelay.FaultType = typeof(System.Exception);
            this.fhDelay.Name = "fhDelay";
            // 
            // fhException
            // 
            this.fhException.Activities.Add(this.caSetValuestoSendMailtoPMO);
            this.fhException.Activities.Add(this.eaFHAttendance);
            this.fhException.FaultType = typeof(System.Exception);
            this.fhException.Name = "fhException";
            // 
            // faultHandlersActivity2
            // 
            this.faultHandlersActivity2.Activities.Add(this.fhCheckAttendence);
            this.faultHandlersActivity2.Name = "faultHandlersActivity2";
            // 
            // ssaCheckAttendenceToWait
            // 
            this.ssaCheckAttendenceToWait.Name = "ssaCheckAttendenceToWait";
            this.ssaCheckAttendenceToWait.TargetStateName = "saCheckAttendance";
            // 
            // caCheckAttendance
            // 
            this.caCheckAttendance.Name = "caCheckAttendance";
            this.caCheckAttendance.ExecuteCode += new System.EventHandler(this.caCheckAttendance_ExecuteCode);
            // 
            // faultHandlersActivity5
            // 
            this.faultHandlersActivity5.Activities.Add(this.fhOnStop);
            this.faultHandlersActivity5.Name = "faultHandlersActivity5";
            // 
            // ssaToCompleted
            // 
            this.ssaToCompleted.Name = "ssaToCompleted";
            this.ssaToCompleted.TargetStateName = "csaCompleted";
            // 
            // heaStopAttendanceChecker
            // 
            this.heaStopAttendanceChecker.EventName = "Stop";
            this.heaStopAttendanceChecker.InterfaceType = typeof(V2.Orbit.Workflow.AttendanceCheckerWF.IAttendanceChecker);
            this.heaStopAttendanceChecker.Name = "heaStopAttendanceChecker";
            // 
            // faultHandlersActivity4
            // 
            this.faultHandlersActivity4.Activities.Add(this.fhOnDemand);
            this.faultHandlersActivity4.Name = "faultHandlersActivity4";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // ssaGoToSleepfromhea
            // 
            this.ssaGoToSleepfromhea.Name = "ssaGoToSleepfromhea";
            this.ssaGoToSleepfromhea.TargetStateName = "saCheckAttendenceSendMail";
            // 
            // heaOnDemandAttendanceCheck
            // 
            this.heaOnDemandAttendanceCheck.EventName = "OnDemandAttendanceCheck";
            this.heaOnDemandAttendanceCheck.InterfaceType = typeof(V2.Orbit.Workflow.AttendanceCheckerWF.IAttendanceChecker);
            this.heaOnDemandAttendanceCheck.Name = "heaOnDemandAttendanceCheck";
            // 
            // faultHandlersActivity3
            // 
            this.faultHandlersActivity3.Activities.Add(this.fhDelay);
            this.faultHandlersActivity3.Name = "faultHandlersActivity3";
            // 
            // ssaGotoSleepFromChecker
            // 
            this.ssaGotoSleepFromChecker.Name = "ssaGotoSleepFromChecker";
            this.ssaGotoSleepFromChecker.TargetStateName = "saCheckAttendenceSendMail";
            // 
            // daCheckAttendance
            // 
            this.daCheckAttendance.Name = "daCheckAttendance";
            this.daCheckAttendance.TimeoutDuration = System.TimeSpan.Parse("05:00:00");
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.fhException);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // ssaGotoCheckAttendence
            // 
            this.ssaGotoCheckAttendence.Name = "ssaGotoCheckAttendence";
            this.ssaGotoCheckAttendence.TargetStateName = "saCheckAttendenceSendMail";
            // 
            // siaAttendenceSendMail
            // 
            this.siaAttendenceSendMail.Activities.Add(this.caCheckAttendance);
            this.siaAttendenceSendMail.Activities.Add(this.ssaCheckAttendenceToWait);
            this.siaAttendenceSendMail.Activities.Add(this.faultHandlersActivity2);
            this.siaAttendenceSendMail.Name = "siaAttendenceSendMail";
            // 
            // edaOnStop
            // 
            this.edaOnStop.Activities.Add(this.heaStopAttendanceChecker);
            this.edaOnStop.Activities.Add(this.ssaToCompleted);
            this.edaOnStop.Activities.Add(this.faultHandlersActivity5);
            this.edaOnStop.Name = "edaOnStop";
            // 
            // edaOnDemand
            // 
            this.edaOnDemand.Activities.Add(this.heaOnDemandAttendanceCheck);
            this.edaOnDemand.Activities.Add(this.ssaGoToSleepfromhea);
            this.edaOnDemand.Activities.Add(this.cancellationHandlerActivity1);
            this.edaOnDemand.Activities.Add(this.faultHandlersActivity4);
            this.edaOnDemand.Name = "edaOnDemand";
            // 
            // edaDelay
            // 
            this.edaDelay.Activities.Add(this.daCheckAttendance);
            this.edaDelay.Activities.Add(this.ssaGotoSleepFromChecker);
            this.edaDelay.Activities.Add(this.faultHandlersActivity3);
            this.edaDelay.Name = "edaDelay";
            // 
            // siaAttendance
            // 
            this.siaAttendance.Activities.Add(this.ssaGotoCheckAttendence);
            this.siaAttendance.Activities.Add(this.faultHandlersActivity1);
            this.siaAttendance.Name = "siaAttendance";
            // 
            // saCheckAttendenceSendMail
            // 
            this.saCheckAttendenceSendMail.Activities.Add(this.siaAttendenceSendMail);
            this.saCheckAttendenceSendMail.Name = "saCheckAttendenceSendMail";
            // 
            // saCheckAttendance
            // 
            this.saCheckAttendance.Activities.Add(this.edaDelay);
            this.saCheckAttendance.Activities.Add(this.edaOnDemand);
            this.saCheckAttendance.Activities.Add(this.edaOnStop);
            this.saCheckAttendance.Name = "saCheckAttendance";
            // 
            // csaCompleted
            // 
            this.csaCompleted.Name = "csaCompleted";
            // 
            // isaAttendance
            // 
            this.isaAttendance.Activities.Add(this.siaAttendance);
            this.isaAttendance.Name = "isaAttendance";
            // 
            // AttendanceWF
            // 
            this.Activities.Add(this.isaAttendance);
            this.Activities.Add(this.csaCompleted);
            this.Activities.Add(this.saCheckAttendance);
            this.Activities.Add(this.saCheckAttendenceSendMail);
            this.CompletedStateName = "csaCompleted";
            this.DynamicUpdateCondition = null;
            this.InitialStateName = "isaAttendance";
            this.Name = "AttendanceWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaFHCheckAttendence;
        private CodeActivity caFHCheckAttendence;
        private FaultHandlerActivity fhCheckAttendence;
        private FaultHandlersActivity faultHandlersActivity2;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eafhDelay;
        private CodeActivity cafhDelay;
        private FaultHandlerActivity fhDelay;
        private FaultHandlersActivity faultHandlersActivity3;
        private FaultHandlersActivity faultHandlersActivity4;
        private CancellationHandlerActivity cancellationHandlerActivity1;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eafhOnDemand;
        private CodeActivity cafhOnDemand;
        private FaultHandlerActivity fhOnDemand;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eafhOnStop;
        private CodeActivity cafhOnStop;
        private FaultHandlerActivity fhOnStop;
        private FaultHandlersActivity faultHandlersActivity5;
        private StateInitializationActivity siaAttendance;
        private StateActivity saCheckAttendance;
        private StateActivity csaCompleted;
        private SetStateActivity ssaGotoCheckAttendence;
        private SetStateActivity ssaGotoSleepFromChecker;
        private DelayActivity daCheckAttendance;
        private EventDrivenActivity edaDelay;
        private SetStateActivity ssaGoToSleepfromhea;
        private HandleExternalEventActivity heaOnDemandAttendanceCheck;
        private EventDrivenActivity edaOnDemand;
        private SetStateActivity ssaToCompleted;
        private HandleExternalEventActivity heaStopAttendanceChecker;
        private EventDrivenActivity edaOnStop;
        private StateActivity saCheckAttendenceSendMail;
        private SetStateActivity ssaCheckAttendenceToWait;
        private CodeActivity caCheckAttendance;
        private StateInitializationActivity siaAttendenceSendMail;
        private FaultHandlersActivity faultHandlersActivity1;
        private CodeActivity caSetValuestoSendMailtoPMO;
        private FaultHandlerActivity fhException;
        private V2.Orbit.Workflow.Activities.MailActivity.EmailActivity eaFHAttendance;
        private StateActivity isaAttendance;






















































    }
}
