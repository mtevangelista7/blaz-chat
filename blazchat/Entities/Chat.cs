namespace blazchat.Entities;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ICollection<ChatUser> ChatUsers { get; set; }
    public ICollection<Message> Messages { get; set; }
}