using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Data.context;
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
        private readonly GenericRepository<Section> _sectionRepository;
        private readonly GenericRepository<Doctor> _doctorRepository;
        private readonly GenericRepository<Group> _groupRepository;
        public DoctorController(GenericRepository<Doctor> doctorRepositor, GenericRepository<Section> sectionRepository, GenericRepository<Group> groupRepository)
        {
            _doctorRepository = doctorRepositor;
            _sectionRepository = sectionRepository;
            _groupRepository = groupRepository;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound(new ApiResponse(404, "Doctor not found."));
            }

            var doctorDTO = new DoctorDTO
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                GroupIds = doctor.Groups.Select(d => d.Id).ToList() 

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


                });
            }

            return Ok(doctorDTOs);
        }
        [HttpGet("{doctorId}/groups")]
        public async Task<IActionResult> GetDoctorWithGroups(int doctorId)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var groups = await _groupRepository.GetAllAsync();


            var doctor = doctors.Where(d => d.Id == doctorId).Select(d => new
            {
              Groups = groups.Where(g => g.DoctorId == d.Id)
                                           .Select(g => new { g.Id, g.Name })
                                           .ToList()});


            if (doctor == null)
            {
                return NotFound(new ApiResponse(404, "Doctor not found"));
            }

            return Ok(doctor);



        }
    } }