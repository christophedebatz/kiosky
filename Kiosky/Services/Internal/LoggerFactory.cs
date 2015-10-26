using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ILogger = KioskyInterfaces.ILogger;
using ILoggerFactory = KioskyInterfaces.ILoggerFactory;

namespace Kiosky.Services.Internal
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(string loggerName)
        {
            ILog logger = LogManager.Exists(loggerName);

            if (logger == null)
            {
                throw new LogException(
                    string.Format("Could not use logger {0}", loggerName)
                );
            }

            return new Logger(logger);
        }

        public ILogger ExceptionLogger
        {
            get
            {
                return new Logger(LogManager.GetLogger("ExceptionLogger"));
            }
        }

        public ILogger MonitoringLogger
        {
            get
            {
                return new Logger(LogManager.GetLogger("MonitoringLogger"));
            }
        }
    }
}