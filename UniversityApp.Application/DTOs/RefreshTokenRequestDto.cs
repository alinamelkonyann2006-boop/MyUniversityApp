using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.DTOs;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}