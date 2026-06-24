namespace Commerce.BuildingBlocks.Application.Persistence;

/// <summary>
/// Defines the transaction boundary used by application use cases to persist aggregate changes.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all changes made during the current use case.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
