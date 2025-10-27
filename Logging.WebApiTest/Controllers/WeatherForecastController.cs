using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging; 
using PostaGuvercini.Logging; // BÝZÝM KENDÝ ARAYÜZLERÝMÝZ
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

        // 1. Alaný, bizim evrensel ILogger arayüzümüz olarak deðiþtir
        private readonly ILogger _logger;

        // 2. Constructor'da bizim kendi ILoggerFactory'mizi istiyoruz.
        public WeatherForecastController(ILoggerFactory loggerFactory)
        {
            // Bu, "Logging.WebApiTest.Controllers.WeatherForecastController" string'ini gönderir
            _logger = loggerFactory.CreateLogger(typeof(WeatherForecastController).FullName);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // 4. Metot imzalarýmýz uyumlu olduðu için bu log satýrlarý
            //    hiçbir deðiþiklik gerektirmedi.
            _logger.LogInformation("Hava durumu verileri için bir GET isteði geldi.");
            _logger.LogWarning("Bu bir uyarý logu testidir.");

            try
            {
                if (DateTime.Now.Second % 10 == 0)
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
                return Enumerable.Empty<WeatherForecast>();
            }
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationModel user)
        {
            // 5. Yapýlandýrýlmýþ loglamayý (structured logging) destekleyen
            //    arayüzümüz sayesinde bu kod da sorunsuz çalýþmaya devam ediyor.
            _logger.LogInformation("Yeni kullanýcý kayýt isteði alýndý: {@UserData}", user);
            return Ok("Kayýt isteði alýndý.");
        }

        [HttpGet("fail-payment")]
        public IActionResult SimulatePaymentFailure()
        {
            try
            {
                // Bu hatayý kasýtlý olarak oluþturuyoruz
                throw new InvalidOperationException("Banka API'sinden onay alýnamadý.");
            }
            catch (Exception ex)
            {
                // 1. TEST: Mesaj içeriðine göre filtreleme ("Ödeme Baþarýsýz")
                _logger.LogError(ex, "Ýþlem sýrasýnda Ödeme Baþarýsýz oldu. Sipariþ ID: {OrderId}", 12345);

                // 2. TEST: Property içeriðine göre filtreleme ("EventType")
                // ILogger arayüzümüz params object[] aldýðý için,
                // Serilog bunu otomatik olarak "EventType" property'sine dönüþtürecektir.
                _logger.LogWarning("Ödeme zaman aþýmýna uðradý. {EventType}", "PaymentFailure");
            }

            return BadRequest("Ödeme iþlemi baþarýsýz oldu (Simülasyon).");
        }
    }
}