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

            // Проверка подключения к БД
            if (DatabaseConnection.TestConnection())
            {
                MessageBox.Show("Подключение к базе данных успешно установлено!");
            }
            else
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
            }

            // Загружаем главную страницу
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
