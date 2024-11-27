namespace Guestline.HotelManagement.Domain.Models;

public record Booking(string HotelId,
    DateTime Arrival,
    DateTime Departure,
    string RoomType,
    string RoomRate);
