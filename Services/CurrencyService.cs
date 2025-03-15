using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookingWPF.Services
{
    public class CurrencyService
    {
        private const string API_KEY = "ваш_ключ";
        private const string BASE_URL = "https://api.exchangerate-api.com/v4/latest/RUB";
        private static readonly HttpClient client = new HttpClient();
        private static Dictionary<string, decimal> _rates;
        private static DateTime _lastUpdate = DateTime.MinValue;

        private static readonly string[] availableCurrencies = new[] { "RUB", "USD", "EUR", "GBP" };

        public static string[] AvailableCurrencies => availableCurrencies;

        public async Task<decimal> GetOriginalPrice(decimal price, string fromCurrency)
        {
            if (fromCurrency == "RUB") return price;

            // Обновляем курсы если нужно
            if (_rates == null || DateTime.Now - _lastUpdate > TimeSpan.FromHours(1))
            {
                await UpdateRates();
            }

            if (_rates.TryGetValue(fromCurrency, out decimal rate) && rate > 0)
            {
                return Math.Round(price / rate, 2);
            }

            throw new Exception($"Не удалось конвертировать из {fromCurrency}");
        }

        public async Task<decimal> ConvertPrice(decimal priceInRub, string targetCurrency)
        {
            if (targetCurrency == "RUB") return priceInRub;

            if (_rates == null || DateTime.Now - _lastUpdate > TimeSpan.FromHours(1))
            {
                await UpdateRates();
            }

            if (_rates.TryGetValue(targetCurrency, out decimal rate))
            {
                return Math.Round(priceInRub * rate, 2);
            }

            throw new Exception($"Валюта {targetCurrency} не поддерживается");
        }

        private async Task UpdateRates()
        {
            try
            {
                var response = await client.GetStringAsync(BASE_URL);
                var data = JObject.Parse(response);
                var rates = data["rates"].ToObject<Dictionary<string, decimal>>();

                _rates = rates;
                _lastUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении курсов валют: " + ex.Message);
            }
        }
    }
}
