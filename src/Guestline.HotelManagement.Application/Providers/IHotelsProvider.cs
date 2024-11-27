using Guestline.HotelManagement.Domain.Models;

namespace Guestline.HotelManagement.Application.Providers;

public interface IHotelsProvider
{
    Task<Hotel> GetHotelAsync(string hotelId);
}
