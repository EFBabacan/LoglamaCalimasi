using Xunit;
using Moq; // Moq k�t�phanesi
using PostaGuvercini.Logging; // Bizim aray�zlerimiz
using Logging.WebApiTest.Controllers; // Test edece�imiz Controller
using Logging.WebApiTest;

namespace PostaGuvercini.Logging.Tests
{
    public class AbstractionTests
    {
        [Fact] // Bu metodun bir test oldu�unu belirtir
        public void WeatherForecastController_Get_Should_Call_LogInformation()
        {
            // 1. ARRANGE (Haz�rl�k)

            // Sahte bir ILogger nesnesi olu�tur
            var mockLogger = new Mock<ILogger>();

            // Sahte bir ILoggerFactory olu�tur
            var mockFactory = new Mock<ILoggerFactory>();

            // Sahte fabrika'n�n CreateLogger metodu �a�r�ld���nda,
            // ona sahte logger'�m�z� d�nd�rmesini s�yl�yoruz.
            mockFactory.Setup(f => f.CreateLogger(It.IsAny<string>()))
                       .Returns(mockLogger.Object);

            // Test edece�imiz s�n�f�, sahte fabrikam�zla olu�turuyoruz.
            // D�nk� refactoring sayesinde bu m�mk�n oldu!
            var controller = new WeatherForecastController(mockFactory.Object);

            // 2. ACT (Eylem)
            // Test etti�imiz metodu �a��r
            try
            {
                controller.Get();
            }
            catch { /* Hata f�rlat�rsa g�rmezden gel, konumuz o de�il */ }


            // 3. ASSERT (Do�rulama)

            // �unu do�rula: Sahte logger'�m�z�n LogInformation metodu,
            // i�inde "Hava durumu verileri" ge�en bir mesajla
            // EN AZ B�R KEZ �a�r�ld� m�?
            mockLogger.Verify(
                logger => logger.LogInformation(
                    It.Is<string>(s => s.Contains("Hava durumu verileri")),
                    It.IsAny<object[]>()),
                Times.AtLeastOnce()
            );
        }
    }
}