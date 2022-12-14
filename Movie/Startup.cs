using Autofac.Core;
using k8s.Util.Common;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Movie.Models;
using Serilog;

public class Startup
{
    public IConfiguration configRoot
    {
        get;
    }
    public Startup(IConfiguration configuration, IWebHostEnvironment env, ILoggingBuilder loggerFactory)
    {
        var envname = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (envname.ToLower() != "production")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{envname}.json", optional: true)
                .AddEnvironmentVariables();

            configRoot = builder.Build();
        } else
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            configRoot = builder.Build();
        }

        loggerFactory.AddFile("Logs/mylog-{Date}.txt");
    }
    public void ConfigureServices(IServiceCollection services)
    {
        //var section = configRoot.GetSection("MovieReviewDatabase");
        //var sectionExists = section.Exists();

        //if (!sectionExists)

        // Add services to the container.
        services.Configure<DBSetting>(
            configRoot.GetSection("MovieReviewDatabase"));

        services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true));
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}