using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Application.DTOs.Students;

public class CreateStudentDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(15, 100)]
    public int Age { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int UniversityId { get; set; }
}
