namespace blazchat.WebAPI.Endpoints.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/generateToken", async (UserDto request, IAuthenticationService authenticationService) =>
        {
            try
            {
                var accessToken =
                    await authenticationService.GenerateAccessToken(request.Name, request.Password);

                return accessToken is null ? Results.BadRequest() : Results.Ok(accessToken);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}