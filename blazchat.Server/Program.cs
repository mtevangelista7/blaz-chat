using blazchat.Infra.Data.Context;
using blazchat.Server.Components;
using blazchat.Server.DependencyInjection;
using blazchat.Server.Endpoints.Auth;
using blazchat.Server.Endpoints.Chats;
using blazchat.Server.Endpoints.Messages;
using blazchat.Server.Endpoints.Users;
using blazchat.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MudBlazor.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();


builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();

builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = "mongodb://sa:securepassword@localhost:27017/?authSource=admin";
    return new MongoClient(connectionString);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapChatEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();
app.MapAuthEndpoints();

app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(blazchat.Client._Imports).Assembly);

app.Run();