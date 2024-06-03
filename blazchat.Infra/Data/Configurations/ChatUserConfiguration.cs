namespace blazchat.Infra.Data.Configurations;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.Entity<ChatUser>(entity =>
        {
            entity.ToTable("ChatUsers");

            entity.HasKey(cu => new { cu.ChatId, cu.UserId });

            entity.HasOne(cu => cu.Chat)
                .WithMany(c => c.ChatUsers)
                .HasForeignKey(cu => cu.ChatId);

            entity.HasOne(cu => cu.User)
                .WithMany(u => u.ChatUsers)
                .HasForeignKey(cu => cu.UserId);
        });
    }
}