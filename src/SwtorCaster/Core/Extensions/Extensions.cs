namespace SwtorCaster.Core.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    public static class Extensions
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

        public static FontFamily FromStringToFont(this string value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(FontFamily));
                return (FontFamily)converter.ConvertFromString(value);
            }
            catch
            {
                return new FontFamily(new Uri("pack://application:,,,/"), "./resources/#SF Distant Galaxy");
            }
        }

        public static string FromFontToString(this FontFamily family)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(FontFamily));
            return converter.ConvertToString(family);
        }
    }
}
