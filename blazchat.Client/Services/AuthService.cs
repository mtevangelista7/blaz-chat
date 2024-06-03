using blazchat.Client.Entities;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.JSInterop;

namespace blazchat.Client.Services;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IUserEndpoints _userEndpoints;
    private const string TokenKey = "authToken";

    public AuthService(IJSRuntime jsRuntime, IUserEndpoints userEndpoints)
    {
        _jsRuntime = jsRuntime;
        _userEndpoints = userEndpoints;
    }

    public async Task<bool> Login(string username, string password)
    {
        var token = await _userEndpoints.Login(new User()
        {
            Username = username,
            Password = password
        });

        if (string.IsNullOrWhiteSpace(token)) return false;

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        return true;
    }

    public async Task<string> GetToken()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}