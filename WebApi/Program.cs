using Application;
using Persistence;
using Persistence.Seed;
using Shared;
using WebApi.Extensions;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Persistence.Contexts.ApplicationDbContext>();
    
    // As per requirements: EnsureDeleted and EnsureCreated to start fresh on every run
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    await IdentitySeed.SeedAsync(services);
}

// Middleware para redirigir "/" a "/swagger" sin generar un endpoint visible en Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(variable =>
    {
        variable.DefaultModelsExpandDepth(-1);
    });
}
app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
