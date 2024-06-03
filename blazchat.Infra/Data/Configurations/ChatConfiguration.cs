namespace blazchat.Infra.Data.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnType("uniqueidentifier");

            entity.HasMany(c => c.ChatUsers)
                .WithOne(cu => cu.Chat)
                .HasForeignKey(cu => cu.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}