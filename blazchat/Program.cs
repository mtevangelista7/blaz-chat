using blazchat.Client.Pages;
using blazchat.Components;
using blazchat.Endpoints.Chats;
using blazchat.Endpoints.Messages;
using blazchat.Endpoints.Users;
using blazchat.Hubs;
using blazchat.Infra.Context;
using blazchat.Infra.Interfaces;
using blazchat.Infra.Repositories;
using blazchat.Services;
using blazchat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IChatUsersRepository, ChatUsersRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = "mongodb://sa:securepassword@localhost:27017/?authSource=admin";
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapChatEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();

app.MapHub<ChatHub>("/chathub");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(blazchat.Client._Imports).Assembly);

app.Run();