using Xunit;
using Serilog; // Gerçek Serilog'u kurmak için
using Serilog.Events;
using Serilog.Sinks.InMemory; // Bellek içi sink
using System.Linq;
using PostaGuvercini.Logging; // Bizim adaptörümüz ve arayüzümüz
using Logging.WebApiTest;

namespace PostaGuvercini.Logging.Tests
{
    public class FeaturesTests
    {
        [Fact]
        public void SerilogAdapter_Should_Mask_Sensitive_Data()
        {
            // 1. ARRANGE (Hazırlık)

            // Serilog'u, logları bellekteki bir listeye yazacak şekilde kur
            Log.Logger = new LoggerConfiguration()
                .Destructure.With<SensitiveDataMaskingPolicy>() // Maskeleme politikamızı ekle
                .WriteTo.InMemory() // Logları belleğe yaz
                .CreateLogger();

            // Kendi adaptörümüzü oluştur ve ona gerçek, yapılandırılmış Serilog logger'ını ver
            ILogger logger = new SerilogLogger(Log.Logger);

            // Hassas veri içeren test nesnemiz
            var user = new UserRegistrationModel
            {
                Username = "testUser",
                Password = "123456", // Bu maskelenmeli (isimden dolayı)
                TCKimlikNo = "11122233344" // Bu maskelenmeli ([Sensitive] attribute'undan dolayı)
            };

            // 2. ACT (Eylem)

            // BİZİM ILogger ARAYÜZÜMÜZ üzerinden log at
            logger.LogInformation("Kullanıcı verisi: {@User}", user);

            // 3. ASSERT (Doğrulama)

            // Bellekteki sink'ten atılan ilk logu al
            var loggedEvent = InMemorySink.Instance.LogEvents.FirstOrDefault();

            // Logun var olduğunu doğrula
            Assert.NotNull(loggedEvent);

            // Logun özelliklerini (properties) al
            var properties = loggedEvent.Properties;
            var loggedUser = (StructureValue)properties["User"];
            var loggedUserProps = loggedUser.Properties.ToDictionary(p => p.Name, p => p.Value.ToString());

            // Değerleri doğrula
            Assert.Equal("\"testUser\"", loggedUserProps["Username"]);
            Assert.Equal("\"****\"", loggedUserProps["Password"]); // Maskeleme çalıştı mı?
            Assert.Equal("\"****\"", loggedUserProps["TCKimlikNo"]); // Maskeleme çalıştı mı?
        }
    }
}