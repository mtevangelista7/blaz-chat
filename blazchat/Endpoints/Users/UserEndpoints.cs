using blazchat.Client.Dtos;
using blazchat.Entities;
using blazchat.Services.Interfaces;

namespace blazchat.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("/api/user/create", async (UserDto request, IUserService userService) =>
            {
                User user = new()
                {
                    Id = request.Id,
                    Name = request.Name
                };

                var userId = await userService.CreateUser(user);
                return Results.Ok(userId);
            });

            app.MapGet("/api/user/getUser/{id:guid}", async (Guid id, IUserService userService) =>
            {
                var user = await userService.GetUser(id);
                return Results.Ok(user);
            });

            app.MapGet("/api/user/getUsers", async (IUserService userService) =>
            {
                var users = await userService.GetUsers();
                return Results.Ok(users);
            });

            app.MapPost("/api/user/login", async (UserDto request, IUserService userService) =>
            {
                // This is a simplified version of the login method.
                // TODO: Implement a more secure login method.

                User user = new()
                {
                    Name = request.Name
                };

                var userLogged = (await userService.GetUsers()).Find(x => x.Name == user.Name);
                return Results.Ok(userLogged);
            });
        }
    }
}