using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BookingWPF.Services;
using BookingWPF.Pages;

namespace BookingWPF.Pages
{
    public partial class HotelDetailsPage : Page
    {
        public async Task UpdatePrices(string currency)
        {
            if (PriceTextBlock != null)
            {
                decimal originalPrice = decimal.Parse(PriceTextBlock.Text.Split(' ')[1]);
                decimal convertedPrice = await App.CurrencyService.ConvertPrice(originalPrice, currency);
                string currencySymbol = currency switch
                {
                    "USD" => "$",
                    "EUR" => "€",
                    "GBP" => "£",
                    _ => "₽"
                };
                PriceTextBlock.Text = $"От {convertedPrice} {currencySymbol} за ночь";
            }
        }
    }
} 