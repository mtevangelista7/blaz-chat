using blazchat.Entities;
using blazchat.Interfaces;

namespace blazchat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }

        public Task<User> GetUser(Guid userId)
        {
            return _userRepository.GetUser(userId);
        }
    }
}
