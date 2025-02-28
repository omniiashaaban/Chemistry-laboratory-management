using System.ComponentModel.DataAnnotations;

namespace laboratory.DAL.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Experiment> Experiments { get; set; } = new List<Experiment>();
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }

}
