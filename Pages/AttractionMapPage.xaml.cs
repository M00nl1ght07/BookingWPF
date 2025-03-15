using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using System.Collections.Generic;
using BookingWPF.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq; // Для Select

namespace BookingWPF
{
    public partial class AttractionMapPage : Page
    {
        private readonly double _hotelLat;
        private readonly double _hotelLon;
        private readonly string _hotelName;
        private readonly AttractionsService _attractionsService;
        private Dictionary<string, List<Attraction>> _categorizedAttractions;
        private List<Attraction> _allAttractions;

        public AttractionMapPage(double latitude, double longitude, string hotelName)
        {
            InitializeComponent();
            _hotelLat = latitude;
            _hotelLon = longitude;
            _hotelName = hotelName;
            _attractionsService = new AttractionsService();
            Loaded += AttractionMapPage_Loaded;
        }

        private async void AttractionMapPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _allAttractions = await _attractionsService.GetNearbyAttractions(_hotelLat, _hotelLon);
                
                // Группируем места по категориям
                _categorizedAttractions = _allAttractions
                    .GroupBy(a => GetMainCategory(a.Type))
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Создаем элементы интерфейса для категорий
                foreach (var category in _categorizedAttractions)
                {
                    var expander = new Expander
                    {
                        Header = $"{category.Key} ({category.Value.Count})",
                        Margin = new Thickness(0, 0, 0, 5),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D")),
                        Foreground = Brushes.White
                    };

                    var stackPanel = new StackPanel();
                    foreach (var attraction in category.Value)
                    {
                        var attractionPanel = CreateAttractionPanel(attraction);
                        stackPanel.Children.Add(attractionPanel);
                    }

                    expander.Content = stackPanel;
                    expander.Expanded += (s, args) => ShowCategoryOnMap(category.Key);
                    expander.Collapsed += (s, args) => HideCategoryFromMap(category.Key);

                    CategoriesPanel.Children.Add(expander);
                }

                await InitializeMap(_allAttractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}");
            }
        }

        private StackPanel CreateAttractionPanel(Attraction attraction)
        {
            var panel = new StackPanel
            {
                Margin = new Thickness(5),
                Cursor = Cursors.Hand
            };

            var nameBlock = new TextBlock
            {
                Text = attraction.Name,
                FontSize = 14,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            var infoBlock = new TextBlock
            {
                Text = $"{Math.Round(attraction.Distance)} м",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0B0B0")),
                FontSize = 12
            };

            panel.Children.Add(nameBlock);
            panel.Children.Add(infoBlock);

            panel.MouseLeftButtonDown += async (s, e) =>
            {
                try
                {
                    await MapView.CoreWebView2.ExecuteScriptAsync(
                        $"focusAttraction('{attraction.Name.Replace("'", "\\'")}')");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при фокусировке: {ex.Message}");
                }
            };

            return panel;
        }

        private string GetMainCategory(string type)
        {
            switch (type)
            {
                case "Ресторан":
                case "Кафе":
                case "Бар":
                    return "Места питания";

                case "Кинотеатр":
                case "Театр":
                case "Арт-центр":
                case "Концертный зал":
                    return "Развлечения";

                case "Музей":
                case "Памятник":
                case "Мемориал":
                case "Достопримечательность":
                    return "Достопримечательности";

                case "Торговый центр":
                    return "Шоппинг";

                case "Парк":
                case "Сад":
                    return "Парки и сады";

                default:
                    return "Другое";
            }
        }

        private async void ShowCategoryOnMap(string category)
        {
            if (_categorizedAttractions.ContainsKey(category))
            {
                var attractions = _categorizedAttractions[category];
                await MapView.CoreWebView2.ExecuteScriptAsync(
                    $"showCategory('{category}', {JsonConvert.SerializeObject(attractions)})");
            }
        }

        private async void HideCategoryFromMap(string category)
        {
            await MapView.CoreWebView2.ExecuteScriptAsync($"hideCategory('{category}')");
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            await FilterAttractions(searchText);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            await FilterAttractions(searchText);
        }

        private async Task FilterAttractions(string searchText)
        {
            foreach (Expander expander in CategoriesPanel.Children)
            {
                var stackPanel = expander.Content as StackPanel;
                if (stackPanel != null)
                {
                    var category = expander.Header.ToString().Split('(')[0].Trim();
                    var attractions = _categorizedAttractions[category];
                    
                    var filteredAttractions = attractions
                        .Where(a => a.Name.ToLower().Contains(searchText) || 
                                   a.Type.ToLower().Contains(searchText))
                        .ToList();

                    expander.Header = $"{category} ({filteredAttractions.Count})";
                    stackPanel.Children.Clear();

                    foreach (var attraction in filteredAttractions)
                    {
                        stackPanel.Children.Add(CreateAttractionPanel(attraction));
                    }
                }
            }
        }

        private async Task InitializeMap(List<Attraction> attractions)
        {
            try
            {
                await MapView.EnsureCoreWebView2Async();

                string htmlPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "map.html"
                );

                if (!System.IO.File.Exists(htmlPath))
                {
                    MessageBox.Show($"Файл карты не найден по пути: {htmlPath}");
                    return;
                }

                MapView.CoreWebView2.Navigate($"file:///{htmlPath}");

                var pageLoadedTcs = new TaskCompletionSource<bool>();
                EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = null;
                handler = (s, e) =>
                {
                    MapView.CoreWebView2.NavigationCompleted -= handler;
                    pageLoadedTcs.SetResult(true);
                };
                MapView.CoreWebView2.NavigationCompleted += handler;
                await pageLoadedTcs.Task;
                await Task.Delay(500);

                // Инициализируем карту только с отелем
                var script = $@"initMap(
                    {_hotelLat.ToString(System.Globalization.CultureInfo.InvariantCulture)},
                    {_hotelLon.ToString(System.Globalization.CultureInfo.InvariantCulture)},
                    '{_hotelName.Replace("'", "\\'")}'
                );";

                await MapView.CoreWebView2.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации карты: {ex.Message}");
            }
        }
    }
}