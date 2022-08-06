using MediatR;

namespace MediatorPattern.Query;

public class GetUserCountQuery : IRequest<int>
{
    public string? NameFilter { get; set; }
}