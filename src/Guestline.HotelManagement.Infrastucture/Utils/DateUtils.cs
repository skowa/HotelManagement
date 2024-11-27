using System.Globalization;

namespace Guestline.HotelManagement.Infrastucture.Utils;

public static class DateUtils
{
    private const string DateFormat = "yyyyMMdd";

    public static DateTime ParseWrappedFormat(string wrappedDate) =>
        DateTime.ParseExact(wrappedDate, DateFormat, CultureInfo.InvariantCulture);

    public static string ToWrappedFormat(DateTime dateTime) => dateTime.ToString(DateFormat);
}
