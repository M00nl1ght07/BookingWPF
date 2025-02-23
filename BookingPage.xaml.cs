using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace BookingWPF
{
    public partial class BookingPage : Page
    {
        private readonly int _roomId;
        private readonly User _currentUser;
        private readonly DateTime _checkIn;
        private readonly DateTime _checkOut;
        private decimal _pricePerNight;

        public BookingPage(int roomId, User currentUser, DateTime checkIn, DateTime checkOut)
        {
            InitializeComponent();
            _roomId = roomId;
            _currentUser = currentUser;
            _checkIn = checkIn;
            _checkOut = checkOut;

            LoadRoomInfo();
            LoadUserInfo();
            CalculatePrice();
        }

        private void LoadRoomInfo()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@RoomID", _roomId) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT r.RoomType, r.PricePerNight, h.Name as HotelName
                    FROM Rooms r
                    JOIN Hotels h ON r.HotelID = h.HotelID
                    WHERE r.RoomID = @RoomID", parameters))
                {
                    if (reader.Read())
                    {
                        HotelNameText.Text = reader["HotelName"].ToString();
                        RoomTypeText.Text = reader["RoomType"].ToString();
                        _pricePerNight = reader.GetDecimal(reader.GetOrdinal("PricePerNight"));
                        PricePerNightText.Text = _pricePerNight.ToString("C0");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации о номере: {ex.Message}");
            }
        }

        private void LoadUserInfo()
        {
            GuestNameText.Text = _currentUser.Name;
            GuestEmailText.Text = _currentUser.Email;
            CheckInText.Text = _checkIn.ToShortDateString();
            CheckOutText.Text = _checkOut.ToShortDateString();
        }

        private void CalculatePrice()
        {
            int nights = (_checkOut - _checkIn).Days;
            NightsCountText.Text = nights.ToString();
            decimal totalPrice = nights * _pricePerNight;
            TotalPriceText.Text = totalPrice.ToString("C0");
        }

        private void ConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем даты еще раз
                if (_checkIn < DateTime.Today)
                {
                    MessageBox.Show("Дата заезда не может быть в прошлом",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_checkIn >= _checkOut)
                {
                    MessageBox.Show("Дата выезда должна быть позже даты заезда",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if ((_checkOut - _checkIn).TotalDays > 30)
                {
                    MessageBox.Show("Максимальный срок бронирования - 30 дней",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверяем, не забронирован ли номер на эти даты
                SqlParameter[] checkParams = {
                    new SqlParameter("@RoomID", _roomId),
                    new SqlParameter("@CheckIn", _checkIn),
                    new SqlParameter("@CheckOut", _checkOut)
                };

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT COUNT(*) FROM Bookings 
                    WHERE RoomID = @RoomID 
                    AND Status = 'Активно'
                    AND NOT (CheckOutDate <= @CheckIn OR CheckInDate >= @CheckOut)",
                    checkParams))
                {
                    if (reader.Read() && reader.GetInt32(0) > 0)
                    {
                        MessageBox.Show("Извините, этот номер уже забронирован на выбранные даты",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Создаем бронирование
                decimal totalPrice = (_checkOut - _checkIn).Days * _pricePerNight;

                SqlParameter[] bookParams = {
                    new SqlParameter("@UserID", _currentUser.UserID),
                    new SqlParameter("@RoomID", _roomId),
                    new SqlParameter("@CheckIn", _checkIn),
                    new SqlParameter("@CheckOut", _checkOut),
                    new SqlParameter("@TotalPrice", totalPrice)
                };

                DatabaseConnection.ExecuteNonQuery(@"
                    INSERT INTO Bookings (UserID, RoomID, CheckInDate, CheckOutDate, TotalPrice, Status, BookingDate)
                    VALUES (@UserID, @RoomID, @CheckIn, @CheckOut, @TotalPrice, 'Активно', GETDATE())",
                    bookParams);

                MessageBox.Show("Бронирование успешно создано!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new BookingsPage(_currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании бронирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}