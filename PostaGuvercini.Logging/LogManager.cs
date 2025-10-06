// .NET'in standart Configuration arayüzünü kullanmak için bu using ifadesi gerekli.
using Microsoft.Extensions.Configuration;
using Serilog;

namespace PostaGuvercini.Logging
{
    //static olması, bu sınıftan new LogManager() diye bir nesne oluşturmaya gerek kalmadan,
    //doğrudan LogManager.Configure(...) şeklinde kullanabilmemizi sağlar.
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
            //.ReadFrom.Configuration(configuration): Serilog'a "git bütün ayarlarını bu configuration nesnesinin içinden oku" der.
            Log.Information("Loglama altyapısı, appsettings.json dosyasından yapılandırıldı.");
        }
    }
}
//public static class LogManager :
//Configure(IConfiguration configuration): Bu tek metot, tüm loglama sistemini ayağa kaldırır.
//Parametre olarak appsettings.json dosyasından okunan ayarları alır.
//.Enrich.With<CorporateInfoEnricher>(): Burası işin sihirli kısmıdır. Serilog'a der ki:
//"Her bir log mesajını kaydetmeden önce, CorporateInfoEnricher adında bir zenginleştiriciden geçir ve onun eklediği ekstra bilgileri de loga dahil et."