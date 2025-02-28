using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly GenericRepository<Request> _requestRepository;
        private readonly GenericRepository<Doctor> _doctorRepository;
        private readonly GenericRepository<Experiment> _experimentRepository;

        public RequestController(
            GenericRepository<Request> requestRepository,
            GenericRepository<Doctor> doctorRepository,
            GenericRepository<Experiment> experimentRepository)
        {
            _requestRepository = requestRepository;
            _doctorRepository = doctorRepository;
            _experimentRepository = experimentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _requestRepository.GetAllAsync();

            var requestDTOs = requests.Select(request => new RequestDTO
            {
                Id = request.Id,
                DoctorId = request.DoctorId,
                ExperimentName = request.ExperimentName,
                Status = request.Status,
                Level = request.Level,
                Group = request.Group
            }).ToList();

            return Ok(requestDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest(new ApiResponse(400)); 
            }


            var doctors = await _doctorRepository.GetAllAsync();
            bool doctorExists = doctors.Any(d => d.Id == requestDTO.DoctorId);
            if (!doctorExists)
            {
                return NotFound(new ApiResponse(404, "Doctor not found."));
            }



            var experiments = await _experimentRepository.GetAllAsync();
            bool experimentExists = experiments.Any(e => e.Name == requestDTO.ExperimentName);
            if (!experimentExists)
            {
                return NotFound(new ApiResponse(404, "Experiment not found."));
            }

            var request = new Request
            {
                DoctorId = requestDTO.DoctorId,
                ExperimentName = requestDTO.ExperimentName,
                Status = requestDTO.Status,
                Level = requestDTO.Level,
                Group = requestDTO.Group
            };

            await _requestRepository.AddAsync(request);

            return Created("", new RequestDTO
            {
                Id = request.Id,
                DoctorId = request.DoctorId,
                ExperimentName = request.ExperimentName,
                Status = request.Status,
                Level = request.Level,
                Group = request.Group
            });
        }
    }
}