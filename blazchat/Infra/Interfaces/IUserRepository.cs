using blazchat.Entities;

namespace blazchat.Infra.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(User user);
        Task<List<User>> GetUsers();
    }
}
