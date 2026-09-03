using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.DTOs;

public class StudentDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;

    public int UniversityId { get; set; }

    public string UniversityName { get; set; } = string.Empty;
}
