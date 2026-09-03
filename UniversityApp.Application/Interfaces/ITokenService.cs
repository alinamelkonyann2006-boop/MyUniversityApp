using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.DTOs;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Interfaces;

public interface ITokenService
{
    TokenResponseDto CreateTokens(User user);

    string GenerateRefreshToken();
}