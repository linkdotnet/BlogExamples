using ReduxBlazor.Redux;

namespace ReduxBlazor;

public class Store
{
    private ApplicationState _state;
    private readonly IReducer<ApplicationState> _reducer;

    public Store(IReducer<ApplicationState> reducer)
    {
        _reducer = reducer;
        _state = new ApplicationState(new CounterState());
    }

    public ApplicationState GetState() => _state;

    public void Dispatch(IAction action)
    {
        _state = _reducer.Reduce(_state, action);
    }
}