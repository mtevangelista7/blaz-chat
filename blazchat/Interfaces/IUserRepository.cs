using blazchat.Entities;

namespace blazchat.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(User user);
    }
}
