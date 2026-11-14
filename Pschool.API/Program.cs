using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Pschool.API.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- Services ----

builder.Services.AddControllers();

// Describe numbers strictly as numbers in the API description,
// so the Swagger test page handles Id boxes correctly.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict);

// Connect EF Core to PostgreSQL using the address in appsettings.json.
builder.Services.AddDbContext<PschoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Allow the Blazor client to call this API from its own address.
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("Client", policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()));

// Machine-readable API description, used by the Swagger test page.
builder.Services.AddOpenApi();

var app = builder.Build();

// ---- Startup options (off unless turned on in settings) ----

var applyMigrations = app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup");
var seedSampleData = app.Configuration.GetValue<bool>("Database:SeedSampleData");
var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");

if (applyMigrations || seedSampleData)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PschoolDbContext>();

    // Creates or updates the tables.
    if (applyMigrations)
    {
        await context.Database.MigrateAsync();
    }

    // Adds sample parents and students if the database is empty.
    if (seedSampleData)
    {
        await DbSeeder.SeedAsync(context);
    }
}

// Test page for trying the API in the browser: /swagger
if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Pschool API");
    });
}

// ---- Request pipeline (order matters) ----

app.UseHttpsRedirection();

// Answer the browser's cross-origin check before anything else runs.
app.UseCors("Client");

app.UseAuthorization();

app.MapControllers();

app.Run();