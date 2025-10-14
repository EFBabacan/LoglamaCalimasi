using Microsoft.Extensions.Hosting; // IHostBuilder için gerekli
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PostaGuvercini.Logging
{
    // Extension metotlar her zaman "public static" bir sınıf içinde olmalıdır.
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
        {
            var levelSwitch = new LoggingLevelSwitch();

            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                var configuration = context.Configuration;

                loggerConfiguration
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .Destructure.With<SensitiveDataMaskingPolicy>()
                    .Enrich.FromLogContext()
                    .Enrich.With<CorporateInfoEnricher>()
                    .WriteTo.Console()
                    .WriteTo.PeriodicBatching(pbs => pbs.File(
                        path: "logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {CorrelationId} {Message:lj} {Properties:j}{NewLine}{Exception}"
                    ), new PeriodicBatchingSinkOptions
                    {
                        BatchSizeLimit = 100,
                        Period = TimeSpan.FromSeconds(5),
                        EagerlyEmitFirstEvent = true
                    })
                    .WriteTo.PeriodicBatching(pbs => pbs.Seq(
                        serverUrl: configuration["Serilog:WriteTo:2:Args:serverUrl"] ?? "http://localhost:5341"
                    ), new PeriodicBatchingSinkOptions
                    {
                        BatchSizeLimit = 100,
                        Period = TimeSpan.FromSeconds(5)
                    })
                    .ReadFrom.Services(services);
            });

            return hostBuilder;
        }
    }
}
