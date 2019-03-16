
namespace MonopolyTracker.Shared.Data
{
    using System.Text;

    public static class TextHandler
    {
        public static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return default;
            }

            var ascii = new ASCIIEncoding();

            var text = input.Trim().ToUpperInvariant().SubstituteChars();

            var encodedBytes = ascii.GetBytes(text);

            return ascii.GetString(encodedBytes);
        }

        public static string SubstituteChars(this string @this) =>
            @this
                .Replace(" ", string.Empty)
                .Replace(']', 'J');
    }
}
