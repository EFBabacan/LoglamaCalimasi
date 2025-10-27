using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog; // Serilog'un kendi arayüzlerini kullanmak için
using System;

namespace PostaGuvercini.Logging
{
    /// <summary>
    /// Bizim ILogger arayüzümüzü Serilog'un gerçek logger'ına bağlayan
    /// bir adaptör sınıfı.
    /// </summary>
    public class SerilogLogger : ILogger // Bizim arayüzümüzü implemente et
    {
        // Perde arkasında asıl işi yapacak olan gerçek Serilog logger'ı
        private readonly Serilog.ILogger _serilogLogger;

        public SerilogLogger(Serilog.ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger ?? throw new ArgumentNullException(nameof(serilogLogger));
        }

        // --- Arayüz Metotlarının Implementasyonu ---
        // Gelen çağrıları doğrudan Serilog'taki karşılıklarına yönlendiriyoruz.

        public void LogDebug(string messageTemplate, params object[] args)
        {
            _serilogLogger.Debug(messageTemplate, args);
        }

        public void LogInformation(string messageTemplate, params object[] args)
        {
            _serilogLogger.Information(messageTemplate, args);
        }

        public void LogWarning(string messageTemplate, params object[] args)
        {
            _serilogLogger.Warning(messageTemplate, args);
        }

        public void LogError(Exception exception, string messageTemplate, params object[] args)
        {
            _serilogLogger.Error(exception, messageTemplate, args);
        }

        public void LogFatal(Exception exception, string messageTemplate, params object[] args)
        {
            _serilogLogger.Fatal(exception, messageTemplate, args);
        }
    }
}