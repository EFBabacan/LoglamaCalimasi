using Microsoft.AspNetCore.Mvc;
// ARTIK MICROSOFT'UN ARAYÜZÜNE İHTİYACIMIZ YOK
// using Microsoft.Extensions.Logging; 
using PostaGuvercini.Logging;
using ILogger = PostaGuvercini.Logging.ILogger;
using ILoggerFactory = PostaGuvercini.Logging.ILoggerFactory; // BİZİM KENDİ ARAYÜZLERİMİZİ İÇEREN NAMESPACE

namespace Logging.WebAppTest.Controllers
{
    public class HomeController : Controller
    {
        // 1. Alanı, bizim evrensel ILogger arayüzümüz olarak değiştir
        private readonly ILogger _logger;

        // 2. Constructor'da artık Microsoft'un logger'ını değil,
        //    bizim kendi ILoggerFactory'mizi (fabrikamızı) istiyoruz.
        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(HomeController).FullName);
        }

        public IActionResult Index()
        {
            // 4. Loglama metodumuz, bizim ILogger arayüzümüzdeki
            //    imza ile birebir aynı olduğu için bu satırı değiştirmemize gerek kalmadı.
            _logger.LogInformation("HomeController'a istek geldi ve Index sayfası oluşturuldu.");

            return View();
        }

        // ... (Varsa diğer metotlar) ...
    }
}