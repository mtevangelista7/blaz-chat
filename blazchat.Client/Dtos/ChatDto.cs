namespace blazchat.Client.Dtos;

public class ChatDto
{
    public Guid Id { get; set; }
    public ICollection<ChatUserDto> ChatUsers { get; set; }
}