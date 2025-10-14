using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace PostaGuvercini.Logging
{
    public class SensitiveDataMaskingPolicy : IDestructuringPolicy
    {
        // İsmine göre maskelenecek property adlarının bir listesi (küçük harfle)
        private readonly List<string> _sensitivePropertyNames = new List<string>
        {
            "password", "şifre", "parola",
            "creditcard", "kredikartı", "cvv", "cvc",
            "ssn", "tckimlik", "tckn"
        };

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            // Eğer loglanan değer basit bir tip ise (string, int vb.) bu politikayı uygulama.
            if (value == null || value is ValueType || value is string)
            {
                result = null;
                return false;
            }

            // Reflection kullanarak nesnenin tüm property'lerini alıyoruz.
            var properties = value.GetType().GetProperties();
            var logEventProperties = new List<LogEventProperty>();

            foreach (var prop in properties)
            {
                object propValue;
                try
                {
                    propValue = prop.GetValue(value);
                }
                catch
                {
                    // Bazı property'ler okunamayabilir, onları atla.
                    continue;
                }

                // MASKLEME KONTROLÜ
                if (IsSensitive(prop))
                {
                    // Eğer property hassas ise, değerini "****" ile değiştir.
                    logEventProperties.Add(new LogEventProperty(prop.Name, new ScalarValue("****")));
                }
                else
                {
                    // Hassas değilse, normal değeriyle loga ekle.
                    logEventProperties.Add(new LogEventProperty(prop.Name, propertyValueFactory.CreatePropertyValue(propValue, true)));
                }
            }

            result = new StructureValue(logEventProperties);
            return true;
        }

        private bool IsSensitive(PropertyInfo prop)
        {
            // 1. İsim kontrolü: Property adı listemizde var mı?
            if (_sensitivePropertyNames.Contains(prop.Name.ToLower()))
            {
                return true;
            }

            // 2. Attribute kontrolü: Property [Sensitive] ile işaretlenmiş mi?
            if (prop.GetCustomAttribute<SensitiveAttribute>() != null)
            {
                return true;
            }

            return false;
        }
    }
}
