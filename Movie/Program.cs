using DnsClient;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Azure;
using Movie.Context;
using Movie.Models;
using Movie.Services;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.Configure<DBSetting>(
//    builder.Configuration.GetSection("MovieReviewDatabase"));

builder.Services.AddSingleton<MovieReviewService>();

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var startup = new Startup(builder.Configuration, builder.Environment, builder.Logging);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method