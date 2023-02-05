using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Per request we want to have the same underlying service
// for TenantGetter and TenantSetter
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<ITenantGetter>(r => r.GetRequiredService<TenantService>());
builder.Services.AddScoped<ITenantSetter>(r => r.GetRequiredService<TenantService>());

// Register our tenant services for RavenDB
// The factory should be singleton over the whole lifetime so that we don't create
// new IDocumentStores for every request
builder.Services.AddSingleton<IDocumentStoreFactory, DocumentStoreFactory>();
builder.Services.AddScoped<ITenantDocumentStore>(x =>
{
    var tenantId = x.GetRequiredService<ITenantGetter>().Tenant;
    return new TenantDocumentStore(tenantId, x.GetService<IDocumentStoreFactory>());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use our tenant middleware
app.UseMiddleware<TenantMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();