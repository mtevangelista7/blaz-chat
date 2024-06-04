using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace blazchat.Client.HttpClientHandler;

public class AuthenticatedHttpClientHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
    private const string TokenKey = "authToken";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await localStorageService.GetItemAsync<string>(TokenKey, cancellationToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}