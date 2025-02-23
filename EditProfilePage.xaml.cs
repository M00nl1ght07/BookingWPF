using System.Windows;
using System.Windows.Controls;

namespace BookingWPF
{
    public partial class EditProfilePage : Page
    {
        public EditProfilePage()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика сохранения изменений
            NavigationService.Navigate(new ProfilePage());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }
    }
}