namespace Panadero.Extensions;

public static class DateTimeExtensions
{
    public static void Deconstruct(this DateTime date, out int year, out int month, out int day)
    {
        year = date.Year;
        month = date.Month;
        day = date.Day;
    }
}
