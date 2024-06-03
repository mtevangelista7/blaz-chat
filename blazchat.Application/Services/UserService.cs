namespace blazchat.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public UserService(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<User> CreateUser(UserDto userRequest)
        {
            string password = userRequest.Password;
            
            // create hash and salt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            
            var user = new User
            {
                Name = userRequest.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            
            await _userRepository.CreateUser(user);

            // generate token
            string userToken = await _authenticationService.GenerateAccessToken(user.Name, password);

            _authenticationService.StoresJwtCache(user.Id, userToken);
            
            return user;
        }

        public Task<User> GetUser(Guid userId)
        {
            return _userRepository.GetUser(userId);
        }
        
        public Task<List<User>> GetUsers()
        {
            return _userRepository.GetUsers();
        }
        
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
