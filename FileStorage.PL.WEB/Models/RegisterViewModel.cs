using System;
using System.ComponentModel.DataAnnotations;
using App_LocalResources;

namespace FileStorage.PL.WEB.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "Введіть логін")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}", ErrorMessage = "Некоректний формат адреси")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = "SecondName", ResourceType = typeof(Resource))]
        public string SecondName { get; set; }

        [Display(Name = "BirthDate", ResourceType = typeof(Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime BirthDate { get; set; }
    }
}