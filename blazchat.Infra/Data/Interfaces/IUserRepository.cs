namespace blazchat.Infra.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(User user);
        Task<List<User>> GetUsers();
        Task<User> GetByUsername(string email);
    }
}
