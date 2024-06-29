namespace FishingAppBackend.Services;

public interface IEmailSender
{
    Task SendPasswordResetEmailAsync(string email, string token);
    Task SendEmailConfirmationAsync(string email, string token);
}
