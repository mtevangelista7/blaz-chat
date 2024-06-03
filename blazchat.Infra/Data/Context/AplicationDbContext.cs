namespace blazchat.Infra.Data.Context;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
}