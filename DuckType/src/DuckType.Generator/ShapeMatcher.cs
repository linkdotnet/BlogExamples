using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal static class ShapeMatcher
{
    public static string? FindMismatch(INamedTypeSymbol shape, INamedTypeSymbol concreteType)
    {
        foreach (var member in GetShapeMembers(shape))
        {
            switch (member)
            {
                case IMethodSymbol shapeMethod when !HasMatchingMethod(concreteType, shapeMethod):
                    return $"missing method '{shapeMethod.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}'";
                case IPropertySymbol shapeProperty:
                    var propertyMismatch = FindPropertyMismatch(concreteType, shapeProperty);
                    if (propertyMismatch is not null) return propertyMismatch;
                    break;
            }
        }
        return null;
    }

    public static IEnumerable<ISymbol> GetShapeMembers(INamedTypeSymbol shape)
    {
        foreach (var m in shape.GetMembers())
            if (IsRelevant(m)) yield return m;
        foreach (var baseInterface in shape.AllInterfaces)
            foreach (var m in baseInterface.GetMembers())
                if (IsRelevant(m)) yield return m;
    }

    private static bool IsRelevant(ISymbol member)
    {
        if (member.IsStatic) return false;
        return member switch
        {
            IMethodSymbol { MethodKind: MethodKind.Ordinary } => true,
            IPropertySymbol => true,
            _ => false,
        };
    }

    private static bool HasMatchingMethod(INamedTypeSymbol concreteType, IMethodSymbol shapeMethod)
    {
        foreach (var candidate in GetAllMembers(concreteType).OfType<IMethodSymbol>())
        {
            if (candidate.MethodKind != MethodKind.Ordinary) continue;
            if (candidate.Name != shapeMethod.Name) continue;
            if (candidate.DeclaredAccessibility != Accessibility.Public) continue;
            if (candidate.Parameters.Length != shapeMethod.Parameters.Length) continue;
            if (!SymbolEqualityComparer.Default.Equals(candidate.ReturnType, shapeMethod.ReturnType)) continue;

            var paramsMatch = true;
            for (var i = 0; i < candidate.Parameters.Length; i++)
            {
                if (!SymbolEqualityComparer.Default.Equals(candidate.Parameters[i].Type, shapeMethod.Parameters[i].Type) ||
                    candidate.Parameters[i].RefKind != shapeMethod.Parameters[i].RefKind)
                {
                    paramsMatch = false;
                    break;
                }
            }
            if (paramsMatch) return true;
        }
        return false;
    }

    private static string? FindPropertyMismatch(INamedTypeSymbol concreteType, IPropertySymbol shapeProperty)
    {
        var candidate = GetAllMembers(concreteType).OfType<IPropertySymbol>()
            .FirstOrDefault(p => p.Name == shapeProperty.Name &&
                                  p.DeclaredAccessibility == Accessibility.Public &&
                                  SymbolEqualityComparer.Default.Equals(p.Type, shapeProperty.Type));

        if (candidate is null)
            return $"missing property '{shapeProperty.Name}' of type '{shapeProperty.Type.ToDisplayString()}'";

        if (shapeProperty.GetMethod is not null && candidate.GetMethod is not { DeclaredAccessibility: Accessibility.Public })
            return $"property '{shapeProperty.Name}' has no public getter";

        if (shapeProperty.SetMethod is not null && candidate.SetMethod is not { DeclaredAccessibility: Accessibility.Public })
            return $"property '{shapeProperty.Name}' has no public setter";

        return null;
    }

    public static IEnumerable<ISymbol> GetAllMembers(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.BaseType)
            foreach (var m in current.GetMembers())
                yield return m;
    }
}
