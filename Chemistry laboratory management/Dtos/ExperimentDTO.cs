namespace Chemistry_laboratory_management.Dtos
{
    public class ExperimentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SafetyInstruction { get; set; }
        public string PdfFilePath { get; set; }
        public int Level { get; set; }
        public int DepartmentId { get; set; }
    }

    public class AddExperimentDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SafetyInstruction { get; set; }

        public int Level { get; set; }
        public int DepartmentId { get; set; }
        public int id { get; set; }
        public List<ExperimentMaterialDTO> Materials { get; set; }
    }

    public class ExperimentMaterialDTO
    {
        public int MaterialId { get; set; }
        public int QuantityRequired { get; set; }
    }

    public class ExperimentResponseDTO
    {
        public bool Success { get; set; }
        public int? ExperimentId { get; set; }
        public string SafetyInstruction { get; set; }
    }
}
