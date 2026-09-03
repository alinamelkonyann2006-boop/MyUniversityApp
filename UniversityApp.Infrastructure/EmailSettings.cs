using System;
using System.Collections.Generic;
using System.Text;
namespace UniversityApp.Infrastructure.Email;

public class EmailSettings
{
    public string SenderName { get; set; } = string.Empty;

    public string SenderEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string SmtpServer { get; set; } = "smtp.gmail.com";

    public int SmtpPort { get; set; } = 587;
}