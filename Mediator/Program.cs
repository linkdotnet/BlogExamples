using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddMediator()
    .AddScoped<ProducerService>()
    .BuildServiceProvider();

var producer = services.GetRequiredService<ProducerService>();
producer.ProduceEmail("Hello World");
