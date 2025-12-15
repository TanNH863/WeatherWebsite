using Microsoft.AspNetCore.Mvc;
using WeatherWebsiteAPI.Services;
using WeatherWebsiteAPI.Models;

namespace WeatherWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current")]
        public async Task<ActionResult<WeatherData>> GetCurrentWeather([FromQuery] string location)
        {
            try
            {
                var data = await _weatherService.GetCurrentWeather(location);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("current/coordinates")]
        public async Task<ActionResult<WeatherData>> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)
        {
            try
            {
                var data = await _weatherService.GetCurrentWeather(lat, lon);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("forecast")]
        public async Task<ActionResult<ForecastData>> GetWeatherForecast([FromQuery] string location)
        {
            try
            {
                var data = await _weatherService.GetWeatherForecast(location);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("air_pollution")]
        public async Task<ActionResult<AQIData>> GetAQIData([FromQuery] double lat, [FromQuery] double lng)
        {
            try
            {
                var data = await _weatherService.GetAQIData(lat, lng);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}