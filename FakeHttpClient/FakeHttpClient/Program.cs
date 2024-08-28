var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<GreeterService>(
    o => o.BaseAddress = new Uri("https://steven-giesel.com"));
var app = builder.Build();
app.MapGet("/", async (GreeterService service) => await service.GetSomethingAsync());

app.Run();

public partial class Program;

public class GreeterService
{
    private readonly HttpClient _client;

    public GreeterService(HttpClient client) => _client = client;

    public async Task<string> GetSomethingAsync() => await _client.GetStringAsync("/api");
}