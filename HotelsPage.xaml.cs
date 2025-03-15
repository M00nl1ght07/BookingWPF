using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingWPF.Services;

namespace BookingWPF
{
    public partial class HotelsPage : Page
    {
        private WrapPanel hotelsPanel;
        private decimal? minPrice;
        private decimal? maxPrice;
        private List<int> selectedRatings = new List<int>();
        private HotelSearchParameters searchParams;
        private Dictionary<TextBlock, decimal> _originalPrices = new Dictionary<TextBlock, decimal>();

        public HotelsPage(HotelSearchParameters searchParams = null)
        {
            InitializeComponent();
            this.searchParams = searchParams;
            hotelsPanel = (WrapPanel)((ScrollViewer)((Grid)Content).Children[1]).Content;
            LoadHotels();
            
            // После загрузки отелей сразу конвертируем в текущую валюту, если она не рубли
            if (App.CurrentCurrency != "RUB")
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await UpdatePrices(App.CurrentCurrency);
                }));
            }
        }

        private void LoadHotels()
        {
            try
            {
                string query = @"
                    SELECT h.HotelID, h.Name, h.City, h.Rating, h.BasePrice, h.Description, h.PhotoPath 
                    FROM Hotels h
                    WHERE 1=1";

                var parameters = new List<SqlParameter>();

                // Фильтр по городу из параметров поиска с главной страницы
                if (searchParams?.City != null && searchParams.City != "Выберите город")
                {
                    query += " AND h.City = @City";
                    parameters.Add(new SqlParameter("@City", searchParams.City));
                }

                // Фильтры по цене
                if (minPrice.HasValue)
                {
                    query += " AND h.BasePrice >= @MinPrice";
                    parameters.Add(new SqlParameter("@MinPrice", minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    query += " AND h.BasePrice <= @MaxPrice";
                    parameters.Add(new SqlParameter("@MaxPrice", maxPrice.Value));
                }

                // Фильтр по рейтингу
                if (selectedRatings.Count > 0)
                {
                    query += " AND h.Rating IN (SELECT value FROM STRING_SPLIT(@Ratings, ','))";
                    parameters.Add(new SqlParameter("@Ratings", string.Join(",", selectedRatings)));
                }

                using (SqlDataReader reader = DatabaseConnection.ExecuteReader(query, parameters.ToArray()))
                {
                    hotelsPanel.Children.Clear();

                    while (reader.Read())
                    {
                        // Используем существующий шаблон карточки из XAML
                        Border hotelCard = new Border
                        {
                            Width = 300,
                            Height = 450,
                            Margin = new Thickness(0, 0, 20, 20),
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                            CornerRadius = new CornerRadius(10)
                        };

                        Grid grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                        Image hotelImage = new Image
                        {
                            Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, reader["PhotoPath"].ToString()))),
                            Stretch = Stretch.UniformToFill
                        };
                        Grid.SetRow(hotelImage, 0);

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
                            Foreground = Brushes.LightGray
                        });

                        decimal originalPrice = Convert.ToDecimal(reader["BasePrice"]);
                        var priceTextBlock = new TextBlock
                        {
                            Text = $"От {originalPrice} ₽ за ночь",
                            FontSize = 16,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Margin = new Thickness(0, 10, 0, 10)
                        };

                        // Сохраняем оригинальную цену сразу при создании
                        _originalPrices[priceTextBlock] = originalPrice;

                        infoPanel.Children.Add(priceTextBlock);

                        infoPanel.Children.Add(new TextBlock
                        {
                            Text = reader["Description"].ToString(),
                            FontSize = 14,
                            Foreground = Brushes.LightGray,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(0, 0, 0, 15),
                            MaxHeight = 60,
                            TextTrimming = TextTrimming.CharacterEllipsis
                        });

                        Button bookButton = new Button
                        {
                            Content = "Забронировать",
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")),
                            Foreground = Brushes.White,
                            Padding = new Thickness(20, 5, 20, 5),
                            Height = 35,
                            Margin = new Thickness(0, 5, 0, 0),
                            BorderThickness = default(Thickness)
                        };

                        int hotelId = Convert.ToInt32(reader["HotelID"]);
                        bookButton.Click += (s, e) => 
                        {
                            var mainWindow = (MainWindow)Application.Current.MainWindow;
                            NavigationService.Navigate(new RoomSelectionPage(hotelId, mainWindow.CurrentUser));
                        };

                        infoPanel.Children.Add(bookButton);

                        grid.Children.Add(hotelImage);
                        grid.Children.Add(infoPanel);
                        hotelCard.Child = grid;

                        hotelsPanel.Children.Add(hotelCard);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отелей: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateFilters()
        {
            // Обновляем значения фильтров цены
            decimal parsedMin;
            decimal parsedMax;
            minPrice = decimal.TryParse(MinPriceTextBox.Text, out parsedMin) ? (decimal?)parsedMin : null;
            maxPrice = decimal.TryParse(MaxPriceTextBox.Text, out parsedMax) ? (decimal?)parsedMax : null;

            // Обновляем выбранные рейтинги
            selectedRatings.Clear();
            if (Rating5CheckBox.IsChecked == true) selectedRatings.Add(5);
            if (Rating4CheckBox.IsChecked == true) selectedRatings.Add(4);
            if (Rating3CheckBox.IsChecked == true) selectedRatings.Add(3);
            if (Rating2CheckBox.IsChecked == true) selectedRatings.Add(2);

            LoadHotels();
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            UpdateFilters();
        }

        public async Task UpdatePrices(string currency)
        {
            foreach (var hotel in hotelsPanel.Children.OfType<Border>())
            {
                var priceTextBlock = FindPriceTextBlock(hotel);
                if (priceTextBlock != null)
                {
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
                    priceTextBlock.Text = $"От {convertedPrice} {currencySymbol} за ночь";
                }
            }
        }

        private TextBlock FindPriceTextBlock(Border hotel)
        {
            var grid = hotel.Child as Grid;
            var stackPanel = grid?.Children.OfType<StackPanel>().FirstOrDefault();
            return stackPanel?.Children.OfType<TextBlock>()
                .FirstOrDefault(t => t.Text.StartsWith("От"));
        }
    }
}


