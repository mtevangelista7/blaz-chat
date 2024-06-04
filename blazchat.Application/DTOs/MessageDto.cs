namespace blazchat.Application.DTOs;

public record MessageDto(Guid UserId, string Text, DateTime Timestamp);