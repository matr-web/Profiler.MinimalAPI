using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Services;

public interface IUserService
{
    public Task<IEnumerable<User>?> GetAllAsync();
    public Task<User?> GetByIdAsync(int id);
    public Task<int> CreateAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(int id);
}
