using Dapper;
using Microsoft.Data.SqlClient;
using Profiler.MinimalAPI.Entities;
using System.Data;

namespace Profiler.MinimalAPI.Services;

public class UserService : IUserService
{
    private readonly SqlConnection _connection;

    public UserService(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public async Task<IEnumerable<User>?> GetAllAsync()
    {
        var sql = "SELECT u.*, r.*, a.*" +
            " FROM Users u " +
            "LEFT JOIN Roles r ON u.RoleId = r.Id " +
            "LEFT JOIN Addresses a ON u.Id = a.UserId;";

        var userDictionary = await GetUsersDictionaryBySQL(sql);

        return userDictionary.Values.AsEnumerable();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var sql = "SELECT u.*, r.*, a.* " +
            " FROM Users u " +
            "LEFT JOIN Roles r ON u.RoleId = r.Id " +
            "LEFT JOIN Addresses a ON u.Id = a.UserId "
            +"WHERE u.Id = @id;";

        var userDictionary = await GetUsersDictionaryBySQL(sql, new { id });

        return userDictionary.Values.FirstOrDefault();
    }

    public async Task<int> CreateAsync(User user)
    {
        await _connection.OpenAsync();

        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                var userSQL = "INSERT INTO Users (FirstName, LastName, RoleId) VALUES (@FirstName, @LastName, @RoleId); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                var id = await _connection.QuerySingleAsync<int>(userSQL, new
                {
                    user.FirstName,
                    user.LastName,
                    user.RoleId
                }, transaction);
                
                user.Id = id;

                if (user.Address != null)
                {
                    user.Address.UserId = id;

                    var addressSQL = "INSERT INTO Addresses (UserId, Country, City, AddressLine1, AddressLine2, ZipCode) " +
                        "VALUES (@UserId, @Country, @City, @AddressLine1, @AddressLine2, @ZipCode);";

                    await _connection.ExecuteAsync(addressSQL, new
                    {
                        user.Address.UserId,
                        user.Address.Country,
                        user.Address.City,
                        user.Address.AddressLine1,
                        user.Address.AddressLine2,
                        user.Address.ZipCode,
                    }, transaction);

                    transaction.Commit();
                }

                return id;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    public async Task UpdateAsync(User user)
    {
        await _connection.OpenAsync();

        using (var transaction = _connection.BeginTransaction())
        {
            try 
            { 
                var userSQL = "UPDATE Users " +
                    "SET FirstName = @FirstName, LastName = @LastName, RoleId = @RoleId " +
                    "WHERE Id = @Id";

                await _connection.ExecuteAsync(userSQL, new
                {
                    user.FirstName,
                    user.LastName,
                    user.RoleId,
                    user.Id
                }, transaction);

                if (user.Address != null)
                {
                    var addressSQL = "UPDATE Addresses " +
                    "SET Country = @Country, City = @City, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, ZipCode = @ZipCode " +
                    "WHERE UserId = @UserId";

                    await _connection.ExecuteAsync(addressSQL, new
                    {
                        user.Address.Country,
                        user.Address.City,
                        user.Address.AddressLine1,
                        user.Address.AddressLine2,
                        user.Address.ZipCode,
                        user.Address.UserId
                    }, transaction);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.OpenAsync();

        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                await _connection.ExecuteAsync("DELETE FROM Addresses WHERE UserId = @id", new { id }, transaction);

                await _connection.ExecuteAsync("DELETE FROM Users WHERE Id = @id", new { id }, transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    private async Task<Dictionary<int, User>> GetUsersDictionaryBySQL(string sql, object? parameters = null)
    {
        var userDictionary = new Dictionary<int, User>();

        var user = await _connection.QueryAsync<User, Role, Address, User>(sql, (user, role, address) =>
        {
            if (!userDictionary.TryGetValue(user.Id, out User? currentUser))
            {
                currentUser = user;
                currentUser.Role = role;
                currentUser.Address = address;

                userDictionary.Add(currentUser.Id, currentUser);
            }

            return currentUser;

        }, parameters, splitOn: "Id, Id, UserId");

        return userDictionary;  
    }
}
