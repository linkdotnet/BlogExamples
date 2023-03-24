using Microsoft.CodeAnalysis;

[Generator]
public sealed class IncrementalBuildInformationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilerOptions = context.CompilationProvider.Select((s, _)  => s.Options);

        context.RegisterSourceOutput(compilerOptions, static (productionContext, options) =>
        {
            var buildInformation = new BuildInformationInfo
            {
                BuildAt = DateTime.UtcNow.ToString("O"),
                Platform = options.Platform.ToString(),
                WarningLevel = options.WarningLevel,
                Configuration = options.OptimizationLevel.ToString(),
            };

            productionContext.AddSource("LinkDotNet.BuildInformation.g", GenerateBuildInformationClass(buildInformation));
        });
    }

    private static string GenerateBuildInformationClass(BuildInformationInfo buildInformation)
    {
        return $@"
using System;
using System.Globalization;

public static class BuildInformation
{{
    /// <summary>
    /// Returns the build date (UTC).
    /// </summary>
    /// <remarks>Value is: {buildInformation.BuildAt}</remarks>
    public static readonly DateTime BuildAt = DateTime.ParseExact(""{buildInformation.BuildAt}"", ""O"", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

    /// <summary>
    /// Returns the platform.
    /// </summary>
    /// <remarks>Value is: {buildInformation.Platform}</remarks>
    public const string Platform = ""{buildInformation.Platform}"";

    /// <summary>
    /// Returns the warning level.
    /// </summary>
    /// <remarks>Value is: {buildInformation.WarningLevel}</remarks>
    public const int WarningLevel = {buildInformation.WarningLevel};

    /// <summary>
    /// Returns the configuration.
    /// </summary>
    /// <remarks>Value is: {buildInformation.Configuration}</remarks>
    public const string Configuration = ""{buildInformation.Configuration}"";
}}
";
    }

    private sealed class BuildInformationInfo
    {
        public string BuildAt { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public int WarningLevel { get; set; }
        public string Configuration { get; set; } = string.Empty;
    }
}
