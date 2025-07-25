using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Warning);
builder.Services
    .AddMcpServer()
    .WithHttpTransport("http://localhost:5000")
    .WithTools<RandomNumberTools>();

await builder.Build().RunAsync();
