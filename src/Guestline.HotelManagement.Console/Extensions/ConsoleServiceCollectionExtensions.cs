using Guestline.HotelManagement.Console;
using Guestline.HotelManagement.Console.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Guestline.HotelManagement.Console.Extensions;

internal static class ConsoleServiceCollectionExtensions
{
    internal static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services.AddScoped<IConsoleCommandHandler, AvailabilityConsoleCommandHandler>();
        services.AddScoped<IConsoleCommandHandler, SearchConsoleCommandHandler>();

        return services;
    }
}
