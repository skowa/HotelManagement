namespace Guestline.HotelManagement.Console.Handlers;

internal interface IConsoleCommandHandler
{
    bool IsMatch(string input);

    Task<string?> HandleCommandAsync(string input);
}
