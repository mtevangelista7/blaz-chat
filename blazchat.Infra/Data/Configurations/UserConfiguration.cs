namespace blazchat.Infra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnType("uniqueidentifier");

            entity.Property(e => e.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            entity.HasMany(u => u.ChatUsers)
                .WithOne(cu => cu.User)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}