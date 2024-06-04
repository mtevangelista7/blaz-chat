using blazchat.Client;
using blazchat.Client.HttpClientHandler;
using blazchat.Client.RefitInterfaceApi;
using blazchat.Client.Services;
using blazchat.Client.Services.Interfaces;
using blazchat.Client.States;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddRefitClient<IUserEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<IChatEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<IMessageEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();