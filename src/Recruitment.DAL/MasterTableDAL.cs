using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DAL
{

    public class MasterTableDAL
    {
        DataTable dtTable = new DataTable();
        DataTable dtSchema = new DataTable();
        ArrayList parameterArray = new ArrayList();
        string errorMsg = "";
        int Flag = 0;


        public bool DeleteMasterTable(string tableName, int RowIndex, DataTable dtTable)
        {
            string deleteQuery = string.Empty;
            deleteQuery = GenerateDeleteQuery(RowIndex, dtTable);
            bool result = false;
            using (SqlConnection myConnection = new SqlConnection(AppConfiguration.ConnectionString))
            {
                SqlCommand myCommand = new SqlCommand("sp_Deletemaster", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@Operation", "DELETE");
                myCommand.Parameters.AddWithValue("@TableName", tableName);
                myCommand.Parameters.AddWithValue("@values", deleteQuery);
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
            //Session["DeleteFlag"] = 1;

            return result;
            //GridViewCreateTemplated();
        }


        public string InsertMasterTable(string tableName, DataTable dtTable, ArrayList parameterArray, int userID)
        {
            //bool insertrResult = false;
            string errorCheck = "";
            string InsertQuery = string.Empty;
            InsertQuery = GenerateInsertQuery(tableName, dtTable, parameterArray, userID);

            if (!string.IsNullOrEmpty(InsertQuery))
            {
                using (SqlConnection myConnection = new SqlConnection(AppConfiguration.ConnectionString))
                {
                    SqlCommand myCommand = new SqlCommand("sp_AddUpdateMaster", myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@Operation", "Add");
                    myCommand.Parameters.AddWithValue("@TableName", tableName);
                    if (!string.IsNullOrEmpty(InsertQuery))
                    {
                        myCommand.Parameters.AddWithValue("@values", InsertQuery);
                        myConnection.Open();
                        myCommand.ExecuteNonQuery();

                        myConnection.Close();

                        //insertrResult = true;
                    }

                    return errorCheck;
                }
            }
            else
            {
                errorCheck = errorMsg;
                return errorCheck;
            }
            //return insertrResult;
        }

        string GenerateUpdateQuery(string tableName, DataTable dtTable, ArrayList parameterArray, int userID, string Row_value)
        {
            int i = 0;
            string tempstr = "";
            int temp_index = -1;
            // string TableName = (string)Session["MasterTableSelected"];
            string Query = "";
            //dtTable.Columns.Remove("Last Modified By");
            int[] colMaxLenth = checkColumnLength(tableName);

            int UpdatedRow = Convert.ToInt32(Row_value);

            //             int RowId=dtTable.Rows[i][1].ToString()).ToUpper()

            for (i = 0; i < dtTable.Rows.Count; i++)
            {


                if ((parameterArray[1].ToString()).ToUpper() == (dtTable.Rows[i][1].ToString()).ToUpper())
                {

                    if ((dtTable.Rows[i][0].ToString() == Row_value.ToString()) && (dtTable.Rows[i][1].ToString() == parameterArray[1].ToString()))
                    {
                        errorMsg = string.Empty;
                        Flag = 0;
                        break;
                    }
                    else
                    {

                        //System.Windows.Forms.MessageBox.Show("The data item " + parameterArray[1].ToString() + " already exists in the table", "Recruitment module");
                        errorMsg = "The data item " + parameterArray[1].ToString() + " already exists in the table";
                        Flag = 1;
                        break;
                    }

                }
            }


            for (i = 1; i < dtTable.Columns.Count - 1; i++)
            {
                switch (dtTable.Columns[i].DataType.Name)
                {

                    case "Boolean":
                        if ((Boolean )parameterArray[i]== true)
                            parameterArray[i] = "1";
                        else if ((Boolean)parameterArray[i] == false)
                            parameterArray[i] = "0";

                        
                        if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                        {
                            // Query = string.Empty;
                            Query = Query + dtTable.Columns[i].ColumnName + "=' ', ";
                        }
                        else
                        {
                            Query = Query + dtTable.Columns[i].ColumnName + "=" + parameterArray[i] + ", ";
                        }
                        break;

                    case "Int32":


                        string colname = parameterArray[i].ToString().Trim();

                        //if (colname.Length < 1)
                        //{
                        //    errorMsg = "Please enter some text  in " + dtTable.Columns[i];
                        //    Flag = 1;
                        //    Query = string.Empty;
                        //    break;
                        //}
                         Regex r;
                        if(dtTable.Columns[i].ColumnName == "Score")
                            r = new Regex("^[1-4]*$");
                        else
                            r = new Regex("^[0-9]*$");

                        Regex reg1 = new Regex("^[a-zA-Z0-9_]*$");
                        if (colname.Length < 1)
                        {
                            errorMsg = "Please enter numeric value in " + dtTable.Columns[i];
                            Flag = 1;
                            Query = string.Empty;
                            break;
                        }
                       
                        else if (!r.IsMatch(parameterArray[i].ToString()))
                        {
                            //lblmessage.Text = "Please enter numeric values in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Please enter numeric values in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Please enter numeric values between 1-4 in " + dtTable.Columns[i];
                            Query = string.Empty;
                            Flag = 1;
                            break;
                        }
                        //}else if (dtTable.Columns[i].AllowDBNull == false)
                        //{
                        //    lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                        //    Flag = 1;
                        //    break;
                        //}
                        else if ((dtSchema != null) && (dtSchema.Rows.Count > 0))
                        {
                            if (dtTable.Columns[i].ToString() == dtSchema.Rows[i].ItemArray[0].ToString())
                            {
                                if ((dtSchema.Rows[i]["AllowDBNull"].ToString() == "False") && (string.IsNullOrEmpty(parameterArray[i].ToString())))
                                {
                                    //lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                                    //System.Windows.Forms.MessageBox.Show("Please enter value in " + dtTable.Columns[i], "Recruitment module");
                                    errorMsg = "Please enter value in " + dtTable.Columns[i];
                                    Flag = 1;
                                    Query = string.Empty;
                                    break;
                                }

                            }


                        }
                        else if (!reg1.IsMatch(parameterArray[i].ToString()))
                        {
                            //lblmessage.Text = "Please enter alphanumeric values in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Please enter alphanumeric values in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Please enter alphanumeric values in  " + dtTable.Columns[i];
                            Flag = 1;
                            Query = string.Empty;
                            break;
                        }
                        if ((string)parameterArray[i] == "True")
                            parameterArray[i] = "1";
                        else if ((string)parameterArray[i] == "False")
                            parameterArray[i] = "0";

                        if (i == dtTable.Columns.Count - 3)
                            //if (string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            //{
                            //    Query = Query + dtTable.Columns[i].ColumnName + "=' ' ";
                            //}
                            //else
                            //{
                            //    Query = Query + dtTable.Columns[i].ColumnName + "=" + ParameterArray[i];
                            //}
                            Query = Query + dtTable.Columns[i].ColumnName + "=" + userID + ", ";
                        else
                            if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {
                                // Query = string.Empty;
                                Query = Query + dtTable.Columns[i].ColumnName + "=' ', ";
                            }
                            else
                            {
                                Query = Query + dtTable.Columns[i].ColumnName + "=" + parameterArray[i] + ", ";
                            }
                        break;
                    case "String":
                        // Regex reg = new Regex("^[a-zA-Z0-9_]*$");
                        //Regex reg = new Regex("^[a-zA-Z0-9_\\s\\.\\#\\(\\)]*$");

                        Regex reg = new Regex("^[a-zA-Z0-9_\\s\\.\\#\\+\\[\\]\\,\\(\\)]*$");

                        string colname1 = parameterArray[i].ToString().Trim();

                        if (colname1.Length < 1)
                        {
                            errorMsg = "Please enter some text  in " + dtTable.Columns[i];
                            Flag = 1;
                            Query = string.Empty;
                            break;
                        }
                        //if (dtTable.Columns[i].AllowDBNull == false)
                        //{
                        //    lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                        //    break;
                        //}
                        if ((dtSchema != null) && (dtSchema.Rows.Count > 0))
                        {
                            if (dtTable.Columns[i].ToString() == dtSchema.Rows[i].ItemArray[0].ToString())
                            {
                                if ((dtSchema.Rows[i]["AllowDBNull"].ToString() == "False") && (string.IsNullOrEmpty(parameterArray[i].ToString())))
                                {
                                    //lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                                    //System.Windows.Forms.MessageBox.Show("Please enter value in " + dtTable.Columns[i], "Recruitment module");
                                    errorMsg = "Please enter value in " + dtTable.Columns[i];
                                    Flag = 1;
                                    Query = string.Empty;
                                    break;
                                }

                            }


                        }
                        else if (!reg.IsMatch(parameterArray[i].ToString()))
                        {
                            //lblmessage.Text = "Please enter alphanumeric values in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Please enter alphanumeric values in " + dtTable.Columns[i]);
                            //System.Windows.Forms.MessageBox.Show("Please enter alphanumeric values in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Please enter alphanumeric values in " + dtTable.Columns[i];
                            Query = string.Empty;
                            Flag = 1;
                            break;
                        }
                        else if (colMaxLenth[i - 1] == 1)
                        {
                            if (!(parameterArray[i].ToString().StartsWith("F")) || (parameterArray[i].ToString().StartsWith("f")) || (parameterArray[i].ToString().StartsWith("M")) || (parameterArray[i].ToString().StartsWith("m")))
                            {
                                //lblmessage.Text = "Please enter M/F character value in " + dtTable.Columns[i];
                                //System.Windows.Forms.MessageBox.Show("Please enter M/F character value in " + dtTable.Columns[i], "Recruitment module");
                                errorMsg = "Please enter M/F character value in " + dtTable.Columns[i];
                                Flag = 1;
                                Query = string.Empty;
                                break;
                            }
                        }
                        else if (colname1.Length > colMaxLenth[i])
                        {
                            //lblmessage.Text = "Entered character is too long in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Entered character is too long in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Entered character is too long in " + dtTable.Columns[i];
                            Flag = 1;
                            break;
                        }

                        if (((string)parameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)parameterArray[i]);
                            parameterArray[i] = ((string)parameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }

                        if (i == dtTable.Columns.Count - 1)
                            if (!string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {
                                Query = Query + dtTable.Columns[i].ColumnName + "='" + parameterArray[i] + "' ";
                            }
                            else
                            {
                                Query = Query + dtTable.Columns[i].ColumnName + "=' ' ";
                            }
                        else
                            if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {
                                Query = Query + dtTable.Columns[i].ColumnName + "=' ', ";
                            }
                            else
                            {
                                Query = Query + dtTable.Columns[i].ColumnName + "='" + parameterArray[i] + "', ";
                            }
                        break;
                    case "DateTime":
                        if (((string)parameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)parameterArray[i]);
                            parameterArray[i] = ((string)parameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }

                        if (i == dtTable.Columns.Count - 1)
                            Query = Query + dtTable.Columns[i].ColumnName + "='" + DateTime.Now.ToString() + "' ";
                        else
                            //Query = Query + dtTable.Columns[i].ColumnName + "='" + DateTime.Now.ToString() + "', ";
                            Query = Query + dtTable.Columns[i].ColumnName + "='" + DateTime.Now.ToString() + "' ";
                        break;

                }
            }
            if (temp_index > -1)
                parameterArray[temp_index] = tempstr;
            if (dtTable.Columns[0].DataType.Name == "String" || dtTable.Columns[0].DataType.Name == "DateTime")
            {
                if (!string.IsNullOrEmpty(Query))
                {
                    Query = Query + " where " + dtTable.Columns[0].ColumnName + " = '" + parameterArray[0] + "'";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Query))
                {
                    Query = Query + " where " + dtTable.Columns[0].ColumnName + " = " + parameterArray[0];
                }
            }


            if (Flag == 1)
            {
                Query = "";
                return Query;
            }
            else
            {
                Query = Query + "";

                return Query;
            }
        }

        string GenerateInsertQuery(string tableName, DataTable dtTable, ArrayList parameterArray, int userID)
        {
            int i = 0;
            string tempstr = "";
            int temp_index = -1;
            string Query = string.Empty;

            //Logic to stop duplicate entries
            for (i = 0; i < dtTable.Rows.Count; i++)
            {
                if ((parameterArray[1].ToString()).ToUpper() == (dtTable.Rows[i][1].ToString()).ToUpper())
                {
                    //System.Windows.Forms.MessageBox.Show("The data item " + parameterArray[1].ToString() + " already exists in the table", "Recruitment module");
                    errorMsg = "The data item " + parameterArray[1].ToString() + " already exists in the table";
                    Flag = 1;
                    break;
                }
            }




            int[] colMaxLenth = checkColumnLength(tableName);
            for (i = 1; i < dtTable.Columns.Count - 1; i++)
            {
                string colname = parameterArray[i].ToString().Trim();

                switch (dtTable.Columns[i].DataType.Name)
                {


                    case "Boolean":
                        if (Convert.ToBoolean(parameterArray[i])  == true)
                            parameterArray[i] = "1";
                        else if ((Boolean)parameterArray[i] == false)
                            parameterArray[i] = "0";

                        if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                        {

                            Query = Query + " ' ', ";
                        }
                        else
                        {
                            Query = Query + parameterArray[i] + ", ";
                        }

                        break;


                    case "Int32":
                        //Regex r = new Regex("^[0-9]*$");
                        //Regex reg1 = new Regex("^[a-zA-Z0-9_]*$");
                          Regex r;
                        
                        if(dtTable.Columns[i].ColumnName == "Score")
                            r = new Regex("^[1-4]*$");
                        else
                            r = new Regex("^[0-9]*$");

                        Regex reg1 = new Regex("^[a-zA-Z0-9_]*$");
                        if (colname.Length < 1 && dtTable.Columns[i].ColumnName == "Score")
                        {
                            errorMsg = "Please enter numeric value in " + dtTable.Columns[i];
                            Flag = 1;
                            Query = string.Empty;
                            break;
                        }

                        else if (!r.IsMatch(parameterArray[i].ToString()))
                        {
                            errorMsg = "Please enter numeric values between 1-4 in " + dtTable.Columns[i];
                            Flag = 1;
                            break;
                        }
                        else if ((dtSchema != null) && (dtSchema.Rows.Count > 0))
                        {
                            if (dtTable.Columns[i].ToString() == dtSchema.Rows[i].ItemArray[0].ToString())
                            {
                                if ((dtSchema.Rows[i]["AllowDBNull"].ToString() == "False") && (string.IsNullOrEmpty(parameterArray[i].ToString())))
                                {
                                    errorMsg = "Please enter value in " + dtTable.Columns[i];
                                    Flag = 1;
                                    break;
                                }

                            }


                        }


                        if (!reg1.IsMatch(parameterArray[i].ToString()))
                        {
                            //lblmessage.Text = "Please enter alphanumeric values in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Please enter alphanumeric values in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Please enter alphanumeric values in " + dtTable.Columns[i];
                            Flag = 1;
                            break;
                        }



                        if ((string)parameterArray[i] == "True")
                            parameterArray[i] = "1";
                        else if ((string)parameterArray[i] == "False")
                            parameterArray[i] = "0";

                        //if (i == dtTable.Columns.Count - 1)
                        //{
                        //    //if (Table.Columns[i].AllowDBNull == true)
                        //    //{
                        //    //    Query = Query + Convert.(ParameterArray[i]) ;
                        //    //}
                        //    //else
                        //    //{
                        //        Query = Query + ParameterArray[i] + "";
                        //   // }
                        //}
                        //else

                        //    if (string.IsNullOrEmpty(ParameterArray[i].ToString()))
                        //    {
                        //        Query = string.Empty;
                        //    }
                        //    else
                        //    {
                        //        Query = Query + ParameterArray[i] + "', ";
                        //    }

                        //break;

                        if (i == dtTable.Columns.Count - 3)
                            //if (string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            //{
                            //    Query = Query + " ' ' ";
                            //}
                            //else
                            //{
                            //  Query = Query + ParameterArray[i] + "";
                            //}

                            Query = Query + userID + ", ";
                        else
                            if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {

                                Query = Query + " ' ', ";
                            }
                            else
                            {
                                Query = Query + parameterArray[i] + ", ";
                            }
                        break;

                    case "String":
                        Regex reg2 = new Regex("^[a-zA-Z0-9_\\s\\.\\#\\+\\[\\]\\,\\(\\)]*$");
                        //Regex reg2 = new Regex("^[a-zA-Z0-9_\\s\\.\\(\\)]*$");



                        //if (dtTable.Columns[i].AllowDBNull == true )
                        //if ((dtTable.Columns[i].AllowDBNull == false) )    
                        //   {
                        //       lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                        //       Flag = 1;
                        //       break;
                        //   }


                        //  string colname2 = parameterArray[i].ToString().Trim() ;

                        if (colname.Length < 1)
                        {
                            errorMsg = "Please enter some text ";
                            Flag = 1;
                            Query = string.Empty;
                            break;
                        }



                        if ((dtSchema != null) && (dtSchema.Rows.Count > 0))
                        {
                            if (dtTable.Columns[i].ToString() == dtSchema.Rows[i].ItemArray[0].ToString())
                            {
                                if ((dtSchema.Rows[i]["AllowDBNull"].ToString() == "False") && (string.IsNullOrEmpty(parameterArray[i].ToString())))
                                {
                                    //lblmessage.Text = "Please enter value in " + dtTable.Columns[i];
                                    //System.Windows.Forms.MessageBox.Show("Please enter value in " + dtTable.Columns[i], "Recruitment module");
                                    errorMsg = "Please enter value in " + dtTable.Columns[i];
                                    Flag = 1;
                                    break;
                                }

                            }


                        }


                        //else if (colMaxLenth[i - 1] == 1)
                        //{
                        //    if ((!(ParameterArray[i].ToString().StartsWith("F")) || (ParameterArray[i].ToString().StartsWith("f")) || (ParameterArray[i].ToString().StartsWith("M")) || (ParameterArray[i].ToString().StartsWith("m"))) && (dtTable.Columns[i].AllowDBNull==true ))
                        //    {
                        //        lblmessage.Text = "Please enter M/F character value in " + dtTable.Columns[i];
                        //        Flag = 1;
                        //        break;
                        //    }
                        //}
                        if (!reg2.IsMatch(parameterArray[i].ToString()))
                        {
                            //lblmessage.Text = "Please enter alphanumeric values in" + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Please enter alphanumeric values in" + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Please enter alphanumeric values in  " + dtTable.Columns[i];
                            Flag = 1;
                            break;
                        }
                        string colname1 = parameterArray[i].ToString();
                        if (colname1.Length > colMaxLenth[i])
                        {
                            // lblmessage.Text = "Entered character is too long in " + dtTable.Columns[i];
                            //System.Windows.Forms.MessageBox.Show("Entered character is too long in " + dtTable.Columns[i], "Recruitment module");
                            errorMsg = "Entered character is too long in " + dtTable.Columns[i];
                            Flag = 1;
                            break;
                        }


                        if ((string)parameterArray[i] == "True")
                            parameterArray[i] = "1";
                        else if ((string)parameterArray[i] == "False")
                            parameterArray[i] = "0";



                        if (i == dtTable.Columns.Count - 1)
                        {
                            if (!string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {
                                Query = Query + "'" + parameterArray[i] + "'";
                            }
                            else
                            {
                                Query = Query + "' '";
                            }

                        }
                        else

                            if (string.IsNullOrEmpty(parameterArray[i].ToString()))
                            {
                                while (string.IsNullOrEmpty(parameterArray[i].ToString()))
                                {
                                    //System.Windows.Forms.MessageBox.Show("Please enter value in the " + dtTable.Columns[i] + " column.", "Recruitment module");
                                    errorMsg = "Please enter value in the " + dtTable.Columns[i] + " column.";
                                    Flag = 1;
                                    break;

                                }
                                Query = Query + "' ', ";
                            }
                            else
                            {
                                Query = Query + "'" + parameterArray[i] + "', ";
                            }
                        break;

                    case "DateTime":


                        if (((string)parameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)parameterArray[i]);
                            parameterArray[i] = ((string)parameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }
                        if (i == dtTable.Columns.Count - 1)
                            Query = Query + "'" + DateTime.Now.ToString() + "' ";
                        else
                            //Query = Query + "'" + DateTime.Now.ToString() + "', ";
                            Query = Query + "'" + DateTime.Now.ToString() + "' ";
                        break;

                    default:
                        break;

                }

            }
            if (Flag == 1)
            {
                Query = "";
                return Query;

            }
            else
            {
                Query = Query + "";
                return Query;
            }


        }

        public string GenerateDeleteQuery(int index, DataTable dtTable)
        {
            string query = string.Empty;
            if (dtTable.Columns[0].DataType.Name == "String" || dtTable.Columns[0].DataType.Name == "DateTime")
                query = dtTable.Columns[0].ColumnName + "='" + dtTable.Rows[index][0].ToString() + "'";
            else
                query = dtTable.Columns[0].ColumnName + "=" + dtTable.Rows[index][0].ToString();


            return query;

        }
        public int[] checkColumnLength(string tableName)
        {

            DataSet Ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand("sp_GetMaxDatatypeLenth", myConnection);
            myCommand.Parameters.AddWithValue("@TableName", "'" + tableName + "'");
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(Ds);

            myConnection.Open();

            int[] colMaxLenth = new int[Ds.Tables[0].Rows.Count];

            for (int z = 0, x = 0; z < Ds.Tables[0].Rows.Count && x < Ds.Tables[0].Rows.Count; z++, x++)
            {
                if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[x][Ds.Tables[0].Columns[0].ColumnName].ToString()))
                {
                    colMaxLenth[z] = Convert.ToInt32(Ds.Tables[0].Rows[x][Ds.Tables[0].Columns[0].ColumnName]);

                }
            }

            return colMaxLenth;
        }


        public string MasterTableUpdate(string tableName, DataTable dtTable, ArrayList parameterArray, int userID, string Row_value)
        {
            //bool Updateresult = false;
            string errorCheck = "";
            string UpdateStringQuery = string.Empty;

            UpdateStringQuery = GenerateUpdateQuery(tableName, dtTable, parameterArray, userID, Row_value);

            if (!string.IsNullOrEmpty(UpdateStringQuery))
            {
                SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
                using (SqlConnection myConnection = new SqlConnection(AppConfiguration.ConnectionString))
                {
                    SqlCommand myCommand = new SqlCommand("sp_AddUpdateMaster", myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@Operation", "UPDATE");
                    myCommand.Parameters.AddWithValue("@TableName", tableName);
                    if (!string.IsNullOrEmpty(UpdateStringQuery))
                    {
                        myCommand.Parameters.AddWithValue("@values", UpdateStringQuery);
                        myConnection.Open();
                        myCommand.ExecuteNonQuery();
                        myConnection.Close();
                        //Updateresult = true;
                    }

                }
                return errorCheck;
            }
            else
            {
                errorCheck = errorMsg;
                return errorCheck;
            }




        }


        public DataTable GetSchemaDetails(string tableName)
        {
            DataTable DT = new DataTable();
            using (SqlConnection myConnection = new SqlConnection(AppConfiguration.ConnectionString))
            {
                SqlCommand myCommand = new SqlCommand("sp_GetTableName", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@TableName", tableName);

                myConnection.Open();
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {

                    DT = myReader.GetSchemaTable();
                    myReader.Close();

                    myReader.Close();
                }
                myConnection.Close();
            }
            return DT;
        }




        public DataSet GetMasterTableName()
        {
            DataSet Ds = new DataSet();
            using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                SqlDataAdapter sqlCommand = new SqlDataAdapter("sp_GetMasterTableList", myConnection);
                sqlCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                myConnection.Open();

                sqlCommand.Fill(Ds);
                sqlCommand.Dispose();
                myConnection.Close();

            }
            return Ds;

        }
        public DataSet populateDatatable(string tableName)
        {
            DataSet Ds = new DataSet();

            SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            SqlDataAdapter sqlCommand = new SqlDataAdapter("sp_GetTableName", Connection);
            sqlCommand.SelectCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar));
            sqlCommand.SelectCommand.Parameters["@TableName"].Value = tableName;
            sqlCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            Connection.Open();

            sqlCommand.Fill(Ds);
            Connection.Close();
            sqlCommand.Dispose();
            return Ds;
        }


        public DataSet getUserRole(int userId)
        {
            DataSet Ds = new DataSet();

            SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            SqlDataAdapter sqlCommand = new SqlDataAdapter("sp_GetRole", Connection);
            sqlCommand.SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
            sqlCommand.SelectCommand.Parameters["@UserId"].Value = userId;
            sqlCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            Connection.Open();

            sqlCommand.Fill(Ds);
            Connection.Close();
            sqlCommand.Dispose();
            return Ds;
        }

    }

}
