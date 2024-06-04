using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace blazchat.Client.HttpClientHandler;

public class AuthenticatedHttpClientHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
    private const string TokenKey = "authToken";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Get the token from local storage
        var token = await localStorageService.GetItemAsync<string>(TokenKey, cancellationToken);

        // If the token is not null or empty, add it to the request's Authorization header
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Continue sending the request, without the tokens
        return await base.SendAsync(request, cancellationToken);
    }
}