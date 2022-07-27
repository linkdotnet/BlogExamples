using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LoggingSourceCodeGenerator;
using Microsoft.Extensions.Logging;

BenchmarkRunner.Run<Benchmark>();

[MemoryDiagnoser]
public class Benchmark
{
    private readonly UserCreationService _service = new(new NoLogger());
    private readonly User _user = new("Steven", "Giesel");
    private readonly DateTime _now = DateTime.UtcNow;

    [Benchmark]
    public void LogCreateUserBad() => _service.LogCreateUserBad(_user, _now);

    [Benchmark]
    public void LogCreateUserTraditionally() => _service.LogCreateUserTraditionally(_user, _now);

    [Benchmark]
    public void LogCreateUser() => _service.LogCreateUser(_user, _now);
}

public class NoLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    { }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => default;
}