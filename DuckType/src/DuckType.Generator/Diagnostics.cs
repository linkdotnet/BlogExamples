using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal static class Diagnostics
{
    public static readonly DiagnosticDescriptor ShapeMismatch = new(
        id: "DUCK001",
        title: "Argument does not structurally satisfy duck shape",
        messageFormat: "Type '{0}' does not structurally satisfy shape '{1}': {2}",
        category: "DuckType",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ContainingTypeNotPartial = new(
        id: "DUCK002",
        title: "Duck-typed method's containing type must be partial",
        messageFormat: "Method '{0}' is marked [DuckTyped] but its containing type '{1}' is not declared 'partial'",
        category: "DuckType",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ParameterNotShape = new(
        id: "DUCK003",
        title: "Duck-typed parameter must be a [DuckShape] interface",
        messageFormat: "Method '{0}' is marked [DuckTyped] but its parameter type '{1}' is not an interface marked [DuckShape]",
        category: "DuckType",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UnsupportedSignature = new(
        id: "DUCK004",
        title: "Unsupported [DuckTyped] method signature",
        messageFormat: "Method '{0}' is marked [DuckTyped] but {1}. [DuckTyped] methods currently support exactly one parameter, of a [DuckShape] interface type",
        category: "DuckType",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UnsupportedShapeMember = new(
        id: "DUCK005",
        title: "Unsupported shape member",
        messageFormat: "Shape '{0}' member '{1}' is not supported (only ordinary instance methods and properties are)",
        category: "DuckType",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
