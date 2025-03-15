using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using BookingWPF.Services;

namespace BookingWPF
{
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; private set; }

        private readonly CurrencyService _currencyService = new CurrencyService();
        private string _currentCurrency = "RUB";

        public MainWindow()
        {
            InitializeComponent();
            
            // Устанавливаем сохраненную валюту в ComboBox
            foreach (ComboBoxItem item in CurrencyComboBox.Items)
            {
                if (item.Content.ToString() == App.CurrentCurrency)
                {
                    CurrencyComboBox.SelectedItem = item;
                    break;
                }
            }

            if (!DatabaseConnection.TestConnection())
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            MainFrame.Navigate(new HomePage());
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        private void MenuHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void MenuHotels_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HotelsPage());
        }

        private void MenuBookings_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("Для просмотра бронирований необходимо войти в систему",
                    "Требуется авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                MainFrame.Navigate(new LoginPage());
                return;
            }

            MainFrame.Navigate(new BookingsPage(CurrentUser));
        }

        private void MenuProfile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MainFrame.Navigate(new LoginPage());
            }
            else
            {
                MainFrame.Navigate(new ProfilePage(CurrentUser));
            }
        }

        public void Logout()
        {
            CurrentUser = null;
            MainFrame.Navigate(new HomePage());
        }

        private async void CurrencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrencyComboBox.SelectedItem is ComboBoxItem selectedItem && MainFrame?.Content != null)
            {
                App.CurrentCurrency = selectedItem.Content.ToString(); // Сохраняем выбранную валюту
                await UpdatePrices();
            }
        }

        private async Task UpdatePrices()
        {
            if (MainFrame.Content is HomePage homePage)
            {
                await homePage.UpdatePrices(App.CurrentCurrency);
            }
            else if (MainFrame.Content is HotelsPage hotelsPage)
            {
                await hotelsPage.UpdatePrices(App.CurrentCurrency);
            }
            else if (MainFrame.Content is RoomSelectionPage roomPage)
            {
                await roomPage.UpdatePrices(App.CurrentCurrency);
            }
            else if (MainFrame.Content is BookingsPage bookingsPage)
            {
                await bookingsPage.UpdatePrices(App.CurrentCurrency);
            }
        }
    }
}
