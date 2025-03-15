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

        public AttractionMapPage(double latitude, double longitude, string hotelName)
        {
            InitializeComponent();
            _hotelLat = latitude;
            _hotelLon = longitude;
            _hotelName = hotelName;
            _attractionsService = new AttractionsService();

            // Добавляем отладочный вывод
            MessageBox.Show($"Координаты отеля: {latitude}, {longitude}");

            Loaded += AttractionMapPage_Loaded;
        }

        private async void AttractionMapPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var attractions = await _attractionsService.GetNearbyAttractions(_hotelLat, _hotelLon);

                foreach (var attraction in attractions)
                {
                    var attractionPanel = new StackPanel 
                    { 
                        Margin = new Thickness(0, 0, 0, 10),
                        Cursor = Cursors.Hand
                    };

                    var nameBlock = new TextBlock
                    {
                        Text = attraction.Name,
                        FontSize = 16,
                        Foreground = Brushes.White,
                        FontWeight = FontWeights.Bold
                    };

                    var infoBlock = new TextBlock
                    {
                        Text = $"{attraction.Type} • {attraction.Distance:F0} м от отеля",
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"))
                    };

                    attractionPanel.Children.Add(nameBlock);
                    attractionPanel.Children.Add(infoBlock);

                    // Добавляем обработчик клика
                    attractionPanel.MouseLeftButtonDown += async (s, args) =>
                    {
                        try
                        {
                            await MapView.CoreWebView2.ExecuteScriptAsync(
                                $"focusAttraction('{attraction.Name.Replace("'", "\\'")}')");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при фокусировке на достопримечательности: {ex.Message}");
                        }
                    };

                    AttractionsPanel.Children.Add(attractionPanel);
                }

                await InitializeMap(attractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке достопримечательностей: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

                // Включаем отладку
                MapView.CoreWebView2.OpenDevToolsWindow();

                MapView.CoreWebView2.Navigate($"file:///{htmlPath}");

                // Ждем загрузку карты
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

                // Отладочный вывод данных
                var debugData = new
                {
                    hotel = new { lat = _hotelLat, lon = _hotelLon, name = _hotelName },
                    attractions = attractions.Select(a => new
                    {
                        a.Name,
                        a.Type,
                        a.Latitude,
                        a.Longitude,
                        a.Distance
                    }).ToList()
                };

                MessageBox.Show($"Данные для карты: {JsonConvert.SerializeObject(debugData, Formatting.Indented)}");

                var script = $@"initMap(
                    {_hotelLat.ToString(System.Globalization.CultureInfo.InvariantCulture)},
                    {_hotelLon.ToString(System.Globalization.CultureInfo.InvariantCulture)},
                    '{_hotelName.Replace("'", "\\'")}',
                    {JsonConvert.SerializeObject(attractions)}
                );";

                await MapView.CoreWebView2.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации карты: {ex.Message}\n\nStack: {ex.StackTrace}");
            }
        }
    }
}