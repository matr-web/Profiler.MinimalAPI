using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Services;

public interface IRoleService
{
    public Task<Role?> GetByIdAsync(int id);
    public Task CreateAsync(Role role);
    public Task DeleteAsync(int id);
}
