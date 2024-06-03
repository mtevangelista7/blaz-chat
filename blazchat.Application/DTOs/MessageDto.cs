namespace blazchat.Client.Dtos;

public class MessageDto
{
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public string Text { get; set; }

    public DateTime Timestamp { get; set; }
}