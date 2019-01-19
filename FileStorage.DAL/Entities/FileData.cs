using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileStorage.DAL.Entities
{
    public class FileData
    {
        [Key]
        public string Id { get; set; }
        public string FileName { get; set; }
        public double Size { get; set; }
        public DateTime DownloadDate { get; set; }
        public bool IsPrivate { get; set; }
        public string FilePath { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
