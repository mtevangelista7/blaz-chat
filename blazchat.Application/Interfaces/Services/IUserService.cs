namespace blazchat.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Guid userId);
        Task<User> CreateUser(UserDto user);
        Task<List<User>> GetUsers();
    }
}
