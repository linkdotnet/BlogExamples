namespace ServiceCollectionVerify;

// Ohoh - a singleton with a dependency on a scoped service -> captive dependency
public class SingletonService
{
    private readonly TransientService _transientService;

    public SingletonService(TransientService transientService)
    {
        _transientService = transientService;
    }
}
public class TransientService { }

// Service with a missing dependency
public class ServiceWithMissingDependency
{
    public ServiceWithMissingDependency(MissingDependency missingDependency)
    {
    }
}

// Does not get registered in the service collection
public class MissingDependency
{
}