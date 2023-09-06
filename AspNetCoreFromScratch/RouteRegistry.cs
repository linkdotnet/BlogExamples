using System.Reflection;

namespace AspNetCoreFromScratch;

public class RouteRegistry
{
    internal Dictionary<string, (Type Controller, MethodInfo Method)> Routes { get; } = new();

    public RouteRegistry()
    {
        // Get all controllers
        var controllers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods();
            foreach (var method in methods)
            {
                var routeAttr = method.GetCustomAttribute<RouteAttribute>();
                if (routeAttr != null)
                {
                    // Map the route to the controller and method that will be invoked
                    Routes.Add(routeAttr.Route, (controller, method));
                }
            }
        }
    }
}