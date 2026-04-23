using Career635.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Maps to the standard AspNetUsers table but with our custom fields
        builder.ToTable("Users");

        builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);

        // Relationships to Master Data
        builder.HasOne(u => u.Department)
               .WithMany()
               .HasForeignKey(u => u.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Designation)
               .WithMany()
               .HasForeignKey(u => u.DesignationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.PayScale)
               .WithMany()
               .HasForeignKey(u => u.PayScaleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}