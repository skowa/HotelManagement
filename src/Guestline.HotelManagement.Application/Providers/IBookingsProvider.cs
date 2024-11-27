using Guestline.HotelManagement.Domain.Models;

namespace Guestline.HotelManagement.Application.Providers;

public interface IBookingsProvider
{
    Task<IReadOnlyCollection<Booking>> GetBookingsAsync(
        string hotelId,
        string roomType,
        DateTime fromDate,
        DateTime toDate);
}
