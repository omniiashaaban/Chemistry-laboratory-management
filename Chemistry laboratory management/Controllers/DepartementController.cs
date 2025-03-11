﻿using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartementController : ControllerBase
    {
        private readonly GenericRepository<Department> _departmentRepository;
        private readonly GenericRepository<Doctor> _doctorRepository;
        private readonly GenericRepository<Experiment> _experimentRepository;
        public DepartementController(GenericRepository<Department> departmentRepository, GenericRepository<Doctor> doctorRepository, GenericRepository<Experiment> experimentRepository)
        {
            _departmentRepository = departmentRepository;
            _doctorRepository = doctorRepository;
            _experimentRepository = experimentRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAlldepartments()
        {
            var department = await _departmentRepository.GetAllAsync();
            var departmentDTOs = department.Select(department => new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,

            }).ToList();

            return Ok(departmentDTOs);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorDTO>> CreateDoctor([FromBody] DepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
            {
                return BadRequest(new ApiResponse(404, "Invalid department data."));
            }

            var dept = new Department
            {
                Name = departmentDTO.Name

            };

            await _departmentRepository.AddAsync(dept);

            departmentDTO.Id = dept.Id;

            return Ok(new ApiResponse(200, "Department created successfully.", departmentDTO));
        }

    }
}
