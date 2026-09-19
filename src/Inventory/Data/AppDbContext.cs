using Inventory.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, AppRole, int>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<AppUser>(e => e.ToTable("Users"));
        modelBuilder.Entity<AppRole>(e => e.ToTable("Roles"));
        modelBuilder.Entity<IdentityUserRole<int>>(e => e.ToTable("UserRoles"));
        modelBuilder.Entity<IdentityRoleClaim<int>>(e => e.ToTable("RoleClaims"));
        modelBuilder.Entity<IdentityUserClaim<int>>(e => e.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<int>>(e => e.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<int>>(e => e.ToTable("UserTokens"));

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
