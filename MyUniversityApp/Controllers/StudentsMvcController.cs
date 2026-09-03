using Microsoft.AspNetCore.Mvc;
using UniversityApp.Application.DTOs;
using UniversityApp.Application.DTOs.Students;
using UniversityApp.Application.Interfaces;
using UniversityApp.Domain.Entities;


namespace MyUniversityApp.Controllers;

public class StudentsMvcController : Controller
{
    private readonly IStudentService _studentService;

    public StudentsMvcController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllAsync();
        return View(students);

    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var student = new Student
        {
            Name = dto.Name,
            Age = dto.Age,
            Email = dto.Email,
            UniversityId = dto.UniversityId
        };

        await _studentService.AddAsync(student);

        return RedirectToAction(nameof(Index));
    }

}