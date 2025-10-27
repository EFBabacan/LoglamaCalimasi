using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PeriodicBatching;
using System;
using Microsoft.Extensions.DependencyInjection;

//namespace PostaGuvercini.Logging
//{
//    public static class HostBuilderExtensions
//    {
//        public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
//        {
//            var levelSwitch = new LoggingLevelSwitch();

//            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
//            {
//                var selfLogFile = File.CreateText("selflog.txt");
//                Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(selfLogFile));

//                var configuration = context.Configuration;

//                // Serilog yapılandırması
//                loggerConfiguration
//                    .MinimumLevel.ControlledBy(levelSwitch)  // Log seviyesini kontrol et
//                    .Destructure.With<SensitiveDataMaskingPolicy>()  // Özel veri maskeleme
//                    .Enrich.FromLogContext()  // Log context bilgileri ekle
//                    .Enrich.With<CorporateInfoEnricher>()  // Özel zenginleştirici
//                    .WriteTo.Console()  // Konsola log yaz
//                    .WriteTo.Sink(new BatchedFileSink(
//                        filePath: "logs/batched-log.txt",  // Log dosyasının yolu
//                        options: new PeriodicBatchingSinkOptions
//                        {
//                            BatchSizeLimit = 100,  // 100 log biriktiğinde gönderim yapılır
//                            Period = TimeSpan.FromSeconds(5),  // Her 5 saniyede bir gönderim yapılır
//                            EagerlyEmitFirstEvent = true  // İlk log geldiğinde hemen gönder
//                        }))
//                    .WriteTo.Seq(
//                        serverUrl: configuration["Serilog:WriteTo:2:Args:serverUrl"] ?? "http://localhost:5341"  // Seq URL
//                    )
//                    .ReadFrom.Services(services);
//            });

//            return hostBuilder;
//        }
//    }
//}


namespace PostaGuvercini.Logging
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// PostaGuvercini Logging altyapısını başlatır ve yapılandırır.
        /// </summary>
        /// <param name="hostBuilder">Yapılandırılacak host builder.</param>
        /// <param name="configure">Adaptör seçmek için yapılandırma eylemi (örn: builder.UseSerilogAdapter()).</param>
        public static IHostBuilder AddCustomLogging(this IHostBuilder hostBuilder,
            Action<LoggerConfigurationBuilder> configure)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                // 1. Builder nesnesini DI servisleri ve Host bağlamı ile oluştur
                var builder = new LoggerConfigurationBuilder(services, context);

                // 2. Kullanıcının yapılandırma eylemini (lambda ifadesini) çalıştır.
                //    Kullanıcı burada .UseSerilogAdapter() çağıracak.
                configure(builder);
            });

            return hostBuilder;
        }
    }
}