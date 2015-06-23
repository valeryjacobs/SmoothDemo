using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SmoothDemo
{
    public class ListValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Newtonsoft.Json.Linq.JArray)
            {
                string result = "";
                foreach (string s in (Newtonsoft.Json.Linq.JArray)value)
                {
                    result += s;
                }
                return result;
            }
            else if (value is string)
            {
                return value;
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
