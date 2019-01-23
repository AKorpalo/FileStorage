using System;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.BLL.DTO
{
    public class RegisterDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Введіть логін")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}", ErrorMessage = "Некоректний формат адреси")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
