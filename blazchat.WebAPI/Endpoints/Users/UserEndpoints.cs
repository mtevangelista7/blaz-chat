using blazchat.Application.Interfaces.Services;
using blazchat.Client.Dtos;

namespace blazchat.WebAPI.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("/api/user/create", async (CreateUserDto request, IUserService userService) =>
            {
                try
                {
                    var generatedToken = await userService.CreateUser(request);

                    return generatedToken is null ? Results.BadRequest("") : Results.Ok(generatedToken);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/api/user/getUser/{userId:guid}", async (Guid userId, IUserService userService) =>
            {
                try
                {
                    var user = await userService.GetUser(userId);

                    return user is null ? Results.NotFound() : Results.Ok(user);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            }).RequireAuthorization();

            app.MapGet("/api/user/getUsers", async (IUserService userService) =>
            {
                try
                {
                    var users = await userService.GetUsers();
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            }).RequireAuthorization();
        }
    }
}