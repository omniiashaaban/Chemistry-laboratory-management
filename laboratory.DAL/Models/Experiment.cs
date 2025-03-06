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
        public int DepartmentId { get; set; } // مفتاح خارجي للمجموعة
        public Department Department { get; set; } // العلاقة مع المجموعة
        public string PdfFilePath { get; set; } = "";
        public ICollection<ExperimentMaterial> ExperimentMaterials { get; set; } // المواد المستخدمة في التجربة
    }
}