namespace blazchat.Application.DTOs;

public record ValidateChatDto(Guid ChatId, Guid UserId)
{
    public Guid ChatId { get; set; } = ChatId;
    public Guid UserId { get; set; } = UserId;
}