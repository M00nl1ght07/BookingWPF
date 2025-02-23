using System.Windows;

using System.Windows.Controls;



namespace BookingWPF

{

    public partial class ProfilePage : Page

    {

        public ProfilePage()

        {

            InitializeComponent();

        }



        private void EditProfile_Click(object sender, RoutedEventArgs e)

        {

            NavigationService.Navigate(new EditProfilePage());

        }



        private void Logout_Click(object sender, RoutedEventArgs e)

        {

            NavigationService.Navigate(new HomePage());

        }

    }

}
