using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using IConfigurator = KioskyInterfaces.IConfigurator;

namespace Kiosky.Services.Internal
{
    public class Configurator : IConfigurator
    {
        /// <summary>
        /// Get property as a string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAsString(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);

            if (value == null)
            {
                throw new SettingsPropertyNotFoundException(
                    string.Format("Could not find entry {0} on configuration.", key)
                );
            }

            return value;
        }

        /// <summary>
        /// Get property as an integer.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetAsInt(string key)
        {
            int result;

            if (!int.TryParse(GetAsString(key), out result))
            {
                throw new FormatException("Could not parse setting to int.");
            }

            return result;
        }
    }
}