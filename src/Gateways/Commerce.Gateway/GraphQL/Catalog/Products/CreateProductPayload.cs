namespace Commerce.Gateway.GraphQL.Catalog.Products
{
    /// <summary>
    /// Represents the GraphQL payload returned after creating a product.
    /// </summary>
    /// <param name="ProductId">Identifier of the created product.</param>
    public sealed record CreateProductPayload(Guid ProductId);
}
