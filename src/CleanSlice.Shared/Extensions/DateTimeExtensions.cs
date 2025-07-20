namespace CleanSlice.Shared.Extensions;

public static class DateTimeExtensions
{
    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    public static bool IsWeekday(this DateTime date)
        => !date.IsWeekend();

    public static DateTime StartOfDay(this DateTime date)
        => new(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);

    public static DateTime EndOfDay(this DateTime date)
        => new(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);

    public static DateTime StartOfMonth(this DateTime date)
        => new(date.Year, date.Month, 1, 0, 0, 0, date.Kind);

    public static DateTime EndOfMonth(this DateTime date)
        => date.StartOfMonth().AddMonths(1).AddTicks(-1);

    public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        => date >= start && date <= end;

    public static string ToFriendlyString(this DateTime date)
    {
        var now = DateTime.UtcNow;
        var diff = now - date;

        return diff.TotalDays switch
        {
            < 1 when diff.TotalHours < 1 => $"{(int)diff.TotalMinutes} minutes ago",
            < 1 => $"{(int)diff.TotalHours} hours ago",
            < 7 => $"{(int)diff.TotalDays} days ago",
            < 30 => $"{(int)(diff.TotalDays / 7)} weeks ago",
            < 365 => $"{(int)(diff.TotalDays / 30)} months ago",
            _ => $"{(int)(diff.TotalDays / 365)} years ago"
        };
    }
}
