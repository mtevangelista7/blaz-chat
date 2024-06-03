namespace blazchat.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public ICollection<ChatUser> ChatUsers { get; set; }
}