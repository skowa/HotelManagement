using Guestline.HotelManagement.Application.Services.Models;

namespace Guestline.HotelManagement.Application.Services;

public interface IHotelsService
{
    Task<AvailabilityResult> GetAvailabilityAsync(AvailabilityRequest availabilityRequest);
    Task<SearchResult> SearchRoomsAvailabilitiesAsync(SearchRequest searchRequest);
}