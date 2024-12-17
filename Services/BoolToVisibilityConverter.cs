using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace FlashcardsMVP.Services
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        // Convert bool to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        // Convert back is not needed, but it must be implemented.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false; // We don't need this functionality in this case
        }
    }
}