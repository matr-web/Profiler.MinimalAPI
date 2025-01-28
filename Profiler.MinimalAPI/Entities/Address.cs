namespace Profiler.MinimalAPI.Entities;

public class Address
{
    public int UserId { get; set; }

    public required string Country { get; set; }

    public required string City { get; set; }

    public required string AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public required string ZipCode { get; set; }
}
