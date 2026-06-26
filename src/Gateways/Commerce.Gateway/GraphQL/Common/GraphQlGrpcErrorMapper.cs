using Grpc.Core;
using HotChocolate;

namespace Commerce.Gateway.GraphQL.Common;

/// <summary>
/// Converts downstream gRPC failures into GraphQL errors exposed by the gateway.
/// </summary>
public static class GraphQlGrpcErrorMapper
{
    /// <summary>
    /// Converts a gRPC failure into a GraphQL exception.
    /// </summary>
    /// <param name="exception">gRPC exception returned by the downstream service.</param>
    /// <returns>The GraphQL exception exposed by the gateway.</returns>
    public static GraphQLException ToGraphQlException(RpcException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(exception.Status.Detail)
                .SetCode(exception.StatusCode.ToString())
                .Build());
    }
}
