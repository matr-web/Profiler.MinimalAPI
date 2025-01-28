using FluentValidation;
using Profiler.MinimalAPI.Entities;
using Profiler.MinimalAPI.Services;

namespace Profiler.MinimalAPI.Endpoints;

public static class UserRequest
{
    public static WebApplication RegisterUserEndpoints(this WebApplication app)
    {
        var getUserGroup = app.MapGroup("/users");

        var manageUserGroup = app.MapGroup("/users")
            .RequireAuthorization(policy => policy.RequireRole("Administrator")); ;

        getUserGroup.MapGet("/", UserRequest.GetAllAsync);
        getUserGroup.MapGet("/{id}", UserRequest.GetByIdAsync);
        manageUserGroup.MapPost("/", UserRequest.CreateAsync);
        manageUserGroup.MapPut("/", UserRequest.PutAsync);
        manageUserGroup.MapDelete("/{id}", UserRequest.DeleteAsync);

        return app;
    }

    public async static Task<IResult> GetAllAsync(IUserService userService)
    {
        var users = await userService.GetAllAsync();

        return Results.Ok(users);
    }

    public async static Task<IResult> GetByIdAsync(IUserService userService, int id)
    {
        var user = await userService.GetByIdAsync(id);

        return user == null ? Results.NotFound("This role doesn't exist in the database.") : Results.Ok(user);
    }

    public async static Task<IResult> CreateAsync(IUserService userService, IValidator<User> validator, User user)
    {
        var validationResult = await validator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        await userService.CreateAsync(user);

        return Results.Created($"/users/{user.Id}", user);
    }

    public async static Task<IResult> PutAsync(IUserService userService, IValidator<User> validator, int id, User user)
    {
        if(id != user.Id)
        {
            return Results.BadRequest();
        }

        var validationResult = await validator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var _user = await userService.GetByIdAsync(id);

        if(_user == null)
        {
            return Results.NotFound();
        }

        await userService.UpdateAsync(user);

        return Results.Ok();
    }

    public async static Task<IResult> DeleteAsync(IUserService userService, int id)
    {
        var user = await userService.GetByIdAsync(id);

        if (user == null)
            Results.NotFound();

        await userService.DeleteAsync(id);

        return Results.NoContent();
    }
}
