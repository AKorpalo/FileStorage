using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [DisplayName("User")]
        public string UserId { get; set; }
    }
}
