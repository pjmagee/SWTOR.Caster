using System.Linq;

namespace SwtorCaster.Core
{
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

        public static string GetFamilyName(this FontFamily fontFamily)
        {
            return fontFamily.FamilyNames.Values == null ? 
                        fontFamily.Source.Split('#').Last() : 
                        fontFamily.FamilyNames.Values.First();
        }

        public static string GetFamilyName(this GlyphTypeface fontFamily)
        {
            return fontFamily.FamilyNames.Values.Any() ?
                        fontFamily.FamilyNames.Values.First() :
                        "";
        }
    }
}
