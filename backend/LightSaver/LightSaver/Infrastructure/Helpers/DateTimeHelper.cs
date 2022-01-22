namespace LightSaver.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public const string PriceDateTimeFormat = "dd/MM/yyyy";

        public static string ToPriceDate(DateTime date)
        {
            return date.ToString(PriceDateTimeFormat);
        }
    }
}
