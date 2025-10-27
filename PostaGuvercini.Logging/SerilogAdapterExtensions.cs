using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Expressions;
using Serilog.Settings.Configuration;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace PostaGuvercini.Logging
{
    public static class SerilogAdapterExtensions
    {
        public static LoggerConfigurationBuilder UseSerilogAdapter(this LoggerConfigurationBuilder builder)
        {
            // 1. ADIM: SelfLog (Aynı kalıyor)
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            // 2. ADIM: DI Kaydı (Aynı kalıyor)
            builder.Services.AddSingleton<ILoggerFactory, SerilogLoggerFactory>();

            // 3. ADIM: Seviye Anahtarı (Aynı kalıyor)
            var levelSwitch = new LoggingLevelSwitch();

            // 4. ADIM: Ana Yapılandırma (Tüm WriteTo kuralları silindi)
            builder.Services.AddSerilog((services, loggerConfiguration) =>
            {
                var configuration = builder.HostContext.Configuration;

                // C#'ta SADECE TEMEL YAPILANDIRMAYI yapıyoruz
                loggerConfiguration
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .Destructure.With<SensitiveDataMaskingPolicy>()
                    .Enrich.FromLogContext()
                    .Enrich.With<CorporateInfoEnricher>()
                    .ReadFrom.Services(services)

                    // 5. ADIM: JSON'dan Okuma (Tüm kurallar buradan gelecek)
                    // Önceki karmaşık ".ReadFrom.Configuration(..., new ConfigurationReaderOptions...)"
                    // satırını bu basit satırla değiştiriyoruz.
                    .ReadFrom.Configuration(configuration);
            });

            return builder;
        }
    }
}