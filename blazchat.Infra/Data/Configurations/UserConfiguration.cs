using blazchat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blazchat.Infra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Username)
            .HasColumnType("nvarchar")
            .HasMaxLength(100);

        builder.Property(c => c.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasColumnType("VARBINARY(MAX)")
            .IsRequired();

        builder.Property(c => c.PasswordSalt)
            .HasColumnName("PasswordSalt")
            .HasColumnType("VARBINARY(MAX)")
            .IsRequired();

        builder.HasMany(u => u.ChatUsers)
            .WithOne(cu => cu.User)
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}