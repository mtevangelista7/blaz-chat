namespace blazchat.Domain.Entities;

public class Chat : EntityBase
{
    public ICollection<ChatUser> ChatUsers { get; set; }
}