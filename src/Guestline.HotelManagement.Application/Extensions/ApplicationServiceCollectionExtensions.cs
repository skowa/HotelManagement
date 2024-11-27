using Guestline.HotelManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Guestline.HotelManagement.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IHotelsService, HotelsService>();

        return services;
    }
}
