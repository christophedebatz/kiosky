using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ILogger = KioskyInterfaces.ILogger;

namespace Kiosky.Services.Internal
{
    public class Logger : ILogger
    {
        private static ILog _logger;

        public Logger(ILog logger)
        {
            _logger = logger;
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Critical(string message)
        {
            _logger.Fatal(message);
        }

        public void Critical(string message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Warning(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }
    }
}