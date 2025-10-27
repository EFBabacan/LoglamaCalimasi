using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PostaGuvercini.Logging
{
    /// <summary>
    /// Loglama altyapısını yapılandırmak için kullanılan Builder nesnesi.
    /// DI servislerini ve Host bağlamını taşır.
    /// </summary>
    public class LoggerConfigurationBuilder
    {
        public IServiceCollection Services { get; }
        public HostBuilderContext HostContext { get; }

        public LoggerConfigurationBuilder(IServiceCollection services, HostBuilderContext hostContext)
        {
            Services = services;
            HostContext = hostContext;
        }
    }
}