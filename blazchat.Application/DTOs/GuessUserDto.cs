namespace blazchat.Application.DTOs;

public record GuessUserDto(Guid Id, string Username)
{
    public string Username { get; set; } = Username;
    public Guid Id { get; set; } = Id;
}