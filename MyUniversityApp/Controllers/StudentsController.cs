using Microsoft.AspNetCore.Mvc;

using UniversityApp.Application.DTOs;
using UniversityApp.Application.DTOs.Students;
using UniversityApp.Application.Interfaces;
using UniversityApp.Domain.Entities;
using UniversityApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace MyUniversityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IGenericRepository<Student> _studentRepository;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;

    public StudentsController(
        IGenericRepository<Student> studentRepository,
        IEmailService emailService,
        ApplicationDbContext context)
    {
        _studentRepository = studentRepository;
        _emailService = emailService;
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> Get(int id)
    {
        var student = await _studentRepository.GetAsync(id);

        if (student is null)
        {
            return NotFound(new
            {
                message = $"Student with ID {id} was not found."
            });
        }

        var university = await _context.Universities
            .FindAsync(student.UniversityId);

        var result = new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Age = student.Age,
            Email = student.Email,
            UniversityId = student.UniversityId,
            UniversityName = university?.Name ?? string.Empty
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Post(
        CreateStudentDto dto)
    {
        var university = await _context.Universities
            .FindAsync(dto.UniversityId);

        if (university is null)
        {
            return BadRequest(new
            {
                message = "University was not found."
            });
        }

        var student = new Student
        {
            Name = dto.Name.Trim(),
            Age = dto.Age,
            Email = dto.Email.Trim(),
            UniversityId = dto.UniversityId
        };

        await _studentRepository.PostAsync(student);

        await _emailService.SendAdmissionEmailAsync(
            student.Email,
            student.Name,
            university.Name);

        var result = new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Age = student.Age,
            Email = student.Email,
            UniversityId = student.UniversityId,
            UniversityName = university.Name
        };

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Student>> Put(
        int id,
        CreateStudentDto dto)
    {
        var student = await _studentRepository.GetAsync(id);

        if (student is null)
        {
            return NotFound(new
            {
                message = $"Student with ID {id} was not found."
            });
        }

        var university = await _context.Universities
            .FindAsync(dto.UniversityId);

        if (university is null)
        {
            return BadRequest(new
            {
                message = "University was not found."
            });
        }

        student.Name = dto.Name.Trim();
        student.Age = dto.Age;
        student.Email = dto.Email.Trim();
        student.UniversityId = dto.UniversityId;

        await _studentRepository.PutAsync(student);

        return Ok(student);
    }
}
