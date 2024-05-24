using blazchat.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();

APAGAR.Id = Guid.NewGuid();

await builder.Build().RunAsync();
