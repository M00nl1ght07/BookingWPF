using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Linq;

namespace BookingWPF.Services
{
    public class AttractionsService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<Attraction>> GetNearbyAttractions(double latitude, double longitude, double radius = 1500)
        {
            var attractions = new List<Attraction>();
            
            try
            {
                var latStr = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var lonStr = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string query = $@"[out:json];
                    (
                        // Достопримечательности
                        node['tourism'](around:{radius},{latStr},{lonStr});
                        node['historic'](around:{radius},{latStr},{lonStr});
                        node['leisure'='park'](around:{radius},{latStr},{lonStr});
                        
                        // Развлечения и культура
                        node['amenity'='cinema'](around:{radius},{latStr},{lonStr});
                        node['amenity'='theatre'](around:{radius},{latStr},{lonStr});
                        node['amenity'='arts_centre'](around:{radius},{latStr},{lonStr});
                        node['amenity'='concert_hall'](around:{radius},{latStr},{lonStr});
                        
                        // Еда и напитки
                        node['amenity'='restaurant'](around:{radius},{latStr},{lonStr});
                        node['amenity'='cafe'](around:{radius},{latStr},{lonStr});
                        node['amenity'='bar'](around:{radius},{latStr},{lonStr});
                        
                        // Торговые центры
                        node['shop'='mall'](around:{radius},{latStr},{lonStr});
                    );
                    out body;";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "BookingWPF/1.0");
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("data", query)
                });

                var response = await client.PostAsync("https://overpass-api.de/api/interpreter", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API вернул ошибку: {response.StatusCode}. {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                foreach (var element in json["elements"])
                {
                    var tags = element["tags"];
                    if (tags != null && tags["name"] != null)
                    {
                        var attraction = new Attraction
                        {
                            Name = tags["name"].ToString(),
                            Type = GetAttractionType(tags),
                            Latitude = (double)element["lat"],
                            Longitude = (double)element["lon"],
                            Distance = CalculateDistance(latitude, longitude, 
                                (double)element["lat"], (double)element["lon"])
                        };
                        attractions.Add(attraction);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении мест: {ex.Message}");
                return new List<Attraction>();
            }

            return attractions.OrderBy(a => a.Distance).ToList();
        }

        private async Task<List<Attraction>> GetAttractionsViaNominatim(double latitude, double longitude, double radius)
        {
            var attractions = new List<Attraction>();
            
            try
            {
                // Конвертируем радиус из метров в градусы (приблизительно)
                double degreeRadius = radius / 111000.0;
                
                string bbox = $"{longitude - degreeRadius},{latitude - degreeRadius}," +
                            $"{longitude + degreeRadius},{latitude + degreeRadius}";

                var response = await client.GetAsync(
                    $"https://nominatim.openstreetmap.org/search?" +
                    $"format=json&" +
                    $"bounded=1&" +
                    $"bbox={bbox}&" +
                    $"amenity=tourism,historic_site,museum,park&" +
                    $"limit=50");

                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var places = JArray.Parse(jsonResponse);

                foreach (var place in places)
                {
                    var attraction = new Attraction
                    {
                        Name = place["display_name"].ToString().Split(',')[0],
                        Type = place["type"].ToString(),
                        Latitude = double.Parse(place["lat"].ToString()),
                        Longitude = double.Parse(place["lon"].ToString()),
                        Distance = CalculateDistance(latitude, longitude,
                            double.Parse(place["lat"].ToString()),
                            double.Parse(place["lon"].ToString()))
                    };
                    attractions.Add(attraction);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении достопримечательностей: {ex.Message}");
            }

            return attractions;
        }

        private string GetBoundingBox(double lat, double lon, double radius)
        {
            // Конвертируем радиус из метров в градусы (приблизительно)
            double degreeRadius = radius / 111000.0; // 1 градус ≈ 111 км
            return $"{lat - degreeRadius},{lat + degreeRadius},{lon - degreeRadius},{lon + degreeRadius}";
        }

        private string GetAttractionType(JToken tags)
        {
            // Определяем тип места
            if (tags["amenity"] != null)
            {
                switch (tags["amenity"].ToString())
                {
                    case "restaurant": return "Ресторан";
                    case "cafe": return "Кафе";
                    case "bar": return "Бар";
                    case "cinema": return "Кинотеатр";
                    case "theatre": return "Театр";
                    case "arts_centre": return "Арт-центр";
                    case "concert_hall": return "Концертный зал";
                }
            }

            if (tags["shop"] != null && tags["shop"].ToString() == "mall")
                return "Торговый центр";

            if (tags["tourism"] != null)
            {
                switch (tags["tourism"].ToString())
                {
                    case "museum": return "Музей";
                    case "gallery": return "Галерея";
                    case "hotel": return "Отель";
                    case "attraction": return "Достопримечательность";
                    case "viewpoint": return "Смотровая площадка";
                }
            }

            if (tags["historic"] != null)
            {
                switch (tags["historic"].ToString())
                {
                    case "monument": return "Памятник";
                    case "memorial": return "Мемориал";
                    case "castle": return "Замок";
                    case "ruins": return "Руины";
                    case "archaeological_site": return "Археологический объект";
                }
            }

            if (tags["leisure"] != null)
            {
                switch (tags["leisure"].ToString())
                {
                    case "park": return "Парк";
                    case "garden": return "Сад";
                }
            }

            return "Достопримечательность";
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Радиус Земли в км
            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c * 1000; // Переводим в метры
        }

        private double ToRad(double degrees) => degrees * Math.PI / 180;
    }
}