using PostaGuvercini.Logging; // 1. Kendi loglama kütüphanemizi çağırıyoruz.
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // 2. appsettings.json dosyasını okuyacak olan yapılandırma nesnesini kuruyoruz.
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // 3. HAFTANIN EMEĞİNİN KARŞILIĞI OLAN O TEK SATIR!
        // Tüm loglama sistemini bu tek satırla ayağa kaldırıyoruz.
        LogManager.Configure(configuration);

        try
        {
            Log.Information("Uygulama başlıyor...");

            Log.Information("Bu bir standart bilgi logudur.");
            Log.Warning("Dikkat! Disk alanı azalıyor olabilir.");
            Log.Error("Veritabanına bağlanırken bir hata oluştu.");

            // Hata nesnesiyle birlikte loglama örneği
            var ex = new InvalidOperationException("Geçersiz işlem denemesi.");
            Log.Fatal(ex, "Uygulamanın çalışmasını engelleyen kritik bir hata!");

            Log.Information("Uygulama görevlerini tamamladı ve kapanıyor.");
        }
        catch (Exception ex)
        {
            // Beklenmedik bir hata olursa bunu da loglayıp öyle çıkalım.
            Log.Fatal(ex, "Uygulama beklenmedik bir şekilde çöktü!");
        }
        finally
        {
            // 4. Bu çok önemli! Uygulama kapanmadan önce
            // bellekteki logların dosyaya yazıldığından emin olmak için kullanılır.
            Log.CloseAndFlush();
        }
    }
}