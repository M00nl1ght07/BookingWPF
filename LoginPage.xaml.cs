using System.Windows.Controls;
using System.Windows.Input;

namespace BookingWPF
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Пока просто перенаправляем в личный кабинет
            NavigationService.Navigate(new ProfilePage());
        }

        private void RegisterLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminPanelPage()); // Временно для демонстрации
        }
    }
}