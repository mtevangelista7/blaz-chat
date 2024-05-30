using blazchat.Entities;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Context;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }
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