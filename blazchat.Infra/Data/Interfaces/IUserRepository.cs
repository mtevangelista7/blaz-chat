using blazchat.Domain.Entities;

namespace blazchat.Infra.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}