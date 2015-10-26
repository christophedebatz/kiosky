using Kiosky.Services.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace Kiosky.Controllers.Formatter
{
    public static class MediaFormatterFactory
    {
        private const string DefaultFormat = "xml";
        private const string JsonFormat = "json";

        private static readonly JsonFormatter JsonFormatter = new JsonFormatter();
        private static readonly XmlFormatter XmlFormatter = new XmlFormatter();

        /// <summary>
        /// Construct formatter.
        /// </summary>
        /// <param name="format">json or xml</param>
        /// <returns>The formatter ready to be use in output configuration</returns>
        public static MediaTypeFormatter GetFormatter(string format)
        {
            switch (format.ToLower())
            {
                case JsonFormat:
                    return JsonFormatter;

                case DefaultFormat:
                    break;

                default:
                    LoggerRegistrar.Factory.MonitoringLogger.Debug(
                        string.Format("No formatter for {0}. Using {1} instead.", format, DefaultFormat)
                    );
                    break;
            }

            return XmlFormatter;
        }
    }
}