using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp;

public sealed class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _host;
    private readonly IEnumerable<IExample> _examples;
    private readonly ILogger<Worker> _logger;

    public Worker(IHostApplicationLifetime host, IEnumerable<IExample> examples, ILogger<Worker> logger)
    {
        _host = host;
        _examples = examples;
        _logger = logger;
    }

    /// <summary>
    /// Runs examples of nested transactions issue
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var example in _examples)
        {
            var exampleName = example.GetType().Name;
            _logger.LogInformation("""
                ==============================================================
                ==============================================================
                {ExampleName} started...
                ==============================================================
                ==============================================================
                """, exampleName);
            try
            {
                example.Outer();
            }
            catch (Exception)
            {
            }
            finally
            {
                _logger.LogInformation("""
                ==============================================================
                ==============================================================
                {ExampleName} finished...
                ==============================================================
                ==============================================================
                """, exampleName);
            }
        }

        _host.StopApplication();
        return Task.CompletedTask;
    }
}
