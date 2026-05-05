using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Career635.Domain.Entities.Auth;
using Career635.Domain.Entities.Locations;

namespace Career635.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder) {
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Code).HasMaxLength(20);
    }
}

public class DesignationConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder) {
        builder.Property(d => d.Title).IsRequired().HasMaxLength(100);
    }
}

public class PayScaleConfiguration : IEntityTypeConfiguration<PayScale>
{
    public void Configure(EntityTypeBuilder<PayScale> builder) {
        builder.Property(p => p.Grade).IsRequired().HasMaxLength(50);
    }
}

public class LocationConfiguration : 
    IEntityTypeConfiguration<Province>, 
    IEntityTypeConfiguration<District>, 
    IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<Province> builder) {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }

    public void Configure(EntityTypeBuilder<District> builder) {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }

    public void Configure(EntityTypeBuilder<City> builder) {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}