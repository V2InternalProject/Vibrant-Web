using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using V2.Helpdesk.Model;
using V2.Helpdesk.BusinessLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.web.admin
{
	/// <summary>
	/// Summary description for SummaryEmployeeWise.
	/// </summary>
	public partial class SummaryEmployeeWise : System.Web.UI.Page
	{
		#region Declare Variables
		clsSummaryReport objSummaryReport = new clsSummaryReport();
		clsBLSummaryReport objBLSummaryReport = new clsBLSummaryReport();
		#endregion

		#region Page Load
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
			// Put user code to initialize the page here
			if(!IsPostBack)
			{
				EmployeeDetails();
				
			}
		}
			catch(V2Exceptions ex)
			{
				throw;
			}
			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryEmployeeWise.aspx", "Page_Load", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		#region EmployeeDetails
		public void EmployeeDetails()
		{
			try
			{
				ddlEmployees.Items.Clear();
				DataSet dsEmployee = new DataSet();
			
				dsEmployee=objBLSummaryReport.EmployeeDetails();

				ddlEmployees.Items.Add(new ListItem( "Select Employee", "0"));
				if(dsEmployee.Tables[0].Rows.Count>0)
				{				
					for(int i=0;i<dsEmployee.Tables[0].Rows.Count;i++)
					{
						ddlEmployees.Items.Add(new ListItem(dsEmployee.Tables[0].Rows[i]["EmployeeName"].ToString(),dsEmployee.Tables[0].Rows[i]["employeeID"].ToString()));
					}
				
				}
			}
			catch(V2Exceptions ex)
			{
				throw;
			}
			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryEmployeeWise.aspx", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		#endregion

		#region GetEmployeeWiseReportDetails
		public void GetEmployeeWiseReportDetails()
		{
			try
			{
			DataSet dsEmpSummaryReport = new DataSet();
			objSummaryReport.EmployeeID = Convert.ToInt32( ddlEmployees.SelectedValue);
			objSummaryReport.ReportIssueDate = Convert.ToDateTime(txtFromDate.Text.ToString());
			objSummaryReport.ReportCloseDate = Convert.ToDateTime(txtToDate.Text.ToString());
			dsEmpSummaryReport = objBLSummaryReport.EmpSummaryDetails(objSummaryReport);
			if(dsEmpSummaryReport.Tables[0].Rows.Count < 1)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "No Records Found";
				dgEmpSummaryReport.DataSource = dsEmpSummaryReport;
				dgEmpSummaryReport.DataBind();
			}
			else
			{
				lblMsg.Visible = false;
				dgEmpSummaryReport.DataSource = dsEmpSummaryReport;
				dgEmpSummaryReport.DataBind();
			}
			if(dgEmpSummaryReport.PageCount<=1)
			{
				dgEmpSummaryReport.PagerStyle.Visible=false;
			}
			else
			{
				dgEmpSummaryReport.PagerStyle.Visible=true;
			}
		}
			catch(V2Exceptions ex)
			{
				throw;
			}
			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryEmployeeWise.aspx", "GetEmployeeWiseReportDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		#endregion

		#region Submit
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			try
			{
			GetEmployeeWiseReportDetails();
		}
			catch(V2Exceptions ex)
			{
				throw;
			}
			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryEmployeeWise.aspx", "btnSubmit_Click", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		#endregion
	}
}
