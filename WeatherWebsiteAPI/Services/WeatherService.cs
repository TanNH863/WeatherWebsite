using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherWebsiteAPI.Models;
using DotNetEnv;

namespace WeatherWebsiteAPI.Services
{
    public class WeatherService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string? apiKey;
        private readonly string apiUrl = "http://api.openweathermap.org/data/2.5/";

        public WeatherService()
        {
            Env.Load();
            apiKey = Environment.GetEnvironmentVariable("API_KEY");
        }

        public async Task<WeatherData> GetCurrentWeather(string location)
        {
            string url = $"{apiUrl}weather?q={location}&units=metric&appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonString);
                return weatherData ?? new WeatherData();
            }
            else
            {
                throw new Exception("Error fetching current weather data.");
            }
        }

        public async Task<WeatherData> GetCurrentWeather(double lat, double lon)
        {
            string url = $"{apiUrl}weather?lat={lat}&lon={lon}&units=metric&appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonString);
                return weatherData ?? new WeatherData();
            }
            else
            {
                throw new Exception("Error fetching current weather data.");
            }
        }

        public async Task<ForecastData> GetWeatherForecast(string location)
        {
            string url = $"{apiUrl}forecast?q={location}&units=metric&appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var forecastData = JsonConvert.DeserializeObject<ForecastData>(jsonString);
                return forecastData ?? new ForecastData();
            }
            else
            {
                throw new Exception("Error fetching weather forecast data.");
            }
        }

        public async Task<AQIData> GetAQIData(double lat, double lon)
        {
            string url = $"{apiUrl}air_pollution?lat={lat}&lon={lon}&appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var aqiData = JsonConvert.DeserializeObject<AQIData>(jsonString);
                return aqiData ?? new AQIData();
            }
            else
            {
                throw new Exception("Error fetching weather air pollution data.");
            }
        }

        public async Task<byte[]> GetWeatherTileAsync(string layer, int z, int x, int y)
        {
            string url = $"https://tile.openweathermap.org/map/{layer}/{z}/{x}/{y}.png?appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                throw new Exception("Error fetching weather tile data.");
            }
        }
    }
}
