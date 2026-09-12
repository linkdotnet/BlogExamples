using Microsoft.CodeAnalysis;
using Xunit;

namespace DuckType.Tests;

public class DuckTypeGeneratorTests
{
    [Fact]
    public void MethodShape_DuckTypesUnrelatedClasses_WithoutSharedInterface()
    {
        const string source = """
            using DuckType;
            using System.Text;

            [DuckShape]
            public interface IDoable { void Do(); }

            public class A { public void Do() { Sink.Log.Append("A.Do;"); } }
            public class B { public void Do() { Sink.Log.Append("B.Do;"); } }

            public static class Sink { public static StringBuilder Log = new(); }

            public static partial class Ops
            {
                [DuckTyped]
                public static void Foo(IDoable a) => a.Do();
            }

            public static class Entry
            {
                public static string Run()
                {
                    Ops.Foo(new A());
                    Ops.Foo(new B());
                    return Sink.Log.ToString();
                }
            }
            """;

        var (compilation, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var assembly = GeneratorTestHelper.EmitAndLoad(compilation);

        var typeA = assembly.GetType("A")!;
        var typeB = assembly.GetType("B")!;
        Assert.Empty(typeA.GetInterfaces());
        Assert.Empty(typeB.GetInterfaces());

        var result = assembly.GetType("Entry")!.GetMethod("Run")!.Invoke(null, null);
        Assert.Equal("A.Do;B.Do;", result);
    }

    [Fact]
    public void PropertyShape_DuckTypesReadWriteProperty()
    {
        const string source = """
            using DuckType;

            [DuckShape]
            public interface INameable { string Name { get; set; } }

            public class Person { public string Name { get; set; } = ""; }

            public static partial class Ops
            {
                [DuckTyped]
                public static string Greet(INameable n)
                {
                    n.Name = n.Name.ToUpperInvariant();
                    return "Hello, " + n.Name;
                }
            }

            public static class Entry
            {
                public static string Run()
                {
                    var person = new Person { Name = "steven" };
                    return Ops.Greet(person);
                }
            }
            """;

        var (compilation, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var assembly = GeneratorTestHelper.EmitAndLoad(compilation);
        var result = assembly.GetType("Entry")!.GetMethod("Run")!.Invoke(null, null);
        Assert.Equal("Hello, STEVEN", result);
    }

    [Fact]
    public void StructuralMismatch_ReportsDuck001()
    {
        const string source = """
            using DuckType;

            [DuckShape]
            public interface IDoable { void Do(); }

            public class C { public string NotDo() => ""; }

            public static partial class Ops
            {
                [DuckTyped]
                public static void Foo(IDoable a) => a.Do();
            }

            public static class Entry
            {
                public static void Run() => Ops.Foo(new C());
            }
            """;

        var (_, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Contains(diagnostics, d => d.Id == "DUCK001");
    }

    [Fact]
    public void NonPartialContainingType_ReportsDuck002()
    {
        const string source = """
            using DuckType;

            [DuckShape]
            public interface IDoable { void Do(); }

            public static class Ops
            {
                [DuckTyped]
                public static void Foo(IDoable a) => a.Do();
            }
            """;

        var (_, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Contains(diagnostics, d => d.Id == "DUCK002");
    }

    [Fact]
    public void NonShapeParameter_ReportsDuck003()
    {
        const string source = """
            using DuckType;

            public interface IDoable { void Do(); }

            public static partial class Ops
            {
                [DuckTyped]
                public static void Foo(IDoable a) => a.Do();
            }
            """;

        var (_, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Contains(diagnostics, d => d.Id == "DUCK003");
    }

    [Fact]
    public void TypeThatAlreadyImplementsShape_StillDispatchesCorrectly()
    {
        const string source = """
            using DuckType;

            [DuckShape]
            public interface IDoable { void Do(); }

            public class RealImpl : IDoable { public void Do() { } }

            public static partial class Ops
            {
                [DuckTyped]
                public static string Foo(IDoable a) { a.Do(); return "ok"; }
            }

            public static class Entry
            {
                public static string Run() => Ops.Foo(new RealImpl());
            }
            """;

        var (compilation, diagnostics) = GeneratorTestHelper.RunGenerator(source);
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var assembly = GeneratorTestHelper.EmitAndLoad(compilation);
        var result = assembly.GetType("Entry")!.GetMethod("Run")!.Invoke(null, null);
        Assert.Equal("ok", result);
    }
}
