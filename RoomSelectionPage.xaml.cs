using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BookingWPF
{
    public partial class RoomSelectionPage : Page
    {
        private readonly int _hotelId;
        private readonly User _currentUser;
        private Dictionary<TextBlock, decimal> _originalPrices = new Dictionary<TextBlock, decimal>();

        public RoomSelectionPage(int hotelId, User currentUser = null)
        {
            InitializeComponent();
            _hotelId = hotelId;
            _currentUser = currentUser;
            LoadHotelInfo();
            LoadRooms();
            
            // После загрузки номеров конвертируем цены
            if (App.CurrentCurrency != "RUB")
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await UpdatePrices(App.CurrentCurrency);
                }));
            }
        }

        private async void LoadHotelInfo()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@HotelID", _hotelId) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT Name, City FROM Hotels WHERE HotelID = @HotelID", parameters))
                {
                    if (reader.Read())
                    {
                        string hotelName = reader["Name"].ToString();
                        string city = reader["City"].ToString();
                        
                        HotelNameText.Text = hotelName;
                        HotelCityText.Text = city;

                        // Загружаем погоду
                        try
                        {
                            var (temperature, description) = await App.WeatherService.GetWeather(city);
                            WeatherText.Text = $"Погода: {temperature:F1}°C, {description}";
                        }
                        catch
                        {
                            WeatherText.Text = "Информация о погоде недоступна";
                        }
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
                        var roomCard = new Border
                        {
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                            Margin = new Thickness(0, 0, 0, 10),
                            Padding = new Thickness(20),
                            CornerRadius = new CornerRadius(5)
                        };

                        var grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
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

                        // Добавляем элементы в правильном порядке
                        grid.Children.Add(roomImage);      // Изображение
                        grid.Children.Add(infoPanel);      // Информация о номере

                        // Цена
                        decimal originalPrice = Convert.ToDecimal(reader["PricePerNight"]);
                        var priceTextBlock = new TextBlock
                        {
                            Text = $"{originalPrice} ₽ за ночь",
                            FontSize = 20,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(0, 0, 0, 10)
                        };

                        // Сохраняем оригинальную цену
                        _originalPrices[priceTextBlock] = originalPrice;

                        Grid.SetColumn(priceTextBlock, 2);
                        grid.Children.Add(priceTextBlock);

                        // Кнопка бронирования
                        Button bookButton = new Button
                        {
                            Content = "Забронировать",
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Foreground = Brushes.White,
                            Padding = new Thickness(20, 10, 20, 10),
                            Height = 40,
                            BorderThickness = new Thickness(0),
                            VerticalAlignment = VerticalAlignment.Bottom
                        };

                        int roomId = Convert.ToInt32(reader["RoomID"]);
                        bookButton.Click += (s, e) => BookRoom_Click(roomId);

                        Grid.SetColumn(bookButton, 2);
                        grid.Children.Add(bookButton);

                        roomCard.Child = grid;
                        RoomsStackPanel.Children.Add(roomCard);
                    }
                }

                // После загрузки сразу конвертируем в текущую валюту
                if (App.CurrentCurrency != "RUB")
                {
                    Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        await UpdatePrices(App.CurrentCurrency);
                    }));
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

        private void ShowAttractions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@HotelID", _hotelId) };
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT Name, Latitude, Longitude FROM Hotels WHERE HotelID = @HotelID", 
                    parameters))
                {
                    if (reader.Read())
                    {
                        NavigationService.Navigate(new AttractionMapPage(
                            reader.GetDouble(reader.GetOrdinal("Latitude")),
                            reader.GetDouble(reader.GetOrdinal("Longitude")),
                            reader.GetString(reader.GetOrdinal("Name"))
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке карты: {ex.Message}");
            }
        }

        public async Task UpdatePrices(string currency)
        {
            foreach (var room in RoomsStackPanel.Children.OfType<Border>())
            {
                var priceTextBlock = FindPriceTextBlock(room);
                if (priceTextBlock != null)
                {
                    // Сохраняем оригинальную цену при первой конвертации
                    if (!_originalPrices.ContainsKey(priceTextBlock))
                    {
                        string priceText = priceTextBlock.Text;
                        decimal originalPrice = decimal.Parse(priceText.Split(' ')[0]); // Берем первое число
                        _originalPrices[priceTextBlock] = originalPrice;
                    }

                    decimal convertedPrice = await App.CurrencyService.ConvertPrice(_originalPrices[priceTextBlock], currency);
                    string currencySymbol;
                    switch (currency)
                    {
                        case "USD":
                            currencySymbol = "$";
                            break;
                        case "EUR":
                            currencySymbol = "€";
                            break;
                        default:
                            currencySymbol = "₽";
                            break;
                    }
                    priceTextBlock.Text = $"{convertedPrice} {currencySymbol} за ночь";
                }
            }
        }

        private TextBlock FindPriceTextBlock(Border room)
        {
            var grid = room.Child as Grid;
            return grid?.Children.OfType<TextBlock>()
                .FirstOrDefault(t => t.Text.Contains("за ночь"));
        }
    }
}