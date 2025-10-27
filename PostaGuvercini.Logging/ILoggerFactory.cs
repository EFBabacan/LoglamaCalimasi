using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostaGuvercini.Logging
{
    /// <summary>
    /// ILogger nesneleri oluşturmak için bir fabrika arayüzü tanımlar.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Belirtilen kategori adına sahip bir ILogger nesnesi oluşturur.
        /// Kategori adı genellikle logu atan sınıfın adıdır.
        /// </summary>
        /// <param name="categoryName">Log kategorisi (kaynak sınıfın adı).</param>
        /// <returns>Yeni bir ILogger nesnesi.</returns>
        ILogger CreateLogger(string categoryName);
    }
}