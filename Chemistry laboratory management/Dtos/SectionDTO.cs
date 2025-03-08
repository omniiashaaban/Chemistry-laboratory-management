using laboratory.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chemistry_laboratory_management.Dtos
{
    public class sectionDTO
    {
        public int DoctorId { get; set; }
        public int Id { get; set; }
        public String ExperimentName { get; set; }
        public int Level { get; set; }
        public int GroupId { get; set; }
        public int ExperimentId { get; set; }

    }
}
