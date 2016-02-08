using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lego.EV3.WP2.Converters
{
    class DoubleRoundedConverter:ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            int roundValue = 2;

            string returnValue = "";

            if (parameter != null)
            {
                int.TryParse(parameter.ToString(), out roundValue);
            }

            if (value != null)
            {
                double parseValue;
                double.TryParse(value.ToString(), out parseValue);

                returnValue = Math.Round(parseValue, roundValue).ToString(CultureInfo.InvariantCulture);
            }

            return returnValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}
