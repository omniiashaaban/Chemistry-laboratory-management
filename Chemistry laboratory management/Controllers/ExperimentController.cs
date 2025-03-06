using Chemistry_laboratory_management.Dtos;
using laboratory.BLL.Services;
using laboratory.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chemistry_laboratory_management.Helper;
using laboratory.BLL.Services.laboratory.BLL.Services;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        private readonly IExperimentService _experimentService;
        private readonly MaterialService _materialService; // Assuming MaterialService for material management
        private readonly IWebHostEnvironment _env;

        // Inject IWebHostEnvironment to handle file paths
        public ExperimentController(IExperimentService experimentService, MaterialService materialService, IWebHostEnvironment env)
        {
            _experimentService = experimentService;
            _materialService = materialService;
            _env = env;
        }

        #region Experiment Operations

        // Endpoint to add a new experiment
        [HttpPost("add")]
        public async Task<ActionResult<ExperimentResponseDTO>> AddExperiment([FromBody] AddExperimentDTO addExperimentDTO)
        {
            // Validate material availability before creating the experiment
            foreach (var material in addExperimentDTO.Materials)
            {
                var isAvailable = await _materialService.CheckMaterialAvailability(material.MaterialId, material.QuantityRequired);
                if (!isAvailable)
                {
                    return BadRequest($"Material with ID {material.MaterialId} is not available in the required quantity.");
                }

                // Decrease the material stock after validation
                await _materialService.DecreaseMaterialStock(material.MaterialId, material.QuantityRequired);
            }

            // Create the experiment object
            var experiment = new Experiment
            {
                Name = addExperimentDTO.Name,
                Type = addExperimentDTO.Type,
                SafetyInstruction = addExperimentDTO.SafetyInstruction,
                Level = addExperimentDTO.Level,
                DepartmentId=addExperimentDTO.DepartmentId
                
                
            };

            await _experimentService.AddExperimentAsync(experiment);
            // Return a success response with the experiment ID
            return Ok(new ExperimentResponseDTO
            {
                Success = true,
                ExperimentId = experiment.Id
            });
        }


        // Endpoint to upload a PDF for an experiment
        [HttpPost("upload-pdf/{experimentId}")]
        public async Task<ActionResult<string>> UploadPdf(int experimentId, IFormFile file)
        {
            // Attempt to upload the PDF file
            var result = await _experimentService.UploadPdfAsync(experimentId, file);

            if (result)
            {
                // Return a success response with the file URL
                return Ok(new { Message = "PDF uploaded successfully", FileUrl = $"/Experiments/{file.FileName}" });
            }

            // Return a failure response if the upload fails
            return BadRequest("Failed to upload PDF.");
        }

        // Endpoint to get experiment details by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ExperimentDTO>> GetExperimentById(int id)
        {
            var experiment = await _experimentService.GetExperimentByIdAsync(id);
            if (experiment == null)
            {
                return NotFound("Experiment not found.");
            }

            var experimentDTO = new ExperimentDTO
            {
                Id = experiment.Id,
                Name = experiment.Name,
                Type = experiment.Type,
                SafetyInstruction = experiment.SafetyInstruction,
                PdfFilePath = experiment.PdfFilePath,
                Level = experiment.Level,
                DepartmentId = experiment.DepartmentId
            };

            return Ok(experimentDTO);
        }

        // Endpoint to get all experiments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExperimentDTO>>> GetAllExperiments()
        {
            var experiments = await _experimentService.GetAllExperimentsAsync();
            var experimentDTOs = new List<ExperimentDTO>();

            foreach (var experiment in experiments)
            {
                experimentDTOs.Add(new ExperimentDTO
                {
                    Id = experiment.Id,
                    Name = experiment.Name,
                    Type = experiment.Type,
                    SafetyInstruction = experiment.SafetyInstruction,
                    PdfFilePath = experiment.PdfFilePath,
                    Level = experiment.Level,
                    DepartmentId = experiment.DepartmentId
                });
            }

            return Ok(experimentDTOs);
        }

        // Endpoint to download a PDF for an experiment
        [HttpGet("download-pdf/{experimentId}")]
        public async Task<IActionResult> DownloadPdf(int experimentId)
        {
            // Get the experiment by ID
            var experiment = await _experimentService.GetExperimentByIdAsync(experimentId);
            if (experiment == null || string.IsNullOrEmpty(experiment.PdfFilePath))
            {
                return NotFound("Experiment PDF not found.");
            }

            // Construct the file path from the stored PDF file path
            var filePath = Path.Combine(_env.WebRootPath, experiment.PdfFilePath.TrimStart('/'));

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("PDF file not found on the server.");
            }

            // Get the file bytes for download
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            // Return the file with content type 'application/pdf'
            return File(fileBytes, "application/pdf", Path.GetFileName(filePath));
        }

        #endregion
    }
}
