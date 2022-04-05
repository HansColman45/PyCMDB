using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CMDB.Infrastructure.Web
{
    public class WebStartup
    {
        public int Run(string[] args, Action<IConfigurationBuilder, IHostEnvironment> configureConfiguration, 
            Action<IServiceCollection, IConfiguration, IHostEnvironment> configureServices, 
            Action<WebApplication, IServiceProvider, IHostEnvironment, IHostApplicationLifetime> configureMiddleware)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting host");
                var builder = WebApplication.CreateBuilder(args);
                configureConfiguration(builder.Configuration, builder.Environment);
                configureServices(builder.Services, builder.Configuration, builder.Environment);
                var app = builder.Build();
                configureMiddleware(app, app.Services, app.Environment, app.Lifetime);
                app.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
