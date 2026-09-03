using System;
using System.Collections.Generic;
using System.Text;
namespace UniversityApp.Domain.Entities;

public class University
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public ICollection<Student> Students { get; set; }
        = new List<Student>();
}