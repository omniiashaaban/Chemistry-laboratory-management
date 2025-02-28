using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public Department Department { get; set; }
        public int Level { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }



    }

}
