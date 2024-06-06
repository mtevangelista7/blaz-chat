using blazchat.Application.DTOs;
using blazchat.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace blazchat.Server.Endpoints.Users
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

            app.MapGet("/api/user/getUsers", async ([FromServices] IUserService userService) =>
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

            app.MapGet("/api/user/getGuessUserByChatId/{chatId:guid}/{currentUser:guid}", async (Guid chatId,
                Guid currentUser, IChatService chatService, IUserService userService) =>
            {
                try
                {
                    var guessUserId = await chatService.GetGuessUserByChatId(chatId, currentUser);

                    if (guessUserId.Equals(Guid.Empty))
                    {
                        return Results.NotFound();
                    }

                    var guessUser = await userService.GetUser(guessUserId);

                    return guessUser is null ? Results.NotFound() : Results.Ok(guessUser);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization();
        }
    }
}