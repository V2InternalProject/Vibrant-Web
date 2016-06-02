using DAL;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Recruitment
{
    internal class DynamicallyTemplatedGridView : ITemplate
    {
        private DataSet dsSLAType = new DataSet();

        #region constructor

        public DynamicallyTemplatedGridView(ListItemType item_type, string field_name, string info_type)
        {
            ItemType = item_type;
            FieldName = field_name;
            InfoType = info_type;
        }

        #endregion constructor

        #region Methods

        public void InstantiateIn(Control Container)
        {
            switch (ItemType)
            {
                case ListItemType.Header:
                    var header_ltrl = new Literal();
                    header_ltrl.Text = "<b>" + FieldName + "</b>";

                    if (FieldName == "...")
                    {
                        header_ltrl.Text = "<b> Action </b>";
                    }
                    if (FieldName == "Stage1")
                    {
                        header_ltrl.Text = "RRF Accepted";
                    }
                    if (FieldName == "Stage2")
                    {
                        header_ltrl.Text = "Interview Scheduled";
                    }
                    if (FieldName == "Stage3")
                    {
                        header_ltrl.Text = "Candidate Selected";
                    }
                    if (FieldName == "Stage4")
                    {
                        header_ltrl.Text = "Offer Generated";
                    }
                    Container.Controls.Add(header_ltrl);

                    break;

                case ListItemType.Footer:

                    if (FieldName == "...")
                    {
                        var insert_button = new ImageButton();
                        insert_button.ID = "insert_button";
                        insert_button.ImageUrl = "../../Images/New Design/add-field-icon.png";
                        insert_button.CommandName = "Edit";
                        insert_button.ToolTip = "Insert";
                        insert_button.Click += insert_button_Click;
                        Container.Controls.Add(insert_button);
                    }
                    else
                    {
                        if (FieldName == "Status")
                        {
                            var chk = new CheckBox();
                            Container.Controls.Add(chk);
                        }
                        else
                        {
                            if (FieldName == "SLATypeID")
                            {
                                if (dsSLAType.Tables.Count <= 0)
                                {
                                    getSLADetails();
                                }
                                var ddlSLA = new DropDownList();
                                ddlSLA.ID = FieldName;
                                ddlSLA.DataBinding += OnDataBinding;
                                Container.Controls.Add(ddlSLA);
                            }
                            else
                            {
                                var txt = new TextBox();
                                Container.Controls.Add(txt);
                            }
                        }
                    }
                    break;

                case ListItemType.Item:
                    switch (InfoType)
                    {
                        case "Command":
                            new Page().Session["DeleteFlag"] = 1;
                            var edit_button = new ImageButton();
                            edit_button.ID = "edit_button";
                            edit_button.ImageUrl = "../../Images/New Design/edit-field-icon.png";
                            edit_button.CommandName = "Edit";
                            edit_button.Click += edit_button_Click;
                            edit_button.ToolTip = "Edit";
                            Container.Controls.Add(edit_button);

                            var delete_button = new ImageButton();
                            delete_button.ID = "delete_button";
                            delete_button.ImageUrl = "../../Images/New Design/cancel-field-icon.png";

                            delete_button.CommandName = "Delete";
                            delete_button.ToolTip = "Delete";
                            delete_button.OnClientClick = "return confirm('Are you sure to delete the record?')";
                            delete_button.Click += delete_button_Click;
                            Container.Controls.Add(delete_button);

                            /* Similarly add button for insert.
                             * It is important to know when 'insert' button is added
                             * its CommandName is set to "Edit"  like that of 'edit' button
                             * only because we want the GridView enter into Edit mode,
                             * and this time we also want the text boxes for corresponding fields empty*/

                            //ImageButton insert_button = new ImageButton();
                            //insert_button.ID = "insert_button";
                            //insert_button.ImageUrl = "~/images/insert.bmp";
                            //insert_button.CommandName = "Edit";
                            //insert_button.ToolTip = "Insert";
                            //insert_button.Click += new ImageClickEventHandler(insert_button_Click);
                            //Container.Controls.Add(insert_button);
                            break;

                        default:

                            if (FieldName == "Status")
                            {
                                var field_lblc = new CheckBox();
                                field_lblc.ID = FieldName;
                                field_lblc.Checked = false; //we will bind it later through 'OnDataBinding' event
                                field_lblc.DataBinding += OnDataBinding;
                                Container.Controls.Add(field_lblc);
                                break;
                            }
                            if (FieldName == "SLATypeID")
                            {
                                if (dsSLAType.Tables.Count <= 0)
                                {
                                    getSLADetails();
                                }
                                var ddlSLA = new DropDownList();
                                ddlSLA.ID = FieldName;
                                ddlSLA.DataBinding += OnDataBinding;
                                Container.Controls.Add(ddlSLA);
                                break;
                            }
                            var field_lbl = new Label();
                            field_lbl.ID = FieldName;
                            field_lbl.Text = String.Empty; //we will bind it later through 'OnDataBinding' event
                            field_lbl.DataBinding += OnDataBinding;
                            Container.Controls.Add(field_lbl);
                            break;
                    }
                    break;

                case ListItemType.EditItem:

                    if (InfoType == "Command")
                    {
                        var update_button = new ImageButton();
                        update_button.ID = "update_button";
                        update_button.CommandName = "Update";
                        update_button.ImageUrl = "../../Images/New Design/save-field-icon.png";
                        if ((int)new Page().Session["InsertFlag"] == 1)
                            update_button.ToolTip = "Add";
                        else
                            update_button.ToolTip = "Update";

                        Container.Controls.Add(update_button);

                        var cancel_button = new ImageButton();
                        cancel_button.ImageUrl = "../../Images/New Design/cancel-field-icon.png";
                        cancel_button.ID = "cancel_button";
                        cancel_button.CommandName = "Cancel";
                        cancel_button.ToolTip = "Cancel";
                        Container.Controls.Add(cancel_button);
                    }
                    else
                    // for other 'non-command' i.e. the key and non key fields, bind textboxes with corresponding field values
                    {
                        if (FieldName == "Status")
                        {
                            var field_Chkbox = new CheckBox();
                            field_Chkbox.ID = FieldName;
                            // field_Chkbox.Text = String.Empty;
                            // if Inert is intended no need to bind it with text..keep them empty
                            if ((int)new Page().Session["InsertFlag"] == 0)
                            {
                                field_Chkbox.DataBinding += OnDataBinding;
                            }
                            Container.Controls.Add(field_Chkbox);
                        }
                        else
                        {
                            if (FieldName == "SLATypeID")
                            {
                                var ddlSLA = new DropDownList();
                                ddlSLA.ID = FieldName;

                                // if Inert is intended no need to bind it with text..keep them empty
                                if ((int)new Page().Session["InsertFlag"] == 0)
                                {
                                    ddlSLA.DataBinding += OnDataBinding;
                                }
                                Container.Controls.Add(ddlSLA);
                            }
                            else
                            {
                                var field_txtbox = new TextBox();
                                field_txtbox.ID = FieldName;
                                field_txtbox.Text = String.Empty;

                                // if Inert is intended no need to bind it with text..keep them empty
                                if ((int)new Page().Session["InsertFlag"] == 0)
                                {
                                    field_txtbox.DataBinding += OnDataBinding;
                                }
                                Container.Controls.Add(field_txtbox);
                            }
                        }
                    }
                    break;
            }
        }

        #endregion Methods

        public void getSLADetails()
        {
            dsSLAType = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure,
                "sp_getSLAType");
        }

        #region data memebers

        private readonly ListItemType ItemType;
        private readonly string FieldName;
        private readonly string InfoType;

        #endregion data memebers

        #region Event Handlers

        //just sets the insert flag ON so that we ll be able to decide in OnRowUpdating event whether to insert or update
        protected void insert_button_Click(Object sender, EventArgs e)
        {
            new Page().Session["InsertFlag"] = 1;
        }

        protected void delete_button_Click(Object sender, EventArgs e)
        {
            new Page().Session["InsertFlag"] = 0;
        }

        //just sets the insert flag OFF so that we ll be able to decide in OnRowUpdating event whether to insert or update
        protected void edit_button_Click(Object sender, EventArgs e)
        {
            new Page().Session["InsertFlag"] = 0;
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            object bound_value_obj = null;
            var ctrl = (Control)sender;
            var data_item_container = (IDataItemContainer)ctrl.NamingContainer;

            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName);

            switch (ItemType)
            {
                case ListItemType.Item:

                    if (FieldName == "Status")
                    {
                        if ((bound_value_obj.ToString().Trim() == "true") ||
                            (bound_value_obj.ToString().Trim() == "True"))
                        {
                            var field_Chkltrl = (CheckBox)sender;
                            field_Chkltrl.Checked = true;
                            field_Chkltrl.Enabled = false;
                        }
                        else
                        {
                            var field_Chkltrl = (CheckBox)sender;
                            field_Chkltrl.Checked = false;
                            field_Chkltrl.Enabled = false;
                        }
                    }
                    else
                    {
                        if (FieldName == "SLATypeID")
                        {
                            var ddlSLA = (DropDownList)sender;

                            for (var i = 0; i < dsSLAType.Tables[0].Rows.Count; i++)
                            {
                                ddlSLA.Items.Add(new ListItem(dsSLAType.Tables[0].Rows[i]["SLAType"].ToString(),
                                    dsSLAType.Tables[0].Rows[i]["ID"].ToString()));
                            }
                            ddlSLA.SelectedValue = Convert.ToString(bound_value_obj);
                            ddlSLA.Enabled = false;
                        }
                        else
                        {
                            var field_ltrl = (Label)sender;
                            field_ltrl.Text = bound_value_obj.ToString();
                        }
                    }

                    break;

                case ListItemType.EditItem:

                    try
                    {
                        var field_txtbox = (TextBox)sender;
                        field_txtbox.Text = bound_value_obj.ToString();
                        break;
                    }
                    catch (Exception ex)
                    {
                        //CheckBox  field_Chkbox = (CheckBox )sender;
                        //field_Chkbox.Text   = bound_value_obj.ToString();

                        if ((bound_value_obj.ToString().Trim() == "true") ||
                            (bound_value_obj.ToString().Trim() == "True"))
                        {
                            var field_Chkltrl = (CheckBox)sender;
                            field_Chkltrl.Checked = true;
                        }
                        else
                        {
                            if (FieldName == "SLATypeID")
                            {
                                var ddlSLA = (DropDownList)sender;
                                if (dsSLAType.Tables.Count <= 0)
                                {
                                    getSLADetails();
                                }
                                for (var i = 0; i < dsSLAType.Tables[0].Rows.Count; i++)
                                {
                                    ddlSLA.Items.Add(new ListItem(dsSLAType.Tables[0].Rows[i]["SLAType"].ToString(),
                                        dsSLAType.Tables[0].Rows[i]["ID"].ToString()));
                                }

                                ddlSLA.SelectedValue = Convert.ToString(bound_value_obj);
                            }
                            else
                            {
                                var field_Chkltrl = (CheckBox)sender;
                                field_Chkltrl.Checked = false;
                            }
                        }

                        break;
                    }

                case ListItemType.Footer:
                    if (FieldName == "SLATypeID")
                    {
                        var ddlSLA = (DropDownList)sender;

                        for (var i = 0; i < dsSLAType.Tables[0].Rows.Count; i++)
                        {
                            ddlSLA.Items.Add(new ListItem(dsSLAType.Tables[0].Rows[i]["SLAType"].ToString(),
                                dsSLAType.Tables[0].Rows[i]["ID"].ToString()));
                        }
                    }
                    else
                    {
                        var field_ltrl1 = (Label)sender;
                        field_ltrl1.Text = bound_value_obj.ToString();
                    }

                    break;
            }
        }

        #endregion Event Handlers
    }
}