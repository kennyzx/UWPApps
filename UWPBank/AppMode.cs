using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPBank
{
    public enum AppMode 
    {
        Feature,
        Story
    }

    public class AppModeBoolConverter: IValueConverter
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

            return Enum.Parse(typeof(AppMode), param);
        }
    }
}
