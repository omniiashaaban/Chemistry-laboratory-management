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
        public string Name { get; set; } // اسم المجموعة مثل "Group A"

        public ICollection<Student> Students { get; set; } // الطلاب في هذه المجموعة
        public ICollection<Experiment> Experiments { get; set; } // التجارب الخاصة بالمجموعة
    }

}
