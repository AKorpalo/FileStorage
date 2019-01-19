using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileStorage.PL.WEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть логін")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}