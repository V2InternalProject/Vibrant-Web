using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using DAL;
using System.Data;
using V2.Orbit.Workflow.Activities.MailActivity;

namespace BLL
{
    public class EmailActivityBLL
    {
        DataSet dsEmailActivityBLL = new DataSet();
        EmailActivityDAL objEmailActivityDAL = new EmailActivityDAL();

        public DataSet GetMailInfo(EmailActivityBOL objEmailActivityBOL)
        {
            return dsEmailActivityBLL = objEmailActivityDAL.GetMailInfo(objEmailActivityBOL);
        }
    }
}
