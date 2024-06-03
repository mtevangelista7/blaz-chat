using blazchat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace blazchat.Infra.Data.Context;

public class AplicationDbContext(DbContextOptions<AplicationDbContext> options) : DbContext(options)
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
}