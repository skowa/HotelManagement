using Guestline.HotelManagement.Application.Providers;
using Guestline.HotelManagement.Infrastucture.Providers;
using Guestline.HotelManagement.Infrastucture.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Guestline.HotelManagement.Infrastucture.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        Action<HotelsFileSettings> configureHotels,
        Action<BookingsFileSettings> configureBookings)
    {
        services.Configure(configureHotels);
        services.Configure(configureBookings);

        services.AddScoped<IHotelsProvider, HotelsJsonFileProvider>();
        services.AddScoped<IBookingsProvider, BookingsJsonFileProvider>();

        return services;
    }
}
