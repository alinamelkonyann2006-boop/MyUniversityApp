using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using UniversityApp.Application.Interfaces;

namespace UniversityApp.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAdmissionEmailAsync(
        string recipientEmail,
        string studentName,
        string universityName)
    {
        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));

        message.To.Add(
            new MailboxAddress(
                studentName,
                recipientEmail));

        message.Subject = "University admission";

        message.Body = new TextPart("plain")
        {
            Text =
                $"Բարև, {studentName}։\n\n" +
                $"Դու ընդունվել ես {universityName} \n\n" +
                "Շնորհավորում ենք։"
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            _settings.SmtpServer,
            _settings.SmtpPort,
            SecureSocketOptions.StartTls);

        await smtpClient.AuthenticateAsync(
            _settings.SenderEmail,
            _settings.Password);

        await smtpClient.SendAsync(message);

        await smtpClient.DisconnectAsync(true);
    }
}