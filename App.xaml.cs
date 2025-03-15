using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookingWPF.Services;

namespace BookingWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CurrencyService CurrencyService { get; } = new CurrencyService();
        public static string CurrentCurrency { get; set; } = "RUB";
    }
}
