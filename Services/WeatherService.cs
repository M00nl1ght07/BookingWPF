using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookingWPF.Services
{
    public class WeatherService
    {
        private const string API_KEY = "e7704bc895b4a8d2dfd4a29d404285b6";
        private const string BASE_URL = "http://api.openweathermap.org/data/2.5/weather";
        private static readonly HttpClient client = new HttpClient();

        private readonly Dictionary<string, string> _cityTranslations = new Dictionary<string, string>
        {
            {"Санкт - Петербург", "Saint Petersburg"},
            {"Санкт-Петербург", "Saint Petersburg"},
            {"Санкт Петербург", "Saint Petersburg"}
        };

        public async Task<(double Temperature, string Description)> GetWeather(string city)
        {
            try
            {
                // Преобразуем название города если нужно
                if (_cityTranslations.ContainsKey(city))
                {
                    city = _cityTranslations[city];
                }

                var response = await client.GetStringAsync(
                    $"{BASE_URL}?q={city},RU&appid={API_KEY}&units=metric&lang=ru");
                var data = JObject.Parse(response);
                var temp = data["main"]["temp"].Value<double>();
                var description = data["weather"][0]["description"].Value<string>();
                return (temp, description);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении погоды: {ex.Message}");
            }
        }
    }
}