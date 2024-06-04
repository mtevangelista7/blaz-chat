using blazchat.Application.Interfaces.Services;
using blazchat.Application.Services;
using blazchat.Infra.Data.Interfaces;
using blazchat.Infra.Data.Repositories;

namespace blazchat.Server.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IChatService, ChatService>();
        serviceCollection.AddScoped<IMessageService, MessageService>();

        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IChatRepository, ChatRepository>();
        serviceCollection.AddScoped<IChatUsersRepository, ChatUsersRepository>();
        serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();

        return serviceCollection;
    }
}