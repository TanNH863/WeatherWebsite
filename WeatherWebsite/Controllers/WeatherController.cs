using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WeatherWebsite.Models;
using WeatherWebsite.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace WeatherWebsite.Controllers
{
    public class WeatherController(ILogger<WeatherController> logger, WeatherApiClient weatherApiClient, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly ILogger<WeatherController> _logger = logger;
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
            ViewData["SelectedLocation"] = selectedLocation;

            // Get weather data for the selected location
            var getCurrentWeather = await _weatherApiClient.GetCurrentWeather(selectedLocation);
            var getWeatherForecast = await _weatherApiClient.GetWeatherForecast(selectedLocation);

            ViewData["CurrentWeather"] = getCurrentWeather;
            ViewData["Forecast"] = getWeatherForecast;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(string selectedLocation)
        {
            var locations = GetLocations();
            ViewData["Locations"] = new SelectList(locations, "Name", "Name", selectedLocation);

            // Get weather data for the selected location
            var getCurrentWeather = await _weatherApiClient.GetCurrentWeather(selectedLocation);
            var getWeatherForecast = await _weatherApiClient.GetWeatherForecast(selectedLocation);

            ViewData["CurrentWeather"] = getCurrentWeather;
            ViewData["Forecast"] = getWeatherForecast;
            ViewData["SelectedLocation"] = selectedLocation;

            return View();
        }
    }
}
