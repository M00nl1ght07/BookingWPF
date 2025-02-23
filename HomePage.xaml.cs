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
            LoadPopularHotels();

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

        private void LoadPopularHotels()
        {
            try
            {
                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(@"
            SELECT h.HotelID, h.Name, h.City, h.Rating, h.BasePrice, h.PhotoPath, h.Description,
                   (SELECT COUNT(*) FROM Rooms r WHERE r.HotelID = h.HotelID) as RoomCount
            FROM Hotels h
            WHERE h.IsPopular = 1"))
                {
                    PopularHotelsPanel.Children.Clear();

                    while (reader.Read())
                    {
                        // Создаем карточку отеля
                        Border hotelCard = new Border
                        {
                            Width = 300,
                            Height = 350,
                            Margin = new Thickness(0, 0, 20, 0),
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
                            Padding = new Thickness(20, 5, 20, 5)
                        };

                        infoPanel.Children.Add(detailsButton);

                        grid.Children.Add(hotelImage);
                        grid.Children.Add(infoPanel);
                        hotelCard.Child = grid;

                        // Добавляем обработчик клика
                        int hotelId = Convert.ToInt32(reader["HotelID"]);
                        detailsButton.Click += (s, e) => OpenHotelDetails(hotelId);
                        hotelCard.Cursor = Cursors.Hand;

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

        private void OpenHotelDetails(int hotelId)
        {
            // Здесь будет переход на страницу отеля
            // NavigationService.Navigate(new HotelDetailsPage(hotelId));
        }

        private void SpecialOffer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HotelsPage());
        }
    }
}