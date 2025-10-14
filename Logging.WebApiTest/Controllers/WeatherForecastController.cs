using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // ILogger i�in gerekli

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

        // D�n yapt���m�z gibi, ILogger'� constructor �zerinden talep ediyoruz.
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // --- LOGLARI BURAYA EKL�YORUZ ---
            _logger.LogInformation("Hava durumu verileri i�in bir GET iste�i geldi.");
            _logger.LogWarning("Bu bir uyar� logu testidir.");

            try
            {
                if (DateTime.Now.Second % 10 == 0) // Saniyesi 0 olan anlarda kas�tl� hata f�rlat
                {
                    throw new InvalidOperationException("�ste�e ba�l� bir hata olu�turuldu!");
                }

                _logger.LogInformation("Hava durumu verileri ba�ar�yla olu�turuldu.");

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
                _logger.LogError(ex, "Hava durumu verileri olu�turulurken bir hata olu�tu.");
                // Hata durumunda bo� bir sonu� d�nelim.
                return Enumerable.Empty<WeatherForecast>();
            }
        }
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationModel user)
        {
            // Nesneyi loglarken '@' operat�r�n� kullan�yoruz ki destructuring politikam�z tetiklensin.
            _logger.LogInformation("Yeni kullan�c� kay�t iste�i al�nd�: {@UserData}", user);
            return Ok("Kay�t iste�i al�nd�.");
        }
    }
}