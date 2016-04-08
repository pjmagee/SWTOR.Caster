namespace SwtorCaster.Core.Extensions
{
    using System.Windows.Media;

    public static class ColorExtensions
    {
        public static Color FromHexToColor(this string value)
        {
            try
            {
                var color = ColorConverter.ConvertFromString(value);
                if (color == null) return Colors.Transparent;
                return (Color) color;
            }
            catch
            {
                return Colors.Transparent;
            }
        }

        public static string ToHex(this Color color)
        {
            return color.ToString();
        }
    }
}
