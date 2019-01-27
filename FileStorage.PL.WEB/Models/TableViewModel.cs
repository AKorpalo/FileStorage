using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileStorage.BLL.DTO;

namespace FileStorage.PL.WEB.Models
{
    public class TableViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Pages { get; set; }
        public string SearchString { get; set; }
    }
}