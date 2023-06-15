using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoahKrip_LB_M295.Controllers
{
    [Route("api/weather]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherDbContext _dbContext;

        public WeatherController(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/weather
        [HttpGet]
        public async Task<IActionResult> GetAllWeatherData()
        {
            var weatherData = await _dbContext.WeatherData.ToListAsync();
            return Ok(weatherData);
        }

        // GET: api/weather/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherDataById(int id)
        {
            var weather = await _dbContext.WeatherData.FindAsync(id);
            if (weather == null)
                return NotFound();

            return Ok(weather);
        }

        // POST: api/weather
        [HttpPost]
        public async Task<IActionResult> CreateWeatherData([FromBody] WeatherData weatherData)
        {
            _dbContext.WeatherData.Add(weatherData);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeatherDataById), new { id = weatherData.Id }, weatherData);
        }

        // PUT: api/weather/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeatherData(int id, [FromBody] WeatherData updatedWeatherData)
        {
            var existingWeatherData = await _dbContext.WeatherData.FindAsync(id);
            if (existingWeatherData == null)
                return NotFound();

            existingWeatherData.City = updatedWeatherData.City;
            existingWeatherData.Temperature = updatedWeatherData.Temperature;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/weather/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherData(int id)
        {
            var existingWeatherData = await _dbContext.WeatherData.FindAsync(id);
            if (existingWeatherData == null)
                return NotFound();

            _dbContext.WeatherData.Remove(existingWeatherData);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

    public class WeatherDbContext : DbContext
    {
        public DbSet<WeatherData> WeatherData { get; set; }

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }
    }

    public class WeatherData
    {
        public int Id { get; set; }
        public string City { get; set; }
        public decimal Temperature { get; set; }
    }

    [HttpPost]
    public IActionResult CreateWeatherData([FromBody] WeatherData weatherData)
    {
        if (weatherData == null)
        {
            return BadRequest("Ungültige Daten.");
        }

        if (string.IsNullOrWhiteSpace(weatherData.City))
        {
            return BadRequest("Die Stadt darf nicht leer sein.");
        }

        if (weatherData.Temperature <= -100 || weatherData.Temperature >= 100)
        {
            return BadRequest("Die Temperatur muss zwischen -100 und 100 liegen.");
        }

        weatherData.Id = _weatherData.Count + 1;
        _weatherData.Add(weatherData);

        return CreatedAtAction(nameof(GetWeatherDataById), new { id = weatherData.Id }, weatherData);
    }







}
