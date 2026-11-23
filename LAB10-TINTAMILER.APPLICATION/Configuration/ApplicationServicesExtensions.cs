using LAB10_TINTAMILER.APPLICATION.UseCases.Auth;
using LAB10_TINTAMILER.APPLICATION.UseCases.Responses;
using LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;
using Microsoft.Extensions.DependencyInjection;

namespace LAB10_TINTAMILER.APPLICATION.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationUseCases(this IServiceCollection services)
    {
        // Casos de uso de Autenticación
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterUseCase, RegisterUseCase>();

        // Casos de uso de Tickets
        services.AddScoped<IGetAllTicketsUseCase, GetAllTicketsUseCase>();
        services.AddScoped<IGetTicketByIdUseCase, GetTicketByIdUseCase>();
        services.AddScoped<IGetTicketsByUserUseCase, GetTicketsByUserUseCase>();
        services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
        services.AddScoped<IUpdateTicketStatusUseCase, UpdateTicketStatusUseCase>();

        // Casos de uso de Respuestas a Tickets
        services.AddScoped<IAddResponseUseCase, AddResponseUseCase>();

        // Configuración de MediatR para escaneo de Commands, Queries y Handlers en Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationAssemblyMarker).Assembly
        ));

        return services;
    }
}
