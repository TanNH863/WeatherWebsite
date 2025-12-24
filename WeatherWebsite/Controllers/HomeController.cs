using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WeatherWebsite.Models;
using WeatherWebsite.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace WeatherWebsite.Controllers
{
    public class HomeController(ILogger<HomeController> logger, WeatherApiClient weatherApiClient, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly WeatherApiClient _weatherApiClient = weatherApiClient;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        private List<Location> GetLocations()
        {
            var json = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.ContentRootPath, "Data", "locations.json"));
            return JsonSerializer.Deserialize<List<Location>>(json);
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var locations = GetLocations();
            ViewData["Locations"] = new SelectList(locations, "Name", "Name");

            // Default location for the initial load
            string selectedLocation = "London, England, UK";
            var selectedTimeZone = locations.First(l => l.Name == selectedLocation).TimeZone;
            ViewData["SelectedLocation"] = selectedLocation;
            ViewData["SelectedTimeZone"] = selectedTimeZone;

            // Get weather data for the selected location
            double lat = locations.First(l => l.Name == selectedLocation).Lat;
            double lng = locations.First(l => l.Name == selectedLocation).Lng;

            var getCurrentWeather = await _weatherApiClient.GetCurrentWeather(selectedLocation);
            var getWeatherForecast = await _weatherApiClient.GetWeatherForecast(selectedLocation);
            var getAirPollutionData = await _weatherApiClient.GetAQIData(lat, lng);

            ViewData["CurrentWeather"] = getCurrentWeather;
            ViewData["Forecast"] = getWeatherForecast;
            ViewData["AQIData"] = getAirPollutionData;

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(string selectedLocation)
        {
            var locations = GetLocations();
            ViewData["Locations"] = new SelectList(locations, "Name", "Name", selectedLocation);

            var selectedTimeZone = locations.First(l => l.Name == selectedLocation).TimeZone;
            ViewData["SelectedTimeZone"] = selectedTimeZone;

            // Get weather data for the selected location
            double lat = locations.First(l => l.Name == selectedLocation).Lat;
            double lng = locations.First(l => l.Name == selectedLocation).Lng;

            var getCurrentWeather = await _weatherApiClient.GetCurrentWeather(selectedLocation);
            var getWeatherForecast = await _weatherApiClient.GetWeatherForecast(selectedLocation);
            var getAirPollutionData = await _weatherApiClient.GetAQIData(lat, lng);

            ViewData["CurrentWeather"] = getCurrentWeather;
            ViewData["Forecast"] = getWeatherForecast;
            ViewData["SelectedLocation"] = selectedLocation;
            ViewData["AQIData"] = getAirPollutionData;

            return View();
        }

        public IActionResult WorldTime()
        {
            return View();
        }

        public IActionResult TimeConvert()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
