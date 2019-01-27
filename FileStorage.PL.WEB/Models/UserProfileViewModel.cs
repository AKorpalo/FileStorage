using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using App_LocalResources;

namespace FileStorage.PL.WEB.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = "SecondName", ResourceType = typeof(Resource))]
        public string SecondName { get; set; }

        [Display(Name = "MaxSize", ResourceType = typeof(Resource))]
        public double MaxSize { get; set; }

        [Display(Name = "BirthDate", ResourceType = typeof(Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }
    }
}