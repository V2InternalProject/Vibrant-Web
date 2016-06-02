using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class InterviewerBLL
    {
        DataSet dsInterveiwer = new DataSet();
        InterviewerDAL objInterviewerDAL = new InterviewerDAL();
        InterviewerBOL objInterviewerBOL = new InterviewerBOL();

        public DataSet GetInterviewDetails(InterviewerBOL objInterviewerBOL)
        {
            dsInterveiwer = objInterviewerDAL.GetInterviewDetails(objInterviewerBOL);
            return dsInterveiwer;
        }
    }
}
