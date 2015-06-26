using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace SmoothDemo.UniversalClient.Converters
{
 public class BooleanToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((bool)value)
            {
                return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);
            }

            return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.DarkGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
