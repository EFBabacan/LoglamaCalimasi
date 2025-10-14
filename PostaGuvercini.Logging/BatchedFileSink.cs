using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PostaGuvercini.Logging
{
    // Özel BatchedFileSink sınıfı: Logları topluca dosyaya yazan sink
    public class BatchedFileSink : PeriodicBatchingSink
    {
        private readonly string _filePath;

        // PeriodicBatchingSink'i özelleştirmek için constructor
        public BatchedFileSink(string filePath, PeriodicBatchingSinkOptions options)
    // DEĞİŞİKLİK BURADA: 'options' nesnesinin içindeki değerleri
    // temel sınıfa ayrı ayrı parametreler olarak gönderiyoruz.
    : base(options.BatchSizeLimit, options.Period)
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
