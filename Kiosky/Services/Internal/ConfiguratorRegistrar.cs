using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IConfigurator = KioskyInterfaces.IConfigurator;

namespace Kiosky.Services.Internal
{
    public class ConfiguratorRegistrar
    {
        public static IConfigurator Config;

        public static void Register(IConfigurator configurator)
        {
            Config = configurator;
        }
    }
}