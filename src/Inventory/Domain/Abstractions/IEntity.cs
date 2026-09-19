namespace Inventory.Domain.Abstractions;

public interface IEntity
{
}

public interface IEntity<TId> : IEntity
    where TId : IEquatable<TId>
{
    TId Id { get; }
}
