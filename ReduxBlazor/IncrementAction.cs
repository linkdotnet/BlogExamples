using ReduxBlazor.Redux;

namespace ReduxBlazor;

public record IncrementAction(int Value) : IAction
{
}