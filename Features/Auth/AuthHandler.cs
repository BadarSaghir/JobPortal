using Microsoft.AspNetCore.Identity;
using Career635.Domain.Entities.Auth;
using Paramore.Brighter;

namespace Career635.Features.Auth;

public class LoginHandler(SignInManager<ApplicationUser> signInManager) : RequestHandlerAsync<LoginCommand>
{
    public override async Task<LoginCommand> HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        // Execute the Identity sign-in logic
        command.Result = await signInManager.PasswordSignInAsync(
            command.Email, 
            command.Password, 
            command.RememberMe, 
            lockoutOnFailure: true); // Safety: Lock account after 5 failed attempts

        return await base.HandleAsync(command, ct);
    }
}