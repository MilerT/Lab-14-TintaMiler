using System.Text;
using LAB10_TINTAMILER.INFRASTRUCTURE.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace LAB10_TINTAMILER.API.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuración de controladores
        services.AddControllers();

        // 2. Configuración de Swagger con soporte para JWT Bearer
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ticketera API - Clean Architecture + Lab 13 ClosedXML",
                Version = "v1",
                Description = """
                    API empresarial para gestión de tickets y soporte con autenticación JWT, Unit of Work y Patrón Repositorio.
                    
                    ## Laboratorio 13 – Reportes Excel con ClosedXML
                    
                    Endpoints de reportes (no requieren autenticación JWT):
                    
                    - **GET /api/reports/users-with-tickets** → Reporte 1: Usuarios con resumen de sus Tickets (archivo .xlsx)
                    - **GET /api/reports/tickets-with-responses** → Reporte 2: Tickets con sus Respuestas, 2 hojas Excel (archivo .xlsx)
                    
                    Implementación: Repository Pattern + CQRS (Queries) + ClosedXML en Infrastructure.
                    """
            });

            // Definición de esquema de seguridad para JWT en Swagger usando HTTP Bearer
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Pegue aquí directamente su Token JWT (Swagger agregará automáticamente el prefijo 'Bearer ').",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
            });
        });

        // 3. Configuración de Autenticación JWT Bearer
        var secretKey = configuration["JwtSettings:SecretKey"] ?? "SuperSecretKeyForTicketeraLab10TecsupEnterpriseApplication2026!";
        var issuer = configuration["JwtSettings:Issuer"] ?? "TicketeraAPI";
        var audience = configuration["JwtSettings:Audience"] ?? "TicketeraUsers";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[JWT ERROR] Falló la autenticación: {context.Exception.Message}");
                    Console.ResetColor();
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[JWT CHALLENGE] {context.Error} - {context.ErrorDescription}");
                    Console.ResetColor();
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        // 4. Inyección de servicios de Infraestructura (EF Core PostgreSQL, UnitOfWork, Repositorios)
        services.AddInfrastructureServices(configuration);

        return services;
    }
}
