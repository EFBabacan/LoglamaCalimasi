using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog; // Serilog'un statik Log sınıfını kullanmak için

namespace PostaGuvercini.Logging
{
    /// <summary>
    /// Bizim ILoggerFactory arayüzümüzü implemente eder ve
    /// SerilogLogger nesneleri oluşturur.
    /// </summary>
    public class SerilogLoggerFactory : ILoggerFactory // Bizim fabrika arayüzümüz
    {
        public ILogger CreateLogger(string categoryName)
        {
            // ÇÖZÜM BURADA: "SourceContext" Serilog'un standart ismidir
            // ve bizim filtremizin aradığı şey tam olarak budur.
            var serilogLogger = Log.ForContext("SourceContext", categoryName);

            return new SerilogLogger(serilogLogger);
        }
    }
}