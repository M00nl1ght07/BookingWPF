using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;
using System.Windows.Navigation;

namespace BookingWPF
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            LoadCities();
            LoadPopularHotels();
            
            // Минимальная дата бронирования - сегодня
            CheckInDatePicker.DisplayDateStart = DateTime.Today;
            CheckOutDatePicker.DisplayDateStart = DateTime.Today;

            try
            {
                string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Image1.Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "Photos", "skidka1.jpg")));
                Image2.Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "Photos", "cashback10.png")));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображений: {ex.Message}");
            }
        }

        private void LoadCities()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT DISTINCT City FROM Hotels ORDER BY City"))
                {
                    while (reader.Read())
                    {
                        CityComboBox.Items.Add(reader["City"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке городов: {ex.Message}");
            }
        }

        private void SearchHotels_Click(object sender, RoutedEventArgs e)
        {
            string selectedCity = CityComboBox.SelectedItem?.ToString();
            DateTime? checkIn = CheckInDatePicker.SelectedDate;
            DateTime? checkOut = CheckOutDatePicker.SelectedDate;

            // Проверяем даты
            if (checkIn.HasValue && checkOut.HasValue)
            {
                if (checkIn.Value < DateTime.Today)
                {
                    MessageBox.Show("Дата заезда не может быть в прошлом", 
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (checkIn.Value >= checkOut.Value)
                {
                    MessageBox.Show("Дата выезда должна быть позже даты заезда", 
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if ((checkOut.Value - checkIn.Value).TotalDays > 30)
                {
                    MessageBox.Show("Максимальный срок бронирования - 30 дней", 
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Создаем объект для передачи параметров поиска
            var searchParams = new HotelSearchParameters
            {
                City = selectedCity,
                CheckInDate = checkIn,
                CheckOutDate = checkOut
            };

            NavigationService.Navigate(new HotelsPage(searchParams));
        }

        private void LoadPopularHotels()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(
                    "SELECT HotelID, Name, City, BasePrice, Description, PhotoPath FROM Hotels WHERE IsPopular = 1"))
                {
                    PopularHotelsPanel.Children.Clear();

                    while (reader.Read())
                    {
                        Border hotelCard = new Border
                        {
                            Width = 300,
                            Height = 400,
                            Margin = new Thickness(0, 0, 20, 20),
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                            CornerRadius = new CornerRadius(10)
                        };

                        Grid grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                        // Фото отеля
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, reader["PhotoPath"].ToString()));
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        Image hotelImage = new Image
                        {
                            Source = bitmap,
                            Stretch = Stretch.UniformToFill
                        };
                        Grid.SetRow(hotelImage, 0);

                        // Информация об отеле
                        StackPanel infoPanel = new StackPanel { Margin = new Thickness(15) };
                        Grid.SetRow(infoPanel, 1);

                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = reader["Name"].ToString(),
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold
                        });

                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = reader["City"].ToString(),
                            FontSize = 14,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"))
                        });

                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = $"От {reader["BasePrice"]:C0} за ночь",
                            FontSize = 16,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Margin = new Thickness(0, 10, 0, 10)
                        });

                        Button detailsButton = new Button
                        {
                            Content = "Подробнее",
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Foreground = Brushes.White,
                            Padding = new Thickness(20, 10, 20, 10),
                            Margin = new Thickness(0),
                            BorderThickness = default(Thickness)
                        };

                        int hotelId = Convert.ToInt32(reader["HotelID"]);
                        detailsButton.Click += (s, e) => 
                        {
                            var mainWindow = (MainWindow)Application.Current.MainWindow;
                            NavigationService.Navigate(new RoomSelectionPage(hotelId, mainWindow.CurrentUser));
                        };

                        infoPanel.Children.Add(detailsButton);

                        grid.Children.Add(hotelImage);
                        grid.Children.Add(infoPanel);
                        hotelCard.Child = grid;

                        PopularHotelsPanel.Children.Add(hotelCard);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке популярных отелей: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SpecialOffer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HotelsPage());
        }
    }

    // Класс для передачи параметров поиска
    public class HotelSearchParameters
    {
        public string City { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}