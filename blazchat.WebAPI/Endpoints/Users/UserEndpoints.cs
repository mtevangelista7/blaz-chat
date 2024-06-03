namespace blazchat.WebAPI.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("/api/user/create", async (UserDto request, IUserService userService) =>
            {
                var userId = await userService.CreateUser(request);
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
        }
    }
}