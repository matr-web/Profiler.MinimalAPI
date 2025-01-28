namespace Profiler.MinimalAPI.Entities;

public class User
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int RoleId { get; set; }

    public Role? Role { get; set; }

    public Address? Address { get; set; }
}
