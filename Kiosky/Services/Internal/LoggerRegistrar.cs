using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ILoggerFactory = KioskyInterfaces.ILoggerFactory;

namespace Kiosky.Services.Internal
{
    public class LoggerRegistrar
    {
        public static ILoggerFactory Factory;

        public static void RegisterFactory(ILoggerFactory loggerFactory)
        {
            Factory = loggerFactory;
        }
    }
}