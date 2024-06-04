using blazchat.Application.DTOs;
using blazchat.Client.Entities;
using Refit;

namespace blazchat.Client.RefitInterfaceApi;

public interface IUserEndpoints
{
    [Post("/api/user/create")]
    public Task<string> CreateUser(CreateUserDto user);

    [Get("/api/user/getUser/{id}")]
    public Task<UserDto> GetUser(Guid id);

    [Get("/api/user/getUsers")]
    public Task<List<UserDto>> GetUsers();

    [Put("/api/user/updateUser")]
    public Task UpdateUser(UserDto user);

    [Delete("/api/user/deleteUser/{id}")]
    public Task DeleteUser(Guid id);

    [Post("/api/auth/generateToken")]
    public Task<string> Login(CreateUserDto user);

    [Get("/api/chat/getGuessUserByChatId/{chatId}/{currentUser}")]
    public Task<GuessUserDto> GetGuessUserByChatId(Guid chatId, Guid currentUser);

    [Get("/api/user/getGuessUserById/{id}")]
    public Task<GuessUserDto> GetGuessUserById(Guid id);
}