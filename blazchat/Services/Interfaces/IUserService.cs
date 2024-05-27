using blazchat.Entities;

namespace blazchat.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(User user);
        Task<List<User>> GetUsers();
    }
}
