using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
   public class LabAdmin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}
