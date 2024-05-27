using blazchat.Entities;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Context;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração da entidade Chat
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnType("uniqueidentifier");

            entity.HasMany(c => c.ChatUsers)
                .WithOne(cu => cu.Chat)
                .HasForeignKey(cu => cu.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade User
        modelBuilder.Entity<User>(entity =>
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

            entity.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Message
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.Timestamp });

            entity.Property(e => e.Text)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime2");

            entity.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

            entity.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);
        });

        // Configuração da entidade ChatUser
        modelBuilder.Entity<ChatUser>(entity =>
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