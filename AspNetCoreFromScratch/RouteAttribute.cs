namespace AspNetCoreFromScratch;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RouteAttribute : Attribute
{
    public string Route { get; }
    public RouteAttribute(string route)
    {
        Route = route;
    }
}