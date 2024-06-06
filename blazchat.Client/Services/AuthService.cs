﻿using Azure;
using blazchat.Application.DTOs;
using blazchat.Client.Entities;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Client.Services.Interfaces;
using Blazored.LocalStorage;

namespace blazchat.Client.Services;

public class AuthService(ILocalStorageService localStorageService, IUserEndpoints userEndpoints)
    : IAuthService
{
    private const string TokenKey = "authToken";

    public async Task<LoginReponse> Login(string username, string password)
    {
        var loginReponse = new LoginReponse();

        var token = await userEndpoints.Login(new CreateUserDto(Username: username, Password: password));

        loginReponse.Flag = !string.IsNullOrWhiteSpace(token);

        if (!loginReponse.Flag)
        {
            return loginReponse;
        }

        token = token.Replace("\u0022", "");
        loginReponse.Token = token;
        await localStorageService.SetItemAsync(TokenKey, token);
        return loginReponse;
    }

    public async Task<RegisterReponse> Register(string username, string password)
    {
        var response = new RegisterReponse();

        var token = await userEndpoints.CreateUser(new CreateUserDto(Username: username, Password: password));

        response.Flag = !string.IsNullOrWhiteSpace(token);

        if (!response.Flag)
        {
            return response;
        }

        token = token.Replace("\u0022", "");
        response.Token = token;
        await localStorageService.SetItemAsync(TokenKey, token);
        return response;
    }

    public async Task<string> GetToken()
    {
        return await localStorageService.GetItemAsync<string>(TokenKey);
    }

    public async Task Logout()
    {
        await localStorageService.RemoveItemAsync(TokenKey);
    }
}