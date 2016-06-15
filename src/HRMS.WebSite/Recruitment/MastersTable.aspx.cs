using BLL;
using DAL;
using HRMS.Recruitment;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters : System.Web.UI.Page
{
    #region Data members

    public static DataTable dtTable = new DataTable();
    public static DataTable dtSchema = new DataTable();
    private ArrayList ParameterArray = new ArrayList();
    public string TableName = string.Empty;
    public string SelectedTableName = string.Empty;
    private int Flag = 0;
    private int count = 0;

    public static DataTable dtMaxTable = new DataTable();

    #endregion Data members

    private MasterTableDAL MasterDal = new MasterTableDAL();

    #region Events Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        //    if (Page.IsPostBack && (bool)Session["IsConnectionInfoSet"] == true)
        //    {
        //        //int BlankRow = 1;
        //        SelectedTableName = Session["SelectedTableName"].ToString();

        //        //GridViewCreateTemplated(BlankRow);
        //        GridViewCreateTemplated();

        //    }
        //    else if (!Page.IsPostBack)
        //    {
        //        BindTable();

        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
        //}
        //finally
        //{
        //}
        try
        {
            if (Page.IsPostBack && (bool)Session["IsConnectionInfoSet"] == true)
            {
                //int BlankRow = 1;
                SelectedTableName = Session["SelectedTableName"].ToString();

                //GridViewCreateTemplated(BlankRow);
                GridViewCreateTemplated();
            }
            else if (!Page.IsPostBack)
            {
                BindTable();
            }
        }
        catch (System.Exception ex)
        {
        }
        finally
        {
        }
    }

    public void BindTable()
    {
        DataSet Ds = new DataSet();
        Ds = MasterDal.GetMasterTableName();

        if (Ds.Tables[0].Rows.Count > 0)
        {
            ddlMasterTableName.DataSource = Ds.Tables[0];
            ddlMasterTableName.DataTextField = "TableNAME";
            ddlMasterTableName.DataValueField = "TableNAME";

            ddlMasterTableName.DataBind();
            ddlMasterTableName.Items.Insert(0, new ListItem("Please select table name", ""));
        }
    }

    protected void ddlTableName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSuccessMessage.Text = string.Empty;
        lblErrorMessage.Text = string.Empty;

        lblSuccessMessage.Visible = false;
        lblErrorMessage.Visible = false;

        string SelectedTableName = string.Empty;
        SelectedTableName = ddlMasterTableName.SelectedValue.ToString();

        if (!string.IsNullOrEmpty(SelectedTableName))
        {
            Session["IsConnectionInfoSet"] = true;
            Session["SelectedTableName"] = SelectedTableName;
            grdMaster.EditIndex = -1;
            int BlankRowCount = 0;
            //GridViewCreateTemplated(BlankRowCount);
            GridViewCreateTemplated();
            grdMaster.Visible = true;
        }
        else
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Please select table Name";
            grdMaster.Visible = false;
        }

        // getDataLenth(ddlMasterTableName.SelectedValue.ToString());
    }

    // void GridViewCreateTemplated(int BlankRow)
    private void GridViewCreateTemplated()
    {
        // fill the table which is to bound to the GridView

        string TableName = Session["SelectedTableName"].ToString();
        //DataTable dtTable =
        PopulateDataTable(TableName);

        // add templated fields to the GridView
        TemplateField BtnTmpField = new TemplateField();

        //if (BlankRow == 0)
        //{
        if (dtTable != null && dtTable.Rows.Count == 0)
        {
            //if (Convert.ToInt16(Session["DeleteFlag"]) != 1)
            //{
            dtTable.Rows.Add(dtTable.NewRow());

            BtnTmpField.HeaderTemplate =
            new DynamicallyTemplatedGridView(ListItemType.Header, "...", "Command");

            BtnTmpField.FooterTemplate =
               new DynamicallyTemplatedGridView(ListItemType.Footer, "...", "Command");
            grdMaster.Columns.Add(BtnTmpField);

            for (int i = 0; i < dtTable.Columns.Count; i++)
            {
                TemplateField ItemTmpField = new TemplateField();
                ItemTmpField.HeaderTemplate = new DynamicallyTemplatedGridView(ListItemType.Header,
                                                              dtTable.Columns[i].ColumnName,
                                                              dtTable.Columns[i].DataType.Name);

                ItemTmpField.FooterTemplate = new DynamicallyTemplatedGridView(ListItemType.Footer,
                                                             dtTable.Columns[i].ColumnName,
                                                             dtTable.Columns[i].DataType.Name);

                grdMaster.Columns.Add(ItemTmpField);
            }

            grdMaster.DataSource = dtTable;
            grdMaster.DataBind();
            grdMaster.Rows[0].Visible = false;
            // BlankRow = 1;
        }
        //}
        else
        {
            BtnTmpField.HeaderTemplate =
                           new DynamicallyTemplatedGridView(ListItemType.Header, "...", "Command");

            BtnTmpField.ItemTemplate =
                new DynamicallyTemplatedGridView(ListItemType.Item, "...", "Command");

            BtnTmpField.EditItemTemplate =
                new DynamicallyTemplatedGridView(ListItemType.EditItem, "...", "Command");
            BtnTmpField.FooterTemplate =
               new DynamicallyTemplatedGridView(ListItemType.Footer, "...", "Command");

            grdMaster.Columns.Add(BtnTmpField);

            for (int i = 0; i < dtTable.Columns.Count; i++)
            {
                TemplateField ItemTmpField = new TemplateField();
                // create HeaderTemplate
                ItemTmpField.ItemTemplate = new DynamicallyTemplatedGridView(ListItemType.Item,
                                                              dtTable.Columns[i].ColumnName,
                                                              dtTable.Columns[i].DataType.Name);
                // create ItemTemplate
                ItemTmpField.HeaderTemplate = new DynamicallyTemplatedGridView(ListItemType.Header,
                                                              dtTable.Columns[i].ColumnName,
                                                              dtTable.Columns[i].DataType.Name);

                //create EditItemTemplate
                ItemTmpField.EditItemTemplate = new DynamicallyTemplatedGridView(ListItemType.EditItem,
                                                              dtTable.Columns[i].ColumnName,
                                                              dtTable.Columns[i].DataType.Name);

                //create footerTemplate

                ItemTmpField.FooterTemplate = new DynamicallyTemplatedGridView(ListItemType.Footer,
                                                             dtTable.Columns[i].ColumnName,
                                                             dtTable.Columns[i].DataType.Name);

                // then add to the GridView
                grdMaster.Columns.Add(ItemTmpField);
            }

            // bind and display the data
            grdMaster.DataSource = null;
            grdMaster.DataSource = dtTable;
            grdMaster.DataBind();
        }
    }

    public void grdMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int count = grdMaster.Columns.Count;
        grdMaster.EditIndex = e.NewEditIndex;

        //grdMaster.Columns[count - 1].Visible = false;
        //grdMaster.Columns[count - 2].Visible = false;

        grdMaster.DataBind();
        Session["SelecetdRowIndex"] = e.NewEditIndex;
    }

    public void grdMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdMaster.EditIndex = -1;
        grdMaster.DataBind();
        Session["SelecetdRowIndex"] = -1;
        lblSuccessMessage.Visible = false;
        lblErrorMessage.Visible = false;
    }

    protected void grdMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblErrorMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        int rowNumber = 0;
        int rowCount = 1;
        string TableName = Session["SelectedTableName"].ToString();
        bool resultDelete = false;
        try
        {
            rowNumber = (grdMaster.PageIndex * grdMaster.PageSize) + e.RowIndex;
            resultDelete = MasterDal.DeleteMasterTable(TableName, rowNumber, dtTable);

            if (resultDelete == true)
            {
                // GridViewCreateTemplated(rowCount);
                GridViewCreateTemplated();
                lblSuccessMessage.Visible = true;
                lblSuccessMessage.Text = "Record deleted successfully";

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "err_msg", "alert('" + error + "');", true);
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Record not deleted successfully";
            }
        }
        catch (SqlException sqlex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "The record cannot be deleted as it is being used in the application.";
            //throw new MyException("Exception in  btnSave_Click(). Message:" + sqlex.Message, sqlex.InnerException);
        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Record not deleted as there are dependencies present.";
            throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
        }
    }

    public void grdMaster_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        lblErrorMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        //string TableNameabc = Session["MasterTableSelected"];
        int userID = Convert.ToInt32(Session["userID"].ToString());
        int rowCount = dtTable.Columns.Count;
        GridViewRow row = grdMaster.Rows[e.RowIndex];

        string field_value = string.Empty;
        string Row_value = string.Empty;
        for (int i = 0; i < dtTable.Columns.Count; i++)
        {
            try
            {
                Row_value = ((TextBox)row.FindControl(dtTable.Columns[0].ColumnName)).Text;
                field_value = ((TextBox)row.FindControl(dtTable.Columns[i].ColumnName)).Text;
                ParameterArray.Add(field_value);
            }
            catch (System.Exception ex)
            {
                if (dtTable.Columns[i].ColumnName == "SLATypeID")
                {
                    string ddlSLATypeValue;

                    ddlSLATypeValue = ((DropDownList)row.FindControl(dtTable.Columns[i].ColumnName)).SelectedValue;
                    ParameterArray.Add(ddlSLATypeValue);
                }
                else
                {
                    Boolean chkvalue;

                    // Row_value = ((CheckBox)row.FindControl(dtTable.Columns[0].ColumnName)).Text;
                    chkvalue = ((CheckBox)row.FindControl(dtTable.Columns[i].ColumnName)).Checked;
                    ParameterArray.Add(chkvalue);
                }
            }
        }
        string Query = string.Empty;

        try
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // new added section
            string UpdateResult = string.Empty;
            if (SelectedTableName == "tbl_SLA_Type")
            {
                if (!string.IsNullOrEmpty(ParameterArray[1].ToString()) && !string.IsNullOrEmpty(ParameterArray[2].ToString()) && !string.IsNullOrEmpty(ParameterArray[3].ToString()) && !string.IsNullOrEmpty(ParameterArray[4].ToString()) && !string.IsNullOrEmpty(ParameterArray[5].ToString()) && !string.IsNullOrEmpty(ParameterArray[6].ToString()))
                {
                    int TotalStageDays;
                    TotalStageDays = Convert.ToInt32(ParameterArray[2].ToString());
                    int StageDays;
                    StageDays = Convert.ToInt32(ParameterArray[3].ToString()) + Convert.ToInt32(ParameterArray[4].ToString()) + Convert.ToInt32(ParameterArray[5].ToString()) + Convert.ToInt32(ParameterArray[6].ToString());
                    if (TotalStageDays == StageDays)
                    {
                        UpdateResult = MasterDal.MasterTableUpdate(SelectedTableName, dtTable, ParameterArray, userID, Row_value);
                    }
                    else
                    {
                        UpdateResult = "Total Days should be Equal to Sum of all Stages";
                    }
                }
                else
                    UpdateResult = "Please enter some text";
            }
            else
            {
                UpdateResult = MasterDal.MasterTableUpdate(SelectedTableName, dtTable, ParameterArray, userID, Row_value);
            }

            // new added section
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //string UpdateResult = MasterDal.MasterTableUpdate(SelectedTableName, dtTable, ParameterArray, userID, Row_value);

            if (UpdateResult == "")
            {
                grdMaster.EditIndex = -1;
                //GridViewCreateTemplated(rowCount);
                GridViewCreateTemplated();
                lblSuccessMessage.Visible = true;
                lblSuccessMessage.Text = "Record updated successfully.";
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = UpdateResult;
            }
        }
        catch (System.Exception ex)
        {
            // throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void grdMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblErrorMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        grdMaster.PageIndex = e.NewPageIndex;
        grdMaster.DataBind();
    }

    #endregion Events Handlers

    protected void grdMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        int rowCount = e.Row.Cells.Count;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[rowCount - 1].Visible = false;
            e.Row.Cells[rowCount - 2].Visible = false;
            e.Row.Cells[rowCount - 3].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[rowCount - 1].Visible = false;
            e.Row.Cells[rowCount - 2].Visible = false;
            e.Row.Cells[rowCount - 3].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[rowCount - 3].Visible = false;
            e.Row.Cells[rowCount - 1].Visible = false;
            e.Row.Cells[rowCount - 2].Visible = false;
        }
    }

    protected void grdMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblErrorMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        string field_value = "";
        int rowCount = 1;

        string Query = string.Empty;

        string TableName = Session["SelectedTableName"].ToString();

        int userID = Convert.ToInt32(Session["userID"].ToString());

        if (Session["InsertFlag"] != null && (int)Session["InsertFlag"] == 1)
        {
            string SLaTypeName = string.Empty;
            GridViewRow row = grdMaster.FooterRow;
            for (int i = 1; i <= dtTable.Columns.Count; i++)
            {
                try
                {
                    field_value = ((TextBox)(row.Cells[i].Controls[0])).Text;
                    ParameterArray.Add(field_value);
                }
                catch (System.Exception ex)
                {
                    if (dtTable.Columns[i - 1].ColumnName == "SLATypeID")
                    {
                        string ddlSLATypeValue;

                        ddlSLATypeValue = ((DropDownList)(row.Cells[i].Controls[0])).SelectedValue;
                        ParameterArray.Add(ddlSLATypeValue);
                    }
                    else
                    {
                        Boolean chkvalue;
                        chkvalue = ((CheckBox)(row.Cells[i].Controls[0])).Checked;
                        ParameterArray.Add(chkvalue);
                    }
                }
            }

            int a = (int)Session["InsertFlag"];

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // new added section

            string InsertResult = string.Empty;
            if (TableName == "tbl_SLA_Type")
            {
                if (!string.IsNullOrEmpty(ParameterArray[1].ToString()) && !string.IsNullOrEmpty(ParameterArray[2].ToString()) && !string.IsNullOrEmpty(ParameterArray[3].ToString()) && !string.IsNullOrEmpty(ParameterArray[4].ToString()) && !string.IsNullOrEmpty(ParameterArray[5].ToString()) && !string.IsNullOrEmpty(ParameterArray[6].ToString()))
                {
                    if (Check_Number(ParameterArray[2].ToString()) && Check_Number(ParameterArray[3].ToString()) && Check_Number(ParameterArray[4].ToString()) && Check_Number(ParameterArray[5].ToString()) && Check_Number(ParameterArray[6].ToString()))
                    {
                        int TotalStageDays;
                        TotalStageDays = Convert.ToInt32(ParameterArray[2].ToString());
                        int StageDays;
                        StageDays = Convert.ToInt32(ParameterArray[3].ToString()) + Convert.ToInt32(ParameterArray[4].ToString()) + Convert.ToInt32(ParameterArray[5].ToString()) + Convert.ToInt32(ParameterArray[6].ToString());
                        if (TotalStageDays == StageDays)
                        {
                            InsertResult = MasterDal.InsertMasterTable(TableName, dtTable, ParameterArray, userID);
                        }
                        else
                        {
                            InsertResult = "Total Days should be Equal to Sum of all Stages";
                        }
                    }
                    else
                    {
                        InsertResult = "Some Data are Invalid";
                    }
                }
                else
                    InsertResult = "Please enter some text";
            }
            else
            {
                InsertResult = MasterDal.InsertMasterTable(TableName, dtTable, ParameterArray, userID);
            }

            // new added section
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Old Section
            // string InsertResult = MasterDal.InsertMasterTable(TableName, dtTable, ParameterArray, userID);
            //Old Section

            if (InsertResult == "")
            {
                count++;

                //GridViewCreateTemplated(rowCount);
                GridViewCreateTemplated();
                lblSuccessMessage.Visible = true;
                lblSuccessMessage.Text = "Record added successfully.";
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = InsertResult;
            }
        }
    }

    public static bool Check_Number(string value)
    {
        int checkInt;
        bool check = int.TryParse(value, out checkInt);
        return check;
    }

    public DataTable PopulateDataTable(string TableName)
    {
        dtTable = new DataTable();
        DataTable Dt = new DataTable();
        grdMaster.Columns.Clear();
        dtSchema = MasterDal.GetSchemaDetails(TableName);
        DataSet Ds = new DataSet();
        Ds = GetTableDetails(TableName);
        dtTable = Ds.Tables[0];

        int colCount = dtTable.Columns.Count;

        //dtTable.Columns.RemoveAt(colCount - 3);

        //dtTable.Columns.Remove("LastModifiedBy");
        return dtTable;
    }

    public DataSet GetTableDetails(string TableName)
    {
        dtTable = new DataTable();
        grdMaster.Columns.Clear();
        // string TableNameValue = (string)Session["MasterTableSelected"];
        DataSet ds = new DataSet();
        DataSet Ds = new DataSet();
        ds = MasterDal.populateDatatable(TableName);
        //Ds = ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["LastModifiedBy"]);
        return ds;
    }

    protected void grdMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtSortTable = grdMaster.DataSource as DataTable;

        if (dtSortTable != null)
        {
            DataView dvSortedView = new DataView(dtSortTable);
            dvSortedView.Sort = e.SortExpression + " " + getSortDirectionString(e.SortDirection);

            grdMaster.DataSource = dvSortedView;
            grdMaster.DataBind();
        }
    }

    private string getSortDirectionString(SortDirection sortDireciton)
    {
        string newSortDirection = String.Empty;
        if (sortDireciton == SortDirection.Ascending)
        {
            newSortDirection = "ASC";
        }
        else
        {
            newSortDirection = "DESC";
        }

        return newSortDirection;
    }
}