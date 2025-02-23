using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingWPF
{
    public class PopularButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPopular = (bool)value;
            return isPopular ? "Убрать из популярных" : "Добавить в популярные";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
