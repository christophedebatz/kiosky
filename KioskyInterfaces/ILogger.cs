using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskyInterfaces
{
    public interface ILogger
    {
        void Debug(string message);
        void Debug(string message, Exception exception);

        void Error(string message);
        void Error(string message, Exception exception);

        void Critical(string message);
        void Critical(string message, Exception exception);

        void Info(string message);
        void Info(string message, Exception exception);

        void Warning(string message);
        void Warning(string message, Exception exception);
    }
}
