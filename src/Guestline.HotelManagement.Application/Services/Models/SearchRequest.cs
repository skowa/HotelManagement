namespace Guestline.HotelManagement.Application.Services.Models;

public record SearchRequest(
    string HotelId,
    string RoomType,
    int DaysAmount);
