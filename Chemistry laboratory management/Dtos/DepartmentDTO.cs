using System.ComponentModel.DataAnnotations;

namespace Chemistry_laboratory_management.Dtos
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
