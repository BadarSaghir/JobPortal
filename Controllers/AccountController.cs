using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;
using Career635.Features.Auth;
using Career635.Features.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Career635.Domain.Entities.Auth;
    using QRCoder;
using Career635.Areas.Admin.Models; // Local QR Generation

namespace Career635.Controllers;

public class AccountController(IAmACommandProcessor processor,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> _userManager,  ILogger<AccountController> _logger) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new LoginCommand { 
            Email = model.Email, 
            Password = model.Password, 
            RememberMe = model.RememberMe 
        };

        await processor.SendAsync(command);

        if (command.Result!.Succeeded)
        {
            return LocalRedirect(returnUrl ?? "/Admin/Dashboard");
        }
if (command.Result.RequiresTwoFactor)
    {
        return RedirectToAction("Verify2FA", new { ReturnUrl = returnUrl });
    }
        if (command.Result.IsLockedOut)
        {
            ModelState.AddModelError("", "Account locked due to multiple failed attempts. Please contact IT.");
        }
        else
        {
            ModelState.AddModelError("", "Invalid credentials for organizational access.");
        }

        return View(model);
    }
    [HttpGet("/Account/AccessDenied")]
public IActionResult AccessDenied()
{
    return View();
}

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
                await signInManager.SignOutAsync();

        // Sign out logic...
        return RedirectToAction("Index", "Home");
    }
// GET: /Account/Verify2FA
 [HttpGet]
    public async Task<IActionResult> Verify2FA(string? returnUrl = null, bool rememberMe = false)
    {
        // 1. Ensure there is a user in the intermediate "Two-Factor" buffer
        // If the user hasn't successfully entered their password first, this will be null
        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        
        if (user == null)
        {
            _logger.LogWarning("Anonymous user attempted to access 2FA verification without password authentication.");
            return RedirectToAction(nameof(Login));
        }

        var model = new VerifyTwoFactorViewModel 
        { 
            ReturnUrl = returnUrl,
            RememberMe = rememberMe 
        };

        return View(model);
    }

    // POST: /Account/Verify2FA
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2FA(VerifyTwoFactorViewModel model)
    {
        if (!ModelState.IsValid) return View("Verify2FA", model);

        // Remove any spaces or dashes from the input code (common for mobile users)
        var verificationCode = model.VerificationCode.Replace(" ", "").Replace("-", "");

        // 2. Perform the 2FA check
        // This validates the code and creates the full authentication cookie if successful
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(
            verificationCode, 
            model.RememberMe, 
            model.RememberDevice);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in with 2FA.");
            return LocalRedirect(model.ReturnUrl ?? "/Admin/Dashboard");
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out due to failed 2FA attempts.");
            return RedirectToAction("Lockout"); // Create a simple Lockout view if needed
        }

        // 3. Handle Failure
        _logger.LogWarning("Invalid 2FA code entered.");
        ModelState.AddModelError(string.Empty, "The verification code provided is incorrect.");
        return View("Verify2FA", model);
    }


}