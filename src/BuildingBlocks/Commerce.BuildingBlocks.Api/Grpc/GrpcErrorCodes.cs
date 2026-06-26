using Grpc.Core;

namespace Commerce.BuildingBlocks.Api.Grpc;

/// <summary>
/// Defines fallback error codes used when a downstream gRPC failure does not carry an application error code.
/// </summary>
public static class GrpcErrorCodes
{
    /// <summary>
    /// Error code used for transport-level invalid argument failures that are not produced by an application use case.
    /// </summary>
    public const string InvalidArgument = "Grpc.InvalidArgument";

    /// <summary>
    /// Creates a stable fallback code from the gRPC status code.
    /// </summary>
    /// <param name="statusCode">gRPC status code returned by the downstream service.</param>
    /// <returns>The fallback error code exposed to clients.</returns>
    public static string FromStatusCode(StatusCode statusCode)
    {
        return $"Grpc.{statusCode}";
    }
}
