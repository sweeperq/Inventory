using Inventory.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var isDevelopment = builder.Environment.IsDevelopment();

services.AddAppData(builder.Configuration);

var app = builder.Build();
app.MapHealthChecks("/health");

app.MapGet("/", () => "Hello World!");

if (isDevelopment)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
