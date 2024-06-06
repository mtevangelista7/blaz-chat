using blazchat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blazchat.Infra.Data.Configurations;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.ToTable("ChatUsers");

        builder.HasKey(cu => new { cu.ChatId, cu.UserId });

        builder.HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId);

        builder.HasOne(cu => cu.User)
            .WithMany(u => u.ChatUsers)
            .HasForeignKey(cu => cu.UserId);

        builder.Ignore(cu => cu.Id);
    }
}