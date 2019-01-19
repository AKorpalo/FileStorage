using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.PL.WEB.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [DisplayName("Електрона адреса")]
        public string Email { get; set; }

        [DisplayName("Логін")]
        public string UserName { get; set; }

        [DisplayName("Ім'я")]
        public string FirstName { get; set; }

        [DisplayName("Призвіще")]
        public string SecondName { get; set; }

        [DisplayName("Дата Народження")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Поточний розмір сховища")]
        public double CurrentSize { get; set; }

        [DisplayName("Максимальний розмір сховища")]
        public double MaxSize { get; set; }

    }
}