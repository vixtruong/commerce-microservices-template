using Commerce.BuildingBlocks.Domain.Results;
using Grpc.Core;

namespace Commerce.BuildingBlocks.Api.Grpc;

/// <summary>
/// Maps application errors to gRPC transport errors used by service API projects.
/// </summary>
public static class GrpcErrorMapper
{
    /// <summary>
    /// Converts an application error to a gRPC exception.
    /// </summary>
    /// <param name="error">Application error produced by a use case.</param>
    /// <returns>The gRPC exception that represents the application error.</returns>
    public static RpcException ToRpcException(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new RpcException(
            new Status(
                ToStatusCode(error.Type),
                error.Message));
    }

    /// <summary>
    /// Creates a gRPC invalid-argument exception for transport-level validation failures.
    /// </summary>
    /// <param name="message">Client-safe validation message.</param>
    /// <returns>The gRPC exception that represents invalid input.</returns>
    public static RpcException InvalidArgument(string message)
    {
        return new RpcException(
            new Status(
                StatusCode.InvalidArgument,
                message));
    }

    /// <summary>
    /// Converts an application error type to a gRPC status code.
    /// </summary>
    /// <param name="errorType">Application error type.</param>
    /// <returns>The matching gRPC status code.</returns>
    public static StatusCode ToStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Conflict => StatusCode.AlreadyExists,
            ErrorType.Unauthorized => StatusCode.Unauthenticated,
            ErrorType.Forbidden => StatusCode.PermissionDenied,
            _ => StatusCode.Internal
        };
    }
}
