using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // ILogger için gerekli

namespace Logging.WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // Dün yaptýðýmýz gibi, ILogger'ý constructor üzerinden talep ediyoruz.
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // --- LOGLARI BURAYA EKLÝYORUZ ---
            _logger.LogInformation("Hava durumu verileri için bir GET isteði geldi.");
            _logger.LogWarning("Bu bir uyarý logu testidir.");

            try
            {
                if (DateTime.Now.Second % 10 == 0) // Saniyesi 0 olan anlarda kasýtlý hata fýrlat
                {
                    throw new InvalidOperationException("Ýsteðe baðlý bir hata oluþturuldu!");
                }

                _logger.LogInformation("Hava durumu verileri baþarýyla oluþturuldu.");

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hava durumu verileri oluþturulurken bir hata oluþtu.");
                // Hata durumunda boþ bir sonuç dönelim.
                return Enumerable.Empty<WeatherForecast>();
            }
        }
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationModel user)
        {
            // Nesneyi loglarken '@' operatörünü kullanýyoruz ki destructuring politikamýz tetiklensin.
            _logger.LogInformation("Yeni kullanýcý kayýt isteði alýndý: {@UserData}", user);
            return Ok("Kayýt isteði alýndý.");
        }
    }
}