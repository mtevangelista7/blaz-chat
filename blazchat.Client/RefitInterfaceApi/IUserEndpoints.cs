using blazchat.Client.Entities;
using Refit;

namespace blazchat.Client.RefitInterfaceApi;

public interface IUserEndpoints
{
    [Post("/api/user/create")]
    public Task<User> CreateUser(User user);

    [Get("/api/user/getUser/{id}")]
    public Task<User> GetUser(Guid id);

    [Get("/api/user/getUsers")]
    public Task<List<User>> GetUsers();

    [Put("/api/user/updateUser")]
    public Task UpdateUser(User user);

    [Delete("/api/user/deleteUser/{id}")]
    public Task DeleteUser(Guid id);

    [Post("/api/auth/generateToken")]
    public Task<string> Login(User user);

    [Get("/api/chat/getGuessUserByChatId/{chatId}/{currentUser}")]
    public Task<User> GetGuessUserByChatId(Guid chatId, Guid currentUser);
}