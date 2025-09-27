using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.Repositories;
using MillionRealEstatecompany.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure strongly typed settings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SectionName));

// Add services to the container.
builder.Services.AddControllers();

// Get configuration settings
var databaseSettings = builder.Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>() ?? new DatabaseSettings();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(databaseSettings.CommandTimeout);
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    });

    // Configure logging based on environment
    if (builder.Environment.IsDevelopment() && databaseSettings.EnableSensitiveDataLogging)
    {
        options.EnableSensitiveDataLogging();
    }

    if (builder.Environment.IsDevelopment() && databaseSettings.EnableDetailedErrors)
    {
        options.EnableDetailedErrors();
    }
});

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure Dependency Injection
// Repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();

// Service layer
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
builder.Services.AddScoped<IPropertyTraceService, PropertyTraceService>();

// Data seeding service
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with settings from configuration
var apiSettings = builder.Configuration.GetSection(ApiSettings.SectionName).Get<ApiSettings>() ?? new ApiSettings();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = apiSettings.Title,
        Version = apiSettings.Version,
        Description = apiSettings.Description,
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = apiSettings.ContactName,
            Email = apiSettings.ContactEmail
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Million Real Estate API V1");
        c.RoutePrefix = "swagger";
    });
}

// Auto-migrate database and seed data on startup (only in development and Docker environments)
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

    try
    {
        // Ensure database is created and migrations are applied
        context.Database.Migrate();

        // Auto-seed data if database is empty
        if (!await dataSeeder.HasDataAsync())
        {
            await dataSeeder.SeedDataAsync();
        }

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database initialized and seeded successfully");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database");
        throw;
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
