using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace AdminClientApp.Views.Converters
{
    internal class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(false);
            if (attributes.Length == 0)
            {
                return value.ToString();
            }
            else
            {
                var attrib = attributes[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
