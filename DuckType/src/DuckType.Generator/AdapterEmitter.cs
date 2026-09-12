using System.Text;
using Microsoft.CodeAnalysis;

namespace DuckType.Generator;

internal static class AdapterEmitter
{
    public static string GetAdapterName(INamedTypeSymbol shape, INamedTypeSymbol concreteType) =>
        $"ShapeAdapter_{Sanitize(shape.ToDisplayString())}_{Sanitize(concreteType.ToDisplayString())}";

    public static void Emit(StringBuilder sb, INamedTypeSymbol shape, INamedTypeSymbol concreteType, string adapterName)
    {
        var concreteTypeName = concreteType.ToDisplayString();

        sb.AppendLine($"    internal readonly struct {adapterName} : global::{shape.ToDisplayString()}");
        sb.AppendLine("    {");
        sb.AppendLine($"        private readonly {concreteTypeName} _value;");
        sb.AppendLine($"        public {adapterName}({concreteTypeName} value) => _value = value;");

        foreach (var member in ShapeMatcher.GetShapeMembers(shape))
        {
            switch (member)
            {
                case IMethodSymbol method:
                    EmitMethod(sb, method);
                    break;
                case IPropertySymbol property:
                    EmitProperty(sb, property);
                    break;
            }
        }

        sb.AppendLine("    }");
    }

    private static void EmitMethod(StringBuilder sb, IMethodSymbol method)
    {
        var parameters = string.Join(", ", method.Parameters.Select(p => $"{RefKindPrefix(p.RefKind)}{p.Type.ToDisplayString()} {p.Name}"));
        var args = string.Join(", ", method.Parameters.Select(p => $"{RefKindPrefix(p.RefKind)}{p.Name}"));
        sb.AppendLine($"        public {method.ReturnType.ToDisplayString()} {method.Name}({parameters}) => _value.{method.Name}({args});");
    }

    private static void EmitProperty(StringBuilder sb, IPropertySymbol property)
    {
        sb.Append($"        public {property.Type.ToDisplayString()} {property.Name} {{ ");
        if (property.GetMethod is not null) sb.Append($"get => _value.{property.Name}; ");
        if (property.SetMethod is not null) sb.Append($"set => _value.{property.Name} = value; ");
        sb.AppendLine("}");
    }

    private static string RefKindPrefix(RefKind kind) => kind switch
    {
        RefKind.Ref => "ref ",
        RefKind.Out => "out ",
        RefKind.In => "in ",
        _ => "",
    };

    private static string Sanitize(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var c in s)
            sb.Append(char.IsLetterOrDigit(c) ? c : '_');
        return sb.ToString();
    }
}
