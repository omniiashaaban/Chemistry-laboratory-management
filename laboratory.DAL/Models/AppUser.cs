using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        // علاقات مع Doctor, LabAdmin, Student
        public Doctor Doctor { get; set; }
        public LabAdmin LabAdmin { get; set; }
        public Student Student { get; set; }
    }
}
