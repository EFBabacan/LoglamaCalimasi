// .NET'in standart Configuration arayüzünü kullanmak için bu using ifadesi gerekli.
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EFBabacan.Logging
{
    public static class LogManager
    {
        // METODUN İMZASI DEĞİŞTİ!
        // Artık dışarıdan bir 'IConfiguration' nesnesi alıyor.
        public static void Configure(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                // Kendi yazdığımız Enricher'ı yapılandırmaya dahil ediyoruz.
                .Enrich.With<CorporateInfoEnricher>()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("Loglama altyapısı, appsettings.json dosyasından yapılandırıldı.");
        }
    }
}