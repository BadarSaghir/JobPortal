using System.Security.Claims;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Domain.Entities.Jobs;
using Career635.Features.Admin;
using Career635.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;
using Paramore.Darker;
using QRCoder;

[Area("Admin")]
[Microsoft.AspNetCore.Mvc.Route("[area]/[controller]")] 
[Authorize]
public class AccountAdminController(IQueryProcessor queryProcessor,AppDbContext _context, IAmACommandProcessor commandProcessor,UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await queryProcessor.ExecuteAsync(new GetUserProfileQuery { UserId = userId });
        return View(result);
    }
    
    [HttpGet("Settings")]
    public IActionResult Settings() => View();

[HttpPost("ChangePassword")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
{
    if (!ModelState.IsValid) return View("Settings", model);

    var user = await userManager.GetUserAsync(User);
    if (user == null) return NotFound();

    var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
    
    if (result.Succeeded)
    {
        TempData["Success"] = "Security credentials updated successfully.";
        return RedirectToAction(nameof(Settings));
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError(string.Empty, error.Description);
    }

    return View("Settings");
}




[HttpGet("GetRecentNotifications")]
public async Task<IActionResult> GetRecentNotifications()
{
    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

    var result = await queryProcessor.ExecuteAsync(new GetNotificationsQuery { UserId = userId });
    
    // We return raw JSON here for the JavaScript Polling engine
    return Json(result);
}

[HttpGet("Notifications")]
public async Task<IActionResult> Notifications(int page = 1, string? type = null, bool? isRead = null)
{
    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    var result = await queryProcessor.ExecuteAsync(new GetPagedNotificationsQuery {
        UserId = userId,
        Page = page,
        TypeFilter = type,
        ReadFilter = isRead
    });

    return View(result);
}


[HttpGet("Enable2FA")]
public async Task<IActionResult> Enable2FA()
{
    var user = await userManager.GetUserAsync(User);
    
    // 1. Get or Create the Shared Secret Key
    var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user!);
    if (string.IsNullOrEmpty(unformattedKey))
    {
        await userManager.ResetAuthenticatorKeyAsync(user!);
        unformattedKey = await userManager.GetAuthenticatorKeyAsync(user!);
    }

    // 2. Generate the OTP Auth URI (Standard Protocol)
    string authenticatorUri = $"otpauth://totp/Career635:{user!.Email}?secret={unformattedKey}&issuer=Career635&digits=6";

    // 3. Generate QR Code Image (Base64) locally
    using var qrGenerator = new QRCodeGenerator();
    using var qrCodeData = qrGenerator.CreateQrCode(authenticatorUri, QRCodeGenerator.ECCLevel.Q);
    using var qrCode = new PngByteQRCode(qrCodeData);
    var qrBytes = qrCode.GetGraphic(20);
    string qrBase64 = Convert.ToBase64String(qrBytes);

    return View(new EnableTwoFactorViewModel(unformattedKey!, $"data:image/png;base64,{qrBase64}", null));
}

[HttpPost("Enable2FA")]
public async Task<IActionResult> Enable2FA(EnableTwoFactorViewModel model)
{
    var user = await userManager.GetUserAsync(User);
    
    // Verify the code before enabling
    var verificationCode = model.VerificationCode?.Replace(" ", "").Replace("-", "");
    var isValid = await userManager.VerifyTwoFactorTokenAsync(user!, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode!);

    if (!isValid)
    {
        ModelState.AddModelError("VerificationCode", "Verification code is invalid.");
        return View(model); // Return with errors
    }

    await userManager.SetTwoFactorEnabledAsync(user!, true);
    TempData["Success"] = "Two-Factor Authentication is now active.";
    return RedirectToAction(nameof(Profile));
}

[HttpPost("Disable2FA")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Disable2FA()
{
    var user = await userManager.GetUserAsync(User);
    await userManager.SetTwoFactorEnabledAsync(user!, false);
    TempData["Success"] = "Two-factor authentication has been disabled.";
    return RedirectToAction(nameof(Profile));
}
}