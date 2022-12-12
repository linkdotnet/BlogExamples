using System.Reflection;

namespace DIContainer;

public class Container
{
    private readonly Dictionary<Type, Type> _registeredTypes = new();
    private readonly Dictionary<Type, object?> _singletons = new();

    public void Register<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
    }

public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
{
    Register<TInterface, TImplementation>();

    // add the type as singleton
    _singletons[typeof(TInterface)] = null;
}

    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface));
    }

    private object Resolve(Type type)
    {
        if (_registeredTypes.ContainsKey(type))
        {
            // Check if we already have a singleton registered and we created an instance
            if (_singletons.TryGetValue(type, out var value) && value is not null)
                return _singletons[type];

            var implementationType = _registeredTypes[type];
            var constructor = implementationType.GetConstructors().First();
            var constructorParameters = constructor.GetParameters();
            object? instance = null;

            if (constructorParameters.Length == 0)
            {
                // If the constructor has no parameters, we can just create an instance
                instance = Activator.CreateInstance(implementationType);
            }
            else
            {
                var parameterInstances = GetConstructorParameters(constructorParameters);
                instance = Activator.CreateInstance(implementationType, parameterInstances.ToArray());
            }

            // If we are a singleton, add this to the dictionary
            TryAddWhenSingleton(type, instance);
            return instance;
        }

        throw new Exception($"The type {type.FullName} has not been registered");
    }

    private List<object> GetConstructorParameters(ParameterInfo[] constructorParameters)
    {
        var parameterInstances = new List<object>();
        foreach (var parameter in constructorParameters)
        {
            var parameterType = parameter.ParameterType;
            var parameterInstance = Resolve(parameterType);
            parameterInstances.Add(parameterInstance);
        }

        return parameterInstances;
    }

    private void TryAddWhenSingleton(Type type, object instance)
    {
        if (_singletons.ContainsKey(type))
            _singletons[type] = instance;
    }
}