using Inventory.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Identity;

public class AppUser : IdentityUser<int>, IEntity<int>, ICreatedAt
{
    private AppUser() { }

    public AppUser(string userName) : base(userName)
    {
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public bool Disabled { get; set; }
    public DateTimeOffset CreatedAt { get; protected set; }
}
