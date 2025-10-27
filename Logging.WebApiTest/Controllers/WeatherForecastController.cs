using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging; 
using PostaGuvercini.Logging; // B�Z�M KEND� ARAY�ZLER�M�Z
using System;
using System.Collections.Generic;
using System.Linq;
using ILogger = PostaGuvercini.Logging.ILogger;
using ILoggerFactory = PostaGuvercini.Logging.ILoggerFactory;

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

        // 1. Alan�, bizim evrensel ILogger aray�z�m�z olarak de�i�tir
        private readonly ILogger _logger;

        // 2. Constructor'da bizim kendi ILoggerFactory'mizi istiyoruz.
        public WeatherForecastController(ILoggerFactory loggerFactory)
        {
            // Bu, "Logging.WebApiTest.Controllers.WeatherForecastController" string'ini g�nderir
            _logger = loggerFactory.CreateLogger(typeof(WeatherForecastController).FullName);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // 4. Metot imzalar�m�z uyumlu oldu�u i�in bu log sat�rlar�
            //    hi�bir de�i�iklik gerektirmedi.
            _logger.LogInformation("Hava durumu verileri i�in bir GET iste�i geldi.");
            _logger.LogWarning("Bu bir uyar� logu testidir.");

            try
            {
                if (DateTime.Now.Second % 10 == 0)
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
                return Enumerable.Empty<WeatherForecast>();
            }
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationModel user)
        {
            // 5. Yap�land�r�lm�� loglamay� (structured logging) destekleyen
            //    aray�z�m�z sayesinde bu kod da sorunsuz �al��maya devam ediyor.
            _logger.LogInformation("Yeni kullan�c� kay�t iste�i al�nd�: {@UserData}", user);
            return Ok("Kay�t iste�i al�nd�.");
        }

        [HttpGet("fail-payment")]
        public IActionResult SimulatePaymentFailure()
        {
            try
            {
                // Bu hatay� kas�tl� olarak olu�turuyoruz
                throw new InvalidOperationException("Banka API'sinden onay al�namad�.");
            }
            catch (Exception ex)
            {
                // 1. TEST: Mesaj i�eri�ine g�re filtreleme ("�deme Ba�ar�s�z")
                _logger.LogError(ex, "��lem s�ras�nda �deme Ba�ar�s�z oldu. Sipari� ID: {OrderId}", 12345);

                // 2. TEST: Property i�eri�ine g�re filtreleme ("EventType")
                // ILogger aray�z�m�z params object[] ald��� i�in,
                // Serilog bunu otomatik olarak "EventType" property'sine d�n��t�recektir.
                _logger.LogWarning("�deme zaman a��m�na u�rad�. {EventType}", "PaymentFailure");
            }

            return BadRequest("�deme i�lemi ba�ar�s�z oldu (Sim�lasyon).");
        }
    }
}