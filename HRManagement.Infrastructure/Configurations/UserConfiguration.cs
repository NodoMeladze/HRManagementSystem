using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagement.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.PersonalNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Gender)
                .HasConversion<int>();

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.DeletedAt)
                .IsRequired(false);

            builder.HasIndex(u => u.PersonalNumber)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");
        }
    }
}
