using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionVerify;

var services = new ServiceCollection();

services.AddScoped<TransientService>();
services.AddSingleton<SingletonService>();
services.AddScoped<ServiceWithMissingDependency>();

services.Verify();
