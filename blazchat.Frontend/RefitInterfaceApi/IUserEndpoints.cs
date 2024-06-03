using blazchat.Client.Dtos;
using Refit;

namespace blazchat.Client.RefitInterfaceApi;

public interface IUserEndpoints
{
    [Post("/api/user/create")]
    public Task<UserDto> CreateUser(UserDto user);
    
    [Get("/api/user/getUser/{id}")]
    public Task<UserDto> GetUser(Guid id);
    
    [Get("/api/user/getUsers")]
    public Task<List<UserDto>> GetUsers();
    
    [Put("/api/user/updateUser")]
    public Task UpdateUser(UserDto user);
    
    [Delete("/api/user/deleteUser/{id}")]
    public Task DeleteUser(Guid id);

    [Post("/api/auth/generateToken")]
    public Task<UserDto> Login(UserDto user);
}