using Profiler.MinimalAPI.Services;

namespace Profiler.MinimalAPI.Endpoints;

public static class AuthRequest
{
    public static WebApplication RegisterAuthEndpoints(this WebApplication app)
    {
        app.MapGet("auth/{id}", AuthRequest.GetToken);

        return app;
    }

    public async static Task<IResult> GetToken(IAuthService authService, IUserService userService, int id)
    {
        var user = await userService.GetByIdAsync(id);

        if (user != null)
        {
            var token = authService.GenerateToken(user);

            return Results.Ok(token);
        }

        return Results.NotFound();
    }
}
