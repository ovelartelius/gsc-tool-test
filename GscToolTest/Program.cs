using GscToolTest.GoogleSearchConsole;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddSingleton<IGoogleService, GoogleService>();

//builder.Services.TryAddSingleton<ICarrierServiceFactory, CarrierServiceFactory>();

builder.Services.TryAddSingleton<IGoogleUtil, GoogleUtil>();
builder.Services.TryAddSingleton<ISearchConsoleServiceFactory, SearchConsoleServiceFactory>();

var configuration = builder.Configuration;
builder.Services.Configure<GoogleSettings>(configuration.GetSection("GoogleSettings"));

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
