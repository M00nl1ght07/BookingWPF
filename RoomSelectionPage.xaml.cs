using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;

namespace BookingWPF
{
    public partial class RoomSelectionPage : Page
    {
        private readonly int _hotelId;
        private readonly User _currentUser;

        public RoomSelectionPage(int hotelId, User currentUser = null)
        {
            InitializeComponent();
            _hotelId = hotelId;
            _currentUser = currentUser;
            LoadHotelInfo();
            LoadRooms();
        }

        private void LoadHotelInfo()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@HotelID", _hotelId) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT Name, City FROM Hotels WHERE HotelID = @HotelID", parameters))
                {
                    if (reader.Read())
                    {
                        HotelNameText.Text = reader["Name"].ToString();
                        HotelCityText.Text = reader["City"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации об отеле: {ex.Message}");
            }
        }

        private void LoadRooms()
        {
            try
            {
                string query = @"
                    SELECT r.RoomID, r.RoomType, r.Area, r.Capacity, 
                           r.PricePerNight, r.Description, r.PhotoPath
                    FROM Rooms r
                    WHERE r.HotelID = @HotelID";

                if (CheckInDatePicker.SelectedDate.HasValue && CheckOutDatePicker.SelectedDate.HasValue)
                {
                    // Используем процедуру для проверки доступности
                    query = @"
                        EXEC UpdateBookingStatuses;
                        SELECT r.RoomID, r.RoomType, r.Area, r.Capacity, 
                               r.PricePerNight, r.Description, r.PhotoPath
                        FROM Rooms r
                        WHERE r.HotelID = @HotelID
                        AND NOT EXISTS (
                            SELECT 1 FROM Bookings b
                            WHERE b.RoomID = r.RoomID
                            AND b.Status = 'Активно'
                            AND NOT (b.CheckOutDate <= @CheckIn OR b.CheckInDate >= @CheckOut)
                        )";
                }

                var parameters = new List<SqlParameter> { new SqlParameter("@HotelID", _hotelId) };

                if (CheckInDatePicker.SelectedDate.HasValue && CheckOutDatePicker.SelectedDate.HasValue)
                {
                    parameters.Add(new SqlParameter("@CheckIn", CheckInDatePicker.SelectedDate.Value));
                    parameters.Add(new SqlParameter("@CheckOut", CheckOutDatePicker.SelectedDate.Value));
                }

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(query, parameters.ToArray()))
                {
                    RoomsStackPanel.Children.Clear();

                    while (reader.Read())
                    {
                        // Создаем карточку номера
                        Border roomCard = new Border
                        {
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                            CornerRadius = new CornerRadius(10),
                            Margin = new Thickness(0, 0, 0, 20),
                            Padding = new Thickness(20)
                        };

                        Grid grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                        // Фото номера
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, reader["PhotoPath"].ToString()));
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        Image roomImage = new Image
                        {
                            Source = bitmap,
                            Stretch = Stretch.UniformToFill,
                            Height = 150
                        };
                        Grid.SetColumn(roomImage, 0);

                        // Информация о номере
                        StackPanel infoPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                        Grid.SetColumn(infoPanel, 1);

                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = reader["RoomType"].ToString(),
                            FontSize = 24,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold
                        });

                        // Характеристики номера
                        WrapPanel features = new WrapPanel { Margin = new Thickness(0, 10, 0, 0) };

                        AddFeature(features, $"Площадь: {reader["Area"]} м²");
                        AddFeature(features, $"{reader["Capacity"]} гостей");
                        
                        infoPanel.Children.Add(features);

                        // Заменяем блок с удобствами на описание
                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = "Описание:",
                            Foreground = Brushes.White,
                            Margin = new Thickness(0, 10, 0, 5)
                        });

                        string description = reader["Description"].ToString();
                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = string.IsNullOrEmpty(description) ? "Нет описания" : description,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                            TextWrapping = TextWrapping.Wrap
                        });

                        // Цена и кнопка бронирования
                        StackPanel pricePanel = new StackPanel
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            MinWidth = 200
                        };
                        Grid.SetColumn(pricePanel, 2);

                        pricePanel.Children.Add(new TextBlock
                        {
                            Text = $"{reader["PricePerNight"]:C0}",
                            FontSize = 24,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A0FF")),
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 10)
                        });

                        pricePanel.Children.Add(new TextBlock
                        {
                            Text = "за ночь",
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 10)
                        });

                        Button bookButton = new Button
                        {
                            Content = "Забронировать",
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Foreground = Brushes.White,
                            Padding = new Thickness(20, 10, 20, 10),
                            Height = 40,
                            BorderThickness = new Thickness(0)
                        };

                        int roomId = Convert.ToInt32(reader["RoomID"]);
                        bookButton.Click += (s, e) => BookRoom_Click(roomId);

                        pricePanel.Children.Add(bookButton);

                        grid.Children.Add(roomImage);
                        grid.Children.Add(infoPanel);
                        grid.Children.Add(pricePanel);
                        roomCard.Child = grid;

                        RoomsStackPanel.Children.Add(roomCard);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке номеров: {ex.Message}");
            }
        }

        private void AddFeature(WrapPanel panel, string text)
        {
            Border border = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E")),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0, 0, 10, 5)
            };

            border.Child = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"))
            };

            panel.Children.Add(border);
        }

        private void BookRoom_Click(int roomId)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Для бронирования необходимо войти в систему", 
                    "Требуется авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
                return;
            }

            if (!CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите даты заезда и выезда", 
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigationService.Navigate(new BookingPage(roomId, _currentUser, 
                CheckInDatePicker.SelectedDate.Value, 
                CheckOutDatePicker.SelectedDate.Value));
        }

        private void SearchRooms_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите даты заезда и выезда", 
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var checkIn = CheckInDatePicker.SelectedDate.Value;
            var checkOut = CheckOutDatePicker.SelectedDate.Value;
            var today = DateTime.Today;

            if (checkIn < today)
            {
                MessageBox.Show("Дата заезда не может быть в прошлом", 
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (checkIn >= checkOut)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда", 
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if ((checkOut - checkIn).TotalDays > 30)
            {
                MessageBox.Show("Максимальный срок бронирования - 30 дней", 
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoadRooms();
        }
    }
}