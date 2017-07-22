using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls.Maps;

namespace UWPBank
{
    public class MapStyleBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string param = parameter as string;
            if (param == null)
                return DependencyProperty.UnsetValue;
            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object paramValue = Enum.Parse(value.GetType(), param);
            return paramValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string param = parameter as string;
            if (parameter == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(typeof(MapStyle), param);
        }
    }
}
