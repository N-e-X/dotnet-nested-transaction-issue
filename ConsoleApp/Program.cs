using ConsoleApp;
using ConsoleApp.Database;
using ConsoleApp.Examples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AppDbConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(connectionString),
    contextLifetime: ServiceLifetime.Singleton,
    optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IExample, WhenThrowsExceptionExample>();
builder.Services.AddSingleton<IExample, WhenReturnsWithoutCallingCompleteExample>();

var host = builder.Build();

using var db = host.Services.GetRequiredService<AppDbContext>();
db.Database.Migrate();

host.Run();
