using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using V2.CommonServices;

/// <summary>
/// Summary description for LeaveTransactionAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class LeaveTransactionAutoComplete : System.Web.Services.WebService
{

    public LeaveTransactionAutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    protected string ConnectionString = "";


    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public string[] GetEmployeeName(string prefixText)
    {
        
        ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        
        //string connString = "Database=OrbitPhase2;Server=db-server;Integrated Security=false;User Id=g1gdev;password=g1gdev;";
        //SqlConnection conn = new SqlConnection(connString);
        //try
        //{
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("EmployeeName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@prefixText", SqlDbType.VarChar, 100).Value = prefixText;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["UserID"].ToString() + " " + dr["EmployeeName"].ToString(), i);
                i++;
            }
            return items;
        //}


        //catch (System.Exception ex)
        //{
        //    throw new V2.CommonServices.Exceptions.V2Exceptions(ex.ToString(),ex);
        //}

    }
}

