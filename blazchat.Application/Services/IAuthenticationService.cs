﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using blazchat.Application.Interfaces.Services;
using blazchat.Domain.Entities;
using blazchat.Infra.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace blazchat.Application.Services;

public class AuthenticationService(
    IUserRepository userRepository,
    IMemoryCache memoryCache,
    IConfiguration configuration)
    : IAuthenticationService
{
    public async Task<string> GenerateAccessToken(string username, string password)
    {
        // Check if the user exists
        User? user = await userRepository.GetByUsername(username);

        // If the user does not exist, return an empty string
        if (user is null)
        {
            return string.Empty;
        }

        // If the password is incorrect, return an empty string
        if (!CheckPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return string.Empty;
        }

        // Check if token is already in cache
        string tokenUserLoggedIn = RetrievesCacheToken(user.Id);

        // If the token is already in cache, return it
        if (!string.IsNullOrWhiteSpace(tokenUserLoggedIn))
        {
            return tokenUserLoggedIn;
        }

        // If the token is not in cache, create a new token
        tokenUserLoggedIn = CreateToken(user);

        // Store the token in cache
        StoresJwtCache(user.Id, tokenUserLoggedIn);

        return tokenUserLoggedIn;
    }

    public bool CheckPasswordHash(string password, IReadOnlyList<byte> passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);
        byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
    }

    public string RetrievesCacheToken(Guid id)
    {
        return memoryCache.TryGetValue(id.ToString(), out string cachedToken) ? cachedToken : null;
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

        // Recupera a chave secreta e valida se não está nula ou vazia
        var tokenKey = configuration.GetSection("AppSettings:Token").Value;
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new ArgumentNullException("Token key is not configured in AppSettings.");
        }

        SymmetricSecurityKey keySecretEncrypted = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey));

        SigningCredentials creds = new SigningCredentials(keySecretEncrypted, SecurityAlgorithms.HmacSha256);

        // Configura as propriedades do token
        SecurityTokenDescriptor tokenPropriedades = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenPropriedades);

        return tokenHandler.WriteToken(token);
    }


    public void StoresJwtCache(Guid id, string token)
    {
        memoryCache.Set(id.ToString(), token, TimeSpan.FromDays(1));
    }
}