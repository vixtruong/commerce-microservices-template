using Grpc.Core;

namespace Commerce.BuildingBlocks.Api.Grpc;

/// <summary>
/// Defines gRPC metadata keys used to preserve application error information across service boundaries.
/// </summary>
public static class GrpcErrorMetadata
{
    /// <summary>
    /// Trailer key that carries the original application error code.
    /// </summary>
    public const string ErrorCodeKey = "error-code";

    /// <summary>
    /// Creates gRPC response trailers containing the supplied application error code.
    /// </summary>
    /// <param name="errorCode">Application error code that should be propagated to the caller.</param>
    /// <returns>Metadata trailers containing the error code when it is available.</returns>
    public static Metadata CreateTrailers(string errorCode)
    {
        Metadata trailers = new Metadata();

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            trailers.Add(ErrorCodeKey, errorCode);
        }

        return trailers;
    }

    /// <summary>
    /// Reads the propagated application error code from a gRPC exception.
    /// </summary>
    /// <param name="exception">gRPC exception returned by a downstream service.</param>
    /// <returns>The propagated application error code when present; otherwise, null.</returns>
    public static string? GetErrorCode(RpcException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        foreach (Metadata.Entry trailer in exception.Trailers)
        {
            if (string.Equals(trailer.Key, ErrorCodeKey, StringComparison.OrdinalIgnoreCase))
            {
                return trailer.Value;
            }
        }

        return null;
    }
}
