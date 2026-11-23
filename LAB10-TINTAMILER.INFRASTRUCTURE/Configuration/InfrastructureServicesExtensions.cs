using LAB10_TINTAMILER.APPLICATION.Configuration;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.APPLICATION.Services;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Repositories;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;
using LAB10_TINTAMILER.INFRASTRUCTURE.Services;
using LAB10_TINTAMILER.APPLICATION.UseCases.Users.Queries;
using LAB10_TINTAMILER.APPLICATION.UseCases.Tickets.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Compatibilidad para timestamp without time zone de PostgreSQL
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // 1. Configuración de Base de Datos PostgreSQL con Entity Framework Core
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Host=localhost;Database=ticketera_db;Username=postgres;Password=aL955TEC5698";

        services.AddDbContext<TicketeraDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Registro de Repositorios y Unit of Work (existentes)
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 3. Registro de Casos de Uso de Aplicación (Clean Architecture)
        services.AddApplicationUseCases();

        // 4. Registro de Servicios de Aplicación y Autenticación JWT (existentes)
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITicketService, TicketService>();

        // ============================================================
        // 5. LAB 13 – Repositorios de Reportes (Repository Pattern)
        // ============================================================
        services.AddScoped<IUserReportRepository, UserReportRepository>();
        services.AddScoped<ITicketReportRepository, TicketReportRepository>();

        // 6. LAB 13 – Servicio de generación Excel con ClosedXML
        services.AddScoped<IExcelReportService, ExcelReportService>();

        // 7. LAB 13 – Casos de Uso CQRS de Reportes
        services.AddScoped<GetUsersReportQuery>();
        services.AddScoped<GetTicketsReportQuery>();

        return services;
    }
}
