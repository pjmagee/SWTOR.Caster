namespace SwtorCaster.Custom
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using SwtorCaster.Core;

    public class FontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontFamily = (FontFamily) value;
            return fontFamily.GetFamilyName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new FontFamily((string) value);
        }
    }
}