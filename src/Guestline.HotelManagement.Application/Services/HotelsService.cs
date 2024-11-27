using Guestline.HotelManagement.Application.Providers;
using Guestline.HotelManagement.Application.Services.Models;
using Guestline.HotelManagement.Domain.Models;

namespace Guestline.HotelManagement.Application.Services;

// On a real project CQRS or use cases approach may be considered.
// So for each command there will be separate handler class, like AvailabilityQueryHandler.
public class HotelsService : IHotelsService
{
    private readonly IHotelsProvider _hotelsProvider;
    private readonly IBookingsProvider _bookingsProvider;

    public HotelsService(IHotelsProvider hotelsProvider, IBookingsProvider bookingsProvider)
    {
        _hotelsProvider = hotelsProvider;
        _bookingsProvider = bookingsProvider;
    }

    public async Task<AvailabilityResult> GetAvailabilityAsync(AvailabilityRequest availabilityRequest)
    {
        ArgumentNullException.ThrowIfNull(availabilityRequest);
        ArgumentException.ThrowIfNullOrEmpty(availabilityRequest.HotelId);
        ArgumentException.ThrowIfNullOrEmpty(availabilityRequest.RoomType);
        if (availabilityRequest.DateFrom > availabilityRequest.DateTo)
        {
            throw new ArgumentException("DateTo can't be less than DateFrom.", nameof(availabilityRequest));
        }

        var roomsAmount = await GetRoomsAmountAsync(availabilityRequest.HotelId, availabilityRequest.RoomType);
        if (roomsAmount == 0)
        {
            return new AvailabilityResult(roomsAmount);
        }

        var bookings = await _bookingsProvider.GetBookingsAsync(
                        availabilityRequest.HotelId,
                        availabilityRequest.RoomType,
                        availabilityRequest.DateFrom,
                        availabilityRequest.DateTo);

        if (availabilityRequest.DateFrom == availabilityRequest.DateTo)
        {
            return new AvailabilityResult(roomsAmount - bookings.Count);
        }

        var rangeDates = GetDateRanges(availabilityRequest.DateFrom, availabilityRequest.DateTo, bookings);
        var roomAvailabilities = GetRoomAvailabilities(rangeDates, bookings, roomsAmount);

        var availableRoomsAmount = roomAvailabilities.Min(availability => availability.RoomsAmount);
        return new AvailabilityResult(availableRoomsAmount);
    }

    public async Task<SearchResult> SearchRoomsAvailabilitiesAsync(SearchRequest searchRequest)
    {
        ArgumentNullException.ThrowIfNull(searchRequest);
        ArgumentException.ThrowIfNullOrEmpty(searchRequest.HotelId);
        ArgumentException.ThrowIfNullOrEmpty(searchRequest.RoomType);
        if (searchRequest.DaysAmount <= 0)
        {
            throw new ArgumentException("DaysAmount should be greater than 0", nameof(searchRequest));
        }

        var currentDate = DateTime.UtcNow.Date;
        var lastDate = currentDate.AddDays(searchRequest.DaysAmount);

        var roomsAmount = await GetRoomsAmountAsync(searchRequest.HotelId, searchRequest.RoomType);
        var bookings = await _bookingsProvider.GetBookingsAsync(
                        searchRequest.HotelId,
                        searchRequest.RoomType,
                        currentDate,
                        lastDate);

        var rangeDates = GetDateRanges(currentDate, lastDate, bookings);
        var availabilities = GetRoomAvailabilities(rangeDates, bookings, roomsAmount);

        return new SearchResult(availabilities
            .Select(availability => new AvailabilityRange(
                availability.DateFrom,
                availability.DateTo,
                availability.RoomsAmount))
            .ToArray());
    }

    private async Task<int> GetRoomsAmountAsync(string hotelId, string roomType)
    {
        var hotel = await _hotelsProvider.GetHotelAsync(hotelId)
           ?? throw new ArgumentException("HotelId is invalid.", nameof(hotelId));

        return hotel.Rooms.Count(room => room.RoomType == roomType);
    }

    private static DateTime[] GetDateRanges(DateTime dateFrom, DateTime dateTo, IReadOnlyCollection<Booking> bookings)
    {
        return new DateTime[] { dateFrom, dateTo }
                        .Concat(bookings.Select(b => b.Departure))
                        .Concat(bookings.Select(b => b.Arrival))
                        .Where(date => date >= dateFrom && date <= dateTo)
                        .Distinct()
                        .Order()
                        .ToArray();
    }

    private static List<RoomsAvailability> GetRoomAvailabilities(
        DateTime[] rangeDates,
        IReadOnlyCollection<Booking> bookings,
        int totalRoomsAmount)
    {
        // That's readable, but less efficient approach.
        // The more efficient algorithm would be to store starts and ends in date ranges.
        // Something like [(2.10.2024, Start), (2.10.2024, Start), (2.10.2024, End),..).
        // And then counting rooms amount each date one by one. If the date has Start then we
        // should subtract 1 from rooms amount. If the date has End then we should add 1.
        // And due to these calculations each range's rooms amount will be calculated.
        // But that will be definitely less readable and understandable.
        var availabilities = new List<RoomsAvailability>();

        for (var i = 0; i < rangeDates.Length - 1; i++)
        {
            var unavailableRoomsAmount = bookings.Count(booking =>
                    booking.Arrival <= rangeDates[i] &&
                    booking.Departure >= rangeDates[i + 1]);

            var availableRoomsAmount = totalRoomsAmount - unavailableRoomsAmount;
            if (availabilities.Count > 0)
            {
                var lastAvailability = availabilities[^1];
                if (lastAvailability.RoomsAmount == availableRoomsAmount)
                {
                    lastAvailability.DateTo = rangeDates[i + 1];
                    continue;
                }
            }

            availabilities.Add(new RoomsAvailability
            {
                DateFrom = rangeDates[i],
                DateTo = rangeDates[i + 1],
                RoomsAmount = availableRoomsAmount
            });
        }

        return availabilities;
    }

    private class RoomsAvailability
    {
        public DateTime DateFrom { get; init; }

        public DateTime DateTo { get; set; }

        public int RoomsAmount { get; init; }
    }
}
