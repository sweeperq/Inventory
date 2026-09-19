namespace Inventory.Domain.Abstractions;

public interface ICreatedAt
{
    DateTimeOffset CreatedAt { get; }
}
