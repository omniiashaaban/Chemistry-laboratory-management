using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class RequestChemical
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public int RequestId { get; set; }
  
        public Request Request { get; set; }
        [ForeignKey("MaterialId ")]
        public int MaterialId { get; set; }
        
        public Material Material { get; set; }

        public double RequestedQuantity { get; set; } 
    }

}
