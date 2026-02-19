using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CobaltAvaloniaDesktopTester;

public class ApiHostedService(IConfiguration configuration) : IHostedService
{
    private WebApplication? _app;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var port = configuration.GetValue("Api:Port", 5100);
        var title = configuration.GetValue("Api:Title", "Cobalt Tester API");

        var builder = WebApplication.CreateSlimBuilder();
        builder.WebHost.UseUrls($"http://localhost:{port}");

        _app = builder.Build();

        _app.MapGet("/", () => Results.Ok(new { Title = title, Status = "Running" }));
        _app.MapGet("/api/info", () => Results.Ok(new
        {
            Title = title,
            Port = port,
            Environment.MachineName,
            StartedAt = DateTime.UtcNow
        }));

        await _app.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_app is not null)
        {
            await _app.StopAsync(cancellationToken);
            await _app.DisposeAsync();
        }
    }
}
