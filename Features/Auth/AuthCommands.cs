using Paramore.Brighter;
using Microsoft.AspNetCore.Identity;
using Career635.Domain.Entities.Auth;

namespace Career635.Features.Auth;

public class LoginCommand : Command
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
    public Microsoft.AspNetCore.Identity.SignInResult? Result { get; set; }

    public LoginCommand() : base(new Id(Guid.NewGuid().ToString())) { }
}

