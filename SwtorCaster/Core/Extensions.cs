namespace SwtorCaster.Core
{
    using System.Windows.Media;

    public static class Extensions
    {
        public static Color ToColorFromRgb(this string value)
        {
            if (string.IsNullOrEmpty(value)) return Colors.Transparent;
            var rgb = value.Split(',');
            return Color.FromRgb(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]));
        }

        public static string ToRgbFromColor(this Color color, string seperator = ",")
        {
            if (color == Colors.Transparent) return string.Empty;
            return $"{color.R}{seperator}{color.G}{seperator}{color.B}";
        }
    }
}
