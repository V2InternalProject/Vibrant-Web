using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using V2.Orbit.Model;
using V2.Orbit.DataLayer;

namespace V2.Orbit.BusinessLayer
{
  public  class SalaryReportBOL
    {
      SalaryReportDAL objSalaryReportDAL = new SalaryReportDAL();

      public DataSet getSalaryReport(SalaryReportModel objSalaryReportModel)
        {
            return objSalaryReportDAL.getSalaryReport(objSalaryReportModel);
        }
    }
}
