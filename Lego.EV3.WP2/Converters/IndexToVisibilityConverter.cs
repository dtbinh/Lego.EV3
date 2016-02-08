using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lego.EV3.WP2.Converters
{
    class IndexToVisibilityConverter:ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            var boolValue = false;

            if (parameter != null)
            {
                boolValue = value.ToString() == parameter.ToString();
            }
            else
            {
                boolValue = System.Convert.ToInt32(value) == 0;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}
