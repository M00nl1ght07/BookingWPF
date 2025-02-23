using System.Windows;
using System.Windows.Controls;



namespace BookingWPF

{

    /// <summary>

    /// Логика взаимодействия для HotelsPage.xaml

    /// </summary>

    public partial class HotelsPage : Page

    {

        public HotelsPage()

        {

            InitializeComponent();

        }



        private void ViewRooms_Click(object sender, RoutedEventArgs e)

        {

            NavigationService.Navigate(new RoomSelectionPage());

        }

    }

}


