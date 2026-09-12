using System.Text;
using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal static class TypeWrapper
{
    public static string WrapInContainingScope(INamedTypeSymbol type, string memberSource)
    {
        var typeChain = new List<INamedTypeSymbol>();
        for (var t = type; t is not null; t = t.ContainingType)
            typeChain.Add(t);
        typeChain.Reverse();

        var sb = new StringBuilder();
        var ns = type.ContainingNamespace;
        var hasNamespace = ns is { IsGlobalNamespace: false };
        if (hasNamespace)
        {
            sb.AppendLine($"namespace {ns.ToDisplayString()}");
            sb.AppendLine("{");
        }

        var indent = hasNamespace ? "    " : "";
        foreach (var t in typeChain)
        {
            var kind = t.TypeKind == TypeKind.Struct ? "struct" : "class";
            var staticMod = t.IsStatic ? "static " : "";
            sb.AppendLine($"{indent}{Utilities.AccessibilityKeyword(t.DeclaredAccessibility)} {staticMod}partial {kind} {t.Name}{TypeParams(t)}");
            sb.AppendLine($"{indent}{{");
            indent += "    ";
        }

        foreach (var line in memberSource.Split('\n'))
            sb.AppendLine(indent + line.TrimEnd('\r'));

        for (var i = 0; i < typeChain.Count; i++)
        {
            indent = indent.Substring(0, indent.Length - 4);
            sb.AppendLine(indent + "}");
        }

        if (hasNamespace)
            sb.AppendLine("}");

        return sb.ToString();
    }

    private static string TypeParams(INamedTypeSymbol t) =>
        t.TypeParameters.Length == 0 ? "" : $"<{string.Join(", ", t.TypeParameters.Select(p => p.Name))}>";
}
