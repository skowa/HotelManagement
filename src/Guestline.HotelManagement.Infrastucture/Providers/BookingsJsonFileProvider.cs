using Guestline.HotelManagement.Application.Providers;
using Guestline.HotelManagement.Domain.Models;
using Guestline.HotelManagement.Infrastucture.Settings;
using Guestline.HotelManagement.Infrastucture.Utils;
using Microsoft.Extensions.Options;

namespace Guestline.HotelManagement.Infrastucture.Providers;

public class BookingsJsonFileProvider : IBookingsProvider
{
    private readonly BookingsFileSettings _fileSettings;

    public BookingsJsonFileProvider(IOptions<BookingsFileSettings> options)
    {
        _fileSettings = options.Value;
    }

    public async Task<IReadOnlyCollection<Booking>> GetBookingsAsync(
        string hotelId,
        string roomType,
        DateTime fromDate,
        DateTime toDate)
    {
        var bookings = await JsonFileUtils.GetFromFileAsync<Booking[]>(_fileSettings.Path);

        return bookings.Where(booking =>
                    booking.HotelId == hotelId &&
                    booking.RoomType == roomType &&
                    booking.Arrival <= toDate &&
                    booking.Departure > fromDate)
                .ToArray();
    }
}
