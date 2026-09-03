using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Interfaces;

public interface IEmailService
{
    Task SendAdmissionEmailAsync(
        string recipientEmail,
        string studentName,
        string universityName);
}
