using System;
using System.Globalization;
using System.Windows.Data;

namespace CommandRecognition.UI.Converts
{
    class LoginConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var passwords = values.Clone();
            var password = (passwords as object[])[0];

            return password;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
