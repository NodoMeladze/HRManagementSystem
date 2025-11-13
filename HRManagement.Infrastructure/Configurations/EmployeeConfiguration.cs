using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagement.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PersonalNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(e => e.Gender)
                .HasConversion<int>();

            builder.Property(e => e.DateOfBirth)
                .IsRequired();

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.ReleaseDate)
                .IsRequired(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ActivationScheduledAt)
                .IsRequired(false);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired();

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            builder.HasIndex(e => e.PersonalNumber)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            builder.HasIndex(e => e.Email)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL AND [Email] IS NOT NULL");
            builder.HasIndex(e => e.PositionId);
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => e.FirstName);
            builder.HasIndex(e => e.LastName);
        }
    }
}
