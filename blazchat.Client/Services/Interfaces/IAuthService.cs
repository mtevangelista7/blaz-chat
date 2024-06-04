using blazchat.Client.Entities;

namespace blazchat.Client.Services.Interfaces;

public interface IAuthService
{
    Task<LoginReponse> Login(string username, string password);
    Task<RegisterReponse> Register(string username, string password);
    Task<string> GetToken();
    Task Logout();
}