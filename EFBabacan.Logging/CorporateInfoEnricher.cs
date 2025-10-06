using Serilog.Core;
using Serilog.Events;
using System;

namespace EFBabacan.Logging
{
    // Bu arayüzü implemente etmek, Serilog'a bu sınıfın bir "zenginleştirici" olduğunu söyler.
    // Bu bir sözleşmedir: "Ben ILogEventEnricher'ım, yani Enrich adında bir metodum olmak zorunda."
    public class CorporateInfoEnricher : ILogEventEnricher
    {
        // Serilog, her bir log olayı için bu metodu otomatik olarak çağırır.
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // 1. Makine Adını Ekle
            // "MachineName" adında bir özellik oluşturuyoruz ve değerini .NET'in Environment sınıfından alıyoruz.
            var machineNameProperty = propertyFactory.CreateProperty("MachineName", Environment.MachineName);
            // Oluşturduğumuz bu özelliği log olayına ekliyoruz.
            logEvent.AddPropertyIfAbsent(machineNameProperty);

            // 2. Çalışma Ortamını Ekle (Development, Staging, Production)
            // Bu bilgi genellikle "Environment Variables" (Ortam Değişkenleri) üzerinden okunur.
            // Bu, .NET'teki standart bir yöntemdir.
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var environmentProperty = propertyFactory.CreateProperty("Environment", environmentName);
            logEvent.AddPropertyIfAbsent(environmentProperty);

            // 3. Uygulama Adını Ekle
            // Bu bilgiyi şimdilik sabit olarak yazıyoruz, ama normalde bu da yapılandırmadan gelir.
            var applicationNameProperty = propertyFactory.CreateProperty("ApplicationName", "MyAwesomeApp");
            logEvent.AddPropertyIfAbsent(applicationNameProperty);
        }
    }
}