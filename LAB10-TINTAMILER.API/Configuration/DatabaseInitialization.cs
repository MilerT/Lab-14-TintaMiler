using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.API.Configuration;

internal static class DatabaseInitialization
{
    public static async Task InitializeTicketeraAsync(this WebApplication app)
    {
        if (!app.Configuration.GetValue<bool>("Ticketera:InitializeSchema"))
            return;

        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TicketeraDbContext>();
        var schema = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "TicketeraBD.sql"));

        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.Database.ExecuteSqlRawAsync("SELECT pg_advisory_xact_lock(hashtext('ticketera-lab14-schema'))");
        await context.Database.ExecuteSqlRawAsync(schema);
        await transaction.CommitAsync();
    }
}
