var (intResult, stringResult, doubleResult) = 
    await TaskHelper.StartWith(() => Task.FromResult(10))
    .And(() => Task.FromResult("Hello World"))
    .And(() => Task.FromResult(10d))
    .WaitAllAsync();

Console.WriteLine($"Int Result: {intResult}");
Console.WriteLine($"String Result: {stringResult}");
Console.WriteLine($"Double Result: {doubleResult}");

public static class TaskHelper
{
    public static TaskHelper<T> StartWith<T>(Func<Task<T>> task)
    {
        return new TaskHelper<T>(task);
    }
}

public class TaskHelper<T>
{
    private readonly List<Func<Task<object>>> _tasks;

    public TaskHelper(Func<Task<T>> initialTask)
    {
        _tasks = [() => initialTask().ContinueWith(t => (object)t.Result)];
    }

    public TaskHelper<T, TNext> And<TNext>(Func<Task<TNext>> nextTask)
    {
        return new TaskHelper<T, TNext>(_tasks, nextTask);
    }

    public async Task<T> WaitAllAsync()
    {
        var results = await Task.WhenAll(_tasks.Select(t => t()));
        return (T)results[0];
    }
}

public class TaskHelper<T1, T2>
{
    private readonly List<Func<Task<object>>> _tasks;

    public TaskHelper(List<Func<Task<object>>> existingTasks, Func<Task<T2>> nextTask)
    {
        _tasks = [..existingTasks, () => nextTask().ContinueWith(t => (object)t.Result)];
    }

    public TaskHelper<T1, T2, T3> And<T3>(Func<Task<T3>> nextTask)
    {
        return new TaskHelper<T1, T2, T3>(_tasks, nextTask);
    }

    public async Task<(T1, T2)> WaitAllAsync()
    {
        var results = await Task.WhenAll(_tasks.Select(t => t()));
        return ((T1)results[0], (T2)results[1]);
    }
}

public class TaskHelper<T1, T2, T3>
{
    private readonly List<Func<Task<object>>> _tasks;

    public TaskHelper(List<Func<Task<object>>> existingTasks, Func<Task<T3>> nextTask)
    {
        _tasks = [..existingTasks, () => nextTask().ContinueWith(t => (object)t.Result)];
    }

    public async Task<(T1, T2, T3)> WaitAllAsync()
    {
        var results = await Task.WhenAll(_tasks.Select(t => t()));
        return ((T1)results[0], (T2)results[1], (T3)results[2]);
    }
}