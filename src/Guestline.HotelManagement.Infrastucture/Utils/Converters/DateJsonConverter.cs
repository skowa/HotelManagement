using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guestline.HotelManagement.Infrastucture.Utils.Converters;

public class DateJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();

        return DateUtils.ParseWrappedFormat(dateString);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(DateUtils.ToWrappedFormat(value));
    }
}
