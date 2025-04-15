using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        #region Ctor
        private readonly GenericRepository<Section> _sectionRepository;
        private readonly GenericRepository<Doctor> _doctorRepository;
        private readonly GenericRepository<Experiment> _experimentRepository;
        private readonly GenericRepository<Student> _studentRepository;

        public SectionController(
            GenericRepository<Section> sectionRepository,
            GenericRepository<Doctor> doctorRepository,
            GenericRepository<Experiment> experimentRepository,
            GenericRepository<Student> studentRepository)
        {
            _sectionRepository = sectionRepository;
            _doctorRepository = doctorRepository;
            _experimentRepository = experimentRepository;
            _studentRepository = studentRepository;
        } 
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllSections()
        {
            var sections = await _sectionRepository.GetAllAsync();

            var sectionDTOs = sections.Select(section => new sectionDTO
            {
                Id = section.Id,
                DoctorId = section.DoctorId,
                ExperimentName = section.Experiment.Name,
                Level = section.Level,
                GroupId = section.GroupId
            }).ToList();
            

            return Ok(sectionDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);

            if (section == null)
                return NotFound(new ApiResponse(404, "Section not found."));

            var sectionDTO = new sectionDTO
            {
                Id = section.Id,
                DoctorId = section.DoctorId,
                ExperimentName = section.Experiment.Name,
                Level = section.Level,
                GroupId = section.GroupId
            };

            return Ok(new ApiResponse(200, "Section retrieved successfully.", sectionDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] sectionDTO sectionDTO)
        {
            if (sectionDTO == null)
                return BadRequest(new ApiResponse(400, "Invalid request data."));

            var doctorExists = (await _doctorRepository.GetAllAsync()).Any(d => d.Id == sectionDTO.DoctorId);
            if (!doctorExists)
                return NotFound(new ApiResponse(404, "Doctor not found."));

            var experimentExists = (await _experimentRepository.GetAllAsync()).Any(e => e.Id == sectionDTO.ExperimentId);
            if (!experimentExists)
                return NotFound(new ApiResponse(404, "Experiment not found."));

            var section = new Section
            {
                DoctorId = sectionDTO.DoctorId,
                GroupId = sectionDTO.GroupId,
                ExperimentId = sectionDTO.ExperimentId,
                Level = sectionDTO.Level
            };

            await _sectionRepository.AddAsync(section);

            var createdSection = new sectionDTO
            {
                Id = section.Id,
                DoctorId = section.DoctorId,
                ExperimentId = section.Experiment.Id,
                Level = section.Level,
                GroupId = section.GroupId
            };

            return Created("", new ApiResponse(201, "Section created successfully.", createdSection));
        }
        [HttpPost("generate-code/{sectionId}")]
        public async Task<IActionResult> GenerateAttendanceCode(int sectionId)
        {
            var section = await _sectionRepository.GetByIdAsync(sectionId);

            if (section == null)
                return NotFound(new ApiResponse(404, "Section not found."));

            section.AttendanceCode = new Random().Next(100000, 999999).ToString();

            // فرق التوقيت لمصر UTC+2 أو +3
            DateTime expiryUtc = DateTime.UtcNow.AddSeconds(10);
            DateTime expiryEgypt = expiryUtc.AddHours(2); // أو 3 لو التوقيت الصيفي شغّال

            section.CodeExpiry = expiryEgypt;

            await _sectionRepository.UpdateAsync(section);

            return Ok(new { Code = section.AttendanceCode, Expiry = section.CodeExpiry });
        }


        [HttpPost("{sectionId}/attendance/{studentId}")]
        public async Task<IActionResult> MarkAttendance(int sectionId, int studentId, [FromBody] string attendanceCode)
        {
            var section = await _sectionRepository.GetByIdAsync(sectionId);
            if (section == null)
                return NotFound(new ApiResponse(404, "Section not found."));

            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                return NotFound(new ApiResponse(404, "Student not found."));

            if (section.AttendanceCode != attendanceCode)
                return BadRequest(new ApiResponse(400, "Invalid attendance code."));

            if (DateTime.UtcNow > section.CodeExpiry)
                return BadRequest(new ApiResponse(400, "Attendance code has expired."));

            if (student.GroupId != section.GroupId)
                return BadRequest(new ApiResponse(400, "Student is not in the same group as the section."));

            if (section.AttendanceRecords.ContainsKey(studentId))
                return BadRequest(new ApiResponse(400, "Attendance already recorded for this student."));

            section.AttendanceRecords[studentId] = true;
            await _sectionRepository.UpdateAsync(section);

            return Ok(new ApiResponse(200, "Attendance recorded successfully."));
        }

        [HttpGet("{sectionId}/attendance")]
        public async Task<IActionResult> GetAttendance(int sectionId)
        {
            var section = await _sectionRepository.GetByIdAsync(sectionId);
            if (section == null)
                return NotFound(new ApiResponse(404, "Section not found."));

            var attendanceList = section.AttendanceRecords
                .Select(a => new { StudentId = a.Key, IsPresent = a.Value })
                .ToList();

            return Ok(attendanceList);
        }
    }
}