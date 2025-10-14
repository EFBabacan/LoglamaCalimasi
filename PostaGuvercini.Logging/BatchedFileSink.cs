using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PostaGuvercini.Logging
{
    // Custom BatchedFileSink: Logları topluca dosyaya yazan özel sink
    public class BatchedFileSink : PeriodicBatchingSink
    {
        private readonly string _filePath;

        // 'options' nesnesindeki parametreleri base sınıfa ayrı ayrı gönderiyoruz.
        public BatchedFileSink(string filePath, PeriodicBatchingSinkOptions options)
            : base(options.BatchSizeLimit, options.Period)  // Base sınıf PeriodicBatchingSink constructor'ına parametreleri gönderiyoruz
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        // Batch log'larını toplu olarak işleme
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var lines = new List<string>();

            // Her bir log event'ini işleyip formatlıyoruz
            foreach (var logEvent in events)
            {
                var writer = new StringWriter();
                logEvent.RenderMessage(writer);  // Log mesajını render et
                lines.Add($"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} [{logEvent.Level}] {writer} {logEvent.Exception}");
            }

            // Logları dosyaya yaz
            await File.AppendAllLinesAsync(_filePath, lines);
        }

        // Dispose metodunu doğru şekilde implement et
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
