using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chemistry_laboratory_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartementController : ControllerBase
    {
        private readonly GenericRepository<Department> _departmentRepository;
        public DepartementController(GenericRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
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

    }
}
