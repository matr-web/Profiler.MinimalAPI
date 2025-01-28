using FluentValidation;
using Profiler.MinimalAPI.Entities;
using Profiler.MinimalAPI.Services;

namespace Profiler.MinimalAPI.Endpoints;

public static class RoleRequest
{
    public static WebApplication RegisterRoleEndpoints(this WebApplication app)
    {
        var manageRoleGroup = app.MapGroup("/roles")
            .RequireAuthorization(policy => policy.RequireRole("Administrator"));

        app.MapGet("/roles/{id}", RoleRequest.GetByIdAsync);
        manageRoleGroup.MapPost("/", RoleRequest.CreateAsync);
        manageRoleGroup.MapDelete("/{id}", RoleRequest.DeleteAsync);

        return app;
    }

    public static async Task<IResult> GetByIdAsync(IRoleService roleService, int id)
    {
        var role = await roleService.GetByIdAsync(id);

        return role == null ? Results.NotFound("This role doesn't exist in the database.") : Results.Ok(role); 
    }

    public static async Task<IResult> CreateAsync(IRoleService roleService, IValidator<Role> validator, Role role)
    {
        var validationResult = await validator.ValidateAsync(role);

        if(!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        await roleService.CreateAsync(role);

        return Results.Created($"/roles/{role.Id}", role);
    }

    public static async Task<IResult> DeleteAsync(IRoleService roleService, int id)
    {
        var role = await roleService.GetByIdAsync(id);

        if (role == null)
            Results.NotFound();

        await roleService.DeleteAsync(id);

        return Results.NoContent();
    }
}
