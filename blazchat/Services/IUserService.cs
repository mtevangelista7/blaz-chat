using blazchat.Entities;

namespace blazchat.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(User user);
        Task<List<User>> GetUsers();
    }
}
