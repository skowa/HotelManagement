using Guestline.HotelManagement.Application.Services.Models;
using Guestline.HotelManagement.Application.Services;
using System.Text.RegularExpressions;
using Guestline.HotelManagement.Console.Handlers;
using Guestline.HotelManagement.Console.Utils;
using Guestline.HotelManagement.Infrastucture.Utils;

namespace Guestline.HotelManagement.Console;

internal partial class AvailabilityConsoleCommandHandler : IConsoleCommandHandler
{
    private const string AvailabilityCommand = @"Availability\(([^,]+),(\d+(-\d+)?),([^)]+)\)";

    [GeneratedRegex(AvailabilityCommand)]
    private static partial Regex AvailabilityRegex();

    private readonly IHotelsService _hotelsService;

    public AvailabilityConsoleCommandHandler(IHotelsService hotelsService)
    {
        _hotelsService = hotelsService;
    }

    public bool IsMatch(string input) => AvailabilityRegex().IsMatch(input);

    public async Task<string?> HandleCommandAsync(string input)
    {
        var availabilityMatch = AvailabilityRegex().Match(input);
        if (availabilityMatch.Success)
        {
            var dateRange = RegexUtils.GetParameterFromMatch(availabilityMatch, 2);
            var dates = dateRange.Split('-');

            var hotelId = RegexUtils.GetParameterFromMatch(availabilityMatch, 1);
            var dateFrom = DateUtils.ParseWrappedFormat(dates[0]);
            var dateTo = dates.Length > 1 ? DateUtils.ParseWrappedFormat(dates[1]) : dateFrom;
            var roomType = RegexUtils.GetParameterFromMatch(availabilityMatch, 4);

            var availabilityRequest =
                new AvailabilityRequest(hotelId, roomType, dateFrom, dateTo);

            var result = await _hotelsService.GetAvailabilityAsync(availabilityRequest);

            return result.RoomsCount.ToString();
        }

        return null;
    }
}
