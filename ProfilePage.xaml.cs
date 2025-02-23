using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace BookingWPF
{
    public partial class ProfilePage : Page
    {
        private User _currentUser;

        public ProfilePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Загружаем актуальные данные пользователя из БД
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", _currentUser.UserID)
                };

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT * FROM Users WHERE UserID = @UserID", parameters))
                {
                    if (reader.Read())
                    {
                        // Обновляем данные пользователя
                        UserNameText.Text = reader.GetString(reader.GetOrdinal("Name"));
                        UserEmailText.Text = reader.GetString(reader.GetOrdinal("Email"));
                        BonusPointsText.Text = reader.GetInt32(reader.GetOrdinal("BonusPoints")).ToString();

                        // Загружаем статистику бронирований
                        LoadBookingsStatistics();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBookingsStatistics()
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", _currentUser.UserID)
                };

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT COUNT(*) FROM Bookings WHERE UserID = @UserID", parameters))
                {
                    if (reader.Read())
                    {
                        BookingsCountText.Text = reader.GetInt32(0).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditProfilePage(_currentUser));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}
