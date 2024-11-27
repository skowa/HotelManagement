namespace Guestline.HotelManagement.Application.Services.Models;

public record AvailabilityRequest(
    string HotelId,
    string RoomType,
    DateTime DateFrom,
    DateTime DateTo);
