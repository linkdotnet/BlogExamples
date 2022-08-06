using MediatorPattern.Infrastructure;
using MediatR;

namespace MediatorPattern.Command;

public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, User>
{
    private readonly UserRepository _userRepository;

    public AddNewUserCommandHandler(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<User> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
    {
        ColorConsole.WriteLineCommandHandler($"Passing user '{request.Name}' to repository");
        var user = new User { Name = request.Name };
        var userFromRepo = _userRepository.AddUser(user);
        ColorConsole.WriteLineCommandHandler($"Saved user to repository and return result");
        return Task.FromResult(userFromRepo);
    }
}