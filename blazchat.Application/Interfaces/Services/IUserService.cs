using blazchat.Client.Dtos;
using blazchat.Domain.Entities;

namespace blazchat.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Guid userId);
        Task<string> CreateUser(CreateUserDto user);
        Task<List<User>> GetUsers();
    }
}
