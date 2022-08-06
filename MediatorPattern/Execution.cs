using MediatorPattern.Command;
using MediatorPattern.Query;
using MediatR;

namespace MediatorPattern;

public class Execution
{
    private readonly IMediator _mediator;

    public Execution(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task RunAsync()
    {
        ColorConsole.WriteLineProgram("Creating a new user with name 'Steven'");
        var user = await _mediator.Send(new AddNewUserCommand { Name = "Steven" });
        ColorConsole.WriteLineProgram($"User has id: {user.Id}");

        Console.WriteLine();
        
        ColorConsole.WriteLineProgram($"Get count of users with name 'Rebecca'");
        var count = await _mediator.Send(new GetUserCountQuery { NameFilter = "Rebecca" });
        ColorConsole.WriteLineProgram($"Found {count} entries");
        
        Console.WriteLine();
        
        ColorConsole.WriteLineProgram($"Deleting all users");
        await _mediator.Send(new DeleteAllUsersCommand());
        ColorConsole.WriteLineProgram($"All users deleted");
        
    }
}