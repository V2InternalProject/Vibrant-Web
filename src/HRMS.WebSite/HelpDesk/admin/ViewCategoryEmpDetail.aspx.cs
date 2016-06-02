using System;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewCategoryEmpDetail.
    /// </summary>
    public partial class ViewCategoryEmpDetail : System.Web.UI.Page
    {
        private int EmployeeId, SuperAdmin;

        //DataView dv;
        private clsBLSubCategoryAssignment objBLSubCategoryAssignment = new clsBLSubCategoryAssignment();

        private clsSubCategoryAssignment objSubCategoryAssignment = new clsSubCategoryAssignment();
        private DataSet dsGetData = new DataSet();
        protected System.Web.UI.WebControls.Label lblCategory;
        protected System.Web.UI.WebControls.LinkButton lnkAddNew;
        private string subCategory = "";
        private int i = 0; private int k = 1;
        private int NoOfTable;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                int EmployeeID, SAEmployeeID;
                EmployeeId = Convert.ToInt32(Request.QueryString["EmpId"]);
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);
                // OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                objSubCategoryAssignment.EmployeeID = EmployeeId;
                lblEmployeeId.Text = EmployeeId.ToString();
                //if (EmployeeID.ToString() == "" || EmployeeID == 0 || SuperAdmin != 0 || OnlySuperAdmin != 0)
                //{
                //    if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && SuperAdmin == 0 && OnlySuperAdmin == 0)
                //    {
                //        Response.Redirect("Login.aspx");
                //    }
                //}
                if (SuperAdmin == 0)
                //{
                //    Response.Redirect("Login.aspx");
                //}
                //else
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
                if (!IsPostBack)
                {
                    bindData(objSubCategoryAssignment);
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewCategoryEmpDetail.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

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
            this.RepSubCategory.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.RepSubCategory_ItemDataBound);
            this.RepSubCategory.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.RepSubCategory_ItemCommand);
        }

        #endregion Web Form Designer generated code

        #region Code for Binding Employeee Name

        public void bindData(clsSubCategoryAssignment objSubCategoryAssignment)
        {
            try
            {
                dsGetData = objBLSubCategoryAssignment.getDetialOfEmployee(objSubCategoryAssignment);
                RepSubCategory.DataSource = dsGetData.Tables[0];

                NoOfTable = dsGetData.Tables[0].Rows.Count;
                if (dsGetData.Tables[0].Rows.Count > 0)
                {
                    lblEmployeeName.Text = dsGetData.Tables[0].Rows[i][1].ToString();
                }
                RepSubCategory.DataBind();

                #region Commented Old Code

                //			dsGetData = objBLSubCategoryAssignment.getDetialOfEmployee(objSubCategoryAssignment);
                //			RepSubCategory.DataBind();
                //			//int i=0;
                //			int NoOfTable=dsGetData.Tables[0].Rows.Count;
                //			//lblEmployeeName.Text=dsGetData.Tables[0].Rows[i][0].ToString();
                //			if(dsGetData.Tables[0].Rows.Count>0)
                //			{
                //				lblEmployeeName.Text=dsGetData.Tables[0].Rows[i][1].ToString();
                //			}
                //			for(i=0;i<dsGetData.Tables[NoOfTable].Rows.Count;i++)
                //			{
                //				if(k<=NoOfTable)
                //				{
                //					if(dsGetData.Tables[0].Rows[i][4].ToString()=="Admin")
                //					{
                //						//lblSuperAdminforAdmin.Visible=true;
                //						if(dsGetData.Tables[0].Rows[i][3].ToString()=="True")
                //						{
                //							//lblSuperAdminforAdmin.Text=SuperUserForAdmin+ "(Super Admin)";
                //						}
                //						for(int j=0;j<dsGetData.Tables[k].Rows.Count;j++)
                //						{
                //							//	lblForAdmin.Visible=true;
                //							subCategoryAdmin=subCategoryAdmin+"</br>"+dsGetData.Tables[k].Rows[j][0].ToString();
                //							//	lblForAdmin.Text=subCategoryAdmin.Replace("<br>","/r/n");
                //						}
                //					}
                //					else if(dsGetData.Tables[0].Rows[i][4].ToString()=="HR")
                //					{
                //						//lblSuperAdminforHR.Visible=true;
                //						if(dsGetData.Tables[0].Rows[i][3].ToString()=="True")
                //						{
                //							//	lblSuperAdminforHR.Text=SuperUserForHR+ "(Super Admin)";
                //						}
                //						for(int j=0;j<dsGetData.Tables[k].Rows.Count;j++)
                //						{
                //							//	lblForHR.Visible=true;
                //							subCategoryHR=subCategoryHR+"</br>"+dsGetData.Tables[k].Rows[j][0].ToString();
                //							//lblForHR.Text=subCategoryHR.Replace("<br>","/r/n");
                //						}
                //					}
                //					else if(dsGetData.Tables[0].Rows[i][4].ToString()=="IT")
                //					{
                //						//lblSuperAdminforIt.Visible=true;
                //						if(dsGetData.Tables[0].Rows[i][3].ToString()=="True")
                //						{
                //							//	lblSuperAdminforIt.Text=SuperUserForIT+ "(Super Admin)";
                //						}
                //						for(int j=0;j<dsGetData.Tables[k].Rows.Count;j++)
                //						{
                //							//	lblForIt.Visible=true;
                //							subCategoryIt=subCategoryIt +"</br>"+dsGetData.Tables[k].Rows[j][0].ToString();
                //							//	lblForIt.Text=subCategoryIt.Replace("<br>","/r/n");
                //						}
                //					}
                //					k++;
                //				}
                //			}

                #endregion Commented Old Code
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewCategoryEmpDetail.aspx", "bindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("ViewEmployeeDetails.aspx");
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewCategoryEmpDetail.aspx", "btnClose_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void RepSubCategory_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
        }

        protected void RepSubCategory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblSystemAdmin = (Label)(e.Item.FindControl("lblSystemAdmin"));
                    Label lblSubCatagory = (Label)(e.Item.FindControl("lblSubCatagory"));

                    if (i < NoOfTable)
                    {
                        if (Convert.ToInt32(dsGetData.Tables[0].Rows[i][3]) == 1)
                        {
                            lblSystemAdmin.Text = dsGetData.Tables[0].Rows[i][4] + "(Administrator)";
                        }
                        else
                        {
                            lblSystemAdmin.Text = dsGetData.Tables[0].Rows[i][4].ToString();
                        }
                        i++;
                    }
                    if (k <= NoOfTable)
                    {
                        for (int j = 0; j < dsGetData.Tables[k].Rows.Count; j++)
                        {
                            subCategory = subCategory + "" + dsGetData.Tables[k].Rows[j][0].ToString();
                            subCategory = subCategory + "</br>";
                            lblSubCatagory.Text = subCategory.Replace("<br>", "/r/n");
                        }
                        subCategory = "";
                    }

                    k++;
                    lblSystemAdmin.DataBind();
                    lblSubCatagory.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewCategoryEmpDetail.aspx", "RepSubCategory_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("ViewEmployeeDetails.aspx");
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewCategoryEmpDetail.aspx", "btnBack_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}