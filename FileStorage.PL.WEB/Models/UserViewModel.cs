using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using App_LocalResources;

namespace FileStorage.PL.WEB.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = "SecondName", ResourceType = typeof(Resource))]
        public string SecondName { get; set; }

        [Display(Name = "BirthDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "CurrentSize", ResourceType = typeof(Resource))]
        public double CurrentSize { get; set; }

        [Display(Name = "MaxSize", ResourceType = typeof(Resource))]
        public double MaxSize { get; set; }

    }
}