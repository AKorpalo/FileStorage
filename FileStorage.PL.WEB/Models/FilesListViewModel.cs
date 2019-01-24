using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileStorage.BLL.DTO;

namespace FileStorage.PL.WEB.Models
{
    public class FilesListViewModel
    {
        public IEnumerable<FileInfoDTO> Files { get; set; }
        public int Pages { get; set; }
        public string SearchString { get; set; }
    }
}