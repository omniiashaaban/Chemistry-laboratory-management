using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly GenericRepository<Student> _studentRepository;
    private readonly GenericRepository<Department> _departmentRepository;
    private readonly GenericRepository<Group> _groupRepository;

    public StudentController(GenericRepository<Student> studentRepository, GenericRepository<Department> departmentRepository, GenericRepository<Group> groupRepository)
    {
        _studentRepository = studentRepository;
        _departmentRepository = departmentRepository;
        _groupRepository = groupRepository;
    }
    [Authorize(Roles = "Doctor")]

    // Endpoint to get all students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
    {
        var students = await _studentRepository.GetAllAsync();
        var studentDTOs = students.Select(student => new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            GroupId = student.GroupId,
        }).ToList();

        return Ok(studentDTOs);
    }

    // Endpoint to get a student by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        var studentDTO = new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            GroupId = student.GroupId,

        };

        return Ok(studentDTO);
    }

    // Endpoint to add a new student
    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent([FromBody] StudentDto studentDto)
    {
        var existingStudentWithEmail = await _studentRepository.GetAllAsync();
        if (existingStudentWithEmail.Any(s => s.Email == studentDto.Email))
        {
            return BadRequest(new ApiResponse(400, "Email is already in use."));
        }

        var group = await _groupRepository.GetByIdAsync(studentDto.GroupId);
        if (group == null)
        {
            return NotFound(new ApiResponse(404, "Group not found."));
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            GroupId = studentDto.GroupId
        };

        await _studentRepository.AddAsync(student);
        studentDto.Id = student.Id;

        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, studentDto);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
    {
       

        if (string.IsNullOrWhiteSpace(studentDto.Name) ||
            string.IsNullOrWhiteSpace(studentDto.Email))
        {
            return BadRequest(new ApiResponse(400, "Name, Email  are required fields."));
        }

        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
            return NotFound(new ApiResponse(404, "Student not found."));
        
        var existingGroup = await _departmentRepository.GetByIdAsync(id);
        if (existingGroup== null)
            return NotFound(new ApiResponse(404, "Department not found."));

        existingStudent.Name = studentDto.Name;
        existingStudent.Email = studentDto.Email;
        existingStudent.GroupId = studentDto.GroupId;

        await _studentRepository.UpdateAsync(existingStudent);

        return Ok(new ApiResponse(200, "Student updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
            return NotFound(new ApiResponse(404, "Student not found."));

        await _studentRepository.DeleteAsync(id);

        return Ok(new ApiResponse(200, "Student deleted successfully."));
    }



}
