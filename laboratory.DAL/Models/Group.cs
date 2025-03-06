using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } // اسم المجموعة مثل "Group 1A"
        public int Level { get; set; }
        public ICollection<Student> Students { get; set; }



    }

}
