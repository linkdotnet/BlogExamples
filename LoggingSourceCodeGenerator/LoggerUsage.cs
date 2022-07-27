using Microsoft.Extensions.Logging;

namespace LoggingSourceCodeGenerator;

public partial class UserCreationService
{
    private ILogger _logger;

    public UserCreationService(ILogger logger)
    {
        _logger = logger;
    }

    public void LogCreateUserBad(User user, DateTime when)
    {
        // That is the "worst" approach as you lose structured logging
        // Helpers like JetBrains Resharper or Rider will even warn you
        _logger.LogInformation($"Creating user: {user} at {when}");
    }

    public void LogCreateUserTraditionally(User user, DateTime when)
    {
        _logger.LogInformation("Creating user: {User} at {When}", user, when);
    }

    // The function has to be partial as the source code generator has to provide the implementation
    // We can also set the LogLevel and an EventId
    // The source code generator will automatically look for an `ILogger` field to log its stuff
    [LoggerMessage(Message = "Creating user: {user} at {when}", Level = LogLevel.Information, EventId = 1)]
    public partial void LogCreateUser(User user, DateTime when);

    [LoggerMessage(Message = "Creating user: {user} at {when}", Level = LogLevel.Information, EventId = 1)]
    public static partial void LogStaticVersion(ILogger logger, User user, DateTime when);

    public static void ExampleUsage()
    {
        var logger = LoggerFactory.Create(l => l.AddConsole()).CreateLogger(typeof(UserCreationService));
        var userService = new UserCreationService(logger);
        var user = new User("Steven", "Giesel");
        userService.LogCreateUser(user, DateTime.Now);
    }
}

public record User(string FirstName, string LastName);