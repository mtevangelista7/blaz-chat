using blazchat.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddMudServices();

builder.Services
    .AddScoped(http => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

APAGAR.CurrentUserId = Guid.NewGuid();

await builder.Build().RunAsync();