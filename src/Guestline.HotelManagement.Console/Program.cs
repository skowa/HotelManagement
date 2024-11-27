using Guestline.HotelManagement.Application.Extensions;
using Guestline.HotelManagement.Console.Extensions;
using Guestline.HotelManagement.Console.Handlers;
using Guestline.HotelManagement.Infrastucture.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guestline.HotelManagement.Console;

internal class Program
{

    public async static Task Main(string[] args)
    {
        var configuration = BuildConfiguration(args);
        if (configuration == null)
        {
            return;
        }

        var serviceProvider = BuildServiceProvider(configuration);

        System.Console.WriteLine("Please write your commands.");
        while (true)
        {
            try
            {
                var line = System.Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                await ProcessLineAsync(line, serviceProvider);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }

    private static async Task ProcessLineAsync(string line, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var consoleCommandHandlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IConsoleCommandHandler>>();
        string? output = null;
        foreach (var consoleCommandHandler in consoleCommandHandlers)
        {
            if (consoleCommandHandler.IsMatch(line))
            {
                output = await consoleCommandHandler.HandleCommandAsync(line);

                break;
            }
        }

        System.Console.WriteLine(output ?? "Invalid command");
    }

    private static IConfiguration? BuildConfiguration(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        if (configuration["hotels"] is null)
        {
            System.Console.WriteLine("hotels argument was not specified", nameof(args));

            return null;
        }

        if (configuration["bookings"] is null)
        {
            System.Console.WriteLine("bookings argument was not specified", nameof(args));

            return null;
        }

        return configuration;
    }

    private static IServiceProvider BuildServiceProvider(IConfiguration configuration)
    {
        return new ServiceCollection()
            .AddInfrastructureServices(fileSettings =>
            {
                fileSettings.Path = configuration["hotels"];
            },
            fileSettings =>
            {
                fileSettings.Path = configuration["bookings"];
            })
            .AddApplicationServices()
            .AddConsoleServices()
            .BuildServiceProvider();
    }
}
