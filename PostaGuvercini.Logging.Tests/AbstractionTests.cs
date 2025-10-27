using Xunit;
using Moq; // Moq kütüphanesi
using PostaGuvercini.Logging; // Bizim arayüzlerimiz
using Logging.WebApiTest.Controllers; // Test edeceðimiz Controller
using Logging.WebApiTest;

namespace PostaGuvercini.Logging.Tests
{
    public class AbstractionTests
    {
        [Fact] // Bu metodun bir test olduðunu belirtir
        public void WeatherForecastController_Get_Should_Call_LogInformation()
        {
            // 1. ARRANGE (Hazýrlýk)

            // Sahte bir ILogger nesnesi oluþtur
            var mockLogger = new Mock<ILogger>();

            // Sahte bir ILoggerFactory oluþtur
            var mockFactory = new Mock<ILoggerFactory>();

            // Sahte fabrika'nýn CreateLogger metodu çaðrýldýðýnda,
            // ona sahte logger'ýmýzý döndürmesini söylüyoruz.
            mockFactory.Setup(f => f.CreateLogger(It.IsAny<string>()))
                       .Returns(mockLogger.Object);

            // Test edeceðimiz sýnýfý, sahte fabrikamýzla oluþturuyoruz.
            // Dünkü refactoring sayesinde bu mümkün oldu!
            var controller = new WeatherForecastController(mockFactory.Object);

            // 2. ACT (Eylem)
            // Test ettiðimiz metodu çaðýr
            try
            {
                controller.Get();
            }
            catch { /* Hata fýrlatýrsa görmezden gel, konumuz o deðil */ }


            // 3. ASSERT (Doðrulama)

            // Þunu doðrula: Sahte logger'ýmýzýn LogInformation metodu,
            // içinde "Hava durumu verileri" geçen bir mesajla
            // EN AZ BÝR KEZ çaðrýldý mý?
            mockLogger.Verify(
                logger => logger.LogInformation(
                    It.Is<string>(s => s.Contains("Hava durumu verileri")),
                    It.IsAny<object[]>()),
                Times.AtLeastOnce()
            );
        }
    }
}