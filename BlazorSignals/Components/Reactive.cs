using BlazorSignals.Reactivity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSignals.Components;

public sealed class Reactive : ComponentBase, IReactiveObserver, IDisposable
{
    private HashSet<IReactiveSource> dependencies = [];
    private HashSet<IReactiveSource>? dependenciesReadDuringRender;
    private bool renderRequested;
    private bool isDisposed;

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var previousDependencies = dependencies;
        var currentDependencies = new HashSet<IReactiveSource>();
        dependenciesReadDuringRender = currentDependencies;

        try
        {
            using (ReactiveContext.Observe(this))
            {
                builder.AddContent(0, ChildContent);
            }
        }
        finally
        {
            dependenciesReadDuringRender = null;

            foreach (var dependency in previousDependencies
                         .Where(dependency => !currentDependencies.Contains(dependency)))
            {
                dependency.Unsubscribe(this);
            }

            dependencies = currentDependencies;
        }
    }

    void IReactiveObserver.DependencyRead(IReactiveSource source)
    {
        if (dependenciesReadDuringRender!.Add(source) && !dependencies.Contains(source))
        {
            source.Subscribe(this);
        }
    }

    void IReactiveObserver.DependencyChanged()
    {
        if (isDisposed || Interlocked.Exchange(ref renderRequested, true))
        {
            return;
        }

        _ = InvokeAsync(() =>
        {
            Interlocked.Exchange(ref renderRequested, false);

            if (!isDisposed)
            {
                StateHasChanged();
            }
        });
    }

    public void Dispose()
    {
        isDisposed = true;

        foreach (var dependency in dependencies)
        {
            dependency.Unsubscribe(this);
        }

        dependencies.Clear();
    }
}
