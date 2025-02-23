using System.Windows;
using System.Windows.Controls;

namespace BookingWPF
{
    public partial class AdminPanelPage : Page
    {
        public AdminPanelPage()
        {
            InitializeComponent();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}