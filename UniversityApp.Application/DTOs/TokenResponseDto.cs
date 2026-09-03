using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.DTOs;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime AccessTokenExpiration { get; set; }
}