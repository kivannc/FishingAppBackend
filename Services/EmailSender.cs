using FishingAppBackend.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FishingAppBackend.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmUrl)
        {
            var smtpClient = new SmtpClient(_emailSettings.Host)
            {
                Port = _emailSettings.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromAddress),
                Subject = "Confirm your email",
                Body = $"Please confirm your account by clicking this link: <a href='{confirmUrl}'>Confirm Email</a>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetUrl)
        {
            var smtpClient = new SmtpClient(_emailSettings.Host)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromAddress),
                Subject = "Password Reset",
                Body = $"Please reset your password using the following link: <a href='{resetUrl}'>Reset Password</a>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
