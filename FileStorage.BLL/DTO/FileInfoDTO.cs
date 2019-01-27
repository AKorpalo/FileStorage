using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using LocalResources;

namespace FileStorage.BLL.DTO
{
    public class FileInfoDTO
    {
        public string Id { get; set; }

        [Display(Name = "FileName", ResourceType = typeof(Resource))]
        public string FileName { get; set; }
        [Display(Name = "Size", ResourceType = typeof(Resource))]
        public double Size { get; set; }
        [Display(Name = "IsPrivate", ResourceType = typeof(Resource))]
        public bool IsPrivate { get; set; }
        [Display(Name = "DownloadDate", ResourceType = typeof(Resource))]
        public DateTime DownloadDate { get; set; }
        public string RelativePath { get; set; }
        public string FilePath { get; set; }
        [Display(Name = "User", ResourceType = typeof(Resource))]
        [DisplayName("User")]
        public string UserId { get; set; }

        public Stream InputStream { get; set; }
    }
}
