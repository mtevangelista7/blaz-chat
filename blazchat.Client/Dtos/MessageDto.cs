namespace blazchat.Client.Dtos;

public class MessageDto
{
    public Guid ChatId { get; set; }

    public Guid User { get; set; }

    public string Text { get; set; }

    public DateTime Timestamp { get; set; }
}