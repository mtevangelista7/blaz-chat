using blazchat.Client;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddRefitClient<IUserEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:44350"));

builder.Services.AddRefitClient<IChatEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:44350"));

builder.Services
    .AddScoped(http => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });


APAGAR.CurrentUserId = Guid.NewGuid();

await builder.Build().RunAsync();