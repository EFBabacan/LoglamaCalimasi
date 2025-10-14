using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Bu, ASP.NET Core'un standart loglama arayüzüdür.
                                    // Serilog arkada bu arayüze bağlanır.

namespace Logging.WebAppTest.Controllers
{
    public class HomeController : Controller
    {
        // Controller'a özel bir logger nesnesi tanımlıyoruz.
        private readonly ILogger<HomeController> _logger;

        // Constructor (Yapıcı Metot) aracılığıyla ILogger servisini talep ediyoruz.
        // ASP.NET Core bu nesneyi bizim için otomatik olarak oluşturup buraya verir.
        // Buna "Dependency Injection" denir.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Sitenin ana sayfasına (örneğin: https://localhost:7123/) bir istek geldiğinde bu metot çalışır.
        public IActionResult Index()
        {
            // İŞTE LOGLAMA KÜTÜPHANEMİZİ KULLANDIĞIMIZ YER!
            // Bir web sayfasını her açtığımızda bir log atmış olacağız.
            _logger.LogInformation("HomeController'a istek geldi ve Index sayfası oluşturuldu.");

            // Ekrana Views/Home/Index.cshtml dosyasını çizmesini söylüyoruz.
            return View();
        }
    }
}