using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;
using Career635.Features.Auth;
using Career635.Features.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Career635.Domain.Entities.Auth;

namespace Career635.Controllers;

public class AccountController(IAmACommandProcessor processor,SignInManager<ApplicationUser> signInManager) : Controller
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
}