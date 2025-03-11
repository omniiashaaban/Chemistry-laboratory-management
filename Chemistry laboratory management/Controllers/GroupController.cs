using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly GenericRepository<Group> _groupRepository;
    private readonly GenericRepository<Department> _departmentRepository;
    private readonly GenericRepository<Doctor> _doctorRepository;

    public GroupController(GenericRepository<Group> groupRepository,
                           GenericRepository<Department> departmentRepository,
                           GenericRepository<Doctor> doctorRepository)
    {
        _groupRepository = groupRepository;
        _departmentRepository = departmentRepository;
        _doctorRepository = doctorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDTO>>> GetAllGroups()
    {
        var groups = await _groupRepository.GetAllAsync();
        var groupDTOs = groups.Select(group => new GroupDTO
        {
            Name = group.Name,
            Level = group.Level,
            DepartmentId = group.Department.Id,
            DoctorId = group.Doctor.Id
        }).ToList();

        return Ok(groupDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDTO>> GetGroupById(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group == null)
        {
            return NotFound(new ApiResponse(404, "Group not found."));
        }

        var groupDTO = new GroupDTO
        {
            Name = group.Name,
            Level = group.Level,
            DepartmentId = group.Department.Id,
            DoctorId = group.Doctor.Id
        };

        return Ok(groupDTO);
    }
    [HttpGet("bydoctorId/{doctorId}")]
    public async Task<IActionResult> GetGroupsByDoctor(int doctorId)
    {
        var Groups = await _groupRepository.GetAllAsync();
        var groups =Groups.Where(g => g.DoctorId == doctorId).Select(g => new 
        {
             g.Id,
             g.Name,
             g.DepartmentId
        })

                                   .ToList();

        
        if (!groups.Any())
        {
            return NotFound(new ApiResponse(404,  "No groups for this doctor." ));
        }
      

        return Ok(groups);
    }


    [HttpPost]
    public async Task<ActionResult<GroupDTO>> CreateGroup([FromBody] GroupDTO groupDto)
    {
        var department = await _departmentRepository.GetByIdAsync(groupDto.DepartmentId);
        if (department == null)
        {
            return NotFound(new ApiResponse(404, "Department not found."));
        }

        var doctor = await _doctorRepository.GetByIdAsync(groupDto.DoctorId);
        if (doctor == null)
        {
            return NotFound(new ApiResponse(404, "Doctor not found."));
        }

        var group = new Group
        {
            Name = groupDto.Name,
            Level = groupDto.Level,
            DepartmentId = groupDto.DepartmentId,
            DoctorId = groupDto.DoctorId
        };

        await _groupRepository.AddAsync(group);

        return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, groupDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGroup(int id, [FromBody] GroupDTO groupDto)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);
        if (existingGroup == null)
        {
            return NotFound(new ApiResponse(404, "Group not found."));
        }

        var department = await _departmentRepository.GetByIdAsync(groupDto.DepartmentId);
        if (department == null)
        {
            return NotFound(new ApiResponse(404, "Department not found."));
        }

        var doctor = await _doctorRepository.GetByIdAsync(groupDto.DoctorId);
        if (doctor == null)
        {
            return NotFound(new ApiResponse(404, "Doctor not found."));
        }

        existingGroup.Name = groupDto.Name;
        existingGroup.Level = groupDto.Level;
        existingGroup.DepartmentId = groupDto.DepartmentId;
        existingGroup.DoctorId = groupDto.DoctorId;

        await _groupRepository.UpdateAsync(existingGroup);

        return Ok(new ApiResponse(200, "Group updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);
        if (existingGroup == null)
        {
            return NotFound(new ApiResponse(404, "Group not found."));
        }

        await _groupRepository.DeleteAsync(id);

        return Ok(new ApiResponse(200, "Group deleted successfully."));
    }
}