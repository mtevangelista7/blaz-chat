using blazchat.Domain.Entities;
using blazchat.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;

namespace blazchat.Infra.Data.Context;

public class AplicationDbContext(DbContextOptions<AplicationDbContext> options) : DbContext(options)
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new ChatUserConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}