using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using GenFit.Infrastructure.Data;
using GenFit.Infrastructure.HealthChecks;
using GenFit.Application.Services;
using GenFit.API.Middleware;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/genfit-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource("GenFit.API")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GenFit.API"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter());

// Add Entity Framework Core with Oracle
builder.Services.AddDbContext<GenFitDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Add Entity Framework Core with Azure SQL
builder.Services.AddDbContext<AzureSqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<OracleHealthCheck>("oracle-db")
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add services
builder.Services.AddScoped<GenFit.Infrastructure.Services.OracleProcedureService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IAzJobService, AzJobService>();

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GenFit API",
        Version = "v1",
        Description = "API RESTful para sistema de gestão de RH e candidatos - O Futuro do Trabalho"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
// Swagger habilitado em todos os ambientes para demonstração
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenFit API v1");
    c.RoutePrefix = "swagger"; // Define a rota /swagger como padrão
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// Health Check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message,
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(result);
    }
});

// Simple welcome endpoint
app.MapGet("/api/v1/wellcome", () => Results.Text("Bem-vindo à API GenFit - O Futuro do Trabalho"))
    .WithName("Wellcome")
    .WithTags("Health")
    .Produces<string>(StatusCodes.Status200OK);

// Rota raiz que redireciona para o Swagger
app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app.UseRouting();

// API Key Authentication
app.UseApiKeyAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting GenFit API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}