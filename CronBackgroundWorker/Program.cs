using CronBackgroundWorker;
using CronBackgroundWorker.Cron;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCronJob<CronJob>("* * * * *");
builder.Services.AddCronJob<AnotherCronJob>("*/2 * * * *");

var app = builder.Build();

app.Run();