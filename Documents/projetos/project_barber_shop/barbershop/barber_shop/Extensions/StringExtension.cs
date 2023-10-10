namespace barber_shop.Extensions
{
    public static class StringExtension
    {
        public static string RemoveFormatCpf(this string value)
        {
            var digits = value.Where(c => char.IsDigit(c)).ToArray();
            return new string(digits);
        }

        public static string FormatCpf(this string value)
        {
            var cpf = $"{value.Substring(0, 3)}.{value.Substring(3, 3)}.{value.Substring(6,3)}-{value.Substring(9,2)}";
            return cpf;
        }
    }
}
