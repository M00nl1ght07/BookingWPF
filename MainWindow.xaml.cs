using System;
using System.Windows;
using System.Windows.Controls;

namespace BookingWPF
{
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; private set; }

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
    }
}
