namespace blazchat.Application.DTOs;

public record CreateUserDto(string Username, string Password)
{
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
}