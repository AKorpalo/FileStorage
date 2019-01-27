using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using App_LocalResources;

namespace FileStorage.PL.WEB.Models
{
    public class LoginViewModel
    {
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "Введіть логін")]
        public string Login { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}