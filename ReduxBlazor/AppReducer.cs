using ReduxBlazor.Redux;

namespace ReduxBlazor;

public class AppReducer : IReducer<ApplicationState>
{
    private readonly CounterReducer _counterReducer = new();

    public ApplicationState Reduce(ApplicationState state, IAction action)
    {
        return new ApplicationState(_counterReducer.Reduce(state.CounterState, action));
    }
}