using System;

namespace HRMS.DAL
{
    public interface IDocuments
    {
        int DocumentId { get; set; }

        string FileName { get; set; }

        string FileDescription { get; set; }

        string FilePath { get; set; }

        string Comments { get; set; }

        DateTime? UploadedDate { get; set; }

        Int32? UploadedBy { get; set; }

        bool IsParent { get; }
    }
}