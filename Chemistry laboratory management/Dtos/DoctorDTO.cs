using System.ComponentModel.DataAnnotations;

namespace Chemistry_laboratory_management.Dtos
{
    public class DoctorDTO
    {
        public int Id{ get; set; }
      

        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<int> GroupIds { get; set; }
    }
}
