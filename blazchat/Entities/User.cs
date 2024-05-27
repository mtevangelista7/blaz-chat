namespace blazchat.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<ChatUser> ChatUsers { get; set; }
    public ICollection<Message> Messages { get; set; }
}