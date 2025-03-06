using Chemistry_laboratory_management.Dtos;
using laboratory.DAL.Models;
using laboratory.DAL.Repository;
using LinkDev.Facial_Recognition.BLL.Helper.Errors;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly GenericRepository<Student> _studentRepository;
    private readonly GenericRepository<Department> _departmentRepository;
    private readonly GenericRepository<Group> _groupRepository;

    public StudentController(GenericRepository<Student> studentRepository, GenericRepository<Department> departmentRepository, GenericRepository<Group> groupRepository  )
    {
        _studentRepository = studentRepository;
        _departmentRepository = departmentRepository;
        _groupRepository = groupRepository;
    }

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
            GroupName = student.GroupName,
            DepartmentName = student.DepartmentName,
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
            GroupName = student.GroupName,
            DepartmentName = student.DepartmentName,

        };

        return Ok(studentDTO);
    }

    // Endpoint to add a new student
    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent([FromBody] StudentDto studentDto)
    {
        var group= await _groupRepository.GetAllAsync();
        bool groupExists = group.Any(d => d.Name == studentDto.GroupName);
        if (!groupExists)
        {
            return NotFound(new ApiResponse(404, "Group not found."));
        }
        var departement = await _departmentRepository.GetAllAsync();
        bool departmentExists = departement.Any(d => d.Name == studentDto.DepartmentName);
        if (!departmentExists)
        {
            return NotFound(new ApiResponse(404, "Department not found."));
        }
        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            DepartmentName = studentDto.DepartmentName,
            GroupName = studentDto.GroupName,


        };

        await _studentRepository.AddAsync(student);

        studentDto.Id = student.Id;

        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, studentDto);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
    {
        if (id != studentDto.Id)
            return BadRequest(new ApiResponse(400, "The provided ID does not match the student ID."));

        if (string.IsNullOrWhiteSpace(studentDto.Name) ||
            string.IsNullOrWhiteSpace(studentDto.Email) ||
            string.IsNullOrWhiteSpace(studentDto.GroupName) ||
            string.IsNullOrWhiteSpace(studentDto.DepartmentName))
        {
            return BadRequest(new ApiResponse(400, "Name, Email, Department, and Group are required fields."));
        }

        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
            return NotFound(new ApiResponse(404, "Student not found."));

        existingStudent.Name = studentDto.Name;
        existingStudent.Email = studentDto.Email;
        existingStudent.GroupName = studentDto.GroupName;
        existingStudent.DepartmentName = studentDto.DepartmentName;

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