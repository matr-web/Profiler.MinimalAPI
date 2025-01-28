using Profiler.MinimalAPI.Entities;

namespace Profiler.MinimalAPI.Services;

public interface IAuthService
{
    public string GenerateToken(User user);
}
