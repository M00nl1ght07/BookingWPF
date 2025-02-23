using System;
using System.Windows;
using System.Windows.Controls;

namespace BookingWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!DatabaseConnection.TestConnection())
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            MainFrame.Navigate(new HomePage());
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
            MainFrame.Navigate(new BookingsPage());
        }

        private void MenuProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoginPage());
        }
    }
}
