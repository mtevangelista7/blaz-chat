namespace blazchat.Application.DTOs;

public record SendMessageDto(Guid ChatId, Guid UserId, string Message);