using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.DTO
{
    public class FileDownloadDTO
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public OperationDetails IsDownload { get; set; }
    }
}
