namespace Commerce.BuildingBlocks.Domain.Entities
{
    /// <summary>
    /// Base class cho domain entity có identity.
    /// </summary>
    public abstract class Entity<TId>
        where TId : notnull
    {
        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; protected set; } = default!;
    }
}
