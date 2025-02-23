using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;

namespace BookingWPF
{
    public partial class BookingsPage : Page
    {
        private readonly User _currentUser;

        public BookingsPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadBookings();
        }

        private void LoadBookings()
        {
            try
            {
                // Сначала обновляем статусы
                DatabaseConnection.ExecuteNonQuery("EXEC UpdateBookingStatuses");
                
                LoadActiveBookings();
                LoadHistoryBookings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке бронирований: {ex.Message}");
            }
        }

        private void LoadActiveBookings()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@UserID", _currentUser.UserID) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT b.BookingID, h.Name as HotelName, r.RoomType,
                           b.CheckInDate, b.CheckOutDate, b.TotalPrice
                    FROM Bookings b
                    JOIN Rooms r ON b.RoomID = r.RoomID
                    JOIN Hotels h ON r.HotelID = h.HotelID
                    WHERE b.UserID = @UserID AND b.Status = 'Активно'
                    ORDER BY b.CheckInDate", parameters))
                {
                    ActiveBookingsPanel.Children.Clear();

                    while (reader.Read())
                    {
                        Border bookingCard = CreateBookingCard(reader, true);
                        ActiveBookingsPanel.Children.Add(bookingCard);
                    }

                    if (ActiveBookingsPanel.Children.Count == 0)
                    {
                        TextBlock noBookings = new TextBlock
                        {
                            Text = "У вас нет активных бронирований",
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                            Margin = new Thickness(0, 20, 0, 0)
                        };
                        ActiveBookingsPanel.Children.Add(noBookings);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке активных бронирований: {ex.Message}");
            }
        }

        private void LoadHistoryBookings()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@UserID", _currentUser.UserID) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
                    SELECT b.BookingID, h.Name as HotelName, r.RoomType,
                           b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.Status
                    FROM Bookings b
                    JOIN Rooms r ON b.RoomID = r.RoomID
                    JOIN Hotels h ON r.HotelID = h.HotelID
                    WHERE b.UserID = @UserID AND b.Status IN ('Завершено', 'Отменено')
                    ORDER BY b.CheckInDate DESC", parameters))
                {
                    HistoryBookingsPanel.Children.Clear();

                    while (reader.Read())
                    {
                        Border bookingCard = CreateBookingCard(reader, false);
                        HistoryBookingsPanel.Children.Add(bookingCard);
                    }

                    if (HistoryBookingsPanel.Children.Count == 0)
                    {
                        TextBlock noHistory = new TextBlock
                        {
                            Text = "История бронирований пуста",
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                            Margin = new Thickness(0, 20, 0, 0)
                        };
                        HistoryBookingsPanel.Children.Add(noHistory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке истории бронирований: {ex.Message}");
            }
        }

        private Border CreateBookingCard(SqlDataReader reader, bool isActive)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(0, 0, 0, 20),
                Padding = new Thickness(20)
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Информация о бронировании
            StackPanel info = new StackPanel();
            Grid.SetColumn(info, 0);

            info.Children.Add(new TextBlock
            {
                Text = reader["HotelName"].ToString(),
                FontSize = 20,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            });

            info.Children.Add(new TextBlock
            {
                Text = reader["RoomType"].ToString(),
                FontSize = 16,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                Margin = new Thickness(0, 5, 0, 5)
            });

            DateTime checkIn = (DateTime)reader["CheckInDate"];
            DateTime checkOut = (DateTime)reader["CheckOutDate"];
            info.Children.Add(new TextBlock
            {
                Text = $"Даты: {checkIn:dd.MM.yyyy} - {checkOut:dd.MM.yyyy}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"))
            });

            info.Children.Add(new TextBlock
            {
                Text = $"Стоимость: {reader["TotalPrice"]:C0}",
                FontSize = 16,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A0FF")),
                Margin = new Thickness(0, 10, 0, 0)
            });

            if (!isActive)
            {
                info.Children.Add(new TextBlock
                {
                    Text = reader["Status"].ToString(),
                    Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(
                            reader["Status"].ToString() == "Завершено" ? "#28A745" : "#DC3545")),
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }

            grid.Children.Add(info);

            // Кнопки
            if (isActive)
            {
                StackPanel buttons = new StackPanel
                {
                    Margin = new Thickness(20, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(buttons, 1);

                Button cancelButton = new Button
                {
                    Content = "Отменить",
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC3545")),
                    Foreground = Brushes.White,
                    Padding = new Thickness(20, 10, 20, 10),
                    BorderThickness = new Thickness(0)
                };

                int bookingId = reader.GetInt32(reader.GetOrdinal("BookingID"));
                cancelButton.Click += (s, e) => CancelBooking(bookingId);

                buttons.Children.Add(cancelButton);
                grid.Children.Add(buttons);
            }

            card.Child = grid;
            return card;
        }

        private void CancelBooking(int bookingId)
        {
            if (MessageBox.Show("Вы уверены, что хотите отменить бронирование?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    SqlParameter[] parameters = { new SqlParameter("@BookingID", bookingId) };
                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE Bookings SET Status = 'Отменено' WHERE BookingID = @BookingID",
                        parameters);

                    LoadBookings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отмене бронирования: {ex.Message}");
                }
            }
        }
    }
}