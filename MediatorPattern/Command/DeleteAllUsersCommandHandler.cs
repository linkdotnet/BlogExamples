using MediatorPattern.Infrastructure;
using MediatR;

namespace MediatorPattern.Command;

public class DeleteAllUsersCommandHandler : IRequestHandler<DeleteAllUsersCommand, Unit>
{
    private readonly UserRepository _userRepository;

    public DeleteAllUsersCommandHandler(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<Unit> Handle(DeleteAllUsersCommand request, CancellationToken cancellationToken)
    {
        ColorConsole.WriteLineCommandHandler($"Deleting all users");
        _userRepository.DeleteAll();
        ColorConsole.WriteLineCommandHandler($"All users deleted");
        return Task.FromResult(Unit.Value);
    }
}