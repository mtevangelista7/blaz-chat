namespace blazchat.Application.DTOs;

public record MessageDto(Guid ChatId, Guid UserId, string Text, DateTime Timestamp);