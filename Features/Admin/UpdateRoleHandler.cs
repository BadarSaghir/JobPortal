using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;

public class UpdateRoleCommand(Guid RoleId, string name, string? description, List<Guid> permissionIds) 
    : Command(new Id(Guid.NewGuid().ToString()))
{
    public Guid RoleId { get; } = RoleId;
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public List<Guid> PermissionIds { get; } = permissionIds;
}


public class UpdateRoleHandler(AppDbContext context, RoleManager<ApplicationRole> roleManager) 
    : RequestHandlerAsync<UpdateRoleCommand>
{
public override async Task<UpdateRoleCommand> HandleAsync(UpdateRoleCommand command, CancellationToken ct = default)
{
    var strategy = context.Database.CreateExecutionStrategy();

    await strategy.ExecuteAsync(async () => 
    {
        using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            var role = await roleManager.FindByIdAsync(command.RoleId.ToString());
            if (role == null) throw new KeyNotFoundException("Role not found.");

            role.Name = command.Name;
            role.Description = command.Description;
            await roleManager.UpdateAsync(role);

            // 1. Fetch ALL permissions for this role, INCLUDING soft-deleted ones
            // We use .IgnoreQueryFilters() to see the ones where IsDeleted is already true
            var dbPerms = await context.RolePermissions
                .IgnoreQueryFilters()
                .Where(rp => rp.RoleId == role.Id)
                .ToListAsync(ct);

            // 2. Identify permissions to Deactivate (Soft Delete)
            // They are in the DB but NOT in the new list from the UI
            var toDeactivate = dbPerms.Where(p => !command.PermissionIds.Contains(p.PermissionId));
            foreach (var p in toDeactivate)
            {
                p.IsDeleted = true;
                p.DeletedAt = DateTimeOffset.UtcNow;
            }

            // 3. Identify permissions to Reactivate or Add
            foreach (var pId in command.PermissionIds)
            {
                var existing = dbPerms.FirstOrDefault(p => p.PermissionId == pId);
                
                if (existing != null)
                {
                    // If it was previously soft-deleted, bring it back to life
                    existing.IsDeleted = false;
                    existing.DeletedAt = null;
                }
                else
                {
                    // Truly new permission the role never had before
                    context.RolePermissions.Add(new ApplicationRolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = pId,
                        IsDeleted = false
                    });
                }
            }

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    });

    return await base.HandleAsync(command, ct);
}
}