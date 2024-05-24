namespace blazchat.Entities;

public class Message
{
    public User User { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}