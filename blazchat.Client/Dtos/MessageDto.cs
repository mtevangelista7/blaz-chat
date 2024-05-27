namespace blazchat.Client.Dtos;

public class MessageDto
{
    public UserDto User { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}