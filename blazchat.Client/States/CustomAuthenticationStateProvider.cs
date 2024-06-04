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
        // Get token from local storage
        var token = await localStorage.GetItemAsync<string>(LocalStorageKey);

        // If token is null or empty, return anonymous user
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(_anonymousUser);
        }

        // Get claims
        var (id, username) = GetClaims(token);

        // If id or username is null or empty, return anonymous user
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(username))
        {
            return new AuthenticationState(_anonymousUser);
        }

        // Set claims principal
        var claims = SetClaimsPrincipal(id, username);

        // If claims is null, return anonymous user else return authenticated user
        return claims is null ? new AuthenticationState(_anonymousUser) : new AuthenticationState(claims);
    }

    private static ClaimsPrincipal SetClaimsPrincipal(string id, string username)
    {
        var claims = new[] { new Claim("id", id), new Claim("username", username) };
        var identity = new ClaimsIdentity(claims, "jwtAuth");
        return new ClaimsPrincipal(identity);
    }

    private static (string, string) GetClaims(string token)
    {
        // If token is null or empty, return null
        if (string.IsNullOrWhiteSpace(token)) return (null!, null!);

        // Get claims from token
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        // get id and username from claims
        var id = jsonToken?.Claims.First(claim => claim.Type == "id").Value;
        var username = jsonToken?.Claims.First(claim => claim.Type == "username").Value;

        return (id, username)!;
    }

    public async Task UpdateAuthenticationStateAsync(string token)
    {
        var claims = new ClaimsPrincipal();

        // If token is null or empty, remove token from local storage
        if (string.IsNullOrWhiteSpace(token))
        {
            await localStorage.RemoveItemAsync(LocalStorageKey);
        }
        // else set token in local storage
        else
        {
            // Get claims
            var (id, username) = GetClaims(token);

            // If id and username is null or empty, return
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

        // Notify authentication state changed
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
    }
}