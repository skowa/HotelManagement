using System.Text.RegularExpressions;

namespace Guestline.HotelManagement.Console.Utils;

internal static class RegexUtils
{
    internal static string GetParameterFromMatch(Match group, int index) =>
        group.Groups[index].Value.Trim();
}
