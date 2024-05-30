using blazchat.Client;
using blazchat.Client.RefitInterfaceApi;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddRefitClient<IUserEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"));

builder.Services.AddRefitClient<IChatEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"));

builder.Services.AddRefitClient<IMessageEndpoints>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7076"));


builder.Services
    .AddScoped(http => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });


await builder.Build().RunAsync();