using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using V2.Orbit.BusinessLayer;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace V2.Orbit.Workflow.LeaveSchedulerWO
{
    public sealed partial class LeaveScheduler : StateMachineWorkflowActivity
    {
      
        public LeaveScheduler()
        {
            Debug.WriteLine("Workflow Initialized");
            InitializeComponent();
            
        }

        public void updateLeavesInEmployeeMaster()
        {
            try
            {
                 LeaveDetailsBOL objLeaveDeatilsBOL; 
                 objLeaveDeatilsBOL=new LeaveDetailsBOL();
                objLeaveDeatilsBOL.wfUpdateLeaveBalance();           
              
            }             
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveScheduler", "updateLeavesInEmployeeMaster", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
      

        private void caCalculateLeaveBalance_ExecuteCode(object sender, EventArgs e)
        {

            try
            {
                 
                updateLeavesInEmployeeMaster();

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveScheduler", "caCalculateLeaveBalance_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        }




        #region Commeted

        //  public void wfUpdateLeaveBalance()
        //{           
        //    try
        //    {
        //        Debug.WriteLine("wfUpdateLeaveBalance");
               

        //        SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,CommandType.StoredProcedure, "GetLeaveBalanceProc");
        //         Debug.WriteLine("wfUpdateLeaveBalanceCompleted");
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveScheduler", "GetLeaveDetails", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
        #endregion
    }
}
