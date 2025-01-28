using Dapper;
using Microsoft.Data.SqlClient;
using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Services;

public class RoleService : IRoleService
{
    private readonly string? _connectionString;
    public RoleService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        using var connection = GetConnection();

        var _role = await connection.QueryFirstOrDefaultAsync<Role>($"SELECT * FROM Roles WHERE Id = @id", new { id });

        return _role;
    }

    public async Task CreateAsync(Role role)
    {
        using var connection = GetConnection();

        int id = await connection.QuerySingleAsync<int>("INSERT INTO Roles (RoleName) VALUES (@RoleName); SELECT CAST(SCOPE_IDENTITY() as int)", role);

        role.Id = id;
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = GetConnection();

        await connection.ExecuteAsync("DELETE FROM Roles WHERE Id = @id", new { id });
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
