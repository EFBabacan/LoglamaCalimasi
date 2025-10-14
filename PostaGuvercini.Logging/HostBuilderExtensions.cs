using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PeriodicBatching;
using System;

namespace PostaGuvercini.Logging
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
        {
            var levelSwitch = new LoggingLevelSwitch();

            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                var configuration = context.Configuration;

                // Serilog yapılandırması
                loggerConfiguration
                    .MinimumLevel.ControlledBy(levelSwitch)  // Log seviyesini kontrol et
                    .Destructure.With<SensitiveDataMaskingPolicy>()  // Özel veri maskeleme
                    .Enrich.FromLogContext()  // Log context bilgileri ekle
                    .Enrich.With<CorporateInfoEnricher>()  // Özel zenginleştirici
                    .WriteTo.Console()  // Konsola log yaz
                    .WriteTo.Sink(new BatchedFileSink(
                        filePath: "logs/batched-log.txt",  // Log dosyasının yolu
                        options: new PeriodicBatchingSinkOptions
                        {
                            BatchSizeLimit = 100,  // 100 log biriktiğinde gönderim yapılır
                            Period = TimeSpan.FromSeconds(5),  // Her 5 saniyede bir gönderim yapılır
                            EagerlyEmitFirstEvent = true  // İlk log geldiğinde hemen gönder
                        }))
                    .WriteTo.Seq(
                        serverUrl: configuration["Serilog:WriteTo:2:Args:serverUrl"] ?? "http://localhost:5341"  // Seq URL
                    )
                    .ReadFrom.Services(services);
            });

            return hostBuilder;
        }
    }
}
