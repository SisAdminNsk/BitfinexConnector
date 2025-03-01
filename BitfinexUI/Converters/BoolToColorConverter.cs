using Avalonia.Data.Converters;
using System.Globalization;
using System;
using System.Drawing;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace BitfinexUI.Converters
{
    public class BoolToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSubscribed)
            {
                return isSubscribed
                    ? new SolidColorBrush(Avalonia.Media.Color.Parse("#eea236")) 
                    : new SolidColorBrush(Avalonia.Media.Color.Parse("#62cfb5")); 
            }

            return new SolidColorBrush(Avalonia.Media.Color.Parse("#eea236")); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
