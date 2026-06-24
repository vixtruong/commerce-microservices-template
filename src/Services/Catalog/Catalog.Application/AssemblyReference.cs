using System.Reflection;

namespace Catalog.Application;

/// <summary>
/// Exposes the Catalog application assembly for dependency-registration scanning.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// Gets the assembly containing Catalog application use cases and event handlers.
    /// </summary>
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
