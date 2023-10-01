namespace barber_shop.Extensions
{
    public static class StringExtension
    {
        public static string FormatCpf(this string value)
        {
            var digits = value.Where(c => char.IsDigit(c)).ToArray();
            return new string(digits);
        }
    }
}
