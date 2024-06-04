using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazchat.Client.States;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    private const string LocalStorageKey = "authToken";

    private readonly ClaimsPrincipal _anonymousUser = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(LocalStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(_anonymousUser);
        }

        var (id, username) = GetClaims(token);

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(username))
        {
            return new AuthenticationState(_anonymousUser);
        }

        var claims = SetClaimsPrincipal(id, username);

        return claims is null ? new AuthenticationState(_anonymousUser) : new AuthenticationState(claims);
    }

    public static ClaimsPrincipal SetClaimsPrincipal(string id, string username)
    {
        var claims = new[] { new Claim("id", id), new Claim("username", username) };
        var identity = new ClaimsIdentity(claims, "jwtAuth");
        return new ClaimsPrincipal(identity);
    }

    private static (string, string) GetClaims(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return (null!, null!);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var id = jsonToken?.Claims.First(claim => claim.Type == "id").Value;
        var username = jsonToken?.Claims.First(claim => claim.Type == "username").Value;

        return (id, username)!;
    }

    public async Task UpdateAuthenticationStateAsync(string token)
    {
        var claims = new ClaimsPrincipal();

        if (string.IsNullOrWhiteSpace(token))
        {
            await localStorage.RemoveItemAsync(LocalStorageKey);
        }
        else
        {
            var (id, username) = GetClaims(token);

            if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(username))
            {
                return;
            }

            var setClaims = SetClaimsPrincipal(id, username);

            if (setClaims is null)
            {
                return;
            }

            await localStorage.SetItemAsync(LocalStorageKey, token);
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
    }
}