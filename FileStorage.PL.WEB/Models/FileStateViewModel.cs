using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FileStorage.PL.WEB.Models
{
    public class FileStateViewModel
    {
        public string Id { get; set; }
        [DisplayName("Статус файлу")]
        public bool IsPrivate { get; set; }
    }
}