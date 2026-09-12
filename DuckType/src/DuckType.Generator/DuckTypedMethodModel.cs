using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal sealed class DuckTypedMethodModel(IMethodSymbol method, INamedTypeSymbol? shape, ImmutableArray<Diagnostic> diagnostics)
{
    public IMethodSymbol Method { get; } = method;
    public INamedTypeSymbol? Shape { get; } = shape;
    public ImmutableArray<Diagnostic> Diagnostics { get; } = diagnostics;
}
