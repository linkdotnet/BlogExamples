namespace ReduxBlazor.Redux;

public interface IReducer<TState>
{
    TState Reduce(TState state, IAction action);
}
