using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
      
        public Doctor Doctor { get; set; }
        [ForeignKey("ExperimentId")]
        public int ExperimentId { get; set; }
        public int Level { get; set; }
        public int Group { get; set; }


        public Experiment Experiment { get; set; }

        public string Status { get; set; } // Pending, Approved, Rejected

        public ICollection<RequestChemical> RequestChemicals { get; set; } = new List<RequestChemical>();
        public string ExperimentName { get; set; }
    }

}
