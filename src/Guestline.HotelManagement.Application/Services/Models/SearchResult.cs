namespace Guestline.HotelManagement.Application.Services.Models;

public record SearchResult(IReadOnlyCollection<AvailabilityRange> RoomsAvailability);

public record AvailabilityRange(
    DateTime DateFrom,
    DateTime DateTo,
    int RoomsAmount);
