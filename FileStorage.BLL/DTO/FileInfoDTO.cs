using System;
using System.ComponentModel;
using System.IO;

namespace FileStorage.BLL.DTO
{
    public class FileInfoDTO
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public double Size { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DownloadDate { get; set; }
        public string RelativePath { get; set; }
        public string FilePath { get; set; }
        [DisplayName("User")]
        public string UserId { get; set; }

        public Stream InputStream { get; set; }
    }
}
