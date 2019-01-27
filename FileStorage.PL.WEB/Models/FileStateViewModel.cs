using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using App_LocalResources;

namespace FileStorage.PL.WEB.Models
{
    public class FileStateViewModel
    {
        public string Id { get; set; }

        [Display(Name = "IsPrivate", ResourceType = typeof(Resource))]
        public bool IsPrivate { get; set; }
    }
}