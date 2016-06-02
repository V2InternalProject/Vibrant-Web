using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;


namespace V2.Orbit.DataLayer
{
    [Serializable]
    public abstract class DBBaseClass
    {
        protected string ConnectionString = "";
        protected string LocalSqlServer = "";

        //protected string V2toolsDBEntity = "";

        #region DBBaseClass
        public DBBaseClass()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            LocalSqlServer = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

            //V2toolsDBEntity = ConfigurationManager.AppSettings["V2toolsDBEntity"].ToString();
        }
        #endregion


    }


    //public class CustomSqlMembershipProvider : SqlMembershipProvider
    //{
    //    public override void Initialize(string name, NameValueCollection config)
    //    {
    //        string connStringName; //= // retrieve from wherever you please...         
    //        config["connectionStringName"] = connStringName;
    //        // base.Initialize(name, config);     
    //    }
    //}
}
