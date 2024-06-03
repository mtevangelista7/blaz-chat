namespace blazchat.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<string> GenerateAccessToken(string userName, string password);
    bool CheckPasswordHash(string password, IReadOnlyList<byte> passwordHash, byte[] passwordSalt);
    public string RetrievesCacheToken(Guid id);
    public void StoresJwtCache(Guid id, string token);
}