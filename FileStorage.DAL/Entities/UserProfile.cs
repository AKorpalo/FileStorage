using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
        public double CurrentSize { get; set; }
        public double MaxSize { get; set; }
        public DateTime RegisterDate { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
