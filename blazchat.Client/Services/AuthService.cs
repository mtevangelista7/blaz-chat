using blazchat.Client.Entities;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace blazchat.Client.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IUserEndpoints _userEndpoints;
    private const string TokenKey = "authToken";

    public AuthService(ILocalStorageService localStorageService, IUserEndpoints userEndpoints)
    {
        _localStorageService = localStorageService;
        _userEndpoints = userEndpoints;
    }

    public async Task<LoginReponse> Login(string username, string password)
    {
        LoginReponse loginReponse = new LoginReponse();

        var token = await _userEndpoints.Login(new User()
        {
            Username = username,
            Password = password
        });

        loginReponse.Flag = !string.IsNullOrWhiteSpace(token);
        loginReponse.Token = token;

        if (!loginReponse.Flag)
        {
            return loginReponse;
        }

        await _localStorageService.SetItemAsync(TokenKey, token);
        return loginReponse;
    }

    public async Task<RegisterReponse> Register(string username, string password)
    {
        RegisterReponse response = new RegisterReponse();

        var token = await _userEndpoints.CreateUser(new User()
        {
            Username = username,
            Password = password
        });

        response.Flag = !string.IsNullOrWhiteSpace(token);
        response.Token = token;

        if (!response.Flag)
        {
            return response;
        }

        await _localStorageService.SetItemAsync(TokenKey, token);
        return response;
    }

    public async Task<string> GetToken()
    {
        return await _localStorageService.GetItemAsync<string>(TokenKey);
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync(TokenKey);
    }
}