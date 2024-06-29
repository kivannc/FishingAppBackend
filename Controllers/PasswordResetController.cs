using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FishingAppBackend.Models;
using System.Threading.Tasks;
using FishingAppBackend.Services;

namespace FishingAppBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PasswordResetController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender; // Optional for sending emails

    public PasswordResetController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestReset([FromBody] ResetRequestModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetUrl = Url.Action("ResetPassword", "PasswordReset", new { token = token, email = model.Email }, Request.Scheme);

        // Send the reset link to the user's email address
        await _emailSender.SendPasswordResetEmailAsync(user.Email, resetUrl);

        return Ok(new { message = "Password reset token sent." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (result.Succeeded)
        {
            return Ok(new { message = "Password reset successfully." });
        }

        return BadRequest(result.Errors);
    }
}

