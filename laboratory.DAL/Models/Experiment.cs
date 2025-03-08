using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Models
{
    public class Experiment
    {
        public int Id { get; set; }
        public string Name { get; set; } // اسم التجربة
        public string Type { get; set; } // نوع التجربة
        public string SafetyInstruction { get; set; } // رسالة الخطأ
        public int Level { get; set; }

        public string PdfFilePath { get; set; } = "";
        public ICollection<ExperimentMaterial> ExperimentMaterials { get; set; } = new List<ExperimentMaterial>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Section> Sections { get; set; }= new List<Section>();
    }
}