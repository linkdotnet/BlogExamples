using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal static class Utilities
{
    public static string AccessibilityKeyword(Accessibility a) => a switch
    {
        Accessibility.Public => "public",
        Accessibility.Internal => "internal",
        Accessibility.Private => "private",
        Accessibility.Protected => "protected",
        Accessibility.ProtectedOrInternal => "protected internal",
        Accessibility.ProtectedAndInternal => "private protected",
        _ => "internal",
    };
}
