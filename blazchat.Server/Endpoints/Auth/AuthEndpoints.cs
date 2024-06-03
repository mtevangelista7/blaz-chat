using blazchat.Application.Interfaces.Services;
using blazchat.Client.Dtos;

namespace blazchat.Server.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/generateToken",
            async (CreateUserDto request, IAuthenticationService authenticationService) =>
            {
                try
                {
                    var accessToken =
                        await authenticationService.GenerateAccessToken(request.username, request.password);

                    return accessToken is null ? Results.BadRequest() : Results.Ok(accessToken);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
    }
}