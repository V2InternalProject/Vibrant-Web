using System.Collections.Generic;

namespace HRMS.DAL
{
    public class SavePersonalDetailsResponse
    {
        public int EmployeeId { get; set; }

        public bool IsAdded { get; set; }

        public List<string> ColumnNames { get; set; }

        public List<string> FieldLabels { get; set; }

        public List<string> NewValues { get; set; }
    }
}