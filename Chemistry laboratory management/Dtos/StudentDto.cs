using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using laboratory.DAL.Models;

namespace Chemistry_laboratory_management.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
    }
}
