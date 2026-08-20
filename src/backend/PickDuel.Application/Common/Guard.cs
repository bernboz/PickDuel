using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PickDuel.Application.Common;

/// <summary>
/// Provides common guard clauses for validating method arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when a GUID is empty.
    /// </summary>
    /// <param name="id">Identifier to validate.</param>
    /// <param name="parameterName">Parameter name.</param>
    public static void AgainstEmptyGuid(Guid id, [CallerArgumentExpression(nameof(id))] string? parameterName = null)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identifier cannot be empty.",
                parameterName);
        }
    }
}