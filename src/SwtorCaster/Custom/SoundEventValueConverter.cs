namespace SwtorCaster.Custom
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Core.Domain.Log;

    public class SoundEventValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (SoundEvent)value;
            return Enum.GetName(typeof(SoundEvent), enumValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SoundEvent)value;
        }
    }
}