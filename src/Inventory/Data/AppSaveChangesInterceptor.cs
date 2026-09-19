using Inventory.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Inventory.Data;

public class AppSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null)
            return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is ICreatedAt && e.State is EntityState.Added);
        
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in entries)
        {
            entry.Property(nameof(ICreatedAt.CreatedAt)).CurrentValue = now;
        }
    }
}
