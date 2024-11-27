namespace Guestline.HotelManagement.Domain.Models;

public record Hotel(string Id,
    string Name,
    IReadOnlyCollection<RoomType> RoomTypes,
    IReadOnlyCollection<Room> Rooms);

public record RoomType(string Code,
    string Description,
    IReadOnlyCollection<string> Amenities,
    IReadOnlyCollection<string> Features);

public record Room(string RoomType, string RoomId);
