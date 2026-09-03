using System;
using System.Collections.Generic;
using System.Text;
namespace UniversityApp.Domain.Entities;

public class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;

    public int UniversityId { get; set; }

    public University University { get; set; } = null!;
}