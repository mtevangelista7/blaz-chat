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

        var jwtSecurityToken = new JwtSecurityToken(token);

        // Get claims
        var (id, username) = GetClaims(jwtSecurityToken);

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

    private static (string, string) GetClaims(JwtSecurityToken token)
    {
        // If token is null or empty, return null
        if (token is null)
        {
            return (null, null);
        }

        // Get claims from token
        var jsonToken = token;

        // get id and username from claims
        var id = jsonToken?.Claims.First(claim => claim.Type == "nameid").Value;
        var username = jsonToken?.Claims.First(claim => claim.Type == "unique_name").Value;

        return (id, username)!;
    }

    public async Task UpdateAuthenticationStateAsync(string tokenString)
    {
        var claims = new ClaimsPrincipal();
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(tokenString))
        {
            await localStorage.RemoveItemAsync(LocalStorageKey);
            return;
        }

        var jwtToken = handler.ReadJwtToken(tokenString);

        // Get claims
        var (id, username) = GetClaims(jwtToken);

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

        // Store the token string in local storage
        await localStorage.SetItemAsync(LocalStorageKey, tokenString);

        // Notify authentication state changed
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
    }
}