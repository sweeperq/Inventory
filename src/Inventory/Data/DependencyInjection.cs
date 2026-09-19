using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddAppData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Inventory")
            ?? throw new InvalidOperationException("Connection string 'Inventory' not found.");

        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseExceptionProcessor();
        });

        services.AddScoped(provider => provider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("Database");

        return services;
    }
}
