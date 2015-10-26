using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskyInterfaces
{
    public interface IConfigurator
    {
        string GetAsString(string key);

        int GetAsInt(string key);
    }
}
