using System;
using System.ComponentModel;

namespace FileStorage.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
        public double CurrentSize { get; set; }
        public double MaxSize { get; set; }

    }
}
