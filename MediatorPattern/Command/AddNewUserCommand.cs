using MediatR;

namespace MediatorPattern.Command;

public class AddNewUserCommand : IRequest<User>
{
    public string Name { get; set; }
}