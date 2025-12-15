using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WeatherWebsite.Models;
using WeatherWebsite.Services;

namespace WeatherWebsite.Controllers
{
    public class WeatherMapController(ILogger<WeatherMapController> logger) : Controller
    {
        private readonly ILogger<WeatherMapController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
    }
}