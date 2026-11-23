using LAB10_TINTAMILER.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure dynamic port binding for Render / Cloud hosting
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Enable Swagger UI for development and cloud environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketera API - Clean Architecture v1");
    c.RoutePrefix = string.Empty; // Swagger UI at root URL
});

// JWT Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint for Render and Cloud monitoring
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    application = "LAB10-TINTAMILER.API",
    version = "1.0.0",
    architecture = "Clean Architecture + CQRS MediatR",
    student = "Miler Andre Tinta Quispe",
    section = "6C24-B",
    course = "Desarrollo de Aplicaciones Empresariales Avanzadas",
    professor = "Ing. Jhordan Leonardo Mayhua Ubillas",
    institution = "TECSUP",
    year = 2026
}));

app.MapControllers();

await app.InitializeTicketeraAsync();
app.Run();
