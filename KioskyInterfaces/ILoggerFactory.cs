using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskyInterfaces
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(string loggerName);

        ILogger ExceptionLogger { get; }

        ILogger MonitoringLogger { get; }
    }
}
