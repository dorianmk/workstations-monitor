using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace AdminClientApp.Views.Converters
{
    internal class CollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object> enumerable)            
                return string.Join("; ", enumerable);            
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
