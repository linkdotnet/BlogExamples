namespace BlazorSignals.Reactivity;

public static class Signals
{
    public static Signal<T> Signal<T>(T initialValue) => new(initialValue);

    public static Computed<T> Computed<T>(Func<T> computation) => new(computation);
}

public sealed class Signal<T> : IReactiveSource
{
    private readonly Lock gate = new();
    private readonly HashSet<IReactiveObserver> observers = [];
    private T value;

    internal Signal(T initialValue)
    {
        value = initialValue;
    }

    public T Value
    {
        get
        {
            ReactiveContext.Track(this);

            lock (gate)
            {
                return value;
            }
        }
    }

    public void Set(T newValue)
    {
        IReactiveObserver[] observersToNotify;

        lock (gate)
        {
            if (EqualityComparer<T>.Default.Equals(value, newValue))
            {
                return;
            }

            value = newValue;
            observersToNotify = [.. observers];
        }

        Notify(observersToNotify);
    }

    public void Update(Func<T, T> updater)
    {
        ArgumentNullException.ThrowIfNull(updater);

        IReactiveObserver[] observersToNotify;

        lock (gate)
        {
            var newValue = updater(value);
            if (EqualityComparer<T>.Default.Equals(value, newValue))
            {
                return;
            }

            value = newValue;
            observersToNotify = [.. observers];
        }

        Notify(observersToNotify);
    }

    void IReactiveSource.Subscribe(IReactiveObserver observer)
    {
        lock (gate)
        {
            observers.Add(observer);
        }
    }

    void IReactiveSource.Unsubscribe(IReactiveObserver observer)
    {
        lock (gate)
        {
            observers.Remove(observer);
        }
    }

    private static void Notify(IReactiveObserver[] observersToNotify)
    {
        foreach (var observer in observersToNotify)
        {
            observer.DependencyChanged();
        }
    }
}

public sealed class Computed<T> : IReactiveSource, IReactiveObserver
{
    private readonly Func<T> computation;
    private readonly Lock gate = new();
    private readonly HashSet<IReactiveObserver> observers = [];
    private readonly HashSet<IReactiveSource> dependencies = [];
    private T? value;
    private bool isDirty = true;
    private bool isComputing;

    internal Computed(Func<T> computation)
    {
        ArgumentNullException.ThrowIfNull(computation);
        this.computation = computation;
    }

    public T Value
    {
        get
        {
            ReactiveContext.Track(this);

            lock (gate)
            {
                if (!isDirty)
                {
                    return value!;
                }

                if (isComputing)
                {
                    throw new InvalidOperationException("A computed value cannot depend on itself.");
                }

                isComputing = true;

                try
                {
                    ClearDependencies();

                    using (ReactiveContext.Observe(this))
                    {
                        value = computation();
                    }

                    isDirty = false;
                    return value;
                }
                finally
                {
                    isComputing = false;
                }
            }
        }
    }

    void IReactiveObserver.DependencyRead(IReactiveSource source)
    {
        lock (gate)
        {
            if (dependencies.Add(source))
            {
                source.Subscribe(this);
            }
        }
    }

    void IReactiveObserver.DependencyChanged()
    {
        IReactiveObserver[] observersToNotify;

        lock (gate)
        {
            if (isDirty)
            {
                return;
            }

            isDirty = true;
            observersToNotify = [.. observers];
        }

        foreach (var observer in observersToNotify)
        {
            observer.DependencyChanged();
        }
    }

    void IReactiveSource.Subscribe(IReactiveObserver observer)
    {
        lock (gate)
        {
            observers.Add(observer);
        }
    }

    void IReactiveSource.Unsubscribe(IReactiveObserver observer)
    {
        lock (gate)
        {
            observers.Remove(observer);
        }
    }

    private void ClearDependencies()
    {
        foreach (var dependency in dependencies)
        {
            dependency.Unsubscribe(this);
        }

        dependencies.Clear();
    }
}

internal interface IReactiveSource
{
    void Subscribe(IReactiveObserver observer);

    void Unsubscribe(IReactiveObserver observer);
}

internal interface IReactiveObserver
{
    void DependencyRead(IReactiveSource source);

    void DependencyChanged();
}

internal static class ReactiveContext
{
    private static readonly AsyncLocal<IReactiveObserver?> CurrentObserver = new();

    public static IDisposable Observe(IReactiveObserver observer)
    {
        var previousObserver = CurrentObserver.Value;
        CurrentObserver.Value = observer;
        return new ObservationScope(previousObserver);
    }

    public static void Track(IReactiveSource source) =>
        CurrentObserver.Value?.DependencyRead(source);

    private sealed class ObservationScope(IReactiveObserver? previousObserver) : IDisposable
    {
        public void Dispose() => CurrentObserver.Value = previousObserver;
    }
}
