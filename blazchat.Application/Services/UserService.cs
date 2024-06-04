using System.Security.Cryptography;
using blazchat.Application.DTOs;
using blazchat.Application.Interfaces.Services;
using blazchat.Domain.Entities;
using blazchat.Infra.Data.Interfaces;

namespace blazchat.Application.Services
{
    public class UserService(IUserRepository userRepository, IAuthenticationService authenticationService)
        : IUserService
    {
        public async Task<string> CreateUser(CreateUserDto userRequest)
        {
            string password = userRequest.Password;

            // create hash and salt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // create user
            var user = new User
            {
                Name = userRequest.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // add user to database
            await userRepository.Add(user);

            // generate token
            string userToken = await authenticationService.GenerateAccessToken(user.Name, password);
            authenticationService.StoresJwtCache(user.Id, userToken);

            // return token
            return userToken;
        }

        public Task<User> GetUser(Guid userId)
        {
            return userRepository.GetById(userId);
        }

        public Task<List<User>> GetUsers()
        {
            return userRepository.GetAll();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}