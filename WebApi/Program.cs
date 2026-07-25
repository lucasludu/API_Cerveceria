using Application;
using Persistence;
using Shared;
using WebApi.Extensions;
using WebApi.Middleware;
using Serilog;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRedisCacheExtension(builder.Configuration);
builder.Services.AddRateLimitingExtension();
builder.Services.AddHealthCheckExtension();
builder.Services.AddOpenTelemetryExtension();

var app = builder.Build();

await app.SeedDatabaseAsync();

// Middleware para redirigir "/" a "/scalar/v1" sin generar un endpoint visible en Scalar
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/scalar/v1");
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}
app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHealthCheckExtension();

app.Run();

public partial class Program { }
