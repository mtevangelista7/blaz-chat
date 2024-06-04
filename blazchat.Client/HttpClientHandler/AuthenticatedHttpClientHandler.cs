using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace blazchat.Client.HttpClientHandler;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private const string TokenKey = "authToken";
    private readonly ILocalStorageService _localStorageService;
    
    public AuthenticatedHttpClientHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>(TokenKey);
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}