using blazchat.Domain.Entities;
using System.Text.Json.Serialization;

namespace blazchat.Application.DTOs;

public record ChatDto(Guid Id, ICollection<ChatUserDto> ChatUsers)
{
    public Guid Id { get; set; } = Id;
    public ICollection<ChatUserDto> ChatUsers { get; set; } = ChatUsers;
}