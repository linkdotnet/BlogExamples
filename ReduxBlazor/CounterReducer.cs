using ReduxBlazor.Redux;

namespace ReduxBlazor;

public class CounterReducer : IReducer<CounterState>
{
    public CounterState Reduce(CounterState state, IAction action)
    {
        return action switch
        {
            IncrementAction incrementAction => state with { Count = state.Count + incrementAction.Value },
            _ => state
        };
    }
}