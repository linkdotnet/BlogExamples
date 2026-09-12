using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Loader;
using DuckType.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace DuckType.Tests;

internal static class GeneratorTestHelper
{
    private static readonly IReadOnlyList<MetadataReference> References = BuildReferences();

    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Preview)
        .WithFeatures([new KeyValuePair<string, string>("InterceptorsPreviewNamespaces", "DuckType.Generated")]);

    public static (Compilation Compilation, ImmutableArray<Diagnostic> Diagnostics) RunGenerator(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source, ParseOptions);

        var compilation = CSharpCompilation.Create(
            assemblyName: "DuckType.Tests.Generated." + Guid.NewGuid().ToString("N"),
            syntaxTrees: [syntaxTree],
            references: References,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

        var generator = new DuckTypeGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create([generator.AsSourceGenerator()], parseOptions: ParseOptions);

        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);

        var allDiagnostics = outputCompilation.GetDiagnostics()
            .Concat(generatorDiagnostics)
            .Where(d => d.Severity >= DiagnosticSeverity.Warning)
            .ToImmutableArray();

        return (outputCompilation, allDiagnostics);
    }

    public static Assembly EmitAndLoad(Compilation compilation)
    {
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            var errors = string.Join(Environment.NewLine,
                result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
            throw new InvalidOperationException("Compilation failed:" + Environment.NewLine + errors);
        }

        stream.Seek(0, SeekOrigin.Begin);
        return AssemblyLoadContext.Default.LoadFromStream(stream);
    }

    private static IReadOnlyList<MetadataReference> BuildReferences()
    {
        var trustedAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator);

        return trustedAssemblies
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToList();
    }
}
