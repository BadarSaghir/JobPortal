using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Paramore.Brighter;

public class CreateRoleCommand(ApplicationRole role, List<Guid> permissionIds) : Command(new Id(Guid.NewGuid().ToString()))
{
    public ApplicationRole Role { get; } = role;
    public List<Guid> PermissionIds { get; } = permissionIds;
}

public class CreateRoleHandler(AppDbContext context, RoleManager<ApplicationRole> roleManager) 
    : RequestHandlerAsync<CreateRoleCommand>
{
    public override async Task<CreateRoleCommand> HandleAsync(CreateRoleCommand command, CancellationToken ct = default)
    {
        using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            // 1. Create the base Role
            var result = await roleManager.CreateAsync(command.Role);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // 2. Link Permissions via Junction Table
            foreach (var pId in command.PermissionIds)
            {
                context.RolePermissions.Add(new ApplicationRolePermission
                {
                    RoleId = command.Role.Id,
                    PermissionId = pId
                });
            }

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch {
            await transaction.RollbackAsync(ct);
            throw;
        }

        return await base.HandleAsync(command, ct);
    }
}