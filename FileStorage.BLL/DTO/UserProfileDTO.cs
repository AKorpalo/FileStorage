using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.BLL.DTO
{
    public class UserProfileDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public double MaxSize { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
