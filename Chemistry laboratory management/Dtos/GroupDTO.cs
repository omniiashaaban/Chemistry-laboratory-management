using laboratory.DAL.Models;

namespace Chemistry_laboratory_management.Dtos
{
    public class GroupDTO
    {
        public string Name { get; set; } // اسم المجموعة مثل "Group 1A"
        public int Level { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
    }
}
