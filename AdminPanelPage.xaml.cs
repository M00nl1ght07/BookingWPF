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
        private AdminPanelPage _adminPanel;

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
                           h.BasePrice, h.IsPopular
                    FROM Hotels h"))
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

        private void AddHotel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddHotelPage(this));
        }

        private void AddToPopular_Click(object sender, RoutedEventArgs e)
        {
            if (HotelsDataGrid.SelectedItem != null)
            {
                try
                {
                    DataRowView row = (DataRowView)HotelsDataGrid.SelectedItem;
                    int hotelId = Convert.ToInt32(row["HotelID"]);
                    bool isCurrentlyPopular = Convert.ToBoolean(row["IsPopular"]);

                    // Меняем статус на противоположный
                    bool newStatus = !isCurrentlyPopular;

                    SqlParameter[] parameters = {
                        new SqlParameter("@HotelID", hotelId),
                        new SqlParameter("@IsPopular", newStatus)
                    };

                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE Hotels SET IsPopular = @IsPopular WHERE HotelID = @HotelID", 
                        parameters);

                    LoadHotels(); // Перезагружаем список отелей

                    string message = newStatus ? 
                        "Отель добавлен в популярные" : 
                        "Отель удален из популярных";
                    
                    MessageBox.Show(message, "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении статуса отеля: {ex.Message}", 
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите отель", "Информация", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            if (HotelsDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)HotelsDataGrid.SelectedItem;
                int hotelId = Convert.ToInt32(row["HotelID"]);
                NavigationService.Navigate(new AddRoomPage(hotelId));
            }
            else
            {
                MessageBox.Show("Выберите отель для добавления номера", "Информация", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void RefreshData()
        {
            LoadHotels();
        }
    }
}