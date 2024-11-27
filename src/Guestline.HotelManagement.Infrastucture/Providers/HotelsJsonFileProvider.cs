using Guestline.HotelManagement.Application.Providers;
using Guestline.HotelManagement.Domain.Models;
using Guestline.HotelManagement.Infrastucture.Settings;
using Guestline.HotelManagement.Infrastucture.Utils;
using Microsoft.Extensions.Options;

namespace Guestline.HotelManagement.Infrastucture.Providers;

public class HotelsJsonFileProvider : IHotelsProvider
{
    private readonly HotelsFileSettings _fileSettings;

    public HotelsJsonFileProvider(IOptions<HotelsFileSettings> options)
    {
        _fileSettings = options.Value;
    }

    public async Task<Hotel> GetHotelAsync(string hotelId)
    {
        var hotels = await JsonFileUtils.GetFromFileAsync<Hotel[]>(_fileSettings.Path);

        return hotels.SingleOrDefault(hotel => hotel.Id == hotelId);
    }
}
