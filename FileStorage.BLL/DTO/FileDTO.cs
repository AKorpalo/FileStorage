using System;
using System.IO;


namespace FileStorage.BLL.DTO
{
    public class FileDTO
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DownloadDate { get; set; }
        public string FilePath { get; set; }
        public string RelativePath { get; set; }
        public string UserId { get; set; }
        public Stream InputStream { get; set; }
    }
}
