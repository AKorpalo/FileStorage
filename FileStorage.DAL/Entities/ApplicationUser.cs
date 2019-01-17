using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FileStorage.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<FileData> Files { get; set; }

        //public ApplicationUser() : base()
        //{
        //    Files = new List<FileData>();
        //}
    }
}
