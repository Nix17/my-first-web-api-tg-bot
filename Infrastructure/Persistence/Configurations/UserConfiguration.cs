using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users").HasKey(m => m.Id);
        builder.ToTable("users").HasIndex(m => m.Email).IsUnique();
        builder.ToTable("users").HasIndex(m => m.Phone).IsUnique();
        builder.Property(m => m.LastName).HasDefaultValue("Not found").IsRequired().HasMaxLength(100);
        builder.Property(m => m.FirstName).HasDefaultValue("Not found").IsRequired().HasMaxLength(100);
        builder.Property(m => m.MiddleName).HasDefaultValue("Not found").IsRequired().HasMaxLength(100);
        builder.Property(m => m.Password).IsRequired().HasMaxLength(300);
        builder.Property(m => m.Role).HasDefaultValue("user").IsRequired().HasMaxLength(10);    }
}