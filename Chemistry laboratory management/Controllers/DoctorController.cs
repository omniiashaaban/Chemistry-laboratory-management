using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Data.context;
using laboratory.DAL.Migrations;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly GenericRepository<Doctor> _doctorRepository;
        private readonly GenericRepository<Department> _departmentRepository;
        public DoctorController(GenericRepository<Doctor> doctorRepositor, GenericRepository<Department> departmentRepository)
        {
            _doctorRepository = doctorRepositor;
            _departmentRepository = departmentRepository;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            var doctorDTO = new DoctorDTO
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                DepartmentName = doctor.DepartmentName
            };

            return Ok(doctorDTO);
        }
  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var doctorDTOs = new List<DoctorDTO>();

            foreach (var doctor in doctors)
            {
                doctorDTOs.Add(new DoctorDTO
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Email = doctor.Email,
                    DepartmentName= doctor.DepartmentName
                });
            }

            return Ok(doctorDTOs);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorDTO>> CreateDoctor([FromBody] DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
            {
                return BadRequest("Invalid doctor data.");
            }
            var departement = await _departmentRepository.GetAllAsync();
            bool departmentExists = departement.Any(d => d.Name == doctorDTO.DepartmentName);
            if (!departmentExists)
            {
                return NotFound(new ApiResponse(404, "Department not found."));
            }

            var doctor = new Doctor
            {

                FirstName = doctorDTO.FirstName,
                LastName = doctorDTO.LastName,             
                 Email = doctorDTO.Email,
                DepartmentName = doctorDTO.DepartmentName
            };

            await _doctorRepository.AddAsync(doctor);

            doctorDTO.Id =doctor.Id; 

            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctorDTO);
        }
    }
}