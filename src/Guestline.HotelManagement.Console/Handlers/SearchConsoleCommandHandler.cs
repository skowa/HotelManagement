using Guestline.HotelManagement.Application.Services.Models;
using Guestline.HotelManagement.Application.Services;
using System.Text.RegularExpressions;
using Guestline.HotelManagement.Console.Utils;
using Guestline.HotelManagement.Infrastucture.Utils;

namespace Guestline.HotelManagement.Console.Handlers;

internal partial class SearchConsoleCommandHandler : IConsoleCommandHandler
{
    private const string SearchCommand = @"Search\(([^,]+),(\d+),([^)]+)\)";
    private readonly IHotelsService _hotelsService;

    [GeneratedRegex(SearchCommand)]
    private static partial Regex SearchRegex();

    public SearchConsoleCommandHandler(IHotelsService hotelsService)
    {
        _hotelsService = hotelsService;
    }

    public bool IsMatch(string input) => SearchRegex().IsMatch(input);

    public async Task<string?> HandleCommandAsync(string input)
    {
        var searchMatch = SearchRegex().Match(input);
        if (searchMatch.Success)
        {
            var hotelId = RegexUtils.GetParameterFromMatch(searchMatch, 1);
            var daysAmount = int.Parse(RegexUtils.GetParameterFromMatch(searchMatch, 2));
            var roomType = RegexUtils.GetParameterFromMatch(searchMatch, 3);

            var searchRequest = new SearchRequest(hotelId, roomType, daysAmount);

            var result = await _hotelsService.SearchRoomsAvailabilitiesAsync(searchRequest);

            var output = string.Join(", ",
                result.RoomsAvailability.Select(availability =>
                $"({DateUtils.ToWrappedFormat(availability.DateFrom)}-{DateUtils.ToWrappedFormat(availability.DateTo)},{availability.RoomsAmount})"));

            return output;
        }

        return null;
    }
}