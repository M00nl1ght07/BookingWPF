using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace BookingWPF
{
    public partial class AdminPanelPage : Page
    {
        private User _currentAdmin;

        public AdminPanelPage(User admin)
        {
            InitializeComponent();
            _currentAdmin = admin;
            AdminEmailText.Text = admin.Email;
            LoadData();
        }

        private void LoadData()
        {
            LoadUsers();
            LoadHotels();
            LoadBookings();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT UserID, Name, Email, RegistrationDate, BonusPoints FROM Users WHERE IsAdmin = 0"))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    UsersDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadHotels()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT h.HotelID, h.Name, h.City, h.Rating, 
                           h.IsPopular, COUNT(r.RoomID) as RoomCount 
                    FROM Hotels h
                    LEFT JOIN Rooms r ON h.HotelID = r.HotelID
                    GROUP BY h.HotelID, h.Name, h.City, h.Rating, h.IsPopular"))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    HotelsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отелей: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBookings()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT b.BookingID, h.Name as HotelName, 
                           r.RoomType, u.Name as GuestName,
                           b.CheckInDate, b.CheckOutDate,
                           b.TotalPrice, b.Status
                    FROM Bookings b
                    JOIN Rooms r ON b.RoomID = r.RoomID
                    JOIN Hotels h ON r.HotelID = h.HotelID
                    JOIN Users u ON b.UserID = u.UserID
                    ORDER BY b.BookingDate DESC"))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    BookingsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке бронирований: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}