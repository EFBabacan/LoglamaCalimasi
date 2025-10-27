using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostaGuvercini.Logging
{
    /// <summary>
    /// Herhangi bir loglama teknolojisinden bağımsız,
    /// loglama işlemleri için temel arayüzü tanımlar.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Detaylı hata ayıklama (debug) seviyesinde bir log atar.
        /// </summary>
        /// <param name="messageTemplate">Mesaj şablonu. Örn: "Kullanıcı {UserId} verisi işleniyor."</param>
        /// <param name="args">Mesaj şablonundaki değişkenlerin değerleri.</param>
        void LogDebug(string messageTemplate, params object[] args);

        /// <summary>
        /// Bilgilendirme (information) seviyesinde bir log atar.
        /// </summary>
        void LogInformation(string messageTemplate, params object[] args);

        /// <summary>
        /// Uyarı (warning) seviyesinde bir log atar.
        /// </summary>
        void LogWarning(string messageTemplate, params object[] args);

        /// <summary>
        /// Hata (error) seviyesinde bir log atar. Genellikle bir hata (exception) ile birlikte kullanılır.
        /// </summary>
        /// <param name="exception">Loglanacak olan hata nesnesi.</param>
        /// <param name="messageTemplate">Hata ile ilgili ek bilgi mesajı.</param>
        /// <param name="args">Mesaj şablonundaki değişkenlerin değerleri.</param>
        void LogError(Exception exception, string messageTemplate, params object[] args);

        /// <summary>
        /// Sistemin çalışmasını engelleyen kritik (fatal/critical) bir hata loglar.
        /// </summary>
        void LogFatal(Exception exception, string messageTemplate, params object[] args);
    }
}