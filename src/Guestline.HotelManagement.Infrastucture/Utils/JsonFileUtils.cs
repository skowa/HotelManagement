using Guestline.HotelManagement.Infrastucture.Utils.Converters;
using System.Text.Json;

namespace Guestline.HotelManagement.Infrastucture.Utils;

internal static class JsonFileUtils
{
    private static readonly JsonSerializerOptions _serializerOptions;

   static JsonFileUtils()
   {
        _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        _serializerOptions.Converters.Add(new DateJsonConverter());
    }

    internal async static Task<T> GetFromFileAsync<T>(string filePath)
    {
        // If there's a memory requirement, the file can be read in batches,
        // each json array object one by one comparing to needed id.
        // So not to overwhelm the process memory.
        using var fileStream = File.OpenRead(filePath);
        var item = await JsonSerializer.DeserializeAsync<T>(fileStream, _serializerOptions);

        return item;
    }
}
