// .NET'in standart Configuration arayüzünü kullanmak için bu using ifadesi gerekli.
using Microsoft.Extensions.Configuration;
using Serilog;
// Yeni eklediğimiz paketi using ile çağırıyoruz.
using Serilog.Sinks.Async;

namespace PostaGuvercini.Logging
{
    //static olması, bu sınıftan new LogManager() diye bir nesne oluşturmaya gerek kalmadan,
    //doğrudan LogManager.Configure(...) şeklinde kullanabilmemizi sağlar.
    public static class LogManager
    {
        public static void Configure(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.With<CorporateInfoEnricher>()
                .WriteTo.Async(a => a.Console()) 
                .WriteTo.Async(a => a.File(
                    path: "logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                ))
                .WriteTo.Async(a => a.Seq(
                    serverUrl: configuration["Serilog:WriteTo:2:Args:serverUrl"] ?? "http://localhost:5341"
                ))
                .CreateLogger();

            Log.Information("Asenkron loglama altyapısı başarıyla yapılandırıldı.");
        }
    }
}

//Configure(IConfiguration configuration): Bu tek metot, tüm loglama sistemini ayağa kaldırır.
//Parametre olarak appsettings.json dosyasından okunan ayarları alır.
//.Enrich.With<CorporateInfoEnricher>(): Burası işin sihirli kısmıdır. Serilog'a der ki:
//"Her bir log mesajını kaydetmeden önce, CorporateInfoEnricher adında bir zenginleştiriciden geçir ve onun eklediği ekstra bilgileri de loga dahil et."