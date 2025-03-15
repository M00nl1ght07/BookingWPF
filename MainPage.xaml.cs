using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HotelApp.Pages
{
    public partial class MainPage : Page
    {
        public async Task UpdatePrices(string currency)
        {
            foreach (var hotel in PopularHotelsPanel.Children.OfType<HotelCard>())
            {
                decimal originalPrice = hotel.OriginalPrice; // Храним оригинальную цену в рублях
                decimal convertedPrice = await App.CurrencyService.ConvertPrice(originalPrice, currency);
                hotel.UpdatePrice(convertedPrice, currency);
            }
        }
    }
} 