using System.Windows.Controls;
using System.Windows.Input;

namespace BookingWPF
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Пока просто перенаправляем на страницу входа
            NavigationService.Navigate(new LoginPage());
        }

        private void LoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}